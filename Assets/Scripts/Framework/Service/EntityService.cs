using BEBE.Engine.Service;
using BEBE.Framework.Component;
using BEBE.Framework.Managers;
using System.Collections.Generic;
using System.IO;
using System.Collections.Concurrent;
using UnityEngine;
using BEBE.Framework.Event;
using BEBE.Framework.Module;
using BEBE.Game.Map;
using BEBE.Game.Inputs;

namespace BEBE.Framework.Service
{
    public abstract class EntityService : BaseService
    {
        protected ConcurrentDictionary<int, Entity> entities = new ConcurrentDictionary<int, Entity>();
        protected SrcMgr srcMgr => MgrsContainer.GetMgr<SrcMgr>();
        public abstract void CreateEntity<T>(int id, string entity_name) where T : Entity;

        public abstract bool TryGetEntity(int id, out Entity entity);

        public abstract void DestroyEntity(int id);

        public abstract void DestroyAll();

    }

    public class PlayerEntityService : EntityService
    {
        public override void CreateEntity<T>(int actorId, string entity_name)
        {
            //Instantiate Entity
            // step 1 -> load info TODO

            // step 2 -> load prefab
            GameObject prefab;
#if UNITY_EDITOR
            prefab = srcMgr.GetPrefabAsset(entity_name);
#elif UNITY_STANDALONE
            prefab = srcMgr.GetPrefabAsset(entity_name,"players");
#endif
            // step 3 -> instantiate gameobject
            var obj = GameObject.Instantiate(prefab);
            obj.name = new DirectoryInfo(entity_name).Name;
            obj.transform.position = MgrsContainer.GetMgr<MapMgr>().CurrentMap.GetComponent<Map>().SpawnSpots[actorId].position;
            // step 4 -> add entity
            var entity = obj.AddComponent<T>();
            // step 5 -> init status TODO
            entity.Id = actorId;
            // step 6 -> add to list
            entities.TryAdd(actorId, entity);
        }
        public override bool TryGetEntity(int id, out Entity entity)
        {
            if (entities.TryGetValue(id, out Entity e))
            {
                entity = e;
                return true;
            }
            else
            {
                entity = null;
                return false;
            }
        }
        public override void DestroyAll()
        {
            try
            {
                foreach (var entity in entities.Values)
                {
                    MonoBehaviour.Destroy(entity.gameObject);
                }
            }
            catch
            {

            }

            entities.Clear();
        }
        public override void DestroyEntity(int id)
        {
            if (entities.TryGetValue(id, out Entity entity))
            {
                MonoBehaviour.Destroy(entity.gameObject);
                entities.Remove(id, out Entity e);
            }

        }

        protected CmdMgr cmdMgr => MgrsContainer.GetMgr<CmdMgr>();
        protected FrameMgr frameMgr => MgrsContainer.GetMgr<FrameMgr>();

        private TickInputsRollbackableCache cmd => cmdMgr.InputsRollbackable;
        public void ExecuteCmd()
        {
            BEBE.Engine.Logging.Debug.Log($"Tick {frameMgr.Tick}  {cmd.ToString()}");

            if (cmd.TryGetRollbackableCmd(frameMgr.Tick, out TickInputs inputs))
            {
                foreach (var input in inputs.Inputs)
                {
                    if (TryGetEntity(input.actorId, out Entity player))
                    {
                        ((PlayerEntity)player).ExecuteCmd(input);
                        input.executed = true;
                    }
                }
            }

            //RollBack
            if (cmd.NeedRollback)
            {
                Debug.Log("NeedRollback");
                // step 1. roll back to the tick before sync_tick
                for (int i = frameMgr.Tick; i > cmdMgr.TickSync; i--)
                {
                    if (cmd.TryGetRollbackableCmd(i, out TickInputs rollback_inputs))
                    {
                        foreach (var input in rollback_inputs.Inputs)
                        {
                            if (input.executed)
                            {
                                // BEBE.Engine.Logging.Debug.Log("getCmd");
                                if (TryGetEntity(input.actorId, out Entity player))
                                {
                                    ((PlayerEntity)player).ExecuteCmd(input.RollbackInput());
                                }
                            }
                        }
                    }
                }

                TickInputs latest_sync_inputs = null;
                // step 2. execute sync tick
                while (cmd.TryGetSyncCmd(out TickInputs sync_inputs))
                {
                    foreach (var input in sync_inputs.Inputs)
                    {
                        if (TryGetEntity(input.actorId, out Entity player))
                        {
                            ((PlayerEntity)player).ExecuteCmd(input);
                        }
                    }
                    cmd.TryRemoveRollbackableCmd(sync_inputs.Tick, out TickInputs o);
                    cmdMgr.SyncTick(sync_inputs.Tick);
                    latest_sync_inputs = sync_inputs;
                }
                // step 3. refresh usync tick to latest sync tick
                if (latest_sync_inputs == null) return;
                for (int i = cmdMgr.TickSync + 1; i <= frameMgr.Tick; i++)
                {
                    if (cmd.TryRemoveRollbackableCmd(i, out TickInputs o))
                    {
                        TickInputs sync = latest_sync_inputs.Clone(i);
                        cmd.TryAddRollbackableCmd(sync);
                        foreach (var input in sync.Inputs)
                        {
                            if (TryGetEntity(input.actorId, out Entity player))
                            {
                                ((PlayerEntity)player).ExecuteCmd(input);
                                input.executed = true;
                            }
                        }
                    }
                }
            }
        }

        protected override void register_events()
        {
            Dispatchor.Register(this, Constant.EVENT_PREFIX);
        }
    }
}

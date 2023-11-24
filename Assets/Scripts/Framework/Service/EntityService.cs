using BEBE.Engine.Service;
using BEBE.Framework.Component;
using BEBE.Framework.Managers;
using System.Collections.Generic;
using System.IO;
using System.Collections.Concurrent;
using UnityEngine;

namespace BEBE.Framework.Service
{
    public abstract class EntityService : BaseService
    {

        protected ConcurrentDictionary<int, Entity> entities = new ConcurrentDictionary<int, Entity>();
        protected SrcMgr srcMgr => MgrsContainer.GetMgr<SrcMgr>();
        public abstract void CreateEntity(int id, string entity_name);

        public abstract bool TryGetEntity(int id, out Entity entity);

        public abstract void DestroyEntity(int id);

        public abstract void DestroyAll();

    }

    public class PlayerEntityService : EntityService
    {
        public override void CreateEntity(int id, string entity_name)
        {
            //Instantiate Entity
            // step 1 -> load info TODO

            // step 2 -> load prefab
            var prefab = srcMgr.GetPrefabAsset(entity_name);
            // step 3 -> instantiate gameobject
            var obj = GameObject.Instantiate(prefab);
            obj.name = new DirectoryInfo(entity_name).Name;
            // step 4 -> add entity
            var entity = obj.AddComponent<PlayerEntity>();
            // step 5 -> init status TODO
            entity.Id = id;
            // step 6 -> add to list
            entities.TryAdd(id, entity);
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
        protected PlayerInputs playerInputs => cmdMgr.GetPlayerInputs();
        public void DoCmd()
        {
            BEBE.Engine.Logging.Debug.Log($"Tick {frameMgr.Tick}  {playerInputs.ToString()}");
            if (playerInputs.get(frameMgr.Tick, out PlayerInput input))
            {
                // BEBE.Engine.Logging.Debug.Log("getCmd");
                if (TryGetEntity(input.actorId, out Entity player))
                {
                    ((PlayerEntity)player).ExecuteCmd(input);
                    input.executed = true;
                }
            }
        }
    }
}

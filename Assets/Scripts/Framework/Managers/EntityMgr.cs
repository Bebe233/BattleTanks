using System.Collections.Generic;
using BEBE.Framework.Managers;
using UnityEngine;
namespace BEBE.Framework.Managers
{
    public class EntityMgr : IMgr
    {
        List<PlayerEntity> players;

        public override void Awake()
        {
            base.Awake();
            players = new List<PlayerEntity>();
        }

        public override void Start()
        {
            //Instantiate Entity
            // step 1 -> load info TODO
            // step 2 -> load prefab
            var prefab = MgrsContainer.GetMgr<SrcMgr>().GetPrefabAsset("player_1");
            // step 3 -> instantiate gameobject
            var obj = GameObject.Instantiate(prefab);
            // step 4 -> add entity
            var entity = obj.AddComponent<PlayerEntity>();
            // step 5 -> init status TODO
            // step 6 -> add to list
            players.Add(entity);
        }
    }
}
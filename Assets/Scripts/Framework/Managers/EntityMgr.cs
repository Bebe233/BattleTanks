using BEBE.Framework.Service;

namespace BEBE.Framework.Managers
{
    public class EntityMgr : IMgr
    {
        protected PlayerEntityService svc_player;
        private bool toggle = false;
        public override void Awake()
        {
            svc_player = new PlayerEntityService();
        }

        public override void Start()
        {
            toggle = true;
        }

        public override void FixedUpdate()
        {
            if (toggle)
            {
                svc_player.ExecuteCmd();
            }
        }

        public override void Update()
        {
            if (toggle)
                svc_player.DoRender();
        }

        public override void OnDestroy()
        {
            toggle = false;
            svc_player.DestroyAll();
        }

        public void CreatePlayer<T>(byte actorId) where T : Entity
        {
            svc_player.CreateEntity<T>(actorId, "roles/player/player_1");
        }

        public bool TryGetPlayer(byte actorId, out Entity player)
        {
            if (svc_player.TryGetEntity(actorId, out Entity p))
            {
                player = p;
                return true;
            }
            else
            {
                player = null;
                return false;
            }
        }
    }
}
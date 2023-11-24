using BEBE.Framework.Service;

namespace BEBE.Framework.Managers
{
    public class EntityMgr : IMgr
    {
        protected PlayerEntityService svc_player;

        public override void Awake()
        {
            svc_player = new PlayerEntityService();
        }

        public override void Start()
        {
            CreatePlayer(((byte)MgrsContainer.GetMgr<NetMgr>().Client.Id));
        }

        public override void FixedUpdate()
        {
            svc_player.DoCmd();
        }

        public override void OnDestroy()
        {
            svc_player.DestroyAll();
        }

        public void CreatePlayer(byte actorId)
        {
            svc_player.CreateEntity(actorId, "roles/player/player_1");
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
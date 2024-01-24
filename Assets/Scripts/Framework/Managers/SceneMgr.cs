using BEBE.Engine.Service.Net;
using BEBE.Framework.Event;
using UnityEngine;
namespace BEBE.Framework.Managers
{
    public class SceneMgr : IMgr
    {
        public async void LoadNecessaryAssets(Service.Net.Msg.EventMsg msg)
        {
            ByteBuf buffer = new ByteBuf(msg.Content);
            buffer.ReadInt(); // id room
            buffer.ReadByte();  // capicity
            byte count = buffer.ReadByte();  // count unit
            byte total = (byte)((byte)3 + count);
            float progress = 0;
            // 1. Load Scene
            MgrsContainer.AddMgr<MapMgr>().LoadMap("maps/map_1");
            progress += 1;
            MgrsContainer.AddMgr<CmdMgr>().Awake();
            progress += 1;
            Dispatchor.Dispatch(EventCode.CALL_SYNC_LOAD_PROGRESS_METHOD, progress / total);
            // 2. Load Player
            MgrsContainer.AddMgr<EntityMgr>().Awake();
            for (byte i = 0; i < count; i++)
            {
                string player_id = buffer.ReadString();
                buffer.ReadBool();
                buffer.ReadBool();
                if (player_id == MgrsContainer.GetMgr<CommonStatusMgr>().PlayerId)
                {
                    MgrsContainer.GetMgr<CommonStatusMgr>().ActorId = i;
                    MgrsContainer.GetMgr<EntityMgr>().CreatePlayer<You>(i);
                }
                else
                    MgrsContainer.GetMgr<EntityMgr>().CreatePlayer<Partner>(i);
                progress += 1;
                Dispatchor.Dispatch(EventCode.CALL_SYNC_LOAD_PROGRESS_METHOD, progress / total);
                await new WaitForSeconds(1);
            }
            Dispatchor.Dispatch(EventCode.CALL_SYNC_LOAD_PROGRESS_METHOD, progress / total);
            await new WaitForSeconds(1);
            MgrsContainer.AddMgr<FrameMgr>().Awake();
            progress += 1;
            Dispatchor.Dispatch(EventCode.CALL_SYNC_LOAD_PROGRESS_METHOD, progress / total);
            await new WaitForSeconds(1);
            MgrsContainer.GetMgr<FrameMgr>().Start();
            MgrsContainer.GetMgr<CmdMgr>().Start();
             MgrsContainer.GetMgr<EntityMgr>().Start();
            Dispatchor.Dispatch(EventCode.CALL_LOADING_COMPLETED_METHOD);
        }
    }
}

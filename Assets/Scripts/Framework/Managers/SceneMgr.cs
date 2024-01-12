using BEBE.Framework.Event;
using UnityEngine;
namespace BEBE.Framework.Managers
{
    public class SceneMgr : IMgr
    {
        public async void LoadNecessaryAssets()
        {
            for (int i = 0; i < 10; i++)
            {
                await new WaitForSeconds(1);
                Dispatchor.Dispatch(EventCode.CALL_SYNC_LOAD_PROGRESS_METHOD, (i + 1) / 10f);
            }
            await new WaitForSeconds(1);
            Dispatchor.Dispatch(EventCode.CALL_LOADING_COMPLETED_METHOD);
        }
    }
}
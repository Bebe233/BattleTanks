using System.Collections.Generic;
using BEBE.Engine.Service.Cmd;
using BEBE.Engine.Service.Net;
using System.Collections.Concurrent;
namespace BEBE.Game.Inputs
{
    public class TickInputsRollbackableCache
    {
        // | count | TickIputs ... |
        private ConcurrentDictionary<int, TickInputs> rollback_cmd = new ConcurrentDictionary<int, TickInputs>(); // 需要回滚的操作
        private ConcurrentQueue<TickInputs> sync_cmd = new ConcurrentQueue<TickInputs>(); //同步的操作

        public bool NeedRollback => sync_cmd.Count > 0;

        public bool TryAddRollbackableCmd(TickInputs inputs)
        {
            return rollback_cmd.TryAdd(inputs.Tick, inputs);
        }

        public bool TryGetRollbackableCmd(int tick, out TickInputs inputs)
        {
            return rollback_cmd.TryGetValue(tick, out inputs);
        }

        public bool TryRemoveRollbackableCmd(int tick, out TickInputs inputs)
        {
            return rollback_cmd.TryRemove(tick, out inputs);
        }

        public void PutSyncCmd(TickInputs inputs)
        {
            sync_cmd.Enqueue(inputs);
        }

        public bool TryGetSyncCmd(out TickInputs inputs)
        {
            return sync_cmd.TryDequeue(out inputs);
        }

        public bool TryPeekSyncCmd(out TickInputs inputs)
        {
            return sync_cmd.TryPeek(out inputs);
        }

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine($"\n<--------Rollback Cmd----------->");
            foreach (var kp in rollback_cmd)
            {
                sb.AppendLine(kp.Value.ToString());
            }
            sb.AppendLine($"\n<--------Sync Cmd----------->");
            foreach (var cmd in sync_cmd)
            {
                sb.AppendLine(cmd.ToString());
            }
            sb.AppendLine("\n<------------------->");
            return sb.ToString();
        }
    }
}
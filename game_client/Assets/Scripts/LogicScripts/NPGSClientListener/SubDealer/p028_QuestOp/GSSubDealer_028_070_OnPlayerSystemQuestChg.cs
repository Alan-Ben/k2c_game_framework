using ALBasicProtocolPack;
using GS2GC.p028_QuestOp;

namespace GOE
{
    /// <summary>
    /// 系统任务变更
    /// </summary>
    public class GSSubDealer_028_070_OnPlayerSystemQuestChg : NPSubDealer<GS2GC_028_070_OnPlayerSystemQuestChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_028_070_OnPlayerSystemQuestChg _createProtocolObj()
        {
            return new GS2GC_028_070_OnPlayerSystemQuestChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_028_070_OnPlayerSystemQuestChg _msg)
        {
            NPPlayer.instance.systemQuestComp.onPlayerSystemQuestChg(_msg);
        }
    }
}
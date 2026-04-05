
using ALBasicProtocolPack;
using GS2GC.p028_QuestOp;

namespace GOE
{
    public class GSSubDealer_028_060_OnPlayerDailyQuestChg : NPSubDealer<GS2GC_028_060_OnPlayerDailyQuestChg>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_028_060_OnPlayerDailyQuestChg _createProtocolObj()
        {
            return new GS2GC_028_060_OnPlayerDailyQuestChg();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_028_060_OnPlayerDailyQuestChg _msg)
        {
            if(_msg == null)
                return;

            NPPlayer.instance.dailyQuestComp.onPlayerDailyQuestChg(_msg.getQuest(),_msg.getType());
        }
    }
}

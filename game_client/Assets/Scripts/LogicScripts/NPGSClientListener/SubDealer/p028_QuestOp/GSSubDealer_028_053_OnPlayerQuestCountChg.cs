
using ALBasicProtocolPack;
using GS2GC.p028_QuestOp;

namespace GOE
{
    public class GSSubDealer_028_053_OnPlayerQuestCountChg : NPSubDealer<GS2GC_028_053_OnPlayerQuestCountChg>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_028_053_OnPlayerQuestCountChg _createProtocolObj()
        {
            return new GS2GC_028_053_OnPlayerQuestCountChg();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_028_053_OnPlayerQuestCountChg _msg)
        {
            if(_msg == null)
                return;

            NPPlayer.instance.questComp.updateQuestCount(_msg.getQuestCount());

            //发送消息刷新自定义加载prefab
            GCommon.reloadCustomLoadPrefab();
        }
    }
}

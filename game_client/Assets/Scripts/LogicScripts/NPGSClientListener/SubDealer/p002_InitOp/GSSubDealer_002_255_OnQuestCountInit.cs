
using ALBasicProtocolPack;
using GS2GC.p002_InitOp;
using GS2GC.p002_InitOp;


namespace GOE
{
    public class GSSubDealer_002_255_OnQuestCountInit : NPSubDealer<GS2GC_002_255_OnQuestCountInit>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_002_255_OnQuestCountInit _createProtocolObj()
        {
            return new GS2GC_002_255_OnQuestCountInit();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_002_255_OnQuestCountInit _msg)
        {
            NPPlayer.instance.questComp.dealPreInitFunc(() =>
            {
                NPPlayer.instance.questComp.retQuestCountListData(_msg.getQuestCountList());
            }); 
        }
    }
}

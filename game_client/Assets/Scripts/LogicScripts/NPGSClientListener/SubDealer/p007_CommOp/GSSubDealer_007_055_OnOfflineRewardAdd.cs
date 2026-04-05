
using ALBasicProtocolPack;

namespace GOE
{
    public class GSSubDealer_007_055_OnOfflineRewardAdd : NPSubDealer<GS2GC.p007_CommOp.GS2GC_007_055_OnOfflineRewardAdd>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC.p007_CommOp.GS2GC_007_055_OnOfflineRewardAdd _createProtocolObj()
        {
            return new GS2GC.p007_CommOp.GS2GC_007_055_OnOfflineRewardAdd();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC.p007_CommOp.GS2GC_007_055_OnOfflineRewardAdd _msg)
        {
            if(_msg == null)
                return;

            NPPlayer.instance.offlineRewardComp.onOfflineRewardAdd(_msg);
        }
    }
}

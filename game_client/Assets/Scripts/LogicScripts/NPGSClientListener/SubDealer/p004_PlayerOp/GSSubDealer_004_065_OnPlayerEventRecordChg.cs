using ALBasicProtocolPack;

namespace GOE
{
    public class GSSubDealer_004_065_OnPlayerEventRecordChg : NPSubDealer<GS2GC.p004_PlayerOp.GS2GC_004_065_OnEventRecordChg>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC.p004_PlayerOp.GS2GC_004_065_OnEventRecordChg _createProtocolObj()
        {
            return new GS2GC.p004_PlayerOp.GS2GC_004_065_OnEventRecordChg();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC.p004_PlayerOp.GS2GC_004_065_OnEventRecordChg _msg)
        {
            if (_msg == null)
                return;

            NPPlayer.instance.eventRecordComp.onPlayerEventRecordUpdated(_msg);
        }
    }
}

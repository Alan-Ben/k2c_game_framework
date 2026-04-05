
using ALBasicProtocolPack;

namespace GOE
{
    public class GSSubDealer_004_051_PushCurrencyInfo : NPSubDealer<GS2GC.p004_PlayerOp.GS2GC_004_051_PushCurrencyInfo>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC.p004_PlayerOp.GS2GC_004_051_PushCurrencyInfo _createProtocolObj()
        {
            return new GS2GC.p004_PlayerOp.GS2GC_004_051_PushCurrencyInfo();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC.p004_PlayerOp.GS2GC_004_051_PushCurrencyInfo _msg)
        {
            if(_msg == null)
                return;

            //更新玩家资源
            NPPlayer.instance.rescourceComp.setValue(_msg.getCurrencyInfo());
        }
    }
}

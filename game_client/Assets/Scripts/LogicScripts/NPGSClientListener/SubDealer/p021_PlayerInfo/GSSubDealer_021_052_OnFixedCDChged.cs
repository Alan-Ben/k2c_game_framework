
using ALBasicProtocolPack;

namespace GOE
{
    public class GSSubDealer_021_052_OnFixedCDChged : NPSubDealer<GS2GC.p021_PlayerInfo.GS2GC_021_052_OnFixedCDChged>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC.p021_PlayerInfo.GS2GC_021_052_OnFixedCDChged _createProtocolObj()
        {
            return new GS2GC.p021_PlayerInfo.GS2GC_021_052_OnFixedCDChged();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC.p021_PlayerInfo.GS2GC_021_052_OnFixedCDChged _msg)
        {
            if (_msg == null)
                return;

            NPPlayer.instance.fixedCdComp.onFixedCDChged(_msg);
        }
    }
}

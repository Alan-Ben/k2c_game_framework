
using ALBasicProtocolPack;

namespace GOE
{
    public class GSSubDealer_021_007_PlayerIconChg : NPSubDealer<GS2GC.p021_PlayerInfo.GS2GC_021_007_PlayerIconChg>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC.p021_PlayerInfo.GS2GC_021_007_PlayerIconChg _createProtocolObj()
        {
            return new GS2GC.p021_PlayerInfo.GS2GC_021_007_PlayerIconChg();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC.p021_PlayerInfo.GS2GC_021_007_PlayerIconChg _msg)
        {
            if(_msg == null)
                return;
            if (null != NPPlayer.instance.iconComp)
                NPPlayer.instance.iconComp.chgItem(_msg);
            
        }
    }
}

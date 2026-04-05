
using ALBasicProtocolPack;

namespace GOE
{
    public class GSSubDealer_021_012_PlayerIconBgkChg : NPSubDealer<GS2GC.p021_PlayerInfo.GS2GC_021_012_PlayerIconBgkChg>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC.p021_PlayerInfo.GS2GC_021_012_PlayerIconBgkChg _createProtocolObj()
        {
            return new GS2GC.p021_PlayerInfo.GS2GC_021_012_PlayerIconBgkChg();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC.p021_PlayerInfo.GS2GC_021_012_PlayerIconBgkChg _msg)
        {
            if(null == _msg)
                return;
            if (null != NPPlayer.instance.iconBgkComp)
                NPPlayer.instance.iconBgkComp.chgItem(_msg);
            
        }
    }
}

using ALBasicProtocolPack;
using GS2GC.p040_MarsPeopleOp;

namespace GOE
{
    /// <summary>
    /// 火星居民 - 求助变更推送
    /// </summary>
    public class GSSubDealer_040_055_OnHelpChg : NPSubDealer<GS2GC_040_055_OnHelpChg>
    {
        /// <summary>
        /// 构造协议对象结构体
        /// </summary>
        protected override GS2GC_040_055_OnHelpChg _createProtocolObj()
        {
            return new GS2GC_040_055_OnHelpChg();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_040_055_OnHelpChg _msg)
        {
            NPPlayer.instance.marsComp?.peopleSubComponent?.onMarsHelpChg(_msg);
        }
    }
}
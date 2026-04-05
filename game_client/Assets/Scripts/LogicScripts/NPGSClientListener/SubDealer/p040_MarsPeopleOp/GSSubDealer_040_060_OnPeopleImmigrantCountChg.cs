using ALBasicProtocolPack;
using GS2GC.p040_MarsPeopleOp;

namespace GOE
{
    /// <summary>
    /// 火星居民 - 移民次数变化推送
    /// </summary>
    public class GSSubDealer_040_060_OnPeopleImmigrantCountChg : NPSubDealer<GS2GC_040_060_OnPeopleImmigrantCountChg>
    {
        /// <summary>
        /// 构造协议对象结构体
        /// </summary>
        protected override GS2GC_040_060_OnPeopleImmigrantCountChg _createProtocolObj()
        {
            return new GS2GC_040_060_OnPeopleImmigrantCountChg();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_040_060_OnPeopleImmigrantCountChg _msg)
        {
            if (NPPlayer.instance?.marsComp?.peopleSubComponent != null)
            {
                NPPlayer.instance.marsComp.peopleSubComponent.onPeopleImmigrantCountChg(_msg);
            }
        }
    }
}

using ALBasicProtocolPack;
using GS2GC.p040_MarsPeopleOp;

namespace GOE
{
    /// <summary>
    /// 火星居民 - 人口数量变化推送
    /// </summary>
    public class GSSubDealer_040_050_OnMarsPeopleNumChg : NPSubDealer<GS2GC_040_050_OnMarsPeopleNumChg>
    {
        /// <summary>
        /// 构造协议对象结构体
        /// </summary>
        protected override GS2GC_040_050_OnMarsPeopleNumChg _createProtocolObj()
        {
            return new GS2GC_040_050_OnMarsPeopleNumChg();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_040_050_OnMarsPeopleNumChg _msg)
        {
            NPPlayer.instance.marsComp.peopleSubComponent.onMarsPeopleNumChg(_msg);
        }
    }
}

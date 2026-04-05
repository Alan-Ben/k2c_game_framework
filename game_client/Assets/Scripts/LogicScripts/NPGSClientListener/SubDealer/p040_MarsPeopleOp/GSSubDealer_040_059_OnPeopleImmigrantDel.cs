using ALBasicProtocolPack;
using GS2GC.p040_MarsPeopleOp;

namespace GOE
{
    /// <summary>
    /// 火星居民 - 移民数据删除推送
    /// </summary>
    public class GSSubDealer_040_059_OnPeopleImmigrantDel : NPSubDealer<GS2GC_040_059_OnPeopleImmigrantDel>
    {
        /// <summary>
        /// 构造协议对象结构体
        /// </summary>
        protected override GS2GC_040_059_OnPeopleImmigrantDel _createProtocolObj()
        {
            return new GS2GC_040_059_OnPeopleImmigrantDel();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_040_059_OnPeopleImmigrantDel _msg)
        {
            if (NPPlayer.instance?.marsComp?.peopleSubComponent != null)
            {
                NPPlayer.instance.marsComp.peopleSubComponent.onPeopleImmigrantDel(_msg);
            }
        }
    }
}

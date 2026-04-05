using ALBasicProtocolPack;
using GS2GC.p040_MarsPeopleOp;

namespace GOE
{
    /// <summary>
    /// 火星居民 - 删除信件推送
    /// </summary>
    public class GSSubDealer_040_054_OnLetterDel : NPSubDealer<GS2GC_040_054_OnLetterDel>
    {
        /// <summary>
        /// 构造协议对象结构体
        /// </summary>
        protected override GS2GC_040_054_OnLetterDel _createProtocolObj()
        {
            return new GS2GC_040_054_OnLetterDel();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_040_054_OnLetterDel _msg)
        {
            NPPlayer.instance.marsComp?.peopleSubComponent?.onMarsLetterDel(_msg);
        }
    }
}
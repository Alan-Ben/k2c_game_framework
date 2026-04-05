using ALBasicProtocolPack;
using GS2GC.p039_MarsBuildingOp;

namespace GOE
{
    /// <summary>
    /// 火星加速道具加速时长数值变化
    /// </summary>
    public class GSSubDealer_039_062_OnItemHelpSecsChg : NPSubDealer<GS2GC_039_062_OnItemHelpSecsChg>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_039_062_OnItemHelpSecsChg _createProtocolObj()
        {
            return new GS2GC_039_062_OnItemHelpSecsChg();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_039_062_OnItemHelpSecsChg _msg)
        {
            if (NPPlayer.instance != null && NPPlayer.instance.marsComp != null && NPPlayer.instance.marsComp.isInited)
            {
                NPPlayer.instance.marsComp.onItemHelpSecsChg(_msg);
            }
        }
    }
}

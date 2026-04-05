using ALBasicProtocolPack;
using GS2GC.p008_TravelOp;

namespace GOE
{
    /// <summary>
    /// 增加游历事件推送
    /// </summary>
    public class GSSubDealer_008_050_OnEventAdd : NPSubDealer<GS2GC_008_050_OnEventAdd>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_008_050_OnEventAdd _createProtocolObj()
        {
            return new GS2GC_008_050_OnEventAdd();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_008_050_OnEventAdd _msg)
        {
			NPPlayer.instance.travelComp.onEventAdd(_msg);
        }
    }
}
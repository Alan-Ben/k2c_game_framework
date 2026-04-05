using ALBasicProtocolPack;
using GS2GC.p039_MarsBuildingOp;

namespace GOE
{
    /// <summary>
    /// 
    /// </summary>
    public class GSSubDealer_039_009_RetHomeCollectTimeChg : NPSubDealer<GS2GC_039_009_RetHomeCollectTimeChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_039_009_RetHomeCollectTimeChg _createProtocolObj()
        {
            return new GS2GC_039_009_RetHomeCollectTimeChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_039_009_RetHomeCollectTimeChg _msg)
        {
            //更新数据
            NPPlayer.instance.marsComp.buildingSubComponent.updateHomeCollectTime(_msg.getLastHomeOutputCollectTimeS());
            
            //触发引导trigger
            WinMsg.SendMsg(WinMsgType.CONTROL_TUROTIAL_SETP, ENPTutorialTriggerType.MARS_COLLECT_HOME_REWARD);

        }
    }
}
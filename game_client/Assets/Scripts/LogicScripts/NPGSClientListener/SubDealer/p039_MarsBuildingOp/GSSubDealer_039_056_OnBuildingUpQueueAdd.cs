using ALBasicProtocolPack;
using GS2GC.p039_MarsBuildingOp;

namespace GOE
{
    /// <summary>
    /// 建筑建造升级队列数据增加
    /// </summary>
    public class GSSubDealer_039_056_OnBuildingUpQueueAdd : NPSubDealer<GS2GC_039_056_OnBuildingUpQueueAdd>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_039_056_OnBuildingUpQueueAdd _createProtocolObj()
        {
            return new GS2GC_039_056_OnBuildingUpQueueAdd();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_039_056_OnBuildingUpQueueAdd _msg)
        {
			NPPlayer.instance.marsComp.buildingSubComponent._onQueueAdd(_msg);
        }
    }
}
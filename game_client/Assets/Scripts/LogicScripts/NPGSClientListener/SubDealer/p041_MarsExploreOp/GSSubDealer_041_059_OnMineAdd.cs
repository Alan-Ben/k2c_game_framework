using ALBasicProtocolPack;
using GS2GC.p041_MarsExploreOp;

namespace GOE
{
    /// <summary>
    /// 火星探索-矿产数据增加
    /// </summary>
    public class GSSubDealer_041_059_OnMineAdd : NPSubDealer<GS2GC_041_059_OnMineAdd>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_041_059_OnMineAdd _createProtocolObj()
        {
            return new GS2GC_041_059_OnMineAdd();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_041_059_OnMineAdd _msg)
        {
			NPPlayer.instance.marsComp.exploreSubComponent._onMineInfoAdd(_msg);
        }
    }
}
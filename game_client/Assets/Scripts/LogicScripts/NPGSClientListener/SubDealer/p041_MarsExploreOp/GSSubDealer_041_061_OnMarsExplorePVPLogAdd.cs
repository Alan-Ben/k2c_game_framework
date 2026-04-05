using ALBasicProtocolPack;
using GS2GC.p041_MarsExploreOp;

namespace GOE
{
    /// <summary>
    /// 火星探索-战报数据增加
    /// </summary>
    public class GSSubDealer_041_061_OnMarsExplorePVPLogAdd : NPSubDealer<GS2GC_041_061_OnMarsExplorePVPLogAdd>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_041_061_OnMarsExplorePVPLogAdd _createProtocolObj()
        {
            return new GS2GC_041_061_OnMarsExplorePVPLogAdd();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_041_061_OnMarsExplorePVPLogAdd _msg)
        {
            if (_msg == null)
                return;
            
            NPPlayer.instance.marsComp.exploreSubComponent._onNewestLogTimeChg(_msg.getCreatedAt());
        }
    }
}
using ALBasicProtocolPack;
using GS2GC.p014_ChildOp;

namespace GOE
{
    /// <summary>
    /// 
    /// </summary>
    public class GSSubDealer_014_014_RetApplyToGroup : NPSubDealer<GS2GC_014_014_RetApplyToGroup>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_014_014_RetApplyToGroup _createProtocolObj()
        {
            return new GS2GC_014_014_RetApplyToGroup();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_014_014_RetApplyToGroup _msg)
        {
			
        }
    }
}
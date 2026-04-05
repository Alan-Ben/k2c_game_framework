using ALBasicProtocolPack;
using GS2GC.p041_MarsExploreOp;

namespace GOE
{
    /// <summary>
    /// 
    /// </summary>
    public class GSSubDealer_041_017_RetGuildMineShareList : NPSubDealer<GS2GC_041_017_RetGuildMineShareList>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_041_017_RetGuildMineShareList _createProtocolObj()
        {
            return new GS2GC_041_017_RetGuildMineShareList();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_041_017_RetGuildMineShareList _msg)
        {
			
        }
    }
}
using ALBasicProtocolPack;
using GS2GC.p015_ConsortOp;

namespace GOE
{
    /// <summary>
    /// 随机邀约中指定的妃子ID列表变更推送
    /// </summary>
    public class GSSubDealer_015_062_OnRandCallConsortChg : NPSubDealer<GS2GC_015_062_OnRandCallConsortChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_015_062_OnRandCallConsortChg _createProtocolObj()
        {
            return new GS2GC_015_062_OnRandCallConsortChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_015_062_OnRandCallConsortChg _msg)
        {
			NPPlayer.instance.consortComp.onRandCallConsortChg(_msg);
        }
    }
}
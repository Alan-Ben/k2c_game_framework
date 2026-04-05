using ALBasicProtocolPack;
using ALPackage;
using GS2GC.p007_CommOp;

namespace GOE
{
    /// <summary>
    /// ClientConfig版本变更推送
    /// </summary>
    public class GSSubDealer_007_066_OnClientVersionChg : NPSubDealer<GS2GC_007_066_OnClientVersionChg>
    {
        private long _m_lSerializeOp;
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_007_066_OnClientVersionChg _createProtocolObj()
        {
            return new GS2GC_007_066_OnClientVersionChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_007_066_OnClientVersionChg _msg)
        {
            long serializeOp = _m_lSerializeOp = ALSerializeOpMgr.next();
            ALCommonTaskController.CommonActionAddNextFrameLaterTask(() =>
            {
                if (serializeOp != _m_lSerializeOp)
                    return;

                CDNSetting_ClientConfigInfo.instance.onCDNClientConfigVersionChg();
            });
        }
    }
}
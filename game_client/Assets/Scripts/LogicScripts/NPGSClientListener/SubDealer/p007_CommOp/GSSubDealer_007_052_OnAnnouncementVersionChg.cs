using ALBasicProtocolPack;
using ALPackage;
using GS2GC.p007_CommOp;

namespace GOE
{
    /// <summary>
    /// 运营公告版本变更推送
    /// </summary>
    public class GSSubDealer_007_052_OnAnnouncementVersionChg : NPSubDealer<GS2GC_007_052_OnAnnouncementVersionChg>
    {
        private long _m_lSerializeOp;
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_007_052_OnAnnouncementVersionChg _createProtocolObj()
        {
            return new GS2GC_007_052_OnAnnouncementVersionChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_007_052_OnAnnouncementVersionChg _msg)
        {
            //更新运营公告配置
            long serializeOp = _m_lSerializeOp = ALSerializeOpMgr.next();
            ALCommonTaskController.CommonActionAddNextFrameLaterTask(() =>
            {
                if (serializeOp != _m_lSerializeOp)
                    return;

                //重新初始化
                CDNSetting_AnnouncementInfo.instance.updateAnnouncement();
            });
        }
    }
}
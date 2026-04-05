
namespace GOE
{
    /// <summary>
    ///运营公告弹窗
    /// </summary>
    public class NoticeDealer_Announcement : _AMainCityCanJumpPushNotice
    {
        //窗口是否已经加载，如果已经加载的情况下。在shownotice的时候不会再加载，避免窗口驻留
        private bool _m_bWndLoaded;

        public NoticeDealer_Announcement(EMainCityPushNoticeTriggerType _pushNoticeTriggerType) : base(_pushNoticeTriggerType)
        {
        }
        
        protected override bool _isEnable { get { return AnnouncementMgr.instance.canShowNotice(); } }
        protected override bool _canCurShow { get { return true; } }
        protected override string _noticeTag { get { return NoticeTagConst.ANNOUNCEMENT_NOTICE; } }
        public override bool canPlayPriority { get { return false; } }
        public override bool needTransBk { get { return true; } }
        public override bool isNoticeFullScreen { get { return false; } }
        public override bool isOnlyUINode { get { return false; } }
        /// <summary>
        /// 字符串标记，可以用来根据tag开区别node的tag
        /// </summary>
        public override string nodeTag { get { return UINodeTagConst.C_ANNOUNCEMENT_NODE; } }


        public override void dealShowNotice()
        {
            //未加载的时候加载
            if (!_m_bWndLoaded)
            {
                _m_bWndLoaded = true;

                GGUIWndAnnouncement.instance.load(() =>
                {
                    GGUIWndAnnouncement.instance.showWnd();
                });
            }
            else
            {
                //已经加载的，需要刷新层级
                if (null != GGUIWndAnnouncement.instance.wnd)
                    GCommon.moveTransformToLastAndRefreshLayer(GGUIWndAnnouncement.instance.rectTransform);
            }
        }

        public override void dealHideNotice()
        {
            //已加载的时候才卸载，同时重置状态
            if (_m_bWndLoaded)
            {
                GGUIWndAnnouncement.instance.discard();
                _m_bWndLoaded = false;
            }
        }

        protected override void __onDealerDone()
        {
            GGUIWndAnnouncement.instance.resetSelect();
        }

        // 临时加上, 防止不能IF修改
        public override void showNotice()
        {
            base.showNotice();
        }

        // 临时加上, 防止不能IF修改
        protected override void _onGotoOtherMainViewNode()
        {
            base._onGotoOtherMainViewNode();
        }
    }
}

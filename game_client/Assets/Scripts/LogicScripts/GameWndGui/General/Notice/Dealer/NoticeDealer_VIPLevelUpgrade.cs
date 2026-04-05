
namespace GOE
{
    /// <summary>
    /// VIP等级升级弹窗
    /// </summary>
    public class NoticeDealer_VIPLevelUpgrade : NPUINoticeMgr._ANPUINoticeDealer
    {
        //窗口是否已经加载，如果已经加载的情况下。在shownotice的时候不会再加载，避免窗口驻留
        private bool _m_bWndLoaded;
        //VIP等级
        private long _m_lVIPLevel;

        public NoticeDealer_VIPLevelUpgrade(long _vipLevel)
        {
            _m_lVIPLevel = _vipLevel;
        }
        
        public override bool canCurShow { get { return true; } }
        public override bool canPlayPriority { get { return false; } }
        public override bool isPriorityDealer { get { return false; } }
        public override bool needTransBk { get { return true; } }
        public override bool isNoticeFullScreen { get { return false; } }
        public override bool isOnlyUINode { get { return false; } }
        /// <summary>
        /// 字符串标记，可以用来根据tag开区别node的tag
        /// </summary>
        public override string nodeTag { get { return UINodeTagConst.C_VIP_LEVEL_UPGRADE; } }


        public override void dealShowNotice()
        {
            //未加载的时候加载
            if (!_m_bWndLoaded)
            {
                _m_bWndLoaded = true;

                GGUIWndVIPLevelUpgrade.instance.load(() =>
                {
                    GGUIWndVIPLevelUpgrade.instance.showWnd();
                    GGUIWndVIPLevelUpgrade.instance.setInfo(_m_lVIPLevel);
                });
            }
            else
            {
                //已经加载的，需要刷新层级
                if (null != GGUIWndVIPLevelUpgrade.instance.wnd)
                {
                    GGUIWndVIPLevelUpgrade.instance.setInfo(_m_lVIPLevel);
                    GCommon.moveTransformToLastAndRefreshLayer(GGUIWndVIPLevelUpgrade.instance.rectTransform);
                }
            }
        }

        public override void dealHideNotice()
        {
            //已加载的时候才卸载，同时重置状态
            if (_m_bWndLoaded)
            {
                GGUIWndVIPLevelUpgrade.instance.discard();
                _m_bWndLoaded = false;
            }
        }

        protected override void _onDealerDone()
        {
        }
    }
}

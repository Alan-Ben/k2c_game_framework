namespace GOE
{
    public class NoticeDealer_ChapterAutoForwardReward : NPUINoticeMgr._ANPUINoticeDealer
    {
        private bool _m_bWndLoaded;
        private ChapterAutoForwardRewardData _m_data;
        
        public NoticeDealer_ChapterAutoForwardReward(ChapterAutoForwardRewardData _data)
        {
            _m_data = _data;
        }
        
        public override ENoticeType[] noticeType { get { return NPNoticeType.g_chapterTypeArr; } }
        public override bool canCurShow { get { return true; } }
        public override bool canPlayPriority { get { return false; } }
        public override bool isPriorityDealer { get { return false; } }
        public override bool needTransBk { get { return true; } }
        public override bool isNoticeFullScreen { get { return false; } }
        public override bool isOnlyUINode { get { return true; } }
        public override string nodeTag { get { return UINodeTagConst_Chapter.C_CHAPTER_AUTO_FORWARD_REWARD; } }


        public override void dealShowNotice()
        {
            //未加载的时候加载
            if (!_m_bWndLoaded)
            {
                _m_bWndLoaded = true;

                GGUIWndChapterAutoForwardReward.instance.load(GGUIWndChapterAutoForwardReward.instance.showWnd);
            }
            else
            {
                //已经加载的，需要刷新层级
                if (null != GGUIWndChapterAutoForwardReward.instance.wnd)
                    GCommon.moveTransformToLastAndRefreshLayer(GGUIWndChapterAutoForwardReward.instance.rectTransform);
            }
            
            GGUIWndChapterAutoForwardReward.instance.setInfo(_m_data);
            GGUIWndChapterAutoForwardReward.instance.showWnd();
        }
        public override void dealHideNotice()
        {
            //已加载的时候才卸载，同时重置状态
            if (_m_bWndLoaded)
            {
                GGUIWndChapterAutoForwardReward.instance.discard();
                _m_bWndLoaded = false;
            }
        }

        protected override void _onDealerDone()
        {
        }
    }
}
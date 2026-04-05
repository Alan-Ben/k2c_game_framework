namespace GOE
{
    /// <summary>
    /// 妃子故事解锁弹窗
    /// </summary>
    public class NoticeDealer_ConsortStoryUnlock : NPUINoticeMgr._ANPUINoticeDealer
    {
        private ConsortStoryRefObj _m_storyRefObj;//故事配表数据
        public NoticeDealer_ConsortStoryUnlock(ConsortStoryRefObj _storyRefObj)
        {
            _m_storyRefObj = _storyRefObj;
        }
        
        public override bool canCurShow { get { return true; } }
        public override bool canPlayPriority { get { return false; } }
        public override bool isPriorityDealer { get { return false; } }
        public override bool needTransBk { get { return true; } }
        public override bool isNoticeFullScreen { get { return false; } }
        public override bool isOnlyUINode { get { return true; } }

        public override string noticeTag { get; }
        public override string nodeTag { get { return UINodeTagConst.C_CONSORT_STORY_UNLOCK; } }
        
        public override void dealShowNotice()
        {
            if (_m_storyRefObj == null)
            {
                setDealerDone();
                return;
            }
            
            GUISceneMain.instance.showAddWnd(GGUIWndConsortStoryUnlock.instance, () =>
            {
                GGUIWndConsortStoryUnlock.instance.showWnd();
                GGUIWndConsortStoryUnlock.instance.setData(_m_storyRefObj, setDealerDone);
            });
        }

        public override void dealHideNotice()
        {
            GGUIWndConsortStoryUnlock.instance.hideWnd();
        }
                
        protected override void _onDealerDone()
        {
        }
    }
}
namespace GOE
{
    /// <summary>
    /// 妃子经营技能解锁弹窗
    /// </summary>
    public class NoticeDealer_ConsortBusinessSkillUnlock : NPUINoticeMgr._ANPUINoticeDealer
    {
        private ConsortBusinessSkillRefObj _m_skillRefObj;
        private GGottenConsortInfo _m_iConsortInfo;
        
        public NoticeDealer_ConsortBusinessSkillUnlock(ConsortBusinessSkillRefObj _skillRefObj, GGottenConsortInfo _consortInfo)
        {
            _m_skillRefObj = _skillRefObj;
            _m_iConsortInfo = _consortInfo;
        }
        
        public override bool canCurShow { get { return true; } }
        public override bool canPlayPriority { get { return false; } }
        public override bool isPriorityDealer { get { return false; } }
        public override bool needTransBk { get { return true; } }
        public override bool isNoticeFullScreen { get { return false; } }
        public override bool isOnlyUINode { get { return true; } }

        public override string noticeTag { get; }
        public override string nodeTag { get { return UINodeTagConst.C_CONSORT_BUSINESS_SKILL_UNLOCK; } }
        
        public override void dealShowNotice()
        {
            if (_m_skillRefObj == null)
            {
                setDealerDone();
                return;
            }
            
            // 若勾选了今日不再提示, 则不弹窗
            if (!AccountSettingMgr.instance.warningTipSaver.needShowWarningTip(ENPWarningType.CONSORT_BUSINESS_SKILL_UNLOCK))
            {
                setDealerDone();
                return;
            }
            
            GUISceneMain.instance.showAddWnd(GGUIWndConsortBusinessSkillUnlock.instance, () =>
            {
                GGUIWndConsortBusinessSkillUnlock.instance.showWnd();
                GGUIWndConsortBusinessSkillUnlock.instance.setDate(_m_skillRefObj, _m_iConsortInfo, setDealerDone);
            });
        }

        public override void dealHideNotice()
        {
            GGUIWndConsortBusinessSkillUnlock.instance.hideWnd();
        }
                
        protected override void _onDealerDone()
        {
        }
    }
}
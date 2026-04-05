using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 
    /// </summary>
    public class GGUIWndTowerBattleShow : _ATALBasicUIWnd<GGUIMonoTowerBattleShow>
    {
        private static GGUIWndTowerBattleShow _g_instance = new GGUIWndTowerBattleShow();

        public static GGUIWndTowerBattleShow instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndTowerBattleShow();
                return _g_instance;
            }
        }

        private NPGGUIWndCommonShowCase _m_showCaseVideoSelf;//展示视频
        private NPGGUIWndCommonShowCase _m_showCaseVideoBoss;//展示视频
        //关闭回调
        private Action _m_aOnClose;
        private TowerChallengeResult _m_result;
        public GGUIWndTowerBattleShow() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoTowerBattleShow.assetPath; }
        protected override string _monoObjName { get => GGUIMonoTowerBattleShow.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
            if (_m_showCaseVideoSelf != null) 
                _m_showCaseVideoSelf.resetWnd();
            
            if (_m_showCaseVideoBoss != null) 
                _m_showCaseVideoBoss.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (_m_showCaseVideoSelf != null) 
                _m_showCaseVideoSelf.discard();
            _m_showCaseVideoSelf = null;
            if (_m_showCaseVideoBoss != null) 
                _m_showCaseVideoBoss.discard();
            _m_showCaseVideoBoss = null;
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
        
            if (wnd.monoShowcaseSelf != null) 
                _m_showCaseVideoSelf = new NPGGUIWndCommonShowCase(wnd.monoShowcaseSelf);
            if (wnd.monoShowcaseBoss != null) 
                _m_showCaseVideoBoss = new NPGGUIWndCommonShowCase(wnd.monoShowcaseBoss);
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        public void refreshWnd(TowerChallengeResult _result, Action _onComplete)
        {
            _m_aOnClose = _onComplete;
            _m_result = _result;
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            TowerChapterRefObj chapterRefObj = GRefdataCoreMgr.instance.towerChapterRefCore.getRef(_m_result.targetChapterId);

            if (_m_result != null)
            {
                //展示当前章节视频
                if (null != _m_showCaseVideoBoss && chapterRefObj != null)
                {
                    _m_showCaseVideoBoss.showWnd(new ShowCaseCommonResUnitInfoObj(chapterRefObj.boss_go_index));
                }
            }

            //展示当前章节视频
            if (null != _m_showCaseVideoSelf)
            {
                _m_showCaseVideoSelf.showWnd(new ShowCaseCommonResUnitInfoObj(NPPlayer.instance?.playerInfo?.curSkinRef?.td_show));
            }

            TowerBattleChatperShowAnim chapterShowAnim = null;
            if (chapterRefObj != null && wnd.chapterShowAnims != null)
            {
                foreach (TowerBattleChatperShowAnim showAnim in wnd.chapterShowAnims)
                {
                    if (showAnim != null && showAnim.chapterId == chapterRefObj.id)
                    {
                        chapterShowAnim = showAnim;
                        break;
                    }
                }
            }
            
            if( AccountSettingMgr.instance.accountSetting.towerBattleSpeedTenTimes)
                wnd.wndAnimation.Play(chapterShowAnim != null ? chapterShowAnim.tenTimesAniName : wnd.tenTimesAniName, _onComplete);
            else
                wnd.wndAnimation.Play(chapterShowAnim != null ? chapterShowAnim.oneTimesAniName : wnd.oneTimesAniName, _onComplete);
        }

        private void _onComplete()
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TOWER_BATTLE_SHOW);
            _m_aOnClose?.Invoke();
        }
    }
}
using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndMarsExploreMineTipFollowerController : _ATALGGUICommonFollowItemController<GGUIMonoMarsExploreMineTip, GGUIWndMarsExploreMineTipFollower>
    {
        private readonly GResPathIndex _m_resIndex;
        private MarsExploreMineView _m_mineView;


        public GGUIWndMarsExploreMineTipFollowerController()
        {
            _m_resIndex = new GResPathIndex(7410);
        }
        public GGUIWndMarsExploreMineTipFollowerController(GResPathIndex _resIndex)
        {
            _m_resIndex = _resIndex;
        }


        public override _AALBasicLoadResIndexInfo followItemIndex { get { return _m_resIndex; } }


        protected override GGUIWndMarsExploreMineTipFollower _createItemWnd(GGUIMonoMarsExploreMineTip _wndMono)
        {
            GGUIWndMarsExploreMineTipFollower wnd = new GGUIWndMarsExploreMineTipFollower(_wndMono);
            wnd.refreshWnd(_m_mineView);
            wnd.showWnd();
            return wnd;
        }
        public void refreshWnd([CanBeNull] MarsExploreMineView _mineView)
        {
            _m_mineView = _mineView;
            wnd?.refreshWnd(_m_mineView);
        }
        public void refreshWnd()
        {
            wnd?.refreshWnd();
        }
        public void refreshTime()
        {
            wnd?.refreshTime();
        }
        public void playLoadedEffect()
        {
            wnd?.playLoadedEffect();
        }
    }

    public class GGUIWndMarsExploreMineTipFollower : _ATALGGUIWndCommonFollowItem<GGUIMonoMarsExploreMineTip>
    {
        private MarsExploreMineView _m_mineView;
        private MarsExploreTeamInfo _m_collectingTeamInfo;
        
        private NPGGuiWndTexture _m_iconWnd;
        private NPGGuiWndTexture _m_myTeamHeroIconWnd;


        public GGUIWndMarsExploreMineTipFollower(GGUIMonoMarsExploreMineTip _wnd) : base(_wnd)
        {
            initWnd();
        }


        protected override void _onShowWnd()
        {
            _m_iconWnd?.showWnd();
            _m_myTeamHeroIconWnd?.showWnd();
            
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_iconWnd?.hideWnd();
            _m_myTeamHeroIconWnd?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_iconWnd?.discardTexture();
            _m_myTeamHeroIconWnd?.discardTexture();
        }
        protected override void _onDiscard()
        {
            _m_iconWnd?.discard();
            _m_iconWnd = null;
            
            _m_myTeamHeroIconWnd?.discard();
            _m_myTeamHeroIconWnd = null;
            
            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onBtnClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.imgIcon != null)
                _m_iconWnd = new NPGGuiWndTexture(wnd.imgIcon);
            if (wnd.imgMyTeamHeroIcon != null)
                _m_myTeamHeroIconWnd = new NPGGuiWndTexture(wnd.imgMyTeamHeroIcon);

            ALUGUICommon.combineBtnClick(wnd.btnClick, _onBtnClick);
        }


        public void refreshWnd([CanBeNull] MarsExploreMineView _mineView)
        {
            _m_mineView = _mineView;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_mineView == null)
                return;

            _IMarsExploreMineItem mineInfo = _m_mineView.mineInfo;
            _m_iconWnd?.setTexture(mineInfo.refObj.icon);
            ALUGUICommon.setLabelTxt(wnd.txtLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, mineInfo.refObj.mine_lvl));
            ALUGUICommon.setUIObjColor(wnd.listColorChg, mineInfo.refObj.mine_ui_color);
            
            bool isEmpty = mineInfo.occupiedCid == 0;
            bool isOccupiedByMe = mineInfo.isMe;
            _m_collectingTeamInfo = null;
            if (isOccupiedByMe)
            {
                _m_collectingTeamInfo = NPPlayer.instance.marsComp.exploreSubComponent.getTeamInfoById(mineInfo.occupiedTeamId);
                if (_m_collectingTeamInfo != null)
                {
                    _m_myTeamHeroIconWnd?.setTexture(_m_collectingTeamInfo.getHeroByIndex(0)?.getIcon());
                    ALUGUICommon.setLabelTxt(wnd.txtMyTeamNum, _m_collectingTeamInfo.teamId);
                    ALUGUICommon.setLabelTxt(wnd.txtMyTeamName, _m_collectingTeamInfo.name);
                }
            }
            
            wnd.setState(isEmpty, isOccupiedByMe);
            
            int eventNum = NPPlayer.instance.marsComp.exploreSubComponent.getEventNumAtPos(mineInfo.posId);
            ALUGUICommon.setLabelTxt(wnd.txtBackEventNum, eventNum);
            wnd.setBackEventNum(eventNum);
            
            refreshTime();
        }
        public void refreshTime()
        {
            if (wnd == null || !_m_bIsShow || _m_mineView == null)
                return;

            _IMarsExploreMineItem mineInfo = _m_mineView.mineInfo;
            if (wnd.sldResourceRemain != null)
            {
                long remainNum = mineInfo.remainNum;
                float progress = (float)remainNum / mineInfo.refObj.res_num;
                wnd.sldResourceRemain.minValue = 0;
                wnd.sldResourceRemain.maxValue = 1;
                wnd.sldResourceRemain.value = progress;
            }

            if (_m_collectingTeamInfo != null)
            {
                ALUGUICommon.setLabelTxt(wnd.txtCollectRemainTime, TimeUtil.millisecondsToTime_DayHourOrHMS(_m_collectingTeamInfo.stateRemainTimeMs));
                if (wnd.sldCollectProgress != null)
                {
                    wnd.sldCollectProgress.minValue = 0;
                    wnd.sldCollectProgress.maxValue = Math.Max(0, _m_collectingTeamInfo.stateEndTime - _m_collectingTeamInfo.stateStartTime);
                    wnd.sldCollectProgress.value = FpsAndPingMgr.instance.serverTimeTag - _m_collectingTeamInfo.stateStartTime;
                }
            }
        }
        public void playLoadedEffect()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            if (wnd.newEventShowAnim != null)
                wnd.newEventShowAnim.Play(wnd.newEventShowAnimName);
            
            if (wnd.newEventSfxParent != null)
                PlaySfxMgr.instance.playUISfx(wnd.newEventSfxId, wnd.newEventSfxParent);
        }


        private void _onBtnClick(GameObject _go)
        {
            _m_mineView?.triggerClick();
        }
    }
}

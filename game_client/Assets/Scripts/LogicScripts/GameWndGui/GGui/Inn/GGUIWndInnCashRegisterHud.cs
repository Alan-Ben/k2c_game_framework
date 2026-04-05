using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndInnCashRegisterHudFollowerController : _ATALGGUICommonFollowItemController<GGUIMonoInnCashRegisterHud, GGUIWndInnCashRegisterHud>
    {
        private readonly GResPathIndex _m_resIndex;
        [NotNull] private readonly InnCashRegisterView _m_cashRegisterView;
        private long _m_rewardNum;
        private bool _m_isCollectingReward;


        public GGUIWndInnCashRegisterHudFollowerController([NotNull] InnCashRegisterView _cashRegisterView)
        {
            _m_resIndex = new GResPathIndex(6441); // Assuming similar resource index pattern
            _m_cashRegisterView = _cashRegisterView;
        }
        public GGUIWndInnCashRegisterHudFollowerController(GResPathIndex _resIndex, [NotNull] InnCashRegisterView _cashRegisterView)
        {
            _m_resIndex = _resIndex;
            _m_cashRegisterView = _cashRegisterView;
        }


        public override _AALBasicLoadResIndexInfo followItemIndex { get { return _m_resIndex; } }


        protected override GGUIWndInnCashRegisterHud _createItemWnd(GGUIMonoInnCashRegisterHud _wndMono)
        {
            GGUIWndInnCashRegisterHud wnd = new GGUIWndInnCashRegisterHud(_wndMono, _m_cashRegisterView);
            wnd.refreshWnd(_m_rewardNum);
            wnd.refreshRewardCollectingShow(_m_isCollectingReward);
            wnd.showWnd();
            return wnd;
        }


        public void refreshWnd(long _rewardNum)
        {
            _m_rewardNum = _rewardNum;
            wnd?.refreshWnd(_rewardNum);
        }
        public void playRewardAddEffect()
        {
            wnd?.playRewardAddEffect();
        }
        public void setRewardCollecting(bool _state)
        {
            _m_isCollectingReward = _state;
            wnd?.refreshRewardCollectingShow(_state);
        }
    }
    public class GGUIWndInnCashRegisterHud : _ATALGGUIWndCommonFollowItem<GGUIMonoInnCashRegisterHud>
    {
        private readonly InnCashRegisterView _m_cashRegisterView;
        private long _m_rewardNum;
        private bool _m_rewardCollectingShowState;


        public GGUIWndInnCashRegisterHud(GGUIMonoInnCashRegisterHud _wnd, [NotNull] InnCashRegisterView _cashRegisterView) : base(_wnd)
        {
            _m_cashRegisterView = _cashRegisterView;
            initWnd();
        }


        protected override void _onShowWnd()
        {
            refreshWnd();
            refreshRewardCollectingShow();
        }
        protected override void _onHideWnd()
        {
        }
        protected override void _onReset()
        {
        }
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onBtnClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnTip, _onBtnTipClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnClick, _onBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnTip, _onBtnTipClick);
        }


        public void refreshWnd(long _rewardNum)
        {
            _m_rewardNum = _rewardNum;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            // Update the display state based on current reward number
            wnd.setGuestNum(_m_rewardNum);
            ALUGUICommon.setLabelTxt(wnd.txtRewardCount, _m_rewardNum);
            ALUGUICommon.setLabelTxt(wnd.txtRewardCount2, _m_rewardNum);
        }
        public void refreshRewardCollectingShow(bool _state)
        {
            _m_rewardCollectingShowState = _state;
            refreshRewardCollectingShow();
        }
        public void refreshRewardCollectingShow()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            wnd.setRewardCollecting(_m_rewardCollectingShowState);
        }
        public void playRewardAddEffect()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            if (wnd.animRewardAdd != null)
                wnd.animRewardAdd.ForcePlay(wnd.animNameRewardAdd);
        }


        private void _onBtnClick(GameObject _)
        {
            if (_m_cashRegisterView == null)
                return;

            // Forward the click event to the cash register view
            _m_cashRegisterView.triggerClick();
        }
        private void _onBtnTipClick(GameObject _)
        {
            NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.inn_noEnoughRewardInCashRegister_none);
        }
    }
}
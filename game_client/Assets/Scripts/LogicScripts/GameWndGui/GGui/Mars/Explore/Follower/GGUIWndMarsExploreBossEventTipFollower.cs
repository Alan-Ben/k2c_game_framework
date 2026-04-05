using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndMarsExploreBossEventTipFollowerController : _ATALGGUICommonFollowItemController<GGUIMonoMarsExploreBossEventTip, GGUIWndMarsExploreBossEventTipFollower>
    {
        private readonly GResPathIndex _m_resIndex;
        private MarsExploreBossEventView _m_eventView;


        public GGUIWndMarsExploreBossEventTipFollowerController()
        {
            _m_resIndex = new GResPathIndex(7415);
        }
        public GGUIWndMarsExploreBossEventTipFollowerController(GResPathIndex _resIndex)
        {
            _m_resIndex = _resIndex;
        }


        public override _AALBasicLoadResIndexInfo followItemIndex { get { return _m_resIndex; } }


        protected override GGUIWndMarsExploreBossEventTipFollower _createItemWnd(GGUIMonoMarsExploreBossEventTip _wndMono)
        {
            GGUIWndMarsExploreBossEventTipFollower wnd = new GGUIWndMarsExploreBossEventTipFollower(_wndMono);
            wnd.refreshWnd(_m_eventView);
            wnd.showWnd();
            return wnd;
        }
        public void refreshWnd([CanBeNull] MarsExploreBossEventView _eventView)
        {
            _m_eventView = _eventView;
            wnd?.refreshWnd(_m_eventView);
        }
        public void refreshWnd()
        {
            wnd?.refreshWnd();
        }
        public void playLoadedEffect()
        {
            wnd?.playLoadedEffect();
        }
    }

    public class GGUIWndMarsExploreBossEventTipFollower : _ATALGGUIWndCommonFollowItem<GGUIMonoMarsExploreBossEventTip>
    {
        private MarsExploreBossEventView _m_eventView;
        private NPGGuiWndTexture _m_iconWnd;


        public GGUIWndMarsExploreBossEventTipFollower(GGUIMonoMarsExploreBossEventTip _wnd) : base(_wnd)
        {
            initWnd();
        }


        protected override void _onShowWnd()
        {
            _m_iconWnd?.showWnd();
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_iconWnd?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_iconWnd?.discardTexture();
        }
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onBtnClick);

            _m_iconWnd?.discard();
            _m_iconWnd = null;
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnClick, _onBtnClick);

            if (wnd.imgIcon != null)
                _m_iconWnd = new NPGGuiWndTexture(wnd.imgIcon);
        }


        public void refreshWnd([CanBeNull] MarsExploreBossEventView _eventView)
        {
            _m_eventView = _eventView;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_eventView == null)
                return;

            MarsExploreBossEventInfo eventInfo = _m_eventView.specificEventInfo;
            _m_iconWnd?.setTexture(eventInfo.typeRefObj.icon);
            wnd.setIsDone(eventInfo.isDone);
            int eventNum = NPPlayer.instance.marsComp.exploreSubComponent.getEventNumAtPos(_m_eventView.eventInfo.posId);
            ALUGUICommon.setLabelTxt(wnd.txtBackEventNum, eventNum);
            wnd.setBackEventNum(eventNum);
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
            _m_eventView?.triggerClick();
        }
    }
}

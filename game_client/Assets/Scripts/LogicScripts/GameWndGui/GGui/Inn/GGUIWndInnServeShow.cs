using System;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public class GGUIWndInnServeShowFollowerController : _ATALGGUICommonFollowItemController<GGUIMonoInnServeShow, GGUIWndInnServeShow>
    {
        private readonly GResPathIndex _m_resIndex;
        [NotNull] private readonly InnGuestInfo _m_guestInfo;


        public GGUIWndInnServeShowFollowerController([NotNull] InnGuestInfo _guestInfo)
        {
            _m_resIndex = new GResPathIndex(6440); // Assuming similar resource index pattern
            _m_guestInfo = _guestInfo;
        }
        public GGUIWndInnServeShowFollowerController(GResPathIndex _resIndex, [NotNull] InnGuestInfo _guestInfo)
        {
            _m_resIndex = _resIndex;
            _m_guestInfo = _guestInfo;
        }


        public override _AALBasicLoadResIndexInfo followItemIndex { get { return _m_resIndex; } }


        protected override GGUIWndInnServeShow _createItemWnd(GGUIMonoInnServeShow _wndMono)
        {
            GGUIWndInnServeShow wnd = new GGUIWndInnServeShow(_wndMono, discard);
            wnd.refreshWnd(_m_guestInfo);
            wnd.showWnd();
            return wnd;
        }


        public void updateGuestInfo([NotNull] InnGuestInfo _guestInfo)
        {
            wnd?.refreshWnd(_guestInfo);
        }
    }
    public class GGUIWndInnServeShow : _ATALGGUIWndCommonFollowItem<GGUIMonoInnServeShow>
    {
        [CanBeNull] private InnGuestInfo _m_guestInfo;
        private Action _m_deleteFunc;
        private NPGGuiWndTexture _m_guestIconWnd;
        private NPGGuiWndTexture _m_dishIconWnd;


        public GGUIWndInnServeShow(GGUIMonoInnServeShow _wnd, Action _deleteFunc) : base(_wnd)
        {
            _m_deleteFunc = _deleteFunc;
            initWnd();
        }


        protected override void _onShowWnd()
        {
            _m_guestIconWnd?.showWnd();
            _m_dishIconWnd?.showWnd();
            refreshWnd();

            NPPlayer.instance.innComp.onMaybeSpecialGuestListChg += refreshSpecialGuestShow;
        }
        protected override void _onHideWnd()
        {
            NPPlayer.instance.innComp.onMaybeSpecialGuestListChg -= refreshSpecialGuestShow;

            _m_guestIconWnd?.hideWnd();
            _m_dishIconWnd?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_guestIconWnd?.discardTexture();
            _m_dishIconWnd?.discardTexture();
        }
        protected override void _onDiscard()
        {
            _m_guestIconWnd?.discard();
            _m_guestIconWnd = null;
            _m_dishIconWnd?.discard();
            _m_dishIconWnd = null;
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgGuestIcon != null)
                _m_guestIconWnd = new NPGGuiWndTexture(wnd.imgGuestIcon);
            if (wnd.imgDishIcon != null)
                _m_dishIconWnd = new NPGGuiWndTexture(wnd.imgDishIcon);
        }


        public void refreshWnd([NotNull] InnGuestInfo _guestInfo)
        {
            _m_guestInfo = _guestInfo;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_guestInfo == null)
                return;

            // Set guest icon and name
            _m_guestIconWnd?.setTexture(_m_guestInfo.refObj.icon);
            ALUGUICommon.setLabelTxt(wnd.txtGuestName, TextTranslate.instance.getLanguage(_m_guestInfo.refObj.name));

            // Set dish icon and name
            _m_dishIconWnd?.setTexture(_m_guestInfo.dishInfo.refObj.icon);
            ALUGUICommon.setLabelTxt(wnd.txtDialogue, TextTranslate.instance.getLanguage(wnd.listDialogueKeys.GetRandomItem(), TextTranslate.instance.getLanguage(_m_guestInfo.dishInfo.refObj.name)));

            // Update special guest display
            refreshSpecialGuestShow();

            // Auto delete after specified delay
            ALCommonActionMonoTask.addMonoTask(_delete, wnd.deleteDelay);
        }


        public void refreshSpecialGuestShow()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            InnSpecialGuestInfo specialGuestInfo = NPPlayer.instance.innComp.tryGetFirstSpecialGuest();
            bool hasSpecialGuest = specialGuestInfo != null;
            wnd.setHasSpecialGuest(hasSpecialGuest);
        }


        public void autoDelete(float _delay)
        {
            ALCommonActionMonoTask.addMonoTask(_delete, _delay);
        }


        private void _delete()
        {
            _m_deleteFunc?.Invoke();
        }
    }
}
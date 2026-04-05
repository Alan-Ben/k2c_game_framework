using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 旅店特殊客人服务完成窗口
    /// </summary>
    public class GGUIWndInnSpecialGuestServed : _ATALBasicUIWnd<GGUIMonoInnSpecialGuestServed>
    {
        [NotNull] public static GGUIWndInnSpecialGuestServed instance { get { return _g_instance ??= new GGUIWndInnSpecialGuestServed(); } }
        private static GGUIWndInnSpecialGuestServed _g_instance;


        private InnSpecialGuestInfo _m_guestInfo;
        private NPGGuiWndTexture _m_guestIconWnd;


        public GGUIWndInnSpecialGuestServed() 
            : base(EALUIWndLayer.ADDITION)
        {
        }
        

        protected override string _monoAssetPath { get { return GGUIMonoInnSpecialGuestServed.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoInnSpecialGuestServed.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_guestIconWnd?.showWnd();
            
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_guestIconWnd?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_guestIconWnd?.discardTexture();
        }
        protected override void _onDiscard()
        {
            _m_guestIconWnd?.discard();
            _m_guestIconWnd = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.imgGuestIcon != null)
                _m_guestIconWnd = new NPGGuiWndTexture(wnd.imgGuestIcon);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }


        public void refreshWnd([NotNull] InnSpecialGuestInfo _guestInfo)
        {
            _m_guestInfo = _guestInfo;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_guestInfo == null)
                return;

            // 设置客人图标
            _m_guestIconWnd?.setTexture(_m_guestInfo.refObj.icon);
        }
        

        private void _onBtnCloseClick(GameObject _obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_INN_SPECIAL_GUEST_SERVED);
        }
    }
}
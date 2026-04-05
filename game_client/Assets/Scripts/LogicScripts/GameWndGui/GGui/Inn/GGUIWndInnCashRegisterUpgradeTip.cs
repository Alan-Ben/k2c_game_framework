using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndInnCashRegisterUpgradeTip : _ATALBasicUIWnd<GGUIMonoInnCashRegisterUpgradeTip>
    {
        [NotNull] public static GGUIWndInnCashRegisterUpgradeTip instance { get { return _g_instance ??= new GGUIWndInnCashRegisterUpgradeTip(); } }
        private static GGUIWndInnCashRegisterUpgradeTip _g_instance;


        private InnLevelRefObj _m_levelRef;
        private NPGGuiWndTexture _m_iconWnd;


        public GGUIWndInnCashRegisterUpgradeTip()
            : base(EALUIWndLayer.ADDITION)
        {
        }


        protected override string _monoAssetPath { get { return GGUIMonoInnCashRegisterUpgradeTip.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoInnCashRegisterUpgradeTip.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


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
            _m_iconWnd?.discard();
            _m_iconWnd = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgIcon != null)
                _m_iconWnd = new NPGGuiWndTexture(wnd.imgIcon);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }


        public void refreshWnd([NotNull] InnLevelRefObj _levelRef)
        {
            _m_levelRef = _levelRef;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_levelRef == null)
                return;

            _m_iconWnd?.setTexture(_m_levelRef.inn_cash_register_icon);
        }


        private void _onBtnCloseClick(GameObject _obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_INN_CASH_REGISTER_UPGRADE_TIP);
        }
    }
}

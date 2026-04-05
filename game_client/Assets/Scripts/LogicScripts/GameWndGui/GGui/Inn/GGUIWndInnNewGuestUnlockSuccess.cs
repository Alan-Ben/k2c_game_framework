using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 旅店新客人解锁成功窗口
    /// </summary>
    public class GGUIWndInnNewGuestUnlockSuccess : _ATALBasicUIWnd<GGUIMonoInnNewGuestUnlockSuccess>
    {
        [NotNull] public static GGUIWndInnNewGuestUnlockSuccess instance { get { return _g_instance ??= new GGUIWndInnNewGuestUnlockSuccess(); } }
        private static GGUIWndInnNewGuestUnlockSuccess _g_instance;

        
        private InnNormalGuestHandbookInfo _m_guestInfo;
        private NPGGuiWndTexture _m_guestIconWnd;

        
        public GGUIWndInnNewGuestUnlockSuccess() 
            : base(EALUIWndLayer.ADDITION)
        {
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoInnNewGuestUnlockSuccess.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoInnNewGuestUnlockSuccess.objName; } }
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


        public void refreshWnd([NotNull] InnNormalGuestHandbookInfo _guestInfo)
        {
            _m_guestInfo = _guestInfo;
            refreshWnd();
        }
        public void refreshWnd() 
        {
            if (wnd == null || !_m_bIsShow || _m_guestInfo == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtGuestName, TextTranslate.instance.getLanguage(_m_guestInfo.refObj.name));
            ALUGUICommon.setLabelTxt(wnd.txtGuestDesc, TextTranslate.instance.getLanguage(_m_guestInfo.refObj.desc));
            ALUGUICommon.setLabelTxt(wnd.txtGuestEffect, TextTranslate.instance.getLanguage(TransKeyConst.inn_normalGuestUnlockEffect_value, _m_guestInfo.refObj.finesse_add / 100f));
            _m_guestIconWnd?.setTexture(_m_guestInfo.refObj.icon);
        }
        
        
        private void _onBtnCloseClick(GameObject _obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_INN_NEW_GUEST_UNLOCK_SUCCESS);
        }
    }
}
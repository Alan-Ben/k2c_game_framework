using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndInnLevelUpSuccess : _ATALBasicUIWnd<GGUIMonoInnLevelUpSuccess>
    {
        [NotNull] public static GGUIWndInnLevelUpSuccess instance { get { return _g_instance ??= new GGUIWndInnLevelUpSuccess(); } }
        private static GGUIWndInnLevelUpSuccess _g_instance;


        private InnLevelRefObj _m_curLevelRef;
        private InnLevelRefObj _m_lastLevelRef;
        
        private TextUpgradePropertyShow<int> _m_maxStaminaShow;
        private GGUISubWndInnLevelIconContainer _m_starIconContainerWnd;
        private NPGGuiWndTexture _m_medalIconWnd;


        public GGUIWndInnLevelUpSuccess() 
            : base(EALUIWndLayer.ADDITION)
        {
        }
        

        protected override string _monoAssetPath { get { return GGUIMonoInnLevelUpSuccess.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoInnLevelUpSuccess.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_starIconContainerWnd?.showWnd();
            _m_medalIconWnd?.showWnd();
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_starIconContainerWnd?.hideWnd();
            _m_medalIconWnd?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_starIconContainerWnd?.resetWnd();
            _m_medalIconWnd?.discardTexture();
        }
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClicked);
            
            _m_starIconContainerWnd?.discard();
            _m_starIconContainerWnd = null;
            _m_medalIconWnd?.discard();
            _m_medalIconWnd = null;
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.txtMaxStamina != null)
                _m_maxStaminaShow = new TextUpgradePropertyShow<int>(wnd.txtMaxStamina, string.Empty);
            
            if (wnd.monoStarIconContainer != null)
                _m_starIconContainerWnd = new GGUISubWndInnLevelIconContainer(wnd.monoStarIconContainer);
            
            if (wnd.imgMedalIcon != null)
                _m_medalIconWnd = new NPGGuiWndTexture(wnd.imgMedalIcon);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClicked);
        }
        

        public void refreshWnd(InnLevelRefObj _curLevelRef)
        {
            _m_curLevelRef = _curLevelRef;
            _m_lastLevelRef = GRefdataCoreMgr.instance.innLevelRefCore.getRef((_m_curLevelRef?.level ?? 0) - 1);
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtName, _m_curLevelRef?.nameTranslated);
            ALUGUICommon.setLabelTxt(wnd.txtLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, _m_curLevelRef?.level ?? 0));
            _m_maxStaminaShow?.setValue((int)(_m_lastLevelRef?.receive_guest_limit ?? 0), (int)(_m_curLevelRef?.receive_guest_limit ?? 0));
            ALUGUICommon.setLabelTxt(wnd.txtTip, _m_curLevelRef?.levelTipTranslated);
            
            // 刷新奖牌图标
            _m_medalIconWnd?.setTexture(_m_curLevelRef?.medal_icon);
            // 刷新星级图标容器
            _m_starIconContainerWnd?.refreshWnd(_m_curLevelRef);
        }


        private void _onBtnCloseClicked(GameObject _)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_INN_LEVEL_UP_SUCCESS);
        }
    }
}
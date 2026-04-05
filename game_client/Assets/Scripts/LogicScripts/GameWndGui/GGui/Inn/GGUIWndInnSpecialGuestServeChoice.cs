using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndInnSpecialGuestServeChoice : _ATALBasicUIWnd<GGUIMonoInnSpecialGuestServeChoice>
    {
        [NotNull] public static GGUIWndInnSpecialGuestServeChoice instance { get { return _g_instance ??= new GGUIWndInnSpecialGuestServeChoice(); } }
        private static GGUIWndInnSpecialGuestServeChoice _g_instance;

        private InnSpecialGuestChoiceRefObj _m_choiceRefObj;
        private GGUISubWndInnSpecialGuestServeChoiceOptionContainer _m_optionContainer;


        private GGUIWndInnSpecialGuestServeChoice()
            : base(EALUIWndLayer.ADDITION)
        {
        }


        protected override string _monoAssetPath { get { return GGUIMonoInnSpecialGuestServeChoice.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoInnSpecialGuestServeChoice.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            _m_optionContainer?.showWnd();
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_optionContainer?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_optionContainer?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_optionContainer?.discard();
            _m_optionContainer = null;
            
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnSure, _onBtnSureClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.optionContainer != null)
                _m_optionContainer = new GGUISubWndInnSpecialGuestServeChoiceOptionContainer(wnd.optionContainer);

            ALUGUICommon.combineBtnClick(wnd.btnSure, _onBtnSureClick);
        }


        public void refreshWnd([NotNull] InnSpecialGuestChoiceRefObj _choiceRefObj)
        {
            _m_choiceRefObj = _choiceRefObj;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_choiceRefObj == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtPlayerName, NPPlayer.instance.playerInfo.PlayerName);
            ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(_m_choiceRefObj.question_desc));
            _m_optionContainer?.refreshWnd(_m_choiceRefObj);
        }


        private void _onBtnSureClick(GameObject _obj)
        {
            if (_m_optionContainer is { selectedOptionId: <= 0 })
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.inn_specialGuestNoSelectOption_none);
                return;
            }
            
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_INN_SPECIAL_GUEST_SERVE_CHOICE);
        }
    }
}
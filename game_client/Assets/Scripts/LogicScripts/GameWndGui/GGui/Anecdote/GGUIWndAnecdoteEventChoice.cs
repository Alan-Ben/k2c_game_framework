using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndAnecdoteEventChoice : _ATALBasicUIWnd<GGUIMonoAnecdoteEventChoice>
    {
        [NotNull] public static GGUIWndAnecdoteEventChoice instance { get { return _g_instance ??= new GGUIWndAnecdoteEventChoice(); } }
        private static GGUIWndAnecdoteEventChoice _g_instance;


        private GGUISubWndAnecdoteEventChoiceContainer _m_choiceContainer;
        private AnecdoteEventChoiceRefObj _m_refObj;
        private Action<int> _m_onSelectChoice;
        
        private int _m_currentSelectedIndex = -1; 
            
        
        public GGUIWndAnecdoteEventChoice()
            : base(EALUIWndLayer.ADDITION)
        {
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoAnecdoteEventChoice.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoAnecdoteEventChoice.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_choiceContainer?.showWnd();
            
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_choiceContainer?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_choiceContainer?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_choiceContainer?.discard();
            _m_choiceContainer = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnConfirm, _onConfirmBtnClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.monoChoiceContainer != null)
                _m_choiceContainer = new GGUISubWndAnecdoteEventChoiceContainer(wnd.monoChoiceContainer, _onItemSelect);
            
            ALUGUICommon.combineBtnClick(wnd.btnConfirm, _onConfirmBtnClick);
        }


        public void refreshWnd(AnecdoteEventChoiceRefObj _refObj, Action<int> _selectChoice)
        {
            _m_refObj = _refObj;
            _m_currentSelectedIndex = -1;
            _m_onSelectChoice = _selectChoice;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_refObj == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtChoiceTitle, TextTranslate.instance.getLanguage(_m_refObj.event_desc));
            _m_choiceContainer?.refreshWnd(_m_refObj.option_ref_list, _m_currentSelectedIndex);
        }
        
        
        private void _onItemSelect(int _index)
        {
            if (_m_refObj == null)
                return;
            
            _m_currentSelectedIndex = _index;
            _m_choiceContainer?.refreshWnd(_m_refObj.option_ref_list, _m_currentSelectedIndex);
        }
        private void _onConfirmBtnClick(GameObject _)
        {
            if (_m_refObj == null)
                return;
            
            if (_m_currentSelectedIndex < 0)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.anecdote_eventChoiceNoSelect_none);
                return;
            }
            
            _m_onSelectChoice?.Invoke(_m_currentSelectedIndex);
        }
    }
}
using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class NPGGUIWndCommonSkip : _ANPGGUIBasicWnd<NPGGUIMonoCommonSkip>
    {
        private static NPGGUIWndCommonSkip _g_instance;
        public static NPGGUIWndCommonSkip instance { get { return _g_instance ??= new NPGGUIWndCommonSkip(); } }


        private Action _m_skipFunction;
        

        private NPGGUIWndCommonSkip() : base(EALUIWndLayer.ADDITION)
        {
        }
        
        
        protected override string _monoAssetPath { get { return NPGGUIMonoCommonSkip.assetPath; } }
        protected override string _monoObjName { get { return NPGGUIMonoCommonSkip.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
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
            
            ALUGUICommon.uncombineBtnClick(wnd.btnSkip, _onBtnSkipClicked);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.combineBtnClick(wnd.btnSkip, _onBtnSkipClicked);
        }

        public void setSkipFunc(Action _function)
        {
            _m_skipFunction = _function;
        }

        private void _onBtnSkipClicked(GameObject _)
        {
            _m_skipFunction?.Invoke();
        }
    }
}
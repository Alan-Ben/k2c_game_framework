using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndTravelProcessSkip : _ANPGGUIBasicWnd<GGUIMonoTravelProcessSkip>
    {
        private static GGUIWndTravelProcessSkip _g_instance = new GGUIWndTravelProcessSkip();
        private Action _m_onSkip;

        public static GGUIWndTravelProcessSkip instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new  GGUIWndTravelProcessSkip();
                return _g_instance;
            }
        }
        public GGUIWndTravelProcessSkip() : base(EALUIWndLayer.TOP)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoTravelProcessSkip.assetPath; }
        protected override string _monoObjName { get => GGUIMonoTravelProcessSkip.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

        public override bool needDiscardOnSwitch { get => true; }

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
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            ALUGUICommon.combineBtnClick(wnd.btnSkip, _clickSkip);
        }

        private void _clickSkip(GameObject obj)
        {
            _m_onSkip?.Invoke();
            _m_onSkip = null;
        }

        public void setInfo(Action _onSkip)
        {
            _m_onSkip = _onSkip;
        }
    }
}
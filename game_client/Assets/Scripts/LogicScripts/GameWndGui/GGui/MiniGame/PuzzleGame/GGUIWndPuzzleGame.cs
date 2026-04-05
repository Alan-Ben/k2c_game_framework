using System;
using System.Collections.Generic;
using ALPackage;
using GOE.MiniGame;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndPuzzleGame : _ANPGGUIBasicWnd<GGUIMonoPuzzleGame>
    {
        [NotNull] public static GGUIWndPuzzleGame instance { get { return _g_instance ??= new GGUIWndPuzzleGame(); } }
        private static GGUIWndPuzzleGame _g_instance;

        private GGUIWndPuzzleGameMatch _m_wGameMatchWnd;
        
        protected override string _monoAssetPath { get { return GGUIMonoPuzzleGame.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoPuzzleGame.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        public GGUIWndPuzzleGame() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override void _onWndInitDone()
        {
        }
        
        protected override void _onDiscard()
        {
            _m_wGameMatchWnd?.discard();
            _m_wGameMatchWnd = null;
        }
        
        protected override void _onShowWnd()
        {
            _m_wGameMatchWnd?.showWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wGameMatchWnd?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wGameMatchWnd?.resetWnd();
        }

        public void loadGameMatchWnd(NPCommonAssetPathInfo _asset, Action<GGUIWndPuzzleGameMatch> _onLoadDone)
        {
            if(_m_wGameMatchWnd != null)
                _m_wGameMatchWnd.discard();
            _m_wGameMatchWnd = null;
            
            _m_wGameMatchWnd = new GGUIWndPuzzleGameMatch(_asset, wnd == null ? null : wnd.parent);
            _m_wGameMatchWnd.load(() =>
            {
                _onLoadDone?.Invoke(_m_wGameMatchWnd);
            });
        }
    }
}
using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 小游戏的跳过游戏窗口
    /// </summary>
    public class GGUIWndMiniGameCommunalWnd : _ANPGGUIBasicWnd<GGUIMonoMiniGameCommunalWnd>
    {
        private static GGUIWndMiniGameCommunalWnd _g_instance;
        [NotNull] public static GGUIWndMiniGameCommunalWnd instance { get { return _g_instance ??= new GGUIWndMiniGameCommunalWnd(); } }

        private MiniGameMainRefObj _m_gameMainRefObj;
        private Action _m_aOnBtnSkipClick;
        
        public GGUIWndMiniGameCommunalWnd() : base(EALUIWndLayer.TOP)
        {
        }
        
        protected override string _monoAssetPath { get { return GGUIMonoMiniGameCommunalWnd.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMiniGameCommunalWnd.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onWndInitDone()
        {
            if(wnd != null)
                ALUGUICommon.combineBtnClick(wnd.btnSkip, _onBtnSkipClick);
        }
        
        protected override void _onDiscard()
        {
            if(wnd != null)
                ALUGUICommon.uncombineBtnClick(wnd.btnSkip, _onBtnSkipClick);
                
            _m_aOnBtnSkipClick = null;
        }
        
        protected override void _onShowWnd()
        {
            if (wnd != null)
            {
                ALUGUICommon.setGameObjEnable(wnd.btnSkip, _m_gameMainRefObj?.canSkip ?? false);
            }
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        public void setData(MiniGameMainRefObj _gameMainRefObj)
        {
            _m_gameMainRefObj = _gameMainRefObj;
        }
        
        public void setOnBtnSkipClick(Action _aOnBtnSkipClick)
        {
            _m_aOnBtnSkipClick = _aOnBtnSkipClick;
        }

        private void _onBtnSkipClick(GameObject _go)
        {
            _m_aOnBtnSkipClick?.Invoke();
        }
    }
}
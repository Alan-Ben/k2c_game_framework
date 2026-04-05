using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndFindThingsGame : _ANPGGUIBasicWnd<GGUIMonoFindThingsGame>
    {
        [NotNull] public static GGUIWndFindThingsGame instance { get { return _g_instance ??= new GGUIWndFindThingsGame(); } }
        private static GGUIWndFindThingsGame _g_instance;

        private GGUIWndFindThingsGamePrefab _m_wFindThingsGamePrefab;//找东西预制
        
        public bool gameSuccess { get { return _m_wFindThingsGamePrefab?.allFind ?? false; } }//游戏是否成功
        
        public GGUIWndFindThingsGame() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoFindThingsGame.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoFindThingsGame.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
        }
        
        protected override void _onDiscard()
        {
            discradFindThingsGamePrefab();
        }
        
        protected override void _onShowWnd()
        {
            _m_wFindThingsGamePrefab?.regLoadDoneDelegate(() =>
            {
                _m_wFindThingsGamePrefab?.showWnd();
            });
        }

        protected override void _onHideWnd()
        {
            _m_wFindThingsGamePrefab?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wFindThingsGamePrefab?.resetWnd();
        }

        /// <summary>
        /// 预加载游戏预制
        /// </summary>
        public void preLoadFindThingsGamePrefab(NPCommonAssetPathInfo _assetPath, Action _onGameSuccess, Action _onLoadDone)
        {
            if (wnd == null)
            {
                Debug.LogError("[GGUIWndFindThingsGame preLoadFindThingsGamePrefab] GGUIWndFindThingsGamePrefab进行预加载时GGUIWndFindThingsGame还未加载完成");
                return;
            }

            _m_wFindThingsGamePrefab = new GGUIWndFindThingsGamePrefab(_assetPath, wnd.parent, _onGameSuccess);
            _m_wFindThingsGamePrefab.load(_onLoadDone);
        }

        /// <summary>
        /// 销毁游戏预制
        /// </summary>
        public void discradFindThingsGamePrefab()
        {
            _m_wFindThingsGamePrefab?.discard();
            _m_wFindThingsGamePrefab = null;
        }
    }
}
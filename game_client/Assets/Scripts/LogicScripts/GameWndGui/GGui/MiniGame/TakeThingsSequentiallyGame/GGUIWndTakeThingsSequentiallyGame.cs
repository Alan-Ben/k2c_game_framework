using System;
using ALPackage;

namespace GOE
{
    public class GGUIWndTakeThingsSequentiallyGame : _ANPGGUIBasicWnd<GGUIMonoTakeThingsSequentiallyGame>
    {
        public static GGUIWndTakeThingsSequentiallyGame instance { get { return _g_instance ??= new GGUIWndTakeThingsSequentiallyGame(); } }
        private static GGUIWndTakeThingsSequentiallyGame _g_instance;
        
        private GGUIWndTakeThingsSequentiallyGamePrefab _m_wGamePrefab;
        
        public GGUIWndTakeThingsSequentiallyGame() : base(EALUIWndLayer.NORMAL)
        {
        }
        
        protected override string _monoAssetPath { get { return GGUIMonoTakeThingsSequentiallyGame.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoTakeThingsSequentiallyGame.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        public GGUIWndTakeThingsSequentiallyGamePrefab gamePrefab { get { return _m_wGamePrefab; } }

        protected override void _onWndInitDone()
        {
        }
        
        protected override void _onDiscard()
        {
            discardGamePrefab();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }
        
        protected override void _onReset()
        {
        }
        
        /// <summary>
        /// 加载GamePrefab
        /// </summary>
        /// <param name="_onLoadDone"></param>
        public void loadGamePrefab(NPCommonAssetPathInfo _assetPath, Action<GGUIWndTakeThingsSequentiallyGamePrefab> _onLoadDone)
        {
            discardGamePrefab();

            if (_assetPath == null || !_assetPath.enable)
            {
                _onLoadDone?.Invoke(null);
                return;
            }
            
            _m_wGamePrefab = new GGUIWndTakeThingsSequentiallyGamePrefab(_assetPath, wnd == null ? null : wnd.parent);
            _m_wGamePrefab.load(() =>
            {
                _onLoadDone?.Invoke(_m_wGamePrefab);
            });
        }
        
        /// <summary>
        /// 销毁GamePrefab
        /// </summary>
        public void discardGamePrefab()
        {
            _m_wGamePrefab?.discard();
            _m_wGamePrefab = null;
        }
    }
}
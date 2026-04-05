using System;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public class GGUIWndQteClickOpportunityGame : _ANPGGUIBasicWnd<GGUIMonoQteClickOpportunityGame>
    {
        private static GGUIWndQteClickOpportunityGame _g_instance;
        [NotNull] public static GGUIWndQteClickOpportunityGame instance { get { return _g_instance ??= new GGUIWndQteClickOpportunityGame(); } }

        private GGUIWndQteClickOpportunityGamePrefab _m_wGamePrefab;
        
        public GGUIWndQteClickOpportunityGame() : base(EALUIWndLayer.NORMAL)
        {
        }
        
        protected override string _monoAssetPath { get { return GGUIMonoQteClickOpportunityGame.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoQteClickOpportunityGame.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        public GGUIWndQteClickOpportunityGamePrefab gamePrefab { get { return _m_wGamePrefab; } }

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
        public void loadGamePrefab(NPCommonAssetPathInfo _assetPath, Action<GGUIWndQteClickOpportunityGamePrefab> _onLoadDone)
        {
            discardGamePrefab();

            if (_assetPath == null || !_assetPath.enable)
            {
                _onLoadDone?.Invoke(null);
                return;
            }
            
            _m_wGamePrefab = new GGUIWndQteClickOpportunityGamePrefab(_assetPath, wnd == null ? null : wnd.prefabParent);
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
using ALPackage;

namespace GOE
{
    public class GGUIWndDragBoxGame : _ANPGGUIBasicWnd<GGUIMonoDragBoxGame>
    {
        public static GGUIWndDragBoxGame instance { get { return _g_instance ??= new GGUIWndDragBoxGame(); } }
        private static GGUIWndDragBoxGame _g_instance;
        
        public GGUIWndDragBoxGame() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoDragBoxGame.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoDragBoxGame.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onWndInitDone()
        {
        }
        
        protected override void _onDiscard()
        {
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
    }
}
using JetBrains.Annotations;

namespace GOE
{
    public class ExploreNormalMarsBuildingView : CommonNormalMarsBuildingView
    {
        private GGUIWndMarsBuildingExploreEntranceBtnFollowerController _m_entranceBtn;
        
        
        public ExploreNormalMarsBuildingView([NotNull] MarsBuildingInfo _buildingInfo) : base(_buildingInfo)
        {
        }


        public override void tick()
        {
            base.tick();
            
            _m_entranceBtn.tick();
        }
        protected override void _onInitDone()
        {
            base._onInitDone();

            if (followTarget == null)
                return;
            
            _m_entranceBtn = new GGUIWndMarsBuildingExploreEntranceBtnFollowerController();
            GGUIWndMarsHud.instance.addController(followTarget, _m_entranceBtn);
        }
        protected override void _onDiscard()
        {
            _m_entranceBtn?.discard();
            
            base._onDiscard();
        }


        protected override void _triggerClickInternal()
        {
            if (followTarget == null)
                return;

            GNodeMarsExplore.openOrQuitToNode(null);
        }
    }
}
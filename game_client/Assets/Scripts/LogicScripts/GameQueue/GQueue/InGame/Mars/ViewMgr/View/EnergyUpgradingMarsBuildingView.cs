using JetBrains.Annotations;

namespace GOE
{
    public class EnergyUpgradingMarsBuildingView : CommonUpgradingMarsBuildingView<GTDMonoMarsBuildingEnergyUpgrading>, _IEnergyMarsBuildingView
    {
        private GGUIWndMarsBuildingEnergyBtnFollowerController _m_energyBtnWnd;


        public EnergyUpgradingMarsBuildingView([NotNull] MarsBuildingInfo _buildingInfo) 
            : base(_buildingInfo)
        {
        }


        protected override void _onInitDone()
        {
            base._onInitDone();
            
            if (followTarget != null)
            {
                _m_energyBtnWnd = new GGUIWndMarsBuildingEnergyBtnFollowerController();
                _m_energyBtnWnd.refreshWnd(this);
                GGUIWndMarsHud.instance.addController(followTarget, _m_energyBtnWnd);
            }
        }
        protected override void _onDiscard()
        {
            base._onDiscard();
            
            _m_energyBtnWnd?.discard();
            _m_energyBtnWnd = null;
        }


        public override void tick()
        {
            base.tick();
            _m_energyBtnWnd?.tick();
        }
        public void playCollectEnergyEffect(long _count)
        {
            if (mono == null)
                return;
            
            MarsUtil.showEnergyCollectTip(position, _count, mono.collectTipId, mono.collectParticleId);
        }
    }
}
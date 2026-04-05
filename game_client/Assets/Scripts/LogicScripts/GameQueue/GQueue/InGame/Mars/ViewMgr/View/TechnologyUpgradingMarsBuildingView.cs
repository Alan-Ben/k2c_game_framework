using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 科研所升级中状态视图
    /// </summary>
    public class TechnologyUpgradingMarsBuildingView : CommonUpgradingMarsBuildingView
    {
        private GGUIWndMarsBuildingTechUpgradingOpBtnsFollowerController _m_TechnologyOperationBtnsFollowerController;
        
        public TechnologyUpgradingMarsBuildingView([NotNull] MarsBuildingInfo _buildingInfo) : base(_buildingInfo)
        {
        }

        protected override void _onInitDone()
        {
            base._onInitDone();

            _m_TechnologyOperationBtnsFollowerController = new GGUIWndMarsBuildingTechUpgradingOpBtnsFollowerController();
        }

        protected override void _onDiscard()
        {
            base._onDiscard();
            
            _m_TechnologyOperationBtnsFollowerController?.discard();
            _m_TechnologyOperationBtnsFollowerController = null;
        }

        public override void triggerClick()
        {
            if (followTarget == null)
                return;

            if (_m_TechnologyOperationBtnsFollowerController != null)
            {
                _m_TechnologyOperationBtnsFollowerController.refreshWnd(this);
                GGUIWndMarsHud.instance.addMutexController(followTarget, _m_TechnologyOperationBtnsFollowerController);       
            }
        }

        public override void tick()
        {
            base.tick();
            
            _m_TechnologyOperationBtnsFollowerController?.tick();
        }
    }
}

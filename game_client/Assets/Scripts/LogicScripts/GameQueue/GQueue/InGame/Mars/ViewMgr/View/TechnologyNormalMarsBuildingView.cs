using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 科研所正常状态视图
    /// </summary>
    public class TechnologyNormalMarsBuildingView : CommonNormalMarsBuildingView
    {
        private GGUIWndMarsBuildingTechnologyNormalOpBtnsFollowerController _m_TechnologyOperationBtnsFollowerController;
        private GGUIWndMarsBuildingTechnologyStateBtnsFollowerController _m_TechnologyStateBtnsFollowerController;
        
        public TechnologyNormalMarsBuildingView([NotNull] MarsBuildingInfo _buildingInfo) : base(_buildingInfo)
        {
        }

        protected override void _onInitDone()
        {
            base._onInitDone();

            _m_TechnologyOperationBtnsFollowerController = new GGUIWndMarsBuildingTechnologyNormalOpBtnsFollowerController();
            
            if (followTarget != null)
            {
                _m_TechnologyStateBtnsFollowerController = new GGUIWndMarsBuildingTechnologyStateBtnsFollowerController();
                _m_TechnologyStateBtnsFollowerController.refreshWnd();
                GGUIWndMarsHud.instance.addController(followTarget, _m_TechnologyStateBtnsFollowerController);
            }
        }

        protected override void _onDiscard()
        {
            base._onDiscard();
            
            _m_TechnologyOperationBtnsFollowerController?.discard();
            _m_TechnologyOperationBtnsFollowerController = null;
            
            _m_TechnologyStateBtnsFollowerController?.discard();
            _m_TechnologyStateBtnsFollowerController = null;
        }

        protected override void _triggerClickInternal()
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

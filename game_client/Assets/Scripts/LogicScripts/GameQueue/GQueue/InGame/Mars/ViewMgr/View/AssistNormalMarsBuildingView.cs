using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 互助建筑正常状态视图
    /// </summary>
    public class AssistNormalMarsBuildingView : CommonNormalMarsBuildingView
    {
        private GGUIWndMarsBuildingAssistOperationBtnsFollowerController _m_AssistOperationBtnsFollowerController;
        private GGUIWndMarsBuildingAssistStateBtnsFollowerController _m_AssistStateBtnsFollowerController;
        
        public AssistNormalMarsBuildingView([NotNull] MarsBuildingInfo _buildingInfo) : base(_buildingInfo)
        {
        }

        protected override void _onInitDone()
        {
            base._onInitDone();

            _m_AssistOperationBtnsFollowerController = new GGUIWndMarsBuildingAssistOperationBtnsFollowerController();
            if (followTarget != null)
            {
                _m_AssistStateBtnsFollowerController = new GGUIWndMarsBuildingAssistStateBtnsFollowerController();
                _m_AssistStateBtnsFollowerController.refreshWnd();
                GGUIWndMarsHud.instance.addController(followTarget, _m_AssistStateBtnsFollowerController);
            }
        }

        protected override void _onDiscard()
        {
            base._onDiscard();
            
            _m_AssistOperationBtnsFollowerController?.discard();
            _m_AssistOperationBtnsFollowerController = null;
            
            _m_AssistStateBtnsFollowerController?.discard();
            _m_AssistStateBtnsFollowerController = null;
        }

        protected override void _triggerClickInternal()
        {
            if (followTarget == null)
                return;

            // 创建互助建筑操作按钮控制器
            if (_m_AssistOperationBtnsFollowerController != null)
            {
                _m_AssistOperationBtnsFollowerController.refreshWnd(this);
                GGUIWndMarsHud.instance.addMutexController(followTarget, _m_AssistOperationBtnsFollowerController);       
            }
        }

        public override void tick()
        {
            base.tick();
            
            _m_AssistOperationBtnsFollowerController?.tick();
        }
    }
}
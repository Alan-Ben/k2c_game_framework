using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 兵工厂正常状态视图
    /// </summary>
    public class ArmoryNormalMarsBuildingView : CommonNormalMarsBuildingView
    {
        private GGUIWndMarsBuildingArmoryOperationBtnsFollowerController _m_ArmoryOperationBtnsFollowerController;
        
        public ArmoryNormalMarsBuildingView([NotNull] MarsBuildingInfo _buildingInfo) : base(_buildingInfo)
        {
        }

        protected override void _onInitDone()
        {
            base._onInitDone();

            _m_ArmoryOperationBtnsFollowerController = new GGUIWndMarsBuildingArmoryOperationBtnsFollowerController();
        }

        protected override void _onDiscard()
        {
            base._onDiscard();
            
            _m_ArmoryOperationBtnsFollowerController?.discard();
            _m_ArmoryOperationBtnsFollowerController = null;
        }

        protected override void _triggerClickInternal()
        {
            if (followTarget == null)
                return;

            // 创建兵工厂操作按钮控制器
            if (_m_ArmoryOperationBtnsFollowerController != null)
            {
                _m_ArmoryOperationBtnsFollowerController.refreshWnd(this);
                GGUIWndMarsHud.instance.addMutexController(followTarget, _m_ArmoryOperationBtnsFollowerController);       
            }
        }

        public override void tick()
        {
            base.tick();
            
            _m_ArmoryOperationBtnsFollowerController?.tick();
        }
    }
}

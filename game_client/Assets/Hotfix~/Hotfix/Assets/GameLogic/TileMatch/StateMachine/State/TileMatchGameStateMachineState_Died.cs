using JetBrains.Annotations;

namespace Hotfix
{
    public partial class TileMatchGameLogic
    {
        public class TileMatchGameStateMachineState_Died : _ATileMatchGameStateMachineBaseState
        {
            public TileMatchGameStateMachineState_Died([NotNull] TileMatchGameLogic _gameLogic) : base(_gameLogic)
            {
            }
            
            public override ETileMatchGameState state { get { return ETileMatchGameState.Died; } }

            protected override void _onEnter()
            {
                long stateEnterSerialize = enterSerialize;
                // 显示死局提示窗口
                _m_gameLogic._m_gameShow.showDiedTipWnd(() =>
                {
                    if(stateEnterSerialize != enterSerialize)
                        return;
                    
                    // 进入重新刷新状态
                    _m_gameLogic._m_stateMachine.setState<TileMatchGameStateMachineState_Reorder>();
                });
            }

            protected override void _onExit()
            {
            }

            protected override void _onTick(float _deltaTime)
            {
            }

            public override bool canEnterState(_ATHotfixStateBase<ETileMatchGameState> _newState)
            {
                return false;
            }

            public override void resetData()
            {
            }
            
            public override void doPress(TileMatchBlockInfo _cubeInfo)
            {
            }
        }
    }
}
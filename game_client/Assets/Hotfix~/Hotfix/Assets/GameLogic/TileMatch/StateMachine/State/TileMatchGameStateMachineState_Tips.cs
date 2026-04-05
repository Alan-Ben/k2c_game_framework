using JetBrains.Annotations;

namespace Hotfix
{
    public partial class TileMatchGameLogic
    {
        public class TileMatchGameStateMachineState_Tips : _ATileMatchGameStateMachineBaseState
        {
            public TileMatchGameStateMachineState_Tips([NotNull] TileMatchGameLogic _gameLogic) : base(_gameLogic)
            {
            }

            public override ETileMatchGameState state { get { return ETileMatchGameState.Tips; } }

            protected override void _onEnter()
            {
                _m_gameLogic._m_errorSwitchCount = 0; // 重置错误交换计数
                _doTip();
            }

            protected override void _onExit()
            {
                _m_gameLogic._m_gameShow.hideOpTip();
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

            private void _doTip()
            {
                TileMatchBlockInfo blockInfo1 = null;
                TileMatchBlockInfo blockInfo2 = null;
                
                // 若没有找到可交换消除的方块
                if(!_m_gameLogic._getCanSwitchBlock(out blockInfo1, out blockInfo2) || blockInfo1 == null || blockInfo2 == null)
                {
                    // 进入死局状态
                    _m_gameLogic._m_stateMachine.setState<TileMatchGameStateMachineState_Died>();
                }
                else
                {
                    // 显示操作提示
                    _showOpTip(blockInfo1, blockInfo2);
                }
            }
            
            private void _showOpTip(TileMatchBlockInfo _blockInfo1, TileMatchBlockInfo _blockInfo2)
            {
                if (_blockInfo1 == null || _blockInfo2 == null)
                {
                    _m_gameLogic._m_gameShow.hideOpTip();
                    return;
                }
                
                ETileMatchDirection direction = _blockInfo1.getCubeDirection(_blockInfo2);
                _m_gameLogic._m_gameShow.showOpTip(_blockInfo1.logicPos, direction);
            }
        }
    }
}
using JetBrains.Annotations;

namespace Hotfix
{
    public partial class TileMatchGameLogic
    {
        public class TileMatchGameStateMachineState_Reorder : _ATileMatchGameStateMachineBaseState
        {
            public TileMatchGameStateMachineState_Reorder([NotNull] TileMatchGameLogic _gameLogic) : base(_gameLogic)
            {
            }

            public override ETileMatchGameState state { get { return ETileMatchGameState.Reorder; } }

            protected override void _onEnter()
            {
                long stateEnterSerialize = enterSerialize;
                
                // 打开操作屏蔽
                int opMaskSerialize = _m_gameLogic._m_gameShow.openOpMask();
                HotfixNPPlayer.instance.tileMatchComponent.reqTileMatchGameOver((_isSucc, _msg) =>
                {
                    // 关闭操作屏蔽
                    _m_gameLogic._m_gameShow.closeOpMask(opMaskSerialize);
                    
                    if(stateEnterSerialize != enterSerialize)
                        return;

                    if (!_isSucc || _msg == null)
                    {
                        Debug.LogError($"[TileMatchGameStateMachineState_Reorder] Failed to initialize Tile Match blocks: _isSucc:{_isSucc} _msg:{_msg}");
                        _m_gameLogic._m_stateMachine.setState<TileMatchGameStateMachineState_None>();
                        return;
                    }
                    
                    _m_gameLogic._fillBlockInfoArray(_msg.getBlockList());
                    _m_gameLogic._m_stateMachine.setState<TileMatchGameStateMachineState_Idle>();
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
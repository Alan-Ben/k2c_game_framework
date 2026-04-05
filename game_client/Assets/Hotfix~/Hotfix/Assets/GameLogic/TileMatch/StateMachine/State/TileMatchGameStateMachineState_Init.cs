using System;
using JetBrains.Annotations;

namespace Hotfix
{
    public partial class TileMatchGameLogic
    {
        public class TileMatchGameStateMachineState_Init : _ATileMatchGameStateMachineBaseState
        {
            private Action _m_aOnComplete;
            private Action _m_aOnFail;

            public TileMatchGameStateMachineState_Init([NotNull] TileMatchGameLogic _gameLogic) : base(_gameLogic)
            {
            }

            public override ETileMatchGameState state { get { return ETileMatchGameState.Init; } }

            protected override void _onEnter()
            {
                long stateEnterSerialize = enterSerialize;
                
                // 打开操作屏蔽
                int opMaskSerialize = _m_gameLogic._m_gameShow.openOpMask();
                HotfixNPPlayer.instance.tileMatchComponent.reqTileMatchBlockInit((_isSucc, _msg) =>
                {
                    // 关闭操作屏蔽
                    _m_gameLogic._m_gameShow.closeOpMask(opMaskSerialize);
                    
                    if(stateEnterSerialize != enterSerialize)
                        return;

                    if (!_isSucc || _msg == null)
                    {
                        Debug.LogError($"[TileMatchGameStateMachineState_Init] Failed to initialize Tile Match blocks: _isSucc:{_isSucc} _msg:{_msg}");

                        Action onfail = _m_aOnFail;
                        _m_aOnComplete = null;
                        _m_aOnFail = null;
                        onfail?.Invoke();
                        return;
                    }
                    
                    _m_gameLogic._fillBlockInfoArray(_msg.getBlockList());
                    _m_gameLogic._updateTaskInfo(_msg.getTaskInfo());
                    
                    Action onComplete = _m_aOnComplete;
                    _m_aOnComplete = null;
                    _m_aOnFail = null;
                    onComplete?.Invoke();
                });
            }

            protected override void _onExit()
            {
                _m_aOnComplete = null;
                _m_aOnFail = null;
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

            public void init(Action _complete, Action _onFail)
            {
                _m_aOnComplete = _complete;
                _m_aOnFail = _onFail;
            }
            
            public override void doPress(TileMatchBlockInfo _cubeInfo)
            {
            }
        }
    }
}
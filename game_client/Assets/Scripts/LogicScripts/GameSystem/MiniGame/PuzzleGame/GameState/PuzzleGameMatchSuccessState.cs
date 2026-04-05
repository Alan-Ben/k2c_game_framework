using JetBrains.Annotations;

namespace GOE.MiniGame
{
    public class PuzzleGameMatchSuccessState : _APuzzleGameState
    {
        public PuzzleGameMatchSuccessState([NotNull] PuzzleGameLogic _gameLogic) : base(_gameLogic)
        {
        }

        public override EPuzzleGameState state { get { return EPuzzleGameState.MATCH_SUCCESS; } }
        
        protected override void _onEnterSub()
        {
            _m_gameLogic.puzzleGameMainWndUnit.setGameShowState(EPuzzleGameState.MATCH_SUCCESS);

            long gameSerialize = _m_gameLogic.gameSerializeId;
            // 暂时不需要什么表现, 直接变回idle状态，延迟到下一帧变回idle状态, 防止同一帧内多次变更状态导致UI动画状态机变化错误
            CommonTaskController.CommonActionAddNextFrameTask(() =>
            {
                if(gameSerialize != _m_gameLogic.gameSerializeId)
                    return;
               
                _m_gameLogic.stateMachine.changeState(new PuzzleGameIdleState(_m_gameLogic));
            });
        }
        
        protected override void _onExitSub()
        {
        }

        public override bool canEnterState(EPuzzleGameState _newState)
        {
            return _newState is EPuzzleGameState.IDLE or EPuzzleGameState.NONE;
        }
    }
}
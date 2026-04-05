using JetBrains.Annotations;

namespace GOE.MiniGame
{
    public class PuzzleGameIdleState : _APuzzleGameState
    {
        public PuzzleGameIdleState([NotNull] PuzzleGameLogic _gameLogic) : base(_gameLogic)
        {
        }

        public override EPuzzleGameState state { get { return EPuzzleGameState.IDLE; } }
        
        protected override void _onEnterSub()
        {
            _m_gameLogic.puzzleGameMainWndUnit.setGameShowState(EPuzzleGameState.IDLE);

            long gameSerialize = _m_gameLogic.gameSerializeId;
            // 延迟到下一帧检查游戏是否胜利, 防止同一帧内多次变更状态导致UI动画状态机变化错误
            CommonTaskController.CommonActionAddNextFrameTask(() =>
            {
                if(gameSerialize != _m_gameLogic.gameSerializeId)
                    return;
                
                _m_gameLogic.checkGameSuccess();//检查游戏是否胜利
            });
        }
        
        protected override void _onExitSub()
        {
        }

        public override bool canEnterState(EPuzzleGameState _newState)
        {
            return _newState is EPuzzleGameState.WAITING_MATCH or EPuzzleGameState.GAME_SUCCESS or EPuzzleGameState.NONE;
        }
    }
}
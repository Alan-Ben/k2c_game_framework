using JetBrains.Annotations;

namespace GOE.MiniGame
{
    public class PuzzleGameWaitingMatchState : _APuzzleGameState
    {
        public PuzzleGameWaitingMatchState([NotNull] PuzzleGameLogic _gameLogic) : base(_gameLogic)
        {
        }

        public override EPuzzleGameState state { get { return EPuzzleGameState.WAITING_MATCH; } }
        
        protected override void _onEnterSub()
        {
            _m_gameLogic.puzzleGameMainWndUnit.setGameShowState(EPuzzleGameState.WAITING_MATCH);
        }
        
        protected override void _onExitSub()
        {
        }

        public override bool canEnterState(EPuzzleGameState _newState)
        {
            return _newState is EPuzzleGameState.MATCH_SUCCESS or EPuzzleGameState.MATCH_FAIL_RESUME or EPuzzleGameState.NONE;
        }
    }
}
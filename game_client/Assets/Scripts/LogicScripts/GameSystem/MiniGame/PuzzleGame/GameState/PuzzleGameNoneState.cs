using JetBrains.Annotations;

namespace GOE.MiniGame
{
    public class PuzzleGameNoneState : _APuzzleGameState
    {
        public PuzzleGameNoneState([NotNull] PuzzleGameLogic _gameLogic) : base(_gameLogic)
        {
        }

        public override EPuzzleGameState state { get { return EPuzzleGameState.NONE; } }
        
        protected override void _onEnterSub()
        {
            _m_gameLogic.puzzleGameMainWndUnit.setGameShowState(EPuzzleGameState.NONE, true);
        }
        
        protected override void _onExitSub()
        {
        }

        public override bool canEnterState(EPuzzleGameState _newState)
        {
            return _newState is EPuzzleGameState.IDLE;
        }
    }
}
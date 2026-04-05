using JetBrains.Annotations;

namespace GOE.MiniGame
{
    public class PuzzleGameSuccessState : _APuzzleGameState
    {
        public PuzzleGameSuccessState([NotNull] PuzzleGameLogic _gameLogic) : base(_gameLogic)
        {
        }

        public override EPuzzleGameState state { get { return EPuzzleGameState.GAME_SUCCESS; } }
        
        protected override void _onEnterSub()
        {
            _m_gameLogic.puzzleGameMainWndUnit.setGameShowState(EPuzzleGameState.GAME_SUCCESS);
            _m_gameLogic.puzzleGameMainWndUnit.playGameSuccess(() =>
            {
                _m_gameLogic.setGameSuccess(true);
            });
        }
        
        protected override void _onExitSub()
        {
        }

        public override bool canEnterState(EPuzzleGameState _newState)
        {
            return _newState is EPuzzleGameState.NONE;
        }
    }
}
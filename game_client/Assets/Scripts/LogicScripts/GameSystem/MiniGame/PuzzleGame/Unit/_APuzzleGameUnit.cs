using JetBrains.Annotations;

namespace GOE.MiniGame
{
    public abstract class _APuzzleGameUnit : _AGameUnit
    {
        [NotNull] protected PuzzleGameLogic _m_puzzleGameLogic;
        [NotNull] protected PuzzleGameController _m_puzzleGameController;

        public _APuzzleGameUnit([NotNull] PuzzleGameLogic _gameLogic, [NotNull] PuzzleGameController _gameController) : base(_gameLogic)
        {
            _m_puzzleGameLogic = _gameLogic;
            _m_puzzleGameController = _gameController;
        }
    }
}
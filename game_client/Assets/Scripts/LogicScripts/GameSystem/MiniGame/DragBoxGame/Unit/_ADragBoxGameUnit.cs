using JetBrains.Annotations;

namespace GOE.MiniGame
{
    public abstract class _ADragBoxGameUnit : _AGameUnit
    {
        [NotNull] protected DragBoxGameLogic _m_gameLogic;
        [NotNull] protected DragBoxGameController _m_gameController;

        public _ADragBoxGameUnit([NotNull] DragBoxGameLogic _gameLogic, [NotNull] DragBoxGameController _gameController) : base(_gameLogic)
        {
            _m_gameLogic = _gameLogic;
            _m_gameController = _gameController;
        }
    }
}
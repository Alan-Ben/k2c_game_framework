using JetBrains.Annotations;

namespace GOE.MiniGame
{
    public abstract class _ATakeThingsSequentiallyGameUnit : _AGameUnit
    {
        [NotNull] protected TakeThingsSequentiallyGameLogic _m_gameLogic;
        [NotNull] protected TakeThingsSequentiallyGameController _m_gameController;

        public _ATakeThingsSequentiallyGameUnit([NotNull] TakeThingsSequentiallyGameLogic _gameLogic, [NotNull] TakeThingsSequentiallyGameController _gameController) : base(_gameLogic)
        {
            _m_gameLogic = _gameLogic;
            _m_gameController = _gameController;
        }
    }
}
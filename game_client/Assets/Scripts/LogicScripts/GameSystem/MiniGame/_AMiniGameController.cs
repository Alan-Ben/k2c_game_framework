using JetBrains.Annotations;

namespace GOE.MiniGame
{
    public abstract class _AMiniGameController
    {
        [NotNull] protected _AMiniGameLogic _m_gameLogic;
        
        internal _AMiniGameController([NotNull]_AMiniGameLogic _gameLogic)
        {
            _m_gameLogic = _gameLogic;
        }

        public void setGameSuccess(bool _isSuccess)
        {
            _m_gameLogic.setGameSuccess(_isSuccess);
        }
    }
}
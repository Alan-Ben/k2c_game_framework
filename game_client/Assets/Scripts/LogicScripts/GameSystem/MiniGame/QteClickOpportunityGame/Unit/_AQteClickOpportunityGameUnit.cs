using JetBrains.Annotations;

namespace GOE.MiniGame
{
    public abstract class _AQteClickOpportunityGameUnit : _AGameUnit
    {
        [NotNull] protected QteClickOpportunityGameLogic _m_gameLogic;
        [NotNull] protected QteClickOpportunityGameController _m_gameController;

        public _AQteClickOpportunityGameUnit([NotNull] QteClickOpportunityGameLogic _gameLogic, [NotNull] QteClickOpportunityGameController _gameController) : base(_gameLogic)
        {
            _m_gameLogic = _gameLogic;
            _m_gameController = _gameController;
        }
    }
}
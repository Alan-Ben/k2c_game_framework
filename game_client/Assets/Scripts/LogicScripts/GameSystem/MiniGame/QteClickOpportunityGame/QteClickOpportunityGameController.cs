using JetBrains.Annotations;

namespace GOE.MiniGame
{
    public class QteClickOpportunityGameController : _AMiniGameController
    {
        [NotNull] private QteClickOpportunityGameLogic _m_gameLogic;
        public QteClickOpportunityGameController([NotNull] QteClickOpportunityGameLogic _gameLogic) : base(_gameLogic)
        {
            _m_gameLogic = _gameLogic;
        }
        
        
    }
}
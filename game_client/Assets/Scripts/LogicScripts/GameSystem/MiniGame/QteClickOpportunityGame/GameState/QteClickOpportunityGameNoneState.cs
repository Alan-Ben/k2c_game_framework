using JetBrains.Annotations;

namespace GOE.MiniGame
{
    public class QteClickOpportunityGameNoneState : _AQteClickOpportunityGameState
    {
        public QteClickOpportunityGameNoneState([NotNull] QteClickOpportunityGameLogic _gameLogic) : base(_gameLogic)
        {
        }

        public override EQteClickOpportunityGameState state { get { return EQteClickOpportunityGameState.NONE; } }

        protected override void _onEnterSub()
        {
        }

        protected override void _onExitSub()
        {
        }
        
        public override bool canEnterState(EQteClickOpportunityGameState _newState)
        {
            return _newState is EQteClickOpportunityGameState.PLAYING;
        }
    }
}
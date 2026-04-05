using JetBrains.Annotations;

namespace GOE.MiniGame
{
    public class QteClickOpportunityGamePlayingState : _AQteClickOpportunityGameState
    {
        public QteClickOpportunityGamePlayingState([NotNull] QteClickOpportunityGameLogic gameLogic) : base(gameLogic)
        {
        }

        public override EQteClickOpportunityGameState state { get { return EQteClickOpportunityGameState.PLAYING; } }

        protected override void _onEnterSub()
        {
        }

        protected override void _onExitSub()
        {
        }
        
        public override bool canEnterState(EQteClickOpportunityGameState newState)
        {
            return newState is EQteClickOpportunityGameState.NONE or EQteClickOpportunityGameState.SUCCESS or EQteClickOpportunityGameState.FAIL;
        }
    }
}
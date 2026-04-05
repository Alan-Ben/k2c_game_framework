using JetBrains.Annotations;

namespace GOE.MiniGame
{
    public class QteClickOpportunityGameSuccessState : _AQteClickOpportunityGameState
    {
        public QteClickOpportunityGameSuccessState([NotNull] QteClickOpportunityGameLogic gameLogic) : base(gameLogic)
        {
        }

        public override EQteClickOpportunityGameState state { get { return EQteClickOpportunityGameState.SUCCESS; } }

        protected override void _onEnterSub()
        {
            long serializeId = _m_gameLogic.gameSerializeId;
            CommonTaskController.CommonActionAddMonoTask(() =>
            {
                if (serializeId != _m_gameLogic.gameSerializeId)
                    return;
                
                _m_gameLogic.setGameSuccess(true);
            }, _m_gameLogic.qteClickOpportunityGameRefObj?.on_success_quit_delay_time ?? 0f);
        }

        protected override void _onExitSub()
        {
        }
        
        public override bool canEnterState(EQteClickOpportunityGameState newState)
        {
            return newState is EQteClickOpportunityGameState.NONE;
        }
    }
}
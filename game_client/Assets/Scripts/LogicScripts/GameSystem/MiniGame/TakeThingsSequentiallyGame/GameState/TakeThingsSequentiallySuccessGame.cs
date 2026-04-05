using JetBrains.Annotations;

namespace GOE.MiniGame
{
    public class TakeThingsSequentiallySuccessGame : _ATakeThingsSequentiallyGameState
    {
        public TakeThingsSequentiallySuccessGame([NotNull] TakeThingsSequentiallyGameLogic _gameLogic) : base(_gameLogic)
        {
        }

        public override ETakeThingsSequentiallyGameState state { get { return ETakeThingsSequentiallyGameState.SUCCESS; } }

        protected override void _onEnterSub()
        {
            long serializeId = _m_gameLogic.gameSerializeId;
            CommonTaskController.CommonActionAddMonoTask(() =>
            {
                if (serializeId != _m_gameLogic.gameSerializeId)
                    return;
                
                _m_gameLogic.setGameSuccess(true);
            }, _m_gameLogic.takeThingsSequentiallyGameRefObj?.on_success_quit_delay_time ?? 0f);
        }

        protected override void _onExitSub()
        {
        }
        
        public override bool canEnterState(ETakeThingsSequentiallyGameState _newState)
        {
            return _newState is ETakeThingsSequentiallyGameState.NONE;
        }
    }
}
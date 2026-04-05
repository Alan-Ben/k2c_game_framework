using JetBrains.Annotations;

namespace GOE.MiniGame
{
    public class TakeThingsSequentiallyIdleGame : _ATakeThingsSequentiallyGameState
    {
        public TakeThingsSequentiallyIdleGame([NotNull] TakeThingsSequentiallyGameLogic _gameLogic) : base(_gameLogic)
        {
        }

        public override ETakeThingsSequentiallyGameState state { get { return ETakeThingsSequentiallyGameState.IDLE; } }

        protected override void _onEnterSub()
        {
        }

        protected override void _onExitSub()
        {
        }
        
        public override bool canEnterState(ETakeThingsSequentiallyGameState _newState)
        {
            return _newState is ETakeThingsSequentiallyGameState.NONE or ETakeThingsSequentiallyGameState.SUCCESS;
        }
    }
}
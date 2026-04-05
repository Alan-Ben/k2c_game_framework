using JetBrains.Annotations;

namespace GOE.MiniGame
{
    public class TakeThingsSequentiallyNoneGame : _ATakeThingsSequentiallyGameState
    {
        public TakeThingsSequentiallyNoneGame([NotNull] TakeThingsSequentiallyGameLogic _gameLogic) : base(_gameLogic)
        {
        }

        public override ETakeThingsSequentiallyGameState state { get { return ETakeThingsSequentiallyGameState.NONE; } }

        protected override void _onEnterSub()
        {
        }

        protected override void _onExitSub()
        {
        }
        
        public override bool canEnterState(ETakeThingsSequentiallyGameState _newState)
        {
            return _newState is ETakeThingsSequentiallyGameState.IDLE;
        }
    }
}
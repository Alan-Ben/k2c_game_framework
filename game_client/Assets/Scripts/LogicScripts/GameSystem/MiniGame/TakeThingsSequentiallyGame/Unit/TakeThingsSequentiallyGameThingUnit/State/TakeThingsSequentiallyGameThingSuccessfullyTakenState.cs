using JetBrains.Annotations;

namespace GOE.MiniGame
{
    public class TakeThingsSequentiallyGameThingSuccessfullyTakenState : _ATakeThingsSequentiallyGameThingState
    {
        public TakeThingsSequentiallyGameThingSuccessfullyTakenState([NotNull] TakeThingsSequentiallyGameThingUnit _thingUnit) : base(_thingUnit)
        {
        }

        public override ETakeThingsSequentiallyGameThingState state { get { return ETakeThingsSequentiallyGameThingState.SUCCESSFULLY_TAKEN; } }

        protected override void _onEnterSub()
        {
        }

        protected override void _onExitSub()
        {
        }
        
        public override bool canEnterState(ETakeThingsSequentiallyGameThingState _newState)
        {
            return _newState is ETakeThingsSequentiallyGameThingState.NONE;
        }
    }
}
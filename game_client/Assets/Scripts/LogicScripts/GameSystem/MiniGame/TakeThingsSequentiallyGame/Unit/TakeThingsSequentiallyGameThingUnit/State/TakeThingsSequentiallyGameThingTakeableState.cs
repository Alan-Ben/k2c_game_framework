using JetBrains.Annotations;

namespace GOE.MiniGame
{
    public class TakeThingsSequentiallyGameThingTakeableState : _ATakeThingsSequentiallyGameThingState
    {
        public TakeThingsSequentiallyGameThingTakeableState([NotNull] TakeThingsSequentiallyGameThingUnit _thingUnit) : base(_thingUnit)
        {
        }

        public override ETakeThingsSequentiallyGameThingState state { get { return ETakeThingsSequentiallyGameThingState.TAKEABLE; } }

        protected override void _onEnterSub()
        {
        }

        protected override void _onExitSub()
        {
        }
        
        public override bool canEnterState(ETakeThingsSequentiallyGameThingState _newState)
        {
            return _newState is ETakeThingsSequentiallyGameThingState.SUCCESSFULLY_TAKEN or ETakeThingsSequentiallyGameThingState.NONE;
        }
    }
}
using JetBrains.Annotations;

namespace GOE.MiniGame
{
    public class TakeThingsSequentiallyGameThingNotTakeableState : _ATakeThingsSequentiallyGameThingState
    {
        public TakeThingsSequentiallyGameThingNotTakeableState([NotNull] TakeThingsSequentiallyGameThingUnit _thingUnit) : base(_thingUnit)
        {
        }

        public override ETakeThingsSequentiallyGameThingState state { get { return ETakeThingsSequentiallyGameThingState.NOT_TAKEABLE; } }

        protected override void _onEnterSub()
        {
        }

        protected override void _onExitSub()
        {
        }
        
        public override bool canEnterState(ETakeThingsSequentiallyGameThingState _newState)
        {
            return _newState is ETakeThingsSequentiallyGameThingState.TAKEABLE or ETakeThingsSequentiallyGameThingState.NONE;
        }
    }
}
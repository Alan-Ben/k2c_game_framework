using JetBrains.Annotations;

namespace GOE.MiniGame
{
    public class TakeThingsSequentiallyGameThingNoneState : _ATakeThingsSequentiallyGameThingState
    {
        public TakeThingsSequentiallyGameThingNoneState([NotNull] TakeThingsSequentiallyGameThingUnit _thingUnit) : base(_thingUnit)
        {
        }

        public override ETakeThingsSequentiallyGameThingState state { get { return ETakeThingsSequentiallyGameThingState.NONE; } }

        protected override void _onEnterSub()
        {
        }

        protected override void _onExitSub()
        {
        }
        
        public override bool canEnterState(ETakeThingsSequentiallyGameThingState _newState)
        {
            return _newState is ETakeThingsSequentiallyGameThingState.TAKEABLE or ETakeThingsSequentiallyGameThingState.NOT_TAKEABLE;
        }
    }
}
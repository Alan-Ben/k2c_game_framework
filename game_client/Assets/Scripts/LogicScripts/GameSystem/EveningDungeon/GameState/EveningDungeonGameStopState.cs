namespace GOE.EveningDungeon
{
    public class EveningDungeonGameStopState : _AEveningDungeonGameState
    {
        public EveningDungeonGameStopState(EveningDungeonGameLogic _gameLogic) : base(_gameLogic)
        {
        }

        public override EEveningDungeonGameState state { get { return EEveningDungeonGameState.STOP; } }
        public override bool canEnterState(EEveningDungeonGameState _newState)
        {
            return _newState is EEveningDungeonGameState.IDLE 
                or EEveningDungeonGameState.WAITING_BOSS_REVIVE
                or EEveningDungeonGameState.BOSS_COMPLETELY_DEAD
                or EEveningDungeonGameState.STOP;
        }

        protected override void _onEnterSub()
        {
        }

        protected override void _onExitSub()
        {
        }
    }
}
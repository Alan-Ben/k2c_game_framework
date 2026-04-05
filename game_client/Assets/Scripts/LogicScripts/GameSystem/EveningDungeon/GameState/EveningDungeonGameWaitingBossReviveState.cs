namespace GOE.EveningDungeon
{
    public class EveningDungeonGameWaitingBossReviveState : _AEveningDungeonGameState
    {
        public EveningDungeonGameWaitingBossReviveState(EveningDungeonGameLogic _gameLogic) : base(_gameLogic)
        {
        }

        public override EEveningDungeonGameState state { get { return EEveningDungeonGameState.WAITING_BOSS_REVIVE; } }
        public override bool canEnterState(EEveningDungeonGameState _newState)
        {
            return _newState is EEveningDungeonGameState.STOP 
                or EEveningDungeonGameState.IDLE;
        }

        protected override void _onEnterSub()
        {
        }

        protected override void _onExitSub()
        {
        }
    }
}
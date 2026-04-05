namespace GOE.EveningDungeon
{
    /// <summary>
    /// BOSS完全死亡状态(没有复活次数)
    /// </summary>
    public class EveningDungeonGameBossCompletelyDeadState : _AEveningDungeonGameState
    {
        public EveningDungeonGameBossCompletelyDeadState(EveningDungeonGameLogic _gameLogic) : base(_gameLogic)
        {
        }

        public override EEveningDungeonGameState state { get { return EEveningDungeonGameState.BOSS_COMPLETELY_DEAD; } }
        public override bool canEnterState(EEveningDungeonGameState _newState)
        {
            return _newState is EEveningDungeonGameState.STOP;
        }

        protected override void _onEnterSub()
        {
        }

        protected override void _onExitSub()
        {
        }
    }
}
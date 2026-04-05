using JetBrains.Annotations;

namespace GOE
{
    public partial class TreasureHuntGameLogic
    {
        // 待机状态
        public class TreasureHuntGameIdleState : _ATreasureHuntGameState
        {
            public TreasureHuntGameIdleState([NotNull] TreasureHuntGameLogic _gameLogic)
                : base(_gameLogic)
            {
            }


            public override TreasureHuntGameStateType state { get { return TreasureHuntGameStateType.IDLE; } }


            protected override void _onEnter()
            {
                // 设置玩家单位速度为待机速度，清空额外速度
                gameLogic._m_playerUnit.animTriggerIdleStart();
                gameLogic._m_playerUnit.setToIdleSpeed(true);
                gameLogic._m_playerUnit.setAdditionalSpeed(0, true);
                gameLogic._m_playerUnit.resetHorizontalToDefault(true);
                gameLogic._m_playerUnit.animSetBoostRemainTime(-1);
                // 清除所有敌人单位
                gameLogic._clearAllEnemyUnits();
                gameLogic._clearAllRewardUnits();
                gameLogic._clearAllRewardDistanceLineUnits();
                // UI 切换到游戏的待机界面
                GMainGUIAddSceneTreasureHuntGame.instance.switchToGameMain();
                // 设置游戏控制器
                GGUIWndTreasureHuntGameMain.instance.setGameController(gameLogic._m_gameController);
            }
            protected override void _onExit()
            {
                GGUIWndTreasureHuntGameMain.instance.setGameController(null);
            }
            public override bool canEnterState(TreasureHuntGameStateType _newState)
            {
                return _newState is TreasureHuntGameStateType.RUN_UP or TreasureHuntGameStateType.NONE;
            }
        }
    }
}
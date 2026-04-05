using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public partial class TreasureHuntGameLogic
    {
        public class TreasureHuntGameGamingState : _ATreasureHuntGameState<float, float, bool>, _ITreasureHuntTickState
        {
            private float _m_gameStartPos; // 游戏开始位置
            private float _m_targetDistance; // 游戏目标距离
            private bool _m_isAdvance; // 是否是高级游戏

            private TreasureHuntAreaDistanceRefObj _m_curDistanceRefObj;
            private TreasureHuntAreaDistanceRefObj _m_nextDistanceRefObj;
            
            private float _m_enemySpawnTimer; // 敌人生成计时器
            
            
            public TreasureHuntGameGamingState([NotNull] TreasureHuntGameLogic _gameLogic)
                : base(_gameLogic)
            {
            }


            public override TreasureHuntGameStateType state { get { return TreasureHuntGameStateType.GAMING; } }


            protected override void _onEnter(float _startPos, float _targetDistance, bool _isAdvance)
            {
                _m_gameStartPos = _startPos;
                _m_targetDistance = _targetDistance;
                _m_isAdvance = _isAdvance;
                // 设置玩家单位速度降到游戏速度，先清空额外速度
                gameLogic._m_playerUnit.resetAllTrigger();
                gameLogic._m_playerUnit.animTriggerGamingStart();
                gameLogic._m_playerUnit.animSetGamingState(true);
                gameLogic._m_playerUnit.setToSearchingSpeed();
                gameLogic._m_playerUnit.setAdditionalSpeed(0);
                gameLogic._m_playerUnit.animSetBoostRemainTime(-1);
                // 加入游戏提示界面
                GMainGUIAddSceneTreasureHuntGame.instance.addGamingTip();
                // 初始化敌人生成计时器
                _m_enemySpawnTimer = 10f;
                // 初始化距离配置RefObj缓存
                gameLogic.refreshDistanceRefObjs(_m_gameStartPos, out _m_curDistanceRefObj, out _m_nextDistanceRefObj);
                // 刷新游戏显示
                GGUIWndTreasureHuntGamePlay.instance.setGamingShow();
                // 设置游戏控制器
                GGUIWndTreasureHuntGamePlay.instance.setGameController(gameLogic._m_gameController);
            }
            protected override void _onExit()
            {
                gameLogic._m_playerUnit.animSetGamingState(false);
                // 清除游戏控制器
                GGUIWndTreasureHuntGamePlay.instance.setGameController(null);
                // 移除游戏提示界面
                GMainGUIAddSceneTreasureHuntGame.instance.removeGamingTip();
            }
            public void tick(float _deltaTime)
            {
                // 计算当前已飞行距离
                float currentDistance = gameLogic._m_playerUnit.forwardOffset - _m_gameStartPos;
                float worldScale = MainAdditionTreasureHuntGameTDScene.instance.getWorldScale();
                
                // 更新UI显示 (转换回原始尺度显示)
                GGUIWndTreasureHuntGamePlay.instance.refreshDistance(currentDistance / worldScale, _m_targetDistance / worldScale);
                GGUIWndTreasureHuntGamePlay.instance.refreshHealthCount(gameLogic._m_playerUnit.currentHealth, gameLogic._m_playerUnit.maxHealth);
                
                // 刷新距离配置RefObj（使用共享工具函数）
                if (gameLogic.shouldRefreshDistanceRefObj(_m_gameStartPos, _m_curDistanceRefObj, _m_nextDistanceRefObj))
                {
                    gameLogic.refreshDistanceRefObjs(_m_gameStartPos, out _m_curDistanceRefObj, out _m_nextDistanceRefObj);
                }
                
                // 更新玩家速度加成（使用共享工具函数）
                gameLogic.updatePlayerSpeedBonus(_m_gameStartPos, _m_curDistanceRefObj, _m_nextDistanceRefObj);
                
                // 处理敌人生成
                _updateEnemySpawn(_deltaTime);
                
                // 检查玩家死亡
                if (gameLogic._m_playerUnit.isDead)
                {
                    // 玩家死亡，结束游戏
                    gameLogic._m_stateMachine.changeState(new TreasureHuntGameEndState(gameLogic), false, _m_isAdvance, _m_gameStartPos);
                    return;
                }
                
                // 检查是否达到目标距离
                if (currentDistance >= _m_targetDistance)
                {
                    // 达到目标距离，游戏胜利结束
                    gameLogic._m_stateMachine.changeState(new TreasureHuntGameEndState(gameLogic), true, _m_isAdvance, _m_gameStartPos);
                    return;
                }
            }
            public override bool canEnterState(TreasureHuntGameStateType _newState)
            {
                return _newState is TreasureHuntGameStateType.END or TreasureHuntGameStateType.NONE;
            }


            private void _updateEnemySpawn(float _deltaTime)
            {
                // 使用共享工具函数检查是否应该生成敌人
                if (gameLogic.shouldSpawnEnemyByInterval(_m_curDistanceRefObj, ref _m_enemySpawnTimer, _deltaTime))
                {
                    // 计算当前游戏范围
                    float currentDistance = gameLogic._m_playerUnit.forwardOffset - _m_gameStartPos;
                    float minGameDistance = _m_gameStartPos + currentDistance;
                    float maxGameDistance = _m_gameStartPos + _m_targetDistance;
                    
                    // 只在游戏范围内生成敌人
                    gameLogic.trySpawnEnemyInRange(minGameDistance, maxGameDistance);
                }
            }
        }
    }
}
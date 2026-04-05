using System.Collections.Generic;
using JetBrains.Annotations;

namespace GOE
{
    public partial class TreasureHuntGameLogic
    {
        /// <summary>
        /// 助跑状态（其实逻辑上是游戏开始状态）
        /// </summary>
        public class TreasureHuntGameRunUpState : _ATreasureHuntGameState<bool>, _ITreasureHuntTickState
        {
            // 游戏开始的位置，用来计算当前跑了多远
            private float _m_gameStartPos;
            // 加速距离
            private float _m_boostDistance;
            // 关卡的目标距离
            private float _m_targetDistance;
            // 玩家生命值
            private int _m_healthCount;
            // 是否是高级游戏
            private bool _m_isAdvance;
            // 敌人生成计时器
            private float _m_enemySpawnTimer;
            // RefObj缓存
            private TreasureHuntAreaDistanceRefObj _m_curDistanceRefObj;
            private TreasureHuntAreaDistanceRefObj _m_nextDistanceRefObj;
            
            
            public TreasureHuntGameRunUpState([NotNull] TreasureHuntGameLogic _gameLogic)
                : base(_gameLogic)
            {
            }


            public override TreasureHuntGameStateType state { get { return TreasureHuntGameStateType.RUN_UP; } }


            protected override void _onEnter(bool _isAdvance)
            {
                _m_isAdvance = _isAdvance;
                // 初始化游戏相关参数
                _m_gameStartPos = gameLogic._m_playerUnit.forwardOffset;
                float worldScale = MainAdditionTreasureHuntGameTDScene.instance.getWorldScale();
                _m_boostDistance = (NPPlayer.instance?.treasureHuntComponent?.stationInfo?.stationLvlRefObj?.auto_fly_distance ?? 0) * worldScale;
                _m_targetDistance = (NPPlayer.instance?.treasureHuntComponent?.stationInfo?.stationLvlRefObj?.max_fly_distance ?? 0) * worldScale;
                _m_healthCount = NPPlayer.instance?.treasureHuntComponent?.stationInfo?.stationLvlRefObj?.fly_protect_times ?? 0;
                _m_healthCount += 1; // 保护次数为 0 时，玩家至少有一次保护机会
                // 设置玩家单位速度为助跑速度，清空额外速度
                gameLogic._m_playerUnit.setToBoostSpeed(true);
                gameLogic._m_playerUnit.setAdditionalSpeed(0, true);
                gameLogic._m_playerUnit.setHealthCount(_m_healthCount);
                gameLogic._m_playerUnit.animSetBoostState(true);
                gameLogic._m_playerUnit.playBoostSound();
                // 清空所有敌人单位
                gameLogic._clearAllEnemyUnits();
                gameLogic._clearAllRewardUnits();
                gameLogic._clearAllRewardDistanceLineUnits();
                // 切换 UI 到游戏界面
                GMainGUIAddSceneTreasureHuntGame.instance.switchToGamePlay();
                // 添加当前助跑的提示界面
                GMainGUIAddSceneTreasureHuntGame.instance.addRunUpTip();
                // 刷新区域相关显示
                GGUIWndTreasureHuntGamePlay.instance.setRunUpShow();
                GGUIWndTreasureHuntGamePlay.instance.refreshArea(gameLogic._m_areaRefObj);
                // 刷新一次相关 UI (转换回原始尺度显示)
                GGUIWndTreasureHuntGamePlay.instance.refreshDistance(0, _m_targetDistance / worldScale);
                GGUIWndTreasureHuntGamePlay.instance.refreshBoostDistance(0, _m_boostDistance / worldScale);
                GGUIWndTreasureHuntGamePlay.instance.refreshHealthCount(_m_healthCount, _m_healthCount);
                if (gameLogic._m_playerUnit.currentForwardSpeed > 0) 
                    GGUIWndTreasureHuntGamePlayRunUpTip.instance.refreshTipTime(_m_boostDistance / gameLogic._m_playerUnit.currentForwardSpeed);

                // 直接一口气把宝箱全生成了，策划说数量会很少，大概 5 个左右
                List<TreasureHuntAreaDistanceRefObj> areaDistanceRefList = GRefdataCoreMgr.instance.treasureHuntAreaDistanceRefCore.refList;
                int rewardNum = 0;
                if (areaDistanceRefList != null)
                {
                    foreach (TreasureHuntAreaDistanceRefObj areaDistance in areaDistanceRefList)
                    {
                        // 需要资源正确配置，同时奖励要在可达到的距离内
                        float scaledDistance = areaDistance.distance * worldScale;
                        if (areaDistance?.reward_box_go_index != null && areaDistance.reward_box_go_index.isValid() && scaledDistance <= _m_targetDistance)
                        {
                            gameLogic.spawnReward(_m_gameStartPos + scaledDistance, areaDistance.reward_box_go_index);
                            rewardNum++;
                        }
                    }
                }

                gameLogic._setTotalRewardNum(rewardNum);
                
                // 初始化敌人生成计时器和RefObj缓存
                _m_enemySpawnTimer = 0f;
                _m_curDistanceRefObj = null;
                _m_nextDistanceRefObj = null;
            }
            protected override void _onExit()
            {
                gameLogic._m_playerUnit.animSetBoostState(false);
                // 移除助跑提示界面
                GMainGUIAddSceneTreasureHuntGame.instance.removeRunUpTip();
            }
            public void tick(float _deltaTime)
            {
                // 计算当前已飞行距离
                float currentDistance = gameLogic._m_playerUnit.forwardOffset - _m_gameStartPos;
                float worldScale = MainAdditionTreasureHuntGameTDScene.instance.getWorldScale();
                
                // 更新UI显示 (转换回原始尺度显示)
                GGUIWndTreasureHuntGamePlay.instance.refreshDistance(currentDistance / worldScale, _m_targetDistance / worldScale);
                GGUIWndTreasureHuntGamePlay.instance.refreshBoostDistance(currentDistance / worldScale, _m_boostDistance / worldScale);
                
                // 更新助跑剩余时间提示
                float remainingBoostDistance = _m_boostDistance - currentDistance;
                if (remainingBoostDistance > 0 && gameLogic._m_playerUnit.currentForwardSpeed > 0)
                {
                    float remainingTime = remainingBoostDistance / gameLogic._m_playerUnit.currentForwardSpeed;
                    gameLogic._m_playerUnit.animSetBoostRemainTime(remainingTime);
                    GGUIWndTreasureHuntGamePlayRunUpTip.instance.refreshTipTime(remainingTime);
                    
                    // RunUp状态的游戏规则：只有在剩余时间小于配置时间时才生成敌人
                    float enemySpawnTime = MainAdditionTreasureHuntGameTDScene.instance.getEnemySpawnTimeInRunUpState();
                    if (remainingTime <= enemySpawnTime)
                    {
                        _updateEnemySpawn(_deltaTime);
                    }
                }
                
                // 检查是否完成助跑阶段
                if (currentDistance >= _m_boostDistance)
                {
                    // 助跑完成，切换到游戏状态
                    gameLogic._m_stateMachine.changeState(new TreasureHuntGameGamingState(gameLogic), _m_gameStartPos, _m_targetDistance, _m_isAdvance);
                    return;
                }
                
                // 检查是否达到最大距离（提前结束）
                if (currentDistance >= _m_targetDistance)
                {
                    // 达到最大距离，直接结束游戏
                    gameLogic._m_stateMachine.changeState(new TreasureHuntGameEndState(gameLogic), true, _m_isAdvance, _m_gameStartPos);
                    return;
                }
            }
            public override bool canEnterState(TreasureHuntGameStateType _newState)
            {
                return _newState is TreasureHuntGameStateType.GAMING or TreasureHuntGameStateType.END or TreasureHuntGameStateType.NONE;
            }


            private void _updateEnemySpawn(float _deltaTime)
            {
                // 刷新RefObj缓存（使用共享工具函数）
                if (gameLogic.shouldRefreshDistanceRefObj(_m_gameStartPos, _m_curDistanceRefObj, _m_nextDistanceRefObj))
                {
                    gameLogic.refreshDistanceRefObjs(_m_gameStartPos, out _m_curDistanceRefObj, out _m_nextDistanceRefObj);
                }
                
                // 使用共享工具函数检查是否应该生成敌人
                if (gameLogic.shouldSpawnEnemyByInterval(_m_curDistanceRefObj, ref _m_enemySpawnTimer, _deltaTime))
                {
                    // 计算当前游戏范围
                    float currentDistance = gameLogic._m_playerUnit.forwardOffset - _m_gameStartPos;
                    float minGameDistance = _m_gameStartPos + currentDistance;
                    float maxGameDistance = _m_gameStartPos + _m_targetDistance;
                    
                    // 在游戏范围内生成敌人
                    gameLogic.trySpawnEnemyInRange(minGameDistance, maxGameDistance);
                }
            }
        }
    }
}
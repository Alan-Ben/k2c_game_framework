using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public partial class TreasureHuntGameLogic : _AGameLogic
    {
        [NotNull] private readonly _TALSimpleStateMachine<TreasureHuntGameStateType> _m_stateMachine;
        
        [NotNull] private readonly RelativePosDealer _m_relativePosDealer;
        [NotNull] private readonly TreasureHuntGameController _m_gameController;
        
        [NotNull] private readonly TreasureHuntPlayerUnit _m_playerUnit;
        [ItemNotNull, NotNull] private readonly List<TreasureHuntEnemyUnit> _m_enemyList;
        [ItemNotNull, NotNull] private readonly List<TreasureHuntRewardUnit> _m_rewardList;
        [ItemNotNull, NotNull] private readonly List<TreasureHuntRewardDistanceLineUnit> _m_rewardDistanceLineList;
        [ItemNotNull, NotNull] private readonly HashSet<TreasureHuntRewardUnit> _m_passedRewards;
 
        private TreasureHuntGroundUnit _m_groundUnit;
        private TreasureHuntAreaRefObj _m_areaRefObj;

        private int _m_gainRewardNum;
        private int _m_totalRewardNum;
        
        
        public TreasureHuntGameLogic()
        {
            _m_stateMachine = new _TALSimpleStateMachine<TreasureHuntGameStateType>();
            
            addGameDealer(new CollisionCheckDealer(this));
            addGameDealer(_m_relativePosDealer = new RelativePosDealer(this));

            _m_gameController = new TreasureHuntGameController(this);

            _m_playerUnit = new TreasureHuntPlayerUnit(this);
            _m_enemyList = new List<TreasureHuntEnemyUnit>();
            _m_rewardList = new List<TreasureHuntRewardUnit>();
            _m_rewardDistanceLineList = new List<TreasureHuntRewardDistanceLineUnit>();
            _m_passedRewards = new HashSet<TreasureHuntRewardUnit>();
        }
        
        
        [NotNull] public TreasureHuntPlayerUnit playerUnit { get { return _m_playerUnit; } }
        [NotNull] public TreasureHuntGameController gameController { get { return _m_gameController; } }


        protected override void _startGameOp(Action _complete, Action _failed)
        {
            long areaId = NPPlayer.instance.treasureHuntComponent.saver.getAreaId();
            _m_areaRefObj = GRefdataCoreMgr.instance.treasureHuntAreaRefCore.getRef(areaId);
            if (_m_areaRefObj == null)
            {
                ALLog.Error($"Cannot start Treasure Hunt Game, area ref obj is null for areaId: {areaId}");
                _failed?.Invoke();
                return;
            }
            
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(2);
            stepCounter.regAllDoneDelegate(_complete);
            
            _m_playerUnit.load(stepCounter.addDoneStepCount);
            _m_groundUnit = new TreasureHuntGroundUnit(_m_areaRefObj.bg_res_index, this);
            _m_groundUnit.load(stepCounter.addDoneStepCount);
        }
        protected override void _onStart()
        {
            _m_relativePosDealer.setOriginPos(Vector3.zero);
            _m_playerUnit.resetHorizontalToDefault(true);
            _m_stateMachine.changeState(new TreasureHuntGameIdleState(this));
        }
        protected override void _onStop()
        {
            _m_playerUnit.unload();
            _m_groundUnit?.unload();
            _m_groundUnit = null;
            _clearAllEnemyUnits();
            _clearAllRewardUnits();
            _clearAllRewardDistanceLineUnits();
            _m_stateMachine.changeState(new NoneSimpleState<TreasureHuntGameStateType>());
        }
        protected override void _onTick(float _deltaTime)
        {
            _m_relativePosDealer.setOriginPos(_m_relativePosDealer.originPos.SetZ(_m_playerUnit.forwardOffset));
            if (_m_stateMachine.curState is _ITreasureHuntTickState tickState)
                tickState.tick(_deltaTime);
            
            _checkRewardCollection();
            _updateRewardTipDistance();
            _updateRewardDistanceLines();
        }


        public void spawnEnemy()
        {
            // 从区域配置中随机选择障碍物资源
            NPGGoIndex enemyResIndex = _getRandomEnemyResIndex();
            if (enemyResIndex == null)
            {
                ALLog.Warning("Cannot spawn enemy, no valid obstacle resources in area config");
                return;
            }
            
            Vector3 spawnRelativePosition = _calculateEnemySpawnPosition();
            Vector3 spawnLogicPosition = spawnRelativePosition + _m_relativePosDealer.originPos;
            
            TreasureHuntEnemyUnit enemyUnit = new TreasureHuntEnemyUnit(this, enemyResIndex, _onEnemyDestroyByItself, spawnRelativePosition, spawnLogicPosition);
            enemyUnit.load();
            _m_enemyList.Add(enemyUnit);
        }
        public bool trySpawnEnemyInRange(float _minDistance, float _maxDistance)
        {
            // 从区域配置中随机选择障碍物资源
            NPGGoIndex enemyResIndex = _getRandomEnemyResIndex();
            if (enemyResIndex == null)
            {
                ALLog.Warning("Cannot spawn enemy, no valid obstacle resources in area config");
                return false;
            }
            
            Vector3 spawnRelativePosition = _calculateEnemySpawnPosition();
            Vector3 spawnLogicPosition = spawnRelativePosition + _m_relativePosDealer.originPos;
            
            // 检查生成位置是否在游戏范围内
            float spawnDistance = spawnLogicPosition.z;
            if (spawnDistance >= _minDistance && spawnDistance <= _maxDistance)
            {
                // 在范围内，创建敌人单位
                TreasureHuntEnemyUnit enemyUnit = new TreasureHuntEnemyUnit(this, enemyResIndex, _onEnemyDestroyByItself, spawnRelativePosition, spawnLogicPosition);
                enemyUnit.load();
                _m_enemyList.Add(enemyUnit);
                return true;
            }
            
            return false;
        }
        public void spawnReward(float _logicPositionZ, NPGGoIndex _rewardResIndex)
        {
            if (_rewardResIndex == null)
            {
                ALLog.Warning("Cannot spawn reward, reward res index is null");
                return;
            }

            Vector3 spawnLogicPosition = _calculateRewardSpawnPosition(_logicPositionZ);
            Vector3 spawnRelativePosition = spawnLogicPosition - _m_relativePosDealer.originPos;

            // Create reward unit
            TreasureHuntRewardUnit rewardUnit = new TreasureHuntRewardUnit(this, _rewardResIndex, _onRewardDestroyByItself, spawnRelativePosition, spawnLogicPosition);
            rewardUnit.load();
            _m_rewardList.Add(rewardUnit);

            // Create distance line unit at same position but without random X offset
            Vector3 distanceLineLogicPosition = Vector3.zero;
            distanceLineLogicPosition.z = _logicPositionZ;
            distanceLineLogicPosition.x = 0f; // No random X offset
            Vector3 distanceLineRelativePosition = distanceLineLogicPosition - _m_relativePosDealer.originPos;
            float worldScale = MainAdditionTreasureHuntGameTDScene.instance.getWorldScale();
            float distanceToPlayer = Vector3.Distance(distanceLineLogicPosition, _m_playerUnit.position) / worldScale;

            NPGGoIndex distanceLineResIndex = MainAdditionTreasureHuntGameTDScene.instance.getRewardDistanceLineResIndex();
            if (distanceLineResIndex.isValid())
            {
                TreasureHuntRewardDistanceLineUnit distanceLineUnit = new TreasureHuntRewardDistanceLineUnit(this, distanceLineResIndex, _onRewardDistanceLineDestroyByItself, distanceLineRelativePosition, distanceLineLogicPosition, distanceToPlayer, worldScale);
                distanceLineUnit.load();
                _m_rewardDistanceLineList.Add(distanceLineUnit);
            }
        }
        public void startGame(bool _isAdvance)
        {
            _m_stateMachine.changeState(new TreasureHuntGameRunUpState(this), _isAdvance);   
        }


        // 共享工具函数：RefObj缓存刷新
        public bool shouldRefreshDistanceRefObj(float _gameStartPos, TreasureHuntAreaDistanceRefObj _curRefObj, TreasureHuntAreaDistanceRefObj _nextRefObj)
        {
            float currentDistance = _m_playerUnit.forwardOffset - _gameStartPos;
            float worldScale = MainAdditionTreasureHuntGameTDScene.instance.getWorldScale();
            
            // 如果还没有初始化RefObj，或者达到了下一个RefObj的距离阈值，则需要刷新
            if (_curRefObj == null && _nextRefObj == null)
            {
                return true; // 初始化
            }
            else if (_nextRefObj != null && currentDistance >= _nextRefObj.distance * worldScale)
            {
                return true; // 达到下一个RefObj的距离阈值，需要切换
            }
            
            return false;
        }
        public void refreshDistanceRefObjs(float _gameStartPos, out TreasureHuntAreaDistanceRefObj _curRefObj, out TreasureHuntAreaDistanceRefObj _nextRefObj)
        {
            float currentDistance = _m_playerUnit.forwardOffset - _gameStartPos;
            float worldScale = MainAdditionTreasureHuntGameTDScene.instance.getWorldScale();
            long originalDistance = (long)(currentDistance / worldScale);
            _curRefObj = GRefdataCoreMgr.instance.getTreasureHuntAreaDistanceRefObj(originalDistance);
            _nextRefObj = GRefdataCoreMgr.instance.getNextTreasureHuntAreaDistanceRefObj(originalDistance);
        }
        // 共享工具函数：玩家速度加成计算
        public void updatePlayerSpeedBonus(float _gameStartPos, TreasureHuntAreaDistanceRefObj _curRefObj, TreasureHuntAreaDistanceRefObj _nextRefObj)
        {
            float speedBonus = 0f;
            float worldScale = MainAdditionTreasureHuntGameTDScene.instance.getWorldScale();
            
            if (_curRefObj != null)
            {
                speedBonus = _curRefObj.player_speed_up_add;
                
                // 如果有下一个RefObj，进行插值；如果没有，则保持使用当前RefObj的值
                if (_nextRefObj != null)
                {
                    float curDistance = _curRefObj.distance * worldScale;  // 将refObj距离放大到游戏尺度
                    float nextDistance = _nextRefObj.distance * worldScale; // 将refObj距离放大到游戏尺度
                    
                    // 计算插值比例
                    if (nextDistance > curDistance)
                    {
                        float currentDistance = _m_playerUnit.forwardOffset - _gameStartPos;
                        float lerpT = (currentDistance - curDistance) / (nextDistance - curDistance);
                        lerpT = Mathf.Clamp01(lerpT);
                        
                        // 在当前和下一个RefObj的速度加成之间插值
                        speedBonus = Mathf.Lerp(_curRefObj.player_speed_up_add, _nextRefObj.player_speed_up_add, lerpT);
                    }
                }
                // 如果没有下一个RefObj，speedBonus保持为当前RefObj的值
            }
            
            // 应用速度加成到玩家
            _m_playerUnit.setAdditionalSpeed(speedBonus);
        }
        // 共享工具函数：敌人生成检查
        public bool shouldSpawnEnemyByInterval(TreasureHuntAreaDistanceRefObj _refObj, ref float _spawnTimer, float _deltaTime)
        {
            if (_refObj == null || _refObj.obstacle_spawn_rate_second <= 0)
                return false;
            
            _spawnTimer += _deltaTime;
            
            if (_spawnTimer >= _refObj.obstacle_spawn_rate_second)
            {
                _spawnTimer = 0f;
                return true;
            }
            
            return false;
        }


        private Vector3 _calculateEnemySpawnPosition()
        {
            GTDMonoTreasureHuntGame sceneMono = MainAdditionTreasureHuntGameTDScene.instance.getSceneMono();
            if (sceneMono == null)
            {
                ALLog.Error("Cannot calculate enemy spawn position, scene mono is null");
                return Vector3.zero;
            }

            if (sceneMono.transEnemySpawnPos == null)
            {
                ALLog.Error("Cannot calculate enemy spawn position, transEnemySpawnPos is null");
                return Vector3.zero;
            }

            Vector3 baseSpawnPos = sceneMono.transEnemySpawnPos.localPosition;
            
            // Add random X offset from range
            if (sceneMono.enemySpawnXRange != null)
            {
                float randomX = sceneMono.enemySpawnXRange.getRandomValue();
                baseSpawnPos.x += randomX;
            }

            return baseSpawnPos;
        }
        private void _onEnemyDestroyByItself(TreasureHuntEnemyUnit _enemy)
        {
            if (_enemy == null)
                return;
                
            _enemy.unload();
            _m_enemyList.Remove(_enemy);
        }
        private NPGGoIndex _getRandomEnemyResIndex()
        {
            // 检查区域RefObj是否存在
            if (_m_areaRefObj == null)
            {
                ALLog.Error("Cannot get random enemy res index, area ref obj is null");
                return null;
            }
            
            // 检查障碍物列表是否存在且不为空
            if (_m_areaRefObj.obstacle_res_list == null || _m_areaRefObj.obstacle_res_list.Count == 0)
            {
                ALLog.Error("Cannot get random enemy res index, obstacle_res_list is null or empty");
                return null;
            }
            
            // 随机选择一个障碍物资源
            int randomIndex = UnityEngine.Random.Range(0, _m_areaRefObj.obstacle_res_list.Count);
            return _m_areaRefObj.obstacle_res_list[randomIndex];
        }
        private Vector3 _calculateRewardSpawnPosition(float _logicPositionZ)
        {
            GTDMonoTreasureHuntGame sceneMono = MainAdditionTreasureHuntGameTDScene.instance.getSceneMono();
            if (sceneMono == null)
            {
                ALLog.Error("Cannot calculate reward spawn position, scene mono is null");
                return Vector3.zero;
            }

            Vector3 spawnPosition = Vector3.zero;
            spawnPosition.z = _logicPositionZ;
            
            // Use enemySpawnXRange to generate random X position
            if (sceneMono.enemySpawnXRange != null)
            {
                spawnPosition.x = sceneMono.enemySpawnXRange.getRandomValue();
            }

            return spawnPosition;
        }
        private void _onRewardDestroyByItself(TreasureHuntRewardUnit _reward)
        {
            if (_reward == null)
                return;
                
            _reward.unload();
            _m_rewardList.Remove(_reward);
            _m_passedRewards.Remove(_reward);
        }
        private void _clearAllEnemyUnits()
        {
            foreach (TreasureHuntEnemyUnit enemyUnit in _m_enemyList)
            {
                enemyUnit.unload();
            }
            _m_enemyList.Clear();
        }
        private void _checkRewardCollection()
        {
            Vector3 playerPosition = _m_playerUnit.position;
            
            foreach (TreasureHuntRewardUnit rewardUnit in _m_rewardList)
            {
                if (rewardUnit.isCollected || _m_passedRewards.Contains(rewardUnit))
                    continue;
                
                // Check if player has passed the reward (player Z position is greater than reward Z position)
                if (playerPosition.z > rewardUnit.position.z)
                {
                    _m_passedRewards.Add(rewardUnit);
                    if (_m_gainRewardNum < _m_totalRewardNum)
                    {
                        _m_gainRewardNum++;
                        GGUIWndTreasureHuntGamePlay.instance.refreshRewardGain(_m_gainRewardNum, _m_totalRewardNum);
                    }
                    _m_playerUnit.showGetRewardEffect();
                }
            }
        }
        private void _updateRewardTipDistance()
        {
            if (_m_rewardList.Count == 0)
            {
                // No rewards available, hide tip
                GGUIWndTreasureHuntGamePlay.instance.refreshRewardTipDistance(-1f);
                return;
            }

            Vector3 playerPosition = _m_playerUnit.position;
            float closestDistance = float.MaxValue;
            
            foreach (TreasureHuntRewardUnit rewardUnit in _m_rewardList)
            {
                if (rewardUnit.isCollected)
                    continue;
                
                // Only consider rewards in front of the player (higher Z value)
                if (rewardUnit.position.z <= playerPosition.z)
                    continue;
                    
                float distance = Vector3.Distance(playerPosition, rewardUnit.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                }
            }
            
            // If no valid rewards found, hide tip
            if (closestDistance == float.MaxValue)
            {
                GGUIWndTreasureHuntGamePlay.instance.refreshRewardTipDistance(-1f);
                return;
            }
            
            // Update UI with closest reward distance (convert back to original scale)
            float worldScale = MainAdditionTreasureHuntGameTDScene.instance.getWorldScale();
            GGUIWndTreasureHuntGamePlay.instance.refreshRewardTipDistance(closestDistance / worldScale);
        }
        private void _clearAllRewardUnits()
        {
            foreach (TreasureHuntRewardUnit rewardUnit in _m_rewardList)
            {
                rewardUnit.unload();
            }
            _m_rewardList.Clear();
            _m_passedRewards.Clear();
        }
        private void _setTotalRewardNum(int _rewardNum)
        {
            _m_totalRewardNum = _rewardNum;
            _m_gainRewardNum = 0;
            GGUIWndTreasureHuntGamePlay.instance.refreshRewardGain(_m_gainRewardNum, _rewardNum);
        }
        private void _onRewardDistanceLineDestroyByItself(TreasureHuntRewardDistanceLineUnit _distanceLine)
        {
            if (_distanceLine == null)
                return;

            _distanceLine.unload();
            _m_rewardDistanceLineList.Remove(_distanceLine);
        }
        private void _updateRewardDistanceLines()
        {
            Vector3 playerPosition = _m_playerUnit.position;

            foreach (TreasureHuntRewardDistanceLineUnit distanceLineUnit in _m_rewardDistanceLineList)
            {
                // Update distance line with current player position (world scale is cached in unit)
                distanceLineUnit.updateDistanceFromPlayerPosition(playerPosition);
            }
        }
        private void _clearAllRewardDistanceLineUnits()
        {
            foreach (TreasureHuntRewardDistanceLineUnit distanceLineUnit in _m_rewardDistanceLineList)
            {
                distanceLineUnit.unload();
            }
            _m_rewardDistanceLineList.Clear();
        }
    }
}
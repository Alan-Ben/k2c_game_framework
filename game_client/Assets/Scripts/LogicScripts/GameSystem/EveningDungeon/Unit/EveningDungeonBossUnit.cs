using System;
using System.Collections.Generic;
using ALPackage;
using Common.DungeonObj;
using JetBrains.Annotations;

namespace GOE.EveningDungeon
{
    /// <summary>
    /// 晚间活动boss单位
    /// </summary>
    public class EveningDungeonBossUnit : _AGameUnit
    {
        [NotNull] private EveningDungeonGameLogic _m_gameLogic;
        private GGUISubWndEveningDungeonBoss _m_bossWnd;

        private GS2GC.p024_DungeonOp.GS2GC_024_012_RetEveningDungeonBossInfo _m_retBossInfoMsg;
        private EEveningDungeonBossState _m_BossState = EEveningDungeonBossState.NONE;//boss状态

        [NotNull] private List<int> _m_lPauseUpdateBossInfoTaskSerializeList = new List<int>();
        private ALCommonEnableTaskController _m_UpdateBossInfoTask;//更新boss信息任务

        private int _m_iInReviveCountDownPauseUpdateBossInfoTaskSerialize;//在复活倒计时中暂停更新boss血量任务的序列号
        private ALCommonEnableTaskController _m_ReviveCountDownTask;//复活倒计时任务
        
        public EveningDungeonBossUnit([NotNull] EveningDungeonGameLogic _gameLogic, GGUISubWndEveningDungeonBoss _bossWnd) : base(_gameLogic)
        {
            _m_gameLogic = _gameLogic;
            _m_bossWnd = _bossWnd;
        }

        public EveningDungeon_BossInfo bossInfo { get { return _m_retBossInfoMsg?.getBossInfo(); } }
        public EEveningDungeonBossState bossState { get { return _m_BossState; } }//boss所处状态

        public event Action<EEveningDungeonBossState, EEveningDungeonBossState> onBossStateChange;
        
        public override void init()
        {
            _m_bossWnd?.showWnd();
            _m_bossWnd?.setGameController(_m_gameLogic.gameController);

            _m_BossState = EEveningDungeonBossState.NONE;
            _m_bossWnd?.setBossStateShow(bossState);

            float updateBossInfoTimeInterval = _m_gameLogic.uiConfig?.updateBossInfoTimeInterval ?? 0f;
            if (updateBossInfoTimeInterval <= 0)
                updateBossInfoTimeInterval = 0.1f;
            
            _m_lPauseUpdateBossInfoTaskSerializeList.Clear();
            // 创建更新boss信息任务
            _m_UpdateBossInfoTask = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_updateBossInfoTaskDeal, updateBossInfoTimeInterval);
        }

        public override void discard()
        {
            _discardReviveCountDownTask();
            _discardUpdateBossInfoTask();

            _m_BossState = EEveningDungeonBossState.NONE;
            _m_bossWnd?.setBossStateShow(bossState);
            _m_bossWnd?.hideWnd();

            _m_retBossInfoMsg = null;
        }

        /// <summary>
        /// 请求boss信息
        /// </summary>
        /// <param name="_reqDone">请求完成回调</param>
        public void reqUpdateBossInfo(Action _reqDone)
        {
            NPPlayer.instance.eveningDungeonComp.reqEveningDungeonBossInfo((_msg) =>
            {
                if(_msg == null)
                {
                    _reqDone?.Invoke();
                    return;
                }
                
                EveningDungeon_BossInfo preBossInfo = bossInfo;
                _m_retBossInfoMsg = _msg;

                // 若boss信息发生变化, 打印协议
                if (preBossInfo == null || bossInfo == null ||
                    preBossInfo.getDeductedHp() != bossInfo.getDeductedHp() ||
                    preBossInfo.getTotalHp() != bossInfo.getTotalHp()
                    || preBossInfo.getWave() != bossInfo.getWave() ||
                    preBossInfo.getDefeatCid() != bossInfo.getDefeatCid() ||
                    preBossInfo.getBeDefeatTimeMs() != bossInfo.getBeDefeatTimeMs())
                {
                    if(Game.instance.mainCamera.gameSetting.printProtocol)
                    {
                        if (_m_retBossInfoMsg.GetFullPackBufSize() <= Game.instance.mainCamera.gameSetting.protocolPrintMinSize)
                        {
                            GCommon.NetRecv(string.Format("<color=red>S -> C: {0} ; </color> {1}", _m_retBossInfoMsg.GetType().Name, GCommon.GetInfoPropertys(_m_retBossInfoMsg)));
                        }
                        else
                        {
                            GCommon.NetWaring($"协议{_m_retBossInfoMsg.GetType().Name}过大，大小：{_m_retBossInfoMsg.GetFullPackBufSize() / 1024}kb。不打印，注意检查");
                        }
                    }
                }
                
                _reqDone?.Invoke();
            });
        }

        /// <summary>
        /// 暂停更新boss信息任务
        /// </summary>
        public int pauseUpdateBossInfoTask()
        {
            int serialize = ALSerializeOpMgr.next();
            _m_lPauseUpdateBossInfoTaskSerializeList.Add(serialize);
            return serialize;
        }
        
        /// <summary>
        /// 恢复更新boss信息任务
        /// </summary>
        public void resumeUpdateBossInfoTask(int _serialize)
        {
            _m_lPauseUpdateBossInfoTaskSerializeList.Remove(_serialize);
        }

        /// <summary>
        /// 强制恢复更新boss信息任务
        /// </summary>
        public void forceResumeUpdateBossInfoTask()
        {
            _m_lPauseUpdateBossInfoTaskSerializeList.Clear();
        }

        /// <summary>
        /// 销毁更新boss信息任务
        /// </summary>
        private void _discardUpdateBossInfoTask()
        {
            _m_UpdateBossInfoTask.setDisable();
        }

        /// <summary>
        /// 进行boss被攻击表现
        /// </summary>
        /// <param name="_deductedHp">已扣除的所有血量</param>
        /// <param name="_totalHp">总血量</param>
        /// <param name="_attackLostHp">攻击失去的血量</param>
        /// <param name="_showDone"></param>
        public void showBossByAttack(long _deductedHp, long _totalHp, long _attackLostHp,  Action _showDone)
        {
            if (_setBossState(EEveningDungeonBossState.BY_ATTACK))
            {
                if (_m_bossWnd != null)
                {
                    _m_bossWnd.showLossBloodTip(_attackLostHp);//掉血tip
                    _m_bossWnd.setBossHp(_deductedHp, _totalHp);//设置boss血量
                    
                    _m_bossWnd.showBossByAttack(_showDone);
                }
                else
                {
                    _showDone?.Invoke();
                }
            }
            else
            {
                _showDone?.Invoke();
            }
        }

        /// <summary>
        /// 进行boss死亡表现
        /// </summary>
        public void showBossDead(long _totalHp, Action _showDone)
        {
            if (_setBossState(EEveningDungeonBossState.DEAD))
            {
                //进行boss死亡表现
                if (_m_bossWnd != null)
                {
                    _m_bossWnd.setBossHp(_totalHp, _totalHp);
                    _m_bossWnd.showBossDead(_showDone);
                }
                else
                {
                    _showDone?.Invoke();
                }
            }
            else
            {
                _showDone?.Invoke();
            }
        }
        
        private bool _setBossState(EEveningDungeonBossState _bossState)
        {
            if (_m_BossState == _bossState)
                return false;

            EEveningDungeonBossState preBossState = _m_BossState;
            _m_BossState = _bossState;
            onBossStateChange?.Invoke(preBossState, _m_BossState);
            return true;
        }

        private bool _m_bHasShowError;
        /// <summary>
        /// 更新boss信息任务
        /// </summary>
        private void _updateBossInfoTaskDeal()
        {
            if (_m_lPauseUpdateBossInfoTaskSerializeList.Count > 0)
            {
                // // 计算超时时最后加1000, 给1s报错的容错
                // if (bossInfo != null && bossInfo.getDeductedHp() >= bossInfo.getTotalHp() && !_m_bHasShowError && 
                //     (bossInfo.getBeDefeatTimeMs() + GRefdataCoreMgr.instance.npGeneral.evening_dungeon_boss_respawn_sec * 1000 + _m_gameLogic.uiConfig?.updateBossInfoTimeInterval * 1000 ?? 0f + 1000) < FpsAndPingMgr.instance.serverTimeTag)
                // {
                //     _m_bHasShowError = true;
                //     Debug.LogError($"boss已经死亡超过复活时间了, 但更新boss信息任务被暂停了, 导致状态没有及时刷新. bossInfo: deductedHp={bossInfo.getDeductedHp()} totalHp={bossInfo.getTotalHp()} beDefeatTimeMs={bossInfo.getBeDefeatTimeMs()} serverTimeTag={FpsAndPingMgr.instance.serverTimeTag} " +
                //                    $" wave={bossInfo.getWave()}");
                // }
                return;
            }

            // // 计算超时时最后加1000, 给1s报错的容错
            // if (bossInfo != null && bossInfo.getDeductedHp() >= bossInfo.getTotalHp() && !_m_bHasShowError && 
            //     (bossInfo.getBeDefeatTimeMs() + GRefdataCoreMgr.instance.npGeneral.evening_dungeon_boss_respawn_sec * 1000 + _m_gameLogic.uiConfig?.updateBossInfoTimeInterval * 1000 ?? 0f + 1000) < FpsAndPingMgr.instance.serverTimeTag)
            // {
            //     _m_bHasShowError = true;
            //     Debug.LogError($"boss已经死亡超过复活时间了, 但boss信息并没有及时更新, 考虑为服务端给的数据错误或由于网络原因导致boss数据更新延迟. bossInfo: deductedHp={bossInfo.getDeductedHp()} totalHp={bossInfo.getTotalHp()} beDefeatTimeMs={bossInfo.getBeDefeatTimeMs()} serverTimeTag={FpsAndPingMgr.instance.serverTimeTag} " +
            //                    $" wave={bossInfo.getWave()}");
            // }
            // else
            // {
            //     _m_bHasShowError = false;
            // }
            
            EveningDungeon_BossInfo preBossInfo = bossInfo;
            reqUpdateBossInfo(() =>
            {
                if(_m_bossWnd == null || bossInfo == null)
                    return;
                
                if (bossInfo.getDeductedHp() >= bossInfo.getTotalHp())//若boss死亡
                {
                    Action afterShowBossDead = () =>
                    {
                        if(bossInfo == null)
                            return;
                        
                        // 若boss复活次数已达到上限, 且 boss当前状态不为完全死亡状态, 进行状态变化
                        if (bossInfo.getWave() >= GRefdataCoreMgr.instance.npGeneral.evening_dungeon_boss_respawn_times_limit 
                            && _setBossState(EEveningDungeonBossState.BOSS_COMPLETELY_DEAD))
                        {
                            _m_bossWnd?.setBossReviveCount(bossInfo.getWave());
                            _m_bossWnd?.setBossHp(bossInfo.getDeductedHp(), bossInfo.getTotalHp());
                            _m_bossWnd?.setFinalAttackPlayer(bossInfo.getDefeatCid());
                            _m_bossWnd?.setBossStateShow(bossState);
                            
                            _discardUpdateBossInfoTask();//销毁更新boss信息任务
                        }
                        // 若boss复活次数未达到上限, 进行等待复活状态处理
                        else if(bossInfo.getWave() < GRefdataCoreMgr.instance.npGeneral.evening_dungeon_boss_respawn_times_limit)
                        {
                            // 若已在等待复活状态, 但boss被再次击杀(新的beDefeatTimeMs), 需要更新显示并重新启动倒计时
                            if (_m_BossState == EEveningDungeonBossState.WAITING_REVIVE)
                            {
                                // 已在等待复活状态, 但boss被再次击杀(新的beDefeatTimeMs), 需要更新显示并重新启动倒计时
                                _m_bossWnd?.setBossReviveCount(bossInfo.getWave());
                                _m_bossWnd?.setBossHp(bossInfo.getDeductedHp(), bossInfo.getTotalHp());
                                _m_bossWnd?.setFinalAttackPlayer(bossInfo.getDefeatCid());
                                _m_bossWnd?.setBossStateShow(bossState);
                                
                                // 重新初始化复活倒计时任务
                                _initReviveCountDownTask();
                            }
                            // 若未在等待复活状态, 进入等待复活状态
                            else if (_setBossState(EEveningDungeonBossState.WAITING_REVIVE))
                            {
                                // 首次进入等待复活状态
                                _m_bossWnd?.setBossReviveCount(bossInfo.getWave());
                                _m_bossWnd?.setBossHp(bossInfo.getDeductedHp(), bossInfo.getTotalHp());
                                _m_bossWnd?.setFinalAttackPlayer(bossInfo.getDefeatCid());
                                _m_bossWnd?.setBossStateShow(bossState);
                                
                                // 初始化复活倒计时任务
                                _initReviveCountDownTask();
                            }
                        }
                        // 其他情况都不需要进行状态变化和显示数据更新
                    };
                    
                    if (preBossInfo != null && preBossInfo.getDeductedHp() < preBossInfo.getTotalHp())//若更新前boss不处于死亡状态
                    {
                        int serialize = pauseUpdateBossInfoTask();//先暂停更新boss信息任务
                        //进行boss死亡表现
                        showBossDead(bossInfo.getTotalHp(), () =>
                        {
                            resumeUpdateBossInfoTask(serialize);//表现完了, 恢复更新boss信息任务

                            // 进行复活倒计时表现
                            afterShowBossDead();
                        });
                    }
                    else
                    {
                        afterShowBossDead();
                    }
                }
                // boss未死亡
                else
                {
                    // 更新血量数据, 这边属于被动信息更新(不是玩家主动操作导致的状态变化), 就算血量有变化也不需要进行表现
                    if(preBossInfo == null || preBossInfo.getDeductedHp() != bossInfo.getDeductedHp() || preBossInfo.getTotalHp() != bossInfo.getTotalHp())
                        _m_bossWnd?.setBossHp(bossInfo.getDeductedHp(), bossInfo.getTotalHp());
                    
                    // 更新波次数据
                    if(preBossInfo == null || preBossInfo.getWave() != bossInfo.getWave())
                        _m_bossWnd?.setBossReviveCount(bossInfo.getWave());

                    // 更新boss状态
                    if (_setBossState(EEveningDungeonBossState.IDLE))
                    {
                        _m_bossWnd?.setBossStateShow(bossState);
                    }
                }
            });
        }

        #region 复活倒计时任务

        /// <summary>
        /// 销毁复活倒计时任务
        /// </summary>
        private void _discardReviveCountDownTask()
        {
            // 先恢复之前的暂停序列号(如果有), 避免重复调用_initReviveCountDownTask时旧序列号泄漏在暂停列表中
            resumeUpdateBossInfoTask(_m_iInReviveCountDownPauseUpdateBossInfoTaskSerialize);
            
            _m_ReviveCountDownTask.setDisable();
        }
        
        /// <summary>
        /// 初始化复活倒计时任务
        /// </summary>
        private void _initReviveCountDownTask()
        {
            _discardReviveCountDownTask();
            
            // 在boss死亡复活倒计时内, 暂停更新boss信息任务
            _m_iInReviveCountDownPauseUpdateBossInfoTaskSerialize = pauseUpdateBossInfoTask();

            // 创建复活倒计时任务
            _m_ReviveCountDownTask = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_reviveCountDownTaskDeal, 1f);
        }
        
        /// <summary>
        /// 复活倒计时任务
        /// </summary>
        private void _reviveCountDownTaskDeal()
        {
            if (bossInfo == null)
            {
                _discardReviveCountDownTask();//销毁复活倒计时任务
                return;
            }
            
            long bossReviveTimeMs = bossInfo.getBeDefeatTimeMs() + GRefdataCoreMgr.instance.npGeneral.evening_dungeon_boss_respawn_sec * 1000;
            long nowTimeMs = FpsAndPingMgr.instance.serverTimeTag;
            
            long reviveCountDown = bossReviveTimeMs - nowTimeMs;
            // 刷新复活倒计时
            _m_bossWnd?.setReviveCountDown(reviveCountDown);

            // 若倒计时结束
            if (reviveCountDown <= 0)
            {
                _discardReviveCountDownTask();//销毁复活倒计时任务
                
                // 立即主动拉取一次boss信息, 不等待下一个轮询周期, 确保状态及时刷新
                _updateBossInfoTaskDeal();
            }
        }

        #endregion
    }
}
using System;
using ALPackage;
using Common.DungeonObj;
using GS2GC.p024_DungeonOp;
using JetBrains.Annotations;

namespace GOE.EveningDungeon
{
    /// <summary>
    /// 攻击状态
    /// </summary>
    public class EveningDungeonGameAttackingState : _AEveningDungeonGameState
    {
        [NotNull] private GS2GC_024_011_RetEveningDungeonAttack _m_attackInfo;//攻击数据
        private long _m_lAttackHeroId;
        private int _m_iPauseUpdateBossInfoTaskSerialize;//
        
        public EveningDungeonGameAttackingState([NotNull] GS2GC_024_011_RetEveningDungeonAttack _attackInfo, long _attackHeroId, EveningDungeonGameLogic _gameLogic) : base(_gameLogic)
        {
            _m_attackInfo = _attackInfo;
            _m_lAttackHeroId = _attackHeroId;
        }

        public override EEveningDungeonGameState state { get { return EEveningDungeonGameState.ATTACKING; } }
        public override bool canEnterState(EEveningDungeonGameState _newState)
        {
            return _newState is EEveningDungeonGameState.STOP 
                or EEveningDungeonGameState.IDLE
                or EEveningDungeonGameState.WAITING_BOSS_REVIVE
                or EEveningDungeonGameState.BOSS_COMPLETELY_DEAD;
        }

        protected override void _onEnterSub()
        {
            // 进入攻击状态后, 先暂停更新boss信息
            _m_iPauseUpdateBossInfoTaskSerialize = _m_gameLogic.bossUnit?.pauseUpdateBossInfoTask() ?? 0;

            long serialize = enterSerialize;
            EveningDungeon_BossInfo bossInfo = null;

            ALProcess process = ALProcess.CreateProcess();
            process
                .addDelegateProcess((_complect) =>
                {
                    if (serialize != enterSerialize)
                        return;

                    // 因为在请求攻击协议时暂停更新boss信息, 所以这里手动请求更新一下boss信息, 获取到最新的boss信息
                    if (_m_gameLogic.bossUnit != null)
                        _m_gameLogic.bossUnit.reqUpdateBossInfo(_complect);
                    else
                        _complect?.Invoke();
                })
                .addDelegateProcess((_complete) =>
                {
                    if (serialize != enterSerialize)
                        return;

                    if (_m_attackInfo.getIsDefeat())
                    {
                        HeroVoiceMgr.instance.playVoice(_m_lAttackHeroId, EHeroVoiceType.KILL_BLOW);//若是最后一击, 播放击杀语音
                    }
                    else
                    {
                        HeroVoiceMgr.instance.playVoice(_m_lAttackHeroId, EHeroVoiceType.FIGHT);//播放普通攻击语音
                    }
                    
                    //飞船开始进行攻击表现
                    _m_gameLogic.uiScene.showAirshipAttack(() =>
                    {
                        // 攻击完成后, 飞船离开
                        _m_gameLogic.uiScene.showAirshipDeparture(null);
                    });
                    // 延迟一段时间后, 刷新当前选中大臣
                    ALCommonTaskController.CommonActionAddMonoTask(() =>
                    {
                        if(serialize != enterSerialize)
                            return;
                        
                        _m_gameLogic.uiScene.refreshCurSelectHero(null);
                    }, _m_gameLogic.uiConfig?.afterAttackChgHeroDelayS ?? 0f);
                    
                    bossInfo = _m_gameLogic.bossUnit?.bossInfo;
                    // 进行boss攻击表现
                    _m_gameLogic.bossUnit?.showBossByAttack(bossInfo?.getDeductedHp() ?? 0, bossInfo?.getTotalHp() ?? 0,
                        _m_attackInfo.getHarmHp(), _complete);
                    
                    // 展示获取的奖励
                    GCommon.showGainRewardTip(_m_attackInfo.getAttackRewardList());

                    // // 延迟指定时间后显示攻击结果弹窗
                    // if (_m_gameLogic.uiConfig != null)
                    // {
                    //     ALCommonTaskController.CommonActionAddMonoTask(() =>
                    //     {
                    //         _complete?.Invoke();
                    //     }, _m_gameLogic.uiConfig.selfAttackShowResultWndDelayTimeS);
                    // }
                    // else
                    // {
                    //     _complete?.Invoke();
                    // }
                })
                .addDelegateProcess((_complete) =>
                {
                    if (serialize != enterSerialize)
                        return;

                    // 判断是否玩家击杀
                    if (_m_attackInfo.getIsDefeat())
                    {
                        Action afterShowBossDead = () =>
                        {
                            long bossRespawnMs = _m_gameLogic.bossUnit != null && _m_gameLogic.bossUnit.bossInfo != null ? 
                                _m_gameLogic.bossUnit.bossInfo.getBeDefeatTimeMs() + GRefdataCoreMgr.instance.npGeneral.evening_dungeon_boss_respawn_sec * 1000 - FpsAndPingMgr.instance.serverTimeTag
                                : -1;
                            // 显示击杀结算弹窗
                            QueueMgr.instance.AddNode(new BaseOnAddContainerSceneUIWndQueueNode(EUIQueueStageType.MAIN,
                                UINodeTagConst.C_EVENING_DUNGEON_KILLED_RESULT, true
                                , false, false, null, GGUIWndEveningDungeonFinalAttackResult.instance, true, false,
                                () =>
                                {
                                    GGUIWndEveningDungeonFinalAttackResult.instance.setShowInfo(_m_attackInfo.getDefeatRewardList(), bossRespawnMs / 1000f);
                                }, null, null, _complete));
                        };

                        _m_gameLogic.uiScene.stopAutoAttack();//完成最后一击时, 停止自动攻击
                        if (_m_gameLogic.bossUnit != null)
                        {
                            // 进行boss死亡表现
                            _m_gameLogic.bossUnit.showBossDead(bossInfo?.getTotalHp() ?? 0, afterShowBossDead);
                        }
                        else
                        {
                            afterShowBossDead();
                        }
                    }
                    else
                    {
                        _complete?.Invoke();
                    }
                })
                // .addDelegateProcess((_complete) =>
                // {
                //     if (serialize != enterSerialize)
                //         return;
                //
                //     // 显示攻击结算弹窗
                //     QueueMgr.instance.AddNode(new BaseOnAddContainerSceneUIWndQueueNode(EUIQueueStageType.MAIN,
                //         UINodeTagConst.C_EVENING_DUNGEON_ATTACK_RESULT, true,
                //         false, false, null, GGUIWndEveningDungeonAttackResult.instance, true, false,
                //         () =>
                //         {
                //             GGUIWndEveningDungeonAttackResult.instance.setData(_m_attackInfo.getAttackRewardList(), _m_attackInfo.getHarmHp(), 
                //                 _m_gameLogic.uiScene.isAutoAttack && _m_gameLogic.uiConfig != null ? _m_gameLogic.uiConfig.onAutoAttackResultWndShowTimeS : -1f);
                //         }, null, null, _complete));
                // })
                .addProcess(() =>
                {
                    // 恢复boss信息更新
                    _m_gameLogic.bossUnit?.resumeUpdateBossInfoTask(_m_iPauseUpdateBossInfoTaskSerialize);
                });
            
            process.deal();
        }

        protected override void _onExitSub()
        {
            // 退出攻击状态后, 开启更新boss信息
            _m_gameLogic.bossUnit?.resumeUpdateBossInfoTask(_m_iPauseUpdateBossInfoTaskSerialize);
        }
    }
}
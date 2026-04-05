using System;
using ALPackage;
using GS2GC.p024_DungeonOp;
using JetBrains.Annotations;

namespace GOE.EveningDungeon
{
    public class EveningDungeonGameLogic : _AGameLogic
    {
        private static EveningDungeonGameLogic _g_instance;
        public static EveningDungeonGameLogic instance { get { return _g_instance ??= new EveningDungeonGameLogic(); } }
        
        
        [NotNull] private EveningDungeonGameController _m_gameController;//游戏控制器
        [NotNull] private EveningDungeonGameOtherPlayerAttackShowMgr _m_otherPlayerAttackShowMgr;//其他玩家攻击表现管理器

        // 对应的 3D 和 UI 表现管理对象
        [NotNull] private readonly GMainGUIAddSceneEveningDungeonGame _m_uiScene;
        private EveningDungeonGameUIConfig _m_uiConfig;
        
        // 游戏状态机
        [NotNull] private readonly _TALSimpleStateMachine<EEveningDungeonGameState> _m_stateMachine;
        
        private EveningDungeonBossUnit _m_bossUnit;// 晚间活动boss单位

        public EveningDungeonGameLogic()
        {
            _m_gameController = new EveningDungeonGameController(this);
            _m_otherPlayerAttackShowMgr = new EveningDungeonGameOtherPlayerAttackShowMgr(this);
            
            _m_uiScene = GMainGUIAddSceneEveningDungeonGame.instance;
            
            // 初始化状态机
            _m_stateMachine = new _TALSimpleStateMachine<EEveningDungeonGameState>();
            _m_stateMachine.changeState(new EveningDungeonGameStopState(this));
        }
        
        [NotNull] public GMainGUIAddSceneEveningDungeonGame uiScene { get { return _m_uiScene; } }
        public EveningDungeonGameUIConfig uiConfig { get { return _m_uiConfig; } }
        public EveningDungeonGameController gameController { get { return _m_gameController; } }
        public EEveningDungeonGameState gameState { get { return _m_stateMachine.curState.state; } }
        public EveningDungeonBossUnit bossUnit { get { return _m_bossUnit; } }

        protected override void _startGameOp(Action _complete, Action _failed)
        {
            if (!_m_uiScene.isEntered)
            {
                Debug.LogError("[**EveningDungeonGameLogic**] GameLogic 启动失败，因为 TDScene 或者 UIScene 没有初始化完成");
                _failed?.Invoke();
                return;
            }

            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(1);
            stepCounter.regAllDoneDelegate(()=>
            {
                _complete?.Invoke();
            });

            _m_uiConfig = _m_uiScene.gameUIConfig;
            _m_uiScene.setGameController(_m_gameController);

            _m_otherPlayerAttackShowMgr.init();
            
            _m_bossUnit = new EveningDungeonBossUnit(this, _m_uiScene.getBossWnd);
            _m_bossUnit.onBossStateChange += _onBossStateChg;
            addGameUnit(_m_bossUnit);
            
            stepCounter.addDoneStepCount();
        }

        protected override void _onStart()
        {
            _m_stateMachine.onStateChg += _onGameStateChg;
            _m_stateMachine.changeState(new EveningDungeonGameStopState(this));
        }

        protected override void _onStop()
        {
            _m_otherPlayerAttackShowMgr.discrad();
            
            _m_stateMachine.changeState(new EveningDungeonGameStopState(this));//进入停止状态
            _m_stateMachine.onStateChg -= _onGameStateChg;

            if (_m_bossUnit != null)
            {
                _m_bossUnit.onBossStateChange -= _onBossStateChg;
                removeGameUnit(_m_bossUnit);
                _m_bossUnit = null;
            }
        }

        protected override void _onTick(float _deltaTime)
        {
        }

        /// <summary>
        /// 发起攻击
        /// </summary>
        public void attack()
        {
            reqAttack((_succ,_msg, _attackHeroId) =>
            {
                if (!_succ || _msg == null)
                    return;
                
                // 进入攻击状态
                enterAttackState(_msg, _attackHeroId);
            });
        }

        /// <summary>
        /// 请求攻击
        /// </summary>
        public void reqAttack(Action<bool, GS2GC_024_011_RetEveningDungeonAttack, long> _onReqDone)
        {
            EveningDungeonHeroFightInfo selectedHero = _m_uiScene.curSelectHeroInfo;
            if (selectedHero == null)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.eveningDungeon_noHeroCanFightTip_none);
                _m_uiScene.stopAutoAttack();
                
                _onReqDone?.Invoke(false, null, 0);
                return;
            }

            if (selectedHero.nowFightCount >= selectedHero.fightMaxCount)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.eveningDungeon_heroFightCountUseUpTip_none);
                _m_uiScene.stopAutoAttack();
                
                _onReqDone?.Invoke(false, null, 0);
                return;
            }
            
            // 只有处于idle状态才能发起攻击
            if (gameState != EEveningDungeonGameState.IDLE || _m_bossUnit == null)
            {
                _onReqDone?.Invoke(false, null, 0);
                return;
            }
            
            int pauseUpdateBossInfoTaskSerialize = _m_bossUnit.pauseUpdateBossInfoTask();// 请求攻击时, 先暂停更新boss信息
            long attackHeroId = selectedHero.heroId;
            int serialize = MainCameraMono.selfInstance.openAllInputMask();//开启屏蔽所有输入遮罩
            
            NPPlayer.instance.eveningDungeonComp.reqEveningDungeonAttack(attackHeroId, (_succ,_msg) =>
            {
                MainCameraMono.selfInstance.closeAllInputMask(serialize);//关闭屏蔽所有输入遮罩
                _m_bossUnit?.resumeUpdateBossInfoTask(pauseUpdateBossInfoTaskSerialize);//恢复更新boss信息 

                if (!_succ || _msg == null)
                {
                    _m_uiScene.stopAutoAttack();
                }
                
                _onReqDone?.Invoke(_succ, _msg, attackHeroId);
            });
        }

        /// <summary>
        /// 进入攻击状态
        /// </summary>
        /// <param name="_msg"></param>
        public void enterAttackState([NotNull] GS2GC_024_011_RetEveningDungeonAttack _msg, long _attackHeroId)
        {
            // 进入攻击状态
            _m_stateMachine.changeState(new EveningDungeonGameAttackingState(_msg, _attackHeroId, this));
        }
        
        /// <summary>
        /// 当boss状态发生改变
        /// </summary>
        /// <param name="_preBossState"></param>
        /// <param name="_nowBossState"></param>
        private void _onBossStateChg(EEveningDungeonBossState _preBossState, EEveningDungeonBossState _nowBossState)
        {
            switch (_nowBossState)
            {
                case EEveningDungeonBossState.IDLE:
                    _m_stateMachine.changeState(new EveningDungeonGameIdleState(this));
                    break;
                
                case EEveningDungeonBossState.WAITING_REVIVE:
                    _m_stateMachine.changeState(new EveningDungeonGameWaitingBossReviveState(this));
                    break;
                
                case EEveningDungeonBossState.BOSS_COMPLETELY_DEAD:
                    _m_stateMachine.changeState(new EveningDungeonGameBossCompletelyDeadState(this));
                    break;
            }
        }
        
        private void _onGameStateChg(EEveningDungeonGameState _lastState, EEveningDungeonGameState _nextState)
        {
            _m_uiScene.refreshStateShow(_nextState);   
        }
    }
}
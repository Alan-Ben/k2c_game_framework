using ALPackage;
using GS2GC.p024_DungeonOp;

namespace GOE.EveningDungeon
{
    public class EveningDungeonGameIdleState : _AEveningDungeonGameState
    {
        public EveningDungeonGameIdleState(EveningDungeonGameLogic _gameLogic) : base(_gameLogic)
        {
        }

        public override EEveningDungeonGameState state { get { return EEveningDungeonGameState.IDLE; } }
        public override bool canEnterState(EEveningDungeonGameState _newState)
        {
            return _newState is EEveningDungeonGameState.STOP 
                or EEveningDungeonGameState.ATTACKING
                or EEveningDungeonGameState.WAITING_BOSS_REVIVE
                or EEveningDungeonGameState.BOSS_COMPLETELY_DEAD;
        }

        protected override void _onEnterSub()
        {
            ALProcess process = ALProcess.CreateProcess();
            process
                .addDelegateProcess((onProcessComplete) =>
                {
                    // 进入idle时, 刷新一下当前选中大臣
                    _m_gameLogic.uiScene.refreshCurSelectHero(onProcessComplete);
                })
                .addProcess(() =>
                {
                    // 若果当前是自动攻击, 请求进行攻击
                    if (_m_gameLogic.uiScene.isAutoAttack)
                    {
                        _m_gameLogic.reqAttack((_isSucc, _msg, _attackHeroId) =>
                        {
                            if(!_isSucc || _msg == null)
                                return;
                    
                            _m_gameLogic.enterAttackState(_msg, _attackHeroId);
                        });
                    }
                })
                .deal();
            
            // GS2GC_024_011_RetEveningDungeonAttack attackMsg = null;
            // long attackHeroId = 0;
            //
            // // 用于自动攻击时, 在切换大臣完成 和 请求攻击完成后, 进入攻击状态
            // ALStepCounter step = new ALStepCounter();
            // step.chgTotalStepCount(2);
            // step.regAllDoneDelegate(() =>
            // {
            //     if(attackMsg != null)
            //         _m_gameLogic.enterAttackState(attackMsg, attackHeroId);
            // });
            //
            // // 进入idle时, 刷新一下当前选中大臣
            // _m_gameLogic.uiScene.refreshCurSelectHero(step.addDoneStepCount);
            //
            // // 若果当前是自动攻击, 请求进行攻击
            // if (_m_gameLogic.uiScene.isAutoAttack)
            // {
            //     _m_gameLogic.reqAttack((_isSucc, _msg, _attackHeroId) =>
            //     {
            //         if(!_isSucc || _msg == null)
            //             return;
            //         
            //         attackMsg = _msg;
            //         attackHeroId = _attackHeroId;
            //         step.addDoneStepCount();
            //     });
            // }
        }

        protected override void _onExitSub()
        {
        }
    }
}
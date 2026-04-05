using System;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public partial class ChapterForwardLogicMgr
    {
        // 关卡状态机积累
        private abstract class _AChapterBaseState : _ATALStateBase<EChapterForwardType>
        {
            [NotNull]protected ChapterForwardLogicMgr _m_forwardLogicMgr;
            
            protected _AChapterBaseState([NotNull]ChapterForwardLogicMgr _chapterForwardLogicMgr) : base()
            {
                _m_forwardLogicMgr = _chapterForwardLogicMgr;
            }
            
            protected void _changeState(EChapterForwardType _newState)
            {
                if(null == _m_forwardLogicMgr._m_stateMachine)
                    return;
                
                switch (_newState)
                {
                    case EChapterForwardType.NONE:
                        _m_forwardLogicMgr._m_stateMachine.changeState<NoneState>();
                        break;
                    case EChapterForwardType.IDLE:
                        _m_forwardLogicMgr._m_stateMachine.changeState<IdleState>();
                        break;
                    case EChapterForwardType.ONCE_FORWARD:
                        _m_forwardLogicMgr._m_stateMachine.changeState<OnceForwardState>();
                        break;
                    case EChapterForwardType.FIGHT_BOSS:
                        _m_forwardLogicMgr._m_stateMachine.changeState<FightBossState>();
                        break;
                    case EChapterForwardType.FIGHT_BOSS_PER:
                        _m_forwardLogicMgr._m_stateMachine.changeState<FightBossPerState>();
                        break;
                    case EChapterForwardType.FIGHT_BOSS_WAIT:
                        _m_forwardLogicMgr._m_stateMachine.changeState<FightBossWaitState>();
                        break;
                    case EChapterForwardType.AUTO_BATTLE:
                        _m_forwardLogicMgr._m_stateMachine.changeState<AutoBattleState>();
                        break;
                    case EChapterForwardType.DEAL_EVENT:
                        _m_forwardLogicMgr._m_stateMachine.changeState<DealEventState>();
                        break;
                    case EChapterForwardType.DIALOG:
                        _m_forwardLogicMgr._m_stateMachine.changeState<DialogState>();
                        break;
                    case EChapterForwardType.CHAPTER_COMPLETE:
                        _m_forwardLogicMgr._m_stateMachine.changeState<ChapterCompleteState>();
                        break;
                    case EChapterForwardType.NODE_MOVE_BOSS:
                        _m_forwardLogicMgr._m_stateMachine.changeState<NodeMoveBossState>();
                        break;
                    case EChapterForwardType.NODE_MOVE_NORMAL:
                        _m_forwardLogicMgr._m_stateMachine.changeState<NodeMoveNormalState>();
                        break;
                }
            }
        }
    }
}
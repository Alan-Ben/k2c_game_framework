using System;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public enum EChapterForwardType
    {
        NONE,
        IDLE,//默认状态
        ONCE_FORWARD,//一次前进
        FIGHT_BOSS,//打boss
        FIGHT_BOSS_PER,//boss战前摇
        FIGHT_BOSS_WAIT,//boss战等待连线
        DEAL_EVENT,//处理事件
        DIALOG,//处理对话
        NODE_MOVE_BOSS,//节点移动到boss的表现
        NODE_MOVE_NORMAL,//节点移动到节点的表现
        CHAPTER_COMPLETE,//章节完成表现
        AUTO_BATTLE,//自动状态
    }
    
    public partial class ChapterForwardLogicMgr
    {
        /// <summary>
        /// 状态机
        /// </summary>
        private class ChapterStateMachine : _TALStateMachine<_AChapterBaseState, EChapterForwardType>
        {
            public ChapterStateMachine(ChapterStateFactory _factory) : base(_factory)
            {
                
            }
        } 
        
        /// <summary>
        /// 状态机工厂
        /// </summary>
        private class ChapterStateFactory :_ATALStateFactory<_AChapterBaseState, EChapterForwardType>
        {
            public ChapterStateFactory([NotNull]ChapterForwardLogicMgr _chapterForwardLogicMgr) : base()
            {
                regCacheController(typeof(NoneState), new StateCache(() => { return new NoneState(_chapterForwardLogicMgr); }));
                regCacheController(typeof(IdleState), new StateCache(() => { return new IdleState(_chapterForwardLogicMgr); }));
                regCacheController(typeof(FightBossState), new StateCache(() => { return new FightBossState(_chapterForwardLogicMgr); }));
                regCacheController(typeof(FightBossPerState), new StateCache(() => { return new FightBossPerState(_chapterForwardLogicMgr); }));
                regCacheController(typeof(OnceForwardState), new StateCache(() => { return new OnceForwardState(_chapterForwardLogicMgr); }));
                regCacheController(typeof(FightBossWaitState), new StateCache(() => { return new FightBossWaitState(_chapterForwardLogicMgr); }));
                regCacheController(typeof(AutoBattleState), new StateCache(() => { return new AutoBattleState(_chapterForwardLogicMgr); }));
                regCacheController(typeof(DealEventState), new StateCache(() => { return new DealEventState(_chapterForwardLogicMgr); }));
                regCacheController(typeof(DialogState), new StateCache(() => { return new DialogState(_chapterForwardLogicMgr); }));
                regCacheController(typeof(ChapterCompleteState), new StateCache(() => { return new ChapterCompleteState(_chapterForwardLogicMgr); }));
                regCacheController(typeof(NodeMoveBossState), new StateCache(() => { return new NodeMoveBossState(_chapterForwardLogicMgr); }));
                regCacheController(typeof(NodeMoveNormalState), new StateCache(() => { return new NodeMoveNormalState(_chapterForwardLogicMgr); }));

            }
            

            private class StateCache : _TALBasicStateCacheController<_AChapterBaseState, EChapterForwardType>
            {
                public StateCache(Func<_AChapterBaseState> _createFunc) : base(_createFunc, 1, 2)
                {

                }
            }
        } 
    }
}
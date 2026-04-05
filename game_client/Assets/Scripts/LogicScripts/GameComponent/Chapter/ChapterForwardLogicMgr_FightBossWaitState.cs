using System;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public partial class ChapterForwardLogicMgr
    {
        // boss战等待连线状态
        private class FightBossWaitState : _AChapterBaseState
        {
            private long _m_serialId = 0;
            //经历时间
            private float _m_dealTime = 0;
            
            public FightBossWaitState(ChapterForwardLogicMgr _chapterForwardLogicMgr) : base(_chapterForwardLogicMgr){}
            public override EChapterForwardType state { get { return EChapterForwardType.FIGHT_BOSS_WAIT; } }

            protected override void _onEnter()
            {
                _m_serialId++;
                long serialId = _m_serialId;

                //切换boss战状态
                _m_forwardLogicMgr._dealPlayForwardVideoAniState(EChapterVideoForwardState.BOSS);
                
                //写死第一关直接打掉，方便引导
                long oldChapterId = NPPlayer.instance.chapterComp.curChapterId;
                if (oldChapterId == 1)
                {
                    NPPlayer.instance.chapterComp.reqChapterFightBoss(oldChapterId, (_msg) =>
                    {
                        //进入章节完成状态
                        _m_forwardLogicMgr._m_stateMachine?.changeState<ChapterCompleteState>(
                            (_state) =>
                            {

                            });

                    }, (_errCode) =>
                    {
                        //进入章节完成状态
                        _m_forwardLogicMgr._m_stateMachine?.changeState<ChapterCompleteState>(
                            (_state) =>
                            {

                            });
                    });
                    return;
                }
                
                //执行boss战前表现状态
                _m_forwardLogicMgr._dealChapterBossWaitEffect();

                // //如果托管状态并且，没有表现接口直接刀到下一个状态
                // if (_m_forwardLogicMgr.isInAutoForward && _m_forwardLogicMgr.effectInterface == null)
                // {
                //     _changeState(EChapterForwardType.AUTO_CALC_BOSS);
                // }
            }

            protected override void _onExit()
            {
                _m_serialId++;
                _m_dealTime = 0;
            }
            
            protected override void _onTick(float _deltaTime)
            {
                _m_dealTime += _deltaTime;
                // //如果在自动前进的话, 会自动前进去打boss状态
                // if (_m_forwardLogicMgr.isInAutoForward && _m_dealTime > 0.3f)
                // {
                //     _changeState(EChapterForwardType.FIGHT_BOSS);
                // }
            }

            public override bool canEnterState(_ATALStateBase<EChapterForwardType> _newState)
            {
                return _newState.state is EChapterForwardType.FIGHT_BOSS 
                       || _newState.state is EChapterForwardType.DEAL_EVENT
                       || _newState.state is EChapterForwardType.DIALOG
                       || _newState.state is EChapterForwardType.AUTO_BATTLE
                       || _newState.state is EChapterForwardType.CHAPTER_COMPLETE;
            }

            public override void resetData()
            {
                
            }
        }
    }
}
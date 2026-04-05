using System;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public partial class ChapterForwardLogicMgr
    {
        // boss战前摇状态
        private class FightBossPerState : _AChapterBaseState
        {
            private long _m_serialId = 0;
            //经历时间
            private float _m_dealTime = 0;
            
            public FightBossPerState(ChapterForwardLogicMgr _chapterForwardLogicMgr) : base(_chapterForwardLogicMgr){}
            public override EChapterForwardType state { get { return EChapterForwardType.FIGHT_BOSS_PER; } }

            protected override void _onEnter()
            {
                _m_serialId++;
                long serialId = _m_serialId;
                
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
                else
                {
                    //切换boss战状态
                    _m_forwardLogicMgr._dealPlayForwardVideoAniState(EChapterVideoForwardState.BOSS);
                    //执行boss战前摇表现
                    _m_forwardLogicMgr._dealChapterBossPerEffect(() =>
                    {
                        if(serialId != _m_serialId)
                            return;
                        _changeState(EChapterForwardType.FIGHT_BOSS_WAIT);
                    });   
                }
            }

            protected override void _onExit()
            {
                _m_serialId++;
                _m_dealTime = 0;
            }
            
            protected override void _onTick(float _deltaTime)
            {
                //累计持续时间
                _m_dealTime += _deltaTime;
                // //大于间隔时间去单次前进状态
                // if (_m_dealTime > GRefdataCoreMgr.instance.npGeneral.chapter_boss_per_wait_delay_time_s)
                // {
                //     _changeState(EChapterForwardType.FIGHT_BOSS_WAIT);
                // }
            }

            public override bool canEnterState(_ATALStateBase<EChapterForwardType> _newState)
            {
                return _newState.state is EChapterForwardType.FIGHT_BOSS_WAIT 
                       || _newState.state is EChapterForwardType.CHAPTER_COMPLETE;
            }

            public override void resetData()
            {
                _m_dealTime = 0;

            }
        }
    }
}
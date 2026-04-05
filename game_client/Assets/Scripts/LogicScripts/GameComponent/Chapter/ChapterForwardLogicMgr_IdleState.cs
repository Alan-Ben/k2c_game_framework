using System;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public partial class ChapterForwardLogicMgr
    {
        // Idle的状态
        private class IdleState : _AChapterBaseState
        {
            //经历时间
            private float _m_dealTime = 0;
            
            public IdleState(ChapterForwardLogicMgr _chapterForwardLogicMgr) : base(_chapterForwardLogicMgr){}

            public override EChapterForwardType state { get { return EChapterForwardType.IDLE; } }

            protected override void _onEnter()
            {
                //如果是boss战了要去boss战等待状态
                if (NPPlayer.instance.chapterComp.curIsBossPoint())
                {
                    _changeState(EChapterForwardType.FIGHT_BOSS_WAIT);
                    return;
                }
                
                //执行到默认状态
                _m_forwardLogicMgr._dealChgIdleEffect();
                
                //如果是快速前进状态 或者 自动前进状态
                if (_m_forwardLogicMgr.isInQuickForward || _m_forwardLogicMgr._m_isInContinueClickForward)
                {
                    _m_forwardLogicMgr._dealPlayForwardVideoAniState(EChapterVideoForwardState.QUICK_FORWARD);
                }
                else
                {
                    _m_forwardLogicMgr._dealPlayForwardVideoAniState(EChapterVideoForwardState.NORMAL);
                }
            }

            protected override void _onExit()
            {
                _m_dealTime = 0;
            }

            protected override void _onTick(float _deltaTime)
            {
                //累计持续时间
                _m_dealTime += _deltaTime;
                
                //通关不允许前进
                if (NPPlayer.instance.chapterComp.chapterRefObj == null)
                {
                    return;
                }
                
                //如果是快速前进状态 或者 自动前进状态
                if (_m_forwardLogicMgr.isInQuickForward)
                {
                    //大于间隔时间去单次前进状态
                    if (_m_dealTime > GRefdataCoreMgr.instance.npGeneral.chapter_idle_forward_wait_delay_time_s)
                    {
                        _changeState(EChapterForwardType.ONCE_FORWARD);
                    }
                }
                
                //如果连续点击前进，状态改成快速前进状态
                if (_m_forwardLogicMgr.isInQuickForward || _m_forwardLogicMgr._m_isInContinueClickForward)
                {
                    _m_forwardLogicMgr._dealPlayForwardVideoAniState(EChapterVideoForwardState.QUICK_FORWARD);
                }
                else
                {
                    _m_forwardLogicMgr._dealPlayForwardVideoAniState(EChapterVideoForwardState.NORMAL);
                }
            }

            public override bool canEnterState(_ATALStateBase<EChapterForwardType> _newState)
            {
                return  _newState.state is EChapterForwardType.ONCE_FORWARD 
                       || _newState.state is EChapterForwardType.FIGHT_BOSS_WAIT
                       || _newState.state is EChapterForwardType.DEAL_EVENT
                       || _newState.state is EChapterForwardType.AUTO_BATTLE
                       || _newState.state is EChapterForwardType.DIALOG;
            }

            public override void resetData()
            {
                _m_dealTime = 0;
            }
        }
    }
}
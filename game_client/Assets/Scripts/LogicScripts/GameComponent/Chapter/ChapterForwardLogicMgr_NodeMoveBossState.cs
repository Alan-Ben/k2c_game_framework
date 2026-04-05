using System;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public partial class ChapterForwardLogicMgr
    {
        // 节点切换状态
        private class NodeMoveBossState : _AChapterBaseState
        {
            private long _m_serialId = 0;
            //经历时间
            private float _m_dealTime = 0;
            
            public NodeMoveBossState(ChapterForwardLogicMgr _chapterForwardLogicMgr) : base(_chapterForwardLogicMgr){}
            public override EChapterForwardType state { get { return EChapterForwardType.NODE_MOVE_BOSS; } }

         
            protected override void _onEnter()
            {
                _m_serialId++;
                long serialId = _m_serialId;
                
                _m_forwardLogicMgr._dealChapterNodeMoveBossEffect();
                //切换boss战状态
                _m_forwardLogicMgr._dealPlayForwardVideoAniState(EChapterVideoForwardState.BOSS);
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
                
                if (_m_dealTime > GRefdataCoreMgr.instance.npGeneral.chapter_node_move_effect_delay_time_s)
                {
                    _changeState(EChapterForwardType.IDLE);
                }
            }

            public override bool canEnterState(_ATALStateBase<EChapterForwardType> _newState)
            {
                return _newState.state is EChapterForwardType.IDLE 
                       || _newState.state is EChapterForwardType.FIGHT_BOSS_PER 
                       || _newState.state is EChapterForwardType.FIGHT_BOSS_WAIT
                    ;
            }

            public override void resetData()
            {
                _m_dealTime = 0;
            }
        }
    }
}
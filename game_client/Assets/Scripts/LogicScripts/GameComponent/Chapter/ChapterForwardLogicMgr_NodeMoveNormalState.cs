using System;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public partial class ChapterForwardLogicMgr
    {
        // 节点切换状态
        private class NodeMoveNormalState : _AChapterBaseState
        {
            private long _m_serialId = 0;
            //经历时间
            private float _m_dealTime = 0;
            private int _m_nodeIndex = 0;
            
            public NodeMoveNormalState(ChapterForwardLogicMgr _chapterForwardLogicMgr) : base(_chapterForwardLogicMgr){}
            public override EChapterForwardType state { get { return EChapterForwardType.NODE_MOVE_NORMAL; } }
            
            public void setNodeIndex(int _nodeIndex)
            {
                _m_nodeIndex = _nodeIndex;
            }
            
            protected override void _onEnter()
            {
                _m_serialId++;
                long serialId = _m_serialId;
                
                _m_forwardLogicMgr._dealChapterNodeMoveEffect();
                //切换默认状态
                _m_forwardLogicMgr._dealPlayForwardVideoAniState(EChapterVideoForwardState.NORMAL);
            }

            protected override void _onExit()
            {
                _m_serialId++;
                _m_dealTime = 0;
                _m_nodeIndex = 0;
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
                return _newState.state is EChapterForwardType.IDLE;
            }

            public override void resetData()
            {
                _m_dealTime = 0;
                _m_nodeIndex = 0;
            }
        }
    }
}
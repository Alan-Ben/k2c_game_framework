using System;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public partial class ChapterForwardLogicMgr
    {
        // boss战等待连线状态
        private class DealEventState : _AChapterBaseState
        {
            private long _m_serialId = 0;
            //经历时间
            private float _m_dealTime = 0;
            
            private long _m_eventId = 0;
            private Action _m_endCallback;//结束回调，如果没有就执行回idle状态
            
            public DealEventState(ChapterForwardLogicMgr _chapterForwardLogicMgr) : base(_chapterForwardLogicMgr){}
            public override EChapterForwardType state { get { return EChapterForwardType.DEAL_EVENT; } }

            public void setEventId(long _eventId)
            {
                _m_eventId = _eventId;
            }
            
            public void setEndCallback(Action _endCallback)
            {
                _m_endCallback = _endCallback;
            }
            
            protected override void _onEnter()
            {
                _m_serialId++;
                long serialId = _m_serialId;

                //如果托管状态并且，没有表现接口直接刀到下一个状态
                if (_m_eventId > 0)
                {
                    _m_forwardLogicMgr.showEventTipEffect();
                    
                    //处理事件
                    GCommon.dealChapterEvent(_m_eventId, () =>
                    {
                        if(serialId != _m_serialId)
                            return;
                        _enterNewState();
                    });
                }
                else
                {
                    _enterNewState();
                }
                _m_forwardLogicMgr._dealPlayForwardVideoAniState(EChapterVideoForwardState.NORMAL);
            }

            protected override void _onExit()
            {
                _m_serialId++;
                _m_dealTime = 0;

                _m_eventId = 0;
                _m_endCallback = null;
            }
            
            protected override void _onTick(float _deltaTime)
            {

            }

            public override bool canEnterState(_ATALStateBase<EChapterForwardType> _newState)
            {
                return _newState.state is EChapterForwardType.IDLE
                       || _newState.state is EChapterForwardType.DIALOG
                       || _newState.state is EChapterForwardType.NODE_MOVE_NORMAL;
            }

            public override void resetData()
            {
                _m_eventId = 0;
                _m_endCallback = null;
            }

            //对话结束有回调直接执行回调
            private void _enterNewState()
            {
                if (_m_endCallback != null)
                    _m_endCallback();
                else
                {
                    _changeState(EChapterForwardType.IDLE);
                }
            }
        }
    }
}
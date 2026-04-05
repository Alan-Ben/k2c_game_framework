using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public class AnecdotePosView : _AALBasicLoadObj
    {
        [NotNull] private readonly AnecdotePosInfo _m_posInfo;
        private _AAnecdoteEventView _m_eventView;
        
        
        public AnecdotePosView([NotNull] AnecdotePosInfo _posInfo)
        {
            _m_posInfo = _posInfo;
        }
        
        
        public AnecdotePosInfo posInfo { get { return _m_posInfo; } }
        
        
        protected void _init()
        {
            // 先执行一次，防止初始化的过程中事件发生了变化
            _onEventListChange();
            _m_posInfo.onEventListChange += _onEventListChange;
            
            _setLoadDone();
        }
        protected override void _discard()
        {
            _m_posInfo.onEventListChange -= _onEventListChange;
            _m_eventView?.discard();
        }
        protected override void _loadOp()
        {
            _AAnecdoteEventInfo eventInfo = _m_posInfo.getNextEnableEvent();
            if (eventInfo == null)
            {
                _init();
                return;
            }

            _m_eventView = _AAnecdoteEventView.createEventView(eventInfo);
            _m_eventView?.load(_init);
        }
        

        internal void _simulateClick()
        {
            _m_eventView?._simulateClick();
        }


        internal void _onEventListChange()
        {
            _AAnecdoteEventInfo eventInfo = _m_posInfo.getNextEnableEvent();
            if (eventInfo == null)
            {
                _m_eventView?.discard();
                _m_eventView = null;
                return;
            }
            
            if (_m_eventView != null && _m_eventView.eventInfo == eventInfo)
                return;
            
            _m_eventView?.discard();
            _m_eventView = _AAnecdoteEventView.createEventView(eventInfo);
            _m_eventView?.load();
        }
    }
}
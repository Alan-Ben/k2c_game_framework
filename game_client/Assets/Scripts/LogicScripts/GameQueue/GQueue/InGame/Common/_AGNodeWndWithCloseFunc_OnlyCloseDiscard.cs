using System;
using ALPackage;

namespace GOE
{
    public abstract class _AGNodeWndWithCloseFunc_OnlyCloseDiscard<T> : _AGAddNode_SingleWnd_OnlyCloseDiscard
        where T : _AALBasicLoadUIWndBasicClass
    {
        private readonly Action _m_onNodeClose;
        private T _m_wnd;


        public _AGNodeWndWithCloseFunc_OnlyCloseDiscard(T _wnd, Action _onNodeClose)
            : base(_wnd)
        {
            _m_wnd = _wnd;
            _m_onNodeClose = _onNodeClose;
        }

        public _AGNodeWndWithCloseFunc_OnlyCloseDiscard(T _wnd, Action _onNodeClose, string _nodeTag)
            : base(_wnd, _nodeTag)
        {
            _m_wnd = _wnd;
            _m_onNodeClose = _onNodeClose;
        }

        public _AGNodeWndWithCloseFunc_OnlyCloseDiscard(T _wnd, Action _onNodeClose, string _nodeTag, bool _needClickBkNoRemove)
            : base(_wnd, _nodeTag, _needClickBkNoRemove)
        {
            _m_wnd = _wnd;
            _m_onNodeClose = _onNodeClose;
        }

        public _AGNodeWndWithCloseFunc_OnlyCloseDiscard(T _wnd, Action _onNodeClose, string _nodeTag, bool _needClickBkNoRemove, Action _onClickBkNoRemoveDealAction)
            : base(_wnd, _nodeTag, _needClickBkNoRemove, _onClickBkNoRemoveDealAction)
        {
            _m_wnd = _wnd;
            _m_onNodeClose = _onNodeClose;
        }

        public _AGNodeWndWithCloseFunc_OnlyCloseDiscard(T _wnd, Action _onNodeClose, string _nodeTag, bool _needClickBkNoRemove, bool _needAutoRemove)
            : base(_wnd, _nodeTag, _needClickBkNoRemove, _needAutoRemove)
        {
            _m_wnd = _wnd;
            _m_onNodeClose = _onNodeClose;
        }

        public _AGNodeWndWithCloseFunc_OnlyCloseDiscard(T _wnd, Action _onNodeClose, EUIQueueStageType _stageType, string _nodeTag)
            : base(_wnd, _stageType, _nodeTag)
        {
            _m_wnd = _wnd;
            _m_onNodeClose = _onNodeClose;
        }

        public _AGNodeWndWithCloseFunc_OnlyCloseDiscard(T _wnd, Action _onNodeClose, EUIQueueStageType _stageType, string _nodeTag, bool _needAutoRemove)
            : base(_wnd, _stageType, _nodeTag, _needAutoRemove)
        {
            _m_wnd = _wnd;
            _m_onNodeClose = _onNodeClose;
        }

        public _AGNodeWndWithCloseFunc_OnlyCloseDiscard(T _wnd, Action _onNodeClose, EUIQueueStageType _stageType, string _nodeTag, bool _needAutoRemove, bool _needTransBk)
            : base(_wnd, _stageType, _nodeTag, _needAutoRemove, _needTransBk)
        {
            _m_wnd = _wnd;
            _m_onNodeClose = _onNodeClose;
        }

        public _AGNodeWndWithCloseFunc_OnlyCloseDiscard(T _wnd, Action _onNodeClose, EUIQueueStageType _stageType, string _nodeTag, bool _needAutoRemove, bool _needTransBk, bool _needRemovePreAutoRemove)
            : base(_wnd, _stageType, _nodeTag, _needAutoRemove, _needTransBk, _needRemovePreAutoRemove)
        {
            _m_wnd = _wnd;
            _m_onNodeClose = _onNodeClose;
        }


        public T wnd { get { return _m_wnd; } }


        public override void onClose()
        {
            base.onClose();
            _m_onNodeClose?.Invoke();

            _onCloseEx();
        }


        protected sealed override void _onWndLoadedDone()
        {
            wnd?.showWnd();
        }

        protected virtual void _onCloseEx() { }
    }
}

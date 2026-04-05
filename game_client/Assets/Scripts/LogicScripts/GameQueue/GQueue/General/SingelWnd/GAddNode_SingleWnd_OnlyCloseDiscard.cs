using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 单个窗口显示的node，都带背景蒙版
    /// </summary>
    public class GAddNode_SingleWnd_OnlyCloseDiscard : _AGAddNode_SingleWnd_OnlyCloseDiscard
    {
        //对应回调处理函数
        private Action _m_dOnWndLoadDone;

        public GAddNode_SingleWnd_OnlyCloseDiscard(_AALBasicLoadUIWndBasicClass _wndObj, Action _onWndLoadDone)
            : base(_wndObj)
        {
            _m_dOnWndLoadDone = _onWndLoadDone;
        }
        public GAddNode_SingleWnd_OnlyCloseDiscard(_AALBasicLoadUIWndBasicClass _wndObj, Action _onWndLoadDone, string _nodeTag)
            : base(_wndObj, _nodeTag)
        {
            _m_dOnWndLoadDone = _onWndLoadDone;
        }
        public GAddNode_SingleWnd_OnlyCloseDiscard(_AALBasicLoadUIWndBasicClass _wndObj, Action _onWndLoadDone, string _nodeTag, bool _needClickBkNoRemove)
            : base(_wndObj, _nodeTag, _needClickBkNoRemove)
        {
            _m_dOnWndLoadDone = _onWndLoadDone;
        }
        public GAddNode_SingleWnd_OnlyCloseDiscard(_AALBasicLoadUIWndBasicClass _wndObj, Action _onWndLoadDone, string _nodeTag, bool _needClickBkNoRemove, Action _onClickBkNoRemoveDealAction) 
            : base(_wndObj, _nodeTag, _needClickBkNoRemove, _onClickBkNoRemoveDealAction)
        {
            _m_dOnWndLoadDone = _onWndLoadDone;
        }
        public GAddNode_SingleWnd_OnlyCloseDiscard(_AALBasicLoadUIWndBasicClass _wndObj, Action _onWndLoadDone, string _nodeTag, bool _needClickBkNoRemove, bool _needAutoRemove)
           : base(_wndObj, _nodeTag, _needClickBkNoRemove,_needAutoRemove)
        {
            _m_dOnWndLoadDone = _onWndLoadDone;
        }

        public GAddNode_SingleWnd_OnlyCloseDiscard(_AALBasicLoadUIWndBasicClass _wndObj, Action _onWndLoadDone, EUIQueueStageType _stageType, string _nodeTag)
            : base(_wndObj, _stageType, _nodeTag)
        {
            _m_dOnWndLoadDone = _onWndLoadDone;
        }
        public GAddNode_SingleWnd_OnlyCloseDiscard(_AALBasicLoadUIWndBasicClass _wndObj, Action _onWndLoadDone, EUIQueueStageType _stageType, string _nodeTag, bool _needAutoRemove)
            : base(_wndObj, _stageType, _nodeTag, _needAutoRemove)
        {
            _m_dOnWndLoadDone = _onWndLoadDone;
        }
        public GAddNode_SingleWnd_OnlyCloseDiscard(_AALBasicLoadUIWndBasicClass _wndObj, Action _onWndLoadDone, EUIQueueStageType _stageType, string _nodeTag, bool _needAutoRemove, bool _needTransBk)
            : base(_wndObj, _stageType, _nodeTag, _needAutoRemove, _needTransBk)
        {
            _m_dOnWndLoadDone = _onWndLoadDone;
        }
        public GAddNode_SingleWnd_OnlyCloseDiscard(_AALBasicLoadUIWndBasicClass _wndObj, Action _onWndLoadDone, EUIQueueStageType _stageType, string _nodeTag, bool _needAutoRemove, bool _needTransBk, bool _needRemovePreAutoRemove)
            : base(_wndObj, _stageType, _nodeTag, _needAutoRemove, _needTransBk, _needRemovePreAutoRemove)
        {
            _m_dOnWndLoadDone = _onWndLoadDone;
        }
        /// <summary>
        /// 窗口加载完成后的处理函数
        /// </summary>
        protected override void _onWndLoadedDone()
        {
            if (null != _m_dOnWndLoadDone)
                _m_dOnWndLoadDone();
        }
    }
}

using System;
using ALPackage;

namespace GOE
{
    public class GNodeCommonWndWithCloseFunc_OnlyCloseDiscar : _AGNodeWndWithCloseFunc_OnlyCloseDiscard<_AALBasicLoadUIWndBasicClass>
    {
        public GNodeCommonWndWithCloseFunc_OnlyCloseDiscar(_AALBasicLoadUIWndBasicClass _wnd, Action _onNodeClose)
            : base(_wnd, _onNodeClose)
        {
        }

        public GNodeCommonWndWithCloseFunc_OnlyCloseDiscar(_AALBasicLoadUIWndBasicClass _wnd, Action _onNodeClose, string _nodeTag)
            : base(_wnd, _onNodeClose, _nodeTag)
        {
        }

        public GNodeCommonWndWithCloseFunc_OnlyCloseDiscar(_AALBasicLoadUIWndBasicClass _wnd, Action _onNodeClose, string _nodeTag, bool _needClickBkNoRemove)
            : base(_wnd, _onNodeClose, _nodeTag, _needClickBkNoRemove)
        {
        }

        public GNodeCommonWndWithCloseFunc_OnlyCloseDiscar(_AALBasicLoadUIWndBasicClass _wnd, Action _onNodeClose, string _nodeTag, bool _needClickBkNoRemove, Action _onClickBkNoRemoveDealAction)
            : base(_wnd, _onNodeClose, _nodeTag, _needClickBkNoRemove, _onClickBkNoRemoveDealAction)
        {
        }

        public GNodeCommonWndWithCloseFunc_OnlyCloseDiscar(_AALBasicLoadUIWndBasicClass _wnd, Action _onNodeClose, string _nodeTag, bool _needClickBkNoRemove, bool _needAutoRemove)
            : base(_wnd, _onNodeClose, _nodeTag, _needClickBkNoRemove, _needAutoRemove)
        {
        }

        public GNodeCommonWndWithCloseFunc_OnlyCloseDiscar(_AALBasicLoadUIWndBasicClass _wnd, Action _onNodeClose, EUIQueueStageType _stageType, string _nodeTag)
            : base(_wnd, _onNodeClose, _stageType, _nodeTag)
        {
        }

        public GNodeCommonWndWithCloseFunc_OnlyCloseDiscar(_AALBasicLoadUIWndBasicClass _wnd, Action _onNodeClose, EUIQueueStageType _stageType, string _nodeTag, bool _needAutoRemove)
            : base(_wnd, _onNodeClose, _stageType, _nodeTag, _needAutoRemove)
        {
        }

        public GNodeCommonWndWithCloseFunc_OnlyCloseDiscar(_AALBasicLoadUIWndBasicClass _wnd, Action _onNodeClose, EUIQueueStageType _stageType, string _nodeTag, bool _needAutoRemove, bool _needTransBk)
            : base(_wnd, _onNodeClose, _stageType, _nodeTag, _needAutoRemove, _needTransBk)
        {
        }

        public GNodeCommonWndWithCloseFunc_OnlyCloseDiscar(_AALBasicLoadUIWndBasicClass _wnd, Action _onNodeClose, EUIQueueStageType _stageType, string _nodeTag, bool _needAutoRemove, bool _needTransBk, bool _needRemovePreAutoRemove)
            : base(_wnd, _onNodeClose, _stageType, _nodeTag, _needAutoRemove, _needTransBk, _needRemovePreAutoRemove)
        {
        }


        public override bool NeedAutoRemove { get { return false; } }
        public override bool IsMainViewNode { get { return false; } }
    }
}

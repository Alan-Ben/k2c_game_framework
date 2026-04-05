using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 通用的进入MainUIScene视图的处理节点对象
    /// </summary>
    public class SubUIMainWndNode_Var_Login : _ASubUIMainWndNode_Var
    {
        public SubUIMainWndNode_Var_Login(_AALBasicLoadUIWndBasicClass _uiWnd, _ANPBasicAddContainerUIScene _parentScene)
            : base(_uiWnd, _parentScene)
        {
        }
        public SubUIMainWndNode_Var_Login(_AALBasicLoadUIWndBasicClass _uiWnd, _ANPBasicAddContainerUIScene _parentScene, string _nodeUITag)
            : base(_uiWnd, _parentScene, _nodeUITag)
        {
        }
        public SubUIMainWndNode_Var_Login(_AALBasicLoadUIWndBasicClass _uiWnd, _ANPBasicAddContainerUIScene _parentScene, string _nodeUITag, Action _preEnterDone, Action _onEnterDone)
            : base(_uiWnd, _parentScene, _nodeUITag, _preEnterDone, _onEnterDone)
        {
        }
        public SubUIMainWndNode_Var_Login(_AALBasicLoadUIWndBasicClass _uiWnd, _ANPBasicAddContainerUIScene _parentScene, EUIQueueStageType _stageType, string _nodeUITag, Action _preEnterDone, Action _onEnterDone)
            : base(_uiWnd, _parentScene, _stageType, _nodeUITag, _preEnterDone, _onEnterDone)
        {
        }
        public SubUIMainWndNode_Var_Login(_AALBasicLoadUIWndBasicClass _uiWnd, _ANPBasicAddContainerUIScene _parentScene, EUIQueueStageType _stageType, string _nodeUITag, bool _isOnlyUINode)
            : base(_uiWnd, _parentScene, _stageType, _nodeUITag, _isOnlyUINode)
        {
        }
        public SubUIMainWndNode_Var_Login(_AALBasicLoadUIWndBasicClass _uiWnd, _ANPBasicAddContainerUIScene _parentScene, EUIQueueStageType _stageType, string _nodeUITag, bool _isOnlyUINode, Action _preEnterDone, Action _onEnterDone)
            : base(_uiWnd, _parentScene, _stageType, _nodeUITag, _isOnlyUINode, _preEnterDone, _onEnterDone)
        {
        }
        
    }
}

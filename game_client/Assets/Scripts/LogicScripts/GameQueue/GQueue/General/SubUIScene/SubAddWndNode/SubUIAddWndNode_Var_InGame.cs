using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 通用的进入MainUIScene视图的处理节点对象
    /// </summary>
    public class SubUIAddWndNode_Var_InGame : _ASubUIAddWndNode_Var
    {
        public SubUIAddWndNode_Var_InGame(_AALBasicLoadUIWndBasicClass _addWnd, _ANPBasicAddContainerUIScene _parentScene)
            : base(_addWnd, _parentScene)
        {
        }
        public SubUIAddWndNode_Var_InGame(_AALBasicLoadUIWndBasicClass _addWnd, _ANPBasicAddContainerUIScene _parentScene, string _nodeTag)
            : base(_addWnd, _parentScene, _nodeTag)
        {
        }
        public SubUIAddWndNode_Var_InGame(_AALBasicLoadUIWndBasicClass _addWnd, _ANPBasicAddContainerUIScene _parentScene, string _nodeTag, Action _preEnterDone, Action _onEnterDone)
            : base(_addWnd, _parentScene, _nodeTag, _preEnterDone, _onEnterDone)
        {
        }
        public SubUIAddWndNode_Var_InGame(_AALBasicLoadUIWndBasicClass _addWnd, _ANPBasicAddContainerUIScene _parentScene, EUIQueueStageType _stageType, string _nodeTag, Action _preEnterDone, Action _onEnterDone)
            : base(_addWnd, _parentScene, _stageType, _nodeTag, _preEnterDone, _onEnterDone)
        {
        }
    }
}

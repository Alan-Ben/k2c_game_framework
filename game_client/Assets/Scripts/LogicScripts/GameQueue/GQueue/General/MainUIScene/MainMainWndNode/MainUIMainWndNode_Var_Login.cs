using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 通用的进入MainUIScene视图的处理节点对象
    /// </summary>
    public class MainUIMainWndNode_Var_Login : _AMainUIMainWndNode_Var
    {
        public MainUIMainWndNode_Var_Login(_AALBasicLoadUIWndBasicClass _uiWnd, _AALBasicTopContainerScene _parentScene)
            : base(_uiWnd, _parentScene)
        {
        }
        public MainUIMainWndNode_Var_Login(_AALBasicLoadUIWndBasicClass _uiWnd, _AALBasicTopContainerScene _parentScene, string _nodeUITag)
            : base(_uiWnd, _parentScene, _nodeUITag)
        {
        }
        public MainUIMainWndNode_Var_Login(_AALBasicLoadUIWndBasicClass _uiWnd, _AALBasicTopContainerScene _parentScene, string _nodeUITag, Action _preEnterDone, Action _onEnterDone)
            : base(_uiWnd, _parentScene, _nodeUITag, _preEnterDone, _onEnterDone)
        {
        }
        public MainUIMainWndNode_Var_Login(_AALBasicLoadUIWndBasicClass _uiWnd, _AALBasicTopContainerScene _parentScene, EUIQueueStageType _stageType, string _nodeUITag, Action _preEnterDone, Action _onEnterDone)
            : base(_uiWnd, _parentScene, _stageType, _nodeUITag, _preEnterDone, _onEnterDone)
        {
        }
        public MainUIMainWndNode_Var_Login(_AALBasicLoadUIWndBasicClass _uiWnd, _AALBasicTopContainerScene _parentScene, EUIQueueStageType _stageType, string _nodeUITag, bool _isOnlyUINode)
            : base(_uiWnd, _parentScene, _stageType, _nodeUITag, _isOnlyUINode)
        {
        }
        public MainUIMainWndNode_Var_Login(_AALBasicLoadUIWndBasicClass _uiWnd, _AALBasicTopContainerScene _parentScene, EUIQueueStageType _stageType, string _nodeUITag, bool _isOnlyUINode, Action _preEnterDone, Action _onEnterDone)
            : base(_uiWnd, _parentScene, _stageType, _nodeUITag, _isOnlyUINode, _preEnterDone, _onEnterDone)
        {
        }
        public MainUIMainWndNode_Var_Login(_AALBasicLoadUIWndBasicClass _uiWnd, _AALBasicTopContainerScene _parentScene, EUIQueueStageType _stageType, string _nodeUITag, bool _isOnlyUINode, bool _needAutoRemove, bool _isCanRollBackQuit, Action _preEnterDone, Action _onEnterDone)
            : base(_uiWnd, _parentScene, _stageType, _nodeUITag, _isOnlyUINode, _needAutoRemove, _isCanRollBackQuit, _preEnterDone, _onEnterDone)
        {
        }

        public MainUIMainWndNode_Var_Login(_AALBasicLoadUIWndBasicClass _uiWnd)
            : base(_uiWnd, NPGUISceneLogin.instance)
        {
        }
        public MainUIMainWndNode_Var_Login(_AALBasicLoadUIWndBasicClass _uiWnd, string _nodeUITag)
            : base(_uiWnd, NPGUISceneLogin.instance, _nodeUITag)
        {
        }
        public MainUIMainWndNode_Var_Login(_AALBasicLoadUIWndBasicClass _uiWnd, string _nodeUITag, Action _preEnterDone, Action _onEnterDone)
            : base(_uiWnd, NPGUISceneLogin.instance, _nodeUITag, _preEnterDone, _onEnterDone)
        {
        }
        public MainUIMainWndNode_Var_Login(_AALBasicLoadUIWndBasicClass _uiWnd, EUIQueueStageType _stageType, string _nodeUITag, Action _preEnterDone, Action _onEnterDone)
            : base(_uiWnd, NPGUISceneLogin.instance, _stageType, _nodeUITag, _preEnterDone, _onEnterDone)
        {
        }
        public MainUIMainWndNode_Var_Login(_AALBasicLoadUIWndBasicClass _uiWnd, EUIQueueStageType _stageType, string _nodeUITag, bool _isOnlyUINode)
            : base(_uiWnd, NPGUISceneLogin.instance, _stageType, _nodeUITag, _isOnlyUINode)
        {
        }
        public MainUIMainWndNode_Var_Login(_AALBasicLoadUIWndBasicClass _uiWnd, EUIQueueStageType _stageType, string _nodeUITag, bool _isOnlyUINode, Action _preEnterDone, Action _onEnterDone)
            : base(_uiWnd, NPGUISceneLogin.instance, _stageType, _nodeUITag, _isOnlyUINode, _preEnterDone, _onEnterDone)
        {
        }
    }
}

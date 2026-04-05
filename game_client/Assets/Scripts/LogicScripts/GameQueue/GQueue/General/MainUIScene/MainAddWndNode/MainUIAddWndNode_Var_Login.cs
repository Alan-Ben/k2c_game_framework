using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 通用的进入MainUIScene视图的处理节点对象
    /// </summary>
    public class MainUIAddWndNode_Var_Login : _AMainUIAddWndNode_Var
    {
        public MainUIAddWndNode_Var_Login(_AALBasicLoadUIWndBasicClass _addWnd, _AALBasicTopContainerScene _parentScene)
            : base(_addWnd, _parentScene)
        {
        }
        public MainUIAddWndNode_Var_Login(_AALBasicLoadUIWndBasicClass _addWnd, _AALBasicTopContainerScene _parentScene, string _nodeTag)
            : base(_addWnd, _parentScene, _nodeTag)
        {
        }
        public MainUIAddWndNode_Var_Login(_AALBasicLoadUIWndBasicClass _addWnd, _AALBasicTopContainerScene _parentScene, string _nodeTag, Action _preEnterDone, Action _onEnterDone)
            : base(_addWnd, _parentScene, _nodeTag, _preEnterDone, _onEnterDone)
        {
        }
        public MainUIAddWndNode_Var_Login(_AALBasicLoadUIWndBasicClass _addWnd, _AALBasicTopContainerScene _parentScene, EUIQueueStageType _stageType, string _nodeTag, Action _preEnterDone, Action _onEnterDone, bool _needTransBk)
            : base(_addWnd, _parentScene, _stageType, _nodeTag, _preEnterDone, _onEnterDone, _needTransBk)
        {
        }

        public MainUIAddWndNode_Var_Login(_AALBasicLoadUIWndBasicClass _addWnd)
            : base(_addWnd, NPGUISceneLogin.instance)
        {
        }
        public MainUIAddWndNode_Var_Login(_AALBasicLoadUIWndBasicClass _addWnd, string _nodeTag)
            : base(_addWnd, NPGUISceneLogin.instance, _nodeTag)
        {
        }
        public MainUIAddWndNode_Var_Login(_AALBasicLoadUIWndBasicClass _addWnd, string _nodeTag, Action _preEnterDone, Action _onEnterDone)
            : base(_addWnd, NPGUISceneLogin.instance, _nodeTag, _preEnterDone, _onEnterDone)
        {
        }
        public MainUIAddWndNode_Var_Login(_AALBasicLoadUIWndBasicClass _addWnd, EUIQueueStageType _stageType, string _nodeTag, Action _preEnterDone, Action _onEnterDone, bool _needTransBk)
            : base(_addWnd, NPGUISceneLogin.instance, _stageType, _nodeTag, _preEnterDone, _onEnterDone, _needTransBk)
        {
        }
    }
}

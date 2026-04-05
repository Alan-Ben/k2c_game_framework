using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 通用的进入MainUIScene视图的处理节点对象
    /// </summary>
    public class MainUIAddWndNode_Var_InGame : _AMainUIAddWndNode_Var
    {
        //跳转下一节点是否需要关闭本节点
        private bool _m_needAutoRemove;

        public override bool NeedAutoRemove { get { return _m_needAutoRemove; } }

        public MainUIAddWndNode_Var_InGame(_AALBasicLoadUIWndBasicClass _addWnd, _AALBasicTopContainerScene _parentScene)
            : base(_addWnd, _parentScene)
        {
        }
        public MainUIAddWndNode_Var_InGame(_AALBasicLoadUIWndBasicClass _addWnd, _AALBasicTopContainerScene _parentScene, string _nodeTag)
            : base(_addWnd, _parentScene, _nodeTag)
        {
        }
        public MainUIAddWndNode_Var_InGame(_AALBasicLoadUIWndBasicClass _addWnd, _AALBasicTopContainerScene _parentScene, string _nodeTag, Action _preEnterDone, Action _onEnterDone)
            : base(_addWnd, _parentScene, _nodeTag, _preEnterDone, _onEnterDone)
        {
        }
        public MainUIAddWndNode_Var_InGame(_AALBasicLoadUIWndBasicClass _addWnd, _AALBasicTopContainerScene _parentScene, EUIQueueStageType _stageType, string _nodeTag, Action _preEnterDone, Action _onEnterDone, bool _needTransBk)
            : base(_addWnd, _parentScene, _stageType, _nodeTag, _preEnterDone, _onEnterDone, _needTransBk)
        {
        }

        public MainUIAddWndNode_Var_InGame(_AALBasicLoadUIWndBasicClass _addWnd, _AALBasicTopContainerScene _parentScene, EUIQueueStageType _stageType, string _nodeTag, Action _preEnterDone, Action _onEnterDone, bool _needTransBk,bool _needAutoRemove)
        : base(_addWnd, _parentScene, _stageType, _nodeTag, _preEnterDone, _onEnterDone, _needTransBk)
        {
            _m_needAutoRemove = _needAutoRemove;
        }
        public MainUIAddWndNode_Var_InGame(_AALBasicLoadUIWndBasicClass _addWnd, _AALBasicTopContainerScene _parentScene, EUIQueueStageType _stageType, string _nodeTag, Action _preEnterDone, Action _onEnterDone, bool _needTransBk, bool _needAutoRemove, bool _needClickBkNoRemove)
            : base(_addWnd, _parentScene, _stageType, _nodeTag, _preEnterDone, _onEnterDone, _needTransBk, _needClickBkNoRemove)
        {
            _m_needAutoRemove = _needAutoRemove;
        }

        public MainUIAddWndNode_Var_InGame(_AALBasicLoadUIWndBasicClass _addWnd)
            : base(_addWnd, GUISceneMain.instance)
        {
        }
        public MainUIAddWndNode_Var_InGame(_AALBasicLoadUIWndBasicClass _addWnd, string _nodeTag)
            : base(_addWnd, GUISceneMain.instance, _nodeTag)
        {
        }
        public MainUIAddWndNode_Var_InGame(_AALBasicLoadUIWndBasicClass _addWnd, string _nodeTag, Action _preEnterDone, Action _onEnterDone)
            : base(_addWnd, GUISceneMain.instance, _nodeTag, _preEnterDone, _onEnterDone)
        {
        }
        public MainUIAddWndNode_Var_InGame(_AALBasicLoadUIWndBasicClass _addWnd, EUIQueueStageType _stageType, string _nodeTag, Action _preEnterDone, Action _onEnterDone, bool _needTransBk)
            : base(_addWnd, GUISceneMain.instance, _stageType, _nodeTag, _preEnterDone, _onEnterDone, _needTransBk)
        {
        }

    }
}

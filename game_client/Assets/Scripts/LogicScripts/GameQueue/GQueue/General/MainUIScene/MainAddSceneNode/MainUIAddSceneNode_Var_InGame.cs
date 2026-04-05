using ALPackage;
using System;

namespace GOE
{
    /// <summary>
    /// 通用的进入MainUIScene视图的处理节点对象
    /// </summary>
    public class MainUIAddSceneNode_Var_InGame : _AMainUIAddSceneNode_Var
    {
        public MainUIAddSceneNode_Var_InGame(_ANPBasicAddContainerUIScene _addContainerUIScene, _AALBasicTopContainerScene _parentScene)
            : base(_addContainerUIScene, _parentScene)
        {
        }
        public MainUIAddSceneNode_Var_InGame(_ANPBasicAddContainerUIScene _addContainerUIScene, _AALBasicTopContainerScene _parentScene, string _nodeTag)
            : base(_addContainerUIScene, _parentScene, _nodeTag)
        {
        }
        public MainUIAddSceneNode_Var_InGame(_ANPBasicAddContainerUIScene _addContainerUIScene, _AALBasicTopContainerScene _parentScene, string _nodeTag, Action _preEnterDone, Action _onEnterDone)
            : base(_addContainerUIScene, _parentScene, _nodeTag, _preEnterDone, _onEnterDone)
        {
        }
        public MainUIAddSceneNode_Var_InGame(_ANPBasicAddContainerUIScene _addContainerUIScene, _AALBasicTopContainerScene _parentScene, EUIQueueStageType _stageType, string _nodeTag, Action _preEnterDone, Action _onEnterDone)
            : base(_addContainerUIScene, _parentScene, _stageType, _nodeTag, _preEnterDone, _onEnterDone)
        {
        }

        public MainUIAddSceneNode_Var_InGame(_ANPBasicAddContainerUIScene _addContainerUIScene)
            : base(_addContainerUIScene, GUISceneMain.instance)
        {
        }
        public MainUIAddSceneNode_Var_InGame(_ANPBasicAddContainerUIScene _addContainerUIScene, string _nodeTag)
            : base(_addContainerUIScene, GUISceneMain.instance, _nodeTag)
        {
        }
        public MainUIAddSceneNode_Var_InGame(_ANPBasicAddContainerUIScene _addContainerUIScene, string _nodeTag, Action _preEnterDone, Action _onEnterDone)
            : base(_addContainerUIScene, GUISceneMain.instance, _nodeTag, _preEnterDone, _onEnterDone)
        {
        }
        public MainUIAddSceneNode_Var_InGame(_ANPBasicAddContainerUIScene _addContainerUIScene, EUIQueueStageType _stageType, string _nodeTag, Action _preEnterDone, Action _onEnterDone)
            : base(_addContainerUIScene, GUISceneMain.instance, _stageType, _nodeTag, _preEnterDone, _onEnterDone)
        {
        }
    }
}

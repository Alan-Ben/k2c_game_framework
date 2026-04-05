using ALPackage;
using System;

namespace GOE
{
    /// <summary>
    /// 通用的进入MainUIScene视图的处理节点对象
    /// </summary>
    public class SubUIAddSceneNode_Var_InGame : _ASubUIAddSceneNode_Var
    {
        public SubUIAddSceneNode_Var_InGame(_ANPBasicAddContainerUIScene _addContainerUIScene, _ANPBasicAddContainerUIScene _parentScene)
            : base(_addContainerUIScene, _parentScene)
        {
        }
        public SubUIAddSceneNode_Var_InGame(_ANPBasicAddContainerUIScene _addContainerUIScene, _ANPBasicAddContainerUIScene _parentScene, string _nodeTag)
            : base(_addContainerUIScene, _parentScene, _nodeTag)
        {
        }
        public SubUIAddSceneNode_Var_InGame(_ANPBasicAddContainerUIScene _addContainerUIScene, _ANPBasicAddContainerUIScene _parentScene, string _nodeTag, Action _preEnterDone, Action _onEnterDone)
            : base(_addContainerUIScene, _parentScene, _nodeTag, _preEnterDone, _onEnterDone)
        {
        }
        public SubUIAddSceneNode_Var_InGame(_ANPBasicAddContainerUIScene _addContainerUIScene, _ANPBasicAddContainerUIScene _parentScene, EUIQueueStageType _stageType, string _nodeTag, Action _preEnterDone, Action _onEnterDone)
            : base(_addContainerUIScene, _parentScene, _stageType, _nodeTag,_preEnterDone, _onEnterDone)
        {
        }
    }
}

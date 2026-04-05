using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 通用的进入MainUIScene视图的处理节点对象
    /// </summary>
    public class SubUIMainSceneNode_Var_Login : _ASubUIMainSceneNodeVar
    {
        public SubUIMainSceneNode_Var_Login(_ANPBasicAddContainerUIScene _uiScene, _ANPBasicAddContainerUIScene _parentScene)
            : base(_uiScene, _parentScene)
        {
        }
        public SubUIMainSceneNode_Var_Login(_ANPBasicAddContainerUIScene _uiScene, _ANPBasicAddContainerUIScene _parentScene, string _nodeUITag)
            : base(_uiScene, _parentScene, _nodeUITag)
        {
        }
        public SubUIMainSceneNode_Var_Login(_ANPBasicAddContainerUIScene _uiScene, _ANPBasicAddContainerUIScene _parentScene, string _nodeUITag, Action _preEnterDone, Action _onEnterDone)
            : base(_uiScene, _parentScene, _nodeUITag, _preEnterDone, _onEnterDone)
        {
        }
        public SubUIMainSceneNode_Var_Login(_ANPBasicAddContainerUIScene _uiScene, _ANPBasicAddContainerUIScene _parentScene, EUIQueueStageType _stageType, string _nodeUITag, Action _preEnterDone, Action _onEnterDone)
            : base(_uiScene, _parentScene, _stageType, _nodeUITag, _preEnterDone, _onEnterDone)
        {
        }
        public SubUIMainSceneNode_Var_Login(_ANPBasicAddContainerUIScene _uiScene, _ANPBasicAddContainerUIScene _parentScene, EUIQueueStageType _stageType, string _nodeUITag, bool _isOnlyUINode)
            : base(_uiScene, _parentScene, _stageType, _nodeUITag, _isOnlyUINode)
        {
        }
        public SubUIMainSceneNode_Var_Login(_ANPBasicAddContainerUIScene _uiScene, _ANPBasicAddContainerUIScene _parentScene, EUIQueueStageType _stageType, string _nodeUITag, bool _isOnlyUINode, Action _preEnterDone, Action _onEnterDone)
            : base(_uiScene, _parentScene, _stageType, _nodeUITag, _isOnlyUINode, _preEnterDone, _onEnterDone)
        {
        }
    }
}

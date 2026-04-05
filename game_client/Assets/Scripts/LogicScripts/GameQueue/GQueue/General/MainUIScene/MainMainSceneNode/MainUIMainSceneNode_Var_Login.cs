using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 通用的进入MainUIScene视图的处理节点对象
    /// </summary>
    public class MainUIMainSceneNode_Var_Login : _AMainUIMainSceneNode_Var
    {
        public MainUIMainSceneNode_Var_Login(_ANPBasicAddContainerUIScene _uiScene, _AALBasicTopContainerScene _parentScene)
            : base(_uiScene, _parentScene)
        {
        }
        public MainUIMainSceneNode_Var_Login(_ANPBasicAddContainerUIScene _uiScene, _AALBasicTopContainerScene _parentScene, string _nodeUITag)
            : base(_uiScene, _parentScene, _nodeUITag)
        {
        }
        public MainUIMainSceneNode_Var_Login(_ANPBasicAddContainerUIScene _uiScene, _AALBasicTopContainerScene _parentScene, string _nodeUITag, Action _preEnterDone, Action _onEnterDone)
            : base(_uiScene, _parentScene, _nodeUITag, _preEnterDone, _onEnterDone)
        {
        }
        public MainUIMainSceneNode_Var_Login(_ANPBasicAddContainerUIScene _uiScene, _AALBasicTopContainerScene _parentScene, EUIQueueStageType _stageType, string _nodeUITag, Action _preEnterDone, Action _onEnterDone)
            : base(_uiScene, _parentScene, _stageType, _nodeUITag, _preEnterDone, _onEnterDone)
        {
        }
        public MainUIMainSceneNode_Var_Login(_ANPBasicAddContainerUIScene _uiScene, _AALBasicTopContainerScene _parentScene, EUIQueueStageType _stageType, string _nodeUITag, bool _isOnlyUINode)
            : base(_uiScene, _parentScene, _stageType, _nodeUITag, _isOnlyUINode)
        {
        }
        public MainUIMainSceneNode_Var_Login(_ANPBasicAddContainerUIScene _uiScene, _AALBasicTopContainerScene _parentScene, EUIQueueStageType _stageType, string _nodeUITag, bool _isOnlyUINode, Action _preEnterDone, Action _onEnterDone)
            : base(_uiScene, _parentScene, _stageType, _nodeUITag, _isOnlyUINode, _preEnterDone, _onEnterDone)
        {
        }

        public MainUIMainSceneNode_Var_Login(_ANPBasicAddContainerUIScene _uiScene)
            : base(_uiScene, NPGUISceneLogin.instance)
        {
        }
        public MainUIMainSceneNode_Var_Login(_ANPBasicAddContainerUIScene _uiScene, string _nodeUITag)
            : base(_uiScene, NPGUISceneLogin.instance, _nodeUITag)
        {
        }
        public MainUIMainSceneNode_Var_Login(_ANPBasicAddContainerUIScene _uiScene, string _nodeUITag, Action _preEnterDone, Action _onEnterDone)
            : base(_uiScene, NPGUISceneLogin.instance, _nodeUITag, _preEnterDone, _onEnterDone)
        {
        }
        public MainUIMainSceneNode_Var_Login(_ANPBasicAddContainerUIScene _uiScene, EUIQueueStageType _stageType, string _nodeUITag, Action _preEnterDone, Action _onEnterDone)
            : base(_uiScene, NPGUISceneLogin.instance, _stageType, _nodeUITag, _preEnterDone, _onEnterDone)
        {
        }
        public MainUIMainSceneNode_Var_Login(_ANPBasicAddContainerUIScene _uiScene, EUIQueueStageType _stageType, string _nodeUITag, bool _isOnlyUINode)
            : base(_uiScene, NPGUISceneLogin.instance, _stageType, _nodeUITag, _isOnlyUINode)
        {
        }
        public MainUIMainSceneNode_Var_Login(_ANPBasicAddContainerUIScene _uiScene, EUIQueueStageType _stageType, string _nodeUITag, bool _isOnlyUINode, Action _preEnterDone, Action _onEnterDone)
            : base(_uiScene, NPGUISceneLogin.instance, _stageType, _nodeUITag, _isOnlyUINode, _preEnterDone, _onEnterDone)
        {
        }
    }
}

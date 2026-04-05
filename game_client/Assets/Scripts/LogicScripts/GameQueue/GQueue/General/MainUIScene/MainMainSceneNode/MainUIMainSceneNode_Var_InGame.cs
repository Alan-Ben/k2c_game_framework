using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 通用的进入MainUIScene视图的处理节点对象
    /// </summary>
    public class MainUIMainSceneNode_Var_InGame : _AMainUIMainSceneNode_Var
    {
        //回退按钮资源id，为0代表不需要返回按钮
        private long _m_BackBtnResId;
        //回退按钮
        private NPGGUIWndInstanceCommonBack _m_cbCommonBackObj;
        
        public MainUIMainSceneNode_Var_InGame(_ANPBasicAddContainerUIScene _uiScene, _AALBasicTopContainerScene _parentScene, long _backResPathId)
            : base(_uiScene, _parentScene)
        {
            _m_BackBtnResId = _backResPathId;
        }
        public MainUIMainSceneNode_Var_InGame(_ANPBasicAddContainerUIScene _uiScene, _AALBasicTopContainerScene _parentScene, long _backResPathId, string _nodeUITag)
            : base(_uiScene, _parentScene, _nodeUITag)
        {
            _m_BackBtnResId = _backResPathId;
        }
        public MainUIMainSceneNode_Var_InGame(_ANPBasicAddContainerUIScene _uiScene, _AALBasicTopContainerScene _parentScene, long _backResPathId, string _nodeUITag, Action _preEnterDone, Action _onEnterDone)
            : base(_uiScene, _parentScene, _nodeUITag, _preEnterDone, _onEnterDone)
        {
            _m_BackBtnResId = _backResPathId;
        }
        public MainUIMainSceneNode_Var_InGame(_ANPBasicAddContainerUIScene _uiScene, _AALBasicTopContainerScene _parentScene, long _backResPathId, EUIQueueStageType _stageType, string _nodeUITag, Action _preEnterDone, Action _onEnterDone)
            : base(_uiScene, _parentScene, _stageType, _nodeUITag, _preEnterDone, _onEnterDone)
        {
            _m_BackBtnResId = _backResPathId;
        }
        public MainUIMainSceneNode_Var_InGame(_ANPBasicAddContainerUIScene _uiScene, _AALBasicTopContainerScene _parentScene, long _backResPathId, EUIQueueStageType _stageType, string _nodeUITag, bool _isOnlyUINode)
            : base(_uiScene, _parentScene, _stageType, _nodeUITag, _isOnlyUINode)
        {
            _m_BackBtnResId = _backResPathId;
        }
        public MainUIMainSceneNode_Var_InGame(_ANPBasicAddContainerUIScene _uiScene, _AALBasicTopContainerScene _parentScene, long _backResPathId, EUIQueueStageType _stageType, string _nodeUITag, bool _isOnlyUINode, Action _preEnterDone, Action _onEnterDone)
            : base(_uiScene, _parentScene, _stageType, _nodeUITag, _isOnlyUINode, _preEnterDone, _onEnterDone)
        {
            _m_BackBtnResId = _backResPathId;
        }

        public MainUIMainSceneNode_Var_InGame(_ANPBasicAddContainerUIScene _uiScene, long _backResPathId)
            : base(_uiScene, GUISceneMain.instance)
        {
            _m_BackBtnResId = _backResPathId;
        }
        public MainUIMainSceneNode_Var_InGame(_ANPBasicAddContainerUIScene _uiScene, long _backResPathId, string _nodeUITag)
            : base(_uiScene, GUISceneMain.instance, _nodeUITag)
        {
            _m_BackBtnResId = _backResPathId;
        }
        public MainUIMainSceneNode_Var_InGame(_ANPBasicAddContainerUIScene _uiScene, long _backResPathId, string _nodeUITag, Action _preEnterDone, Action _onEnterDone)
            : base(_uiScene, GUISceneMain.instance, _nodeUITag, _preEnterDone, _onEnterDone)
        {
            _m_BackBtnResId = _backResPathId;
        }
        public MainUIMainSceneNode_Var_InGame(_ANPBasicAddContainerUIScene _uiScene, long _backResPathId, EUIQueueStageType _stageType, string _nodeUITag, Action _preEnterDone, Action _onEnterDone)
            : base(_uiScene, GUISceneMain.instance, _stageType, _nodeUITag, _preEnterDone, _onEnterDone)
        {
            _m_BackBtnResId = _backResPathId;
        }
        public MainUIMainSceneNode_Var_InGame(_ANPBasicAddContainerUIScene _uiScene, long _backResPathId, EUIQueueStageType _stageType, string _nodeUITag, bool _isOnlyUINode)
            : base(_uiScene, GUISceneMain.instance, _stageType, _nodeUITag, _isOnlyUINode)
        {
            _m_BackBtnResId = _backResPathId;
        }
        public MainUIMainSceneNode_Var_InGame(_ANPBasicAddContainerUIScene _uiScene, long _backResPathId, EUIQueueStageType _stageType, string _nodeUITag, bool _isOnlyUINode, Action _preEnterDone, Action _onEnterDone)
            : base(_uiScene, GUISceneMain.instance, _stageType, _nodeUITag, _isOnlyUINode, _preEnterDone, _onEnterDone)
        {
            _m_BackBtnResId = _backResPathId;
        }

        public override void EnterNode()
        {
            //调用基类函数
            base.EnterNode();

            //判断是否需要回退按钮
            if (_m_BackBtnResId != 0)
            {
                if (null != _m_cbCommonBackObj)
                {
                    NPUIInstanceCommonBackController.instance.hideCommonBack(_m_cbCommonBackObj, _m_BackBtnResId);
                    _m_cbCommonBackObj = null;
                }

                //加载通用回退按钮
                _m_cbCommonBackObj = NPUIInstanceCommonBackController.instance.showCommonBack(EALUIWndLayer.NORMAL, () => { QueueMgr.instance.DoUIRollBackByEscByCheckLastObj(this); }, _m_BackBtnResId);
                
                //处理的窗口加载完再移动到下一级，保证位置
                if (null != _m_cbCommonBackObj && null != _getDealUIScene)
                {
                    _getDealUIScene.regInitDelegate(() =>
                    {
                        //放到最后
                        GCommon.moveTransformToLastAndRefreshLayer(_m_cbCommonBackObj.wnd);
                    });
                }
            }
        }
        public override void QuitNode()
        {
            base.QuitNode();

            if (null != _m_cbCommonBackObj)
            {
                NPUIInstanceCommonBackController.instance.hideCommonBack(_m_cbCommonBackObj, _m_BackBtnResId);
                _m_cbCommonBackObj = null;
            }
        }
    }
}

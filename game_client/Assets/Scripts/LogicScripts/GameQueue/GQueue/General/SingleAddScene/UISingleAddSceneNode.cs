using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 通用的进入MainUIScene视图的处理节点对象
    /// </summary>
    public class UISingleAddSceneNode : _AUISingleAddSceneNode
    {
        private _AALBasicSubContainerScene_NoChild _m_addContainerUIScene;

        /// <summary>
        /// 进入节点前的行为
        /// </summary>
        private Action _m_dPreEnterDone;
        /// <summary>
        /// 进入完成之后调用的函数
        /// </summary>
        private Action _m_dOnEnterDone;

        public UISingleAddSceneNode(_ANPBasicAddContainerUIScene _addContainerUIScene)
            : base()
        {
            _m_addContainerUIScene = _addContainerUIScene;
            _m_dPreEnterDone = null;
            _m_dOnEnterDone = null;
        }
        public UISingleAddSceneNode(_ANPBasicAddContainerUIScene _addContainerUIScene, string _nodeTag)
            : base(_nodeTag)
        {
            _m_addContainerUIScene = _addContainerUIScene;
            _m_dPreEnterDone = null;
            _m_dOnEnterDone = null;
        }
        public UISingleAddSceneNode(_ANPBasicAddContainerUIScene _addContainerUIScene, string _nodeTag, Action _preEnterDone, Action _onEnterDone)
            : base(_nodeTag)
        {
            _m_addContainerUIScene = _addContainerUIScene;
            _m_dPreEnterDone = _preEnterDone;
            _m_dOnEnterDone = _onEnterDone;
        }
        public UISingleAddSceneNode(_ANPBasicAddContainerUIScene _addContainerUIScene, EUIQueueStageType _stageType, string _nodeTag)
            : base(_stageType, _nodeTag)
        {
            _m_addContainerUIScene = _addContainerUIScene;
            _m_dPreEnterDone = null;
            _m_dOnEnterDone = null;
        }
        public UISingleAddSceneNode(_ANPBasicAddContainerUIScene _addContainerUIScene, EUIQueueStageType _stageType, string _nodeTag, Action _preEnterDone, Action _onEnterDone)
            : base(_stageType, _nodeTag)
        {
            _m_addContainerUIScene = _addContainerUIScene;
            _m_dPreEnterDone = _preEnterDone;
            _m_dOnEnterDone = _onEnterDone;
        }

        protected override _AALBasicSubContainerScene_NoChild _getAddScene { get { return _m_addContainerUIScene; } }

        protected override void _preEnterAddScene()
        {
            if (null != _m_dPreEnterDone)
                _m_dPreEnterDone();
            _m_dPreEnterDone = null;
        }

        protected override void _onEnterAddSceneDone()
        {
            if (null != _m_dOnEnterDone)
                _m_dOnEnterDone();
            _m_dOnEnterDone = null;
        }
    }
}

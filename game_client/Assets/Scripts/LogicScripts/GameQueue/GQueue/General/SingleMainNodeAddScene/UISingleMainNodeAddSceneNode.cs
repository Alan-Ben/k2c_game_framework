using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 通用的进入MainUIScene视图的处理节点对象
    /// </summary>
    public class UISingleMainNodeAddSceneNode : _AUISingleMainNodeAddSceneNode
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

        public UISingleMainNodeAddSceneNode(_ANPBasicAddContainerUIScene _addContainerUIScene)
            : base()
        {
            _m_addContainerUIScene = _addContainerUIScene;
            _m_dPreEnterDone = null;
            _m_dOnEnterDone = null;
        }
        public UISingleMainNodeAddSceneNode(_ANPBasicAddContainerUIScene _addContainerUIScene, string _nodeTag)
            : base(_nodeTag, true)
        {
            _m_addContainerUIScene = _addContainerUIScene;
            _m_dPreEnterDone = null;
            _m_dOnEnterDone = null;
        }
        public UISingleMainNodeAddSceneNode(_ANPBasicAddContainerUIScene _addContainerUIScene, string _nodeTag, Action _preEnterDone, Action _onEnterDone, bool _needTransBk)
            : base(_nodeTag, _needTransBk)
        {
            _m_addContainerUIScene = _addContainerUIScene;
            _m_dPreEnterDone = _preEnterDone;
            _m_dOnEnterDone = _onEnterDone;
        }
        public UISingleMainNodeAddSceneNode(_ANPBasicAddContainerUIScene _addContainerUIScene, EUIQueueStageType _stageType, string _nodeTag)
            : base(_stageType, _nodeTag, true)
        {
            _m_addContainerUIScene = _addContainerUIScene;
            _m_dPreEnterDone = null;
            _m_dOnEnterDone = null;
        }
        public UISingleMainNodeAddSceneNode(_ANPBasicAddContainerUIScene _addContainerUIScene, EUIQueueStageType _stageType, string _nodeTag, Action _preEnterDone, Action _onEnterDone, bool _needTransBk)
            : base(_stageType, _nodeTag, _needTransBk)
        {
            _m_addContainerUIScene = _addContainerUIScene;
            _m_dPreEnterDone = _preEnterDone;
            _m_dOnEnterDone = _onEnterDone;
        }

        /// <summary>
        /// 返回模糊背景情况下当前显示的UI对象
        /// </summary>
        protected override _AALBasicLoadUIWndBasicClass blurCurUI { get { return null; } }

        protected override _AALBasicSubContainerScene_NoChild _getAddScene { get { return _m_addContainerUIScene; } }

        protected override void _preEnterAddScene()
        {
            if (null != _m_dPreEnterDone)
                _m_dPreEnterDone();
        }

        protected override void _onEnterAddSceneDone()
        {
            if (null != _m_dOnEnterDone)
                _m_dOnEnterDone();
        }
    }
}

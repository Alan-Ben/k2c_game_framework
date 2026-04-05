using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 通用的进入MainUIScene视图的处理节点对象
    /// </summary>
    public abstract class _AMainUIAddSceneNode_Var : _AMainUIAddSceneNode
    {
        private _ANPBasicAddContainerUIScene _m_addContainerUIScene;
        private _AALBasicTopContainerScene _m_scParentScene;

        /// <summary>
        /// 进入节点前的行为
        /// </summary>
        private Action _m_dPreEnterDone;
        /// <summary>
        /// 进入完成之后调用的函数
        /// </summary>
        private Action _m_dOnEnterDone;

        public _AMainUIAddSceneNode_Var(_ANPBasicAddContainerUIScene _addContainerUIScene, _AALBasicTopContainerScene _parentScene)
            : base()
        {
            _m_addContainerUIScene = _addContainerUIScene;
            _m_scParentScene = _parentScene;
            _m_dPreEnterDone = null;
            _m_dOnEnterDone = null;
        }
        public _AMainUIAddSceneNode_Var(_ANPBasicAddContainerUIScene _addContainerUIScene, _AALBasicTopContainerScene _parentScene, string _nodeTag)
            : base(_nodeTag)
        {
            _m_addContainerUIScene = _addContainerUIScene;
            _m_scParentScene = _parentScene;
            _m_dPreEnterDone = null;
            _m_dOnEnterDone = null;
        }
        public _AMainUIAddSceneNode_Var(_ANPBasicAddContainerUIScene _addContainerUIScene, _AALBasicTopContainerScene _parentScene, string _nodeTag, Action _preEnterDone, Action _onEnterDone)
            : base(_nodeTag)
        {
            _m_addContainerUIScene = _addContainerUIScene;
            _m_scParentScene = _parentScene;
            _m_dPreEnterDone = _preEnterDone;
            _m_dOnEnterDone = _onEnterDone;
        }
        public _AMainUIAddSceneNode_Var(_ANPBasicAddContainerUIScene _addContainerUIScene, _AALBasicTopContainerScene _parentScene, EUIQueueStageType _stageType, string _nodeTag, Action _preEnterDone, Action _onEnterDone)
            : base(_stageType, _nodeTag)
        {
            _m_addContainerUIScene = _addContainerUIScene;
            _m_scParentScene = _parentScene;
            _m_dPreEnterDone = _preEnterDone;
            _m_dOnEnterDone = _onEnterDone;
        }

        /// <summary>
        /// 获取基于的主UI场景是哪个
        /// </summary>
        protected override _AALBasicTopContainerScene _getBasicUIScene { get { return _m_scParentScene; } }

        protected override _ANPBasicAddContainerUIScene _getDealUIScene { get { return _m_addContainerUIScene; } }

        protected override void _preEnterDealUIScene()
        {
            if (null != _m_dPreEnterDone)
                _m_dPreEnterDone();
            _m_dPreEnterDone = null;
        }

        protected override void _onEnterDealUISceneDone()
        {
            if (null != _m_dOnEnterDone)
                _m_dOnEnterDone();
            _m_dOnEnterDone = null;
        }
    }
}

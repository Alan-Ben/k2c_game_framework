using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 通用的进入MainUIScene视图的处理节点对象
    /// </summary>
    public abstract class _ASubUIAddWndNode_Var : _ASubUIAddWndNode
    {
        private _AALBasicLoadUIWndBasicClass _m_addWnd;
        private _ANPBasicAddContainerUIScene _m_scParentScene;

        /// <summary>
        /// 进入节点前的行为
        /// </summary>
        private Action _m_dPreEnterDone;
        /// <summary>
        /// 进入完成之后调用的函数
        /// </summary>
        private Action _m_dOnEnterDone;

        public _ASubUIAddWndNode_Var(_AALBasicLoadUIWndBasicClass _addWnd, _ANPBasicAddContainerUIScene _parentScene)
            : base()
        {
            _m_addWnd = _addWnd;
            _m_scParentScene = _parentScene;
            _m_dPreEnterDone = null;
            _m_dOnEnterDone = null;
        }
        public _ASubUIAddWndNode_Var(_AALBasicLoadUIWndBasicClass _addWnd, _ANPBasicAddContainerUIScene _parentScene, string _nodeTag)
            : base(_nodeTag)
        {
            _m_addWnd = _addWnd;
            _m_scParentScene = _parentScene;
            _m_dPreEnterDone = null;
            _m_dOnEnterDone = null;
        }
        public _ASubUIAddWndNode_Var(_AALBasicLoadUIWndBasicClass _addWnd, _ANPBasicAddContainerUIScene _parentScene, string _nodeTag, Action _preEnterDone, Action _onEnterDone)
            : base(_nodeTag)
        {
            _m_addWnd = _addWnd;
            _m_scParentScene = _parentScene;
            _m_dPreEnterDone = _preEnterDone;
            _m_dOnEnterDone = _onEnterDone;
        }
        public _ASubUIAddWndNode_Var(_AALBasicLoadUIWndBasicClass _addWnd, _ANPBasicAddContainerUIScene _parentScene, EUIQueueStageType _stageType, string _nodeTag, Action _preEnterDone, Action _onEnterDone)
            : base(_stageType, _nodeTag)
        {
            _m_addWnd = _addWnd;
            _m_scParentScene = _parentScene;
            _m_dPreEnterDone = _preEnterDone;
            _m_dOnEnterDone = _onEnterDone;
        }

        /// <summary>
        /// 获取基于的主UI场景是哪个
        /// </summary>
        protected override _ANPBasicAddContainerUIScene _getBasicUIScene { get { return _m_scParentScene; } }

        /// <summary>
        /// 获取对应处理的主UI视图对象
        /// </summary>
        protected override _AALBasicLoadUIWndBasicClass _getDealUIWnd { get { return _m_addWnd; } }

        protected override void _preEnterDealUIWnd()
        {
            if (null != _m_dPreEnterDone)
                _m_dPreEnterDone();
            _m_dPreEnterDone = null;
        }

        protected override void _onEnterDealUIWndDone()
        {
            if (null != _m_dOnEnterDone)
                _m_dOnEnterDone();
            _m_dOnEnterDone = null;
        }
    }
}

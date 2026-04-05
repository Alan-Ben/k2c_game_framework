using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 通用的进入MainUIScene视图的处理节点对象
    /// </summary>
    public abstract class _ASubUIMainWndNode_Var : _ASubUIMainWndNode
    {
        //对应视图对象
        private _AALBasicLoadUIWndBasicClass _m_uiWnd;
        private _ANPBasicAddContainerUIScene _m_scParentScene;

        /// <summary>
        /// 进入节点前的行为
        /// </summary>
        private Action _m_dPreEnterDone;
        /// <summary>
        /// 进入完成之后调用的函数
        /// </summary>
        private Action _m_dOnEnterDone;


        public _ASubUIMainWndNode_Var(_AALBasicLoadUIWndBasicClass _uiWnd, _ANPBasicAddContainerUIScene _parentScene)
            : base()
        {
            _m_uiWnd = _uiWnd;
            _m_scParentScene = _parentScene;
            _m_dPreEnterDone = null;
            _m_dOnEnterDone = null;
        }
        public _ASubUIMainWndNode_Var(_AALBasicLoadUIWndBasicClass _uiWnd, _ANPBasicAddContainerUIScene _parentScene, string _nodeUITag)
            : base(_nodeUITag)
        {
            _m_uiWnd = _uiWnd;
            _m_scParentScene = _parentScene;
            _m_dPreEnterDone = null;
            _m_dOnEnterDone = null;
        }
        public _ASubUIMainWndNode_Var(_AALBasicLoadUIWndBasicClass _uiWnd, _ANPBasicAddContainerUIScene _parentScene, string _nodeUITag, Action _preEnterDone, Action _onEnterDone)
            : base(_nodeUITag, true)
        {
            _m_uiWnd = _uiWnd;
            _m_scParentScene = _parentScene;
            _m_dPreEnterDone = _preEnterDone;
            _m_dOnEnterDone = _onEnterDone;
        }
        public _ASubUIMainWndNode_Var(_AALBasicLoadUIWndBasicClass _uiWnd, _ANPBasicAddContainerUIScene _parentScene, EUIQueueStageType _stageType, string _nodeUITag, Action _preEnterDone, Action _onEnterDone)
            : base(_stageType, _nodeUITag, true)
        {
            _m_uiWnd = _uiWnd;
            _m_scParentScene = _parentScene;
            _m_dPreEnterDone = _preEnterDone;
            _m_dOnEnterDone = _onEnterDone;
        }
        public _ASubUIMainWndNode_Var(_AALBasicLoadUIWndBasicClass _uiWnd, _ANPBasicAddContainerUIScene _parentScene, EUIQueueStageType _stageType, string _nodeUITag, bool _isOnlyUINode)
            : base(_stageType, _nodeUITag, _isOnlyUINode)
        {
            _m_uiWnd = _uiWnd;
            _m_scParentScene = _parentScene;
            _m_dPreEnterDone = null;
            _m_dOnEnterDone = null;
        }
        public _ASubUIMainWndNode_Var(_AALBasicLoadUIWndBasicClass _uiWnd, _ANPBasicAddContainerUIScene _parentScene, EUIQueueStageType _stageType, string _nodeUITag, bool _isOnlyUINode, Action _preEnterDone, Action _onEnterDone)
            : base(_stageType, _nodeUITag, _isOnlyUINode)
        {
            _m_uiWnd = _uiWnd;
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
        protected override _AALBasicLoadUIWndBasicClass _getDealUIWnd { get { return _m_uiWnd; } }
        /// <summary>
        /// 切换进入视图前的事件函数
        /// </summary>
        protected override void _preEnterDealUIWnd()
        {
            if (null != _m_dPreEnterDone)
                _m_dPreEnterDone();
            _m_dPreEnterDone = null;
        }
        /// <summary>
        /// 切换进入视图时完成的处理函数
        /// </summary>
        protected override void _onEnterDealUIWndDone()
        {
            if (null != _m_dOnEnterDone)
                _m_dOnEnterDone();
            _m_dOnEnterDone = null;
        }

    }
}

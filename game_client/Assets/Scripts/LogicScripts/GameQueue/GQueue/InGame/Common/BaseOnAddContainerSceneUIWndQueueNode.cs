using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 基于AddContainerScene管理的UI窗口节点
    /// </summary>
    public class BaseOnAddContainerSceneUIWndQueueNode : _ABaseOnAddContainerSceneUIWndQueueNode
    {
        private bool _m_bIsMainViewNode;//是否MainViewNode, 即进入节点时会调用上一节点的QuitNode
        private _ANPBasicAddContainerUIScene _m_wContainerUIScene;//获取基于的主UI场景
        private _AALBasicLoadUIWndBasicClass _m_wDealUIWnd;//操作的UI窗口
        private bool _m_bNeedControlRes;//是否进行资源控制
        private bool _m_bShowAsMainWndInScene;//是否作为主窗口在Scene中显示, 只有当_getBasicUIScene不为空时有效
        private Action _m_aOnWndShowDone;//当窗口显示时调用
        private Action _m_aOnClickBkClose;//当点击遮罩关闭节点时调用
        private Action _m_aOnQuitNode;//当QuitNode时调用
        private Action _m_aOnCloseNode;//当CloseNode时调用

        public BaseOnAddContainerSceneUIWndQueueNode(EUIQueueStageType _stageType, string _nodeUITag, bool _needTransBk, bool _bNeedClickBkNoRemove, bool _isMainViewNode, _ANPBasicAddContainerUIScene _containerUIScene
            , _AALBasicLoadUIWndBasicClass _dealUIWnd, bool _needControlRes, bool _showAsMainWndInScene, Action _onWndShowDone, Action _onClickBkClose = null, Action _onQuitNode = null, Action _onCloseNode = null
            , bool _isOnlyUINode = true, bool _needAutoRemove = false, bool _isCanRollBackQuit = true) 
            : base(_stageType, _nodeUITag, _needTransBk, _bNeedClickBkNoRemove, _isOnlyUINode, _needAutoRemove, _isCanRollBackQuit)
        {
            _m_bIsMainViewNode = _isMainViewNode;
            _m_wContainerUIScene = _containerUIScene;
            _m_wDealUIWnd = _dealUIWnd;
            _m_bNeedControlRes = _needControlRes;
            _m_bShowAsMainWndInScene = _showAsMainWndInScene;
            _m_aOnWndShowDone = _onWndShowDone;
            _m_aOnClickBkClose = _onClickBkClose;
            _m_aOnQuitNode = _onQuitNode;
            _m_aOnCloseNode = _onCloseNode;
        }

        /// <summary>
        /// 是否MainViewNode, 即进入节点时会调用上一节点的QuitNode
        /// </summary>
        public override bool IsMainViewNode { get { return _m_bIsMainViewNode; } }
        protected override _ANPBasicAddContainerUIScene _getBasicAddUIScene { get { return _m_wContainerUIScene; } }
        protected override _AALBasicLoadUIWndBasicClass _getDealUIWnd { get { return _m_wDealUIWnd; } }
        protected override bool _needControlRes { get { return _m_bNeedControlRes; } }
        protected override bool _showAsMainWndInScene { get { return _m_bShowAsMainWndInScene; } }

        protected override void _onWndShowDone()
        {
            _m_aOnWndShowDone?.Invoke();
        }
        
        protected override void _onClickBkClose()
        {
            _m_aOnClickBkClose?.Invoke();
            _m_aOnClickBkClose = null;
        }

        protected override void _onQuitNode()
        {
            _m_aOnQuitNode?.Invoke();
        }

        protected override void _onCloseNode()
        {
            _m_aOnCloseNode?.Invoke();
            _m_aOnCloseNode = null;
        }
    }
}
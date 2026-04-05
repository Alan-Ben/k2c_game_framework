using ALPackage;

namespace GOE
{
    /// <summary>
    /// 基于AddContainerScene管理的UI窗口节点
    /// </summary>
    public abstract class _ABaseOnAddContainerSceneUIWndQueueNode : BaseQueueNode
    {
        //是否需要加载背景蒙版
        private bool _m_bNeedTransBk;
        //背景部分对象
        private NPPGUIWndInstanceTransparentBk _m_wTransBk;

        //点击遮罩是否不关闭弹窗
        private bool _m_bNeedClickBkNoRemove;
        //是否是纯UI节点
        private bool _m_bIsOnlyUINode;
        private bool _m_bNeedAutoRemove;
        private bool _m_bIsCanRollBackQuit;
        private long _m_lSerializeOp; 
        
        public _ABaseOnAddContainerSceneUIWndQueueNode(EUIQueueStageType _stageType) : base(_stageType)
        {
            _m_bNeedTransBk = true;
            _m_wTransBk = null;
            _m_bNeedClickBkNoRemove = false;
            _m_bNeedAutoRemove = false;
            _m_bIsCanRollBackQuit = true;
        }

        public _ABaseOnAddContainerSceneUIWndQueueNode(EUIQueueStageType _stageType, string _nodeUITag) : base(_stageType, _nodeUITag)
        {
            _m_bNeedTransBk = true;
            _m_wTransBk = null;
            _m_bNeedClickBkNoRemove = false;
            _m_bNeedAutoRemove = false;
            _m_bIsCanRollBackQuit = true;
        }
        
        public _ABaseOnAddContainerSceneUIWndQueueNode(EUIQueueStageType _stageType, string _nodeUITag, bool _needTransBk)
            : base(_stageType, _nodeUITag)
        {
            _m_bNeedTransBk = _needTransBk;
            _m_wTransBk = null;
            _m_bNeedClickBkNoRemove = false;
            _m_bNeedAutoRemove = false;
            _m_bIsCanRollBackQuit = true;
        }
        public _ABaseOnAddContainerSceneUIWndQueueNode(EUIQueueStageType _stageType, string _nodeUITag, bool _needTransBk,bool _bNeedClickBkNoRemove,bool _isOnlyUINode, bool _needAutoRemove, bool _canRollBackQuit)
            : base(_stageType, _nodeUITag)
        {
            _m_bNeedTransBk = _needTransBk;
            _m_wTransBk = null;
            _m_bNeedClickBkNoRemove = _bNeedClickBkNoRemove;
            _m_bIsOnlyUINode = _isOnlyUINode;
            _m_bNeedAutoRemove = _needAutoRemove;
            _m_bIsCanRollBackQuit = _canRollBackQuit;
        }
        
        /// <summary>
        /// 在进行回退处理的时候，队列系统是否能直接退出本节点
        /// </summary>
        public override bool IsCanRollBackQuit { get { return _m_bIsCanRollBackQuit; } }

        /// <summary>
        /// 本节点在进行前进跳转的时候是否需要自动删除
        /// </summary>
        public override bool NeedAutoRemove { get { return _m_bNeedAutoRemove; } }
        
        /// <summary>
        /// 当前节点是否还有效
        /// </summary>
        public override bool isEnable { get { return true; } }

        /// <summary>
        /// 在最后一个节点是同一类型节点时是否允许添加本node
        /// </summary>
        public override bool canAddWhenLastNodeIsTheSameType { get { return true; } }

        /// <summary>
        /// 是否是纯UI节点
        /// </summary>
        public override bool isOnlyUINode { get { return _m_bIsOnlyUINode; } }

        public override void onEnterQueue()
        {
            if (_getBasicAddUIScene != null)//若存在依赖的Scene
            {
                if (_needControlRes)//若需要对资源进行控制
                {
                    _getBasicAddUIScene.enterScene();
                }
                else//若不需要对资源进行控制, 资源应该由外部初始化并加载完成后传入
                {
                    // 不需要对Scene进行enter
                }
            }
            else//若不存在依赖的Scene, 需要Node自己对wnd进行加载
            {
                if (_needControlRes)//若需要对资源进行控制
                {
                    if(_getDealUIWnd != null)
                        _getDealUIWnd.load();
                }
                else//若不需要对资源进行控制, 资源应该由外部初始化并加载完成后传入
                {
                    // 不需要对wnd进行load
                }
            }
        }

        public override void onClose()
        {
            if (_getBasicAddUIScene != null)//若存在依赖的Scene
            {
                if (_needControlRes)//若需要对资源进行控制, 因为_getBasicAddUIScene != null, 窗口由Scene进行管理, 所以这里不用专门做窗口销毁 
                {
                    _getBasicAddUIScene.quitScene();
                }
                else//若不需要对资源进行控制, 资源由外部决定是否销毁, 这里不进行任何操作
                {
                }
            }
            else//若不存在依赖的Scene
            {
                if (_needControlRes)//若需要对资源进行控制, 窗口自己进行销毁
                {
                    if(_getDealUIWnd != null)
                        _getDealUIWnd.discard();
                }
                else//若不需要对资源进行控制, 资源由外部决定是否销毁, 这里不进行任何操作
                {
                    
                }
            }

            _onCloseNode();
        }

        public override void EnterNode()
        {
            //显示背景
            if (_m_bNeedTransBk)
            {
                if (null != _m_wTransBk)
                {
                    _m_wTransBk.showWnd();
                    _afterTransBkAction();
                }
                else
                {
                    _m_lSerializeOp = ALSerializeOpMgr.next();
                    long serializeOp = _m_lSerializeOp;
                    
                    _m_wTransBk = NPPGUIWndInstanceTransparentBk.showTransparentBk(
                        _getDealUIWnd
                        , () => {
                            if (!_m_bNeedClickBkNoRemove)
                            {
                                QueueMgr.instance.forceCloseNode(this);
                                _onClickBkClose();
                            } }
                        , () =>
                        {
                            //防止quitNode后再执行回调
                            if (serializeOp != _m_lSerializeOp)
                                return;
                            // _m_lSerializeOp = ALSerializeOpMgr.next();//在一次EnterNode显示完成后，后续显示不应该再调用一次

                            _afterTransBkAction();
                        });
                }
            }
            else
            {
                _afterTransBkAction();
            }
        }
        
        /// <summary>
        /// 透明背景处理后的实际窗口显示处理
        /// </summary>
        protected void _afterTransBkAction()
        {
            //模糊背景需要跟着窗口后面，先将模糊背景移动到最前
            if (_m_bNeedTransBk && _m_wTransBk != null)
                GCommon.moveTransformToLastAndRefreshLayer(_m_wTransBk.getGameObj());
            
            if (_getBasicAddUIScene != null)//若存在依赖的Scene, wnd通过Scene进行显示, 不需要自己显示
            {
                _getBasicAddUIScene.regInitDelegate(() =>
                {
                    _getBasicAddUIScene.showScene(_afterShowScene);
                });
            }
            else if(_getDealUIWnd != null)// 当不存在依赖的Scene时, 窗口自己进行显示
            {
                _getDealUIWnd.regLoadDoneDelegate(() =>
                {
                    _getDealUIWnd.showWnd(_afterWndShowDone);
                });
            }
        }

        /// <summary>
        /// Scene显示完成
        /// </summary>
        protected void _afterShowScene()
        {
            if (_getBasicAddUIScene != null)//若控制Scene不空, 通过Scene进行窗口显示
            {
                if (_showAsMainWndInScene)//窗口需要作为MainWnd在Scene中显示
                {
                    _getBasicAddUIScene.showMainWnd(_getDealUIWnd, _afterWndShowDone);
                }
                else//窗口需要作为AddWnd在Scene中显示
                {
                    _getBasicAddUIScene.showAddWnd(_getDealUIWnd, _afterWndShowDone);
                }
            }
            else//否则直接调用窗口显示完成回调
            {
                _afterWndShowDone();
            }
        }

        /// <summary>
        /// 窗口显示完成
        /// </summary>
        protected void _afterWndShowDone()
        {
            //TODO: 这个移动背景放在这里可能会有问题, 因为_onWndShowDone会在窗口展示完成动画后调用
            //保证顺序在bk后
            if (_getDealUIWnd != null) 
                GCommon.moveTransformToLastAndRefreshLayer(_getDealUIWnd.getGameObj());

            _onWndShowDone();
        }

        public override void QuitNode()
        {
            _m_lSerializeOp = ALSerializeOpMgr.next();

            //隐藏背景
            NPUIInstanceTransparentBkController.instance.hideTransparentBk(_m_wTransBk);
            _m_wTransBk = null;
            
            if (_getBasicAddUIScene != null)//若存在依赖的Scene
            {
                if (_needControlRes)//若需要对资源进行控制, 因为_getBasicAddUIScene != null, 窗口由Scene进行管理, 所以这里不用专门做窗口隐藏
                {
                    _getBasicAddUIScene.hideScene();
                }
                else//若不需要对资源进行控制, 资源由外部决定是否隐藏, 这里不进行任何操作
                {
                }
            }
            else//若不存在依赖的Scene
            {
                if (_needControlRes)//若需要对资源进行控制, 窗口自己进行隐藏
                {
                    if(_getDealUIWnd != null)
                        _getDealUIWnd.hideWnd();
                }
                else//若不需要对资源进行控制, 资源由外部决定是否隐藏, 不进行任何操作
                {
                    
                }
            }

            _onQuitNode();
        }
        
        /// <summary>
        /// 获取基于的主UI场景是哪个
        /// </summary>
        protected abstract _ANPBasicAddContainerUIScene _getBasicAddUIScene { get; }
        /// <summary>
        /// 获取对应处理的主UI视图对象
        /// </summary>
        protected abstract _AALBasicLoadUIWndBasicClass _getDealUIWnd { get; }
        
        /// <summary>
        /// 是否需要在Node中对资源进行控制, 若为true表示在Node中需要对资源进行加载控制
        /// 若存在_getBasicAddUIScene, 那么_getDealUIWnd窗口由_getBasicAddUIScene管理, 不管_needControlRes为true或者false, 都不需要在Node中对_getDealUIWnd进行任何加载释放操作
        /// 若不存在_getBasicAddUIScene, 那么若_needControlRes为true将会在节点中对窗口资源控制加载和销毁
        /// </summary>
        protected abstract bool _needControlRes { get; }
        
        /// <summary>
        /// 是否作为主窗口在Scene中显示, 只有当_getBasicAddUIScene不为空时有效
        /// </summary>
        protected abstract bool _showAsMainWndInScene { get; }

        /// <summary>
        /// 窗口显示完成时调用
        /// </summary>
        protected abstract void _onWndShowDone();

        /// <summary>
        /// 当点击遮罩关闭时调用
        /// </summary>
        protected abstract void _onClickBkClose();
        
        /// <summary>
        /// QuitNode时调用
        /// </summary>
        protected abstract void _onQuitNode();

        /// <summary>
        /// CloseNode时调用
        /// </summary>
        protected abstract void _onCloseNode();
    }
}
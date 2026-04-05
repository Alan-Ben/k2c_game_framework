using ALPackage;

namespace GOE
{
    /// <summary>
    /// 火星基地 - 拓展建筑升级节点
    /// </summary>
    public class GNodeMarsExpandBuildingUpgrade : UIQueueBaseNode
    {
        private string _m_sAssetPath;
        private string _m_sObjName;
        private _IMarsBuildingView _m_buildingView;
        private bool _m_bNeedFocusBuilding;//是否需要聚焦建筑

        private GGUIWndMarsExpandBuildingUpgrade _m_wndUpgrade;

        //背景部分对象
        private NPPGUIWndInstanceTransparentBk _m_wTransBk;
        private long _m_lBkShowSerialize;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_assetPath">资源路径</param>
        /// <param name="_objName">对象名称</param>
        /// <param name="_buildingView">建筑视图</param>
        public GNodeMarsExpandBuildingUpgrade(string _assetPath, string _objName, _IMarsBuildingView _buildingView, bool _needFocusBuilding = true)
            : base(EUIQueueStageType.MAIN, UINodeTagConst_Mars.C_MARS_EXPAND_BUILDING_UPGRADE)
        {
            _m_sAssetPath = _assetPath;
            _m_sObjName = _objName;
            _m_buildingView = _buildingView;
            _m_bNeedFocusBuilding = _needFocusBuilding;
            
            _m_wTransBk = null;
        }

        public GNodeMarsExpandBuildingUpgrade(long _uiAssetPathId, _IMarsBuildingView _buildingView, bool _needFocusBuilding = true)
            : base(EUIQueueStageType.MAIN, UINodeTagConst_Mars.C_MARS_EXPAND_BUILDING_UPGRADE)
        {
            _m_sAssetPath = UIResPathAssistant.getAssetPath(_uiAssetPathId);
            _m_sObjName = UIResPathAssistant.getObjName(_uiAssetPathId);
            _m_buildingView = _buildingView;
            _m_bNeedFocusBuilding = _needFocusBuilding;
            
            _m_wTransBk = null;
        }
        
        public GNodeMarsExpandBuildingUpgrade(_IMarsBuildingView _buildingView, bool _needFocusBuilding = true)
            : base(EUIQueueStageType.MAIN, UINodeTagConst_Mars.C_MARS_EXPAND_BUILDING_UPGRADE)
        {
            _m_sAssetPath = UIResPathAssistant.getAssetPath(7301);
            _m_sObjName = UIResPathAssistant.getObjName(7301);
            _m_buildingView = _buildingView;
            _m_bNeedFocusBuilding = _needFocusBuilding;
            
            _m_wTransBk = null;
        }
        
        /// <summary>
        /// 在进行回退处理的时候，队列系统是否能直接退出本节点
        /// </summary>
        public override bool IsCanRollBackQuit { get { return true; } }
        
        /// <summary>
        /// 本节点在进行前进跳转的时候是否需要自动删除
        /// </summary>
        public override bool NeedAutoRemove { get { return false; } }
        
        /// <summary>
        /// 在回退操作的时候，是否需要执行上一个节点的进入操作
        /// </summary>
        public override bool IsMainViewNode { get { return false; } }
        
        /// <summary>
        /// 当前节点是否还有效
        /// </summary>
        public override bool isEnable { get { return true; } }
        
        /// <summary>
        /// 本节点是否一个纯粹的UI节点，如果是则将关闭主摄像头
        /// </summary>
        public override bool isOnlyUINode { get { return true; } }

        /// <summary>
        /// 进入队列时调用，创建并加载窗口
        /// </summary>
        public override void onEnterQueue()
        {
            base.onEnterQueue();

            // 创建窗口实例
            if (_m_wndUpgrade == null)
            {
                _m_wndUpgrade = new GGUIWndMarsExpandBuildingUpgrade(_m_sAssetPath, _m_sObjName, _m_bNeedFocusBuilding);
                _m_wndUpgrade.load();
            }
        }

        /// <summary>
        /// 关闭时调用，销毁窗口
        /// </summary>
        public override void onClose()
        {
            base.onClose();

            // 销毁窗口
            if (_m_wndUpgrade != null)
            {
                _m_wndUpgrade.discard();
                _m_wndUpgrade = null;
            }

            // 清空引用
            _m_buildingView = null;
        }

        /// <summary>
        /// 当进入节点时做的操作
        /// </summary>
        public override void EnterNode()
        {
            if (_m_wndUpgrade == null)
                return;

            // if (null != _m_wTransBk)
            // {
            //     _m_wTransBk.showWnd();
            //     _afterTransBkAction();
            // }
            // else
            // {
            //     _m_lBkShowSerialize = ALSerializeOpMgr.next();
            //     long serialize = _m_lBkShowSerialize;
            //         
            //     _m_wTransBk = NPPGUIWndInstanceTransparentBk.showTransparentBk(
            //         _m_wndUpgrade
            //         , () => { QueueMgr.instance.forceCloseNode(this); }
            //         , () =>
            //         {
            //             if(serialize != _m_lBkShowSerialize)
            //                 return;
            //                 
            //             _afterTransBkAction();
            //         });
            // }
            
            _afterTransBkAction();
        }

        /// <summary>
        /// 透明背景处理后的实际窗口显示处理
        /// </summary>
        protected void _afterTransBkAction()
        {
            if(_m_wndUpgrade == null)
                return;
            
            _m_wndUpgrade.regLoadDoneDelegate(() =>
            {
                if(_m_wndUpgrade == null)
                    return;
                
                //模糊背景需要跟着窗口后面，先将模糊背景移动到最前
                if(_m_wTransBk != null)
                    GCommon.moveTransformToLastAndRefreshLayer(_m_wTransBk.getGameObj(), _m_wndUpgrade.getGameObj());
                else
                    //没有遮罩就只将窗口移到最前
                    GCommon.moveTransformToLastAndRefreshLayer(_m_wndUpgrade.getGameObj());
                
                _m_wndUpgrade.showWnd();
                _m_wndUpgrade.refreshWnd(_m_buildingView);
            });
        }
        
        /// <summary>
        /// 当退出节点时做的操作
        /// </summary>
        public override void QuitNode()
        {
            _m_lBkShowSerialize = ALSerializeOpMgr.next();

            //隐藏背景
            NPUIInstanceTransparentBkController.instance.hideTransparentBk(_m_wTransBk);
            _m_wTransBk = null;
            
            _m_wndUpgrade?.hideWnd();
        }
    }
}

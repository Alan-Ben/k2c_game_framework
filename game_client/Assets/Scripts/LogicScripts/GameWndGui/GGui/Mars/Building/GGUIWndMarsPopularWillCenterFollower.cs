using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星民意中心Hub控制器
    /// </summary>
    public class GGUIWndMarsPopularWillCenterFollowerController : _ATALGGUICommonFollowItemController<GGUIMonoMarsPopularWillCenterFollower, GGUIWndMarsPopularWillCenterFollower>
    {
        private readonly GResPathIndex _m_resIndex;
        private readonly MarsPopularWillCenterView _m_popularWillCenterView;


        public GGUIWndMarsPopularWillCenterFollowerController(int _uiAssetPathId, MarsPopularWillCenterView _view)
        {
            _m_resIndex = new GResPathIndex(_uiAssetPathId);
            _m_popularWillCenterView = _view;
        }
        
        
        public override _AALBasicLoadResIndexInfo followItemIndex { get { return _m_resIndex; } }
        
        
        protected override GGUIWndMarsPopularWillCenterFollower _createItemWnd(GGUIMonoMarsPopularWillCenterFollower _wndMono)
        {
            if (_wndMono == null)
                return null;

            GGUIWndMarsPopularWillCenterFollower wnd = new GGUIWndMarsPopularWillCenterFollower(_wndMono, _m_popularWillCenterView);
            wnd.showWnd();
            return wnd;
        }

        public override void onFollowRootShow()
        {
            if(wnd != null)
                wnd.showWnd();
        }

        public override void onFollowRootHide()
        {
            if(wnd != null)
                wnd.hideWnd();
        }

        public void refreshWnd()
        {
            if (wnd == null)
                return;
            
            wnd.refreshWnd();
        }
    }


    /// <summary>
    /// 火星民意中心Follower窗口
    /// </summary>
    public class GGUIWndMarsPopularWillCenterFollower : _ATALGGUIWndCommonFollowItem<GGUIMonoMarsPopularWillCenterFollower>
    {
        private readonly MarsPopularWillCenterView _m_popularWillCenterView;


        public GGUIWndMarsPopularWillCenterFollower(GGUIMonoMarsPopularWillCenterFollower _wnd, MarsPopularWillCenterView _view) : base(_wnd)
        {
            _m_popularWillCenterView = _view;
            
            initWnd();
        }


        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            // 绑定按钮点击事件
            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClickPopularWillCenter);
        }


        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                // 解绑按钮点击事件
                ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onClickPopularWillCenter);
            }
        }


        protected override void _onShowWnd()
        {
            refreshWnd();
        }


        protected override void _onHideWnd()
        {
        }


        protected override void _onReset()
        {
        }


        /// <summary>
        /// 刷新窗口显示
        /// </summary>
        public void refreshWnd()
        {
            if (wnd == null || !isShow)
                return;

            // TODO: 根据实际需求刷新民意中心相关状态显示
            // 例如：显示待处理信件数量、求助数量等
        }


        /// <summary>
        /// 点击民意中心按钮
        /// </summary>
        private void _onClickPopularWillCenter(GameObject _go)
        {
            if (_m_popularWillCenterView == null)
                return;

            _m_popularWillCenterView._onClickPopularWillCenter();
        }
    }
}
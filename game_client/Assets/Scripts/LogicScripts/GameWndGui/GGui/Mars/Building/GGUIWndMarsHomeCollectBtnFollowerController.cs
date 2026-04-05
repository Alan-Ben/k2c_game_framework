using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndMarsHomeCollectBtnFollowerController : _ATALGGUICommonFollowItemController<GGUIMonoMarsHomeCollectBtn, GGUIWndMarsHomeCollectBtnFollower>
    {
        private readonly GResPathIndex _m_resIndex;
        private _IMarsBuildingView _m_buildingView;


        public GGUIWndMarsHomeCollectBtnFollowerController()
        {
            _m_resIndex = new GResPathIndex(7127);
        }
        public GGUIWndMarsHomeCollectBtnFollowerController(GResPathIndex _resIndex)
        {
            _m_resIndex = _resIndex;
        }


        public override _AALBasicLoadResIndexInfo followItemIndex { get { return _m_resIndex; } }


        public void tick()
        {
            if (NPPlayer.instance.marsComp.buildingSubComponent.canCollecHome)
            {
                //显示
                wnd?.showWnd();
                //刷新窗口状态
                wnd?.refreshWnd();
            }
            else
            {
                //隐藏
                wnd?.hideWnd();
            }
        }

        protected override GGUIWndMarsHomeCollectBtnFollower _createItemWnd(GGUIMonoMarsHomeCollectBtn _wndMono)
        {
            GGUIWndMarsHomeCollectBtnFollower wnd = new GGUIWndMarsHomeCollectBtnFollower(_wndMono);

            wnd.showWnd();

            return wnd;
        }
    }

    public class GGUIWndMarsHomeCollectBtnFollower : _ATALGGUIWndCommonFollowItem<GGUIMonoMarsHomeCollectBtn>
    {
        public GGUIWndMarsHomeCollectBtnFollower(GGUIMonoMarsHomeCollectBtn _wnd) : base(_wnd)
        {
            initWnd();
        }
        

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_MARS_HOME_COLLECT, _onSimulateClickCollect);
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_MARS_HOME_COLLECT, _onSimulateClickCollect);
        }
        protected override void _onReset()
        {
        }
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnCollect, _onBtnCollectClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.combineBtnClick(wnd.btnCollect, _onBtnCollectClick);
        }
        

        public void refreshWnd(_IMarsBuildingView _buildingView)
        {
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            _setRed(NPPlayer.instance.marsComp.buildingSubComponent.isCollectRed);
        }
        

        private void _onBtnCollectClick(GameObject _obj)
        {
            //调用收集处理
            NPPlayer.instance.marsComp.buildingSubComponent.collectHome();
        }

        private void _onSimulateClickCollect()
        {
            if (wnd == null) return;
            if (wnd.btnCollect == null) return;
            _onBtnCollectClick(wnd.btnCollect.gameObject);
        }

        private void _setRed(bool _isRed)
        {
            if (wnd == null)
                return;
            
            if(_isRed)
            {
                ALUGUICommon.setGameObjEnable(wnd.listRedHide, false);
                ALUGUICommon.setGameObjEnable(wnd.listRedShow, true);
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.listRedShow, false);
                ALUGUICommon.setGameObjEnable(wnd.listRedHide, true);
            }
        }
    }
}
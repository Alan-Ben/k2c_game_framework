using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 卧室建筑入口 FollowItem Controller
    /// </summary>
    public class GGUIWndRoomBuildingEntranceFollowItemController : _ATALGGUICommonFollowItemController<GGUIMonoRoomBuildingEntrance, GGUIWndRoomBuildingEntrance>
    {
        [NotNull] private readonly GResPathIndex _m_resIndex;
        private PlayerRoomSkinRefObj _m_roomSkinRefObj;
        
        
        public GGUIWndRoomBuildingEntranceFollowItemController()
        {
            _m_resIndex = new GResPathIndex(1122);
        }
        
        
        public override _AALBasicLoadResIndexInfo followItemIndex { get { return _m_resIndex; } }
        protected override GGUIWndRoomBuildingEntrance _createItemWnd(GGUIMonoRoomBuildingEntrance _wndMono)
        {
            GGUIWndRoomBuildingEntrance wnd = new GGUIWndRoomBuildingEntrance(_wndMono);
            wnd.refreshWnd(_m_roomSkinRefObj);
            wnd.showWnd();
            return wnd;
        }


        public void setRoomSkinRef(PlayerRoomSkinRefObj _roomSkinRef)
        {
            _m_roomSkinRefObj = _roomSkinRef;
            wnd?.refreshWnd(_m_roomSkinRefObj);
        }
    }
    
    /// <summary>
    /// 卧室建筑入口 Wnd
    /// </summary>
    public class GGUIWndRoomBuildingEntrance : _ATALGGUIWndCommonFollowItem<GGUIMonoRoomBuildingEntrance>
    {
        private PlayerRoomSkinRefObj _m_roomSkinRefObj;
        
        
        public GGUIWndRoomBuildingEntrance(GGUIMonoRoomBuildingEntrance _wnd) 
            : base(_wnd)
        {
            initWnd();
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
        protected override void _onDiscard()
        {
        }
        protected override void _onWndInitDone()
        {
        }


        public void refreshWnd(PlayerRoomSkinRefObj _roomSkinRef)
        {
            _m_roomSkinRefObj = _roomSkinRef;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_roomSkinRefObj == null)
                return;

            // TODO: 需要根据实际需求设置名字文本，例如从配置表获取名称
            // ALUGUICommon.setLabelTxt(wnd.txtName, _m_roomSkinRefObj.getName());
        }
    }
}

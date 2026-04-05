namespace GOE
{
    public class GGUISubWndAdultEngageRequestSendGuildContainer : _AGGUISubWndCommonContainer<GGUIMonoAdultEngageRequestSendGuildContainerItem, GGUIMonoAdultEngageRequestSendGuildContainer, GGUISubWndAdultEngageRequestSendGuildContainerItem>
    {
        public GGUISubWndAdultEngageRequestSendGuildContainer(GGUIMonoAdultEngageRequestSendGuildContainer _containerMono) 
            : base(_containerMono)
        {
            initWnd();
        }
        

        protected override GGUISubWndAdultEngageRequestSendGuildContainerItem _createItemWnd(GGUIMonoAdultEngageRequestSendGuildContainerItem _itemMono)
        {
            return new GGUISubWndAdultEngageRequestSendGuildContainerItem(_itemMono);
        }
        protected override void _refreshItemWnd(GGUISubWndAdultEngageRequestSendGuildContainerItem _itemWnd, int _index)
        {
        }
    }
}
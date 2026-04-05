
namespace GOE
{
    public class GGUISubWndAdultRankGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoAdultRankGridItem, GGUIMonoAdultRankGrid, GGUISubWndAdultRankGridItem>
    {
        public GGUISubWndAdultRankGrid(GGUIMonoAdultRankGrid _wnd) 
            : base(_wnd)
        {
            initWnd();
        }
        

        protected override void _onShowWnd()
        {
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
        protected override GGUISubWndAdultRankGridItem _createItemWnd(GGUIMonoAdultRankGridItem _itemMono)
        {
            return new GGUISubWndAdultRankGridItem(_itemMono);
        }
        protected override void _onRefreshItemWnd(GGUISubWndAdultRankGridItem _itemMono, int _itemIdx)
        {
        }


        public void refreshWnd()
        {
        }
    }
}
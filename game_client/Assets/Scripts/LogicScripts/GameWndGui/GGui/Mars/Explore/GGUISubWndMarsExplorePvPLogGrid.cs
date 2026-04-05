
using System.Collections.Generic;
using Common.MarsObj;

namespace GOE
{
    public class GGUISubWndMarsExplorePvPLogGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoMarsExplorePvPLogGridItem, GGUIMonoMarsExplorePvPLogGrid, GGUISubWndMarsExplorePvPLogGridItem>
    {
        private List<GGUIWndMarsExplorePvPLog._ALogData> _m_logList;


        public GGUISubWndMarsExplorePvPLogGrid(GGUIMonoMarsExplorePvPLogGrid _containerMono)
            : base(_containerMono)
        {
            _m_logList = new List<GGUIWndMarsExplorePvPLog._ALogData>();

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


        protected override GGUISubWndMarsExplorePvPLogGridItem _createItemWnd(GGUIMonoMarsExplorePvPLogGridItem _itemMono)
        {
            GGUISubWndMarsExplorePvPLogGridItem gridItem = new GGUISubWndMarsExplorePvPLogGridItem(_itemMono);
            return gridItem;
        }
        protected override void _onRefreshItemWnd(GGUISubWndMarsExplorePvPLogGridItem _itemWnd, int _itemIdx)
        {
            if (_itemWnd == null)
                return;

            GGUIWndMarsExplorePvPLog._ALogData logIdx = _m_logList.SafeGet(_itemIdx);
            _itemWnd.refreshWnd(logIdx, _m_logList);
        }


        public void refreshWnd(List<GGUIWndMarsExplorePvPLog._ALogData> _logList)
        {
            _m_logList = _logList;
            setItemCount(_m_logList?.Count ?? 0);
        }
    }
}

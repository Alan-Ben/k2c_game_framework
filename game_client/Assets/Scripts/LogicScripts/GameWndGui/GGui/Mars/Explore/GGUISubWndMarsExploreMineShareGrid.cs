
using System.Collections.Generic;
using _AMineShareItemViewData = GOE.GGUIWndMarsExploreMineShare._AMineShareItemViewData;

namespace GOE
{
    /// <summary>
    /// 矿分享列表Grid子窗口
    /// 管理循环利用的GridItem列表
    /// </summary>
    public class GGUISubWndMarsExploreMineShareGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoMarsExploreMineShareGridItem, GGUIMonoMarsExploreMineShareGrid, GGUISubWndMarsExploreMineShareGridItem>
    {
        /// <summary>表现数据列表</summary>
        private List<_AMineShareItemViewData> _m_dataList;


        public GGUISubWndMarsExploreMineShareGrid(GGUIMonoMarsExploreMineShareGrid _containerMono)
            : base(_containerMono)
        {
            _m_dataList = new List<_AMineShareItemViewData>();

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


        /// <summary>
        /// 创建GridItem窗口实例
        /// </summary>
        protected override GGUISubWndMarsExploreMineShareGridItem _createItemWnd(GGUIMonoMarsExploreMineShareGridItem _itemMono)
        {
            GGUISubWndMarsExploreMineShareGridItem gridItem = new GGUISubWndMarsExploreMineShareGridItem(_itemMono);
            return gridItem;
        }

        /// <summary>
        /// 刷新单个GridItem（当item被循环利用时调用）
        /// </summary>
        protected override void _onRefreshItemWnd(GGUISubWndMarsExploreMineShareGridItem _itemWnd, int _itemIdx)
        {
            if (_itemWnd == null)
                return;

            _AMineShareItemViewData data = _m_dataList.SafeGet(_itemIdx);
            _itemWnd.refreshWnd(data);
        }


        /// <summary>
        /// 刷新Grid，设置数据列表
        /// </summary>
        public void refreshWnd(List<_AMineShareItemViewData> _dataList)
        {
            _m_dataList = _dataList;
            setItemCount(_m_dataList?.Count ?? 0);
        }
    }
}

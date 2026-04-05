using ALPackage;
using JetBrains.Annotations;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 冲榜详情排行榜列表
    /// </summary>
    public class GGUIWndRankRushDetailRankGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoRankRushDetailRankGridItem, GGUIMonoRankRushDetailRankGrid, GGUIWndRankRushDetailRankGridItem>
    {
        //总的数据列表
        [NotNull] private List<NPRankCommonShowInfo> _m_lInfoList = new List<NPRankCommonShowInfo>();

        public GGUIWndRankRushDetailRankGrid(GGUIMonoRankRushDetailRankGrid _containerMono) : base(_containerMono)
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
            if (_m_lInfoList != null)
                _m_lInfoList.Clear();
        }

        protected override void _onWndInitDone()
        {
            // 初始化容器
            setItemCount(0);
        }

        // 创建对象
        protected override GGUIWndRankRushDetailRankGridItem _createItemWnd(GGUIMonoRankRushDetailRankGridItem _itemMono)
        {
            // 创建对象
            GGUIWndRankRushDetailRankGridItem gridItem = new GGUIWndRankRushDetailRankGridItem(_itemMono);
            return gridItem;
        }

        // 刷新对象
        protected override void _onRefreshItemWnd(GGUIWndRankRushDetailRankGridItem _itemMono, int _itemIdx)
        {
            if (_m_lInfoList.Count <= _itemIdx)
                return;

            _itemMono.setInfo(_m_lInfoList[_itemIdx]);
        }

        /// <summary>
        /// 初始化列表
        /// </summary>
        /// <param name="_infoList"></param>
        public void setInfo(List<NPRankCommonShowInfo> _infoList)
        {
            _m_lInfoList.Clear();
            if(_infoList != null)
                _m_lInfoList.AddRange(_infoList);

            //刷新列表
            _refreshList();
        }

        // 刷新列表
        private void _refreshList()
        {
            if (wnd == null)
                return;

            //空物品提示
            ALUGUICommon.setUIObjScale(wnd.noneItemsTips, _m_lInfoList.Count <= 0 ? 1 : 0);

            //刷新grid
            setItemCount(_m_lInfoList.Count);
        }

        //移动到顶部
        public void scrollMoveToTop()
        {
            ALCommonActionMonoTask.addNextFrameTask(() =>
            {
                moveToTop();
            });
        }
    }
}

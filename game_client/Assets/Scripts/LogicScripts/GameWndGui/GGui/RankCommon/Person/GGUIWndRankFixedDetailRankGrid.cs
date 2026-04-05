using ALPackage;
using JetBrains.Annotations;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 排行榜详情排行榜列表
    /// </summary>
    public class GGUIWndRankFixedDetailRankGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoRankFixedDetailRankGridItem, GGUIMonoRankFixedDetailRankGrid, GGUIWndRankFixedDetailRankGridItem>
    {
        //总的数据列表
        [NotNull] private List<NPRankCommonShowInfo> _m_lInfoList = new List<NPRankCommonShowInfo>();

        public GGUIWndRankFixedDetailRankGrid(GGUIMonoRankFixedDetailRankGrid _containerMono) : base(_containerMono)
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
        protected override GGUIWndRankFixedDetailRankGridItem _createItemWnd(GGUIMonoRankFixedDetailRankGridItem _itemMono)
        {
            // 创建对象
            GGUIWndRankFixedDetailRankGridItem gridItem = new GGUIWndRankFixedDetailRankGridItem(_itemMono);
            return gridItem;
        }

        // 刷新对象
        protected override void _onRefreshItemWnd(GGUIWndRankFixedDetailRankGridItem _itemMono, int _itemIdx)
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

            //空列表提示
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

using ALPackage;
using JetBrains.Annotations;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 宴会排行榜列表
    /// </summary>
    public class GGUIWndMiddayDungeonRankGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoMiddayDungeonRankGridItem, GGUIMonoMiddayDungeonRankGrid, GGUIWndMiddayDungeonRankGridItem>
    {
        //总的数据列表
        [NotNull] private List<NPRankCommonShowInfo> _m_lInfoList = new List<NPRankCommonShowInfo>();

        public GGUIWndMiddayDungeonRankGrid(GGUIMonoMiddayDungeonRankGrid _containerMono) : base(_containerMono)
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
        protected override GGUIWndMiddayDungeonRankGridItem _createItemWnd(GGUIMonoMiddayDungeonRankGridItem _itemMono)
        {
            // 创建对象
            GGUIWndMiddayDungeonRankGridItem gridItem = new GGUIWndMiddayDungeonRankGridItem(_itemMono);
            return gridItem;
        }

        // 刷新对象
        protected override void _onRefreshItemWnd(GGUIWndMiddayDungeonRankGridItem _itemMono, int _itemIdx)
        {
            if (_m_lInfoList.Count <= _itemIdx)
                return;

            _itemMono.setInfo(_m_lInfoList[_itemIdx]);
        }

        /// <summary>
        /// 初始化列表
        /// </summary>
        /// <param name="_infoList"></param>
        public void setInfo(List<NPRankCommonShowInfo> _infoList, bool _needMoveScrollToTop)
        {
            if (null == _infoList)
                return;

            _m_lInfoList.Clear();
            _m_lInfoList.AddRange(_infoList);

            //刷新列表
            _refreshList();
            
            if(_needMoveScrollToTop)
                //滚到上边
                scrollMoveToTop();
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

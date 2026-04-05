using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 火星消耗物品容器
    /// </summary>
    public class GGUIWndMarsCostItemContainer : _AGGUISubWndCommonContainer<GGUIMonoMarsCostItem, GGUIMonoMarsCostItemContainer, GGUIWndMarsCostItem>
    {
        private List<NPCommonCostItem> _m_costItemDataList;
        private Action<GGUIWndMarsCostItem> _m_aOnItemClick;

        public GGUIWndMarsCostItemContainer([NotNull] GGUIMonoMarsCostItemContainer _containerMono) : base(_containerMono)
        {
            _m_costItemDataList = new List<NPCommonCostItem>();
            initWnd();
        }

        protected override void _onHideWnd()
        {
            _m_costItemDataList = null;
            _m_aOnItemClick = null;
        }

        /// <summary>
        /// 创建Item窗口
        /// </summary>
        /// <param name="_itemMono">Item的Mono对象</param>
        /// <returns>创建的Item窗口实例</returns>
        protected override GGUIWndMarsCostItem _createItemWnd(GGUIMonoMarsCostItem _itemMono)
        {
            if (_itemMono == null)
                return null;

            return new GGUIWndMarsCostItem(_itemMono);
        }

        /// <summary>
        /// 刷新指定Item窗口
        /// </summary>
        /// <param name="_itemWnd">要刷新的Item窗口</param>
        /// <param name="_index">Item索引</param>
        protected override void _refreshItemWnd(GGUIWndMarsCostItem _itemWnd, int _index)
        {
            if (_m_costItemDataList != null && _index >= 0 && _index < _m_costItemDataList.Count)
            {
                NPCommonCostItem costItemData = _m_costItemDataList[_index];
                _itemWnd.setData(costItemData, _m_aOnItemClick);
            }
        }

        /// <summary>
        /// 刷新容器显示
        /// </summary>
        /// <param name="_costItemDataList">消耗物品数据列表</param>
        /// <param name="_aOnItemClick">物品点击回调</param>
        public void setData(List<NPCommonCostItem> _costItemDataList, Action<GGUIWndMarsCostItem> _aOnItemClick = null)
        {
            _m_costItemDataList = _costItemDataList;
            _m_aOnItemClick = _aOnItemClick;

            refreshWnd(_m_costItemDataList?.Count ?? 0);
        }
    }
}
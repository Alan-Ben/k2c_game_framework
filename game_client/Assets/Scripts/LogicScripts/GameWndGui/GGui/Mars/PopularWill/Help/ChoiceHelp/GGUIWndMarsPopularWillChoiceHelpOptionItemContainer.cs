using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 民意-选择求助-选项容器
    /// </summary>
    public class GGUIWndMarsPopularWillChoiceHelpOptionItemContainer : _AGGUISubWndCommonContainer<GGUIMonoMarsPopularWillChoiceHelpOptionItem, GGUIMonoMarsPopularWillChoiceHelpOptionItemContainer, GGUIWndMarsPopularWillChoiceHelpOptionItem>
    {
        private List<string> _m_lDescList;
        private int _m_iSelectedIndex = -1;

        public GGUIWndMarsPopularWillChoiceHelpOptionItemContainer(GGUIMonoMarsPopularWillChoiceHelpOptionItemContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        public int selectedIndex { get { return _m_iSelectedIndex; } }
        
        /// <summary>
        /// 当点击某个选项
        /// </summary>
        public event Action<GGUIWndMarsPopularWillChoiceHelpOptionItem> onClickOption;

        protected override void _onDiscard()
        {
            _m_lDescList = null;
        }

        protected override GGUIWndMarsPopularWillChoiceHelpOptionItem _createItemWnd(GGUIMonoMarsPopularWillChoiceHelpOptionItem _itemMono)
        {
            var itemWnd = new GGUIWndMarsPopularWillChoiceHelpOptionItem(_itemMono);
            itemWnd.onClick += _onItemClick;
            return itemWnd;
        }

        protected override void _discardItem(GGUIWndMarsPopularWillChoiceHelpOptionItem _itemWnd)
        {
            if (_itemWnd != null)
                _itemWnd.onClick -= _onItemClick;
            base._discardItem(_itemWnd);
        }

        protected override void _refreshItemWnd(GGUIWndMarsPopularWillChoiceHelpOptionItem _itemWnd, int _index)
        {
            if (_m_lDescList == null || _index < 0 || _index >= _m_lDescList.Count)
                return;

            string desc = _m_lDescList[_index];
            _itemWnd.setInfo(_index, desc);
            _itemWnd.setSelected(_index == _m_iSelectedIndex);
        }

        /// <summary>
        /// 设置数据列表与默认选中项
        /// </summary>
        public void setData(List<string> _descList, int _selectedIndex = -1)
        {
            _m_lDescList = _descList;
            _m_iSelectedIndex = _selectedIndex;

            refreshWnd(_m_lDescList?.Count ?? 0);
        }

        /// <summary>
        /// 设置选中索引，自动刷新前后两项
        /// </summary>
        public void setSelectedIndex(int _index)
        {
            if (_m_lDescList == null || _index < -1 || _index >= _m_lDescList.Count)
                return;

            int prev = _m_iSelectedIndex;
            if (prev == _index)
                return;

            _m_iSelectedIndex = _index;

            if (prev >= 0) refreshItem(prev);
            if (_m_iSelectedIndex >= 0) refreshItem(_m_iSelectedIndex);
        }

        private void _onItemClick(GGUIWndMarsPopularWillChoiceHelpOptionItem _itemWnd)
        {
            if (_itemWnd == null)
                return;

            onClickOption?.Invoke(_itemWnd);
        }
    }
}

using System;
using System.Collections.Generic;

namespace GOE
{
    public class GGUISubWndAnecdoteEventChoiceContainer : _AGGUISubWndCommonContainer<GGUIMonoAnecdoteEventChoiceContainerItem, GGUIMonoAnecdoteEventChoiceContainer, GGUISubWndAnecdoteEventChoiceContainerItem>
    {
        private readonly Action<int> _m_onItemSelect;
        
        private List<AnecdoteEventChoiceOptionRefObj> _m_optionList;
        private int _m_currentSelectIndex;
        
        
        public GGUISubWndAnecdoteEventChoiceContainer(GGUIMonoAnecdoteEventChoiceContainer _containerMono, Action<int> _onItemSelect) 
            : base(_containerMono)
        {
            _m_onItemSelect = _onItemSelect;
            initWnd();
        }
        

        protected override GGUISubWndAnecdoteEventChoiceContainerItem _createItemWnd(GGUIMonoAnecdoteEventChoiceContainerItem _itemMono)
        {
            return new GGUISubWndAnecdoteEventChoiceContainerItem(_itemMono, _onSelect);
        }
        protected override void _refreshItemWnd(GGUISubWndAnecdoteEventChoiceContainerItem _itemWnd, int _index)
        {
            if (_m_optionList == null || _index < 0 || _index >= _m_optionList.Count)
                return;

            _itemWnd.refreshWnd(_m_optionList[_index], _index, _index == _m_currentSelectIndex);
        }


        public void refreshWnd(List<AnecdoteEventChoiceOptionRefObj> _optionList, int _currentSelectIndex)
        {
            _m_optionList = _optionList;
            _m_currentSelectIndex = _currentSelectIndex;
            refreshWnd(_m_optionList?.Count ?? 0);
        }
        private void _onSelect(int _index)
        {
            _m_onItemSelect?.Invoke(_index);
        }
    }
}
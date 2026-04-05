using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// item容器
    /// </summary>
    public class GGUIWndChapterEventChoiceItemContainer : _AGGUISubWndCommonContainer<GGUIMonoChapterEventChoiceItem,GGUIMonoChapterEventChoiceItemContainer,GGUIWndChapterEventChoiceItem>
    {
        private readonly Action<int> _m_onItemSelect;
        
        private List<ChapterEventChoiceOptionRefObj> _m_optionList;
        
        public GGUIWndChapterEventChoiceItemContainer(GGUIMonoChapterEventChoiceItemContainer _containerMono, Action<int> _onItemSelect) 
            : base(_containerMono)
        {
            _m_onItemSelect = _onItemSelect;
            initWnd();
        }
        

        protected override GGUIWndChapterEventChoiceItem _createItemWnd(GGUIMonoChapterEventChoiceItem _itemMono)
        {
            return new GGUIWndChapterEventChoiceItem(_itemMono, _onSelect);
        }
        protected override void _refreshItemWnd(GGUIWndChapterEventChoiceItem _itemWnd, int _index)
        {
            if (_m_optionList == null || _index < 0 || _index >= _m_optionList.Count)
                return;

            _itemWnd.refreshWnd(_m_optionList[_index], _index);
        }


        public void refreshWnd(List<ChapterEventChoiceOptionRefObj> _optionList)
        {
            _m_optionList = _optionList;
            refreshWnd(_m_optionList?.Count ?? 0);
        }
        private void _onSelect(int _index)
        {
            _m_onItemSelect?.Invoke(_index);
        }
    }
}

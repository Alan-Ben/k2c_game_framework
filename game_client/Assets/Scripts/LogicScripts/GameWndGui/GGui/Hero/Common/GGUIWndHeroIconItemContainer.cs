using System.Collections.Generic;

namespace GOE
{
    public class GGUIWndHeroIconItemContainer : _ATNPGGUIWndSingleChoiceContainer<GGUIMonoHeroIconItem,GGUIMonoHeroIconItemContainer,GGUIWndHeroIconItem>
    {
        public List<GGUIWndHeroIconItem> _m_lItemGroupList;//子控件列表
        
        public GGUIWndHeroIconItemContainer(GGUIMonoHeroIconItemContainer _mono) : base(_mono)
        {
            initWnd();
        }

        protected override GGUIWndHeroIconItem _createItemWnd(GGUIMonoHeroIconItem _itemMono)
        {
            GGUIWndHeroIconItem itemWnd = new GGUIWndHeroIconItem(_itemMono);
            return itemWnd;
        }

        protected override void _onWndInitDoneEx()
        {
            if(wnd == null)
                return;

            _m_lItemGroupList = new List<GGUIWndHeroIconItem>();
        }
        
        protected override void _onDiscardEx()
        {
            if(_m_lItemGroupList != null)
                _m_lItemGroupList.Clear();
            _m_lItemGroupList = null;
        }
        
        protected override void _onShowWndEx()
        {
        }

        protected override void _onHideWndEx()
        {
        }

        protected override void _onResetEx()
        {
            if(_m_lItemGroupList != null)
                _m_lItemGroupList.Clear();
        }
        
        /// <summary>
        /// 显示item列表
        /// </summary>
        /// <param name="_itemDataList"></param>
        /// <param name="_defaultHeroId">默认item</param>
        public void showItemList(List<_IHeroCardShow> _itemDataList, long _defaultHeroId = 0)
        {
            if (_itemDataList == null)
                return;

            if (_m_lItemGroupList == null)
                _m_lItemGroupList = new List<GGUIWndHeroIconItem>();
                
            _IHeroCardShow tempData = null;
            GGUIWndHeroIconItem tempItemWnd = null;
            GGUIWndHeroIconItem selectedItemWnd = null;
            int wndCount = 0;
            for (int i = 0; i < _itemDataList.Count; ++i)
            {
                tempData = _itemDataList[i];
                if (tempData == null)
                    continue;
                if (wndCount >= _m_lItemGroupList.Count)
                {
                    tempItemWnd = addItemWnd();
                    if (tempItemWnd == null)
                        continue;
                    //放入数据队列
                    _m_lItemGroupList.Add(tempItemWnd);
                }
                else
                {
                    tempItemWnd = _m_lItemGroupList[wndCount];
                }

                tempItemWnd.setData(tempData);
                if (_defaultHeroId == tempData.id)
                {
                    selectedItemWnd = tempItemWnd;
                }
                wndCount++;
            }

            for (int i = _m_lItemGroupList.Count; i > wndCount; i--)
            {
                removeItemWnd(_m_lItemGroupList[i - 1]);
                _m_lItemGroupList.RemoveAt(i - 1);
            }

            if(selectedItemWnd != null)
                setSelectItem(selectedItemWnd);
        }
    }
}
using System.Collections.Generic;
using JetBrains.Annotations;

namespace GOE
{
    public class GGUIWndConsortHaloLvlContainer : _ATNPGGUIWndSingleChoiceContainer<GGUIMonoConsortHaloLvlContainerItem,GGUIMonoConsortHaloLvlContainer,GGUIWndConsortHaloLvlContainerItem>
    {
        [NotNull] private List<GGUIWndConsortHaloLvlContainerItem> _m_lSubWndItemList = new List<GGUIWndConsortHaloLvlContainerItem>();//子窗口列表
        
        public GGUIWndConsortHaloLvlContainer(GGUIMonoConsortHaloLvlContainer _mono) : base(_mono)
        {
            initWnd();
        }

        protected override GGUIWndConsortHaloLvlContainerItem _createItemWnd(GGUIMonoConsortHaloLvlContainerItem _itemMono)
        {
            GGUIWndConsortHaloLvlContainerItem item = new GGUIWndConsortHaloLvlContainerItem(_itemMono);
            return item;
        }

        protected override void _onWndInitDoneEx()
        {
        }
        
        protected override void _onDiscardEx()
        {
            // 销毁时基类会销毁子窗口, 所以这里只要清除列表就行
            _m_lSubWndItemList.Clear();
        }
        
        protected override void _onShowWndEx()
        {
        }

        protected override void _onHideWndEx()
        {
            foreach (var itemWnd in _m_lSubWndItemList)
            {
                itemWnd?.hideWnd();
            }
        }

        protected override void _onResetEx()
        {
            foreach (var itemWnd in _m_lSubWndItemList)
            {
                itemWnd?.resetWnd();
            }
        }
        
        /// <summary>
        /// 显示item列表
        /// </summary>
        public void setData(long _consortId, List<ConsortHaloLvlRefObj> _consortHaloLvlRefList)
        {
            if (_consortHaloLvlRefList == null)
                return;

            ConsortHaloLvlRefObj haloLevelRef = null;
            GGUIWndConsortHaloLvlContainerItem tempItemWnd = null;
            int wndCount = 0;
            for (int i = 0; i < _consortHaloLvlRefList.Count; ++i)
            {
                haloLevelRef = _consortHaloLvlRefList[i];
                if (haloLevelRef == null)
                    continue;
                
                if (wndCount >= _m_lSubWndItemList.Count)
                {
                    tempItemWnd = addItemWnd();
                    if (tempItemWnd == null)
                        continue;
                    //放入数据队列
                    _m_lSubWndItemList.Add(tempItemWnd);
                }
                else
                {
                    tempItemWnd = _m_lSubWndItemList[wndCount];
                }

                tempItemWnd.showWnd();
                tempItemWnd.setData(_consortId, haloLevelRef);
                wndCount++;
            }

            for (int i = _m_lSubWndItemList.Count - 1; i >= wndCount; i--)
            {
                removeItemWnd(_m_lSubWndItemList[i]);
                _m_lSubWndItemList.RemoveAt(i);
            }
        }

        /// <summary>
        /// 选中item
        /// </summary>
        public void selectItem(long _haloLevel, bool _needCallBack)
        {
            foreach (var itemWnd in _m_lSubWndItemList)
            {
                if (itemWnd != null && itemWnd.haloLvlRefObj != null && itemWnd.haloLvlRefObj.level == _haloLevel)
                {
                    if (_needCallBack)
                    {
                        setSelectItem(itemWnd);
                    }
                    else
                    {
                        setSelectItemWithOutCallBack(itemWnd);
                    }
                }
            }
        }
    }
}
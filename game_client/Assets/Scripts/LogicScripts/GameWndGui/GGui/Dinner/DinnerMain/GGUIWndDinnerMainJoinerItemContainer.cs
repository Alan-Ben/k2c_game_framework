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
    public class GGUIWndDinnerMainJoinerItemContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoDinnerMainJoinerItem,GGUIMonoDinnerMainJoinerItemContainer,GGUIWndDinnerMainJoinerItem>
    {
        public List<GGUIWndDinnerMainJoinerItem> _m_lItemGroupList;//子控件列表
        private List<GDinnerJoinerInfo> _m_itemDataList;
        private int _m_defaultSeatCount;
        public GGUIWndDinnerMainJoinerItemContainer(GGUIMonoDinnerMainJoinerItemContainer _containerMono) : base(_containerMono)
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
            if(_m_lItemGroupList != null)
                _m_lItemGroupList.Clear();
        }

        protected override void _onDiscard()
        {
            if(_m_lItemGroupList != null)
                _m_lItemGroupList.Clear();
            _m_lItemGroupList = null;
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            _m_lItemGroupList = new List<GGUIWndDinnerMainJoinerItem>();
        }

        protected override GGUIWndDinnerMainJoinerItem _createItemWnd(GGUIMonoDinnerMainJoinerItem _itemMono)
        {
            GGUIWndDinnerMainJoinerItem itemWnd = new GGUIWndDinnerMainJoinerItem(_itemMono);
            return itemWnd;
        }


        /// <summary>
        /// 显示item列表
        /// </summary>
        /// <param name="_itemDataList"></param>
        public void showItemList(List<GDinnerJoinerInfo> _itemDataList, int _seatCount)
        {
            if (_itemDataList == null)
                return;
            _m_itemDataList = _itemDataList;
            _m_defaultSeatCount = _seatCount;
            GDinnerJoinerInfo tempData = null;
            GGUIWndDinnerMainJoinerItem tempItemWnd = null;
            int count = 0;
            for (int i = 0; i < _seatCount; ++i)
            {
    
                if (count >= _m_lItemGroupList.Count)
                {
                    tempItemWnd = addItemWnd();
                    if (tempItemWnd == null)
                        continue;
                    //放入数据队列
                    _m_lItemGroupList.Add(tempItemWnd);
                }
                else
                {
                    tempItemWnd = _m_lItemGroupList[i];
                }

                if (i < _m_itemDataList.Count)
                    tempData = _m_itemDataList[i];
                else
                    tempData = null;
                tempItemWnd?.setInfo(tempData);
                count++;
            }

            for (int i = _m_lItemGroupList.Count; i > count; i--)
            {
                removeItemWnd(_m_lItemGroupList[i - 1]);
                _m_lItemGroupList.RemoveAt(i - 1);
            }
            moveToTop();
            _refreshContentLayout();
        }

        public void addItem(GDinnerJoinerInfo _itemData)
        {
            if(_m_itemDataList.Count >= _m_defaultSeatCount)
                return;
       
            _itemData?.regDetailInfo(_info =>
            {
                if (_m_lItemGroupList == null)
                    return;

                _m_itemDataList.Add(_itemData);
                GGUIWndDinnerMainJoinerItem itemWnd;
                if (_m_lItemGroupList.Count < _m_itemDataList.Count)
                {
                    itemWnd = addItemWnd();
                }
                else
                {
                    itemWnd = _m_lItemGroupList[_m_itemDataList.Count - 1];
                }
         
                if (itemWnd == null)
                    return;
                
                itemWnd.hideWnd();
                itemWnd.setInfo(_itemData, true);
                itemWnd.showWnd();
            });
        }

        public void showRandomTalk(string _content)
        {
            GGUIWndDinnerMainJoinerItem item = _m_lItemGroupList.GetRandomItem();
            item?.setTalkShow(_content);
        }
        
        /// <summary>
        /// 刷新容器布局
        /// </summary>
        public void _refreshContentLayout()
        {
            ALCommonActionMonoTask.addNextFrameTask(() =>
            {
                if (wnd == null || wnd.itemContainer == null)
                    return;
        
                LayoutRebuilder.ForceRebuildLayoutImmediate(wnd.itemContainer.GetComponent<RectTransform>());
            });
        }
    }
}

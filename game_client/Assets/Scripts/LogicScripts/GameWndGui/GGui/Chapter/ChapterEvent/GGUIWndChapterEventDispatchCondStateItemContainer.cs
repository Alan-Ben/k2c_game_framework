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
    public class GGUIWndChapterEventDispatchCondStateItemContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoChapterEventDispatchCondStateItem,GGUIMonoChapterEventDispatchCondStateItemContainer,GGUIWndChapterEventDispatchCondStateItem>
    {
        public List<GGUIWndChapterEventDispatchCondStateItem> _m_lItemGroupList;//子控件列表
		
        public GGUIWndChapterEventDispatchCondStateItemContainer(GGUIMonoChapterEventDispatchCondStateItemContainer _containerMono) : base(_containerMono)
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

            _m_lItemGroupList = new List<GGUIWndChapterEventDispatchCondStateItem>();
        }

        protected override GGUIWndChapterEventDispatchCondStateItem _createItemWnd(GGUIMonoChapterEventDispatchCondStateItem _itemMono)
        {
            GGUIWndChapterEventDispatchCondStateItem itemWnd = new GGUIWndChapterEventDispatchCondStateItem(_itemMono);
            return itemWnd;
        }


        /// <summary>
        /// 显示item列表
        /// </summary>
        /// <param name="_itemDataList"></param>
        public void showItemList(List<bool> _itemDataList)
        {
            if (_itemDataList == null)
                return;

            GGUIWndChapterEventDispatchCondStateItem tempItemWnd = null;
            int count = 0;
            for (int i = 0; i < _itemDataList.Count; ++i)
            {
                bool tempData = _itemDataList[i];
                if (tempData == null)
                    continue;
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

                tempItemWnd.setInfo(tempData);
                count++;
            }

            for (int i = _m_lItemGroupList.Count; i > count; i--)
            {
                removeItemWnd(_m_lItemGroupList[i - 1]);
                _m_lItemGroupList.RemoveAt(i - 1);
            }

            _refreshContentLayout();
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

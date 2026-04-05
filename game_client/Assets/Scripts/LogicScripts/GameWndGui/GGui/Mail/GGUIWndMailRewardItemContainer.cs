using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    //邮件奖励itemcontainer
    public class GGUIWndMailRewardItemContainer : _ANPGGUIBasicSubWndContainer<GGUIMonoMailRewardItem, GGUIMonoMailRewardItemContainer, GGUIWndMailRewardItem>
    {
        public List<GGUIWndMailRewardItem> _m_lItemGroupList;//子控件列表
        private List<CommonItemData> _m_itemList = new List<CommonItemData>(); //显示列表
        private bool _m_hasGet = false;

        public GGUIWndMailRewardItemContainer(GGUIMonoMailRewardItemContainer _containerMono) : base(_containerMono)
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

            _m_lItemGroupList = new List<GGUIWndMailRewardItem>();
        }

        protected override GGUIWndMailRewardItem _createItemWnd(GGUIMonoMailRewardItem _itemMono)
        {
            GGUIWndMailRewardItem itemWnd = new GGUIWndMailRewardItem(_itemMono);
            return itemWnd;
        }
        /// <summary>
        /// 展示item列表
        /// </summary>
        /// <param name="_hasGet">是否已领取</param>
        /// <param name="_list"></param>
        public void showItemList(bool _hasGet,List<NPCommon.NPCommon_ItemInfo> _list)
        {
            _m_hasGet = _hasGet;

            List<CommonItemData> itemList = new List<CommonItemData>();
            NPCommon.NPCommon_ItemInfo info = null;
            for (int i = 0; i < _list.Count; i++)
            {
                info = _list[i];
                if (null == info)
                    continue;

                itemList.Add(new CommonItemData((NPEnum.ENPItemType)info.getItemType(), info.getSubId(), info.getCount()));
            }
            base.showWnd();
            setItemList(itemList);
        }
        
        /// <summary>
        /// 初始化物品
        /// </summary>
        /// <param name="_itemList"></param>
        public void setItemList(List<CommonItemData> _itemList)
        {
            _m_itemList = _itemList;
            CommonItemData tempData = null;
            GGUIWndMailRewardItem tempItemWnd = null;
            int count = 0;
            for (int i = 0; i < _m_itemList.Count; ++i)
            {
                tempData = _m_itemList[i];
                if (tempData == null)
                    continue;

                if (i >= _m_lItemGroupList.Count)
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

                tempItemWnd.showItem(_m_hasGet,tempData);
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
                moveToLeft();
            });
        }
    }
}
using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 骑士头像item容器
    /// </summary>
    public class GGUIWndConsortBagItemContainer: _ATNPGGUIWndSingleChoiceContainer<GGUIMonoConsortBagItem,GGUIMonoConsortBagItemContainer,GGUIWndConsortBagItem>
    {
        public List<GGUIWndConsortBagItem> _m_lItemGroupList;//子控件列表

        public GGUIWndConsortBagItemContainer(GGUIMonoConsortBagItemContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override void _onShowWndEx()
        {
            WinMsg.RegisterMsgAct(WinMsgType.ON_RED_TIP_CHANGE, _onRedTipChange);
        }

        protected override void _onHideWndEx()
        {
            WinMsg.RegisterMsgAct(WinMsgType.ON_RED_TIP_CHANGE, _onRedTipChange);
        }

        protected override void _onResetEx()
        {
            if(_m_lItemGroupList != null)
                _m_lItemGroupList.Clear();
        }

        protected override void _onDiscardEx()
        {
            if(_m_lItemGroupList != null)
                _m_lItemGroupList.Clear();
            _m_lItemGroupList = null;
        }

        protected override void _onWndInitDoneEx()
        {
            if(wnd == null)
                return;

            _m_lItemGroupList = new List<GGUIWndConsortBagItem>();
        }

        protected override GGUIWndConsortBagItem _createItemWnd(GGUIMonoConsortBagItem _itemMono)
        {
            return new GGUIWndConsortBagItem(_itemMono);
        }

        /// <summary>
        /// 显示item列表
        /// </summary>
        /// <param name="_itemDataList"></param>
        public void showItemList(List<BagItem> _itemDataList, Func<BagItemRefObj, bool> _m_checkRedTipFunc = null)
        {
            if (_itemDataList == null)
                return;

            BagItem tempData = null;
            GGUIWndConsortBagItem tempItemWnd = null;
            int count = 0;
            for (int i = 0; i < _itemDataList.Count; ++i)
            {
                tempData = _itemDataList[i];
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
                    tempItemWnd = _m_lItemGroupList[count];
                }

                tempItemWnd.setInfo(tempData, i, _m_checkRedTipFunc);
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
        /// 显示item列表
        /// </summary>
        /// <param name="_itemDataList"></param>
        public void showItemList(List<BagItemRefObj> _itemDataList, Func<BagItemRefObj, bool> _m_checkRedTipFunc = null)
        {
            if (_itemDataList == null)
                return;

            BagItemRefObj tempData = null;
            GGUIWndConsortBagItem tempItemWnd = null;
            int count = 0;
            for (int i = 0; i < _itemDataList.Count; ++i)
            {
                tempData = _itemDataList[i];
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
                    tempItemWnd = _m_lItemGroupList[count];
                }

                tempItemWnd.setInfo(tempData, i, _m_checkRedTipFunc);
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

        /// <summary>
        /// 根据下标选中item
        /// </summary>
        /// <param name="_index"></param>
        public void setSelectByIndex(int _index)
        {
            if (_m_lItemGroupList == null || _index >= _m_lItemGroupList.Count || _index < 0)
                return;

            setSelectItem(_m_lItemGroupList[_index]);
        }

        /// <summary>
        /// 根据下标选中item
        /// </summary>
        /// <param name="_index"></param>
        public void setSelectByIndexWithOutCallBack(int _index)
        {
            if (_m_lItemGroupList == null || _index >= _m_lItemGroupList.Count || _index < 0)
                return;

            GGUIWndConsortBagItem tempItem = _m_lItemGroupList[_index];
            setSelectItemWithOutCallBack(tempItem);
            if(tempItem != null && tempItem.bagItemRefObj != null)
                NPPlayer.instance.consortComp.setConsortSendGiftBagItemRedTipRead(tempItem.bagItemRefObj.id);
        }

        /// <summary>
        /// 刷新item
        /// </summary>
        /// <param name="_index"></param>
        public void forceRefreshItem(int _index)
        {
            if (_m_lItemGroupList == null || _index >= _m_lItemGroupList.Count || _index < 0)
                return;
            
            _m_lItemGroupList[_index]?.refreshWnd();
        }

        /// <summary>
        /// 刷新所有item红点
        /// </summary>
        public void refreshAllItemRedTip()
        {
            if(_m_lItemGroupList == null || _m_lItemGroupList.Count <= 0)
                return;

            foreach (var itemWnd in _m_lItemGroupList)
            {
                if(itemWnd == null || !itemWnd.isShow)
                    return;
                
                itemWnd.refreshRedTipShow();
            }
        }
        
        protected override void _onSelectItemChg(GGUIWndConsortBagItem _itemWnd)
        {
            base._onSelectItemChg(_itemWnd);
            if(_itemWnd != null && _itemWnd.bagItemRefObj != null)
                NPPlayer.instance.consortComp.setConsortSendGiftBagItemRedTipRead(_itemWnd.bagItemRefObj.id);
        }
        
        private void _onRedTipChange()
        {
            refreshAllItemRedTip();
        }
    }
}
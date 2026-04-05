using UnityEngine;
using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    public class GGUIWndBagSelectItemContainer : _ANPGGUIBasicSubWndContainer<GGUIMonoBagSelectItemContainerItem, GGUIMonoBagSelectItemContainer, GGUIWndBagSelectItemContainerItem>
    {
        private static GGUIWndBagSelectItemContainer _m_instance = null;
        public static GGUIWndBagSelectItemContainer instance { get { return _m_instance; } }

        public GGUIWndBagSelectItemContainer(GGUIMonoBagSelectItemContainer _wnd)
            : base(_wnd)
        {
            _m_instance = this;
            initWnd();
        }

        protected override GGUIWndBagSelectItemContainerItem _createItemWnd(GGUIMonoBagSelectItemContainerItem _itemMono)
        {
            return new GGUIWndBagSelectItemContainerItem(_itemMono);
        }

        //选择道具的个数
        private int _m_selectCount;

        protected List<GGUIWndBagSelectItemContainerItem> _m_lItemList;
        private int _m_iNeedSelectNum;
        private List<int> _m_selectedIndexList;
        //选中回调
        private Action<int,int> _m_dSelectDelegate;
        //设置回调
        public void setSelectDelegate(Action<int, int> _action)
        {
            _m_dSelectDelegate = _action;
        }

        public List<int> SelectedIndexList { get { return _m_selectedIndexList; } }

        protected override void _onShowWnd()
        {

        }

        protected override void _onHideWnd()
        {

        }

        protected override void _onReset()
        {
            if(_m_lItemList != null)
            {
                GGUIWndBagSelectItemContainerItem tmpWnd = null;
                for(int i = 0; i < _m_lItemList.Count; ++i)
                {
                    tmpWnd = _m_lItemList[i];
                    if(tmpWnd == null)
                        continue;
                    tmpWnd.resetWnd();
                }
                _m_lItemList.Clear();
            }
        }

        protected override void _onDiscard()
        {
            if(_m_lItemList != null)
            {
                GGUIWndBagSelectItemContainerItem tmpWnd = null;
                for(int i = 0; i < _m_lItemList.Count; ++i)
                {
                    tmpWnd = _m_lItemList[i];
                    if(tmpWnd == null)
                        continue;
                    tmpWnd.discard();
                }
                _m_lItemList.Clear();
                _m_lItemList = null;
            }
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

        }

        //设置容器内容
        public void refreshWindow(List<NPCommonCostItem> _uniformIdToNumList, int _needSelectNum)
        {
            if(wnd == null || _uniformIdToNumList == null)
                return;

            _m_iNeedSelectNum = _needSelectNum;

            //清空显示列表
            if(_m_lItemList == null)
            {
                _m_lItemList = new List<GGUIWndBagSelectItemContainerItem>();
            }

            _m_lItemList.Clear();
            clearAll();

            //填入物品信息
            for(int i = 0; i < _uniformIdToNumList.Count; i++)
            {
                NPCommonCostItem item = _uniformIdToNumList[i];
                if(null == item)
                    continue;

                //根据不同类型对象创建不同item
                GGUIWndBagSelectItemContainerItem newItem = addItemWnd();

                //判断数据是否有效，有效则继续设置信息
                if (newItem == null)
                    continue;

                _m_lItemList.Add(newItem);

                newItem.showWnd();
                newItem.setSelected(false);
                newItem.Index = i;
                newItem.setItem(item);
            }

            _m_selectedIndexList = new List<int>();
        }

        //设置容器内容
        public void chgIndex(int _index)
        {
            if(wnd == null || _m_lItemList == null)
                return;

            GGUIWndBagSelectItemContainerItem item = null;

            //已选中
            if(!_isNewSelectIndex(_index))
            {
                //取消选择
                item = _getItem(_index);
                if(item != null)
                    item.setSelected(false);

                //删除索引
                _m_selectedIndexList.Remove(_index);
            }
            else if (_m_iNeedSelectNum == 1)
            {
                //此时只需要选择一个，直接使用替换
                if (_m_selectedIndexList.Count == 1)
                {
                    int selectedIdx = _m_selectedIndexList[0];
                    item = _getItem(selectedIdx);
                    if (item != null)
                        item.setSelected(false);
                }

                //设置选中
                item = _getItem(_index);
                if (item != null)
                    item.setSelected(true);

                //加入索引
                _m_selectedIndexList.Clear();
                _m_selectedIndexList.Add(_index);
            }
            else
            {
                //选择已达上限
                if (_m_selectedIndexList.Count == _m_iNeedSelectNum)
                {
                    //弹出提示提示需要取消选择
                    NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.bag_item_select_too_much_none));//#ui_err_item_select_too_much
                    return;
                }

                //设置选中
                item = _getItem(_index);
                if (item != null)
                    item.setSelected(true);

                //加入索引
                _m_selectedIndexList.Add(_index);
            }
            if (null != _m_dSelectDelegate)
                _m_dSelectDelegate(_m_selectedIndexList.Count, _m_iNeedSelectNum);
        }

        private GGUIWndBagSelectItemContainerItem _getItem(int _index)
        {
            if(_m_lItemList == null)
                return null;

            if(_index < 0 || _index >= _m_lItemList.Count)
                return null;

            return _m_lItemList[_index];
        }

        private bool _isNewSelectIndex(int _index)
        {
            if (null == _m_selectedIndexList)
                return false;

            for(int i = 0; i < _m_selectedIndexList.Count; i++)
            {
                if(_m_selectedIndexList[i] == _index)
                    return false;
            }

            return true;
        }

        //设置使用该道具的个数
        public void setSelectCount(int _count)
        {
            _m_selectCount = _count;

            GGUIWndBagSelectItemContainerItem item = null;
            for (int i = 0; i < _m_lItemList.Count; i++)
            {
                item = _m_lItemList[i];
                if (null == item)
                    continue;
                item.setSelectCount(_count);
            }
        }
    }
}

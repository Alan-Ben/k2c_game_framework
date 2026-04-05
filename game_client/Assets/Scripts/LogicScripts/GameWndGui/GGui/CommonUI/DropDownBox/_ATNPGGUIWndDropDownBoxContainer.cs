using ALPackage;
using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    // 通用下拉框容器
    public abstract class _ATNPGGUIWndDropDownBoxContainer<_T_ITEM_MONO, _T_CONTAINER_MONO, _T_ITEM_WND> : _ANPGGUIBasicSubWndContainer<_T_ITEM_MONO, _T_CONTAINER_MONO, _T_ITEM_WND>
        where _T_ITEM_MONO: NPGGUIMonoDropDownBoxItem
        where _T_CONTAINER_MONO: _TALUGUIMonoContainerWnd<_T_ITEM_MONO>
        where _T_ITEM_WND: _ATNPGGUIWndDropDownBoxItem<_T_ITEM_MONO>
    {
        
        //窗口容器
        protected List<_T_ITEM_WND> _m_lItemList;
        //数据列表
        private List<_INPGGUICommonDropDownBoxInstance> _m_dataList;
        //点击item的回调
        private Action<_INPGGUICommonDropDownBoxInstance> _m_aOnClickItem;

        public _ATNPGGUIWndDropDownBoxContainer(_T_CONTAINER_MONO _containerMono) : base(_containerMono)
        {
            //初始化
            _m_lItemList = new List<_T_ITEM_WND>();
            initWnd();
        }
        public Action<_INPGGUICommonDropDownBoxInstance> onClickItem { get { return _m_aOnClickItem; } set { _m_aOnClickItem = value; } }

        protected override _T_ITEM_WND _createItemWnd(_T_ITEM_MONO _itemMono)
        {
            _T_ITEM_WND item = _createItemWndExt(_itemMono);
            item.onClickItem += _onClickItem;
            return item;
        }
        
        protected abstract _T_ITEM_WND _createItemWndExt(_T_ITEM_MONO _itemMono);

        
        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

        }

        protected override void _onShowWnd()
        {

        }


        protected override void _onHideWnd()
        {

        }

        protected override void _onReset()
        {
            if (_m_lItemList != null)
            {
                _ATNPGGUIWndDropDownBoxItem<_T_ITEM_MONO> temp = null;
                for (int i = 0; i < _m_lItemList.Count; ++i)
                {
                    temp = _m_lItemList[i];
                    if (temp == null)
                        continue;
                    temp.resetWnd();
                }
                _m_lItemList.Clear();
            }
        }

        protected override void _onDiscard()
        {
            if (_m_lItemList != null)
            {
                _m_lItemList.Clear();
                _m_lItemList = null;
            }
        }
        
        //item点击事件
        private void _onClickItem(_INPGGUICommonDropDownBoxInstance _instanceInfo)
        {
            if (_instanceInfo == null)
                return;


            if (null != _m_aOnClickItem)
                _m_aOnClickItem(_instanceInfo);
        }
        
        public void setItemList(List<_INPGGUICommonDropDownBoxInstance> _list)
        {
            if (null == _list)
                return;
            _m_dataList = _list;
            _T_ITEM_WND itemWnd = null;
            int count = 0;
            //遍历玩家数据
            for (int i = 0; i < _list.Count; i++)
            {
                _INPGGUICommonDropDownBoxInstance item = _list[i];
                if (null == item)
                    continue;
                //如果容器内部个数不足则新增视图
                if (i >= _m_lItemList.Count)
                {
                    itemWnd = addItemWnd();
                    if (null == itemWnd)
                        continue;
                    _m_lItemList.Add(itemWnd);
                }
                //如果容器个数足够，则取出
                else
                    itemWnd = _m_lItemList[i];
                itemWnd.showWnd();
                itemWnd.setItemInfo(item);
                count++;
            }
            //隐藏容器中多余的视图
            for (int j = _m_lItemList.Count - 1; j >= count; j--)
            {
                //移除窗口
                removeItemWnd(_m_lItemList[j]);
                //从队列删除
                _m_lItemList.RemoveAt(j);
            }
        }

        /// <summary>
        /// 根据id获取对应item
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        protected _T_ITEM_WND getItem(long _id)
        {
            if (null == _m_lItemList)
                return null;

            foreach (_T_ITEM_WND itemWnd in _m_lItemList)
            {
                if (null != itemWnd && null != itemWnd.InstanceInfo && itemWnd.InstanceInfo.instanceId == _id)
                {
                    return itemWnd;
                }
            }

            return null;
        }
    }
}

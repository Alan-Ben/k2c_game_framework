using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;


namespace GOE
{
    /// <summary>
    /// 玩家装扮列表容器基类，偏移穿插带间隔点的Grid
    /// </summary>
    /// <typeparam name="_T_ITEM_MONO"></typeparam>
    /// <typeparam name="_T_CONTAINER_MONO"></typeparam>
    /// <typeparam name="_T_ITEM_WND"></typeparam>
    public abstract class _AGGUIWndPlayerInfoBaseListItemAlternateGrid<_T_ITEM_MONO, _T_CONTAINER_MONO, _T_ITEM_WND> : _ATGGUIAlternateBasicGrid<_T_ITEM_MONO, _T_CONTAINER_MONO, _T_ITEM_WND>
        where _T_ITEM_MONO : _TALUGUIMonoGridItem
        where _T_CONTAINER_MONO : _AGGUIMonoPlayerInfoBaseListItemAlternateGrid<_T_ITEM_MONO>
        where _T_ITEM_WND : _ATALUGUIBasicGridItemWnd<_T_ITEM_MONO>
    {
        //当前数据
        private List<_APlayerBaseShowInfo> _m_ilItemList;

        //当前选中的item
        private _APlayerBaseShowInfo _m_selectInfo;

        //当选中数据变更时触发的回调给外部处理
        private Action<_APlayerBaseShowInfo> _m_dSelectChgDelegate;

        public List<_APlayerBaseShowInfo> itemList { get { return _m_ilItemList; } }

        public _APlayerBaseShowInfo selectInfo { get { return _m_selectInfo; } }

        public long selectId { get { return null == _m_selectInfo ? 0 : _m_selectInfo.id; } }
        
        //初始化数据
        protected abstract void _initItemList(List<_APlayerBaseShowInfo> _recList);

        //排序
        protected abstract int _getSort(_APlayerBaseShowInfo _x, _APlayerBaseShowInfo _y);

        protected _AGGUIWndPlayerInfoBaseListItemAlternateGrid(_T_CONTAINER_MONO _wnd)
         : base(_wnd)
        {
            _m_selectInfo = null;

        }

        protected override void _onWndInitDone()
        {
            _m_ilItemList = new List<_APlayerBaseShowInfo>();

        }

        protected override void _onDiscard()
        {
            _m_ilItemList.Clear();

        }

        //刷新数据
        public void refresh()
        {
            if (null == wnd)
                return;

            //获取数据对象
            _initItemList(itemList);

            //进行排序
            itemList.Sort(_getSort);

            // 刷新grid
            setItemCount(itemList.Count);

            _sendDelegate();
        }

        protected void _setSelectInfo(_APlayerBaseShowInfo _info)
        {
            _m_selectInfo = _info;
        }

        //设置item点击事件回调外部
        public void setClickDelegate(Action<_APlayerBaseShowInfo> _action)
        {
            _m_dSelectChgDelegate = _action;
            if (null != _action)
                _action(_m_selectInfo);
        }

        //选中grid的item事件
        protected void _selectItem(int _index, _IPlayerInfoItemPlay _iItem = null)
        {
            if (itemList.Count == 0 || _index == -1)
                return;

            //选中下标没变不处理
            int preIdx = -1;
            if (null != selectInfo)
                preIdx = itemList.IndexOf(selectInfo);
            if (preIdx == _index)
                return;

            //设置当前选中对象
            _m_selectInfo = itemList[_index];

            //刷新两个数据对象
            if (preIdx >= 0)
                forceRefreshItem(preIdx);

            forceRefreshItem(_index);
            //播放动画
            _iItem?.playSelectAni();
            _sendDelegate();
        }

        //发送选中回调
        protected void _sendDelegate()
        {
            if (null != _m_dSelectChgDelegate)
                _m_dSelectChgDelegate(selectInfo);
        }

        /// <summary>
        /// 重置选中item 
        /// </summary>
        public void resetSelectItem()
        {
            refresh();
        }
    }
}

using ALPackage;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 玩家装扮列表容器基类
    /// </summary>
    /// <typeparam name="_T_ITEM_MONO"></typeparam>
    /// <typeparam name="_T_CONTAINER_MONO"></typeparam>
    /// <typeparam name="_T_ITEM_WND"></typeparam>
    public abstract class _AGGUIWndPlayerInfoBaseListItemGrid<_T_ITEM_MONO, _T_CONTAINER_MONO, _T_ITEM_WND> : _AALUGUIBasicGridSubWnd<_T_ITEM_MONO, _T_CONTAINER_MONO, _T_ITEM_WND>
        where _T_ITEM_MONO : _TALUGUIMonoGridItem
        where _T_CONTAINER_MONO : _AGGUIMonoPlayerInfoBaseListItemGrid<_T_ITEM_MONO>
        where _T_ITEM_WND : _ATALUGUIBasicGridItemWnd<_T_ITEM_MONO>
    {
        //当前数据
        private List<_APlayerBaseShowInfo> _m_ilItemList;
        //当前选中的item
        private _APlayerBaseShowInfo _m_selectInfo;
        //当选中数据变更时触发的回调给外部处理
        private Action<_APlayerBaseShowInfo> _m_dSelectChgDelegate;
        //需要展示红点的部件id列表
        [NotNull] protected HashSet<long> _m_hNeedShowRedTipId = new HashSet<long>();

        /// <summary>
        /// 数据列表
        /// </summary>
        public List<_APlayerBaseShowInfo> itemList { get { return _m_ilItemList; } }
        /// <summary>
        /// 当前选中的数据
        /// </summary>
        public _APlayerBaseShowInfo selectInfo { get { return _m_selectInfo; } }
        /// <summary>
        /// 当前选中的id
        /// </summary>
        public long selectId { get { return null == _m_selectInfo ? 0 : _m_selectInfo.id; } }
        /// <summary>
        /// 选中的数据变更
        /// </summary>
        public Action<_APlayerBaseShowInfo> onSelectChg { get { return _m_dSelectChgDelegate; } set { _m_dSelectChgDelegate = value; } }

        protected _AGGUIWndPlayerInfoBaseListItemGrid(_T_CONTAINER_MONO _wnd) : base(_wnd)
        {
            _m_selectInfo = null;
        }

        protected sealed override void _onShowWnd()
        {
            _onShowWndEx();
        }

        protected sealed override void _onHideWnd()
        {
            _m_hNeedShowRedTipId.Clear();
            _onHideWndEx();
        }
        protected sealed override void _onReset()
        {
            _onResetEx();
        }

        protected sealed override void _onDiscard()
        {
            _m_ilItemList?.Clear();
            _m_dSelectChgDelegate = null;
            _onDiscardEx();
        }

        protected sealed override void _onWndInitDone()
        {
            _m_ilItemList = new List<_APlayerBaseShowInfo>();
            _onWndInitDoneEx();
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        public void refresh()
        {
            if (null == wnd)
                return;

            //获取数据对象
            _initItemList(_m_ilItemList);

            //进行排序
            _m_ilItemList?.Sort(_getSort);

            // 空物品提示
            if (null != wnd.noneItemsTips)
            {
                ALUGUICommon.setUIObjScale(wnd.noneItemsTips, _m_ilItemList.Count <= 0 ? 1 : 0);
            }

            if (_m_ilItemList != null)
            {
                for (int i = 0; i < _m_ilItemList.Count; i++)
                {
                    if (_m_ilItemList[i].canShowRedTip)
                    {
                        //设置已看
                        _m_ilItemList[i].setIsViewed();
                        //记录部件id
                        if (!_m_hNeedShowRedTipId.Contains(_m_ilItemList[i].id))
                            _m_hNeedShowRedTipId.Add(_m_ilItemList[i].id);
                    }
                }
            }

            // 刷新grid
            setItemCount(_m_ilItemList?.Count ?? 0);

            _sendDelegate();
        }

        /// <summary>
        /// 设置选中信息
        /// </summary>
        /// <param name="_info"></param>
        protected void _setSelectInfo(_APlayerBaseShowInfo _info)
        {
            _m_selectInfo = _info;

            //移除红点展示
            if(_m_selectInfo != null && _m_hNeedShowRedTipId.Contains(_m_selectInfo.id))
                _m_hNeedShowRedTipId.Remove(_m_selectInfo.id);
        }

        /// <summary>
        /// 选中grid的item事件
        /// </summary>
        /// <param name="_index"></param>
        /// <param name="_iItem"></param>
        protected void _selectItem(int _index,_IPlayerInfoItemPlay _iItem = null)
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

            //移除红点展示
            if (_m_selectInfo != null && _m_hNeedShowRedTipId.Contains(_m_selectInfo.id))
                _m_hNeedShowRedTipId.Remove(_m_selectInfo.id);

            //刷新两个数据对象
            if (preIdx >= 0)
                forceRefreshItem(preIdx);

            forceRefreshItem(_index);
            _iItem?.playSelectAni();
            _sendDelegate();
        }

        /// <summary>
        /// 发送选中回调
        /// </summary>
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

        protected abstract void _onShowWndEx();
        protected abstract void _onHideWndEx();
        protected abstract void _onResetEx();
        protected abstract void _onDiscardEx();
        protected abstract void _onWndInitDoneEx();

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="_recList"></param>
        protected abstract void _initItemList(List<_APlayerBaseShowInfo> _recList);
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="_x"></param>
        /// <param name="_y"></param>
        /// <returns></returns>
        protected abstract int _getSort(_APlayerBaseShowInfo _x, _APlayerBaseShowInfo _y);
    }
}

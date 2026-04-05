using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 头像列表
    /// </summary>
    public class GGUIWndPlayerIconListItemGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoPlayerIconListItem, GGUIMonoPlayerIconListItemGrid, GGUIWndPlayerIconListItem>
    {
        //信息列表
        private List<PlayerIconShowInfo> _m_lInfoList;
        //当前选中的
        private PlayerIconShowInfo _m_lCurSelectInfo;
        //点击选中
        private Action<PlayerIconShowInfo> _m_aClickSelect;
        //标题栏
        private List<GGUIWndPlayerIconListBarController> _m_lBarControlerList;
        //需要展示红点的头像id列表
        [NotNull] private HashSet<long> _m_hNeedShowRedTipId = new HashSet<long>();

        /// <summary>
        /// 当前选中的头像
        /// </summary>
        public PlayerIconShowInfo curSelectInfo { get { return _m_lCurSelectInfo; } }
        /// <summary>
        /// 点击选中
        /// </summary>
        public Action<PlayerIconShowInfo> onClickSelect { get { return _m_aClickSelect; } set { _m_aClickSelect = value; } }

        public GGUIWndPlayerIconListItemGrid(GGUIMonoPlayerIconListItemGrid _wnd) : base(_wnd)
        {
            _m_lInfoList = new List<PlayerIconShowInfo>();
            initWnd();
        }

        protected override GGUIWndPlayerIconListItem _createItemWnd(GGUIMonoPlayerIconListItem _itemMono)
        {
            GGUIWndPlayerIconListItem item = new GGUIWndPlayerIconListItem(_itemMono);
            item.onClickItem += _onClickSelect;
            return item;
        }

        protected override void _onRefreshItemWnd(GGUIWndPlayerIconListItem _itemWnd, int _itemIdx)
        {
            if(_itemIdx<0 || _itemIdx >= _m_lInfoList.Count)
                return;

            _itemWnd.refreshItem(_m_lInfoList[_itemIdx], _m_lCurSelectInfo != null && _m_lCurSelectInfo.id == _m_lInfoList[_itemIdx].id, _m_hNeedShowRedTipId.Contains(_m_lInfoList[_itemIdx].id));
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_hNeedShowRedTipId.Clear();
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            if (_m_lBarControlerList != null)
            {
                for (int i = 0; i < _m_lBarControlerList.Count; i++)
                {
                    if (_m_lBarControlerList[i] != null)
                    {
                        removeBar(_m_lBarControlerList[i]);
                        _m_lBarControlerList[i].discard();
                    }
                }
                _m_lBarControlerList.Clear();
                _m_lBarControlerList = null;
            }
        }

        protected override void _onWndInitDone()
        {
        }

        /// <summary>
        /// 设置显示数据
        /// </summary>
        /// <param name="_list"></param>
        public void setShowData()
        {
            //取出所有静态数据集
            List<PlayerIconRefObj> refList = GRefdataCoreMgr.instance.playerIconCore.refList;
            //当前玩家使用的头像Id
            long curId = NPPlayer.instance.playerInfo.getCurrentIconId();

            _m_lInfoList.Clear();
            PlayerIconRefObj tmpObj;
            for (int i = 0; i < refList.Count; i++)
            {
                tmpObj = refList[i];
                if (null == tmpObj)
                    continue;

                //查询玩家是否有对应数据，有则创建
                PlayerIconShowInfo showInfo = null;
                NPPlayerIconItem item = NPPlayer.instance.iconComp.getIcon(tmpObj.id);
                if (null != item)
                {
                    //已有数据则创建
                    showInfo = new PlayerIconShowInfo(item);
                }
                else
                {
                    //判断是否无数据，无则创建新的
                    if (!tmpObj.disable_cannot_see)
                        showInfo = new PlayerIconShowInfo(tmpObj);
                }

                if (null == showInfo)
                    continue;

                //设置当前选中对象
                if (tmpObj.id == curId)
                    setSelect(showInfo);

                //是否需要展示红点，这里保存可展示的红点头像id
                if (showInfo.canShowRedTip)
                {
                    //设置已读
                    showInfo.setIsViewed();
                    //记录需要展示红点的头像id
                    if (!_m_hNeedShowRedTipId.Contains(showInfo.id))
                        _m_hNeedShowRedTipId.Add(showInfo.id);
                }

                _m_lInfoList.Add(showInfo);
            }
            _m_lInfoList.Sort(_sortList);

            setItemCount(_m_lInfoList.Count);

            //显示bar
            _showListBar();
        }

        /// <summary>
        /// 设置选中
        /// </summary>
        /// <param name="_id"></param>
        public void setSelect(PlayerIconShowInfo _info)
        {
            if (_info == null || _m_lInfoList == null)
                return;

            int preIndex = -1;
            int curIndex = -1;
            for (int i = 0; i < _m_lInfoList.Count; i++)
            {
                if (_m_lInfoList[i] == null)
                    continue;

                if (_m_lCurSelectInfo != null && _m_lInfoList[i].id == _m_lCurSelectInfo.id)
                    preIndex = i;
                if (_m_lInfoList[i].id == _info.id)
                    curIndex = i;
            }

            _m_lCurSelectInfo = _info;
            //移除红点展示
            if(_m_hNeedShowRedTipId.Contains(_info.id))
                _m_hNeedShowRedTipId.Remove(_info.id);

            //刷新对应item
            if (preIndex >= 0)
                forceRefreshItem(preIndex);
            if (curIndex >= 0)
                forceRefreshItem(curIndex);

            _m_aClickSelect?.Invoke(_m_lCurSelectInfo);
        }
        
        //显示bar
        private void _showListBar()
        {
            if (wnd == null || _m_lInfoList == null || _m_lInfoList.Count <= 0)
                return;

            //先清空bar
            if (_m_lBarControlerList != null)
            {
                for (int i = 0; i < _m_lBarControlerList.Count; i++)
                {
                    if (_m_lBarControlerList[i] != null)
                    {
                        removeBar(_m_lBarControlerList[i]);
                        _m_lBarControlerList[i].discard();
                    }
                }
                _m_lBarControlerList.Clear();
            }

            //bar需要插入的位置
            List<int> barIndexList = new List<int>();
            //bar需要的groupId列表
            List<EPlayerInfoIconType> typeList = new List<EPlayerInfoIconType>();

            EPlayerInfoIconType tempIconType = EPlayerInfoIconType.NONE;
            for (int i = 0; i < _m_lInfoList.Count; i++)
            {
                if (tempIconType != _m_lInfoList[i].refObj.show_type)
                {
                    tempIconType = _m_lInfoList[i].refObj.show_type;
                    barIndexList.Add(i);
                    typeList.Add(tempIconType);
                }
            }

            if (_m_lBarControlerList == null)
                _m_lBarControlerList = new List<GGUIWndPlayerIconListBarController>();

            for (int i = 0; i < typeList.Count; i++)
            {
                EPlayerInfoIconType iconType = typeList[i];
                int barIndex = barIndexList[i];
                GGUIWndPlayerIconListBarController barController = new GGUIWndPlayerIconListBarController(wnd.gridAreaUIObj);
                addBar(barController);
                barController.regLoadDoneDelegate(() =>
                {
                    barController.setInsertIndex(barIndex);
                    barController.setInfo(iconType);
                    forceRefreshBar();
                });
                _m_lBarControlerList.Add(barController);
            }
        }

        //排序规则
        private int _sortList(PlayerIconShowInfo _a, PlayerIconShowInfo _b)
        {
            //比较类型
            int res = _a.refObj.show_type.CompareTo(_b.refObj.show_type);
            if (res != 0)
                return res;

            //比较是否有锁
            res = _a.isLock.CompareTo(_b.isLock);
            if (res != 0)
                return res;

            //比较失效
            res = _a.isExpired.CompareTo(_b.isExpired);
            if (res != 0)
                return res;

            //比较品质
            if (null != _a.baseData && null != _b.baseData)
                res = -_a.baseData.quality.CompareTo(_b.baseData.quality);
            if (res != 0)
                return res;

            //比较物品id
            res = _a.refObj.id.CompareTo(_b.refObj.id);
            return res;
        }

        //点击item事件
        private void _onClickSelect(GGUIWndPlayerIconListItem _item)
        {
            if (_item == null || _item.showInfo == null || (_m_lCurSelectInfo != null && _m_lCurSelectInfo.id == _item.showInfo.id))
                return;

            int preIndex = -1;
            int curIndex = -1;
            for (int i = 0; i < _m_lInfoList.Count; i++)
            {
                if (_m_lInfoList[i] == null)
                    continue;

                if (_m_lCurSelectInfo != null && _m_lInfoList[i].id == _m_lCurSelectInfo.id)
                    preIndex = i;
                if (_m_lInfoList[i].id == _item.showInfo.id)
                    curIndex = i;
            }

            _m_lCurSelectInfo = _item.showInfo;
            //移除红点展示
            if (_m_hNeedShowRedTipId.Contains(_item.showInfo.id))
                _m_hNeedShowRedTipId.Remove(_item.showInfo.id);

            //刷新对应item
            if (preIndex >= 0)
                forceRefreshItem(preIndex);
            if (curIndex >= 0)
                forceRefreshItem(curIndex);

            _m_aClickSelect?.Invoke(_m_lCurSelectInfo);
        }
    }
}

using ALPackage;
using DG.Tweening;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndInnStationListContainer : _AGGUISubWndCommonContainer<GGUIMonoInnStationListContainerItem, GGUIMonoInnStationListContainer, GGUISubWndInnStationListContainerItem>
    {
        [ItemNotNull, NotNull] private readonly List<InnStationInfo> _m_stationList;
        private InnViewMgr _m_viewMgr;
        private GGUISubWndInnStationListContainerItem _m_wCurSelectItem;
        private InnStationInfo _m_curSelectInfo;
        private Action<GGUISubWndInnStationListContainerItem> _m_aOnSelectItem;

        /// <summary>
        /// 选中item时的回调
        /// </summary>
        public Action<GGUISubWndInnStationListContainerItem> onSelectItem { get { return _m_aOnSelectItem; } set { _m_aOnSelectItem = value; } }


        public GGUISubWndInnStationListContainer(GGUIMonoInnStationListContainer _containerMono) 
            : base(_containerMono)
        {
            _m_stationList = new List<InnStationInfo>();
            
            initWnd();
        }

        protected override void _onShowWnd()
        {
            base._onShowWnd();
        }

        protected override void _onHideWnd()
        {
            base._onHideWnd();
            resertSelect();
            moveToLeft();
        }

        protected override void _onReset()
        {
            base._onReset();
        }

        protected override void _onDiscard()
        {
            base._onDiscard();
            _m_wCurSelectItem = null;
            _m_curSelectInfo = null;
            _m_aOnSelectItem = null;
        }

        protected override void _onWndInitDone()
        {
            base._onWndInitDone();
        }


        protected override GGUISubWndInnStationListContainerItem _createItemWnd(GGUIMonoInnStationListContainerItem _itemMono)
        {
            GGUISubWndInnStationListContainerItem item = new GGUISubWndInnStationListContainerItem(_itemMono);
            item.onSelectItem += _onSelectItem;
            return item;
        }

        protected override void _refreshItemWnd(GGUISubWndInnStationListContainerItem _itemWnd, int _index)
        {
            InnStationInfo stationInfo = _m_stationList.SafeGet(_index);
            if (stationInfo == null)
                return;

            _itemWnd.refreshWnd(stationInfo, _m_viewMgr, _index);

            if (_m_curSelectInfo != null)
            {
                // item顺序发生了变化，重新选中
                if (_m_wCurSelectItem?.stationInfo?.stationId != _m_curSelectInfo.stationId)
                {
                    if(_itemWnd?.stationInfo?.stationId == _m_curSelectInfo.stationId)
                        _onSelectItem(_itemWnd);
                    else
                        _itemWnd.setSelect(false);
                }
                else if(stationInfo?.stationId != _m_curSelectInfo.stationId)
                    _itemWnd.setSelect(false);
                else
                    _itemWnd.setSelect(true);
            }
            else
            {
                //默认选中第一个
                if (_m_wCurSelectItem == null && _index == 0)
                    _onSelectItem(_itemWnd);
                else if (_m_wCurSelectItem != _itemWnd)
                    _itemWnd.setSelect(false);
            }
        }


        public void refreshWnd(InnViewMgr _viewMgr, InnStationInfo _selectStationInfo = null)
        {
            _m_viewMgr = _viewMgr;
            _m_curSelectInfo = _selectStationInfo;
            NPPlayer.instance.innComp.getStationListNonAlloc(_m_stationList);
            _m_stationList.Sort(_sortInfoList);
            refreshWnd(_m_stationList.Count);
        }
        public void resertSelect()
        {
            _m_wCurSelectItem = null;
            _m_curSelectInfo = null;
        }

        /// <summary>
        /// 排序：可解锁，可升级，待解锁，已解锁，未解锁，按ID升序
        /// </summary>
        /// <param name="_a"></param>
        /// <param name="_b"></param>
        /// <returns></returns>
        private int _sortInfoList(InnStationInfo _a, InnStationInfo _b)
        {
            if (_a == null || _b == null || _m_viewMgr == null)
                return 0;

            bool isBuildA = _a.isBuilt;
            bool isBuildB = _b.isBuilt;
            InnStationInfo requireStationInfoA = NPPlayer.instance.innComp.getRequireStationInfo(_a);
            bool isNextBuildingA = requireStationInfoA is { isBuilt: true } or null;
            InnStationInfo requireStationInfoB = NPPlayer.instance.innComp.getRequireStationInfo(_b);
            bool isNextBuildingB = requireStationInfoB is { isBuilt: true } or null;
            bool canBuildA = _m_viewMgr.getHadSettleGuestsCount() >= _a.refObj.need_receive_guest_num && !isBuildA && isNextBuildingA;
            bool canBuildB = _m_viewMgr.getHadSettleGuestsCount() >= _b.refObj.need_receive_guest_num && !isBuildB && isNextBuildingB;
            bool pendingUnlockA = _m_viewMgr.getHadSettleGuestsCount() < _a.refObj.need_receive_guest_num && !isBuildA && isNextBuildingA;
            bool pendingUnlockB = _m_viewMgr.getHadSettleGuestsCount() < _b.refObj.need_receive_guest_num && !isBuildB && isNextBuildingB;
            bool canUpgradeA = _a.isUpgradable();
            bool canUpgradeB = _b.isUpgradable();

            //可解锁在前
            if (canBuildA != canBuildB)
                return -canBuildA.CompareTo(canBuildB);

            //可升级在前
            if (canUpgradeA != canUpgradeB)
                return -canUpgradeA.CompareTo(canUpgradeB);

            //待解锁在前
            if (pendingUnlockA != pendingUnlockB)
                return -pendingUnlockA.CompareTo(pendingUnlockB);

            //已解锁在前
            if (isBuildA != isBuildB)
                return -isBuildA.CompareTo(isBuildB);

            // 再按ID升序
            return _a.refObj.id.CompareTo(_b.refObj.id);
        }

        /// <summary>
        /// 选中item时的回调
        /// </summary>
        /// <param name="_info"></param>
        private void _onSelectItem(GGUISubWndInnStationListContainerItem _item)
        {
            if(_item == null || wnd == null) 
                return;

            _m_wCurSelectItem?.setSelect(false);
            _m_wCurSelectItem = _item;
            _m_wCurSelectItem?.setSelect(true);
            _m_curSelectInfo = _m_wCurSelectItem?.stationInfo;
            _m_aOnSelectItem?.Invoke(_m_wCurSelectItem);

            //设置超出的item移动到里面
            GCommon.setContainerMoveItemWithinRangeInHorizontal(_item.rectTransform, rectTransform, (RectTransform)wnd.itemContainer?.transform);
        }
    }
}
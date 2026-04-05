using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 火星基地 - 居民派遣列表Grid容器窗口
    /// </summary>
    public class GGUIWndMarsResidentDispatchItemGrid : _ANPGGUIBasicGridSubWnd<GGUIMonoMarsResidentDispatchItem, GGUIMonoMarsResidentDispatchItemGrid, GGUIWndMarsResidentDispatchItem>
    {
        private List<MarsBuildingInfo> _m_buildingInfoList;

        public GGUIWndMarsResidentDispatchItemGrid(GGUIMonoMarsResidentDispatchItemGrid _containerMono) : base(_containerMono)
        {
            _m_buildingInfoList = new List<MarsBuildingInfo>();
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            // Grid 初始化，可在此绑定其他按钮事件
        }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.ON_MARS_PEOPLE_NUM_CHG, _onMarsPeopleNumChg);
            NPPlayer.instance.marsComp.onSettleSlotPeopleLimitChanged += _onPeopleSlotLimitChanged;
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_MARS_PEOPLE_NUM_CHG, _onMarsPeopleNumChg);
            NPPlayer.instance.marsComp.onSettleSlotPeopleLimitChanged -= _onPeopleSlotLimitChanged;
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            if (_m_buildingInfoList != null)
            {
                _m_buildingInfoList.Clear();
                _m_buildingInfoList = null;
            }
        }

        protected override GGUIWndMarsResidentDispatchItem _createItemWnd(GGUIMonoMarsResidentDispatchItem _itemMono)
        {
            // 创建Grid Item窗口对象
            GGUIWndMarsResidentDispatchItem gridItem = new GGUIWndMarsResidentDispatchItem(_itemMono);
            return gridItem;
        }

        protected override void _refreshItemwnd(GGUIWndMarsResidentDispatchItem _itemWnd, int _itemIdx)
        {
            if (_m_buildingInfoList == null || _itemIdx >= _m_buildingInfoList.Count || _itemIdx < 0 || _itemWnd == null)
                return;

            // 获取建筑数据对象
            MarsBuildingInfo buildingInfo = _m_buildingInfoList[_itemIdx];
            if (buildingInfo == null)
                return;

            // 设置数据并刷新Item
            _itemWnd.setData(buildingInfo);
        }

        /// <summary>
        /// 刷新Grid显示
        /// </summary>
        public void refreshGrid()
        {
            _updateBuildingList();
            setItemCount(_m_buildingInfoList?.Count ?? 0);
        }

        /// <summary>
        /// 更新建筑数据列表
        /// </summary>
        private void _updateBuildingList()
        {
            if (_m_buildingInfoList == null)
                _m_buildingInfoList = new List<MarsBuildingInfo>();
            _m_buildingInfoList.Clear();

            foreach (var buildingInfo in NPPlayer.instance.marsComp.buildingSubComponent._getBuildingInfos())
            {
                if(buildingInfo.isEnable && buildingInfo.settleSlotData.isValid())
                    _m_buildingInfoList.Add(buildingInfo);
            }
        }

        private void _onMarsPeopleNumChg()
        {
            forceRefreshAllItem();
        }
        
        /// <summary>
        /// 当可派遣人数上限变化时
        /// </summary>
        private void _onPeopleSlotLimitChanged(long _num)
        {
            forceRefreshAllItem();
        }
    }
}

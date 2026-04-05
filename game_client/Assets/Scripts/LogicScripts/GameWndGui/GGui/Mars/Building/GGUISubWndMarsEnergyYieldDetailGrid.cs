using System.Collections.Generic;
using ALPackage;
using Common.MarsEnum;
using JetBrains.Annotations;

namespace GOE
{
    public class GGUISubWndMarsEnergyYieldDetailGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoMarsEnergyYieldDetailGridItem, GGUIMonoMarsEnergyYieldDetailGrid, GGUISubWndMarsEnergyYieldDetailGridItem>
    {
        [ItemNotNull, NotNull] private readonly List<MarsBuildingInfo> _m_energyBuildingList;
        private ALCommonEnableTaskController _m_tickTask;
        
        
        public GGUISubWndMarsEnergyYieldDetailGrid([NotNull] GGUIMonoMarsEnergyYieldDetailGrid _wnd)
            : base(_wnd)
        {
            _m_energyBuildingList = new List<MarsBuildingInfo>();
            
            initWnd();
        }


        protected override void _onShowWnd()
        {
            _m_tickTask = ALCommonEnableTickActionMonoTask.addMonoTask(_tick);
        }
        protected override void _onHideWnd()
        {
            _m_tickTask.setDisable();
        }
        protected override void _onReset()
        {
        }
        protected override void _onDiscard()
        {
        }
        protected override void _onWndInitDone()
        {
        }
        protected override GGUISubWndMarsEnergyYieldDetailGridItem _createItemWnd(GGUIMonoMarsEnergyYieldDetailGridItem _itemMono)
        {
            return new GGUISubWndMarsEnergyYieldDetailGridItem(_itemMono);
        }
        protected override void _onRefreshItemWnd(GGUISubWndMarsEnergyYieldDetailGridItem _itemMono, int _itemIdx)
        {
            MarsBuildingInfo buildingInfo = _m_energyBuildingList.SafeGet(_itemIdx);
            if (buildingInfo == null)
                return;
            
            _itemMono?.refreshWnd(buildingInfo, _itemIdx);
        }


        public void refreshWnd()
        {
            NPPlayer.instance.marsComp.buildingSubComponent.getBuildingInfoListNonAlloc(_m_energyBuildingList,
                _info => _info.type == EMarsBuildingType.ENERGY && _info.state != MarsBuildingInfo.StateType.Unbuilt);
            setItemCount(_m_energyBuildingList.Count);
        }


        private void _tick()
        {
            refreshAllItem((_item, _index) => _item?.tick());
        }
    }
}
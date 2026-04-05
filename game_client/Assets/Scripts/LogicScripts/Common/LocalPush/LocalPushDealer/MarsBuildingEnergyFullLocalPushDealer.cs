using Common.MarsEnum;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 火星能源建筑能量满本地推送
    /// </summary>
    public class MarsBuildingEnergyFullLocalPushDealer : _ARefDataLocalPushDealer
    {
        public MarsBuildingEnergyFullLocalPushDealer():base(ELocalPushType.MARS_BUILDING_ENERGY_FULL)
        {
        }

        /// <summary>
        /// 获取推送原始数据项列表
        /// </summary>
        protected override IReadOnlyList<PushItemData> getItemDataList()
        {
            //获取能源建筑列表
            List<MarsBuildingInfo> buildingList = new List<MarsBuildingInfo>();
            NPPlayer.instance.marsComp.buildingSubComponent.getBuildingInfoListNonAlloc(buildingList, 
                _info=> _info != null && 
                _info.type == EMarsBuildingType.ENERGY && 
                _info.state != MarsBuildingInfo.StateType.Unbuilt);

            if (buildingList.Count <= 0)
                return null;

            long minLeftTimeSec = -1;
            for (int i = 0; i < buildingList.Count; i++)
            {
                MarsBuildingInfo buildingInfo = buildingList[i];
                if(buildingInfo == null)
                    continue;

                long leftStoreTimeSec = buildingInfo.energyProperty.leftStoreTimeSec;
                if (minLeftTimeSec == -1 || (leftStoreTimeSec > 0 && minLeftTimeSec > leftStoreTimeSec))
                    minLeftTimeSec = leftStoreTimeSec;
            }

            if (minLeftTimeSec <= 0)
                return null;

            return new List<PushItemData> { new PushItemData(minLeftTimeSec) };
        }
    }
}

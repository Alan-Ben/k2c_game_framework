using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 火星建筑建造升级完成本地推送
    /// </summary>
    public class MarsBuildingConstructUpgradeCompleteLocalPushDealer : _ARefDataLocalPushDealer
    {
        public MarsBuildingConstructUpgradeCompleteLocalPushDealer() : base(ELocalPushType.MARS_BUILDING_CONSTRUCT_UPGRADE_COMPLETE)
        {
        }

        /// <summary>
        /// 获取推送原始数据项列表（每栋建造或升级中的建筑对应一条推送）
        /// </summary>
        protected override IReadOnlyList<PushItemData> getItemDataList()
        {
            List<MarsBuildingInfo> buildingList = new List<MarsBuildingInfo>();
            NPPlayer.instance.marsComp.buildingSubComponent.getBuildingInfoListNonAlloc(buildingList,
                _info => _info != null &&
                (_info.state == MarsBuildingInfo.StateType.Upgrading || _info.state == MarsBuildingInfo.StateType.Constructing));

            if (buildingList.Count <= 0)
                return null;

            List<PushItemData> itemList = new List<PushItemData>(buildingList.Count);
            for (int i = 0; i < buildingList.Count; i++)
            {
                MarsBuildingInfo buildingInfo = buildingList[i];
                if (buildingInfo == null)
                    continue;

                long leftTimeSec = buildingInfo.remainingBuildOrUpgradeTime / 1000;
                if (leftTimeSec <= 0)
                    continue;

                itemList.Add(new PushItemData(leftTimeSec, null, new object[] { buildingInfo.nameTranslated }));
            }

            return itemList;
        }
    }
}

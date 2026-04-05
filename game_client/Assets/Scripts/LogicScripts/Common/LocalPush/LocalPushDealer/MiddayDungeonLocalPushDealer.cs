using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 午间活动开始本地推送
    /// </summary>
    public class MiddayDungeonLocalPushDealer : _ARefDataLocalPushDealer
    {
        public MiddayDungeonLocalPushDealer():base(ELocalPushType.MIDDAY_DUNGEON)
        {
        }

        /// <summary>
        /// 获取推送原始数据项列表
        /// </summary>
        protected override IReadOnlyList<PushItemData> getItemDataList()
        {
            long curTimeMs = FpsAndPingMgr.instance.serverTimeTag;
            long startTimeMs = NPPlayer.instance.middayDungeonComp.startTimeMs;

            //如果已经过了开始时间，则加上一天
            if (startTimeMs < curTimeMs)
                startTimeMs = startTimeMs + 24 * 3600 * 1000;

            long leftTimeSec = (startTimeMs - curTimeMs) / 1000;
            return new List<PushItemData> { new PushItemData(leftTimeSec) };
        }
    }
}

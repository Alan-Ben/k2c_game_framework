using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 火星招募居民次数重置本地推送
    /// </summary>
    public class MarsPeopleImmigrantCountResetLocalPushDealer : _ARefDataLocalPushDealer
    {
        public MarsPeopleImmigrantCountResetLocalPushDealer() : base(ELocalPushType.MARS_PEOPLE_IMMIGRANT_COUNT_RESET)
        {
        }

        /// <summary>
        /// 获取推送原始数据项列表（距离跨天0点的剩余时间）
        /// </summary>
        protected override IReadOnlyList<PushItemData> getItemDataList()
        {
            long curTimeMs = FpsAndPingMgr.instance.serverTimeTag;
            long leftTimeSec = TimeUtil.getNextAssignTimeRemainMs(curTimeMs, 0, 0) / 1000;
            if (leftTimeSec <= 0)
                return null;

            return new List<PushItemData> { new PushItemData(leftTimeSec) };
        }
    }
}

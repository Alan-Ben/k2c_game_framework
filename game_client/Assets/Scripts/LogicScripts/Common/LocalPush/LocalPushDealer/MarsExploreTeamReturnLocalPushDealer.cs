using Common.MarsEnum;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星队伍派遣返回本地推送
    /// </summary>
    public class MarsExploreTeamReturnLocalPushDealer : _ARefDataLocalPushDealer
    {
        public MarsExploreTeamReturnLocalPushDealer() : base(ELocalPushType.MARS_EXPLORE_TEAM_RETURN)
        {
        }

        /// <summary>
        /// 获取推送原始数据项列表（每支处于返程中的队伍对应一条推送）
        /// </summary>
        protected override IReadOnlyList<PushItemData> getItemDataList()
        {
            List<MarsExploreTeamInfo> teamInfoList = new List<MarsExploreTeamInfo>();
            NPPlayer.instance.marsComp.exploreSubComponent.getTeamListNonAlloc(teamInfoList);
            if (teamInfoList.Count == 0)
                return null;

            List<PushItemData> itemList = null;
            for (int i = 0; i < teamInfoList.Count; i++)
            {
                MarsExploreTeamInfo teamInfo = teamInfoList[i];
                if (teamInfo == null)
                    continue;

                //剩余时间
                long leftTimeSec = 0;

                switch (teamInfo.state)
                {
                    //返程中
                    case EMarsExploreTeamState.BACK:
                        leftTimeSec = teamInfo.stateRemainTimeMs / 1000;
                        break;
                    //行军中
                    case EMarsExploreTeamState.MARCH:
                        if (teamInfo.exDataObj is MarsTeamEx_March marchExData && marchExData.data != null )
                        {
                            //不是前往采矿
                            if (marchExData.data.getTargetState() != (int) EMarsExploreTeamState.COLLECT)
                            {
                                //前往和返回的时间相同，这里用前往剩余时间加返回时间
                                leftTimeSec = teamInfo.stateRemainTimeMs / 1000 + (teamInfo.stateEndTime - teamInfo.stateStartTime) / 1000;
                            }
                            else
                            {
                                //前往剩余时间
                                leftTimeSec = teamInfo.stateRemainTimeMs / 1000;
                                //挖矿时间
                                if (marchExData.exMarchDataObj is MarsTeamEx_March_Mine marchMineExData)
                                {
                                    long speed = 0;
                                    long remainNum = 0;
                                    if (marchMineExData.refObj != null)
                                    {
                                        speed = NPPlayer.instance.marsComp.exploreSubComponent.calculateCollectSpeed(marchMineExData.refObj, teamInfo);
                                        remainNum = marchMineExData.remainNum;
                                    }
                                    else
                                    {
                                        MarsExploreMineInfo mineInfo =  NPPlayer.instance.marsComp.exploreSubComponent.getMineInfoByInstanceId(marchMineExData.instanceId);
                                        speed = NPPlayer.instance.marsComp.exploreSubComponent.calculateCollectSpeed(mineInfo.refObj, teamInfo);
                                        remainNum = mineInfo.remainNum;
                                    }
                                    leftTimeSec += (Mathf.FloorToInt(remainNum * 1000f / speed) / 1000);
                                }
                                //返回时间
                                leftTimeSec += ((teamInfo.stateEndTime - teamInfo.stateStartTime) / 1000);
                            }
                        }
                        break;
                    //采集中
                    case EMarsExploreTeamState.COLLECT:
                        if (teamInfo.exDataObj is MarsTeamEx_Collect collectExData && collectExData.data != null)
                        {
                            //挖矿时间
                            long speed = 0;
                            long remainNum = 0;
                            if (collectExData.refObj != null)
                            {
                                speed = NPPlayer.instance.marsComp.exploreSubComponent.calculateCollectSpeed(collectExData.refObj, teamInfo);
                                remainNum = collectExData.remainNum;
                            }
                            else
                            {
                                MarsExploreMineInfo mineInfo = NPPlayer.instance.marsComp.exploreSubComponent.getMineInfoByInstanceId(collectExData.instanceId);
                                speed = NPPlayer.instance.marsComp.exploreSubComponent.calculateCollectSpeed(mineInfo.refObj, teamInfo);
                                remainNum = mineInfo.remainNum;
                            }
                            leftTimeSec += (Mathf.FloorToInt(remainNum * 1000f / speed) / 1000);
                            //返回时间
                            leftTimeSec += (collectExData.data.getMarchTimeMS() / 1000);
                        }
                        break;
                }

                if (leftTimeSec <= 0)
                    continue;

                if (itemList == null)
                    itemList = new List<PushItemData>();

                object[] args = new object[] { teamInfo.name };
                itemList.Add(new PushItemData(leftTimeSec, null, args));
            }

            return itemList;
        }
    }
}


using System.Collections.Generic;
using NPCommon;
using GC2GS.p021_PlayerInfo;

namespace GOE
{
    //玩家信息相关
    public class NPGSWriter_021_PlayerInfoOp
    {
        public static GC2GS_021_005_ReqViewPlayerIcon make_005_ReqViewPlayerIcon(long _id)
        {
            GC2GS_021_005_ReqViewPlayerIcon protocol = new GC2GS_021_005_ReqViewPlayerIcon(_id);
            return protocol;
        }
        public static GC2GS_021_010_ReqViewPlayerIconBgk make_010_ReqViewPlayerIconBgk(long _id)
        {
            GC2GS_021_010_ReqViewPlayerIconBgk protocol = new GC2GS_021_010_ReqViewPlayerIconBgk(_id);
            return protocol;
        }
        public static GC2GS_021_015_ReqViewPlayerBubble make_015_ReqViewPlayerBubble(long _id)
        {
            GC2GS_021_015_ReqViewPlayerBubble protocol = new GC2GS_021_015_ReqViewPlayerBubble(_id);
            return protocol;
        }
        
        /// <summary>
        /// 领取成就步骤奖励
        /// </summary>
        /// <param name="_achieveId"></param>
        /// <param name="_step"></param>
        /// <returns></returns>
        public static GC2GS_021_025_ReqDoneAchieveStep make_025_ReqDoneAchieveStep(long _achieveId,int _step)
        {
            GC2GS_021_025_ReqDoneAchieveStep protocol = new GC2GS_021_025_ReqDoneAchieveStep(_achieveId,_step);
            return protocol;
        }
        
        /// <summary>
        /// 请求领取成就阶段奖励
        /// </summary>
        /// <param name="_achieveStepRewardId"></param>
        /// <returns></returns>
        public static GC2GS_021_026_ReqGainAchievePointReward make_026_ReqGainAchievePointReward(long _achieveStepRewardId)
        {
            GC2GS_021_026_ReqGainAchievePointReward protocol = new GC2GS_021_026_ReqGainAchievePointReward(_achieveStepRewardId);
            return protocol;
        }
        
        /// <summary>
        /// 每日签到刷新
        /// </summary>
        /// <returns></returns>
        public static GC2GS_021_027_ReqDailyCheckRefresh make_027_ReqDailyCheckRefresh()
        {
            GC2GS_021_027_ReqDailyCheckRefresh protocol = new GC2GS_021_027_ReqDailyCheckRefresh();
            return protocol;
        }

        /// <summary>
        /// 每日签到
        /// </summary>
        /// <returns></returns>
        public static GC2GS_021_028_ReqDailyCheck make_028_ReqDailyCheck(long _dessertId)
        {
            GC2GS_021_028_ReqDailyCheck protocol = new GC2GS_021_028_ReqDailyCheck(_dessertId);
            return protocol;
        }

        /// <summary>
        /// 每日签到累计奖励
        /// </summary>
        /// <param name="_days"></param>
        /// <returns></returns>
        public static GC2GS_021_029_ReqDailyCheckDrawReward make_029_ReqDailyCheckDrawReward()
        {
            GC2GS_021_029_ReqDailyCheckDrawReward protocol = new GC2GS_021_029_ReqDailyCheckDrawReward();
            return protocol;
        }

        public static GC2GS_021_030_ReqSendFriendApply make_030_ReqSendFriendApply(long _cid)
        {
            GC2GS_021_030_ReqSendFriendApply protocol = new GC2GS_021_030_ReqSendFriendApply(_cid);
            return protocol;
        }
        public static GC2GS_021_031_ReqDealFriendApply make_031_ReqDealFriendApply(bool _isAgree, long _cid)
        {
            GC2GS_021_031_ReqDealFriendApply protocol = new GC2GS_021_031_ReqDealFriendApply(_isAgree,_cid);
            return protocol;
        }

        public static GC2GS_021_032_ReqRemoveFriend make_032_ReqRemoveFriend(long _cid)
        {
            GC2GS_021_032_ReqRemoveFriend protocol = new GC2GS_021_032_ReqRemoveFriend(_cid);
            return protocol;
        }

        public static GC2GS_021_033_ReqFriendApplyExpired make_033_ReqFriendApplyExpired(long _applyCid)
        {
            GC2GS_021_033_ReqFriendApplyExpired protocol = new GC2GS_021_033_ReqFriendApplyExpired(_applyCid);
            return protocol;
        }

        public static GC2GS_021_034_ReqFriendRecommend make_034_ReqFriendRecommend( )
        {
            GC2GS_021_034_ReqFriendRecommend protocol = new GC2GS_021_034_ReqFriendRecommend();
            return protocol;
        }
        
        public static GC2GS_021_035_ReqCreateFriendGroup make_035_ReqCreateFriendGroup(string _groupName, List<long> _cidList)
        {
            GC2GS_021_035_ReqCreateFriendGroup protocol = new GC2GS_021_035_ReqCreateFriendGroup(_groupName, _cidList);
            return protocol;
        }
        public static GC2GS_021_036_ReqChgBelongFriendGroup make_036_ReqChgBelongFriendGroup(List<long> _cidList, long _groupDbId)
        {
            GC2GS_021_036_ReqChgBelongFriendGroup protocol = new GC2GS_021_036_ReqChgBelongFriendGroup(_cidList, _groupDbId);
            return protocol;
        }
        public static GC2GS_021_037_ReqDeleteFriendGroup make_037_ReqDeleteFriendGroup(long _groupDbId)
        {
            GC2GS_021_037_ReqDeleteFriendGroup protocol = new GC2GS_021_037_ReqDeleteFriendGroup(_groupDbId);
            return protocol;
        }
        public static GC2GS_021_038_ReqChgFriendGroupOrderList make_038_ReqChgFriendGroupOrderList(List<long> _groupIdList)
        {
            GC2GS_021_038_ReqChgFriendGroupOrderList protocol = new GC2GS_021_038_ReqChgFriendGroupOrderList(_groupIdList);
            return protocol;
        }
        public static GC2GS_021_039_ReqChgFriendGroupName make_039_ReqChgFriendGroupName(long _groupDbId, string _name)
        {
            GC2GS_021_039_ReqChgFriendGroupName protocol = new GC2GS_021_039_ReqChgFriendGroupName(_groupDbId, _name);
            return protocol;
        }
        
        public static GC2GS_021_049_ReqViewPlayerChatEmoteGroup make_049_ReqViewPlayerChatEmoteGroup(long _refId)
        {
            GC2GS_021_049_ReqViewPlayerChatEmoteGroup protocol = new GC2GS_021_049_ReqViewPlayerChatEmoteGroup(_refId);
            return protocol;
        }
    }
}

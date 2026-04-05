
using System.Collections.Generic;
using ALBasicProtocolPack;
using Common.LevyEnum;
using Common.PlayerShowObj;
using Common.WeekCardObj;
using GC2GS.p004_PlayerOp;

namespace GOE
{
    public static class NPGSWriter_004_PlayerOp
    {
        public static GC2GS.p004_PlayerOp.GC2GS_004_001_ReqLevelUp make_001_ReqLevelUp(int _curLvl)
        {
            GC2GS.p004_PlayerOp.GC2GS_004_001_ReqLevelUp protocol = new GC2GS.p004_PlayerOp.GC2GS_004_001_ReqLevelUp();
            protocol.setCurLvl(_curLvl);
            return protocol;
        }
        public static GC2GS.p004_PlayerOp.GC2GS_004_002_ReqGmCommand make_002_ReqGmCommand(string _cmd)
        {
            GC2GS.p004_PlayerOp.GC2GS_004_002_ReqGmCommand protocol = new GC2GS.p004_PlayerOp.GC2GS_004_002_ReqGmCommand();
            protocol.setCommand(_cmd);
            return protocol;
        }
        public static GC2GS.p004_PlayerOp.GC2GS_004_004_ReqSetName make_004_ReqSetName(string _name)
        {
            GC2GS.p004_PlayerOp.GC2GS_004_004_ReqSetName protocol = new GC2GS.p004_PlayerOp.GC2GS_004_004_ReqSetName();
            protocol.setNewName(_name);
            return protocol;
        }

        public static GC2GS.p004_PlayerOp.GC2GS_004_005_ReqSetIcon make_005_ReqSetIcon(long _id)
        {
            GC2GS.p004_PlayerOp.GC2GS_004_005_ReqSetIcon protocol = new GC2GS.p004_PlayerOp.GC2GS_004_005_ReqSetIcon();
            protocol.setIconId(_id);
            return protocol;
        }
        
        public static GC2GS.p004_PlayerOp.GC2GS_004_006_ReqSetPrefab make_006_ReqSetPrefab(long _refId)
        {
            GC2GS.p004_PlayerOp.GC2GS_004_006_ReqSetPrefab protocol = new GC2GS.p004_PlayerOp.GC2GS_004_006_ReqSetPrefab(_refId);
            return protocol;
        }

        public static GC2GS.p004_PlayerOp.GC2GS_004_007_ReqSetIconBgk make_007_ReqSetIconBgk(long _id)
        {
            GC2GS.p004_PlayerOp.GC2GS_004_007_ReqSetIconBgk protocol = new GC2GS.p004_PlayerOp.GC2GS_004_007_ReqSetIconBgk();
            protocol.setIconBgkId(_id);
            return protocol;
        }

        public static GC2GS.p004_PlayerOp.GC2GS_004_008_ReqSetDefault make_008_ReqSetDefault(string _name)
        {
            GC2GS.p004_PlayerOp.GC2GS_004_008_ReqSetDefault protocol = new GC2GS.p004_PlayerOp.GC2GS_004_008_ReqSetDefault(_name);
            return protocol;
        }

        public static GC2GS.p004_PlayerOp.GC2GS_004_009_ReqSetBubble make_009_ReqSetBubble(long _id)
        {
            GC2GS.p004_PlayerOp.GC2GS_004_009_ReqSetBubble protocol = new GC2GS.p004_PlayerOp.GC2GS_004_009_ReqSetBubble();
            protocol.setBubbleId(_id);
            return protocol;
        }
        
        public static GC2GS.p004_PlayerOp.GC2GS_004_010_ReqSomeOnePlayerInfo make_010_ReqSomeOnePlayerInfo(long _cid)
        {
            GC2GS.p004_PlayerOp.GC2GS_004_010_ReqSomeOnePlayerInfo protocol = new GC2GS.p004_PlayerOp.GC2GS_004_010_ReqSomeOnePlayerInfo();
            protocol.setCid(_cid);
            return protocol;
        }

        public static GC2GS.p004_PlayerOp.GC2GS_004_011_ReqSomeOnePlayerBriefInfo make_011_ReqSomeOnePlayerBriefInfo(long _cid)
        {
            GC2GS.p004_PlayerOp.GC2GS_004_011_ReqSomeOnePlayerBriefInfo protocol = new GC2GS.p004_PlayerOp.GC2GS_004_011_ReqSomeOnePlayerBriefInfo();
            protocol.setCid(_cid);
            return protocol;
        }
        
        /// <summary>
        /// 请求领取宝箱奖励
        /// </summary>
        /// <param name="_instanceId"></param>
        /// <returns></returns>
        public static GC2GS.p004_PlayerOp.GC2GS_004_023_ReqGainChatBox make_023_ReqGainChatBox(long _instanceId)
        {
            GC2GS.p004_PlayerOp.GC2GS_004_023_ReqGainChatBox protocol = new GC2GS.p004_PlayerOp.GC2GS_004_023_ReqGainChatBox();
            protocol.setInstanceId(_instanceId);
            return protocol;
        }

        /// <summary>
        /// 请求刷新宝箱状态
        /// </summary>
        /// <param name="_instanceId"></param>
        /// <returns></returns>
        public static GC2GS.p004_PlayerOp.GC2GS_004_024_ReqRefreshChatBoxStatus make_024_ReqRefreshChatBoxStatus(long _instanceId)
        {
            GC2GS.p004_PlayerOp.GC2GS_004_024_ReqRefreshChatBoxStatus protocol = new GC2GS.p004_PlayerOp.GC2GS_004_024_ReqRefreshChatBoxStatus();
            protocol.setInstanceId(_instanceId);
            return protocol;
        }
        
        public static GC2GS.p004_PlayerOp.GC2GS_004_014_ReqGainVisitOtherPlayerReward make_004_014_ReqGainVisitOtherPlayerReward()
        {
            GC2GS.p004_PlayerOp.GC2GS_004_014_ReqGainVisitOtherPlayerReward protocol = new GC2GS.p004_PlayerOp.GC2GS_004_014_ReqGainVisitOtherPlayerReward();
            return protocol;
        }

        public static GC2GS_004_015_ReqWeekCardSettleInfo make_015_ReqWeekCardSettleInfo()
        {
            GC2GS_004_015_ReqWeekCardSettleInfo protocol = new GC2GS_004_015_ReqWeekCardSettleInfo();
            return protocol;
        }
        public static GC2GS_004_016_ReqWeekCardChgSetting make_016_ReqWeekCardChgSetting(WeekCard_SingleSettingInfo _info)
        {
            GC2GS_004_016_ReqWeekCardChgSetting protocol = new GC2GS_004_016_ReqWeekCardChgSetting(_info);
            return protocol;
        }
        public static GC2GS_004_017_ReqWeekCardActiveFreeTrial make_017_ReqWeekCardActiveFreeTrial()
        {
            GC2GS_004_017_ReqWeekCardActiveFreeTrial protocol = new GC2GS_004_017_ReqWeekCardActiveFreeTrial();
            return protocol;
        }
        public static GC2GS_004_018_ReqWeekCardChgNPC make_018_ReqWeekCardChgNPC(CommonEnum.EWeekCardNPCType _npcType
            , long _npcId)
        {
            GC2GS_004_018_ReqWeekCardChgNPC protocol = new GC2GS_004_018_ReqWeekCardChgNPC(_npcType, _npcId);
            return protocol;
        }

        public static GC2GS_004_022_ReqSetCuteActor make_004_022_ReqSetCuteActor(long _cuteActorId)
        {
            GC2GS_004_022_ReqSetCuteActor protocol = new GC2GS_004_022_ReqSetCuteActor(_cuteActorId);
            return protocol;
        }

        public static GC2GS_004_031_ReqSetShieldPlayer make_004_031_ReqSetShieldPlayer(long _cid)
        {
            GC2GS_004_031_ReqSetShieldPlayer protocol = new GC2GS_004_031_ReqSetShieldPlayer(_cid);
            return protocol;
        }

        public static GC2GS_004_032_ReqUnsetShieldPlayer make_004_032_ReqUnsetShieldPlayer(long _cid)
        {
            GC2GS_004_032_ReqUnsetShieldPlayer protocol = new GC2GS_004_032_ReqUnsetShieldPlayer(_cid);
            return protocol;
        }

        /// <summary>
        /// 请求今天点赞过玩家信息
        /// </summary>
        /// <returns></returns>
        public static GC2GS_004_033_ReqTodayLikeCidInfo make_004_033_ReqTodayLikeCidInfo()
        {
            GC2GS_004_033_ReqTodayLikeCidInfo protocol = new GC2GS_004_033_ReqTodayLikeCidInfo();
            return protocol;
        }

        /// <summary>
        /// 给玩家详情点赞
        /// </summary>
        /// <returns></returns>
        public static GC2GS_004_034_ReqPlayerDetailLike make_004_034_ReqPlayerDetailLike(long _cid)
        {
            GC2GS_004_034_ReqPlayerDetailLike protocol = new GC2GS_004_034_ReqPlayerDetailLike(_cid);
            return protocol;
        }
        /// <summary>
        /// 请求玩家自己的被点赞总数
        /// </summary>
        /// <returns></returns>
        public static GC2GS_004_035_ReqSelfLikeCount make_004_035_ReqSelfLikeCount()
        {
            GC2GS_004_035_ReqSelfLikeCount protocol = new GC2GS_004_035_ReqSelfLikeCount();
            return protocol;
        }

        /// <summary>
        /// 请求领取VIP等级奖励
        /// </summary>
        /// <param name="_vipLevel"></param>
        /// <returns></returns>
        public static GC2GS_004_037_ReqDrawVipLevelReward make_004_037_ReqDrawVipLevelReward(long _vipLevel)
        {
            GC2GS_004_037_ReqDrawVipLevelReward protocol = new GC2GS_004_037_ReqDrawVipLevelReward((int)_vipLevel);
            return protocol;
        }

        /// <summary>
        /// 请求领取vip充值奖励
        /// </summary>
        /// <param name="_vipLevel"></param>
        /// <returns></returns>
        public static GC2GS_004_038_ReqDrawVipLevelRechargeReward make_004_038_ReqDrawVipLevelRechargeReward(long _vipLevel)
        {
            GC2GS_004_038_ReqDrawVipLevelRechargeReward protocol = new GC2GS_004_038_ReqDrawVipLevelRechargeReward((int)_vipLevel);
            return protocol;
        }
    }
}

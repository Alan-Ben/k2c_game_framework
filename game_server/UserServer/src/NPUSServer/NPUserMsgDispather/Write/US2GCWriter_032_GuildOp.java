package NPUSServer.NPUserMsgDispather.Write;

import Common.GuildCooperateObj.*;
import Common.GuildObj.*;
import Common.MarsObj.Mars_GuildBattleReportIdx;
import Common.RankObj.Rank_BaseItem;
import GS2GC.p032_GuildOp.*;

import java.util.List;

/**
 * 032 协议writer
 */
public class US2GCWriter_032_GuildOp
{
    public static GS2GC_032_001_RetCreateGuild make_001_RetCreateGuild()
    {
        return new GS2GC_032_001_RetCreateGuild();
    }

    public static GS2GC_032_002_RetJoinGuild make_002_RetJoinGuild()
    {
        return new GS2GC_032_002_RetJoinGuild();
    }

    public static GS2GC_032_003_RetRandomJoinGuild make_003_RetRandomJoinGuild()
    {
        return new GS2GC_032_003_RetRandomJoinGuild();
    }

    public static GS2GC_032_004_RetOtherGuildInfo make_004_RetOtherGuildInfo(Guild_ShowInfo _showInfo, List<Guild_MemberBaseInfo> _memberList)
    {
        GS2GC_032_004_RetOtherGuildInfo proto = new GS2GC_032_004_RetOtherGuildInfo();
        proto.setShowInfo(_showInfo);
        proto.getMemberList().addAll(_memberList);
        return proto;
    }

    public static GS2GC_032_005_RetTransferGuild make_005_RetTransferGuild()
    {
        return new GS2GC_032_005_RetTransferGuild();
    }

    public static GS2GC_032_006_RetDissolveGuild make_006_RetDissolveGuild()
    {
        return new GS2GC_032_006_RetDissolveGuild();
    }

    public static GS2GC_032_007_RetGuildPositionAppoint make_007_RetGuildPositionAppoint()
    {
        return new GS2GC_032_007_RetGuildPositionAppoint();
    }

    public static GS2GC_032_008_RetChgGuildFlag make_008_RetChgGuildFlag()
    {
        return new GS2GC_032_008_RetChgGuildFlag();
    }

    public static GS2GC_032_009_RetChgGuildName make_009_RetChgGuildName()
    {
        return new GS2GC_032_009_RetChgGuildName();
    }

    public static GS2GC_032_010_RetChgGuildDeclaration make_010_RetChgGuildDeclaration()
    {
        return new GS2GC_032_010_RetChgGuildDeclaration();
    }

    public static GS2GC_032_011_RetChgGuildAnnouncement make_011_RetChgGuildAnnouncement()
    {
        return new GS2GC_032_011_RetChgGuildAnnouncement();
    }

    public static GS2GC_032_012_RetSetGuildJoinType make_012_RetSetGuildJoinType()
    {
        return new GS2GC_032_012_RetSetGuildJoinType();
    }

    public static GS2GC_032_013_RetProcessGuildJoinRequest make_013_RetProcessGuildJoinRequest()
    {
        return new GS2GC_032_013_RetProcessGuildJoinRequest();
    }

    public static GS2GC_032_014_RetGuildKickOutMember make_014_RetGuildKickOutMember()
    {
        return new GS2GC_032_014_RetGuildKickOutMember();
    }

    public static GS2GC_032_015_RetGuildBroadcastMessage make_015_RetGuildBroadcastMessage()
    {
        return new GS2GC_032_015_RetGuildBroadcastMessage();
    }

    public static GS2GC_032_016_RetLeaveGuild make_016_RetLeaveGuild()
    {
        return new GS2GC_032_016_RetLeaveGuild();
    }

    public static GS2GC_032_017_RetGuildConstruct make_017_RetGuildConstruct()
    {
        return new GS2GC_032_017_RetGuildConstruct();
    }

    public static GS2GC_032_018_RetGuildImpeachLeader make_018_RetGuildImpeachLeader()
    {
        return new GS2GC_032_018_RetGuildImpeachLeader();
    }

    public static GS2GC_032_019_RetMemberContribute make_019_RetMemberContribute(Guild_MemberContributeInfo _proto)
    {
        return new GS2GC_032_019_RetMemberContribute(_proto);
    }

    public static GS2GC_032_021_RetSearchGuild make_021_RetSearchGuild(Guild_ShowInfo _showInfo, Rank_BaseItem _rankBaseInfo)
    {
        GS2GC_032_021_RetSearchGuild proto = new GS2GC_032_021_RetSearchGuild();
        proto.setShowInfo(_showInfo);
        if (_rankBaseInfo != null)
            proto.setRankBaseInfo(_rankBaseInfo);
        return proto;
    }

    public static GS2GC_032_022_RetGuildRankList make_022_RetGuildRankList(List<Rank_BaseItem> _rankBaseList)
    {
        GS2GC_032_022_RetGuildRankList proto = new GS2GC_032_022_RetGuildRankList();
        proto.getRankList().addAll(_rankBaseList);
        return proto;
    }

    public static GS2GC_032_023_RetGuildJoinRequestAKeyDeal make_023_RetGuildJoinRequestAKeyDeal()
    {
        return new GS2GC_032_023_RetGuildJoinRequestAKeyDeal();
    }

    public static GS2GC_032_024_RetCancelSelfJoinRequest make_024_RetCancelSelfJoinRequest()
    {
        return new GS2GC_032_024_RetCancelSelfJoinRequest();
    }

    public static GS2GC_032_025_RetCancelLeaderImpeachEvent make_025_RetCancelLeaderImpeachEvent()
    {
        return new GS2GC_032_025_RetCancelLeaderImpeachEvent();
    }

    public static GS2GC_032_026_RetApproveLeaderImpeachEvent make_026_RetApproveLeaderImpeachEvent()
    {
        return new GS2GC_032_026_RetApproveLeaderImpeachEvent();
    }

    public static GS2GC_032_027_RetOpenRecruit make_027_RetOpenRecruit()
    {
        return new GS2GC_032_027_RetOpenRecruit();
    }

    public static GS2GC_032_033_RetGuildEntrust make_033_RetGuildEntrust(int _critMul) {
        GS2GC_032_033_RetGuildEntrust proto = new GS2GC_032_033_RetGuildEntrust();
        proto.setCritMul(_critMul);
        return proto;
    }

    public static GS2GC_032_034_RetGuildDispatchHero make_034_RetGuildDispatchHero() {
        return new GS2GC_032_034_RetGuildDispatchHero();
    }

    public static GS2GC_032_035_RetGuildAllMemberEntrustInfo make_035_RetGuildAllMemberEntrustInfo(List<Guild_MemberEntrustInfo> _infoList) {
        GS2GC_032_035_RetGuildAllMemberEntrustInfo proto = new GS2GC_032_035_RetGuildAllMemberEntrustInfo();
        proto.getInfoList().addAll(_infoList);
        return proto;
    }

    public static GS2GC_032_037_RetDrawConstructReward make_037_RetDrawConstructReward() {
        return new GS2GC_032_037_RetDrawConstructReward();
    }

    public static GS2GC_032_039_RetGuildDispatchHeroList make_039_RetGuildDispatchHeroList(List<Guild_DispatchHeroDetailInfo> _list)
    {
        GS2GC_032_039_RetGuildDispatchHeroList proto = new GS2GC_032_039_RetGuildDispatchHeroList();
        proto.getInfoList().addAll(_list);
        return proto;
    }

    public static GS2GC_032_040_RetGuildLogList make_040_RetGuildLogList(List<Guild_LogInfo> _list)
    {
        GS2GC_032_040_RetGuildLogList proto = new GS2GC_032_040_RetGuildLogList();
        proto.getInfoList().addAll(_list);
        return proto;
    }

    public static GS2GC_032_059_OnGuildEventAdd make_059_OnGuildEventAdd(Guild_EventInfo _eventInfo)
    {
        GS2GC_032_059_OnGuildEventAdd proto = new GS2GC_032_059_OnGuildEventAdd();
        proto.setEventInfo(_eventInfo);
        return proto;
    }

    public static GS2GC_032_060_OnGuildEventChg make_060_OnGuildEventChg(Guild_EventInfo _eventInfo)
    {
        GS2GC_032_060_OnGuildEventChg proto = new GS2GC_032_060_OnGuildEventChg();
        proto.setEventInfo(_eventInfo);
        return proto;
    }

    public static GS2GC_032_061_OnGuildEventRemove make_061_OnGuildEventRemove(long _eventId)
    {
        return new GS2GC_032_061_OnGuildEventRemove(_eventId);
    }

    public static GS2GC_032_064_OnGuildJoinRequestRemove make_064_OnGuildJoinRequestRemove(long _requestDbId)
    {
        GS2GC_032_064_OnGuildJoinRequestRemove proto = new GS2GC_032_064_OnGuildJoinRequestRemove();
        proto.getRequestDbIdList().add(_requestDbId);
        return proto;
    }

    public static GS2GC_032_064_OnGuildJoinRequestRemove make_064_OnGuildJoinRequestRemove(List<Long> _requestDbIdList)
    {
        GS2GC_032_064_OnGuildJoinRequestRemove proto = new GS2GC_032_064_OnGuildJoinRequestRemove();
        proto.getRequestDbIdList().addAll(_requestDbIdList);
        return proto;
    }

    public static GS2GC_032_071_OnGuildEntrustChg make_071_OnGuildEntrustChg(Guild_EntrustInfo _data)
    {
        GS2GC_032_071_OnGuildEntrustChg proto = new GS2GC_032_071_OnGuildEntrustChg();
        proto.setData(_data);
        return proto;
    }

    public static GS2GC_032_072_OnGuildDispatchChg make_072_OnGuildDispatchChg(Guild_AttrDispatchInfo _data)
    {
        GS2GC_032_072_OnGuildDispatchChg proto = new GS2GC_032_072_OnGuildDispatchChg();
        proto.setData(_data);
        return proto;
    }

    public static GS2GC_032_073_OnSelfDailyDataChg make_073_OnSelfDailyDataChg(Guild_MemberDailyData _data)
    {
        GS2GC_032_073_OnSelfDailyDataChg proto = new GS2GC_032_073_OnSelfDailyDataChg();
        proto.setData(_data);
        return proto;
    }

    public static GS2GC_032_041_RetGuildCooperateAttackLogList make_041_RetGuildCooperateAttackLogList(List<GuildCooperate_AttackLog> _attackLogList)
    {
        GS2GC_032_041_RetGuildCooperateAttackLogList proto = new GS2GC_032_041_RetGuildCooperateAttackLogList();
        proto.getAttackLogList().addAll(_attackLogList);
        return proto;
    }

    public static GS2GC_032_042_RetSetRecommendRewardPoint make_042_RetSetRecommendRewardPoint()
    {
        return new GS2GC_032_042_RetSetRecommendRewardPoint();
    }

    public static GS2GC_032_043_RetDrawRewardPointReward make_043_RetDrawRewardPointReward()
    {
        return new GS2GC_032_043_RetDrawRewardPointReward();
    }

    public static GS2GC_032_044_RetAttackPropertyPoint make_044_RetAttackPropertyPoint(long _damageHp, NPCommon.CommonObj.NPItemCollector _collector)
    {
        GS2GC_032_044_RetAttackPropertyPoint proto = new GS2GC_032_044_RetAttackPropertyPoint();
        proto.setAttackHp(_damageHp);
        _collector.fillProtoList(proto.getItemList());
        return proto;
    }

    public static GS2GC_032_045_RetAddHeroRecoveryCount make_045_RetAddHeroRecoveryCount()
    {
        return new GS2GC_032_045_RetAddHeroRecoveryCount();
    }

    public static GS2GC_032_046_RetGuildCooperateDamageRank make_046_RetGuildCooperateDamageRank(List<Rank_BaseItem> _rankList)
    {
        GS2GC_032_046_RetGuildCooperateDamageRank proto = new GS2GC_032_046_RetGuildCooperateDamageRank();
        if (_rankList != null)
        {
            proto.getRankList().addAll(_rankList);
        }
        return proto;
    }

    public static GS2GC_032_047_RetGuildIconShow make_047_RetGuildIconShow(Guild_IconShow _info)
    {
    	GS2GC_032_047_RetGuildIconShow proto = new GS2GC_032_047_RetGuildIconShow();
        proto.setInfo(_info);
        return proto;
    }

    public static GS2GC_032_075_OnRecommendRewardPointChange make_075_OnRecommendRewardPointChange(GuildCooperate_RewardPointPos _recommendPos)
    {
        GS2GC_032_075_OnRecommendRewardPointChange proto = new GS2GC_032_075_OnRecommendRewardPointChange();
        proto.setRecommendPos(_recommendPos);
        return proto;
    }

    public static GS2GC_032_076_OnPropertyPointInfoChange make_076_OnPropertyPointInfoChange(GuildCooperate_RewardPointPos _pos, GuildCooperate_PropertyPointInfo _propertyPointInfo)
    {
        GS2GC_032_076_OnPropertyPointInfoChange proto = new GS2GC_032_076_OnPropertyPointInfoChange();
        proto.setPos(_pos);
        proto.setPropertyPointInfo(_propertyPointInfo);
        return proto;
    }

    public static GS2GC_032_077_OnRewardPointDefeat make_077_OnRewardPointDefeat(GuildCooperate_RewardPointPos _pos, long _leaderCid)
    {
        GS2GC_032_077_OnRewardPointDefeat proto = new GS2GC_032_077_OnRewardPointDefeat();
        proto.setPos(_pos);
        proto.setLeaderCid(_leaderCid);
        return proto;
    }

    public static GS2GC_032_078_OnHadDrawRewardPointListChg make_078_OnHadDrawRewardPointListChg(GuildCooperate_HadDrawRewardPointList _data)
    {
        GS2GC_032_078_OnHadDrawRewardPointListChg proto = new GS2GC_032_078_OnHadDrawRewardPointListChg();
        proto.setData(_data);
        return proto;
    }

    public static GS2GC_032_079_OnHeroUseInfoChange make_079_OnHeroUseInfoChange(GuildCooperate_HeroUseInfo _heroUseInfo)
    {
        GS2GC_032_079_OnHeroUseInfoChange proto = new GS2GC_032_079_OnHeroUseInfoChange();
        proto.setHeroUseInfo(_heroUseInfo);
        return proto;
    }

    public static GS2GC_032_080_OnRewardPointUnlock make_080_OnRewardPointUnlock(List<GuildCooperate_RewardPointPos> _posList)
    {
        GS2GC_032_080_OnRewardPointUnlock proto = new GS2GC_032_080_OnRewardPointUnlock();
        proto.getPosList().addAll(_posList);
        return proto;
    }

    public static GS2GC_032_048_RetGuildMarsBattleReportList make_048_RetGuildMarsBattleReportList(List<Mars_GuildBattleReportIdx> _reportList)
    {
        GS2GC_032_048_RetGuildMarsBattleReportList proto = new GS2GC_032_048_RetGuildMarsBattleReportList();
        proto.getReportList().addAll(_reportList);
        return proto;
    }

    public static GS2GC_032_081_OnGuildMarsBattleReportAdd make_081_OnGuildMarsBattleReportAdd(long _id)
    {
        GS2GC_032_081_OnGuildMarsBattleReportAdd proto = new GS2GC_032_081_OnGuildMarsBattleReportAdd();
        proto.setId(_id);
        return proto;
    }
}

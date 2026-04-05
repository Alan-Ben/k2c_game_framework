package NPUSServer.NPUserMsgDispather.Write;

import Common.GuildObj.Guild_MineShareInfo;
import Common.MarsObj.Mars_ExplorePVPLogIdx;
import Common.MarsObj.Mars_MineDynamic;
import Common.ServerObj.ServerObj_MarsMine;
import GS2GC.p041_MarsExploreOp.*;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.NPUSUserMgr.UserComp.MarsMineComp.MarsMineInfo;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreEvent._AMarsExploreEventInfo;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.MarsExploreTeam;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.MarsExploreInfo;

import java.util.List;

/**
 * 40 - 火星居民系统
 * @author mj
 *
 */
public class US2GCWriter_041_MarsExploreOp
{
    public static GS2GC_041_001_RetBuildExploreEventByPos make_001_RetBuildExploreEventByPos()
    {
    	GS2GC_041_001_RetBuildExploreEventByPos proto = new GS2GC_041_001_RetBuildExploreEventByPos();
    	
        return proto;
    }
    
    public static GS2GC_041_002_RetBuildExploreEvent make_002_RetBuildExploreEvent()
    {
    	GS2GC_041_002_RetBuildExploreEvent proto = new GS2GC_041_002_RetBuildExploreEvent();
    	
        return proto;
    }
    
    public static GS2GC_041_003_RetSetExploreTeamHero make_003_RetSetExploreTeamHero()
    {
    	GS2GC_041_003_RetSetExploreTeamHero proto = new GS2GC_041_003_RetSetExploreTeamHero();
    	
        return proto;
    }
    
    public static GS2GC_041_004_RetSetExploreTeamName make_004_RetSetExploreTeamName()
    {
    	GS2GC_041_004_RetSetExploreTeamName proto = new GS2GC_041_004_RetSetExploreTeamName();
    	
        return proto;
    }
    
    public static GS2GC_041_005_RetStartDealExploreEvent make_005_RetStartDealExploreEvent()
    {
    	GS2GC_041_005_RetStartDealExploreEvent proto = new GS2GC_041_005_RetStartDealExploreEvent();
    	
        return proto;
    }
    
    public static GS2GC_041_006_RetGetBattleDoneReward make_006_RetGetBattleDoneReward(NPPlayerContext _context)
    {
    	GS2GC_041_006_RetGetBattleDoneReward proto = new GS2GC_041_006_RetGetBattleDoneReward();
    	_context.getCollector().fillProtoList(proto.getItemList());
    	
        return proto;
    }
    
    public static GS2GC_041_007_RetStartExploreTeamRepair make_007_RetStartExploreTeamRepair()
    {
    	GS2GC_041_007_RetStartExploreTeamRepair proto = new GS2GC_041_007_RetStartExploreTeamRepair();
    	
        return proto;
    }
    
    public static GS2GC_041_008_RetNoticeExploreTeamState make_008_RetNoticeExploreTeamState()
    {
    	GS2GC_041_008_RetNoticeExploreTeamState proto = new GS2GC_041_008_RetNoticeExploreTeamState();
    	
        return proto;
    }
    
    public static GS2GC_041_009_RetGetBossDoneReward make_009_RetGetBossDoneReward(NPPlayerContext _context)
    {
    	GS2GC_041_009_RetGetBossDoneReward proto = new GS2GC_041_009_RetGetBossDoneReward();
    	_context.getCollector().fillProtoList(proto.getItemList());
    	
        return proto;
    }
    
    public static GS2GC_041_010_RetForwardCollectMine make_010_RetForwardCollectMine()
    {
    	GS2GC_041_010_RetForwardCollectMine proto = new GS2GC_041_010_RetForwardCollectMine();
    	
        return proto;
    }
    
    public static GS2GC_041_011_RetMarsMineInfo make_011_RetMarsMineInfo(ServerObj_MarsMine _usMineProto)
    {
    	GS2GC_041_011_RetMarsMineInfo proto = new GS2GC_041_011_RetMarsMineInfo();
        proto.setInfo(makeMineDynamicInfo(_usMineProto));
        return proto;
    }

    public static GS2GC_041_012_RetMarsTeamBack make_012_RetMarsTeamBack()
    {
    	GS2GC_041_012_RetMarsTeamBack proto = new GS2GC_041_012_RetMarsTeamBack();
    	
        return proto;
    }

    public static GS2GC_041_013_RetNoticeMarsMine make_013_RetNoticeMarsMine(ServerObj_MarsMine _usMineProto)
    {
        GS2GC_041_013_RetNoticeMarsMine proto = new GS2GC_041_013_RetNoticeMarsMine();
        proto.setInfo(makeMineDynamicInfo(_usMineProto));
        return proto;
    }

    public static GS2GC_041_014_RetGetCollectPVPLogIdxList make_014_RetGetCollectPVPLogIdxList(List<Mars_ExplorePVPLogIdx> _list)
    {
    	GS2GC_041_014_RetGetCollectPVPLogIdxList proto = new GS2GC_041_014_RetGetCollectPVPLogIdxList();
        for(int i = 0; i < _list.size(); i++)
        {
        	Mars_ExplorePVPLogIdx idx = _list.get(i);
        	if(null == idx)
        		continue;
        	
        	proto.addIdxList(idx);
        }
    	
        return proto;
    }

    public static GS2GC_041_015_RetGetTempBuildingQueue make_015_RetGetTempBuildingQueue()
    {
    	GS2GC_041_015_RetGetTempBuildingQueue proto = new GS2GC_041_015_RetGetTempBuildingQueue();
    	
        return proto;
    }
    
    public static GS2GC_041_016_RetSetTempRepairDone make_016_RetSetTempRepairDone()
    {
    	GS2GC_041_016_RetSetTempRepairDone proto = new GS2GC_041_016_RetSetTempRepairDone();

        return proto;
    }

    /**
     * 构造取消维修响应协议
     */
    public static GS2GC_041_020_RetCancelExploreTeamRepair make_020_RetCancelExploreTeamRepair()
    {
    	GS2GC_041_020_RetCancelExploreTeamRepair proto = new GS2GC_041_020_RetCancelExploreTeamRepair();

        return proto;
    }

    /**
     * 构造开始并立即完成维修响应协议
     */
    public static GS2GC_041_021_RetStartAndFinishRepair make_021_RetStartAndFinishRepair()
    {
    	GS2GC_041_021_RetStartAndFinishRepair proto = new GS2GC_041_021_RetStartAndFinishRepair();

        return proto;
    }

    public static GS2GC_041_017_RetGuildMineShareList make_017_ReqGuildMineShareList(List<Guild_MineShareInfo> _mineShareList)
    {
    	GS2GC_041_017_RetGuildMineShareList proto = new GS2GC_041_017_RetGuildMineShareList();
        proto.getMineShareList().addAll(_mineShareList);
        return proto;
    }

    public static GS2GC_041_018_RetGuildMateForwardCollectMine make_018_RetGuildMateForwardCollectMine()
    {
        return new GS2GC_041_018_RetGuildMateForwardCollectMine();
    }

    public static GS2GC_041_019_RetGuildShareMineInfo make_019_RetGuildShareMineInfo(ServerObj_MarsMine _usMineProto)
    {
        GS2GC_041_019_RetGuildShareMineInfo proto = new GS2GC_041_019_RetGuildShareMineInfo();
        proto.setInfo(makeMineDynamicInfo(_usMineProto));
        return proto;
    }

    public static GS2GC_041_024_RetGuildShareMineHadAttackByOthersTag make_024_RetGuildShareMineHadAttackByOthersTag(boolean _hadTag)
    {
        return new GS2GC_041_024_RetGuildShareMineHadAttackByOthersTag(_hadTag);
    }

    public static Mars_MineDynamic makeMineDynamicInfo(ServerObj_MarsMine _usMineProto)
    {
        Mars_MineDynamic dy = new Mars_MineDynamic();
        dy.setId(_usMineProto.getId());
        dy.setRefId(_usMineProto.getRefId());
        dy.setSerialize(_usMineProto.getSerialize());
        dy.setOccupiedCid(_usMineProto.getCid());
        dy.setOccupiedTeamId(_usMineProto.getTeamId());
        dy.setRemainNum(_usMineProto.getRemainNum());
        dy.setOccupiedMs(_usMineProto.getStartCollectMs());
        dy.setCollectSpeed(_usMineProto.getCollectSpeed());
        dy.setTroopNum(_usMineProto.getTroopNum());
        dy.setTeamPower(_usMineProto.getTeamPower());
        return dy;
    }

    public static GS2GC_041_050_OnExploreChg make_050_OnExploreChg(MarsExploreInfo _info)
    {
    	GS2GC_041_050_OnExploreChg proto = new GS2GC_041_050_OnExploreChg();
    	proto.setExplore(_info.toProto());
    	
        return proto;
    }

    public static GS2GC_041_051_OnExploreEventAdd make_051_OnExploreEventAdd(_AMarsExploreEventInfo _info)
    {
    	GS2GC_041_051_OnExploreEventAdd proto = new GS2GC_041_051_OnExploreEventAdd();
    	proto.setInfo(_info.toProto());
    	
        return proto;
    }

    public static GS2GC_041_052_OnExploreEventDel make_052_OnExploreEventDel(long _id)
    {
    	GS2GC_041_052_OnExploreEventDel proto = new GS2GC_041_052_OnExploreEventDel();
    	proto.setId(_id);
    	
        return proto;
    }

    public static GS2GC_041_055_OnExploreEventDone make_055_OnExploreEventDone(long _id)
    {
    	GS2GC_041_055_OnExploreEventDone proto = new GS2GC_041_055_OnExploreEventDone();
    	proto.setId(_id);
    	
        return proto;
    }

    public static GS2GC_041_056_OnExploreTeamNameChg make_056_OnExploreTeamNameChg(MarsExploreTeam _team)
    {
    	GS2GC_041_056_OnExploreTeamNameChg proto = new GS2GC_041_056_OnExploreTeamNameChg();
    	proto.setTeamId(_team.getTeamId());
    	proto.setName(_team.getName());
    	
        return proto;
    }

    public static GS2GC_041_057_OnExploreTeamHeroChg make_057_OnExploreTeamHeroChg(MarsExploreTeam _team)
    {
    	GS2GC_041_057_OnExploreTeamHeroChg proto = new GS2GC_041_057_OnExploreTeamHeroChg();
    	proto.setTeamId(_team.getTeamId());
    	proto.getHeroIdList().addAll(_team.getHeroIdList());
    	
        return proto;
    }

    public static GS2GC_041_058_OnExploreTeamState make_058_OnExploreTeamState(MarsExploreTeam _team)
    {
    	GS2GC_041_058_OnExploreTeamState proto = new GS2GC_041_058_OnExploreTeamState();
    	proto.setTeamId(_team.getTeamId());
    	proto.setState(_team.toStateProto());
    	
        return proto;
    }

    public static GS2GC_041_059_OnMineAdd make_059_OnMineAdd(MarsMineInfo _info)
    {
    	GS2GC_041_059_OnMineAdd proto = new GS2GC_041_059_OnMineAdd();
    	proto.setIdx(_info.toIdxProto());
    	
        return proto;
    }

    public static GS2GC_041_060_OnMineDel make_060_OnMineDel(long _id)
    {
    	GS2GC_041_060_OnMineDel proto = new GS2GC_041_060_OnMineDel();
    	proto.setId(_id);
    	
        return proto;
    }
    
    public static GS2GC_041_061_OnMarsExplorePVPLogAdd make_061_OnMarsExplorePVPLogAdd(long _createdAt)
    {
    	GS2GC_041_061_OnMarsExplorePVPLogAdd proto = new GS2GC_041_061_OnMarsExplorePVPLogAdd();
    	proto.setCreatedAt(_createdAt);
    	
        return proto;
    }

    public static GS2GC_041_062_OnExploreTeamLossValueChg make_062_OnExploreTeamLossValueChg(MarsExploreTeam _team)
    {
    	GS2GC_041_062_OnExploreTeamLossValueChg proto = new GS2GC_041_062_OnExploreTeamLossValueChg();
    	proto.setTeamId(_team.getTeamId());
    	proto.setLossValue(_team.getLossValue());
    	
        return proto;
    }
    
    public static GS2GC_041_063_OnGuildMarsMineShareAdd make_063_OnGuildMarsMineShareAdd(GuildInfo _guild)
    {
    	GS2GC_041_063_OnGuildMarsMineShareAdd proto = new GS2GC_041_063_OnGuildMarsMineShareAdd();
    	proto.setId(_guild.getMarsMineShareMgr().getMaxId());

        return proto;
    }

    public static GS2GC_041_025_RetShareMarsMineToGuildChat make_025_RetShareMarsMineToGuildChat()
    {
        GS2GC_041_025_RetShareMarsMineToGuildChat proto = new GS2GC_041_025_RetShareMarsMineToGuildChat();
        return proto;
    }
}
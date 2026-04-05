package NPUSServer.NPUserMsgDispather.Write;

import GS2GC.p037_GuildDungeonOp.*;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Guild.GuildDungeon.GuildDungeonInstanceInfo;
import NPUSServer.Guild.GuildDungeon.GuildDungeonMonsterInfo;
import NPUSServer.Guild.GuildDungeon.GuildDungeonSetInfo;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.NPUSUserMgr.UserComp.GuildDungeonComp.GuildDungeonHeroFightInfo;

/**
 * 37 - 联盟副本
 * @author mj
 *
 */
public class US2GCWriter_037_GuildDungeonOp
{
    public static GS2GC_037_001_RetDungeontGlobalSet make_001_RetDungeontGlobalSet(GuildInfo _guild)
    {
    	GS2GC_037_001_RetDungeontGlobalSet proto = new GS2GC_037_001_RetDungeontGlobalSet();
    	_guild.getDungeonMgr().getSetMgr().makeAutoStartDungeonIdList(proto.getAutoStartDungeonIdList());
    	proto.setAutoHour(_guild.getDungeonMgr().getGlobalSetInfo().getAutoHour());
    	proto.setAutoMin(_guild.getDungeonMgr().getGlobalSetInfo().getAutoMin());
    	
        return proto;
    }
    
    public static GS2GC_037_002_RetSetAutoStartDungeon make_002_RetSetAutoStartDungeon()
    {
    	GS2GC_037_002_RetSetAutoStartDungeon proto = new GS2GC_037_002_RetSetAutoStartDungeon();
    	
        return proto;
    }
    
    public static GS2GC_037_003_RetStartDungeon make_003_RetStartDungeon()
    {
    	GS2GC_037_003_RetStartDungeon proto = new GS2GC_037_003_RetStartDungeon();
    	
        return proto;
    }
    
    public static GS2GC_037_004_RetUpgradeDungeonLvl make_004_RetUpgradeDungeonLvl(GuildDungeonSetInfo _setInfo)
    {
    	GS2GC_037_004_RetUpgradeDungeonLvl proto = new GS2GC_037_004_RetUpgradeDungeonLvl();
    	proto.setUpgradeLvl(_setInfo.getLvl());
    	
        return proto;
    }
    
    public static GS2GC_037_005_RetAttackDungeon make_005_RetAttackDungeon(boolean _isKilled, NPPlayerContext _context)
    {
    	GS2GC_037_005_RetAttackDungeon proto = new GS2GC_037_005_RetAttackDungeon();
    	proto.setIsKilled(_isKilled);
    	_context.getCollector().fillProtoList(proto.getGainItemList());
    	
        return proto;
    }
    
    public static GS2GC_037_006_RetRecoverHeroFight make_006_RetRecoverHeroFight()
    {
    	GS2GC_037_006_RetRecoverHeroFight proto = new GS2GC_037_006_RetRecoverHeroFight();
    	
        return proto;
    }
    
    public static GS2GC_037_007_RetDamageRank make_007_RetDamageRank(long _cid, int _page, int _pageNum, GuildInfo _guild)
    {
    	GS2GC_037_007_RetDamageRank proto = new GS2GC_037_007_RetDamageRank();
    	_guild.getDungeonMgr().getDamageRank().makeProto(_cid, _page, _pageNum, proto);
    	
        return proto;
    }
    
    public static GS2GC_037_008_RetGainAllDungeonReward make_008_RetGainAllDungeonReward()
    {
    	GS2GC_037_008_RetGainAllDungeonReward proto = new GS2GC_037_008_RetGainAllDungeonReward();
    	
        return proto;
    }
    
    public static GS2GC_037_010_RetGetDungeonLogList make_010_RetGetDungeonLogList(GuildDungeonInstanceInfo _instanceInfo)
    {
    	GS2GC_037_010_RetGetDungeonLogList proto = new GS2GC_037_010_RetGetDungeonLogList();
    	_instanceInfo.getLogMgr().makeProto(proto.getLogList());
    	
        return proto;
    }
    
    public static GS2GC_037_011_RetGainDungeonReward make_011_RetGainDungeonReward()
    {
    	GS2GC_037_011_RetGainDungeonReward proto = new GS2GC_037_011_RetGainDungeonReward();
    	
        return proto;
    }
    
    public static GS2GC_037_012_RetSetTagMonsterList make_012_RetSetTagMonsterList()
    {
    	GS2GC_037_012_RetSetTagMonsterList proto = new GS2GC_037_012_RetSetTagMonsterList();
    	
        return proto;
    }
    
    public static GS2GC_037_050_OnDungeonSetChg make_050_OnDungeonSetChg(GuildDungeonSetInfo _setInfo)
    {
    	GS2GC_037_050_OnDungeonSetChg proto = new GS2GC_037_050_OnDungeonSetChg();
    	proto.setSetInfo(_setInfo.toProto());
    	
        return proto;
    }
    
    public static GS2GC_037_051_OnDungeonInstanceChg make_051_OnDungeonInstanceChg(GuildDungeonInstanceInfo _instanceInfo)
    {
    	GS2GC_037_051_OnDungeonInstanceChg proto = new GS2GC_037_051_OnDungeonInstanceChg();
    	proto.setInstanceInfo(_instanceInfo.toProto());
    	
        return proto;
    }
    
    public static GS2GC_037_052_OnDungeonMonsterChg make_052_OnDungeonMonsterChg(GuildDungeonMonsterInfo _monster)
    {
    	GS2GC_037_052_OnDungeonMonsterChg proto = new GS2GC_037_052_OnDungeonMonsterChg();
    	proto.setId(_monster.getDungeon().getId());
    	proto.setMonsterId(_monster.getMonsterId());
    	proto.setHp(_monster.getHp());
    	
        return proto;
    }
    
    public static GS2GC_037_053_OnDungeonGainedRewardChg make_053_OnDungeonGainedRewardChg(GuildInfo _guild, long _cid)
    {
    	GS2GC_037_053_OnDungeonGainedRewardChg proto = new GS2GC_037_053_OnDungeonGainedRewardChg();
		_guild.getDungeonMgr().getInstanceMgr().makeGainedRewardList(_cid, proto.getGainedRewardMonsterIdList());
    	
        return proto;
    }
    
    public static GS2GC_037_054_OnDungeonHeroFightChg make_054_OnDungeonHeroFightChg(GuildDungeonHeroFightInfo _info)
    {
    	GS2GC_037_054_OnDungeonHeroFightChg proto = new GS2GC_037_054_OnDungeonHeroFightChg();
		proto.setInfo(_info.toProto());
    	
        return proto;
    }
    
    public static GS2GC_037_055_OnDungeonTagMonsterChg make_055_OnDungeonTagMonsterChg(GuildDungeonInstanceInfo _instance)
    {
    	GS2GC_037_055_OnDungeonTagMonsterChg proto = new GS2GC_037_055_OnDungeonTagMonsterChg();
		proto.setId(_instance.getId());
		_instance.getMonsterMgr().makeTagMonsterIdList(proto.getTagMonsterIdList());
    	
        return proto;
    }
    
    public static GS2GC_037_056_OnDungeonPush make_056_OnDungeonPush(long _cid, GuildInfo _guild)
    {
    	GS2GC_037_056_OnDungeonPush proto = new GS2GC_037_056_OnDungeonPush();
		//设置数据列表
    	_guild.getDungeonMgr().getSetMgr().makeUnlcokProto(proto.getSetList());
		//实例数据列表
    	_guild.getDungeonMgr().getInstanceMgr().makeProto(proto.getInstanceList());
		//已领取奖励的怪物数据列表
    	_guild.getDungeonMgr().getInstanceMgr().makeGainedRewardList(_cid, proto.getGainedRewardMonsterIdList());

        return proto;
    }
}
package NPUSServer.NPUserMsgDispather.Write;

import Common.GuildEnum.EGuildBoxType;
import Common.GuildEnum.EGuildMarsHelpObjType;
import Common.GuildObj.Guild_BoxReward;
import GS2GC.p042_GuildRelatedOp.*;
import NPCommon.CommonObj.NPCommonCostItem;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.Guild.MarsHelp.GuildMarsHelpInfo;
import NPUSServer.Guild.Rally.GuildRallyInfo;
import NPUSServer.Guild.Rally.RallyMemberInfo;
import NPUSServer.NPUSUserMgr.UserComp.MarsComp._IGuildMarsHelp;

import java.util.ArrayList;

/**
 * 42 - 联盟关联系统
 * @author mj
 *
 */
public class US2GCWriter_042_GuildRelatedOp
{
    public static GS2GC_042_001_RetSendMarsHelp make_001_RetSendMarsHelp()
    {
    	GS2GC_042_001_RetSendMarsHelp proto = new GS2GC_042_001_RetSendMarsHelp();
    	
        return proto;
    }

    public static GS2GC_042_002_RetDealMarsHelp make_002_RetDealMarsHelp(int _dealCount, long _realCount)
    {
    	GS2GC_042_002_RetDealMarsHelp proto = new GS2GC_042_002_RetDealMarsHelp();
    	proto.setDealCount(_dealCount);
    	proto.setRewardCount(_realCount);
    	
        return proto;
    }

    public static GS2GC_042_003_RetMarsHelpList make_003_RetMarsHelpList(GuildInfo _guild, long _cid, ArrayList<Long> _helpIdList)
    {
    	GS2GC_042_003_RetMarsHelpList proto = new GS2GC_042_003_RetMarsHelpList();
    	_guild.getMarsHelpMgr().makePlayerCanDealMarsHelpProto(_cid, proto.getHelpList());

        //逐个查询添加
        if(null != _helpIdList) {
            for (Long helpId : _helpIdList) {
                GuildMarsHelpInfo helpInfo = _guild.getMarsHelpMgr().lookup(helpId);

                if(null != helpInfo)
                    proto.getMyHelpList().add(helpInfo.toProto());
            }
        }
    	
        return proto;
    }

    public static GS2GC_042_004_RetMarsHelpDealedList make_004_RetMarsHelpDealedList(GuildMarsHelpInfo _helpInfo)
    {
    	GS2GC_042_004_RetMarsHelpDealedList proto = new GS2GC_042_004_RetMarsHelpDealedList();
    	proto.setDealedCount(_helpInfo.getDealedCount());
    	proto.setDealLimit(_helpInfo.getDealLimit());
    	
        return proto;
    }

    public static GS2GC_042_005_RetGetGuildBoxList make_005_RetGetGuildBoxList()
    {
    	GS2GC_042_005_RetGetGuildBoxList proto = new GS2GC_042_005_RetGetGuildBoxList();
    	
        return proto;
    }

    public static GS2GC_042_006_RetGainGuildRewardBoxList make_006_RetGainGuildRewardBoxList()
    {
    	GS2GC_042_006_RetGainGuildRewardBoxList proto = new GS2GC_042_006_RetGainGuildRewardBoxList();
    	
        return proto;
    }

    public static GS2GC_042_007_RetGainGuildRewardBox make_007_RetGainGuildRewardBox()
    {
    	GS2GC_042_007_RetGainGuildRewardBox proto = new GS2GC_042_007_RetGainGuildRewardBox();
    	
        return proto;
    }

    public static GS2GC_042_008_RetSetGuildBoxShareAnonymous make_008_RetSetGuildBoxShareAnonymous()
    {
    	GS2GC_042_008_RetSetGuildBoxShareAnonymous proto = new GS2GC_042_008_RetSetGuildBoxShareAnonymous();
    	
        return proto;
    }

    public static GS2GC_042_009_RetMarsHelpAutoDailyRecord make_009_RetMarsHelpAutoDailyRecord(int _count)
    {
    	GS2GC_042_009_RetMarsHelpAutoDailyRecord proto = new GS2GC_042_009_RetMarsHelpAutoDailyRecord();
    	proto.setCount(_count);
    	
        return proto;
    }

    public static GS2GC_042_050_OnMyMarsHelpChg make_050_OnMyMarsHelpChg(_IGuildMarsHelp _obj)
    {
    	GS2GC_042_050_OnMyMarsHelpChg proto = new GS2GC_042_050_OnMyMarsHelpChg();
    	proto.setId(_obj.getHelpId());
    	proto.setGuildHelpSecs(_obj.getHelpSecs());
    	proto.setObjType(_obj.getObjType());
    	proto.setObjId(_obj.getObjId());
    	
        return proto;
    }

    public static GS2GC_042_051_OnCanDealMarsHelpAdd make_051_OnCanDealMarsHelpAdd(long _id)
    {
    	GS2GC_042_051_OnCanDealMarsHelpAdd proto = new GS2GC_042_051_OnCanDealMarsHelpAdd();
    	proto.setCanDealId(_id);
    	
        return proto;
    }

    public static GS2GC_042_052_OnCanDealMarsHelpDel make_052_OnCanDealMarsHelpDel(long _id)
    {
    	GS2GC_042_052_OnCanDealMarsHelpDel proto = new GS2GC_042_052_OnCanDealMarsHelpDel();
    	proto.setCanDealId(_id);
    	
        return proto;
    }

    public static GS2GC_042_053_OnMyMarsHelpDealed make_053_OnMyMarsHelpDealed(long _id, 
    		long _dealCid, int _dealedCount, int _dealLimit,
    		EGuildMarsHelpObjType _objType, long _objId)
    {
    	GS2GC_042_053_OnMyMarsHelpDealed proto = new GS2GC_042_053_OnMyMarsHelpDealed();
    	proto.setId(_id);
    	proto.setDealCid(_dealCid);
    	proto.setDealedCount(_dealedCount);
    	proto.setDealLimit(_dealLimit);
    	proto.setObjType(_objType);
    	proto.setObjId(_objId);
    	proto.setIsAuto(false);
    	
        return proto;
    }
    public static GS2GC_042_053_OnMyMarsHelpDealed make_053_OnMyMarsAutoHelpDealed(long _id, 
    		long _dealCid, int _dealedCount, int _dealLimit,
    		EGuildMarsHelpObjType _objType, long _objId)
    {
    	GS2GC_042_053_OnMyMarsHelpDealed proto = new GS2GC_042_053_OnMyMarsHelpDealed();
    	proto.setId(_id);
    	proto.setDealCid(_dealCid);
    	proto.setDealedCount(_dealedCount);
    	proto.setDealLimit(_dealLimit);
    	proto.setObjType(_objType);
    	proto.setObjId(_objId);
    	proto.setIsAuto(true);
    	
        return proto;
    }

    public static GS2GC_042_054_OnMyMarsHelpDel make_054_OnMyMarsHelpDel(long _id)
    {
    	GS2GC_042_054_OnMyMarsHelpDel proto = new GS2GC_042_054_OnMyMarsHelpDel();
    	proto.setId(_id);
    	
        return proto;
    }

    public static GS2GC_042_055_OnGuildBoxAddCountChg make_055_OnGuildBoxAddCountChg(EGuildBoxType _boxType, int _addValue)
    {
    	GS2GC_042_055_OnGuildBoxAddCountChg proto = new GS2GC_042_055_OnGuildBoxAddCountChg();
    	proto.setBoxType(_boxType);
    	proto.setAddCount(_addValue);
    	
        return proto;
    }

    public static GS2GC_042_056_OnGuildBoxRewardShow make_056_OnGuildBoxRewardShow(EGuildBoxType _boxType, long _id, NPCommonCostItem _item)
    {
    	GS2GC_042_056_OnGuildBoxRewardShow proto = new GS2GC_042_056_OnGuildBoxRewardShow();
    	proto.setBoxType(_boxType);
    	
    	Guild_BoxReward reward = new Guild_BoxReward();
    	reward.setId(_id);
    	reward.setItem(_item.toProto());
    	proto.addRewardList(reward);
    	
        return proto;
    }

    public static GS2GC_042_058_OnGuildActivePointChg make_058_OnGuildActivePointChg(GuildInfo _guild)
    {
    	GS2GC_042_058_OnGuildActivePointChg proto = new GS2GC_042_058_OnGuildActivePointChg();
    	proto.setActivePoint(_guild.getActivePoint());
    	proto.setTargetLvl(_guild.getActivePointTargetLvl());
    	
        return proto;
    }

    /**
     * 060-返回创建集结结果
     */
    public static GS2GC_042_060_RetCreateRally make_060_RetCreateRally(long _rallyId)
    {
        GS2GC_042_060_RetCreateRally proto = new GS2GC_042_060_RetCreateRally();
        proto.setRallyId(_rallyId);

        return proto;
    }

    /**
     * 061-返回加入集结结果
     */
    public static GS2GC_042_061_RetJoinRally make_061_RetJoinRally(long _rallyId)
    {
        GS2GC_042_061_RetJoinRally proto = new GS2GC_042_061_RetJoinRally();
        proto.setRallyId(_rallyId);

        return proto;
    }

    /**
     * 062-返回单个集结信息
     */
    public static GS2GC_042_062_RetQueryRallyInfo make_062_RetQueryRallyInfo(GuildRallyInfo<?> _rallyInfo)
    {
        GS2GC_042_062_RetQueryRallyInfo proto = new GS2GC_042_062_RetQueryRallyInfo();

        proto.setRallyId(_rallyInfo.getRallyId());
        proto.setRallyType(_rallyInfo.getRallyTypeEnum());
        proto.setLeaderCid(_rallyInfo.getLeaderCid());
        proto.setLeaderTeamId(_rallyInfo.getLeaderTeamId());
        proto.setCreateTimeMs(_rallyInfo.getCreateTimeMs());
        proto.setExpireTimeMs(_rallyInfo.getExpireTimeMs());
        proto.setMinPowerLimit(_rallyInfo.getMinPowerLimit());
        proto.setMaxMemberLimit(_rallyInfo.getMaxMemberLimit());
        proto.setLeaderTeamSnapshot(_rallyInfo.getTeamSnapshot());
        proto.setExtData(_rallyInfo.getExtData());

        for (RallyMemberInfo memberInfo : _rallyInfo.cpyMemberInfoList()) {
            if (memberInfo == null) {
                continue;
            }

            proto.addMemberCidList(memberInfo.getCid());
        }

        return proto;
    }

    /**
     * 063-成员到达集结推送
     */
    public static GS2GC_042_063_OnRallyMemberArrive make_063_OnRallyMemberArrive(long _rallyId, long _memberCid)
    {
        GS2GC_042_063_OnRallyMemberArrive proto = new GS2GC_042_063_OnRallyMemberArrive();
        proto.setRallyId(_rallyId);
        proto.setMemberCid(_memberCid);

        return proto;
    }

    /**
     * 064-成员退出集结推送
     */
    public static GS2GC_042_064_OnRallyMemberExit make_064_OnRallyMemberExit(long _rallyId, long _memberCid)
    {
        GS2GC_042_064_OnRallyMemberExit proto = new GS2GC_042_064_OnRallyMemberExit();
        proto.setRallyId(_rallyId);
        proto.setMemberCid(_memberCid);

        return proto;
    }
}
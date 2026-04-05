package NPUSServer.GMCommand.Cmds;


import ALServerLog.ALServerLog;
import Common.NpPlayerInfoObj.PlayerInfo_CommonShow;
import MJLog.MJSectionLog;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.CommonObj.NPRefreshTimeObj;
import NPCommon.CommonProcess.ProcessExample;
import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate.HandlerOne;
import NPCommon.Util.Delegate.HandlerTwo;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPCommon.Util.NumberCompact.NumberCompressList;
import NPCommon.Util.Pair.WCGPairIntList;
import NPCommon.Util.StringFunc;
import NPCommon.Util.WCGRandomGen;
import NPEnum.ENPDDAlertType;
import NPEnum.ENPPlayerPropertyType;
import NPGameRes.GameObjs.NPPlayerProperty.NPPlayerPropertyContainer;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;
import NPGameRes.GameObjs.RefUnionBonus.UnionBonus;
import NPGameRes.LogicEvent.MetaData.EventMeta;
import NPGameRes.LogicEvent.MetaData.EventMetaMgr_Refdata;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.Refs.Quest.RefQuest;
import NPGameRes.Refs.Quest.RefQuestStep;
import NPGameRes.Refs.Quest.RefQuestTarget;
import NPGameRes.Refs.RefGeneral;
import NPServerProtocolWriter.NP2LCS.Request.NP2LCS_R_Writer_001_BasicOp;
import NPUSServer.Common.Event.Events.Event_P_ARENA_INFLUENCE_CHG;
import NPUSServer.Common.Event.Events.Event_P_CONSORT_RAND_CALL;
import NPUSServer.Common.Event.Events.Event_P_CROSS_DAY;
import NPUSServer.Common.Event.Events.Event_S_ONLY_TEST;
import NPUSServer.Common.UsFunc;
import NPUSServer.GMCommand.UsCmdBase;
import NPUSServer.NPEvent.EventMgr.EventObj.NPGlobalUserEventObj;
import NPUSServer.NPEvent.EventMgr.EventObj._INPGlobalUserEventObj;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortInfo;
import USDB.Update.Update_1_0_1_1_To_1_0_1_2;
import WCGCommon.Enum.NPEnum;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.Map;

@ACommander(comment = "测试命令", name = "test")
public class CmdTest extends UsCmdBase
{
    public static final NumberCompressList _m_rangeList = new NumberCompressList();

    static
    {
        List<Integer> numList = Arrays.asList(1, 3, 5, 7, 9, 10, 11, 14);
        for (int number : numList)
        {
            _m_rangeList.addNumber(number);
        }
    }

    @ACommand(comment = "测试A")
    public String testA()
    {
        return "testA";
    }

    @ACommand(comment = "显示价格")
    public String getPrice(int _priceId, int _times)
    {
        NPCommonCostItem item = UsFunc.calCostPrice(getOwner(), _priceId, _times - 1);
        if (null == item)
        {
            return "null item";
        } else
            return item.toString();
    }

    @ACommand(comment = "显示物品列表")
    public String parseItemList(String _str)
    {
        List<NPCommonCostItem> itemList = StringFunc.listFromString(_str, () -> new NPCommonCostItem());
        return StringFunc.list2String(itemList);
    }

    @ACommand(comment = "event")
    public String evt()
    {
        return "no deal";
    }

    @ACommand(comment = "processTest")
    public String tpr()
    {
        ProcessExample.process();
        return "ok";
    }

    @ACommand(comment = "测试刷新时间计算[参数][是否是上一个]")
    public String testRefreshTime(String _param,long _timestamp, boolean _isLast)
    {
        if (_timestamp == 0)
            _timestamp = CommonFunc.getNowTimeMS();

        NPRefreshTimeObj refreshTimeObj = new NPRefreshTimeObj();
        refreshTimeObj.parseFromString(_param);
        return String.valueOf(_isLast ? refreshTimeObj.getBeforeFreshTimeTagMS(_timestamp)
                : refreshTimeObj.getNextFreshTimeTagMS(_timestamp));
    }

    @ACommand(comment = "testEvnetHandlerMgr")
    public String teh()
    {
        TestObject object = new TestObject();

        getOwner().getEventHandlerMgr().regHandler(Event_P_CROSS_DAY.ID, object, new HandlerTwo<_ALogicEventBase, NPUSUserData>()
        {
            @Override
            public void handle(_ALogicEventBase aLogicEventBase, NPUSUserData _userData)
            {
                System.out.println("teh handler...");
            }
        });
        Event_P_CROSS_DAY event = new Event_P_CROSS_DAY(getContext(), 20210812);
        getOwner().onLogicEvent(event);

        //没有主动反注册，调用一次gc后会被主动回收

        return getOwner().getEventHandlerMgr().toString();
    }

    @ACommand(comment = "testGlobalMgr")
    public String teg()
    {
        TestObject object = new TestObject();
        getUserServer().getGlobalEventHandlerMgr().regHandler(Event_P_CROSS_DAY.ID, object,
                new HandlerTwo<_ALogicEventBase, _INPGlobalUserEventObj>()
                {
                    @Override
                    public void handle(_ALogicEventBase aLogicEventBase, _INPGlobalUserEventObj _eventObj)
                    {
                        System.out.println("testGlobalMgr handler...");
                    }
                });

        Event_P_CROSS_DAY event = new Event_P_CROSS_DAY(getContext(), 20210812);
        getUserServer().getGlobalEventHandlerMgr().handle(event, new NPGlobalUserEventObj(getOwner()));

        //没有主动反注册，调用一次gc后会被主动回收

        return getUserServer().getGlobalEventHandlerMgr().toString();
    }

    @ACommand(comment = "强制gc")
    public String gc()
    {
        System.gc();
        Runtime.getRuntime().runFinalization();
        System.gc();

        String s1 = getOwner().getEventHandlerMgr().toString();
        String s2 = getUserServer().getGlobalEventHandlerMgr().toString();

        return s1 + "\n" + s2;
    }

    class TestObject implements _IHandlerHolder
    {

    }

    @ACommand(comment = "测试属性加成容器")
    public String testRand(long seed)
    {
        WCGRandomGen ran = new WCGRandomGen(seed);

        for (int i = 10; i > 0; i--)
        {
            int rand = ran.nextInt(1, 100);
            System.out.println("rand = " + rand);
        }
        return "ok";
    }

    @ACommand(comment = "测试缓存加载")
    public String loadCache()
    {
        List<Long> cidList = new ArrayList<>();
        cidList.add(10100001L);
        cidList.add(20100001L);

        getUserServer().getPlayerCacheGetter().getInfoListA(PlayerInfo_CommonShow.class, cidList, new HandlerOne<Map<Long, PlayerInfo_CommonShow>>()
        {
            @Override
            public void handle(Map<Long, PlayerInfo_CommonShow> _infoMap)
            {
                for (PlayerInfo_CommonShow value : _infoMap.values())
                {
                    CommLog.info(value.toString());
                }
            }
        });

        return "ok";
    }

    @ACommand(comment = "测试红包分配（红包金额，红包数量，红包的随机区间，最小值，最大值，测试循环次数）")
    public String testRedPacketAllocat(long _sum, int _count, String _per, long _min, long _max, int _testCount)
    {
        StringBuilder sb = new StringBuilder();

        WCGPairIntList perObj = WCGPairIntList.fromString(_per);
        if (null == perObj)
        {
            return "fail, per is error:" + _per;
        }

        sb.append("\nsum:").append(_sum)
                .append(", count:").append(_count)
                .append(", per:").append(_per)
                .append(", min:").append(_min)
                .append(", max:").append(_max);
        sb.append("\n===================\n");

        for (int i = 0; i < _testCount; i++)
        {
            sb.append("loop:").append(i + 1).append(" [");

            ArrayList<Long> resList = new ArrayList<>();
            CommonFunc.redPacketAllocat(_sum, _count, perObj, _min, _max, resList, 0);

            if (resList.isEmpty())
            {
                sb.append("no data >>>>>>>>>>>>>>>>");
            }

            sb.append(StringFunc.joinString(",", CommonFunc.toStringList(CommonFunc.toStringList(resList))));
            sb.append("]");
            sb.append("\n---------------------------\n");
        }

        return sb.toString();
    }

    @ACommand(comment = "测试配表加载")
    public String testRefReload()
    {
        RefQuestTarget refQuestTarget = RefQuestTarget.getMgr().get(10103002001L);
        return refQuestTarget.process_cur_count.toString();
    }

    @ACommand(comment = "测试条件")
    public String cond(String _condStr)
    {
        NPPlayerConditionGroupObj condObj = new NPPlayerConditionGroupObj();
        condObj.readConditionGroupList(_condStr, "err:" + _condStr);

        return NPPlayerConditionDealerMgr.IsEnable(condObj, getOwner(), null) ? "pass" : "refuse";
    }

    @ACommand(comment = "发钉钉预警")
    public String ddAlert(String _content)
    {
        getUserServer().getDDAlert().sendToHS(ENPDDAlertType.MEMORY, ALServerLog.LogLevel.FATAL, _content);
        return "ok";
    }

    @ACommand(comment = "设置是否开启账号验证")
    public String setNeedCheck(boolean _needCheck)
    {
        //允许作弊本用户类型才可登录
        getUserServer().sendRequestToBSServer(NPEnum.EServerType.SINGLE.ordinal()
                , NPEnum.ENPSingleServerType.LOGIN_CHECK.ordinal()
                , NP2LCS_R_Writer_001_BasicOp.make_098_ReqSetNeedCheckAcc(_needCheck));

        return "ok";
    }


    @ACommand(comment = "更新账号信息")
    public String updateAccInfo(String _accName, String _pass)
    {
        //允许作弊本用户类型才可登录
        getUserServer().sendRequestToBSServer(NPEnum.EServerType.SINGLE.ordinal()
                , NPEnum.ENPSingleServerType.LOGIN_CHECK.ordinal()
                , NP2LCS_R_Writer_001_BasicOp.make_099_ReqUpdateAcc(_accName, _pass));

        return "ok";
    }

    @ACommand(comment = "printInt")
    public String printInt()
    {
        return _m_rangeList.toString();
    }

    @ACommand(comment = "addInt")
    public String addInt(int _number)
    {
        System.out.println("加入：" + _number);
        boolean b = _m_rangeList.addNumber(_number);
        if (!b)
        {
            return "已存在";
        }
        return "加入后：" + _m_rangeList;
    }

    @ACommand(comment = "rmInt")
    public String rmInt(int _number)
    {
        System.out.println("移除：" + _number);
        boolean b = _m_rangeList.removeNumber(_number);
        if (!b)
        {
            return "不存在";
        }
        return "移除后：" + _m_rangeList;
    }

    @ACommand(comment = "好友推荐")
    public String friendTip()
    {
        List<Long> recommendList = getUserServer().getFriendTipMgr().getRecommendList(getOwner().getCid(), new ArrayList<>());
        return CommonFunc.list2String(recommendList);
    }
    @ACommand(comment = "测试梦加截面")
    public String testSectionLog()
    {
        MJSectionLog.sectionLog(getOwner());
        return "ok";
    }

    @ACommand(comment = "加成解析测试")
    public String unionBonusTest()
    {
        UnionBonus unionBonus = new UnionBonus();
        unionBonus.parseFromString("NONE|NONE|POWER_PER:1000;POWER:1000#POWER_PER:1000;POWER:1000");
        return "ok";
    }

    @ACommand(comment = "测试排行榜事件")
    public String testRankEvent(long _cid, int _value)
    {
        if (0 == _cid)
        {
            getOwner().onLogicEvent(new Event_P_ARENA_INFLUENCE_CHG(getContext(), _value));
        }else
        {
            getUserServer().getGlobalEventHandlerMgr().handle(new Event_P_ARENA_INFLUENCE_CHG(getContext(), _value), new NPGlobalUserEventObj(_cid));
        }
        return "ok";
    }

    @ACommand(comment = "测试妃子随机邀约事件")
    public String testConsortRandCall()
    {
    	ConsortInfo consort = getOwner().getConsortComponent().lookupRnd();
    	if(null == consort)
    		return "fail, not have consort";
    	
    	Event_P_CONSORT_RAND_CALL evt = new Event_P_CONSORT_RAND_CALL(getIContext(), consort.getConsortId(), 100);
    	getOwner().onLogicEvent(evt);
    	
        return "ok";
    }

	@ACommand(comment = "测试事件[事件名称，参数列表]")
	public String testEvt(String _evtName, ArrayList<Long> _params)
	{
		EventMeta meta = EventMetaMgr_Refdata.getInstance().lookupMetaByEventName(_evtName);
		if(null == meta)
			return "fail, not find evt.";
		
		Event_S_ONLY_TEST evt = new Event_S_ONLY_TEST(getContext());
		evt.setMeta(meta);
		evt.setParamValue(_params);
		
		getOwner().onLogicEvent(evt);
		
		return "ok";
	}
	
    @ACommand(comment = "测试庆祝buff相关信息")
    public String testGraveBuff()
    {
    	long nowTimeMs = CommonFunc.getNowTimeMS();
    	long buffEndTimeMs = RefGeneral.Ref().graveBuffEndTimeObj.getNextFreshTimeTagMS(nowTimeMs);
    	int secs = (int) Math.max(0, ((buffEndTimeMs - nowTimeMs) / 1000)) ;
    	
    	return "sec:" + secs;
    }
	
    @ACommand(comment = "测试妃子领悟技能次数[次数，分组]")
    public String testRandConsortProAdd(int _count, int _groupId)
    {
    	StringBuilder sb = new StringBuilder();
    	
    	for(int i = 0; i < _count; i++)
    	{
    		int curProAdd = RefGeneral.Ref().randomConsortSkillProAdd(_count, _groupId);
    		
    		sb.append("\ncount:").append(i + 1).append(", proAdd:").append(curProAdd);
    	}
    	
    	return sb.toString();
    }

    @ACommand(comment = "测试联盟ID转换[usId，转换联盟ID]")
    public String testTransGuildId(int _usId, long _preId)
    {
        long newId = Update_1_0_1_1_To_1_0_1_2.transNewId(_usId, _preId);
        return "newId:" + newId;
    }
    


    @ACommand(comment = "测试时间加速[时长（秒），时间加速倍率]")
    public String testTimePer(int _secs, int _per)
    {
    	long secs = _secs;
    	long finalMs = secs * 1000 * 10000 / (10000 + _per);
    	
        return _secs + " -> " + finalMs + " [" + finalMs/1000 + "(s)]" ;
    }
    

    @ACommand(comment = "增加玩家属性[属性类型，属性值]")
    public String setPlayerProperty(ENPPlayerPropertyType _type, int _value)
    {
    	NPPlayerPropertyContainer test = new NPPlayerPropertyContainer();
    	getOwner().getPlayerComponent().getPropertyMgr().regPropertyContainer(test);
    	
    	test.setValue(_type, _value);
    	
    	return "ok";
    }

    @ACommand(comment = "测试活动ID规则-生成活动队伍ID[组ID，队伍索引]")
    public String makeActivityTeamId(long _groupId, int _idx)
    {
        long teamId = CommonFunc.makeActivityTeamId(_groupId, _idx);

        return "groupId:" + _groupId + ", idx:" + _idx + " -> teamId:" + teamId;
    }

    @ACommand(comment = "测试活动ID规则-解析活动队伍ID[队伍ID]")
    public String parseActivityTeamId(long _teamId)
    {
        long groupId = CommonFunc.parseActivityTeamGroupId(_teamId);
        int idx = CommonFunc.parseActivityTeamIdx(_teamId);

        return "teamId:" + _teamId + " -> groupId:" + groupId + ", idx:" + idx;
    }

    @ACommand(comment = "测试最近的主线任务步骤")
    public String testNearQuestStep(long _step)
    {
        RefQuest questRef = RefQuest.getMgr().get(1L);
        if(null == questRef)
            return "fail, not find questRef.";

        RefQuestStep stepRef = questRef.getNearStepRef(_step);
        if(null == stepRef)
            return "fail, not find stepRef. step:" + _step;

        return "succ, step:" + _step + " -> nearStep:" + stepRef.step_id;
    }
}
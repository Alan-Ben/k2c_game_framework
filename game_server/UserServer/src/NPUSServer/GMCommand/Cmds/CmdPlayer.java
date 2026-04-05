package NPUSServer.GMCommand.Cmds;

import Common.QuestEnum.EQuestType;
import CommonEnum.ECurrency;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.ErrMain.Result.Result;
import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.RunResult;
import NPEnum.*;
import NPGameRes.GameObjs.Reward.RewardMgr;
import NPGameRes.GameObjs.Reward.RewardObj;
import NPGameRes.Refs.BagItem.RefBagItem;
import NPGameRes.Refs.Building.RefBuilding;
import NPGameRes.Refs.Consort.RefConsort;
import NPGameRes.Refs.Mars.RefMarsBuilding;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep2_BuildingComp.MarsBuildingInfo;
import NPGameRes.Refs.Equip.RefEquip;
import NPGameRes.Refs.Hero.RefHero;
import NPGameRes.Refs.Parse.NPPlayerEffectListParse;
import NPGameRes.Refs.Player.RefPlayerLevel;
import NPGameRes.Refs.Quest.RefQuest;
import NPGameRes.Refs.Quest.RefQuestStep;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.GMCommand.UsCmdBase;
import NPUSServer.NPUSUserMgr.EffectDealer.NPPlayerEffectDealer;
import NPUSServer.ShieldCidMgr.ShieldCidInfo;

import java.util.ArrayList;
import java.util.List;


/**
 * 玩家属性作弊命令
 */
@ACommander(comment = "玩家属性命令", name = "player")
public class CmdPlayer extends UsCmdBase
{
    @ACommand(comment = "显示所有货币")
    public String showMoney()
    {
        StringBuilder sb = new StringBuilder();
        for (ECurrency eCurrency : ECurrency.values())
        {
            sb.append(eCurrency).append(":").append(getOwner().getCurrencyComponent().getItemCount(eCurrency.ordinal())).append("\n");
        }
        return sb.toString();
    }

    @ACommand(comment = "查看玩家赚速")
    public String showEarningsSpeed()
    {
        return "赚速 = " + getOwner().getPlayerComponent().getEarnings() + "\n自动点击赚速 = " + getOwner().getPlayerComponent().getAutoTapEarnings();
    }

    @ACommand(comment = "查看玩家属性")
    public String listProperty()
    {
        return getOwner().getPlayerComponent().getPropertyMgr().toString();
    }

    @ACommand(comment = "设置指定参数的值(参数枚举值,值)")
    public String setParam(ENPPlayerParam _param, long _value)
    {
        getOwner().getPlayerComponent().setParam(_param, _value);
        if (ENPPlayerParam.LEVEL == _param)
        {
            getOwner().getPlayerComponent().reInitLevel();
        }
        return "setParam ok!";
    }

    @ACommand(comment = "设置玩家等级(等级)")
    public String setLevel(int _level)
    {
        boolean result = getOwner().getPlayerComponent().forceSetLevel(_level, getContext());
        if (result)
        {
            // 获取当前等级、经验和赚速信息
            int currentLevel = (int) getOwner().getPlayerComponent().getParamV(ENPPlayerParam.LEVEL);
            long currentExp = getOwner().getCurrencyComponent().getItemCount(ECurrency.P_EXP.ordinal());
            long currentEarnings = getOwner().getPlayerComponent().getEarnings();

            return String.format("ok, level: %d, exp: %d, earnings: %d", currentLevel, currentExp, currentEarnings);
        }
        return "fail, invalid level or level config not found";
    }

    @ACommand(comment = "获得物品(物品枚举ENPItemType，物品子id,数量)")
    public String gain(ENPItemType _itemType, long _subId, long _count)
    {
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.GM_CMD);
        getOwner().gainItem(_itemType, _subId, _count, context);
        if (!context.getCollector().isEmpty())
        {
            getOwner().sendMsgToGC(context.getCollector().toProto());
        }
        return "ok";
    }

    @ACommand(comment = "获得货币(货币类型，数量)")
    public RunResult gainCurrency(ECurrency _eCurrency, long _num)
    {
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.GM_CMD);
        getOwner().gainItem(ENPItemType.CURRENCY, _eCurrency.ordinal(), _num, context);
        if (!context.getCollector().isEmpty())
        {
            getOwner().sendMsgToGC(context.getCollector().toProto());
        }
        return RunResult.succ(String.format("add %s =%d", _eCurrency, _num));
    }

    @ACommand(comment = "获得背包物品(背包物品id,数量)")
    public String gainBagItem(long _subId, long _count)
    {
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.GM_CMD);
        getOwner().gainItem(ENPItemType.BAG_ITEM, _subId, _count, context);
        if (!context.getCollector().isEmpty())
        {
            getOwner().sendMsgToGC(context.getCollector().toProto());
        }
        return "ok";
    }

    @ACommand(comment = "获得奖励(奖励ID)")
    public String gainReward(long _rewardId)
    {
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.GM_CMD);
        getOwner().gainItem(ENPItemType.REWARD, _rewardId, 1, context);
        if (!context.getCollector().isEmpty())
        {
            getOwner().sendMsgToGC(context.getCollector().toProto());
        }
        return "ok";
    }

    @ACommand(comment = "消耗物品(物品枚举ENPItemType，物品子id,数量)")
    public String consume(ENPItemType _itemType, long _subId, long _count)
    {
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.GM_CMD);
        //消耗数量大于当前拥有的最大值，只消耗到0
        long itemCount = getOwner().getItemCount(_itemType, _subId);
        _count = Math.min(itemCount, _count);
        getOwner().spendItem(_itemType, _subId, _count, context);
        return "ok";
    }

    @ACommand(comment = "消耗货币(货币枚举ECurrency, 数量)")
    public String consumeCurrency(ECurrency _currencyId, long _count)
    {
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.GM_CMD);
        //消耗数量大于当前拥有的最大值，只消耗到0
        long itemCount = getOwner().getItemCount(ENPItemType.CURRENCY, _currencyId.ordinal());
        _count = Math.min(itemCount, _count);
        getOwner().spendItem(ENPItemType.CURRENCY, _currencyId.ordinal(), _count, context);
        return "ok";
    }


    @ACommand(comment = "消耗背包物品(背包物品id,数量)")
    public void consumeBag(long _subId, long _count)
    {
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.GM_CMD);
        //消耗数量大于当前拥有的最大值，只消耗到0
        long itemCount = getOwner().getItemCount(ENPItemType.BAG_ITEM, _subId);
        _count = Math.min(itemCount, _count);
        getOwner().spendItem(ENPItemType.BAG_ITEM, _subId, _count, context);
    }

    @ACommand(comment = "强制扣除背包物品，支持扣到负数(背包物品id,数量)")
    public String forceConsumeBag(long _subId, long _count)
    {
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.GM_CMD);
        boolean result = getOwner().getBagItemComponent().gmSpendItem(_subId, _count, context);
        if (result)
        {
            long currentCount = getOwner().getItemCount(ENPItemType.BAG_ITEM, _subId);
            return String.format("ok, current count: %d", currentCount);
        }
        return "fail";
    }

    @ACommand(comment = "设置货币数量")
    public String setCurrency(ECurrency _type, long _count)
    {
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.GM_CMD);
        getOwner().getCurrencyComponent().setCurrencyCount(_type.ordinal(), _count, context);
        return "ok";
    }


    @ACommand(comment = "设置背包物品数量(背包物品id,数量)")
    public void setBag(long _subId, int _count)
    {
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.GM_CMD);
        getOwner().getBagItemComponent().setItem(_subId, _count, context);
    }

    @ACommand(comment = "列出所有参数")
    public String params()
    {
        StringBuilder sb = new StringBuilder();
        for (ENPPlayerParam eParam : ENPPlayerParam.values())
        {
            String str = String.format("[%d] %s= %d \n", eParam.ordinal(), eParam.toString(), getOwner().getPlayerComponent().getParamV(eParam));
            sb.append(str);
        }
        return sb.toString();
    }


    @ACommand(comment = "获取所有背包物品（数量）")
    public String gainBagAll(long _count)
    {
        if (_count <= 0) return "fail, count must > 0";

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.GM_CMD);

        List<RefBagItem> itemList = RefBagItem.getMgr().getList();
        for (int i = 0; i < itemList.size(); i++)
        {
            RefBagItem ref = itemList.get(i);
            if (null == ref) continue;

            getOwner().gainItem(ENPItemType.BAG_ITEM, ref.id, _count, context);
        }
        return "ok";
    }

    @ACommand(comment = "清除玩家背包物品[背包ID，0-全部]")
    public String bagClear(long _itemId)
    {
        getOwner().getBagItemComponent().clearItemCount(_itemId, getContext());

        return "ok";
    }

    @ACommand(comment = "打印玩家事件记录计数")
    public String printEventRecord()
    {
        return getOwner().getEventRecordComp().toString();
    }

    @ACommand(comment = "打印玩家记录计数")
    public String printRecord()
    {
        return getOwner().getRecordComponent().toString();
    }
    
    @ACommand(comment = "设置玩家记录计数[类型，计数]")
    public String setRecord(ENPPlayerRecordParam _type, long _count)
    {
    	getOwner().getRecordComponent().setRecord(_type, _count, getContext());
    	
        return "ok";
    }

    @ACommand(comment = "移除功能解锁记录")
    public String funcUnlockRecordRemove(ENPFunctionType _type)
    {
        getOwner().getFuncUnlockComponent().removeUnlockInfo(_type);
        return "ok";
    }

    @ACommand(comment = "设置情人下次金币邀约计数刷新时间[偏移（秒数）]")
    public String setConsortCallNextRefreshGap(int _secs)
    {
    	long newMs = CommonFunc.getNowTimeMS() + _secs * 1000;
    	getOwner().setParam(ENPPlayerParam.CONSORT_CALL_NEXT_REFRESH_MS, newMs);
    	
    	return "ok";
    }
    
    @ACommand(comment = "增加屏蔽玩家[屏蔽玩家CID]")
    public String addShieldCid(long _shieldCid)
    {
    	getUserServer().getShieldCidMgr().getShield(getOwner().getCid()).addShieldCid(_shieldCid);
    	
    	return "ok";
    }

    @ACommand(comment = "移除屏蔽玩家[屏蔽玩家CID]")
    public String removeShieldCid(long _shieldCid)
    {
    	ShieldCidInfo shield = getUserServer().getShieldCidMgr().lookup(getOwner().getCid());
    	if(null != shield)
    	{
    		shield.removeShieldCid(_shieldCid);
    	}
    	
    	return "ok";
    }

    @ACommand(comment = "清空所有屏蔽玩家数据")
    public String clearShieldCid()
    {
    	getUserServer().getShieldCidMgr().clear();
    	
    	return "ok";
    }

    @ACommand(comment = "清空公告领取记录 需重登")
    public String clearAnnouncementRecord()
    {
    	getOwner().getAnnouncementComponent().cleanRecord();

    	return "ok";
    }

    @ACommand(comment = "godlike")
    public String godlike()
    {
        //变更玩家等级
        RefPlayerLevel refPlayerLevel = RefPlayerLevel.getMgr().get(30);
        if (refPlayerLevel == null)
            return "player level 30 not found";
        getOwner().gainItem(ENPItemType.CURRENCY, ECurrency.P_EXP.ordinal(), refPlayerLevel.exp, getContext());

        while(getOwner().getPlayerComponent().checkLevelUp(getOwner().getPlayerComponent().getCurLvl(),true, getContext()).isSucc())
        {

        }

        do
        {
            //完成主线任务的最后一个step
            RefQuest refQuest = RefQuest.getMgr().get(1L);
            if (null == refQuest || refQuest.quest_type != EQuestType.MAIN || refQuest.listStep.isEmpty())
                break;

            RefQuestStep refQuestStep = refQuest.listStep.get(refQuest.listStep.size() - 1);
            getOwner().getQuestComponent().gmSetQuestStep(refQuest, refQuestStep, getContext());
        } while (false);
        //修改章节进度
        getOwner().getChapterComponent().getChapterInfo().gmChgPos(200, 20);

        //获得物品
        ArrayList<NPCommonCostItem> allItemList = new ArrayList<>();
        //获得妃子
        RefConsort.getMgr().getList().forEach(refConsort -> allItemList.add(new NPCommonCostItem(ENPItemType.CONSORT, refConsort.Id(), 1)));
        //获得大臣
        RefHero.getMgr().getList().forEach(refHero -> allItemList.add(new NPCommonCostItem(ENPItemType.HERO, refHero.Id(), 1)));
        //背包物品
        RefBagItem.getMgr().getList().forEach(refBagItem -> allItemList.add(new NPCommonCostItem(ENPItemType.BAG_ITEM, refBagItem.Id(), 999)));
        //藏品
        RefEquip.getMgr().getList().forEach(refEquip -> allItemList.add(new NPCommonCostItem(ENPItemType.EQUIP, refEquip.Id(), 1)));
        //银币
        allItemList.add(new NPCommonCostItem(ENPItemType.CURRENCY, ECurrency.SILVER.ordinal(), 999999999));
        //钻石
        allItemList.add(new NPCommonCostItem(ENPItemType.CURRENCY, ECurrency.GEM.ordinal(), 999999999));
        //大臣经验
        allItemList.add(new NPCommonCostItem(ENPItemType.CURRENCY, ECurrency.HERO_EXP.ordinal(), 999999999));
        //火星能源
        allItemList.add(new NPCommonCostItem(ENPItemType.CURRENCY, ECurrency.MARS_ENERGY.ordinal(), 999999999));

        //建造所有建筑
        RefBuilding.getMgr().getList().forEach(refBuilding ->
                getOwner().getBuildingComponent().create(refBuilding.Id(), getContext()));

        getOwner().gainItemList(allItemList, getContext());

        //完成前往火星
        getOwner().getMarsGoRouteComponent().setAllDone(getContext());

        //建造所有火星建筑
        RefMarsBuilding.getMgr().getList().forEach(refMarsBuilding -> {
            MarsBuildingInfo building = getOwner().getMarsBuildingComponent().lookupBuilding(refMarsBuilding.id);
            if (building != null && !building.isBuilt())
                building.cmdGmBuild(getContext());
        });

        //设置阶段目标到2
        getOwner().getStageGoalComponent().cmdSetStep(17,getContext());

        //初始化名称
        String initNameResult = _initName();
        if (initNameResult != null)
            return initNameResult;

        //解锁竞技场
        NPPlayerEffectListParse parser = new NPPlayerEffectListParse();
        parser.parseFromString("S_UNLOCK_ARENA");
        NPPlayerEffectDealer.dealEffect(parser, getOwner(), null, getContext());

        return "ok";
    }

    @ACommand(comment = "跳过新手引导")
    public String allPass()
    {
        //变更玩家等级
        RefPlayerLevel refPlayerLevel = RefPlayerLevel.getMgr().get(20);
        if (refPlayerLevel == null)
            return "player level 20 not found";
        getOwner().gainItem(ENPItemType.CURRENCY, ECurrency.P_EXP.ordinal(), refPlayerLevel.exp, getContext());

        while(getOwner().getPlayerComponent().checkLevelUp(getOwner().getPlayerComponent().getCurLvl(), true, getContext()).isSucc())
        {

        }

        do
        {
            //设置主线任务通过到1-70001
            RefQuest refQuest = RefQuest.getMgr().get(1L);
            if (null == refQuest)
                break;
            if (refQuest.quest_type != EQuestType.MAIN)
                break;

            RefQuestStep refQuestStep = RefQuestStep.getMgr().get(70001L);
            if(null == refQuestStep)
            	break;

            getOwner().getQuestComponent().gmSetQuestStep(refQuest, refQuestStep, getContext());
        }while (false);

        //获得初始妃子
        getOwner().gainItem(ENPItemType.CONSORT, 2101, 1, getContext());
        //获得初始大臣
        getOwner().gainItem(ENPItemType.HERO, 1101, 1, getContext());
        //修改章节进度
        getOwner().getChapterComponent().getChapterInfo().gmChgPos(50, 6);

        //获得物品
        ArrayList<NPCommonCostItem> allItemList = new ArrayList<>();
        //背包物品
        RefBagItem.getMgr().getList().forEach(refBagItem -> allItemList.add(new NPCommonCostItem(ENPItemType.BAG_ITEM, refBagItem.Id(), 10)));
        //藏品
        allItemList.add(new NPCommonCostItem(ENPItemType.EQUIP, 1001, 1));
        //银币
        allItemList.add(new NPCommonCostItem(ENPItemType.CURRENCY, ECurrency.SILVER.ordinal(), 1000000));
        //钻石
        allItemList.add(new NPCommonCostItem(ENPItemType.CURRENCY, ECurrency.GEM.ordinal(), 1000000));
        //大臣经验
        allItemList.add(new NPCommonCostItem(ENPItemType.CURRENCY, ECurrency.HERO_EXP.ordinal(), 1000000));
        //火星能源
        allItemList.add(new NPCommonCostItem(ENPItemType.CURRENCY, ECurrency.MARS_ENERGY.ordinal(), 1000000));
        getOwner().gainItemList(allItemList, getContext());

        //建造经营建筑
        long[] buildingIds = {1010, 2010, 3040, 3050};
        for (long buildingId : buildingIds)
        {
            getOwner().getBuildingComponent().create(buildingId, getContext());
        }

        //完成前往火星
        getOwner().getMarsGoRouteComponent().setAllDone(getContext());

        //设置阶段目标到2
        getOwner().getStageGoalComponent().cmdSetStep(5,getContext());

        //初始化名称
        String initNameResult = _initName();
        if (initNameResult != null)
            return initNameResult;

        return "ok";
    }

    /**
     * 初始化名称
     * @return
     */
    private String _initName()
    {
        //设置玩家名称
        if (getOwner().getPlayerComponent().getName().startsWith("@"))
        {
            Result opResult = getOwner().getPlayerComponent().setPlayerName(String.valueOf(getOwner().getCid()), false, getContext());
            if (!opResult.isSucc())
            {
                //如果设置失败，尝试加上数字后缀
                for (int i = 1; i < 10000; i++)
                {
                    opResult = getOwner().getPlayerComponent().setPlayerName(String.valueOf(getOwner().getCid()) + i, false, getContext());
                    if (opResult.isSucc())
                        break;
                }

                //如果还是失败，返回错误
                if (!opResult.isSucc())
                {
                    return "set name fail";
                }
            }
        }

        getOwner().getPlayerComponent().setParam(ENPPlayerParam.IS_SET_DEFAULT, 1);
        return null;
    }


    @ACommand(comment = "展示奖励(奖励ID)")
    public String showReward(long _rewardId)
    {
    	RewardObj rewardObj = RewardMgr.getInstance().lookupReward(_rewardId);
    	if(null == rewardObj)
    		return "fail, not find reward obj.";

    	List<NPCommonCostItem> itemList = rewardObj.getItemList();
    	
    	StringBuilder sb = new StringBuilder();
    	sb.append("\nitemList:").append(itemList.size());
    	for(int i = 0; i < itemList.size(); i++)
    	{
    		NPCommonCostItem item = itemList.get(i);
    		if(null == item)
    			continue;
    		
    		sb.append("\nitem:").append(item.toString());
    	}
    	
    	return sb.toString();
    }

    @ACommand(comment = "设置禁言[房间0-全部，禁言时长秒数，-1永久]")
    public String forbidChat(int _roomType, int _secs)
    {
        long endMs = _secs == -1 ? -1 : CommonFunc.getNowTimeMS() + _secs * 1000;

        getOwner().getForbidChatComponent().setForbidChat(_roomType, endMs);

        return "ok";
    }

    @ACommand(comment = "设置解禁")
    public String unsetForbidChat()
    {
        getOwner().getForbidChatComponent().unsetForbidChat(0);

        return "ok";
    }

}

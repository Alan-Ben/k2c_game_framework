package NPUSServer.NPUSUserMgr.UserComp.ArenaComp;

import Common.ArenaEnum.EArenaBuffType;
import Common.ArenaObj.Arena_BattleInfo;
import Common.ArenaObj.Arena_BattleResult;
import Common.ArenaObj.Arena_BuffList;
import Common.ArenaObj.Arena_SingleBuffInfo;
import Common.HeroObj.Hero_ArenaShowInfo;
import Common.HeroObj.Hero_ArenaShowList;
import CommonEnum.ECurrency;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.CommonObj.NPItemCostCollector_nosafe;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPItemType;
import NPGameRes.GameObjs.Reward.RewardMgr;
import NPGameRes.GameObjs.Reward.RewardObj;
import NPGameRes.Refs.Arena.RefArenaBuff;
import NPGameRes.Refs.Arena.RefArenaRoundReward;
import NPGameRes.Refs.Arena.RefArenaSelectAttackConsume;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_ARENA_GAIN_ROUND_REWARD;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ArenaComp.Battle.ArenaBattleProcessor;
import NPUSServer.NPUSUserMgr.UserComp.ArenaComp.Battle.BuffProcessor;
import NPUSServer.NPUSUserMgr.UserComp.ArenaComp.Battle.SettlementProcessor;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_023_ArenaOp;
import com.google.gson.JsonObject;

import java.util.ArrayList;
import java.util.List;

public class ArenaAKeyAttackDealer
{
    private NPUSUserData _m_userData;
    private EArenaBuffType _m_buffPrefer;

    private ArenaBattleInfo _m_battleInfo;

    public int _m_round;
    private int _m_attackType;
    private long _m_selectAttackItemId;

    //玩家相关数据
    private long _m_heroId;
    private long _m_basePower;
    private long _m_deductedHp;
    private boolean _m_hadBuyBuff;
    private List<Arena_SingleBuffInfo> _m_buffList = new ArrayList<>();

    //对手相关数据
    private long _m_opponentCid;
    private boolean _m_opponentIsBot;
    private String _m_botName;
    private List<Long> _m_hadDefeatHeroIdList = new ArrayList<>();
    private Hero_ArenaShowList _m_opponentHeroList;
    private Hero_ArenaShowInfo _m_tempOpponentHero;

    //奖励收集器
    private NPItemCostCollector_nosafe _m_itemCostCollector = new NPItemCostCollector_nosafe();

    //buff消耗道具数量
    private NPItemCostCollector_nosafe _m_buffCostCollector = new NPItemCostCollector_nosafe();

    //不同增益 对应拥有的道具数量
    private RefArenaBuff[] _m_buffListRef = new RefArenaBuff[EArenaBuffType.values().length];
    private ItemKeyList _m_buffItemNum = new ItemKeyList();

    public ArenaAKeyAttackDealer(ArenaBattleInfo _battleInfo, EArenaBuffType _buffPrefer)
    {
        _m_userData = _battleInfo.getUserData();
        _m_buffPrefer = _buffPrefer;
        _m_battleInfo = _battleInfo;
        _m_round = _battleInfo.getRound();
        _m_attackType = _battleInfo.getAttackType();
        _m_selectAttackItemId = _battleInfo.getSelectAttackItemId();
        _m_heroId = _battleInfo.getHeroId();
        _m_basePower = _battleInfo.getBasePower();
        _m_deductedHp = _battleInfo.getDeductedHp();
        _m_hadBuyBuff = _battleInfo.getHadBuyBuff();
        copyBuffList(_battleInfo.getBuffList());
        _m_opponentCid = _battleInfo.getOpponentCid();
        _m_opponentIsBot = _battleInfo.getOpponentIsBot();
        _m_botName = _battleInfo.getBotName();
        _m_hadDefeatHeroIdList.addAll(_battleInfo.getDefeatHeroList().getValueList());
        _m_opponentHeroList = _battleInfo.getOpponentHeroList();

        _m_buffListRef = RefGeneral.Ref().arenaChooseBuffListRef;
        for (RefArenaBuff refArenaBuff : _m_buffListRef)
        {
            if (refArenaBuff == null)
                continue;

            //初始化buff道具数量
            _m_buffItemNum.initItemKey(refArenaBuff.cost.getItemType(), refArenaBuff.cost.getItemId(),
                    _m_userData.getItemCount(refArenaBuff.cost.getItemType(), refArenaBuff.cost.getItemId()));
        }
    }

    /**
     * 复制buff列表
     * @param _buffList
     * @return
     */
    private void copyBuffList(Arena_BuffList _buffList)
    {
        for (Arena_SingleBuffInfo buffInfo : _buffList.getBuffList())
        {
            Arena_SingleBuffInfo newBuffInfo = new Arena_SingleBuffInfo();
            newBuffInfo.setBuffId(buffInfo.getBuffId());
            newBuffInfo.setNum(buffInfo.getNum());
            _m_buffList.add(newBuffInfo);
        }
    }

    /**
     * 获取实力
     * @return
     */
    public long getPower()
    {
        return ArenaBattleProcessor.calculateBuffedPower(_m_buffList, _m_basePower);
    }

    /**
     * 获取当前血量
     * @return
     */
    public long curHp()
    {
        return getPower() - _m_deductedHp;
    }

    /**
     * 一键攻击
     * @return
     */
    public void aKeyAttack(NPPlayerContext _context)
    {
        // 计算倍率
        int ratio = 1;
        if (_m_selectAttackItemId != 0)
        {
            RefArenaSelectAttackConsume refItem = RefArenaSelectAttackConsume.getMgr().get(_m_selectAttackItemId);
            if (refItem != null)
                ratio = Math.max(1, refItem.ratio);
        }
    	
        //判断是否达到结束条件
        while (curHp() > 0)
        {
            //按照规则选择
            Hero_ArenaShowInfo opponentHero = aKeyChooseOpponentHero();
            if (opponentHero == null)
                break;

            _m_tempOpponentHero = opponentHero;

            //尝试选择buff
            _tryChooseBuff();

            //计算战斗结果
            ArenaBattleProcessor.BattleResult battleResult =
                    ArenaBattleProcessor.processBattle(curHp(), opponentHero.getPower());

            //是否可以击败对方
            boolean canDefeat = battleResult.isVictory();
            if (canDefeat)
                _m_hadDefeatHeroIdList.add(opponentHero.getHeroId());

            _m_round++;
            _m_deductedHp += battleResult.getDeductedHp();
            _m_hadBuyBuff = false;

            //连胜奖励
            if (canDefeat)
            {
                RefArenaRoundReward refArenaRoundReward = RefArenaRoundReward.getMgr().get(_m_round);
                if (refArenaRoundReward != null)
                {
                    RewardObj rewardObj = RewardMgr.getInstance().lookupReward(refArenaRoundReward.reward_id);
                    if (rewardObj != null)
                    {
                    	List<NPCommonCostItem> rewardItemList = rewardObj.makeNewItemList(ratio);
                    	
                    	//检查对应的buff是否可以支持翻倍
                        Event_P_ARENA_GAIN_ROUND_REWARD evt = new Event_P_ARENA_GAIN_ROUND_REWARD(_context);
                        if(_m_userData.getBuffComponent().checkLogicEvt(RefGeneral.Ref().grave_arena_round_reward_buff, evt, _context))
                        {
                        	rewardItemList = CommonFunc.itemMultiple(rewardItemList, RefGeneral.Ref().grave_arena_round_reward_multi);
                        }
                    	
                    	_m_itemCostCollector.addItemList(rewardItemList);
                    }
                }
            }


            long eachDefeatCanGainCoin = RefGeneral.Ref().arena_gain_coin_for_defeating_each_hero;
            // 额外加成
//            HeroInfo heroInfo = _m_userData.getHeroComponent().lookupHero(_m_heroId);
//            if (heroInfo != null)
//            {
//                eachDefeatCanGainCoin += (int) heroInfo.getPlayerBonusAddValue(EBonusPropertyType.ARENA_COINS);
//            }

            //竞技场币
            _m_itemCostCollector.addItem(ENPItemType.CURRENCY, ECurrency.ARENA_COIN.ordinal(), eachDefeatCanGainCoin * ratio);
        }
    }

    /**
     * 尝试选择buff
     */
    private void _tryChooseBuff()
    {
        //首轮不选择buff
        if (_m_round == 0)
            return;

        //如果已经购买buff，则不再购买
        if (_m_hadBuyBuff)
            return;

        RefArenaBuff refArenaBuff = BuffProcessor.chooseBestBuff(_m_buffPrefer, _m_buffItemNum, _m_buffListRef, _m_buffCostCollector);
        if (refArenaBuff == null)
            return;

        //添加buff
        BuffProcessor.addBuff(_m_buffList, refArenaBuff.Id());
        _m_hadBuyBuff = true;
    }

    /**
     * 一键操作选择对手大臣
     * @return
     */
    public Hero_ArenaShowInfo aKeyChooseOpponentHero()
    {
        //判断是否是第三轮，如果是第三轮，从高战力伙伴中随机，否则从低战力伙伴中随机
        //即第三轮正序选择，否则倒序选择
        if (_m_round + 1 % 3 != 0)//
        {
            for (int i = _m_opponentHeroList.getHeroList().size() - 1; i >= 0; i--)
            {
                if (!_m_hadDefeatHeroIdList.contains(_m_opponentHeroList.getHeroList().get(i).getHeroId()))
                    return _m_opponentHeroList.getHeroList().get(i);
            }
        } else
        {
            for (int i = 0; i < _m_opponentHeroList.getHeroList().size(); i++)
            {
                if (!_m_hadDefeatHeroIdList.contains(_m_opponentHeroList.getHeroList().get(i).getHeroId()))
                    return _m_opponentHeroList.getHeroList().get(i);
            }
        }

        return null;
    }

    /**
     * 结算
     * @param _battleResult
     * @param _context
     */
    public Result settle(Arena_BattleResult _battleResult, NPPlayerContext _context)
    {
        //扣除buff消耗
        if (!_m_userData.hasCostItemList(_m_buffCostCollector.getItemList()))
            return CommErr.ITEM_NOT_ENOUGH;

        if (!_m_userData.spendItem(_m_buffCostCollector.getItemList(), _context))
            return CommErr.CONSUME_FAIL;

        //获得过程奖励
        if (!_m_itemCostCollector.isEmpty())
            _m_userData.gainItemList(_m_itemCostCollector.getItemList(), _context);

        // 使用统一的结算处理器
        SettlementProcessor.processSettlement(
                _m_userData,
                _m_hadDefeatHeroIdList.size(),
                _m_opponentCid,
                _m_opponentHeroList.getHeroList().size(),
                _m_opponentIsBot,
                _m_botName,
                _m_heroId,
                _m_attackType,
                _m_selectAttackItemId,
                _battleResult,
                _context,
                calculateTotalPower(_m_opponentHeroList),// 对手总血量
                _m_basePower,// 起始血量
                Math.max(0, _m_basePower - _m_deductedHp),// 剩余血量
                convertBuffListToJson(_m_buffList)// 增益明细
        );

        //通知客户端
        _m_userData.sendMsgToGC(US2GCWriter_023_ArenaOp.make_051_OnArenaBattleInfoChg(makeProto()));

        return Result.SUCC;
    }

    /**
     * 获取战斗信息
     * @return
     */
    public Arena_BattleInfo makeProto()
    {
        Arena_BattleInfo proto = new Arena_BattleInfo();
        proto.setOpponentCid(_m_opponentCid);
        proto.setHadDefeatNum(_m_hadDefeatHeroIdList.size());
        proto.setOpponentHeroNum(_m_opponentHeroList.getHeroList().size());
        proto.setOpponentPower(_m_battleInfo.OpponentPower());
        Hero_ArenaShowInfo opponentHero = _m_tempOpponentHero;
        if (opponentHero != null)
            proto.addCanAttackHeroList(opponentHero);
        proto.setHadBuyBuff(_m_hadBuyBuff);
        proto.getBuffList().addAll(_m_buffList);
        proto.setHeroId(_m_heroId);
        proto.setDeductedHp(_m_deductedHp);
        proto.setBasePower(_m_basePower);
        proto.setIsNpc(_m_opponentIsBot);
        proto.setNpcName(_m_botName);
        return proto;
    }

    /**
     * 物品key
     */
    public static class ItemKey
    {
        final ENPItemType itemType;
        final long itemId;
        long itemCount;

        public ItemKey(ENPItemType itemType, long itemId, long itemCount)
        {
            this.itemType = itemType;
            this.itemId = itemId;
            this.itemCount = itemCount;
        }
    }

    public static class ItemKeyList
    {
        private List<ItemKey> _itemKeyList = new ArrayList<>();

        private ItemKey lookup(ENPItemType itemType, long itemId)
        {
            for (ItemKey itemKey : _itemKeyList)
            {
                if (itemKey.itemType == itemType && itemKey.itemId == itemId)
                {
                    return itemKey;
                }
            }
            return null;
        }

        public void initItemKey(ENPItemType itemType, long itemId, long itemCount)
        {
            ItemKey itemKey = lookup(itemType, itemId);
            //重复初始化不处理
            if (itemKey != null)
                return;

            _itemKeyList.add(new ItemKey(itemType, itemId, itemCount));
        }

        public boolean consumeItem(ENPItemType itemType, long itemId, long num)
        {
            ItemKey itemKey = lookup(itemType, itemId);
            if (itemKey == null || itemKey.itemCount < num)
                return false;

            itemKey.itemCount -= num;
            return true;
        }
    }

    /**
     * 计算英雄列表中所有英雄的实力总和
     * @param _heroList 英雄列表
     * @return 实力总和
     */
    public static long calculateTotalPower(Hero_ArenaShowList _heroList)
    {
        if (_heroList == null || _heroList.getHeroList() == null)
            return 0L;

        long totalPower = 0L;
        for (Hero_ArenaShowInfo hero : _heroList.getHeroList())
        {
            if (hero != null)
                totalPower += hero.getPower();
        }
        return totalPower;
    }

    /**
     * 将Buff列表转换为JSON格式字符串
     *
     * 格式: {buff_id: 使用次数, buff_id: 使用次数, ...}
     * 示例: {"1001": 3, "1002": 5}
     *
     * @param _buffList Buff信息列表
     * @return JSON格式字符串，如果列表为空或null则返回"{}"
     */
    public static String convertBuffListToJson(List<Arena_SingleBuffInfo> _buffList)
    {
        JsonObject jsonObj = new JsonObject();

        if (_buffList == null || _buffList.isEmpty())
            return jsonObj.toString();

        for (Arena_SingleBuffInfo buffInfo : _buffList)
        {
            if (buffInfo == null)
                continue;

            jsonObj.addProperty(String.valueOf(buffInfo.getBuffId()), buffInfo.getNum());
        }

        return jsonObj.toString();
    }

}

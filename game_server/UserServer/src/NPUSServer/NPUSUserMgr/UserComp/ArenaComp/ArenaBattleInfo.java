package NPUSServer.NPUSUserMgr.UserComp.ArenaComp;

import Common.ArenaObj.*;
import Common.Common_LongList;
import Common.HeroObj.Hero_ArenaShowInfo;
import Common.HeroObj.Hero_ArenaShowList;
import CommonEnum.EBonusPropertyType;
import CommonEnum.ECurrency;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.ArenaErr;
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
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroInfo;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_023_ArenaOp;
import USDB.Bo.PlayerArenaBattleBO;

import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

public class ArenaBattleInfo
{
    private ArenaComponent _m_comp;
    private PlayerArenaBattleBO _m_bo;

    //我方相关数据
    //临时增益数据
    private Arena_BuffList _m_buffList;

    //对手相关数据
    //对手英雄列表
    private Hero_ArenaShowList _m_opponentHeroList;
    //已击败的英雄列表
    private Common_LongList _m_defeatHeroList;
    //本回合可攻击英雄列表
    private Common_LongList _m_canAttackHeroList;

    public ArenaBattleInfo(ArenaComponent _comp, PlayerArenaBattleBO _bo)
    {
        _m_comp = _comp;
        _m_bo = _bo;

        _m_buffList = new Arena_BuffList();
        if (_bo.getBuffList() != null)
            _m_buffList.readPackage(ByteBuffer.wrap(_bo.getBuffList()));

        _m_opponentHeroList = new Hero_ArenaShowList();
        if (_bo.getOpponentHeroList() != null)
            _m_opponentHeroList.readPackage(ByteBuffer.wrap(_bo.getOpponentHeroList()));

        _m_defeatHeroList = new Common_LongList();
        if (_bo.getHadDefeatHeroList() != null)
            _m_defeatHeroList.readPackage(ByteBuffer.wrap(_bo.getHadDefeatHeroList()));

        _m_canAttackHeroList = new Common_LongList();
        if (_bo.getCanAttackHeroList() != null)
            _m_canAttackHeroList.readPackage(ByteBuffer.wrap(_bo.getCanAttackHeroList()));
    }

    public ArenaComponent getComp()
    {
        return _m_comp;
    }

    public PlayerArenaBattleBO getBo()
    {
        return _m_bo;
    }

    public NPUSUserData getUserData()
    {
        return _m_comp.getUserData();
    }

    public int getRound()
    {
        return _m_bo.getRound();
    }

    public long getHeroId()
    {
        return _m_bo.getHeroId();
    }

    public long getBasePower()
    {
        return _m_bo.getBasePower();
    }

    public long getDeductedHp()
    {
        return _m_bo.getDeductedHp();
    }

    public int getAttackType()
    {
        return _m_bo.getAttackType();
    }

    public long getSelectAttackItemId()
    {
        return _m_bo.getSelectAttackItemId();
    }

    /**
     * 获取实力
     * @return
     */
    public long getPower()
    {
        return ArenaBattleProcessor.calculateBuffedPower(_m_buffList.getBuffList(), _m_bo.getBasePower());
    }

    /**
     * 获取当前血量
     * @return
     */
    public long curHp()
    {
        return getPower() - _m_bo.getDeductedHp();
    }

    public boolean getHadBuyBuff()
    {
        return _m_bo.getHadBuyBuff();
    }

    public Arena_BuffList getBuffList()
    {
        return _m_buffList;
    }

    public Common_LongList getDefeatHeroList()
    {
        return _m_defeatHeroList;
    }

    public Hero_ArenaShowList getOpponentHeroList()
    {
        return _m_opponentHeroList;
    }

    public long getOpponentCid()
    {
        return _m_bo.getOpponentCid();
    }

    public boolean getOpponentIsBot()
    {
        return _m_bo.getIsBot();
    }

    public String getBotName()
    {
        return _m_bo.getBotName();
    }

    public long OpponentPower()
    {
        return _m_bo.getOpponentPower();
    }

    /**
     * 选择出场大臣
     * @param _heroInfo
     * @param _opponentBotName
     * @param _refItem
     * @param _refBuff
     */
    public void selectBattleHero(HeroInfo _heroInfo, String _opponentBotName, RefArenaSelectAttackConsume _refItem, RefArenaBuff _refBuff)
    {
        //计算实力
        long bonusAddValue = _heroInfo.getPlayerBonusAddValue(EBonusPropertyType.ARENA_POWER_ADD_PER);
        long power = (long) Math.ceil((double) _heroInfo.getPower() * (10000 + bonusAddValue) / 10000);

        //设置出战英雄
        _m_canAttackHeroList.getValueList().addAll(_randomOpponentRoundHero());

        BM bmObj = getUserData().getUSServer().getBM();
        //设置出战英雄
        _m_bo.setHeroId(bmObj, _heroInfo.getHeroId());
        _m_bo.setBasePower(bmObj, power);
        _m_bo.setCanAttackHeroList(bmObj, _m_canAttackHeroList.makePackage().array());
        _m_bo.setSelectAttackItemId(bmObj, _refItem == null ? 0 : _refItem.Id());
        _m_bo.setBotName(bmObj, _opponentBotName);
        //增加buff
        if (_refBuff != null)
        {
            addBuff(_refBuff.Id());
            _m_bo.setBuffList(bmObj, _m_buffList.makePackage().array());
            _m_bo.setHadBuyBuff(bmObj, true);
        }
        _m_bo.saveAllMarked(bmObj);

        //通知客户端
        getUserData().sendMsgToGC(US2GCWriter_023_ArenaOp.make_051_OnArenaBattleInfoChg(makeProto()));
    }

    /**
     * 随机对手出战英雄
     * @return
     */
    private List<Long> _randomOpponentRoundHero()
    {
        List<Hero_ArenaShowInfo> allHeroes = _m_opponentHeroList.getHeroList();
        int total = allHeroes.size();

        // 计算分界索引
        int divideIndex = Math.max(0, (int) Math.ceil(total * 0.3));
        divideIndex = Math.min(divideIndex, total); // 确保不超过总长度

        List<Long> heroIdList = new ArrayList<>();
        //计算是否是高轮次
        boolean isHighRound = ((getRound() + 1) % 3) == 0;

        // 首轮筛选
        int start = isHighRound ? 0 : divideIndex;
        int end = isHighRound ? divideIndex : total;
        for (int i = start; i < end; i++)
        {
            // 索引保护
            if (i >= allHeroes.size())
                break;

            Hero_ArenaShowInfo hero = allHeroes.get(i);
            if (!_m_defeatHeroList.getValueList().contains(hero.getHeroId()))
                heroIdList.add(hero.getHeroId());
        }

        // 交叉补充（当首轮不足3个时）
        if (heroIdList.size() < 3)
        {
            int need = 3 - heroIdList.size();
            int altStart = isHighRound ? divideIndex : 0;
            int altEnd = isHighRound ? total : divideIndex;

            for (int i = altStart; i < altEnd && need > 0; i++)
            {
                // 索引保护
                if (i >= allHeroes.size())
                    break;

                Hero_ArenaShowInfo hero = allHeroes.get(i);
                if (!_m_defeatHeroList.getValueList().contains(hero.getHeroId()))
                {
                    heroIdList.add(hero.getHeroId());
                    need--;
                }
            }
        }

        // 打乱顺序
        Collections.shuffle(heroIdList);

        // 截取子列表
        return heroIdList.size() > 3 ? heroIdList.subList(0, 3) : heroIdList;
    }

    /**
     * 选择buff
     * @param _buffId
     * @param _context
     */
    public Result chooseBuff(long _buffId, NPPlayerContext _context)
    {
        //判断是否已经购买过buff
        if (getBo().getHadBuyBuff())
            return ArenaErr.HAD_BUY_ROUND_BUFF;

        //判断buff是否存在
        RefArenaBuff refArenaBuff = RefArenaBuff.getMgr().get(_buffId);
        if (refArenaBuff == null)
            return CommErr.REF_NOT_FOUND;

        //判断当前回合是否可以购买此buff
        if (getRound() == 0)
        {
            if (!RefGeneral.Ref().arena_initial_choose_buff_list.contains(_buffId))
                return ArenaErr.CANT_BUY_ROUND_BUFF;
        } else
        {
            if (!RefGeneral.Ref().arena_choose_buff_list.contains(_buffId))
                return ArenaErr.CANT_BUY_ROUND_BUFF;
        }

        //尝试消耗
        boolean consumeSuc = getUserData().spendItem(refArenaBuff.cost, _context);
        if (!consumeSuc)
            return CommErr.CONSUME_FAIL;

        //增加buff
        addBuff(_buffId);

        BM bmObj = getUserData().getUSServer().getBM();

        //设置已购买buff
        _m_bo.setHadBuyBuff(bmObj, true);
        _m_bo.setBuffList(bmObj, _m_buffList.makePackage().array());
        _m_bo.saveAllMarked(bmObj);

        //通知客户端
        getUserData().sendMsgToGC(US2GCWriter_023_ArenaOp.make_051_OnArenaBattleInfoChg(makeProto()));

        return Result.SUCC;
    }

    /**
     * 记录新增buff
     */
    public void addBuff(long _buffId)
    {
        BuffProcessor.addBuff(_m_buffList.getBuffList(), _buffId);
    }

    /**
     * 回合战斗
     * @param _opponentHeroId
     * @param _roundResult
     * @param _roundReward
     * @param _context
     */
    public Result roundAttack(long _opponentHeroId, Arena_RoundResult _roundResult, Arena_RoundReward _roundReward, NPPlayerContext _context)
    {
        //判断当前血量是否大于0
        if (curHp() <= 0)
            return ArenaErr.HP_EMPTY;

        //判断是否是本回合可攻击的英雄
        if (!_m_canAttackHeroList.getValueList().contains(_opponentHeroId))
            return CommErr.PARAM_ERROR;

        //获取对手英雄信息
        Hero_ArenaShowInfo opponentHero = null;
        for (Hero_ArenaShowInfo heroInfo : _m_opponentHeroList.getHeroList())
        {
            if (heroInfo.getHeroId() == _opponentHeroId)
            {
                opponentHero = heroInfo;
                break;
            }
        }
        if (opponentHero == null)
            return ArenaErr.OPPONENT_HERO_NOT_FOUND;

        // 使用战斗处理器计算战斗结果
        ArenaBattleProcessor.BattleResult battleResult =
                ArenaBattleProcessor.processBattle(curHp(), opponentHero.getPower());

        //是否可以击败对方
        boolean canDefeat = battleResult.isVictory();
        if (canDefeat)
            _m_defeatHeroList.getValueList().add(_opponentHeroId);

        //随机下一轮对手出战英雄
        _m_canAttackHeroList.getValueList().clear();
        _m_canAttackHeroList.getValueList().addAll(_randomOpponentRoundHero());

        BM bmObj = getUserData().getUSServer().getBM();
        _m_bo.setRound(bmObj, _m_bo.getRound() + 1);
        _m_bo.setDeductedHp(bmObj, _m_bo.getDeductedHp() + battleResult.getDeductedHp());
        _m_bo.setCanAttackHeroList(bmObj, _m_canAttackHeroList.makePackage().array());
        _m_bo.setHadBuyBuff(bmObj, false);
        _m_bo.setHadDefeatHeroList(bmObj, _m_defeatHeroList.makePackage().array());
        _m_bo.saveAllMarked(bmObj);

        // 计算倍率
        int ratio = 1;
        if (_m_bo.getSelectAttackItemId() != 0)
        {
            RefArenaSelectAttackConsume refItem = RefArenaSelectAttackConsume.getMgr().get(_m_bo.getSelectAttackItemId());
            if (refItem != null)
                ratio = Math.max(1, refItem.ratio);
        }

        int eachDefeatCanGainCoin = RefGeneral.Ref().arena_gain_coin_for_defeating_each_hero;
        // 额外加成
//        HeroInfo heroInfo = getUserData().getHeroComponent().lookupHero(_m_bo.getHeroId());
//        if (heroInfo != null)
//        {
//            eachDefeatCanGainCoin += (int) heroInfo.getPlayerBonusAddValue(EBonusPropertyType.ARENA_COINS);
//        }

        // 回合结果
        _roundResult.setRound(getRound());
        _roundResult.setIsDefeat(canDefeat);
        _roundResult.setOpponentDeductinfluence(RefGeneral.Ref().arena_deduct_influence_for_each_hero_defeated);
        _roundResult.setGainCoin(eachDefeatCanGainCoin * ratio);
        _roundResult.setGainInfluence(RefGeneral.Ref().arena_gain_influence_for_defeating_each_hero * ratio);

        // 处理回合奖励
        if (canDefeat)
        {
            RefArenaRoundReward refArenaRoundReward = RefArenaRoundReward.getMgr().get(_m_bo.getRound());
            if (refArenaRoundReward != null)
            {
                RewardObj rewardObj = RewardMgr.getInstance().lookupReward(refArenaRoundReward.reward_id);
                if (rewardObj != null)
                {
                	List<NPCommonCostItem> rewardItemList = CommonFunc.itemMultiple(rewardObj.getItemList(), ratio);
                	
                    //检查对应的buff是否可以支持翻倍
                    Event_P_ARENA_GAIN_ROUND_REWARD evt = new Event_P_ARENA_GAIN_ROUND_REWARD(_context);
                    if(getUserData().getBuffComponent().checkLogicEvt(RefGeneral.Ref().grave_arena_round_reward_buff, evt, _context))
                    {
                    	rewardItemList = CommonFunc.itemMultiple(rewardItemList, RefGeneral.Ref().grave_arena_round_reward_multi);
                    }
                    
                    getUserData().gainItemList(rewardItemList, _context);
                }
            }

            // 竞技场币
            getUserData().gainItem(ENPItemType.CURRENCY, ECurrency.ARENA_COIN.ordinal(),
                    (long) eachDefeatCanGainCoin * ratio, _context);
        }

        _context.getCollector().fillProtoList(_roundReward.getGainItem());

        //通知客户端
        getUserData().sendMsgToGC(US2GCWriter_023_ArenaOp.make_051_OnArenaBattleInfoChg(makeProto()));
        
        return Result.SUCC;
    }

    /**
     * 判断是否回合结束
     * @return
     */
    public boolean isRoundOver()
    {
        return curHp() <= 0 || _m_canAttackHeroList.getValueList().isEmpty();
    }

    /**
     * 回合结算
     * @param _battleResult
     * @param _context
     * @return
     */
    public boolean roundSettle(Arena_BattleResult _battleResult, NPPlayerContext _context)
    {
        //判断是否回合结束
        if (!isRoundOver())
            return false;

        // 使用统一的结算处理器
        SettlementProcessor.processSettlement(
                getUserData(),
                _m_defeatHeroList.getValueList().size(),
                _m_bo.getOpponentCid(),
                _m_opponentHeroList.getHeroList().size(),
                _m_bo.getIsBot(),
                _m_bo.getBotName(),
                _m_bo.getHeroId(),
                _m_bo.getAttackType(),
                _m_bo.getSelectAttackItemId(),
                _battleResult,
                _context,
                ArenaAKeyAttackDealer.calculateTotalPower(_m_opponentHeroList),// 对手总血量
                _m_bo.getBasePower(),// 起始血量
                Math.max(0, _m_bo.getBasePower() - _m_bo.getDeductedHp()),// 剩余血量
                ArenaAKeyAttackDealer.convertBuffListToJson(_m_buffList.getBuffList()));// 增益明细

        return true;
    }

    /**
     * 获取战斗信息
     * @return
     */
    public Arena_BattleInfo makeProto()
    {
        Arena_BattleInfo proto = new Arena_BattleInfo();
        proto.setOpponentCid(getBo().getOpponentCid());
        proto.setHadDefeatNum(_m_defeatHeroList.getValueList().size());
        proto.setOpponentHeroNum(_m_opponentHeroList.getHeroList().size());
        proto.setOpponentPower(getBo().getOpponentPower());
        _m_opponentHeroList.getHeroList().forEach(_opponentHero ->
        {
            if (_m_canAttackHeroList.getValueList().contains(_opponentHero.getHeroId()))
                proto.addCanAttackHeroList(_opponentHero);
        });
        proto.setHadBuyBuff(getBo().getHadBuyBuff());
        proto.getBuffList().addAll(_m_buffList.getBuffList());
        proto.setHeroId(getBo().getHeroId());
        proto.setDeductedHp(getBo().getDeductedHp());
        proto.setBasePower(getBo().getBasePower());
        proto.setIsNpc(getBo().getIsBot());
        proto.setNpcName(getBo().getBotName());
        return proto;
    }

    /**
     * 销毁
     */
    public void discard()
    {
        _m_bo.del(getUserData().getUSServer().getBM());
    }
}

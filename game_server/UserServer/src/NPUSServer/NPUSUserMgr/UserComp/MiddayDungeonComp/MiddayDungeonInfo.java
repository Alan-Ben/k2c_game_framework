package NPUSServer.NPUSUserMgr.UserComp.MiddayDungeonComp;

import Common.Common_LongList;
import Common.DungeonObj.MiddayDungeon_BossInfo;
import Common.DungeonObj.MiddayDungeon_FightHero;
import Common.DungeonObj.MiddayDungeon_Info;
import Common.DungeonObj.MiddayDungeon_SettleInfo;
import Common.ServerObj.ServerObj_MiddayDungeon_FightHeroList;
import CommonEnum.EBonusPropertyType;
import CommonEnum.ECurrency;
import GS2GC.p024_DungeonOp.GS2GC_024_051_OnMiddayDungeonInfoChg;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.HeroErr;
import NPCommon.ErrMain.MiddayDungeonErr;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.NPLogDB.CommLogDB;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Pair.WCGPairLong;
import NPCommon.Util.Random;
import NPEnum.ENPItemType;
import NPEnum.ENPPlayerParam;
import NPGameRes.Refs.Dungeon.Midday.RefMiddayDungeonBox;
import NPGameRes.Refs.Dungeon.Midday.RefMiddayDungeonWave;
import NPGameRes.Refs.RefGeneral;
import NPGameRes.Refs.RefServerStartDays;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_GAIN_DUNGEON_COIN;
import NPUSServer.Common.Event.Events.Event_P_MIDDAY_ATTACK;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroInfo;
import NPUSServer.USLog;
import USDB.Bo.PlayerMiddayDungeonBO;
import USLOGDB.Bo.LogMiddayDungeonWaveSettleBO;

import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.List;

public class MiddayDungeonInfo
{
    private MiddayDungeonComponent _m_comp;
    private PlayerMiddayDungeonBO _m_bo;
    private List<MiddayDungeon_FightHero> _m_hadFightHeroList;
    private List<Long> _m_hadBorrowCidHeroList;

    public MiddayDungeonInfo(MiddayDungeonComponent _comp,PlayerMiddayDungeonBO _bo)
    {
        _m_comp = _comp;
        _m_bo = _bo;

        _m_hadFightHeroList = new ArrayList<>();
        if (_bo.getHadFightHeroList() != null)
        {
            ServerObj_MiddayDungeon_FightHeroList proto = new ServerObj_MiddayDungeon_FightHeroList();
            proto.readPackage(ByteBuffer.wrap(_bo.getHadFightHeroList()));
            _m_hadFightHeroList = proto.getHeroList();
        }

        _m_hadBorrowCidHeroList = new ArrayList<>();
        if (_bo.getHadBorrowHeroList() != null)
        {
            Common_LongList proto = new Common_LongList();
            proto.readPackage(ByteBuffer.wrap(_bo.getHadBorrowHeroList()));
            _m_hadBorrowCidHeroList = proto.getValueList();
        }
    }

    /**
     * 获取玩家数据
     * @return
     */
    public NPUSUserData getUserData()
    {
        return _m_comp.getUserData();
    }

    /**
     * 尝试刷新
     */
    public void tryRefresh(long _currentRoundStartTimeMs)
    {
        //检查是否需要刷新
        if (_m_bo.getRoundStartTimeMs() == _currentRoundStartTimeMs)
            return;

        _doRefresh(_currentRoundStartTimeMs);
    }

    /**
     * 执行刷新逻辑
     */
    private void _doRefresh(long _currentRoundStartTimeMs)
    {
        _m_hadFightHeroList.clear();
        _m_hadBorrowCidHeroList.clear();

        _m_bo.setRoundStartTimeMs(_m_comp.getUSServer().getBM(), _currentRoundStartTimeMs);
        _m_bo.setWave(_m_comp.getUSServer().getBM(), 1);
        _m_bo.setDeductedHp(_m_comp.getUSServer().getBM(), 0);

        ServerObj_MiddayDungeon_FightHeroList selfHeroUseList = new ServerObj_MiddayDungeon_FightHeroList();
        _m_bo.setHadFightHeroList(_m_comp.getUSServer().getBM(),selfHeroUseList.makePackage().array());

        Common_LongList borrowHeroUseList = new Common_LongList();
        _m_bo.setHadBorrowHeroList(_m_comp.getUSServer().getBM(), borrowHeroUseList.makePackage().array());
        _m_bo.saveAllMarked(_m_comp.getUSServer().getBM());

        onInfoChg();
    }

    /**
     * 清除战斗记录
     */
    public void cleanRecord()
    {
        _m_hadFightHeroList.clear();
        _m_hadBorrowCidHeroList.clear();

        ServerObj_MiddayDungeon_FightHeroList selfHeroUseList = new ServerObj_MiddayDungeon_FightHeroList();
        _m_bo.setHadFightHeroList(_m_comp.getUSServer().getBM(),selfHeroUseList.makePackage().array());

        Common_LongList borrowHeroUseList = new Common_LongList();
        _m_bo.setHadBorrowHeroList(_m_comp.getUSServer().getBM(), borrowHeroUseList.makePackage().array());
        _m_bo.saveAllMarked(_m_comp.getUSServer().getBM());

        onInfoChg();
    }

    /**
     * 查询大臣使用记录
     * @param _heroId
     * @return
     */
    private MiddayDungeon_FightHero _lookupHeroUseRecord(long _heroId)
    {
        MiddayDungeon_FightHero record = null;
        for (MiddayDungeon_FightHero fightHero : _m_hadFightHeroList)
        {
            if (fightHero.getHeroId() == _heroId)
                record = fightHero;
        }
        return record;
    }

    /**
     * 攻击
     * @return
     */
    public ResultOne<MiddayDungeon_SettleInfo> attack(long _heroId, NPPlayerContext _context)
    {
        //尝试刷新
        WCGPairLong roundTime = getUserData().getUSServer().getMiddayDungeonMgr().getRoundTime();

        long nowTimeMS = CommonFunc.getNowTimeMS();
        //检查是否在战斗时间内
        if (!checkInFightTime(nowTimeMS, roundTime))
            return ResultOne.failed(MiddayDungeonErr.NOT_IN_FIGHT_TIME);

        tryRefresh(roundTime.first());

        //查询波次信息
        RefMiddayDungeonWave refWave = RefMiddayDungeonWave.getMgr().get(_m_bo.getWave());
        if (refWave == null)
            return ResultOne.failed(CommErr.REF_NOT_FOUND);

        //查询大臣信息
        HeroInfo heroInfo = getUserData().getHeroComponent().lookupHero(_heroId);
        if (heroInfo == null)
            return ResultOne.failed(HeroErr.HERO_NOT_FOUND);

        //获取大臣可以使用的次数
        long canAttackTimes = heroInfo.getPlayerBonusAddValue(EBonusPropertyType.MIDDAY_DUNGEON_HERO_ATTACK_EXTRA_TIMES) + 1;

        //查询大臣派遣记录
        MiddayDungeon_FightHero record = _lookupHeroUseRecord(heroInfo.getHeroId());
        if (record != null && record.getNum() >= canAttackTimes)
            return ResultOne.failed(MiddayDungeonErr.HERO_USE_REACH_LIMIT);

        //增加使用次数
        if (record == null)
        {
            record = new MiddayDungeon_FightHero();
            record.setHeroId(_heroId);
            record.setNum((short) 1);
            _m_hadFightHeroList.add(record);
        } else
        {
            record.setNum((short) (record.getNum() + 1));
        }

        //扣除血量
        _m_bo.setDeductedHp(_m_comp.getUSServer().getBM(), _m_bo.getDeductedHp() + heroInfo.getPower());
        ServerObj_MiddayDungeon_FightHeroList selfHeroUseList = new ServerObj_MiddayDungeon_FightHeroList();
        selfHeroUseList.getHeroList().addAll(_m_hadFightHeroList);
        _m_bo.setHadFightHeroList(_m_comp.getUSServer().getBM(),selfHeroUseList.makePackage().array());
        _m_bo.saveAllMarked(_m_comp.getUSServer().getBM());

        //尝试结算
        MiddayDungeon_SettleInfo settleInfo = trySettle(refWave, _context);

        //推送变更
        onInfoChg();

        //触发事件
        Event_P_MIDDAY_ATTACK evt = new Event_P_MIDDAY_ATTACK(_context);
        getUserData().onLogicEvent(evt);

        return ResultOne.succ(settleInfo);
    }

    /**
     * 检查是否在战斗时间内
     * @param _nowTimeMS
     * @param _roundTime
     * @return
     */
    private boolean checkInFightTime(long _nowTimeMS, WCGPairLong _roundTime)
    {
        return _nowTimeMS >= _roundTime.first() && _nowTimeMS <= _roundTime.second();
    }

    /**
     * 攻击
     * @return
     */
    public ResultOne<MiddayDungeon_SettleInfo> borrowAttack(long _targetCid, long _heroPower, NPPlayerContext _context)
    {
        //尝试刷新
        WCGPairLong roundTime = getUserData().getUSServer().getMiddayDungeonMgr().getRoundTime();

        long nowTimeMS = CommonFunc.getNowTimeMS();
        //检查是否在战斗时间内
        if (!checkInFightTime(nowTimeMS, roundTime))
            return ResultOne.failed(MiddayDungeonErr.NOT_IN_FIGHT_TIME);

        tryRefresh(roundTime.first());

        //查询波次信息
        RefMiddayDungeonWave refWave = RefMiddayDungeonWave.getMgr().get(_m_bo.getWave());
        if (refWave == null)
            return ResultOne.failed(CommErr.REF_NOT_FOUND);

        //查询是否借用过了
        if (_m_hadBorrowCidHeroList.contains(_targetCid))
            return ResultOne.failed(MiddayDungeonErr.ALREADY_BORROW_GUILD_HERO);

        //消耗借用次数
        boolean consumeSucc = getUserData().spendItem(ENPItemType.FIXED_CD, RefGeneral.Ref().midday_dungeon_can_borrow_guild_hero_num_fixed_cd_id, 1, _context);
        if (!consumeSucc)
            return ResultOne.failed(MiddayDungeonErr.BORROW_HERO_REACH_LIMIT);

        _m_hadBorrowCidHeroList.add(_targetCid);

        //扣除血量
        _m_bo.setDeductedHp(_m_comp.getUSServer().getBM(), _m_bo.getDeductedHp() + _heroPower);
        Common_LongList proto = new Common_LongList();
        proto.getValueList().addAll(_m_hadBorrowCidHeroList);
        _m_bo.setHadBorrowHeroList(_m_comp.getUSServer().getBM(),proto.makePackage().array());
        _m_bo.saveAllMarked(_m_comp.getUSServer().getBM());

        //尝试结算
        MiddayDungeon_SettleInfo settleInfo = trySettle(refWave, _context);

        //推送变更
        onInfoChg();

        //尝试结算
        return ResultOne.succ(settleInfo);
    }

    /**
     * 尝试结算当前Boss
     * @param _refWave 波次配置
     * @param _context 玩家上下文
     * @return 结算信息
     */
    public MiddayDungeon_SettleInfo trySettle(RefMiddayDungeonWave _refWave, NPPlayerContext _context)
    {
        //查询当前血量
        if (_m_bo.getDeductedHp() < _refWave.boss_blood)
            return null;

        //记录本次攻击造成的伤害（累计伤害）
        long damage = _m_bo.getDeductedHp();
        int wave = _m_bo.getWave();

        _m_bo.setWave(_m_comp.getUSServer().getBM(), _m_bo.getWave() + 1);
        _m_bo.setDeductedHp(_m_comp.getUSServer().getBM(), 0);
        _m_bo.saveAllMarked(_m_comp.getUSServer().getBM());

        //计算相关奖励
        MiddayDungeon_SettleInfo settleInfo = new MiddayDungeon_SettleInfo();
        settleInfo.setIsDefeat(true);

        //1.击败奖励
        //金币奖励=玩家总村庄收益*收益倍数(万分比,在general中配置)
        long goldNum = (long) Math.ceil(getUserData().getPlayerComponent().getEarnings() * RefGeneral.Ref().midday_dungeon_gold_profit_ratio / 10000d);
        //每个boss的伙伴经验奖励=伙伴经验奖励倍数*基础伙伴经验
        long heroExp = 0;
        //获取开服时间对应的配置
        RefServerStartDays refServerStartDays = RefServerStartDays.getMgr().getRefByDay(getUserData().getUSServer().getServerStartDay());
        if (refServerStartDays != null)
        {
            heroExp = (long) Math.ceil(_refWave.hero_exp_reward_ratio * refServerStartDays.midday_dungeon_base_hero_exp / 10000d);
        }else
        {
            USLog.error(getUserData().getUSServer(), "MiddayDungeonInfo trySettle refServerStartDays not found, cid:{} refWaveId:{} ", getUserData().getCid(), _refWave.Id());
        }
        //获取奖励
        List<NPCommonCostItem> rewardList = new ArrayList<>();
        rewardList.add(new NPCommonCostItem(ENPItemType.CURRENCY, ECurrency.DUNGEON_COIN.ordinal(), _refWave.dungeon_coin));
        rewardList.add(new NPCommonCostItem(ENPItemType.CURRENCY, ECurrency.SILVER.ordinal(), goldNum));
        rewardList.add(new NPCommonCostItem(ENPItemType.CURRENCY, ECurrency.HERO_EXP.ordinal(), heroExp));
        getUserData().gainItemList(rewardList, _context);

        _context.getCollector().fillProtoList(settleInfo.getDefeatRewardList());

        //触发排行榜事件
        getUserData().onLogicEvent(new Event_P_GAIN_DUNGEON_COIN(_context, _refWave.dungeon_coin));

        //2.宝箱掉落
        if (_refWave.box_id != 0 && Random.nextInt(10000) < _refWave.box_drop_per)
        {
            //尝试添加宝箱（受服务器单轮上限影响）
            if (getUserData().getUSServer().getMiddayDungeonMgr().tryAddBox())
            {
                //获取宝箱掉落
                RefMiddayDungeonBox refBox = RefMiddayDungeonBox.getMgr().get(_refWave.box_id);
                if (refBox != null)
                {
                    NPPlayerContext boxDropContext = NPPlayerContext.createNew(_context);
                    getUserData().gainItemList(refBox.drop_draw_item_list, boxDropContext);

                    //发送宝箱
                    getUserData().getUSServer().getMiddayDungeonMgr().getBoxMgr().addBox(refBox, getUserData(), CommonFunc.getNowTimeMS());

                    settleInfo.setDropBoxId(_refWave.box_id);
                    boxDropContext.getCollector().fillProtoList(settleInfo.getBoxRewardList());
                } else
                {
                    USLog.error(getUserData().getUSServer(), "MiddayDungeonInfo trySettle box drop ref not found, cid:{} refWaveId:{} boxId:{} ",
                            getUserData().getCid(), _refWave.Id(), _refWave.box_id);
                }
            }
        }

        BM bmObj = getUserData().getUSServer().getBM();
        //记录午间副本攻击日志
        LogMiddayDungeonWaveSettleBO logBo = new LogMiddayDungeonWaveSettleBO();
        logBo.setCid(bmObj, getUserData().getCid());
        logBo.setPlayerLevel(bmObj, (int) getUserData().getParam(ENPPlayerParam.LEVEL));
        logBo.setTotalHeroPower(bmObj, getUserData().getHeroComponent().getTotalPower());
        logBo.setWave(bmObj, wave);
        logBo.setDamage(bmObj, damage);
        logBo.setHeroExpReward(bmObj, heroExp);
        logBo.setGotBox(bmObj, settleInfo.getDropBoxId() != 0 ? 1 : 0);
        CommLogDB.log(bmObj, logBo);

        return settleInfo;
    }

    /**
     * 数据变化
     */
    public void onInfoChg()
    {
        getUserData().sendMsgToGC(new GS2GC_024_051_OnMiddayDungeonInfoChg(makeInfo()));
    }

    /**
     * 构造Boss信息
     * @return
     */
    public MiddayDungeon_BossInfo makeBossInfo()
    {
        MiddayDungeon_BossInfo info = new MiddayDungeon_BossInfo();
        info.setWave(_m_bo.getWave());
        info.setDeductedHp(_m_bo.getDeductedHp());
        return info;
    }

    /**
     * 构造副本信息
     * @return
     */
    public MiddayDungeon_Info makeInfo()
    {
        MiddayDungeon_Info info = new MiddayDungeon_Info();
        info.setRoundStartTimeMS(_m_bo.getRoundStartTimeMs());
        info.setBossInfo(makeBossInfo());
        info.getHadFightHeroList().addAll(_m_hadFightHeroList);
        info.getBorrowCidList().addAll(_m_hadBorrowCidHeroList);
        return info;
    }
}

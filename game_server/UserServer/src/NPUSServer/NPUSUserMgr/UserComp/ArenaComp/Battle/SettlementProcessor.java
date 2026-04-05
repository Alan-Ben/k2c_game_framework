package NPUSServer.NPUSUserMgr.UserComp.ArenaComp.Battle;

import Common.ArenaObj.Arena_BattleResult;
import CommonEnum.EBonusPropertyType;
import CommonEnum.ECurrency;
import MJLog.MJEventLog;
import NPCommon.CommonObj.NPItemCostCollector_nosafe;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPItemType;
import NPEnum.ENPPlayerPropertyType;
import NPEnum.ENPPlayerRecordParam;
import NPGameRes.Refs.Arena.RefArenaFinalReward;
import NPGameRes.Refs.Arena.RefArenaSelectAttackConsume;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_ARENA_DEFEAT_HERO;
import NPUSServer.Common.Event.Events.Event_P_ARENA_INFLUENCE_CHG;
import NPUSServer.Common.Event.Events.Event_P_ARENA_SELECT_ATTACK;
import NPUSServer.NPEvent.EventMgr.EventObj.NPGlobalUserEventObj;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ArenaComp.ArenaFunc;
import NPUSServer.NPUSUserMgr.UserComp.ArenaComp.EArenaAttackType;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroInfo;
import NPUSServer.USLog;

/**
 * 竞技场结算处理器
 */
public class SettlementProcessor
{
    /**
     * 处理回合结算
     * @param userData           用户数据
     * @param defeatHeroNum      击败英雄数
     * @param opponentCid        对手CID
     * @param _opponentHeroNum   对手英雄数
     * @param isBot              是否机器人
     * @param botName            机器人名称
     * @param heroId             英雄ID
     * @param attackType         攻击类型
     * @param selectAttackItemId 选择攻击道具ID
     * @param battleResult       战斗结果
     * @param context            玩家上下文
     * @param opponentTotalHp    对手总血量
     * @param initialHp          起始血量
     * @param finalHp            剩余血量
     * @param buffDetail         增益明细，格式：{buff_id:使用次数,...}
     */
    public static void processSettlement(
            NPUSUserData userData,
            int defeatHeroNum,
            long opponentCid,
            int _opponentHeroNum,
            boolean isBot,
            String botName,
            long heroId,
            int attackType,
            long selectAttackItemId,
            Arena_BattleResult battleResult,
            NPPlayerContext context,
            long opponentTotalHp,
            long initialHp,
            long finalHp,
            String buffDetail)
    {
        long nowTimeMS = CommonFunc.getNowTimeMS();

        // 计算倍率
        int ratio = 1;

        RefArenaSelectAttackConsume refItem = null;
        if (selectAttackItemId != 0)
        {
            refItem = RefArenaSelectAttackConsume.getMgr().get(selectAttackItemId);
            if (refItem != null)
                ratio = Math.max(1, refItem.ratio);

            userData.getRecordComponent().addRecord(ENPPlayerRecordParam.ARENA_SELECT_ATTACK_TIME, ratio, context);

            userData.onLogicEvent(new Event_P_ARENA_SELECT_ATTACK(context, ratio));
        }

        // 获取英雄信息用于计算加成
        HeroInfo heroInfo = userData.getHeroComponent().lookupHero(heroId);

        // 设置战斗结果
        int opponentDeductInfluence = defeatHeroNum * RefGeneral.Ref().arena_deduct_influence_for_each_hero_defeated;

        // 计算影响力获得，加上万分比加成
        int gainInfluence = defeatHeroNum * RefGeneral.Ref().arena_gain_influence_for_defeating_each_hero * ratio;
        if (heroInfo != null) {
            long influencePer = heroInfo.getPlayerBonusAddValue(EBonusPropertyType.ARENA_GAIN_INFLUENCE_PER);
            gainInfluence = (int) (gainInfluence * (10000 + influencePer) / 10000);
        }

        battleResult.setHadDefeatNum(defeatHeroNum);
        battleResult.setOpponentHeroNum(_opponentHeroNum);
        battleResult.setOpponentDeductinfluence(opponentDeductInfluence);
        battleResult.setGainInfluence(gainInfluence);
        battleResult.setSelectAttackItemId(selectAttackItemId);
        battleResult.setIsSettle(true);

        // 处理谈判奖励实力提升
        if (refItem != null)
        {
            long addPower = ratio * defeatHeroNum * userData.getPlayerComponent().getPropertyMgr().getValue(ENPPlayerPropertyType.ARENA_IDENTITY_EFFECT_VALUE);
            battleResult.setAddPower(addPower);

            if (heroInfo != null)
            {
                heroInfo.addArenaAddPower(addPower, context);

                // 战胜对方全部伙伴后增加竞技场币
                if (defeatHeroNum == _opponentHeroNum)
                {
                    long eachDefeatCanGainCoin = heroInfo.getPlayerBonusAddValue(EBonusPropertyType.ARENA_COINS);
                    userData.gainItem(ENPItemType.CURRENCY, ECurrency.ARENA_COIN.ordinal(), eachDefeatCanGainCoin * ratio, context);
                }

            } else
            {
                USLog.error(userData.getUSServer(), "SettlementProcessor processSettlement heroInfo is null, cid:{} heroId:{} addPower:{}",
                        userData.getCid(), heroId, addPower);
            }
        }

        // 结算奖励
        NPItemCostCollector_nosafe itemCollector = new NPItemCostCollector_nosafe();

        // 最终奖励
        RefArenaFinalReward refReward = RefArenaFinalReward.getMgr().getRefByDefeatNum(defeatHeroNum);
        if (refReward != null)
            itemCollector.addItemList(refReward.reward_item_list);

        // 发放奖励
        userData.gainItemList(CommonFunc.itemMultiple(itemCollector.getItemList(), ratio), context);
        context.getCollector().fillProtoList(battleResult.getGainItem());

        // 名人榜处理
        if (defeatHeroNum >= RefGeneral.Ref().arena_celebrity_rank_up_need_defeat_hero)
            userData.getUSServer().getArenaCelebrityRankMgr().addRankItem(
                    userData, opponentCid, isBot, botName, defeatHeroNum, attackType != EArenaAttackType.RANDOM.ordinal(), nowTimeMS);

        // 触发事件
        userData.onLogicEvent(new Event_P_ARENA_INFLUENCE_CHG(context, gainInfluence));
        userData.onLogicEvent(new Event_P_ARENA_DEFEAT_HERO(context, defeatHeroNum));

        // 记录
        userData.getRecordComponent().addRecord(ENPPlayerRecordParam.ARENA_DEFEAT_HERO_COUNT, defeatHeroNum, context);
        userData.getRecordComponent().setGtRecord(ENPPlayerRecordParam.ARENA_MAX_DEFEAT_HERO_NUM, defeatHeroNum, context);
        userData.getRecordComponent().addRecord(ENPPlayerRecordParam.ARENA_ATTACK_TIMES, ratio, context);

        // 记录已攻击过的对手（用于优化匹配算法）
        userData.getArenaComponent().recordAttackOpponent(opponentCid);

        // 对手处理, 机器人不处理
        if (!isBot)
        {
            // 对手触发影响力变化事件
            Event_P_ARENA_INFLUENCE_CHG opponentEvent = new Event_P_ARENA_INFLUENCE_CHG(context, -opponentDeductInfluence);
            userData.getUSServer().getGlobalEventHandlerMgr().handle(opponentEvent, new NPGlobalUserEventObj(opponentCid));

            // 根据攻击类型发送消息
            if (attackType == EArenaAttackType.CELEBRITY_RANK.ordinal())
            {
                ArenaFunc.addFightBackMsg(opponentCid, userData, defeatHeroNum, opponentDeductInfluence, nowTimeMS);
            } else if (attackType == EArenaAttackType.RANDOM.ordinal())
            {
                ArenaFunc.addFightReportMsg(opponentCid, userData, defeatHeroNum, opponentDeductInfluence, nowTimeMS);
            }

            ArenaFunc.recordBeenDefeatHeroNum(opponentCid, defeatHeroNum, userData.getUSServer());
        }

        // 记录碰撞测试挑战日志
        MJEventLog.logArenaBattle(
                userData,
                heroId,                    // 伙伴id
                opponentCid,               // 对手cid
                opponentTotalHp,           // 对手总血量
                defeatHeroNum,             // 击败对手伙伴数量
                initialHp,                 // 起始血量
                finalHp,                   // 剩余血量
                buffDetail,                // 增益明细
                defeatHeroNum == _opponentHeroNum ? gainInfluence : 0                   // 变更积分
        );
    }
}
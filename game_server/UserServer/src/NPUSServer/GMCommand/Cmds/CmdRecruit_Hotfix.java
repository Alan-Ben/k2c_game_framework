package NPUSServer.GMCommand.Cmds;

import NPCommon.DB.BM.BM;
import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.Log.CommLog;
import NPUSServer.GMCommand.UsCmdBase;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroComponent;
import NPUSServer.NPUSUserMgr.UserComp.RecruitComp.RecruitComponent;
import NPUSServer.USLog;
import USDB.Bo.PlayerRecruitBO;

import java.lang.reflect.Field;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 * 招募相关GM命令
 */
@ACommander(comment = "招募相关命令", name = "recruit_hotfix")
public class CmdRecruit_Hotfix extends UsCmdBase {

    /**
     * 修正recruit_id错误数据
     * 使用方法: recruit fixId
     *
     * 修正逻辑：
     * 1. 从内存中获取玩家的recruit记录（301-304）
     * 2. 从HeroComponent查询玩家是否拥有对应的英雄
     * 3. 根据英雄ID反推正确的recruit_id
     * 4. 更新数据库和内存
     */
    @ACommand(comment = "修正招募ID错误数据")
    public String fixId() {
        long cid = getOwner().getCid();
        BM bm = getOwner().getUSServer().getBM();
        RecruitComponent recruitComp = getOwner().getRecruitComponent();
        HeroComponent heroComp = getOwner().getHeroComponent();

        // 旧配置的映射：recruit_id -> 给的英雄ID
        Map<Long, Integer> oldRecruitToHero = new HashMap<>();
        oldRecruitToHero.put(301L, 1505);
        oldRecruitToHero.put(302L, 1506);
        oldRecruitToHero.put(303L, 1507);
        oldRecruitToHero.put(304L, 1508);

        // 新配置的映射：英雄ID -> 对应的recruit_id
        Map<Integer, Long> heroToNewRecruit = new HashMap<>();
        heroToNewRecruit.put(1505, 303L);
        heroToNewRecruit.put(1506, 304L);
        heroToNewRecruit.put(1507, 302L);
        heroToNewRecruit.put(1508, 301L);

        try {
            // 通过反射获取_m_recruitList
            Field field = RecruitComponent.class.getDeclaredField("_m_recruitList");
            field.setAccessible(true);
            @SuppressWarnings("unchecked")
            List<Long> recruitList = (List<Long>) field.get(recruitComp);

            // 统计玩家拥有的相关英雄数量（1505-1508）
            int ownedHeroCount = 0;
            for (int heroId : oldRecruitToHero.values()) {
                if (heroComp.lookupHero(heroId) != null) {
                    ownedHeroCount++;
                }
            }

            // 如果玩家拥有所有4个英雄，说明已经兑换了所有大臣，无需修正
            if (ownedHeroCount == 4) {
                return "CmdRecruit.fixId - player has all 4 heroes, no need to fix for cid=" + cid;
            }

            // 收集玩家拥有的英雄对应的正确recruit_id
            List<Long> correctRecruitIds = new ArrayList<>();
            for (int heroId : oldRecruitToHero.values()) {
                if (heroComp.lookupHero(heroId) != null) {
                    // 玩家拥有这个英雄，找到对应的新配置recruit_id
                    long correctRecruitId = heroToNewRecruit.get(heroId);
                    correctRecruitIds.add(correctRecruitId);
                }
            }

            if (correctRecruitIds.isEmpty()) {
                return "CmdRecruit.fixId - no recruit_id need to be fixed for cid=" + cid;
            }

            // 收集玩家当前的301-304记录
            List<Long> currentRecruitIds = new ArrayList<>();
            for (Long recruitId : recruitList) {
                if (oldRecruitToHero.containsKey(recruitId)) {
                    currentRecruitIds.add(recruitId);
                }
            }

            // 判断数据是否正确：当前记录和正确记录是否一致
            if (currentRecruitIds.size() == correctRecruitIds.size() &&
                currentRecruitIds.containsAll(correctRecruitIds)) {
                return "CmdRecruit.fixId - data is already correct for cid=" + cid;
            }

            // 数据错误，需要修正：删除所有301-304的记录
            List<Long> toRemove = new ArrayList<>(currentRecruitIds);

            // 从数据库删除
            for (Long recruitId : toRemove) {
                HashMap<String, Object> conditions = new HashMap<>();
                conditions.put("cid", cid);
                conditions.put("recruit_id", recruitId);
                bm.getBM(PlayerRecruitBO.class).delAll(conditions);

                // 从内存删除
                recruitList.remove(recruitId);
            }

            // 重新插入正确的记录
            StringBuilder fixDetails = new StringBuilder();
            for (Long correctRecruitId : correctRecruitIds) {
                PlayerRecruitBO bo = new PlayerRecruitBO();
                bo.setCid(bm, cid);
                bo.setRecruitId(bm, correctRecruitId);
                bo.insert(bm);

                // 添加到内存
                recruitList.add(correctRecruitId);

                if (fixDetails.length() > 0) {
                    fixDetails.append(", ");
                }
                fixDetails.append(correctRecruitId);
            }

            CommLog.info("CmdRecruit.fixId - fixed recruit_id for cid={}: {}", cid, fixDetails.toString());

            return "CmdRecruit.fixId success. " + fixDetails;

        } catch (Exception e) {
            USLog.error(getOwner().getUSServer(),
                    "CmdRecruit.fixId - exception: cid={}, error={}",
                    cid, e.getMessage());
            return "CmdRecruit.fixId failed due to exception: " + e.getMessage();
        }
    }
}
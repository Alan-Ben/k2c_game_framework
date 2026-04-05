package USDB.Update;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlDBExcutor;
import NPCommon.DB.UpdateBase;
import NPCommon.DB.WCGDBObj;
import NPCommon.Log.CommLog;

/**
 * 大臣天赋技能ID刷库处理
 *
 * 问题背景：
 *     hero_id=1307 的大臣，skill_id=133 需要修改为 skill_id=143
 *
 * @author claude
 */
public class Update_1_0_1_4_To_1_0_1_5 extends UpdateBase {

    @Override
    public boolean run(WCGDBObj _dbObj) {
        CommLog.info("Update_1_0_1_4_To_1_0_1_5.run - start: begin hero business skill id update");

        // 更新 hero_id=1307 的 skill_id 从 133 改为 143
        if (!updateHeroBusinessSkillId(_dbObj)) {
            CommLog.error("Update_1_0_1_4_To_1_0_1_5.run - updateHeroBusinessSkillId failed");
            return false;
        }

        CommLog.info("Update_1_0_1_4_To_1_0_1_5.run - complete: hero business skill id update success");
        return true;
    }

    /**
     * 更新大臣天赋技能ID
     *
     * 执行流程：
     * 1. 查找 hero_id=1307 且 skill_id=133 的记录
     * 2. 将 skill_id 更新为 143
     * 3. 记录更新日志
     */
    private boolean updateHeroBusinessSkillId(WCGDBObj _dbObj) {
        CommLog.info("Update_1_0_1_4_To_1_0_1_5.updateHeroBusinessSkillId - start");

        // 更新 hero_id=1307 且 skill_id=133 的记录，将 skill_id 改为 143
        String sql = "UPDATE `player_hero_business_skill` SET `skill_id` = '143' WHERE `hero_id` = '1307' AND `skill_id` = '133';";

        if (!ALMySqlDBExcutor.execute(sql, _dbObj)) {
            CommLog.error("Update_1_0_1_4_To_1_0_1_5.updateHeroBusinessSkillId - update failed: hero_id=1307, skill_id=133->143");
            return false;
        }

        CommLog.info("Update_1_0_1_4_To_1_0_1_5.updateHeroBusinessSkillId - complete: updated hero_id=1307 skill_id from 133 to 143");
        return true;
    }
}

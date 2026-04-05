package USDB.Update;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlDBExcutor;
import NPCommon.DB.UpdateBase;
import NPCommon.DB.WCGDBObj;
import NPCommon.Log.CommLog;

/**
 * 清空公会加入申请数据，由于申请数据进行新的管理，所以需要清理旧数据
 *
 * @author claude
 */
public class Update_1_0_1_6_To_1_0_1_7 extends UpdateBase {

    @Override
    public boolean run(WCGDBObj _dbObj) {
        CommLog.info("Update_1_0_1_6_To_1_0_1_7.run - start: begin update");

        // 清空旧加入申请数据
        if (!clearJoinRequest(_dbObj)) {
            CommLog.error("Update_1_0_1_6_To_1_0_1_7.run - player update failed");
            return false;
        }

        CommLog.info("Update_1_0_1_6_To_1_0_1_7.run - complete: update success");
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
    private boolean clearJoinRequest(WCGDBObj _dbObj) {
        CommLog.info("Update_1_0_1_6_To_1_0_1_7.clearJoinRequest - start");

        //修正玩家数据
        String sql = "delete from guild_join_request;";

        if (!ALMySqlDBExcutor.execute(sql, _dbObj)) {
            CommLog.error("Update_1_0_1_6_To_1_0_1_7.clearJoinRequest - update failed");
        }
        else {
            CommLog.error("update suc");
        }

        CommLog.info("Update_1_0_1_6_To_1_0_1_7.clearJoinRequest - complete");
        return true;
    }
}

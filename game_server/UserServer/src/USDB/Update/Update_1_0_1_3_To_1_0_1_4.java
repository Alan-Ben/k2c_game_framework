package USDB.Update;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlDBExcutor;
import NPCommon.DB.UpdateBase;
import NPCommon.DB.WCGDBObj;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;

/**
 * GOB-8321【刷库】服务器更新，给所有有火星基地的玩家，将货币采集时间算满（24小时）
 * https://www.teambition.com/task/695a6d8cff0db5194a90c384
 * 
 * 补充说明：对已解锁的主基地进行采集时间前置，给玩家额外1天的采集量
 *
 * @author mj
 */
public class Update_1_0_1_3_To_1_0_1_4 extends UpdateBase {

    @Override
    public boolean run(WCGDBObj _dbObj) {
        CommLog.info("Update_1_0_1_3_To_1_0_1_4.run - start: begin mars tech id update");

        // ========== 更新主基地最后收集时间为1天前 ==========
        if (!updateHomeLastCollectTime(_dbObj)) {
            CommLog.error("Update_1_0_1_3_To_1_0_1_4.run - updateHomeLastCollectTime failed.");
            return false;
        }

        CommLog.info("Update_1_0_1_3_To_1_0_1_4.run - complete: updateHomeLastCollectTime success");
        return true;
    }

    /**
     * 更新主基地最后收集时间为当前时间往前减1天
     *
     * 执行流程：
     * 1. 计算目标时间（当前时间 - 1天）
     * 2. 更新所有 lastCollectTimeS > 0 的记录
     * 3. 记录更新日志
     */
    private boolean updateHomeLastCollectTime(WCGDBObj _dbObj) {
        CommLog.info("Update_1_0_1_3_To_1_0_1_4.updateHomeLastCollectTime - start");

        // 1天 = 24小时 * 60分钟 * 60秒 = 86400秒
        int oneDaySeconds = CommonFunc.getNowTimeSec() - 86400;

        // 更新所有 lastCollectTimeS > 0 的记录为当前时间 - 1天
        String sql = "UPDATE `player_mars_building_home_func` SET `lastCollectTimeS` = " + oneDaySeconds + " WHERE `lastCollectTimeS` > 0;";

        if (!ALMySqlDBExcutor.execute(sql, _dbObj)) {
            CommLog.error("Update_1_0_1_3_To_1_0_1_4.updateHomeLastCollectTime - update failed");
            return false;
        }

        CommLog.info("Update_1_0_1_3_To_1_0_1_4.updateHomeLastCollectTime - complete: updated home collect time to 1 day ago");
        return true;
    }
}

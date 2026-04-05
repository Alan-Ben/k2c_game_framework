package USDB.Update;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlDBExcutor;
import NPCommon.DB.UpdateBase;
import NPCommon.DB.WCGDBObj;
import NPCommon.Log.CommLog;

import java.util.HashMap;
import java.util.HashSet;
import java.util.Map;
import java.util.Set;

/**
 * 火星科技ID刷库处理
 * 
 * GOB-8285 研究调整后，根据研究Id变动，服务器需要进行刷库处理。将旧ID统一替换为新ID，所有等级超过5级的修改为5级
 * https://www.teambition.com/task/69593cf60d706e335fd8bf0d
 *
 * 问题背景：
 *     部分科技ID需要更新，但存在ID交换的情况（如 10501↔10601），
 *     直接按顺序update会导致唯一键冲突
 *
 * 解决方案：三阶段更新法
 *     阶段1：删除需要删除的记录（新ID不存在）
 *     阶段2：将需要更新的ID先更新为临时ID（负数）避免冲突
 *     阶段3：将临时ID更新为最终的新ID
 *
 * @author mj
 */
public class Update_1_0_1_2_To_1_0_1_3 extends UpdateBase {

    @Override
    public boolean run(WCGDBObj _dbObj) {
        CommLog.info("Update_1_0_1_2_To_1_0_1_3.run - start: begin mars tech id update");

        // ========== 阶段1：删除需要删除的记录 ==========
        if (!deleteObsoleteTechs(_dbObj)) {
            CommLog.error("Update_1_0_1_2_To_1_0_1_3.run - delete phase failed: delete obsolete techs failed");
            return false;
        }
        
        Map<Long, Long> transMap = getIdMapping();

        // ========== 阶段2：更新为临时ID（负数）==========
        if (!updateToTempIds(_dbObj, transMap)) {
            CommLog.error("Update_1_0_1_2_To_1_0_1_3.run - temp id phase failed: update to temp ids failed");
            return false;
        }

        // ========== 阶段3：更新为最终ID ==========
        if (!updateToFinalIds(_dbObj, transMap)) {
            CommLog.error("Update_1_0_1_2_To_1_0_1_3.run - final id phase failed: update to final ids failed");
            return false;
        }
        
        // ========== 更新科技等级，超过等级5的设置等级5 ==========
        if (!updateTechLvl(_dbObj)) {
            CommLog.error("Update_1_0_1_2_To_1_0_1_3.run - updateTechLvl failed.");
            return false;
        }

        CommLog.info("Update_1_0_1_2_To_1_0_1_3.run - complete: mars tech id update success");
        return true;
    }

    /**
     * 阶段1：删除需要删除的记录
     *
     * 执行流程：
     * 1. 收集所有需要删除的科技ID（新ID不存在的）
     * 2. 遍历删除每个科技ID的数据库记录
     * 3. 记录删除日志
     */
    private boolean deleteObsoleteTechs(WCGDBObj _dbObj) {
        // 需要删除的科技ID列表（新ID不存在）
        Set<Long> deleteIds = new HashSet<>();
        deleteIds.add(10301L);
        deleteIds.add(10302L);
        deleteIds.add(10701L);
        deleteIds.add(10702L);
        deleteIds.add(11101L);
        deleteIds.add(11102L);
        deleteIds.add(11501L);
        deleteIds.add(11502L);
        deleteIds.add(11901L);
        deleteIds.add(11902L);
        deleteIds.add(12301L);
        deleteIds.add(12302L);
        deleteIds.add(12701L);
        deleteIds.add(12702L);

        CommLog.info("Update_1_0_1_2_To_1_0_1_3.deleteObsoleteTechs - start: delete count={}", deleteIds.size());

        for (Long techId : deleteIds) {
            String sql = "DELETE FROM `player_mars_tech` WHERE `techId` = '" + techId + "';";
            if (!ALMySqlDBExcutor.execute(sql, _dbObj)) {
                CommLog.error("Update_1_0_1_2_To_1_0_1_3.deleteObsoleteTechs - delete failed: techId={}", techId);
                return false;
            }
        }

        CommLog.info("Update_1_0_1_2_To_1_0_1_3.deleteObsoleteTechs - complete: deleted {} techs", deleteIds.size());
        return true;
    }

    /**
     * 阶段2：更新为临时ID（负数）
     *
     * 执行流程：
     * 1. 获取旧ID → 新ID 映射表
     * 2. 将所有旧ID先更新为临时ID（使用负数避免唯一键冲突）
     * 3. 记录更新日志
     *
     * 说明：使用临时ID是为了避免交换ID时的冲突
     *      例如：10501 → 10601，10601 → 10501
     *      如果直接更新会导致唯一键冲突
     */
    private boolean updateToTempIds(WCGDBObj _dbObj, Map<Long, Long> _transMap) {
        // 旧ID → 新ID 映射表
        Map<Long, Long> idMapping = _transMap;

        CommLog.info("Update_1_0_1_2_To_1_0_1_3.updateToTempIds - start: update count={}", idMapping.size());

        // 先将所有需要更新的ID改为临时ID（使用负数避免冲突）
        for (Map.Entry<Long, Long> entry : idMapping.entrySet()) {
            long oldId = entry.getKey();
            long tempId = -oldId; // 使用负数作为临时ID

            String sql = "UPDATE `player_mars_tech` SET `techId` = '" + tempId + "' WHERE `techId` = '" + oldId + "';";
            if (!ALMySqlDBExcutor.execute(sql, _dbObj)) {
                CommLog.error("Update_1_0_1_2_To_1_0_1_3.updateToTempIds - update to temp failed: oldId={}, tempId={}",
                        oldId, tempId);
                return false;
            }
        }

        CommLog.info("Update_1_0_1_2_To_1_0_1_3.updateToTempIds - complete: updated {} techs to temp ids",
                idMapping.size());
        return true;
    }

    /**
     * 阶段3：更新为最终ID
     *
     * 执行流程：
     * 1. 获取旧ID → 新ID 映射表
     * 2. 将所有临时ID更新为最终的新ID
     * 3. 记录更新日志
     */
    private boolean updateToFinalIds(WCGDBObj _dbObj, Map<Long, Long> _transMap) {
        // 旧ID → 新ID 映射表
        Map<Long, Long> idMapping = _transMap;

        CommLog.info("Update_1_0_1_2_To_1_0_1_3.updateToFinalIds - start: update count={}", idMapping.size());

        // 将临时ID改为最终的新ID
        for (Map.Entry<Long, Long> entry : idMapping.entrySet()) {
            long oldId = entry.getKey();
            long tempId = -oldId;
            long newId = entry.getValue();

            String sql = "UPDATE `player_mars_tech` SET `techId` = '" + newId + "' WHERE `techId` = '" + tempId + "';";
            if (!ALMySqlDBExcutor.execute(sql, _dbObj)) {
                CommLog.error("Update_1_0_1_2_To_1_0_1_3.updateToFinalIds - update to final failed: tempId={}, newId={}",
                        tempId, newId);
                return false;
            }
        }

        CommLog.info("Update_1_0_1_2_To_1_0_1_3.updateToFinalIds - complete: updated {} techs to final ids",
                idMapping.size());
        return true;
    }
    
    /**
     * 更新科技等级，超过等级5的设置等级5
     * @param _dbObj
     * @return
     */
    private boolean updateTechLvl(WCGDBObj _dbObj) {
        CommLog.info("Update_1_0_1_2_To_1_0_1_3.updateTechLvl - start");

        // 先将所有需要更新的ID改为临时ID（使用负数避免冲突）
        String sql = "UPDATE `player_mars_tech` SET `lvl` = '5' WHERE `lvl` > '5';";
        if (!ALMySqlDBExcutor.execute(sql, _dbObj)) {
            CommLog.error("Update_1_0_1_2_To_1_0_1_3.updateTechLvl - update to temp failed");
            return false;
        }

        CommLog.info("Update_1_0_1_2_To_1_0_1_3.updateTechLvl - complete: updated suc.");
        return true;
    }

    /**
     * 获取旧ID → 新ID 映射表
     *
     * 说明：只包含需要刷库的ID，不包含：
     *   1. 需要删除的ID（阶段1已处理）
     *   2. 不需要处理的ID（旧ID = 新ID）
     *
     * @return 旧ID → 新ID 映射关系
     */
    private Map<Long, Long> getIdMapping() {
        Map<Long, Long> mapping = new HashMap<>();

        mapping.put(10101L, 10301L);
        mapping.put(10401L, 10102L);
        mapping.put(10402L, 10101L);
        mapping.put(10501L, 10601L);
        mapping.put(10601L, 10501L);
        mapping.put(10602L, 10502L);
        mapping.put(10801L, 10402L);
        mapping.put(10802L, 10401L);
        mapping.put(11001L, 10801L);
        mapping.put(11002L, 10802L);
        mapping.put(11201L, 10702L);
        mapping.put(11202L, 10701L);
        mapping.put(11301L, 11201L);
        mapping.put(11401L, 11101L);
        mapping.put(11402L, 11102L);
        mapping.put(11601L, 11002L);
        mapping.put(11602L, 11001L);
        mapping.put(11701L, 11501L);
        mapping.put(11801L, 11401L);
        mapping.put(11802L, 11402L);
        mapping.put(12001L, 11302L);
        mapping.put(12002L, 11301L);
        mapping.put(12101L, 11801L);
        mapping.put(12201L, 11701L);
        mapping.put(12202L, 11702L);
        mapping.put(12401L, 11602L);
        mapping.put(12402L, 11601L);
        mapping.put(12501L, 12101L);
        mapping.put(12601L, 12001L);
        mapping.put(12602L, 12002L);
        mapping.put(12801L, 11902L);
        mapping.put(12802L, 11901L);
        mapping.put(20301L, 20501L);
        mapping.put(20401L, 20601L);
        mapping.put(20402L, 20701L);
        mapping.put(20501L, 21001L);
        mapping.put(20601L, 21101L);
        mapping.put(20602L, 21201L);
        mapping.put(20701L, 21501L);
        mapping.put(20801L, 21601L);
        mapping.put(20901L, 21701L);
        mapping.put(21001L, 22001L);
        mapping.put(21101L, 22101L);
        mapping.put(21201L, 22401L);
        mapping.put(21301L, 22501L);

        return mapping;
    }
}

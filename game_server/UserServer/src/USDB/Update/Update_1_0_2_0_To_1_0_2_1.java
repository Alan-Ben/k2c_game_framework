package USDB.Update;

import ALMySqlCommon.ALMySqlDBConditionObj;
import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlDBExcutor;
import Common.TutorialData;
import NPCommon.DB.ErrDealer.SelectExceptionDealer;
import NPCommon.DB.UpdateBase;
import NPCommon.DB.WCGDBObj;
import NPCommon.Log.CommLog;
import USDB.Bo.PlayerClientDataBO;

import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.List;

/**
 * 引导数据旧版本迁移 - 将旧引导ID转换为新版ID
 * 放到DB升级中统一处理，避免后续版本ID变更导致长时间未登录玩家数据异常
 *
 * @author claude
 */
public class Update_1_0_2_0_To_1_0_2_1 extends UpdateBase
{
    @Override
    public boolean run(WCGDBObj _dbObj)
    {
        CommLog.info("Update_1_0_2_0_To_1_0_2_1.run - start: 开始引导数据迁移");

        // 查询所有 key=0 的客户端数据（引导数据）
        ALMySqlDBConditionObj condition = new ALMySqlDBConditionObj();
        condition.addAndEquals("`key`", 0);

        SelectExceptionDealer errDealer = new SelectExceptionDealer();
        PlayerClientDataBO template = new PlayerClientDataBO();
        ArrayList<PlayerClientDataBO> boList
                = (ArrayList<PlayerClientDataBO>) ALMySqlDBExcutor.getListByCondition(_dbObj, condition, template, errDealer);
        if (errDealer.isHasException())
        {
            CommLog.error("Update_1_0_2_0_To_1_0_2_1.run - 查询player_client_data失败");
            return false;
        }

        int migrateCount = 0;
        for (int i = 0; i < boList.size(); i++)
        {
            PlayerClientDataBO bo = boList.get(i);
            if (bo == null)
                continue;

            byte[] rawData = bo.getClientData();
            if (rawData == null)
                continue;

            // 反序列化引导数据
            TutorialData tutorial = new TutorialData();
            try
            {
                tutorial.readPackage(ByteBuffer.wrap(rawData));
            }
            catch (Exception _e)
            {
                CommLog.error("Update_1_0_2_0_To_1_0_2_1.run - 反序列化失败: cid={}", bo.getCid());
                continue;
            }

            long oldLastId = tutorial.getLastFinishForceTutorial();
            List<Long> oldList = new ArrayList<>(tutorial.getFinishTutorialId());

            // 执行迁移
            if (!Update_1_0_2_1_TutorialDataMigration.migrate(tutorial))
                continue;

            List<Long> newList = tutorial.getFinishTutorialId();
            List<Long> added = new ArrayList<>(newList);
            added.removeAll(oldList);
            List<Long> removed = new ArrayList<>(oldList);
            removed.removeAll(newList);

            CommLog.info("Update_1_0_2_0_To_1_0_2_1.run - 迁移: cid={}, lastId={}=>{}, added={}, removed={}",
                    bo.getCid(), oldLastId, tutorial.getLastFinishForceTutorial(), added, removed);

            // 重新序列化并更新数据库
            ByteBuffer newBuf = tutorial.makePackage();

            ALMySqlDBConditionObj updateCond = new ALMySqlDBConditionObj();
            updateCond.setTablesName("player_client_data");
            updateCond.addAndEquals("id", bo.getId());

            ArrayList<byte[]> byteList = new ArrayList<>();
            byteList.add(newBuf.array());

            int count = ALMySqlDBExcutor.updateByCondition(_dbObj, updateCond, "client_data=? ", byteList);
            if (count <= 0)
            {
                CommLog.error("Update_1_0_2_0_To_1_0_2_1.run - 更新失败: cid={}, id={}", bo.getCid(), bo.getId());
            }
            else
            {
                migrateCount++;
            }
        }

        CommLog.info("Update_1_0_2_0_To_1_0_2_1.run - 完成: 总数={}, 迁移数={}", boList.size(), migrateCount);
        return true;
    }
}

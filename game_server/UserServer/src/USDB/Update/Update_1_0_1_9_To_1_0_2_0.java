package USDB.Update;

import ALMySqlCommon.ALMySqlDBConditionObj;
import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlDBExcutor;
import NPCommon.DB.ErrDealer.SelectExceptionDealer;
import NPCommon.DB.UpdateBase;
import NPCommon.DB.WCGDBObj;
import NPCommon.Log.CommLog;
import NPGameRes.Refs.Quest.RefQuest;
import NPGameRes.Refs.Quest.RefQuestStep;
import NPGameRes.Refs.Quest.RefQuestTarget;
import USDB.Bo.PlayerQuestBO;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

/**
 * GOB-9398 【优化-0】主线任务数据需要进行兼容处理，在玩家当前主线任务无法查询到对应数据的情况下，需要将Id递减往前寻找第一个有数据的stepId，并修改当前step为该有效step
 * https://www.teambition.com/task/69ae640ad7546bcf4b74fe1b
 *
 * @author claude
 */
public class Update_1_0_1_9_To_1_0_2_0 extends UpdateBase
{
    @Override
    public boolean run(WCGDBObj _dbObj)
    {
        CommLog.info("Update_1_0_1_9_To_1_0_2_0.run - start: begin");

        //获取主线任务配置
        RefQuest questRef = RefQuest.getMgr().get(1L);
        if(null == questRef)
        {
            CommLog.error("Update_1_0_1_9_To_1_0_2_0.run - failed for get main quest ref.");
            return false;
        }

        //取出所有数据
        SelectExceptionDealer stepBoErrDealer = new SelectExceptionDealer();
        PlayerQuestBO stepBo = new PlayerQuestBO();
        ArrayList<PlayerQuestBO> stepBoList
                = (ArrayList<PlayerQuestBO>) ALMySqlDBExcutor.getListByCondition(_dbObj, new ALMySqlDBConditionObj(), stepBo, stepBoErrDealer);
        if (stepBoErrDealer.isHasException())
        {
            CommLog.error("Update_1_0_1_9_To_1_0_2_0.run - failed for get all stepBoList.");
            return false;
        }

        //找出需要处理的数据
        ArrayList<PlayerQuestBO> needDealStepBoList = new ArrayList<>();
        //找出需要删除的重复数据（同一玩家同一任务有多条数据的情况，保留后续阶段数据，删除前面阶段数据）
        ArrayList<PlayerQuestBO> needDuplicateStepBoList = new ArrayList<>();
        //记录已经处理过的cid，避免重复处理
        HashMap<Long, PlayerQuestBO> cidStepBoMap = new HashMap<>();
        for(int i = 0; i < stepBoList.size(); i++)
        {
            PlayerQuestBO bo = stepBoList.get(i);
            if(null == bo)
                continue;

            //只处理主线任务
            if(bo.getQuestId() != 1)
                continue;

            PlayerQuestBO preBo = cidStepBoMap.put(bo.getCid(), bo);
            if(null != preBo)
            {
                CommLog.info("Update_1_0_1_9_To_1_0_2_0.run - find duplicate stepBo for cid:{}, questId:{}, pre stepId:{} and stepId:{}",
                        bo.getCid(), bo.getQuestId(), preBo.getQuestStep(), bo.getQuestStep());
                needDealStepBoList.remove(preBo);
                needDuplicateStepBoList.add(preBo);
            }

            RefQuestStep stepRef = RefQuestStep.getMgr().get(bo.getQuestStep());
            if(null != stepRef)
                continue;

            needDealStepBoList.add(bo);
        }

        //检查处理数据
        HashMap<Long, RefQuestStep> stepRefMap = new HashMap<>();
        for(int i = 0; i < needDealStepBoList.size(); i++)
        {
            PlayerQuestBO bo = needDealStepBoList.get(i);

            if(stepRefMap.containsKey(bo.getQuestStep()))
                continue;

            //获取目标阶段数据
            RefQuestStep stepRef = questRef.getNearStepRef(bo.getQuestStep());
            if(null == stepRef)
            {
                CommLog.error("Update_1_0_1_9_To_1_0_2_0.run - failed for get near step ref, cid:{} questId:{} stepId:{}",
                        bo.getCid(), bo.getQuestId(), bo.getQuestStep());
                return false;
            }

            stepRefMap.put(bo.getQuestStep(), stepRef);
        }

        //删除重复数据
        for(int i = 0; i < needDuplicateStepBoList.size(); i++)
        {
            PlayerQuestBO bo = needDuplicateStepBoList.get(i);

            String delSql = "delete from player_quest where id=" + bo.getId();

            if (!ALMySqlDBExcutor.execute(delSql, _dbObj))
            {
                CommLog.error("Update_1_0_1_9_To_1_0_2_0.run - delete duplicate player_quest failed, sql:{}", delSql);
                return false;
            }
        }

        //处理数据
        for(int i = 0; i < needDealStepBoList.size(); i++)
        {
            PlayerQuestBO bo = needDealStepBoList.get(i);

            long preStep = bo.getQuestStep();
            RefQuestStep stepRef = stepRefMap.get(preStep);

            //清空对应任务阶段目标数据
            String delTargetSql = "delete from player_quest_target where cid=" + bo.getCid()
                    + " and quest_id=" + bo.getQuestId()
                    + " and quest_step=" + preStep;

            if (!ALMySqlDBExcutor.execute(delTargetSql, _dbObj))
            {
                CommLog.error("Update_1_0_1_9_To_1_0_2_0.run - delete player_quest_target failed, sql:{}", delTargetSql);
                return false;
            }

            //更新step数据
            String updateStepSql = "update player_quest set quest_step=" + stepRef.step_id
                    + " where id=" + bo.getId();

            if (!ALMySqlDBExcutor.execute(updateStepSql, _dbObj))
            {
                CommLog.error("Update_1_0_1_9_To_1_0_2_0.run - update player_quest failed, sql:{}", updateStepSql);
                return false;
            }

            //补充任务目标计数，以便玩家直接完成该任务
            List<RefQuestTarget> targetRefList = stepRef.listTarget;
            if(null != targetRefList && targetRefList.size() > 0)
            {
                for(int j = 0; j < targetRefList.size(); j++)
                {
                    RefQuestTarget targetRef = targetRefList.get(j);
                    if(null == targetRef)
                        continue;

                    //新增对应任务阶段目标数据
                    String insertSql = "insert into player_quest_target(cid, quest_id, quest_step, quest_target, quest_target_count) values(" + bo.getCid()
                            + ", " + bo.getQuestId()
                            + ", " + stepRef.step_id
                            + ", " + targetRef.id
                            + ", 0)";

                    if (!ALMySqlDBExcutor.execute(insertSql, _dbObj))
                    {
                        CommLog.error("Update_1_0_1_9_To_1_0_2_0.run - insert player_quest_target failed, sql:{}", insertSql);
                        return false;
                    }
                }
            }

            CommLog.info("Update_1_0_1_9_To_1_0_2_0.run - update stepBo, cid:{} quest:{} step:{} to near step:{}",
                    bo.getCid(), bo.getQuestId(), preStep, stepRef.step_id);
        }

        CommLog.info("Update_1_0_1_9_To_1_0_2_0.run - complete : {}", needDealStepBoList.size());
        return true;
    }
}

package NPGameRes.InitDealer;

import NPCommon.Log.CommLog;
import NPGameRes.GameObjs.Tower.TowerStageRefObj;
import NPGameRes.Refs.RefGeneral;
import NPGameRes.Refs.Tower.RefTowerChapter;
import NPGameRes.Refs.Tower.RefTowerChapterStage;

import java.util.ArrayList;
import java.util.List;

public class TowerInitDealer extends _ABasicInitDealer
{
    @Override
    public void dealInit()
    {
        final int STAGE_PROTECT_LIMIT = 100; // 提取为常量

        int stageIndex = 0;
        // 总起始等级
        int totalStartLevel = 1;

        List<TowerStageRefObj> allStageList = new ArrayList<>();

        // 遍历所有章节
        for (RefTowerChapter refChapter : RefTowerChapter.getMgr().getList())
        {
            if (null == refChapter)
                continue;

            RefTowerChapterStage refStage = RefTowerChapterStage.getMgr().get(refChapter.initial_stage_id);
            if (null == refStage)
            {
                CommLog.error("Ref TowerInitDealer get chapter:{} initial_stage_id:{} refChapter fail.", refChapter.id, refChapter.initial_stage_id);
                continue;
            }

            // 长度保护，避免数值配置错误进入无限循环
            int loopTime = 0;
            // 章节起始等级
            int chapterStartLevel = 1;
            // 章节关卡列表
            List<TowerStageRefObj> chapterStageList = new ArrayList<>();

            do
            {
                //触发保护机制，输出日志数值确认
                if (loopTime > STAGE_PROTECT_LIMIT)
                {
                    CommLog.error("Ref TowerInitDealer get next stage expand protected-limit, curStage:{} nextStage:{} loopTime:{}."
                            , refStage.id, refStage.next_stage_id, loopTime);
                    break;
                }

                TowerStageRefObj refObj = new TowerStageRefObj(stageIndex, totalStartLevel, chapterStartLevel, refChapter, refStage);
                allStageList.add(refObj);
                chapterStageList.add(refObj);

                loopTime++;

                stageIndex++;
                totalStartLevel += refObj.getRefChapterStage().levels_in_range_count;
                chapterStartLevel += refObj.getRefChapterStage().levels_in_range_count;

                long curStageId = refStage.id;
                long nextStageId = refStage.next_stage_id;

                // 获取下一个阶段的配置
                if (nextStageId == -1)
                    break;

                refStage = RefTowerChapterStage.getMgr().get(nextStageId);
                if (null == refStage)
                    CommLog.error("Ref TowerInitDealer get stage:{} next stage:{} refChapter fail.", curStageId, nextStageId);

            } while (refStage != null);

            refChapter.setStageList(chapterStageList);
        }

        RefGeneral.Ref().setTowerChapterStageList(allStageList);
    }
}

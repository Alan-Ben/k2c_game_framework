package NPUSServer.Tower;

import Common.TowerObj.Tower_PosInfo;
import NPGameRes.GameObjs.Tower.TowerStageRefObj;
import NPGameRes.Refs.RefGeneral;
import NPGameRes.Refs.Tower.RefTowerChapter;

import java.util.List;

public class TowerUtil
{
    public static TowerStageRefObj getStageRefObj(long _chapterId, int _chapterLevel)
    {
        return RefTowerChapter.getMgr().getStageRefObj(_chapterId, _chapterLevel);
    }

    /**
     * 获取玩家所在的总关卡等级
     * @param _curStageRefObj
     * @param _chapterLevel
     * @return
     */
    public static int getTowerTotalLevel(TowerStageRefObj _curStageRefObj, int _chapterLevel)
    {
        return _curStageRefObj.getTotalStartLevel() + _chapterLevel - _curStageRefObj.getChapterStartLevel();
    }

    /**
     * 计算指定章节和等级的关卡所需的实力值
     * @param _chapterId
     * @param _chapterLevel
     * @return
     */
    public static long calFloorNeedPower(long _chapterId, int _chapterLevel)
    {
        TowerStageRefObj stageRefObj = getStageRefObj(_chapterId, _chapterLevel);
        if (stageRefObj == null)
            return -1;

        return stageRefObj.calPower(_chapterLevel);
    }

    /**
     * 通过爬塔位置和偏移量获取对应位置
     * @param _chapterLevel
     * @param _offset
     */
    public static Tower_PosInfo getTowerStageByOffset(TowerStageRefObj _curStageRefObj, int _chapterLevel, int _offset)
    {
        List<TowerStageRefObj> towerChapterStageList = RefGeneral.Ref().towerChapterStageList;

        int stageIndex = towerChapterStageList.indexOf(_curStageRefObj);
        if (stageIndex < 0)
            return null;

        //计算玩家所在的总关卡等级
        int playerTotalLevel = getTowerTotalLevel(_curStageRefObj, _chapterLevel);

        //目标总等级
        int targetTotalLevel = playerTotalLevel + _offset;

        TowerStageRefObj targetStageRefObj = null;
        for (int j = stageIndex; j < towerChapterStageList.size(); j++)
        {
            TowerStageRefObj temp = towerChapterStageList.get(j);
            if(temp.getTotalStartLevel() > targetTotalLevel)
                break;

            targetStageRefObj = temp;
        }

        if (targetStageRefObj == null || targetStageRefObj.getTotalEndLevel() < targetTotalLevel)
            return null;

        //计算目标关卡等级
        long targetChapterId = targetStageRefObj.getRefChapter().id;
        int targetChapterLevel = targetTotalLevel - targetStageRefObj.getTotalStartLevel() + targetStageRefObj.getChapterStartLevel();

        return new Tower_PosInfo(targetChapterId, targetChapterLevel);
    }
}

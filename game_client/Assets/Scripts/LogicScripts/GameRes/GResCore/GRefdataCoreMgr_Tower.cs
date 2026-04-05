using System.Collections.Generic;

namespace GOE
{
    //爬塔相关
    public partial class GRefdataCoreMgr
    {
        private void _initTowerChapter()
        {
            if (towerChapterRefCore?.refList != null && towerChapterStageRefCore != null)
            {
                int totalLevel = 0;
                foreach (var towerChapterRef in towerChapterRefCore.refList)
                {
                    if (towerChapterRef == null)
                        continue;
                    towerChapterRef.stage_list = new List<TowerChapterStageRefObj>();
                    towerChapterRef.total_level_start = totalLevel;

                    long curStage = towerChapterRef.initial_stage_id;
                    int levelCount = 0;
                    while (curStage != -1)
                    {
                        TowerChapterStageRefObj towerChapterStageRef = towerChapterStageRefCore.getRef(curStage);
                        if (towerChapterStageRef == null)
                        {
                            curStage = -1;
                            continue;
                        }

                        towerChapterStageRef.total_level_start = totalLevel;
                        // towerChapterStageRef.chapter_stage_list_index = towerChapterRef.stage_list.Count;
                        towerChapterRef.stage_list.Add(towerChapterStageRef);
                        curStage = towerChapterStageRef.next_stage_id;

                        if (levelCount + 1 != towerChapterStageRef.level_count_initial_value)
                        {
                            Debug.LogError($"爬塔Stage数据{towerChapterStageRef.id}.level_count_initial_value is not continuous!");
                        }
                        levelCount += towerChapterStageRef.levels_in_range_count;
                        totalLevel += towerChapterStageRef.levels_in_range_count;
                    }

                    towerChapterRef.level_count = levelCount;
                }
            }
        }
        /// <summary>
        /// 获取爬塔的下一个章节
        /// </summary>
        /// <param name="chapterId"></param>
        /// <returns></returns>
        public TowerChapterRefObj getTowerNextChapterRefObj(long chapterId)
        {
            if (towerChapterRefCore == null || towerChapterRefCore.refList == null) return null;
            
            for (int i = 0; i < towerChapterRefCore.refList.Count; i++)
            {
                if (towerChapterRefCore.refList[i] != null && towerChapterRefCore.refList[i].id == chapterId)
                {
                    if (i + 1 < towerChapterRefCore.refList.Count)
                        return towerChapterRefCore.refList[i + 1];
                }
            }

            return null;
        }
        /// <summary>
        /// 获取爬塔的前一个章节
        /// </summary>
        /// <param name="chapterId"></param>
        /// <returns></returns>
        public long getTowerPreChapterId(long chapterId)
        {
            return getTowerPreChapterRef(chapterId)?.id ?? 0;
        }

        public TowerChapterRefObj getTowerPreChapterRef(long _chapterId)
        {
            if (towerChapterRefCore == null || towerChapterRefCore.refList == null) return null;
            
            for (int i = 1; i < towerChapterRefCore.refList.Count; i++)
            {
                if (towerChapterRefCore.refList[i] != null && towerChapterRefCore.refList[i].id == _chapterId)
                {
                    TowerChapterRefObj preRef = towerChapterRefCore.refList[i - 1];
                    if (preRef != null) return preRef;
                }
            }

            return null;
        }

        /// <summary>
        /// 获取当前所在关卡，的总关卡数
        /// </summary>
        /// <param name="chapterId"></param>
        /// <param name="stageId"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public long getTowerCurTotalLevel(long chapterId, int level)
        {
            if (towerChapterRefCore == null || towerChapterRefCore.refList == null || towerChapterStageRefCore == null) return 0;
         
            long totalLevel = 0;
            for (int i = 0; i < towerChapterRefCore.refList.Count; i++)
            {
                TowerChapterRefObj chapterRef = towerChapterRefCore.refList[i];
                if(chapterRef == null)
                    continue;
                if (chapterRef.id == chapterId)
                {
                    totalLevel += level;
                    return totalLevel;
                }
                totalLevel += chapterRef.level_count;
            }

            return 0;
        }
    }
}
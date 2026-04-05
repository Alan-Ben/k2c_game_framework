
using System.Collections.Generic;

namespace GOE
{
    //聊天相关
    public partial class GRefdataCoreMgr
    {
        private void _initChapter()
        {
            chapterStageRefCore.dealAllRef((_refObj) =>
            {
                if(_refObj == null)
                    return;

                for(long chapterId = _refObj.start_chapter_id;chapterId <= _refObj.end_chapter_id; chapterId++)
                {
                    ChapterRefObj chapterRefObj = chapterRefCore.getRef(chapterId);
                    if(chapterRefObj != null)
                        chapterRefObj.chapterStageRefObj = _refObj;
                }
            });
        }
        
        /// <summary>
        /// 获取指定故事的所有节列表
        /// </summary>
        /// <param name="_storyId"></param>
        /// <returns></returns>
        public List<ChapterStageRefObj> getChapterStoryAllStageList(long _storyId)
        {
            List<ChapterStageRefObj> stageList = new List<ChapterStageRefObj>();
            if (chapterStageRefCore == null)
                return stageList;

            chapterStageRefCore.dealAllRef((refObj) =>
            {
                if (refObj == null || refObj.story_id != _storyId)
                    return;

                stageList.Add(refObj);
            });

            return stageList;
        }
        
                /**
         * 根据战力比例获取对应的金币消耗倍率
         *
         * @param _powerRate 战力比例(万分比)，计算公式：(玩家战力 - 关卡战力) / 关卡战力 * 10000
         * @return 金币消耗倍率(万分比)，未找到返回10000(即1倍)
         */
        public int getCostMultipleRate(int _powerRate)
        {
            foreach (ChapterCostRefObj chapterCostRefObj in chapterCostRefCore.refList)
            {
                if(null == chapterCostRefObj)
                    continue;

                if (chapterCostRefObj.power_rate_start <= _powerRate && _powerRate <= chapterCostRefObj.power_rate_end)
                {
                    return chapterCostRefObj.cost_multiple_rate;
                }
            }
            // 未找到匹配区间，返回默认倍率1倍
            return 10000;
        }
    }
}
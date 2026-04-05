using System.Collections.Generic;

namespace GOE
{
    public class GGUIWndChapterStoryContainer : _AGGUISubWndCommonContainer<GGUIMonoChapterStoryItem, GGUIMonoChapterStoryContainer, GGUIWndChapterStoryItem>
    {
        private List<ChapterStoryShowInfo> _m_lChapterStoryInfoList;//故事列表
        
        public GGUIWndChapterStoryContainer(GGUIMonoChapterStoryContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_CHAPTER_PLOT_REWARD_DRAW, _onChapterPlotRewardDraw);
            base._onShowWnd();
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_CHAPTER_PLOT_REWARD_DRAW, _onChapterPlotRewardDraw);
            base._onHideWnd();
        }
        
        protected override GGUIWndChapterStoryItem _createItemWnd(GGUIMonoChapterStoryItem _itemMono)
        {
            GGUIWndChapterStoryItem itemWnd = new GGUIWndChapterStoryItem(_itemMono);
            return itemWnd;
        }

        protected override void _refreshItemWnd(GGUIWndChapterStoryItem _itemWnd, int _index)
        {
            if(_m_lChapterStoryInfoList == null || _index < 0 || _index >= _m_lChapterStoryInfoList.Count)
                return;
            
            _itemWnd.setData(_m_lChapterStoryInfoList[_index]);
        }

        public void setData(List<ChapterStoryShowInfo> _chapterStoryInfoList)
        {
            _m_lChapterStoryInfoList = _chapterStoryInfoList;

            refreshWnd(_m_lChapterStoryInfoList?.Count ?? 0);
        }
        
        /// <summary>
        /// 当章节剧情奖励被领取时调用
        /// </summary>
        /// <param name="_objs"></param>
        private void _onChapterPlotRewardDraw(params object[] _objs)
        {
            if(_objs == null || _objs.Length < 1 || !(_objs[0] is List<long> _plotIdList) || _m_lChapterStoryInfoList == null)
                return;
            
            // 找到包含该剧情的章节关卡 在_m_lStageShowInfoList数据列表下标
            List<int> needRefreshIndexList = new List<int>();
            ChapterStoryShowInfo storyShowInfo = null;
            for (int i = 0, count = _m_lChapterStoryInfoList.Count; i < count; i++)
            {
                storyShowInfo = _m_lChapterStoryInfoList[i];
                if(storyShowInfo == null || storyShowInfo.showStageList == null || storyShowInfo.showStageList.Count <= 0)
                    continue;
                
                bool needRefresh = false;
                foreach (var stageInfo in storyShowInfo.showStageList)
                {
                    if (stageInfo != null && stageInfo.containsPlot(_plotIdList))
                    {
                        needRefresh = true;
                        break;
                    }
                }
                
                if(needRefresh)
                    needRefreshIndexList.Add(i);
            }

            foreach (var refreshIndex in needRefreshIndexList)
            {
                if(refreshIndex < 0 || refreshIndex >= _m_lChapterStoryInfoList.Count)
                    return;
            
                refreshItem(refreshIndex);
            }
        }
    }
}
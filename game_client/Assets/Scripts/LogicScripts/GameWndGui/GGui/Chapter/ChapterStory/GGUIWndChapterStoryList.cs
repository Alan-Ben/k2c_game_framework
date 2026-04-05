using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndChapterStoryList : _ANPGGUIBasicWnd<GGUIMonoChapterStoryList>
    {
        private static GGUIWndChapterStoryList _g_instance;
        public static GGUIWndChapterStoryList instance { get { return _g_instance ??= new GGUIWndChapterStoryList(); } }
        
        private GGUIWndChapterStoryContainer _m_wChapterStoryContainer;
        
        public GGUIWndChapterStoryList() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoChapterStoryList.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoChapterStoryList.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoChapterStoryContainer != null)
                _m_wChapterStoryContainer = new GGUIWndChapterStoryContainer(wnd.monoChapterStoryContainer);
            
            ALUGUICommon.combineBtnClick(wnd.btnReturn, _onReturnBtnClick);
        }
        
        protected override void _onDiscard()
        {
            if(wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnReturn, _onReturnBtnClick);
            }
            
            _m_wChapterStoryContainer?.discard();
            _m_wChapterStoryContainer = null;
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wChapterStoryContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wChapterStoryContainer?.resetWnd();
        }

        private void _refreshWnd()
        {
            if (_m_wChapterStoryContainer != null)
            {
                _m_wChapterStoryContainer.showWnd();
                _m_wChapterStoryContainer.setData(_getShowStoryInfo());
            }
        }

        /// <summary>
        /// 获取需要展示的故事信息
        /// </summary>
        private List<ChapterStoryShowInfo> _getShowStoryInfo()
        {
            List<ChapterStoryShowInfo> showStoryList = new List<ChapterStoryShowInfo>();
         
            ChapterRefObj currentChapterRefObj = NPPlayer.instance.chapterComp.chapterRefObj;

            // 获取当前章所处故事
            long storyId = currentChapterRefObj?.chapterStageRefObj?.story_id ?? 0;
            
            float storyNormalizedProgress = 0f;//故事进度
            List<ChapterStoryStageShowInfo> chapterStageShowInfoList = new List<ChapterStoryStageShowInfo>();
            List<ChapterStagePlotRefObj> chapterStagePlotRefObjList = new List<ChapterStagePlotRefObj>();
            
            GRefdataCoreMgr.instance.chapterStoryRefCore.dealAllRef((_storyRefObj) =>
            {
                // 还未达到的故事不显示,0表示显示所有故事
                if(_storyRefObj == null || (_storyRefObj.story_id > storyId && storyId > 0))
                    return;
                
                // 获取遍历的故事下的所有节
                List<ChapterStageRefObj> chapterStageRefObjList = GRefdataCoreMgr.instance.getChapterStoryAllStageList(_storyRefObj.story_id);
                chapterStageShowInfoList.Clear();
                
                if (chapterStageRefObjList == null || chapterStageRefObjList.Count <= 0)// 若遍历的故事没有任何节, 遍历的故事也算已完成
                {
                    storyNormalizedProgress = 1f;
                }
                else
                {
                    int completeChapterCount = 0;
                    int totalChapterCount = 0;
                    foreach (var chapterStageRefObj in chapterStageRefObjList)
                    {
                        if(chapterStageRefObj == null)
                            continue;

                        for(long chapterId = chapterStageRefObj.start_chapter_id; chapterId <= chapterStageRefObj.end_chapter_id; chapterId++)
                        {
                            totalChapterCount++;
                            if (chapterId < NPPlayer.instance.chapterComp.curChapterId)
                                completeChapterCount++;
                        }

                        chapterStagePlotRefObjList.Clear();
                        if (chapterStageRefObj.plot_id_list != null)
                        {
                            foreach (var plotId in chapterStageRefObj.plot_id_list)
                            {
                                ChapterStagePlotRefObj plotRefObj = GRefdataCoreMgr.instance.chapterStagePlotRefCore.getRef(plotId);
                                if(plotRefObj != null && (plotRefObj.unlock_condition == null || plotRefObj.unlock_condition.isNoConditionOrEnable(null)))
                                    chapterStagePlotRefObjList.Add(plotRefObj);
                            }
                        }
                        if (chapterStagePlotRefObjList.Count > 0)
                        {
                            chapterStageShowInfoList.Add(new ChapterStoryStageShowInfo(chapterStageRefObj, chapterStagePlotRefObjList));
                        }
                    }

                    storyNormalizedProgress = 1f * completeChapterCount / totalChapterCount;
                }
                
                showStoryList.Add(new ChapterStoryShowInfo(_storyRefObj, storyNormalizedProgress, chapterStageShowInfoList));
            });

            return showStoryList;
        }

        /// <summary>
        /// 返回按钮被点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onReturnBtnClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Chapter.C_CHAPTER_STORY_LIST);
        }
    }

    public class ChapterStoryShowInfo
    {
        private ChapterStoryRefObj _m_rChapterStoryRefObj;
        private float _m_fNormalizedProgress;//进度
        private List<ChapterStoryStageShowInfo> _m_lShowStageList = new List<ChapterStoryStageShowInfo>();
        
        public ChapterStoryShowInfo(ChapterStoryRefObj _chapterStoryRefObj, float _normalizedProgress, List<ChapterStoryStageShowInfo> _showStageList)
        {
            updateInfo(_chapterStoryRefObj, _normalizedProgress, _showStageList);
        }
        
        public ChapterStoryRefObj chapterStoryRefObj { get { return _m_rChapterStoryRefObj; } }
        public float normalizedProgress { get { return _m_fNormalizedProgress; } }
        public List<ChapterStoryStageShowInfo> showStageList { get { return _m_lShowStageList; } }
        
        public void updateInfo(ChapterStoryRefObj _chapterStoryRefObj, float _normalizedProgress, List<ChapterStoryStageShowInfo> _showStageList)
        {
            _m_rChapterStoryRefObj = _chapterStoryRefObj;
            _m_fNormalizedProgress = _normalizedProgress;

            if (_m_lShowStageList == null)
                _m_lShowStageList = new List<ChapterStoryStageShowInfo>();
            _m_lShowStageList.Clear();
            if (_showStageList != null && _showStageList.Count > 0)
            {
                _m_lShowStageList.AddRange(_showStageList);
            }
        }
        
        /// <summary>
        /// 是否有未领取的剧情奖励
        /// </summary>
        /// <returns></returns>
        public bool hasUnDrawPlotReward()
        {
            if (_m_lShowStageList == null || _m_lShowStageList.Count <= 0)
                return false;

            foreach (var stageShowInfo in _m_lShowStageList)
            {
                if (stageShowInfo != null && stageShowInfo.hasUnDrawPlotReward())
                    return true;
            }
            
            return false;
        }
    }

    public class ChapterStoryStageShowInfo
    {
        private ChapterStageRefObj _m_rStageRefObj;
        private List<ChapterStoryStagePlotShowInfo> _m_lPlotShowInfoList = new List<ChapterStoryStagePlotShowInfo>();

        public ChapterStoryStageShowInfo(ChapterStageRefObj _chapterStageRefObj, List<ChapterStagePlotRefObj> _showPlotRefObjList)
        {
            updateInfo(_chapterStageRefObj, _showPlotRefObjList);
        }
        
        public ChapterStageRefObj chapterStageRefObj { get { return _m_rStageRefObj; } }
        public List<ChapterStoryStagePlotShowInfo> plotShowInfoList { get { return _m_lPlotShowInfoList; } }
        
        public void updateInfo(ChapterStageRefObj _chapterStageRefObj, List<ChapterStagePlotRefObj> _showPlotRefObjList)
        {
            _m_rStageRefObj = _chapterStageRefObj;

            if (_m_lPlotShowInfoList == null)
                _m_lPlotShowInfoList = new List<ChapterStoryStagePlotShowInfo>();
            _m_lPlotShowInfoList.Clear();
            foreach (var plotRefObj in _showPlotRefObjList)
            {
                _m_lPlotShowInfoList.Add(new ChapterStoryStagePlotShowInfo(plotRefObj));
            }
        }

        /// <summary>
        /// 是否有未领取的剧情奖励
        /// </summary>
        /// <returns></returns>
        public bool hasUnDrawPlotReward()
        {
            if (_m_lPlotShowInfoList == null || _m_lPlotShowInfoList.Count <= 0)
                return false;

            foreach (var plotShowInfo in _m_lPlotShowInfoList)
            {
                if (plotShowInfo != null && plotShowInfo.getState() == EGameCommonUnlockRewardType.UNLOCK_UN_GET)
                    return true;
            }
            
            return false;
        }
        
        /// <summary>
        /// 是否包含指定剧情
        /// </summary>
        /// <param name="_plotId"></param>
        /// <returns></returns>
        public bool containsPlot(long _plotId)
        {
            if (_m_lPlotShowInfoList == null || _m_lPlotShowInfoList.Count <= 0)
                return false;

            foreach (var plotShowInfo in _m_lPlotShowInfoList)
            {
                if (plotShowInfo != null && plotShowInfo.stagePlotRefObj != null && plotShowInfo.stagePlotRefObj.plot_id == _plotId)
                    return true;
            }
            
            return false;
        }

        public bool containsPlot(List<long> _plot)
        {
            if (_m_lPlotShowInfoList == null || _m_lPlotShowInfoList.Count <= 0 || _plot == null || _plot.Count <= 0)
                return false;

            foreach (var plotShowInfo in _m_lPlotShowInfoList)
            {
                if (plotShowInfo != null && plotShowInfo.stagePlotRefObj != null && _plot.Contains(plotShowInfo.stagePlotRefObj.plot_id))
                    return true;
            }
            
            return false;
        }
    }

    public class ChapterStoryStagePlotShowInfo
    {
        private ChapterStagePlotRefObj _m_rStagePlotRefObj;
        
        public ChapterStoryStagePlotShowInfo(ChapterStagePlotRefObj _stagePlotRefObj)
        {
            updateInfo(_stagePlotRefObj);
        }
        
        public ChapterStagePlotRefObj stagePlotRefObj { get { return _m_rStagePlotRefObj; } }
        
        public void updateInfo(ChapterStagePlotRefObj _stagePlotRefObj)
        {
            _m_rStagePlotRefObj = _stagePlotRefObj;
        }

        public EGameCommonUnlockRewardType getState()
        {
            if (_m_rStagePlotRefObj == null || (_m_rStagePlotRefObj.unlock_condition != null && !_m_rStagePlotRefObj.unlock_condition.isNoConditionOrEnable(null)))
                return EGameCommonUnlockRewardType.LOCK;

            return NPPlayer.instance.chapterComp.hasDrawPlotReward(_m_rStagePlotRefObj.plot_id) ? EGameCommonUnlockRewardType.UNLOCK_HAS_GET : EGameCommonUnlockRewardType.UNLOCK_UN_GET;
        }
    }
}
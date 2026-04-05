using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIWndChapterStoryStageContainer : _AGGUISubWndCommonContainer<GGUIMonoChapterStoryStageItem, GGUIMonoChapterStoryStageContainer, GGUIWndChapterStoryStageItem>
    {
        private List<ChapterStoryStageShowInfo> _m_lStageShowInfoList;
        
        public GGUIWndChapterStoryStageContainer(GGUIMonoChapterStoryStageContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        public event Action<ChapterStoryStageShowInfo, ChapterStoryStagePlotShowInfo> onClickDrawPlotRewardBtn;

        protected override void _onDiscardEx()
        {
            onClickDrawPlotRewardBtn = null;
            
            base._onDiscardEx();
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

        protected override GGUIWndChapterStoryStageItem _createItemWnd(GGUIMonoChapterStoryStageItem _itemMono)
        {
            GGUIWndChapterStoryStageItem itemWnd = new GGUIWndChapterStoryStageItem(_itemMono);
            itemWnd.onItemSizeChg += _onItemSizeChg;
            itemWnd.onClickDrawPlotRewardBtn += _onClickDrawRewardBtn;
            
            return itemWnd;
        }

        protected override void _discardItem(GGUIWndChapterStoryStageItem _itemWnd)
        {
            if (_itemWnd != null)
            {
                _itemWnd.onItemSizeChg -= _onItemSizeChg;
                _itemWnd.onClickDrawPlotRewardBtn -= _onClickDrawRewardBtn;
            }
            base._discardItem(_itemWnd);
        }

        protected override void _refreshItemWnd(GGUIWndChapterStoryStageItem _itemWnd, int _index)
        {
            if (_m_lStageShowInfoList == null || _index < 0 || _index >= _m_lStageShowInfoList.Count)
                return;

            _itemWnd.setData(_m_lStageShowInfoList[_index]);
        }

        public void setData(List<ChapterStoryStageShowInfo> _chapterStoryStageShowInfoList)
        {
            _m_lStageShowInfoList = _chapterStoryStageShowInfoList;

            if (wnd != null)
            {
                ALUGUICommon.setGameObjEnable(wnd.noItemShow, _chapterStoryStageShowInfoList == null || _chapterStoryStageShowInfoList.Count <= 0);
            }
            
            refreshWnd(_m_lStageShowInfoList?.Count ?? 0);
        }
        
        private void _onItemSizeChg()
        {
            if(wnd != null && wnd.itemContainer != null)
                LayoutRebuilder.MarkLayoutForRebuild((RectTransform) wnd.itemContainer.transform);
        }

        private void _onClickDrawRewardBtn(ChapterStoryStageShowInfo _stageShowInfo, ChapterStoryStagePlotShowInfo _plotShowInfo)
        {
            onClickDrawPlotRewardBtn?.Invoke(_stageShowInfo, _plotShowInfo);
        }
        
        /// <summary>
        /// 当章节剧情奖励被领取时调用
        /// </summary>
        /// <param name="_objs"></param>
        private void _onChapterPlotRewardDraw(params object[] _objs)
        {
            if(_objs == null || _objs.Length < 1 || !(_objs[0] is List<long> _plotIdList) || _m_lStageShowInfoList == null)
                return;
            
            // 找到包含该剧情的章节关卡 在_m_lStageShowInfoList数据列表下标
            List<int> needRefreshIndexList = new List<int>();
            for (int i = 0, count = _m_lStageShowInfoList.Count; i < count; i++)
            {
                ChapterStoryStageShowInfo stageShowInfo = _m_lStageShowInfoList[i];
                if(stageShowInfo != null && stageShowInfo.containsPlot(_plotIdList))
                {
                    needRefreshIndexList.Add(i);
                }
            }

            foreach (var index in needRefreshIndexList)
            {
                if(index >= 0 && index < _m_lStageShowInfoList.Count)
                    refreshItem(index);
            }
        }
    }
}
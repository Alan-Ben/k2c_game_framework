using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndChapterStoryDetail : _ANPGGUIBasicWnd<GGUIMonoChapterStoryDetail>
    {
        private static GGUIWndChapterStoryDetail _g_instance;
        public static GGUIWndChapterStoryDetail instance { get { return _g_instance ??= new GGUIWndChapterStoryDetail(); } }

        private ChapterStoryShowInfo _m_iStoryInfo;
        private List<long> _m_lCanDrawRewardPlotIdList;
        
        private NPGGuiWndTexture _m_wBannerImg;
        private GGUIWndChapterStoryStageContainer _m_wStageContainer;
        
        public GGUIWndChapterStoryDetail() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoChapterStoryDetail.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoChapterStoryDetail.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.bannerImg != null)
                _m_wBannerImg = new NPGGuiWndTexture(wnd.bannerImg);

            if (wnd.monoStageContainer != null)
            {
                _m_wStageContainer = new GGUIWndChapterStoryStageContainer(wnd.monoStageContainer);
                _m_wStageContainer.onClickDrawPlotRewardBtn += _onClickDrawPlotRewardBtn;
            }
            
            ALUGUICommon.combineBtnClick(wnd.btnReturn, _onReturnBtnClick);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnReturn, _onReturnBtnClick);
            }
            
            _m_lCanDrawRewardPlotIdList?.Clear();
            _m_lCanDrawRewardPlotIdList = null;
            
            _m_wBannerImg?.discard();
            _m_wBannerImg = null;

            if (_m_wStageContainer != null)
            {
                _m_wStageContainer.onClickDrawPlotRewardBtn -= _onClickDrawPlotRewardBtn;
                _m_wStageContainer.discard();
                _m_wStageContainer = null;       
            }
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_lCanDrawRewardPlotIdList?.Clear();
            
            _m_wBannerImg?.hideWnd();
            _m_wStageContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_lCanDrawRewardPlotIdList?.Clear();
            
            _m_wBannerImg?.discardTexture();
            _m_wStageContainer?.resetWnd();
        }

        public void setData(ChapterStoryShowInfo _storyInfo)
        {
            _m_iStoryInfo = _storyInfo;

            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(wnd == null || _m_iStoryInfo == null || _m_iStoryInfo.chapterStoryRefObj == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.storyName, TextTranslate.instance.getLanguage(_m_iStoryInfo.chapterStoryRefObj.name, _m_iStoryInfo.chapterStoryRefObj.name_args_list));

            if (_m_wBannerImg != null)
            {
                _m_wBannerImg.showWnd();
                _m_wBannerImg.setTexture(_m_iStoryInfo.chapterStoryRefObj.bg_img);
            }

            ALUGUICommon.setLabelTxt(wnd.txtProgressPercentage,
                TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, (int)(_m_iStoryInfo.normalizedProgress * 100f)));

            if (_m_wStageContainer != null)
            {
                _m_wStageContainer.showWnd();
                _m_wStageContainer.setData(_m_iStoryInfo.showStageList);
            }
        }

        private void _onClickDrawPlotRewardBtn(ChapterStoryStageShowInfo _stageShowInfo, ChapterStoryStagePlotShowInfo _plotShowInfo)
        {
            if(_m_iStoryInfo == null || _m_iStoryInfo.showStageList == null)
                return;
            
            if(_m_lCanDrawRewardPlotIdList == null)
                _m_lCanDrawRewardPlotIdList = new List<long>();
            _m_lCanDrawRewardPlotIdList.Clear();

            foreach (var showStageInfo in _m_iStoryInfo.showStageList)
            {
                if(showStageInfo == null || showStageInfo.plotShowInfoList == null)
                    continue;

                foreach (var plotShowInfo in showStageInfo.plotShowInfoList)
                {
                    if(plotShowInfo != null && plotShowInfo.stagePlotRefObj != null && plotShowInfo.getState() == EGameCommonUnlockRewardType.UNLOCK_UN_GET)
                        _m_lCanDrawRewardPlotIdList.Add(plotShowInfo.stagePlotRefObj.plot_id);
                }
            }

            if (_m_lCanDrawRewardPlotIdList.Count > 0)
            {
                NPPlayer.instance.chapterComp.reqDrawChapterPlotReward(_m_lCanDrawRewardPlotIdList, null);
            }
        }
        
        /// <summary>
        /// 点击返回按钮
        /// </summary>
        private void _onReturnBtnClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Chapter.C_CHAPTER_STORY_DETAIL);
        }
    }
}
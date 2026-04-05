using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndChapterStoryItem : _ATALBasicUISubWnd<GGUIMonoChapterStoryItem>
    {
        private ChapterStoryShowInfo _m_rChapterStoryInfo;

        private NPGGuiWndTexture _m_wBannerImg;
        
        public GGUIWndChapterStoryItem(GGUIMonoChapterStoryItem _wnd) : base(_wnd)
        {
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.bannerImg != null)
                _m_wBannerImg = new NPGGuiWndTexture(wnd.bannerImg);
            
            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClick);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onClick);
            }
            
            _m_wBannerImg?.discard();
            _m_wBannerImg = null;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wBannerImg?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wBannerImg?.discardTexture();
        }

        public void setData(ChapterStoryShowInfo _storyShowInfo)
        {
            _m_rChapterStoryInfo = _storyShowInfo;

            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(wnd == null || _m_rChapterStoryInfo == null || _m_rChapterStoryInfo.chapterStoryRefObj == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.storyName, TextTranslate.instance.getLanguage(_m_rChapterStoryInfo.chapterStoryRefObj.name, _m_rChapterStoryInfo.chapterStoryRefObj.name_args_list));

            if (_m_wBannerImg != null)
            {
                _m_wBannerImg.showWnd();
                _m_wBannerImg.setTexture(_m_rChapterStoryInfo.chapterStoryRefObj.banner_img);
            }

            ALUGUICommon.setLabelTxt(wnd.txtProgressPercentage, TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, (int)(_m_rChapterStoryInfo.normalizedProgress * 100)));
            
            ALUGUICommon.setGameObjEnable(wnd.goHasPlotRewardUnDrawShow, _m_rChapterStoryInfo.hasUnDrawPlotReward());
        }

        /// <summary>
        /// 被点击时
        /// </summary>
        /// <param name="_go"></param>
        private void _onClick(GameObject _go)
        {
            QueueMgr.instance.AddNode(new BaseOnAddContainerSceneUIWndQueueNode(EUIQueueStageType.MAIN, UINodeTagConst_Chapter.C_CHAPTER_STORY_DETAIL, false
                , false, true, null, GGUIWndChapterStoryDetail.instance, true, true, 
                () =>
                {
                    GGUIWndChapterStoryDetail.instance.setData(_m_rChapterStoryInfo);
                }));
        }
    }
}
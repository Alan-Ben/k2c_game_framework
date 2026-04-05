using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndChapterStoryStageItem : _ATALBasicUISubWnd<GGUIMonoChapterStoryStageItem>
    {
        private ChapterStoryStageShowInfo _m_stageShowInfo;

        private NPGGUIWndCommonToggleEx _m_wToggle;
        private GGUIWndChapterStoryStagePlotContainer _m_wPlotContainer;
        
        public GGUIWndChapterStoryStageItem(GGUIMonoChapterStoryStageItem _wnd) : base(_wnd)
        {
        }

        public event Action onItemSizeChg;
        public event Action<ChapterStoryStageShowInfo, ChapterStoryStagePlotShowInfo> onClickDrawPlotRewardBtn;
        
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoToggle != null)
            {
                _m_wToggle = new NPGGUIWndCommonToggleEx(wnd.monoToggle);
                _m_wToggle.clickDelegate += _onToggleClick;
                _m_wToggle.setSelected(false);
            }

            if (wnd.monoStagePlotContainer != null)
            {
                _m_wPlotContainer = new GGUIWndChapterStoryStagePlotContainer(wnd.monoStagePlotContainer);
                _m_wPlotContainer.onSizeChange += _onPlotContainerSizeChg;
                _m_wPlotContainer.onClickDrawRewardBtn += _onClickDrawRewardBtn;
            }
        }
        
        protected override void _onDiscard()
        {
            onItemSizeChg = null;
            onClickDrawPlotRewardBtn = null;
            
            if(_m_wToggle != null)
            {
                _m_wToggle.clickDelegate -= _onToggleClick;
                _m_wToggle.discard();
                _m_wToggle = null;
            }

            if (_m_wPlotContainer != null)
            {
                _m_wPlotContainer.onClickDrawRewardBtn -= _onClickDrawRewardBtn;
                _m_wPlotContainer.onSizeChange -= _onPlotContainerSizeChg;
                _m_wPlotContainer.discard();
                _m_wPlotContainer = null;
            }
        }
        
        protected override void _onShowWnd()
        {
            _m_wToggle?.showWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wToggle?.hideWnd();
            _m_wPlotContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wToggle?.resetWnd();
            _m_wPlotContainer?.resetWnd();
        }

        public void setData(ChapterStoryStageShowInfo _stageShowInfo)
        {
            _m_stageShowInfo = _stageShowInfo;

            _refreshWnd();
        }

        /// <summary>
        /// 刷新窗口
        /// </summary>
        private void _refreshWnd()
        {
            if (wnd == null || _m_stageShowInfo == null || _m_stageShowInfo.chapterStageRefObj == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtStageName, TextTranslate.instance.getLanguage(_m_stageShowInfo.chapterStageRefObj.stage_name, _m_stageShowInfo.chapterStageRefObj.stage_name_args_list));
            ALUGUICommon.setGameObjEnable(wnd.hasUnCheckedPlotShowGoList, _m_stageShowInfo.hasUnDrawPlotReward());
            
            _refreshPlotContainer();
        }

        /// <summary>
        /// 刷新剧情列表
        /// </summary>
        private void _refreshPlotContainer()
        {
            if (_m_wToggle == null || !_m_wToggle.isOn)
            {
                // 当_m_wToggle为空 或 未选中时，隐藏剧情容器
                _m_wPlotContainer?.hideWnd();

                // 设置item高度为未展开高度
                if (wnd != null && rectTransform != null)
                {
                    rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, wnd.onFoldItemHeight);
                    onItemSizeChg?.Invoke();
                }
                return;
            }

            if (_m_wPlotContainer != null)
            {
                _m_wPlotContainer.showWnd();
                _m_wPlotContainer.setData(_m_stageShowInfo?.plotShowInfoList);
            }
        }

        private void _onToggleClick(NPGGUIWndCommonToggleEx _toggle)
        {
            if(_m_wToggle == null)
                return;
            
            _m_wToggle.setSelected(!_m_wToggle.isOn);
            _refreshPlotContainer();
        }
        
        private void _onPlotContainerSizeChg()
        {
            onItemSizeChg?.Invoke();
        }
        
        private void _onClickDrawRewardBtn(ChapterStoryStagePlotShowInfo _plotShowInfo)
        {
            onClickDrawPlotRewardBtn?.Invoke(_m_stageShowInfo, _plotShowInfo);
        }
    }
}
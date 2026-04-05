
using System.Collections.Generic;
using ALPackage;
using GOE;
using JetBrains.Annotations;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 宝箱详情窗口
    /// </summary>
    public class GGUIWndNumMergeBoxDetail : _AHotfixBaseWnd<GGUIMonoNumMergeBoxDetail>
    {
        [NotNull] public static GGUIWndNumMergeBoxDetail instance { get { return _g_instance ??= new GGUIWndNumMergeBoxDetail(); } }
        private static GGUIWndNumMergeBoxDetail _g_instance;


        // 当前奖励品质概率容器
        private GGUIWndNumMergeBoxRewardQualityContainer _m_wCurrentRewardQualityContainer;
        // 下一级奖励品质概率容器
        private GGUIWndNumMergeBoxRewardQualityContainer _m_wNextRewardQualityContainer;
        // 当前奖励内容容器
        private GGUIWndNumMergeBoxRewardContainer _m_wCurrentRewardContainer;


        private GGUIWndNumMergeBoxDetail() 
            : base(EALUIWndLayer.ADDITION)
        {
        }


        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(8605); } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(8605); } }


        protected override void _onShowWnd()
        {
            _m_wCurrentRewardQualityContainer?.showWnd();
            _m_wNextRewardQualityContainer?.showWnd();
            _m_wCurrentRewardContainer?.showWnd();

            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_wCurrentRewardQualityContainer?.hideWnd();
            _m_wNextRewardQualityContainer?.hideWnd();
            _m_wCurrentRewardContainer?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_wCurrentRewardQualityContainer?.resetWnd();
            _m_wNextRewardQualityContainer?.resetWnd();
            _m_wCurrentRewardContainer?.resetWnd();
        }
        protected override void _onDiscard()
        {
            if (hotfixWnd == null)
                return;

            _m_wCurrentRewardQualityContainer?.discard();
            _m_wCurrentRewardQualityContainer = null;
            _m_wNextRewardQualityContainer?.discard();
            _m_wNextRewardQualityContainer = null;
            _m_wCurrentRewardContainer?.discard();
            _m_wCurrentRewardContainer = null;

            ALUGUICommon.uncombineBtnClick(hotfixWnd.btnClose, _onClickClose);
        }
        protected override void _onWndInitDoneHotfix()
        {
            if (hotfixWnd == null)
                return;

            // 初始化当前奖励品质概率容器
            if (hotfixWnd.monoCurrentRewardQualityContainer != null)
                _m_wCurrentRewardQualityContainer = new GGUIWndNumMergeBoxRewardQualityContainer(hotfixWnd.monoCurrentRewardQualityContainer);
            // 初始化下一级奖励品质概率容器
            if (hotfixWnd.monoNextRewardQualityContainer != null)
                _m_wNextRewardQualityContainer = new GGUIWndNumMergeBoxRewardQualityContainer(hotfixWnd.monoNextRewardQualityContainer);
            // 初始化当前奖励内容容器
            if (hotfixWnd.monoCurrentRewardContainer != null)
                _m_wCurrentRewardContainer = new GGUIWndNumMergeBoxRewardContainer(hotfixWnd.monoCurrentRewardContainer);

            ALUGUICommon.combineBtnClick(hotfixWnd.btnClose, _onClickClose);
        }


        public void refreshWnd()
        {
            if (hotfixWnd == null || !_m_bIsShow)
                return;

            NumMergeBoxInfo boxInfo = HotfixNPPlayer.instance.numMergeComponent.boxInfo;

            NumMergeBoxRefObj currentBoxRef = boxInfo.currentBoxRef;
            NumMergeBoxRefObj nextBoxRef = boxInfo.nextBoxRef;
            int currentStep = currentBoxRef?.step ?? 0;
            int nextStep = nextBoxRef?.step ?? 0;
            List<QualityProbabilityInfo> nextQualityList = nextBoxRef?.getQualityProbabilityList();
            List<QualityProbabilityInfo> currentQualityList = currentBoxRef?.getQualityProbabilityList();
            
            ALUGUICommon.setLabelTxt(hotfixWnd.txtCurrentLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, currentStep));
            _m_wCurrentRewardQualityContainer?.refreshWnd(currentQualityList);
            // 刷新当前奖励内容容器（使用品质概率列表）
            _m_wCurrentRewardContainer?.refreshWnd(currentQualityList);

            ALUGUICommon.setLabelTxt(hotfixWnd.txtNextLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, nextStep));
            // 刷新下一级品质概率容器（对比当前级）
            _m_wNextRewardQualityContainer?.refreshWnd(nextQualityList, currentQualityList);
        }


        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(HotfixUINodeTagConst.NUMMERGE_BOX_DETAIL);
        }
    }
}

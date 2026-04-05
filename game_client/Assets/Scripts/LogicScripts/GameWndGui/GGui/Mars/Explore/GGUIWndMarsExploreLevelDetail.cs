using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndMarsExploreLevelDetail : _ANPGGUIBasicWnd<GGUIMonoMarsExploreLevelDetail>
    {
        private static GGUIWndMarsExploreLevelDetail _g_instance;
        [NotNull] public static GGUIWndMarsExploreLevelDetail instance { get { return _g_instance ??= new GGUIWndMarsExploreLevelDetail(); } }

        private GGUISubWndMarsExploreEventQualityContainer _m_subWndCurEventQualityContainer;
        private GGUISubWndMarsExploreEventQualityContainer _m_subWndNextEventQualityContainer;
        private NPGGUIWndCommonItemContainer _m_subWndUpgradeRewardContainer;


        protected override string _monoAssetPath { get { return GGUIMonoMarsExploreLevelDetail.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsExploreLevelDetail.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        public GGUIWndMarsExploreLevelDetail() : base(EALUIWndLayer.ADDITION)
        {
        }


        protected override void _onShowWnd()
        {
            _m_subWndCurEventQualityContainer?.showWnd();
            _m_subWndNextEventQualityContainer?.showWnd();
            _m_subWndUpgradeRewardContainer?.showWnd();
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_subWndCurEventQualityContainer?.hideWnd();
            _m_subWndNextEventQualityContainer?.hideWnd();
            _m_subWndUpgradeRewardContainer?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_subWndCurEventQualityContainer?.resetWnd();
            _m_subWndNextEventQualityContainer?.resetWnd();
            _m_subWndUpgradeRewardContainer?.resetWnd();
        }
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);

            _m_subWndCurEventQualityContainer?.discard();
            _m_subWndCurEventQualityContainer = null;

            _m_subWndNextEventQualityContainer?.discard();
            _m_subWndNextEventQualityContainer = null;

            _m_subWndUpgradeRewardContainer?.discard();
            _m_subWndUpgradeRewardContainer = null;
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);

            if (wnd.monoCurEventQualityContainer != null)
                _m_subWndCurEventQualityContainer = new GGUISubWndMarsExploreEventQualityContainer(wnd.monoCurEventQualityContainer);

            if (wnd.monoNextEventQualityContainer != null)
                _m_subWndNextEventQualityContainer = new GGUISubWndMarsExploreEventQualityContainer(wnd.monoNextEventQualityContainer);

            if (wnd.monoUpgradeRewardContainer != null)
                _m_subWndUpgradeRewardContainer = new NPGGUIWndCommonItemContainer(wnd.monoUpgradeRewardContainer);
        }


        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            MarsExploreSubComponent exploreComp = NPPlayer.instance.marsComp.exploreSubComponent;
            MarsExploreLvlRefObj curLevelRef = exploreComp.levelRef;
            MarsExploreLvlRefObj nextLevelRef = exploreComp.nextLevelRef;

            bool isMaxLevel = nextLevelRef == null;
            wnd.setLevelMax(isMaxLevel);

            if (curLevelRef != null)
            {
                ALUGUICommon.setLabelTxt(wnd.txtCurLevel, TextTranslate.instance.getLanguage(wnd.levelTranslateKey, curLevelRef.explore_level));
                ALUGUICommon.setLabelTxt(wnd.txtCurExploreCountLimit, curLevelRef.explore_event_exist_limit);
                _m_subWndCurEventQualityContainer?.refreshWnd(curLevelRef.refresh_event_quality_list);
                _m_subWndUpgradeRewardContainer?.showItemList(curLevelRef.upgrade_gain_item_list);
                long curExploreNum = exploreComp.exploreNumTotal;
                long needExploreNum = curLevelRef.upgrade_need_explore_num;
                ALUGUICommon.setLabelTxt(wnd.txtUpgradeProcess, TextTranslate.instance.getLanguage(TransKeyConst.mars_exploreLevelUpDesc_num_num, curExploreNum, needExploreNum));
            }

            if (nextLevelRef != null)
            {
                ALUGUICommon.setLabelTxt(wnd.txtNextLevel, TextTranslate.instance.getLanguage(wnd.levelTranslateKey, nextLevelRef.explore_level));
                ALUGUICommon.setLabelTxt(wnd.txtNextExploreCountLimit, nextLevelRef.explore_event_exist_limit);
                _m_subWndNextEventQualityContainer?.refreshWnd(nextLevelRef.refresh_event_quality_list);
            }
        }


        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_EXPLORE_LEVEL_DETAIL);
        }
    }
}

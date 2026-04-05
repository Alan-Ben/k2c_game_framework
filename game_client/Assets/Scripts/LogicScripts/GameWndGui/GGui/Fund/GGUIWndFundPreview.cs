using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 基金奖励预览页面
    /// </summary>
    public class GGUIWndFundPreview : _ATALBasicUIWnd<GGUIMonoFundPreview>
    {
        public static GGUIWndFundPreview instance { get { return _g_instance ??= new GGUIWndFundPreview(); } }
        private static GGUIWndFundPreview _g_instance;

        private FundInfoSnapshot _m_fundSnapshot;
        private NPGGuiWndTexture _m_wBannerTexture;
        private NPGGUIWndCommonItemContainer _m_wFreeRewardContainer;
        private NPGGUIWndCommonItemContainer _m_wPaidRewardContainer;


        private GGUIWndFundPreview()
            : base(EALUIWndLayer.ADDITION)
        {
        }


        protected override string _monoAssetPath { get { return GGUIMonoFundPreview.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoFundPreview.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            _m_wBannerTexture?.showWnd();
            _m_wFreeRewardContainer?.showWnd();
            _m_wPaidRewardContainer?.showWnd();

            _refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_wBannerTexture?.hideWnd();
            _m_wFreeRewardContainer?.hideWnd();
            _m_wPaidRewardContainer?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_wBannerTexture?.discardTexture();
            _m_wFreeRewardContainer?.resetWnd();
            _m_wPaidRewardContainer?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_wBannerTexture?.discard();
            _m_wBannerTexture = null;
            _m_wFreeRewardContainer?.discard();
            _m_wFreeRewardContainer = null;
            _m_wPaidRewardContainer?.discard();
            _m_wPaidRewardContainer = null;
            
            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgBanner != null)
                _m_wBannerTexture = new NPGGuiWndTexture(wnd.imgBanner);
            if (wnd.monoFreeRewardContainer != null)
                _m_wFreeRewardContainer = new NPGGUIWndCommonItemContainer(wnd.monoFreeRewardContainer);
            if (wnd.monoPaidRewardContainer != null)
                _m_wPaidRewardContainer = new NPGGUIWndCommonItemContainer(wnd.monoPaidRewardContainer);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }


        /// <summary>
        /// 设置基金信息
        /// </summary>
        public void refreshWnd(FundInfoSnapshot _snapshot)
        {
            _m_fundSnapshot = _snapshot;
            _refreshWnd();
        }


        private void _refreshWnd()
        {
            if (wnd == null || _m_fundSnapshot == null || !_m_bIsShow)
                return;

            ActivityFundLevelRefObj levelRef = _m_fundSnapshot.levelRef;
            if (levelRef == null)
                return;

            //设置横幅
            _m_wBannerTexture?.setTexture(levelRef.banner);

            //设置标题
            ALUGUICommon.setLabelTxt(wnd.txtTitle, TextTranslate.instance.getLanguage(levelRef.activity_fund_name, levelRef.activity_fund_name_args));

            //设置激活提示
            ALUGUICommon.setLabelTxt(wnd.txtActivateTip, TextTranslate.instance.getLanguage(levelRef.activate_tip, levelRef.activate_tip_args));

            //刷新基础奖励列表
            _refreshFreeRewardList();

            //刷新付费奖励列表
            _refreshPaidRewardList();
        }
        /// <summary>
        /// 刷新基础奖励列表
        /// </summary>
        private void _refreshFreeRewardList()
        {
            if (_m_wFreeRewardContainer == null || _m_fundSnapshot == null)
                return;

            List<ActivityFundStepRefObj> stepRefList = _m_fundSnapshot.getStepRefList();
            List<NPCommonCostItem> freeRewardList = new List<NPCommonCostItem>();

            //遍历当前等级的所有阶段，收集所有基础奖励
            for (int i = 0; i < stepRefList.Count; i++)
            {
                ActivityFundStepRefObj stepRef = stepRefList[i];
                if (stepRef.free_reward_item_list != null)
                    freeRewardList.AddRange(stepRef.free_reward_item_list);
            }

            //显示列表（合并同类物品）
            List<_IItem> mergedList = freeRewardList.toMergeItemDataList(true);
            _m_wFreeRewardContainer.showWnd();
            _m_wFreeRewardContainer.showItemList(mergedList);
        }
        /// <summary>
        /// 刷新付费奖励列表
        /// </summary>
        private void _refreshPaidRewardList()
        {
            if (_m_wPaidRewardContainer == null || _m_fundSnapshot == null)
                return;

            List<ActivityFundStepRefObj> stepRefList = _m_fundSnapshot.getStepRefList();
            List<NPCommonCostItem> paidRewardList = new List<NPCommonCostItem>();

            //遍历当前等级的所有阶段，收集所有付费奖励
            for (int i = 0; i < stepRefList.Count; i++)
            {
                ActivityFundStepRefObj stepRef = stepRefList[i];
                if (stepRef.pay_reward_item_list != null)
                    paidRewardList.AddRange(stepRef.pay_reward_item_list);
            }

            //显示列表（合并同类物品）
            List<_IItem> mergedList = paidRewardList.toMergeItemDataList(true);
            _m_wPaidRewardContainer.showWnd();
            _m_wPaidRewardContainer.showItemList(mergedList);
        }
        private void _onClickClose(GameObject _)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_FUND_PREVIEW);
        }
    }
}

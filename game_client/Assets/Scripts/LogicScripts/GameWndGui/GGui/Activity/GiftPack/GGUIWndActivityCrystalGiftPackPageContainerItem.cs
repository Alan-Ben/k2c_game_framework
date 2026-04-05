using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 通用活动礼包界面钻石礼包页面item
    /// </summary>
    public class GGUIWndActivityCrystalGiftPackPageContainerItem : _ATALBasicUISubWnd<GGUIMonoActivityCrystalGiftPackPageContainerItem>
    {
        //钻石礼包信息
        private ActivityCrystalGiftPackItemInfo _m_crystalGiftPackInfo;
        //消耗的道具
        private NPGGUIWndCommonItem _m_wCostItem;
        //奖励列表
        private NPGGUIWndCommonItemContainer _m_wRewardContainer;

        public GGUIWndActivityCrystalGiftPackPageContainerItem(GGUIMonoActivityCrystalGiftPackPageContainerItem _mono) : base(_mono)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wCostItem?.hideWnd();
            _m_wRewardContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wCostItem?.resetWnd();
            _m_wRewardContainer?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wCostItem?.discard();
            _m_wCostItem = null;

            _m_wRewardContainer?.discard();
            _m_wRewardContainer = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnBuy, _onClickBuy);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoCostItem != null)
                _m_wCostItem = new NPGGUIWndCommonItem(wnd.monoCostItem);

            if (wnd.monoItemContainer != null)
                _m_wRewardContainer = new NPGGUIWndCommonItemContainer(wnd.monoItemContainer);

            ALUGUICommon.combineBtnClick(wnd.btnBuy, _onClickBuy);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_info"></param>
        public void setInfo(ActivityCrystalGiftPackItemInfo _info)
        {
            if (wnd == null)
                return;

            _m_crystalGiftPackInfo = _info;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_crystalGiftPackInfo == null || _m_crystalGiftPackInfo.crystalGiftPackRef == null)
                return;

            //设置消耗
            _m_wCostItem?.showWnd();
            _m_wCostItem?.setItem(_m_crystalGiftPackInfo.getCostItemByDiscount());

            //设置奖励列表
            _m_wRewardContainer?.showWnd();
            _m_wRewardContainer?.showItemList(_m_crystalGiftPackInfo.crystalGiftPackRef.item_list);

            //剩余购买次数
            ALUGUICommon.setLabelTxt(wnd.txtLeftBuyCount, TextTranslate.instance.getLanguage(TransKeyConst.common_currentTotalNum_num_num, _m_crystalGiftPackInfo.leftBuyCount, _m_crystalGiftPackInfo.crystalGiftPackRef.buy_num));

            //设置名称
            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(_m_crystalGiftPackInfo.crystalGiftPackRef.name));

            //免费显隐
            bool isFree = _m_crystalGiftPackInfo.isFree;
            ALUGUICommon.setGameObjEnable(wnd.goFreeShowList, isFree);
            ALUGUICommon.setGameObjEnable(wnd.goFreeHideList, !isFree);

            //售罄显隐
            ALUGUICommon.setGameObjEnable(wnd.goSellOutShowList, _m_crystalGiftPackInfo.isSellOut);
            ALUGUICommon.setGameObjEnable(wnd.goSellOutHideList, !_m_crystalGiftPackInfo.isSellOut);
        }

        //点击购买
        private void _onClickBuy(GameObject _go)
        {
            if (_m_crystalGiftPackInfo == null || _m_crystalGiftPackInfo.crystalGiftPackRef == null)
                return;

            //是否售罄
            if (_m_crystalGiftPackInfo.isSellOut)
            {
                //售罄提示
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.activity_itemSellOutTip_none);
                return;
            }

            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndGiftPackBatchBuy.instance, () =>
            {
                GGUIWndGiftPackBatchBuy.instance.showWnd();
                GGUIWndGiftPackBatchBuy.instance.setData(_m_crystalGiftPackInfo, _m_crystalGiftPackInfo .activityInstanceId, (_count) =>
                {
                    if (_count <= 0)
                        return;

                    //请求购买
                    NPPlayer.instance.commonActivityComp.reqBuyActivityCrystalGiftPack(
                        _m_crystalGiftPackInfo.activityInstanceId,
                        _m_crystalGiftPackInfo.crystalGiftPackRef.crystal_gift_pack_group_id,
                        _m_crystalGiftPackInfo.crystalGiftPackId,
                        _count);
                });
            }, UINodeTagConst.C_GIFT_PACK_BATCH_BUY);
        }
    }
}
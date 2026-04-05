using ALPackage;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUIWndChildBrainValueGet : _ANPGGUIBasicWnd<GGUIMonoChildBrainValueGet>
    {
        [NotNull] public static GGUIWndChildBrainValueGet instance { get { return _g_instance ??= new GGUIWndChildBrainValueGet(); } }
        private static GGUIWndChildBrainValueGet _g_instance;
        
        
        private NPGGUIWndCommonItem _m_brainGetItem;
        private GGUISubWndChildInfo _m_childInfoWnd;
        private NPGGUIWndCommonItem _m_fullForOneCostItem;
        private NPGGUIWndCommonItem _m_fullForAllCostItem;
        
        private ChildViewMgr _m_viewMgr;


        public GGUIWndChildBrainValueGet() 
            : base(EALUIWndLayer.ADDITION)
        {
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoChildBrainValueGet.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoChildBrainValueGet.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_brainGetItem?.showWnd();
            _m_childInfoWnd?.showWnd();
            _m_fullForOneCostItem?.showWnd();
            _m_fullForAllCostItem?.showWnd();

            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_brainGetItem?.hideWnd();
            _m_childInfoWnd?.hideWnd();
            _m_fullForOneCostItem?.hideWnd();
            _m_fullForAllCostItem?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_brainGetItem?.resetWnd();
            _m_childInfoWnd?.resetWnd();
            _m_fullForOneCostItem?.resetWnd();
            _m_fullForAllCostItem?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_brainGetItem?.discard();
            _m_childInfoWnd?.discard();
            _m_fullForOneCostItem?.discard();
            _m_fullForAllCostItem?.discard();
            _m_brainGetItem = null;
            _m_childInfoWnd = null;
            _m_fullForOneCostItem = null;
            _m_fullForAllCostItem = null;
            
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClicked);
            ALUGUICommon.uncombineBtnClick(wnd.btnUse, _onBtnUseClicked);
            ALUGUICommon.uncombineBtnClick(wnd.btnClickForOne, _onBtnFullForOneClicked);
            ALUGUICommon.uncombineBtnClick(wnd.btnClickForAll, _onBtnFullForAllClicked);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.monoBrainGetItem != null)
                _m_brainGetItem = new NPGGUIWndCommonItem(wnd.monoBrainGetItem);
            if (wnd.monoChildInfo != null)
                _m_childInfoWnd = new GGUISubWndChildInfo(wnd.monoChildInfo);
            if (wnd.monoCostItemForOne != null)
                _m_fullForOneCostItem = new NPGGUIWndCommonItem(wnd.monoCostItemForOne);
            if (wnd.monoCostItemForAll != null)
                _m_fullForAllCostItem = new NPGGUIWndCommonItem(wnd.monoCostItemForAll);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClicked);
            ALUGUICommon.combineBtnClick(wnd.btnUse, _onBtnUseClicked);
            ALUGUICommon.combineBtnClick(wnd.btnClickForOne, _onBtnFullForOneClicked);
            ALUGUICommon.combineBtnClick(wnd.btnClickForAll, _onBtnFullForAllClicked);
        }


        public void refreshWnd(ChildViewMgr _viewMgr)
        {
            _m_viewMgr = _viewMgr;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            NPCommonItem recoverItem = GRefdataCoreMgr.instance.npGeneral.child_seat_recover_item;
            _m_brainGetItem?.showWnd(new NPCommonCostItem(recoverItem, GCommon.getItemCount(recoverItem)));
            ChildInfo childInfo = _m_viewMgr?.curSelectSeatInfo?.childInfo;
            if (childInfo != null)
            {
                _m_childInfoWnd?.refreshWnd(childInfo);
                int needCountForOne = childInfo.getGraduatingEnergyNeed();
                _m_fullForOneCostItem?.showWnd(new NPCommonCostItem(recoverItem, needCountForOne));
                ALUGUICommon.setLabelTxt(wnd.txtEnergyValue, TextTranslate.instance.getLanguage(TransKeyConst.child_EP_current_num, childInfo.seatInfo.energy, needCountForOne));
                wnd.setStep(childInfo.step);
            }
            
            int needCountForAll = 0;
            foreach (SeatInfo seatInfo in _m_viewMgr.seatList)
            {
                if (seatInfo.childInfo == null)
                    continue;
                    
                needCountForAll += seatInfo.childInfo.getGraduatingEnergyNeed();
            }
            _m_fullForAllCostItem?.showWnd(new NPCommonCostItem(recoverItem, needCountForAll));
        }


        private void _onBtnCloseClicked(GameObject _)
        {
            QueueMgr.instance.DoUIRollBackByEsc();
        }
        private void _onBtnUseClicked(GameObject _)
        {
            if (_m_viewMgr == null)
                return;
            
            NPCommonItem commonItem = GRefdataCoreMgr.instance.npGeneral.child_seat_recover_item;
            if (commonItem == null)
                return;

            if (commonItem.itemType != ENPItemType.BAG_ITEM)
            {
                ALLog.Error("恢复脑力的道具不是 bagItem ，无法使用通用弹窗，如果确定配置没错，通知 coda 特殊处理");
                return;
            }

            if (!GCommon.isItemEnough(commonItem.itemType, commonItem.itemId, 1, true))
                return;
            
            BagItem bagItem = NPPlayer.instance.bagComp.getItem(commonItem.itemId);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndBagItemUse.instance, () =>
            {
                GGUIWndBagItemUse.instance.showWnd();
                GGUIWndBagItemUse.instance.init(bagItem, (_count) =>
                {
                    _m_viewMgr.recoverEnergy((int)_count, _isSuc =>
                    {
                        if (_isSuc)
                            _onBtnCloseClicked(null);
                    });
                }, GCommon.getItemCount(commonItem), null, null);
            }, UINodeTagConst.C_ADD_Bag_UseItemNode);
        }
        private void _onBtnFullForOneClicked(GameObject _)
        {
            ChildInfo childInfo = _m_viewMgr?.curSelectSeatInfo?.childInfo;
            if (childInfo == null)
                return;

            int needCount = childInfo.getGraduatingEnergyNeed();
            if (needCount <= 0)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.child_energyIsFullTip_none);
                return;
            }
            
            NPCommonItem consumeItem = GRefdataCoreMgr.instance.npGeneral.child_seat_recover_item;
            long itemCount = GCommon.getItemCount(consumeItem);
            if (itemCount > 1 && itemCount < needCount)
            {
                if (consumeItem.itemType != ENPItemType.BAG_ITEM)
                    ALLog.Error("恢复脑力的道具不是 bagItem ，无法使用通用弹窗，如果确定配置没错，通知 coda 特殊处理");
                else
                {
                    BagItem bagItem = NPPlayer.instance.bagComp.getItem(consumeItem.itemId);
                    QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndBagItemUse.instance, () =>
                    {
                        GGUIWndBagItemUse.instance.showWnd();
                        GGUIWndBagItemUse.instance.init(bagItem, (_count) =>
                        {
                            _m_viewMgr.recoverEnergy((int)_count, _isSuc =>
                            {
                                if (_isSuc)
                                    _onBtnCloseClicked(null);
                            });
                        }, itemCount);
                    }, UINodeTagConst.C_ADD_Bag_UseItemNode);
                    return;
                }
            }

            _m_viewMgr.recoverEnergy(needCount, _isSuc =>
            {
                if (_isSuc)
                    _onBtnCloseClicked(null);
            });
        }
        private void _onBtnFullForAllClicked(GameObject _)
        {
            if (_m_viewMgr == null)
                return;
            
            int needCount = 0;
            foreach (SeatInfo seatInfo in _m_viewMgr.seatList)
            {
                if (seatInfo.childInfo == null)
                    continue;
                
                needCount += seatInfo.childInfo.getGraduatingEnergyNeed();
            }

            if (needCount == 0)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.child_energyIsFullTip_none);
                return;
            }

            NPCommonItem consumeItem = GRefdataCoreMgr.instance.npGeneral.child_seat_recover_item;
            if (!GCommon.isItemEnough(new NPCommonCostItem(consumeItem, needCount), true))
                return;

            _m_viewMgr.recoverAllEnergy(() => { _onBtnCloseClicked(null); });
        }
    }
}
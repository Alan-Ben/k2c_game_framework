using ALPackage;
using GC2GS.p041_MarsExploreOp;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUIWndMarsBuildQueueBuy : _ANPGGUIBasicWnd<GGUIMonoMarsBuildQueueBuy>
    {
        private static GGUIWndMarsBuildQueueBuy _g_instance;
        [NotNull] public static GGUIWndMarsBuildQueueBuy instance { get { return _g_instance ??= new GGUIWndMarsBuildQueueBuy(); } }

        private NPGGUIWndCommonItem _m_wndTempUnlockCostItem;


        protected override string _monoAssetPath { get { return GGUIMonoMarsBuildQueueBuy.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsBuildQueueBuy.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        public GGUIWndMarsBuildQueueBuy() : base(EALUIWndLayer.ADDITION)
        {
        }


        protected override void _onShowWnd()
        {
            _m_wndTempUnlockCostItem?.showWnd();
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_wndTempUnlockCostItem?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_wndTempUnlockCostItem?.resetWnd();
        }
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnTempUnlock, _onClickTempUnlock);
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);

            _m_wndTempUnlockCostItem?.discard();
            _m_wndTempUnlockCostItem = null;
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnTempUnlock, _onClickTempUnlock);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);

            if (wnd.monoTimeUnlockCose != null)
                _m_wndTempUnlockCostItem = new NPGGUIWndCommonItem(wnd.monoTimeUnlockCose);
        }


        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            long times = NPPlayer.instance.recordComp.getValue(ENPPlayerRecordParam.MARS_BUILDING_TEMP_QUEUE_TIMES);
            NPCommonCostItem costItem = GRefdataCoreMgr.instance.getTimesPriceCostItem(GRefdataCoreMgr.instance.npGeneral.mars_building_temp_queue_time_price_type, times + 1);
            _m_wndTempUnlockCostItem?.setItem(costItem);
            NPCommonCostItem buffItem = GRefdataCoreMgr.instance.npGeneral.mars_temp_building_queue_gain_buff_item;
            long time = buffItem?.count ?? 0;
            ALUGUICommon.setLabelTxt(wnd.txtTimeUnlockTime, TextTranslate.instance.getLanguage(TransKeyConst.mars_tempBuildQueueTime_day, Mathf.CeilToInt((float)time / 60 / 60 / 24)));
            wnd.setIsFree(costItem is not { IsValid: true } || costItem.count == 0);
        }


        private void _onClickTempUnlock(GameObject _go)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_041_015_ReqGetTempBuildingQueue(), 
                new CommonErrCodeRequestCallbackDispatherTriggerDealer(() => _onClickClose(null)));
        }
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_BUILD_QUEUE_BUY);
        }
    }
}

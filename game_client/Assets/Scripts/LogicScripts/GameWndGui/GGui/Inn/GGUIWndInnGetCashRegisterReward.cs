using ALPackage;
using Common.InnObj;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndInnGetCashRegisterReward : _ATALBasicUIWnd<GGUIMonoInnGetCashRegisterReward>
    {
        [NotNull] public static GGUIWndInnGetCashRegisterReward instance { get { return _g_instance ??= new GGUIWndInnGetCashRegisterReward(); } }
        private static GGUIWndInnGetCashRegisterReward _g_instance;


        private Inn_SettleInfo _m_serverSettleInfo;

        private GGUISubWndInnGetCashRegisterRewardDetailGrid _m_detailGrid;
        
        
        public GGUIWndInnGetCashRegisterReward() 
            : base(EALUIWndLayer.ADDITION)
        {
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoInnGetCashRegisterReward.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoInnGetCashRegisterReward.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_detailGrid?.showWnd();
            
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_detailGrid?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_detailGrid?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_detailGrid?.discard();
            _m_detailGrid = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnConfirm, _onBtnConfirmClicked);
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClicked);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoDetailGrid != null)
                _m_detailGrid = new GGUISubWndInnGetCashRegisterRewardDetailGrid(wnd.monoDetailGrid);
            
            ALUGUICommon.combineBtnClick(wnd.btnConfirm, _onBtnConfirmClicked);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClicked);
        }
        

        public void refreshWnd(Inn_SettleInfo _settleInfo)
        {
            _m_serverSettleInfo = _settleInfo;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_serverSettleInfo == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtGuestNum, _m_serverSettleInfo.getReceiveNum());
            ALUGUICommon.setLabelTxt(wnd.txtPopularity, _m_serverSettleInfo.getAddPopularity());
            ALUGUICommon.setLabelTxt(wnd.txtStationBlueprint, _m_serverSettleInfo.getAddStationBlueprintNum());
            ALUGUICommon.setLabelTxt(wnd.txtAffection, _m_serverSettleInfo.getAddAffection());
            _m_detailGrid?.refreshWnd(_m_serverSettleInfo.getDishList());
        }


        private void _onBtnConfirmClicked(GameObject _)
        {
            _onBtnCloseClicked(null);
        }
        private void _onBtnCloseClicked(GameObject _)
        {
            dealCloseNode();
        }

        public void dealCloseNode()
        {
            if (wnd != null && _m_serverSettleInfo != null)
            {
                // 显示客人数量粒子特效
                if (wnd.tranGuestNumParticleStart != null && wnd.guestNumParticleNumConfigList != null)
                {
                    int showGuestNumParticleNum = ParticleNumRangeInfo.getTargetParticleNum(wnd.guestNumParticleNumConfigList, _m_serverSettleInfo.getReceiveNum());
                    if (showGuestNumParticleNum > 0)
                    {
                        GGUIHarvestCore.instance.startHarvestCollection(EHarvestType.INN_GUEST, GCommon.getUIRootPos(wnd.tranGuestNumParticleStart), showGuestNumParticleNum, wnd.guestNumParticleId, 1);
                    }
                }

                // 显示熟练度粒子特效
                if (wnd.tranFinesseParticleStart != null && wnd.finesseParticleNumConfigList != null)
                {
                    long totalFinesseValue = 0;
                    if (_m_serverSettleInfo.getDishList() != null)
                    {
                        foreach (var item in _m_serverSettleInfo.getDishList())
                        {
                            totalFinesseValue += (item?.getAddFinesse() ?? 0);
                        }
                    }
                    int showFinesseParticleNum = ParticleNumRangeInfo.getTargetParticleNum(wnd.finesseParticleNumConfigList, totalFinesseValue);
                    if (showFinesseParticleNum > 0)
                    {
                        GGUIHarvestCore.instance.startHarvestCollection(EHarvestType.INN_FINESSE,
                            GCommon.getUIRootPos(wnd.tranFinesseParticleStart), showFinesseParticleNum,
                            wnd.finesseParticleId, 1);
                    }
                }
            }
            
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_INN_GET_CASH_REGISTER_REWARD);   
        }
    }
}
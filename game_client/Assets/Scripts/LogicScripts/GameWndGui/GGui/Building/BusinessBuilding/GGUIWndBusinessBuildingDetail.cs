using ALPackage;
using Common.PrivilegeCardEnum;
using CommonEnum;
using GOE.BonusSpace;
using JetBrains.Annotations;
using System;
using UnityEngine;

namespace GOE
{
    public class GGUIWndBusinessBuildingDetail : _ANPGGUIBasicWnd<GGUIMonoBusinessBuildingDetail>
    {
        [NotNull] public static GGUIWndBusinessBuildingDetail instance { get { return _g_instance ??= new GGUIWndBusinessBuildingDetail(); } }
        private static GGUIWndBusinessBuildingDetail _g_instance;
        
        
        private NPGGuiWndTexture _m_attrIcon;
        private NPGGuiWndTexture _m_buildingIcon;
        private GGUISubWndCommonPropertyDetail _m_propertyDetail;
        
        private BusinessBuildingInfo _m_buildingInfo;


        public GGUIWndBusinessBuildingDetail() 
            : base(EALUIWndLayer.ADDITION)
        {
        }
        
        
        
        protected override string _monoAssetPath { get { return GGUIMonoBusinessBuildingDetail.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoBusinessBuildingDetail.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_attrIcon?.showWnd();
            _m_buildingIcon?.showWnd();
            _m_propertyDetail?.showWnd();

            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_attrIcon?.hideWnd();
            _m_buildingIcon?.hideWnd();
            _m_propertyDetail?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_attrIcon?.discardTexture();
            _m_buildingIcon?.discardTexture();
            _m_propertyDetail?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_attrIcon?.discard();
            _m_attrIcon = null;
            
            _m_buildingIcon?.discard();
            _m_buildingIcon = null;
            
            _m_propertyDetail?.discard();
            _m_propertyDetail = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickCloseBtn);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.imgBuildingAttrIcon != null)
                _m_attrIcon = new NPGGuiWndTexture(wnd.imgBuildingAttrIcon);
            if (wnd.imgBuildingIcon != null)
                _m_buildingIcon = new NPGGuiWndTexture(wnd.imgBuildingIcon);
            if (wnd.monoPropertyDetail != null)
                _m_propertyDetail = new GGUISubWndCommonPropertyDetail(wnd.monoPropertyDetail);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickCloseBtn);
        }


        public void refreshWnd(BusinessBuildingInfo _buildingInfo)
        {
            _m_buildingInfo = _buildingInfo;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_buildingInfo == null)
                return;
            
            BasicAttrRefObj attrRefObj = GRefdataCoreMgr.instance.basicAttrRefCore.getRef((long)_m_buildingInfo.baseRef.attr_type);
            if (attrRefObj != null && _m_attrIcon != null)
                _m_attrIcon.setTexture(attrRefObj.icon);

            BuildingRefObj buildingRef = GRefdataCoreMgr.instance.buildingRefCore.getRef(_m_buildingInfo.id);
            _m_buildingIcon?.setTexture(buildingRef?.preview_tex_index);
            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(_m_buildingInfo.baseRef.name));
            ALUGUICommon.setLabelTxt(wnd.txtTotalEarningsPerS, TextTranslate.instance.getLanguage(TransKeyConst.building_infoPowerNum_num1, _m_buildingInfo.earningsPerS.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD)));
            ALUGUICommon.setLabelTxt(wnd.txtTotalEarningsCoinPerS, TextTranslate.instance.getLanguage(TransKeyConst.building_businessBuildingDetailTotalEarningsGoldPerS_num, _m_buildingInfo.earningsPerS.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD)));
            ALUGUICommon.setLabelTxt(wnd.txtBaseEarningsPerS, TextTranslate.instance.getLanguage(TransKeyConst.building_businessBuildingDetailBaseEarningsPerS_num, _m_buildingInfo.getTotalBaseEarningsPerS().ToLargeString(PrimitiveExtension.ELargeStringType.GOLD)));
            ALUGUICommon.setLabelTxt(wnd.txtHeroEarningsPerS, TextTranslate.instance.getLanguage(TransKeyConst.building_businessBuildingDetailHeroEarningsPerS_num, ((long)Math.Ceiling((double)NPPlayer.instance.heroComponent.totalPower / 1000)).ToLargeString(PrimitiveExtension.ELargeStringType.GOLD)));
            ALUGUICommon.setLabelTxt(wnd.txtEmployeeEarningsPerS, TextTranslate.instance.getLanguage(TransKeyConst.building_businessBuildingDetailEmployeeEarningsPerS_num, (_m_buildingInfo.baseRef.employee_earnings * _m_buildingInfo.employeeNum).ToLargeString(PrimitiveExtension.ELargeStringType.GOLD)));

            //太空运输员工收益加成
            _AUnionBonusMgr innBonusMgr = NPPlayer.instance.playerBonusMgr.findMgrByTag(EUnionBonusMgrTag.INN);
            long innAddValue = innBonusMgr?.getTotalPropertyBonus(EBonusPropertyType.BUILDING_EMPLOYEE_PROFIT_ADD, _m_buildingInfo.bonusJudgeParts) ?? 0;
            ALUGUICommon.setLabelTxt(wnd.txtEmployeeInnEarningsPerS, TextTranslate.instance.getLanguage(TransKeyConst.building_addInnEarningsPerS_num, (innAddValue * _m_buildingInfo.employeeNum).ToLargeString(PrimitiveExtension.ELargeStringType.GOLD)));

            //太空打捞员工收益加成
            _AUnionBonusMgr treasureHuntBonusMgr = NPPlayer.instance.playerBonusMgr.findMgrByTag(EUnionBonusMgrTag.TREASURE_HUNT);
            long treasureHuntAddValue = treasureHuntBonusMgr?.getTotalPropertyBonus(EBonusPropertyType.BUILDING_EMPLOYEE_PROFIT_ADD, _m_buildingInfo.bonusJudgeParts) ?? 0;
            ALUGUICommon.setLabelTxt(wnd.txtEmployeeTreasureHuntEarningsPerS, TextTranslate.instance.getLanguage(TransKeyConst.building_addTreasureHuntPerS_num, (treasureHuntAddValue * _m_buildingInfo.employeeNum).ToLargeString(PrimitiveExtension.ELargeStringType.GOLD)));
           
            ALUGUICommon.setLabelTxt(wnd.txtEarningsBonus, TextTranslate.instance.getLanguage(TransKeyConst.building_businessBuildingDetailEarningsBonus_num, _m_buildingInfo.getTotalEarningBonus() / 100f));
            ALUGUICommon.setLabelTxt(wnd.txtPlacedHeroBonus, TextTranslate.instance.getLanguage(TransKeyConst.building_businessBuildingDetailPlacedHeroBonus_num, _m_buildingInfo.getAllPlacedHeroEarningBonus() / 100f));
            ALUGUICommon.setLabelTxt(wnd.txtUpgradeBonus, TextTranslate.instance.getLanguage(TransKeyConst.building_businessBuildingDetailUpgradeBonus_num, (_m_buildingInfo.levelRef?.earning_rate ?? 0) / 100f));
            ALUGUICommon.setLabelTxt(wnd.txtTowerEarningsPerS, TextTranslate.instance.getLanguage(TransKeyConst.building_businessBuildingDetailTowerEarningsPers_num, NPPlayer.instance.towerComp.towerEarningAddPer/100f));
            ALUGUICommon.setLabelTxt(wnd.txtMonthCardPerS, TextTranslate.instance.getLanguage(TransKeyConst.building_businessBuildingDetailMonthCardEarningsPers_num, NPPlayer.instance.privilegeCardComp.getAddPropValue(EPrivilegeCardType.MONTH, EBonusPropertyType.BUILDING_PROFIT_ADD_PER) /100f));
            ALUGUICommon.setLabelTxt(wnd.txtYearCardPerS, TextTranslate.instance.getLanguage(TransKeyConst.building_businessBuildingDetailYearCardEarningsPers_num, NPPlayer.instance.privilegeCardComp.getAddPropValue(EPrivilegeCardType.YEAR, EBonusPropertyType.BUILDING_PROFIT_ADD_PER) / 100f));
            _m_propertyDetail?.refreshWnd(_m_buildingInfo.bonusJudgeParts);
            ALUGUICommon.setLabelTxt(wnd.txtBuildingDesc, TextTranslate.instance.getLanguage(_m_buildingInfo.baseRef.desc));
        }
        
        
        private void _onClickCloseBtn(GameObject _)
        {
            QueueMgr.instance.DoUIRollBackByEsc();
        }
    }
}
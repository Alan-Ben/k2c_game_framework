
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndBusinessBuilding : _ANPGGUIBasicWnd<GGUIMonoBusinessBuilding>
    {
        [NotNull] public static GGUIWndBusinessBuilding instance { get { return _g_instance ??= new GGUIWndBusinessBuilding(); } }
        private static GGUIWndBusinessBuilding _g_instance;
        
        
        private NPGGuiWndTexture _m_attrIcon;
        private NPGGUIWndCommonToggleEx _m_tenTimesHire;
        private NPGGUIWndCommonItem _m_hireCost;
        private NPGGUIWndCommonItem _m_upgradeCost;
        private GGUISubWndBusinessBuildingOperatingHeroContainer _m_operatingHeroContainer;
        private GGUISubWndBusinessBuildingProductGrid _m_productGrid;
        private GGUIWndBusinessBuildingVideoShow _m_videoShow;
        private GGUISubWndSmoothDampLongText _m_hireNumText;
        private NPGGUIWndProgress _m_wRnDProgress;//研发进度
        private GGUIWndHeroVoiceBubble _m_wHeroBubble;//顾问气泡

        private BusinessBuildingInfo _m_buildingInfo;
        private List<CommonUISfxObj> _m_uiSfxObjList;
        private const int _k_maxSfxCount = 5;
        private int _m_refreshSerialize;
        private long _m_lShowBubbleSerialize;


        public GGUIWndBusinessBuilding() 
            : base(EALUIWndLayer.NORMAL)
        {
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoBusinessBuilding.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoBusinessBuilding.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_attrIcon?.showWnd();
            _m_tenTimesHire?.showWnd();
            _m_hireCost?.showWnd();
            _m_upgradeCost?.showWnd();
            _m_operatingHeroContainer?.showWnd();
            _m_productGrid?.showWnd();
            _m_videoShow?.showWnd();
            _m_hireNumText?.showWnd();
            _m_wRnDProgress?.showWnd();
            _m_wHeroBubble?.hideWnd();//默认隐藏气泡

            refreshWnd();
            
            NPPlayer.instance.buildingComp.onBusinessBuildingChg += _onBusinessBuildingChg;
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_BUSINESS_BUILDING_TEN_TIMES_HIRE_TOGGLE, _onToggleTenTimesClicked);
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_BUSINESS_BUILDING_HIRE_BTN, _onBtnHire);
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_BUSINESS_BUILDING_HERO_SLOT, _onOperatingHeroSlotClick);
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_BUSINESS_BUILDING_UPGRADE_BTN, _onBtnUpgrade);
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_BUSINESS_BUILDING_SET_HERO_BTN, _onBtnSetHeroClicked);
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_BUSINESS_BUILDING_RND_BTN, _onBtnRnDClicked);
            WinMsg.RegisterMsg(WinMsgType.ON_PLAYER_RES_CHANGE, _onCostItemChg);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _onCostItemChg);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _onCostItemChg);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _onCostItemChg);
        }
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_BUSINESS_BUILDING_UPGRADE_BTN, _onBtnUpgrade);
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_BUSINESS_BUILDING_HERO_SLOT, _onOperatingHeroSlotClick);
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_BUSINESS_BUILDING_TEN_TIMES_HIRE_TOGGLE, _onToggleTenTimesClicked);
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_BUSINESS_BUILDING_HIRE_BTN, _onBtnHire);
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_BUSINESS_BUILDING_SET_HERO_BTN, _onBtnSetHeroClicked);
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_BUSINESS_BUILDING_RND_BTN, _onBtnRnDClicked);
            WinMsg.UnregisterMsg(WinMsgType.ON_PLAYER_RES_CHANGE, _onCostItemChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _onCostItemChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _onCostItemChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _onCostItemChg);
            NPPlayer.instance.buildingComp.onBusinessBuildingChg -= _onBusinessBuildingChg;
            
            _m_attrIcon?.hideWnd();
            _m_tenTimesHire?.hideWnd();
            _m_hireCost?.hideWnd();
            _m_upgradeCost?.hideWnd();
            _m_operatingHeroContainer?.hideWnd();
            _m_productGrid?.hideWnd();
            _m_videoShow?.hideWnd();
            _m_hireNumText?.hideWnd();
            _m_wRnDProgress?.hideWnd();
            _m_wHeroBubble?.hideWnd();
            
            if (_m_uiSfxObjList != null)
            {
                foreach (CommonUISfxObj sfx in _m_uiSfxObjList)
                    sfx?.forceDiscard();
                _m_uiSfxObjList.Clear();
            }

            HeroVoiceMgr.instance.stopAllVoice();
            
            _m_refreshSerialize = ALSerializeOpMgr.next();
            _m_lShowBubbleSerialize = ALSerializeOpMgr.next();
        }
        protected override void _onReset()
        {
            _m_attrIcon?.discardTexture();
            _m_tenTimesHire?.resetWnd();
            _m_hireCost?.resetWnd();
            _m_upgradeCost?.resetWnd();
            _m_operatingHeroContainer?.resetWnd();
            _m_productGrid?.resetWnd();
            _m_videoShow?.resetWnd();
            _m_hireNumText?.resetWnd();
            _m_wRnDProgress?.resetWnd();
            _m_wHeroBubble?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_attrIcon?.discard();
            _m_tenTimesHire?.discard();
            _m_hireCost?.discard();
            _m_upgradeCost?.discard();
            _m_operatingHeroContainer?.discard();
            _m_productGrid?.discard();
            _m_videoShow?.discard();
            _m_hireNumText?.discard();
            _m_wRnDProgress?.discard();
            _m_wHeroBubble?.discard();
            
            _m_attrIcon = null;
            _m_tenTimesHire = null;
            _m_hireCost = null;
            _m_upgradeCost = null;
            _m_operatingHeroContainer = null;
            _m_productGrid = null;
            _m_videoShow = null;
            _m_hireNumText = null;
            _m_wRnDProgress = null;
            _m_wHeroBubble = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnBack, _onBtnBack);
            ALUGUICommon.uncombineBtnClick(wnd.btnHire, _onBtnHire);
            ALUGUICommon.uncombineBtnClick(wnd.btnUpgrade, _onBtnUpgrade);
            ALUGUICommon.uncombineBtnClick(wnd.btnEarningDetail, _onBtnEarningDetail);
            ALUGUICommon.uncombineBtnClick(wnd.btnGotoNextBuilding, _onBtnGoToNextBuilding);
            ALUGUICommon.uncombineBtnClick(wnd.btnGotoPrevBuilding, _onBtnGoToPrevBuilding);
            ALUGUICommon.uncombineBtnClick(wnd.btnSetHero, _onBtnSetHeroClicked);
            ALUGUICommon.uncombineBtnClick(wnd.btnRnD, _onBtnRnDClicked);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.imgAttrIcon != null)
                _m_attrIcon = new NPGGuiWndTexture(wnd.imgAttrIcon);
            if (wnd.monoHireCost != null)
                _m_hireCost = new NPGGUIWndCommonItem(wnd.monoHireCost);
            if (wnd.monoUpgradeCost != null)
                _m_upgradeCost = new NPGGUIWndCommonItem(wnd.monoUpgradeCost);
            if (wnd.monoTenTimesHire != null)
            {
                _m_tenTimesHire = new NPGGUIWndCommonToggleEx(wnd.monoTenTimesHire);
                _m_tenTimesHire.clickDelegate += _onToggleTenTimesClicked;
            }
            if (wnd.monoOperatingHeroContainer != null)
                _m_operatingHeroContainer = new GGUISubWndBusinessBuildingOperatingHeroContainer(wnd.monoOperatingHeroContainer, _onOperatingHeroSlotClick);
            if (wnd.monoProductGrid != null)
                _m_productGrid = new GGUISubWndBusinessBuildingProductGrid(wnd.monoProductGrid);
            if (wnd.videoShow != null)
                _m_videoShow = new GGUIWndBusinessBuildingVideoShow(wnd.videoShow);
            if (wnd.txtEmployeeCount != null)
                _m_hireNumText = new GGUISubWndSmoothDampLongText(wnd.txtEmployeeCount);
            if (wnd.monoRnDProgress != null)
                _m_wRnDProgress = new NPGGUIWndProgress(wnd.monoRnDProgress);
            if (wnd.monoHeroBubble != null)
                _m_wHeroBubble = new GGUIWndHeroVoiceBubble(wnd.monoHeroBubble);


            ALUGUICommon.combineBtnClick(wnd.btnBack, _onBtnBack);
            ALUGUICommon.combineBtnClick(wnd.btnHire, _onBtnHire);
            ALUGUICommon.combineBtnClick(wnd.btnUpgrade, _onBtnUpgrade);
            ALUGUICommon.combineBtnClick(wnd.btnEarningDetail, _onBtnEarningDetail);
            ALUGUICommon.combineBtnClick(wnd.btnGotoNextBuilding, _onBtnGoToNextBuilding);
            ALUGUICommon.combineBtnClick(wnd.btnGotoPrevBuilding, _onBtnGoToPrevBuilding);
            ALUGUICommon.combineBtnClick(wnd.btnSetHero, _onBtnSetHeroClicked);
            ALUGUICommon.combineBtnClick(wnd.btnRnD, _onBtnRnDClicked);
        }


        public void refreshWnd(BusinessBuildingInfo _buildingInfo)
        {
            _m_buildingInfo = _buildingInfo;
            _m_refreshSerialize = ALSerializeOpMgr.next();
            _m_lShowBubbleSerialize = ALSerializeOpMgr.next();
            _m_wHeroBubble?.hideWnd();
            if (wnd != null && wnd.animHire != null)
                wnd.animHire.Sample(wnd.animNameHire, 0);
            refreshWnd();
        }
        public void refreshWnd(bool _needSmooth = false)
        {
            if (!_m_bIsShow || wnd == null || _m_buildingInfo == null)
                return;
            
            if (_m_attrIcon != null)
            {
                BasicAttrRefObj attrRef = GRefdataCoreMgr.instance.basicAttrRefCore.getRef((long)_m_buildingInfo.baseRef.attr_type);
                _m_attrIcon.setTexture(attrRef?.icon);
            }
            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(_m_buildingInfo.baseRef.name));
            ALUGUICommon.setLabelTxt(wnd.txtLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, _m_buildingInfo.level));
            ALUGUICommon.setLabelTxt(wnd.txtTotalEarningsPerS, TextTranslate.instance.getLanguage(TransKeyConst.building_businessBuildingTotalEarningsPerS_num, _m_buildingInfo.earningsPerS.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD)));
            ALUGUICommon.setLabelTxt(wnd.txtEmployeeEarningsPerPerson, TextTranslate.instance.getLanguage(TransKeyConst.building_businessBuildingEmployeeEarningsPerPerson_num, _m_buildingInfo.getEmployeeEarningsPerPerson().ToLargeString(PrimitiveExtension.ELargeStringType.GOLD)));
            _m_hireNumText?.refreshWnd(_m_buildingInfo.employeeNum, EValueFormatType.NORMAL, _needSmooth, _m_buildingInfo.maxEmployeeNum.ToString());
            // ALUGUICommon.setLabelTxt(wnd.txtEmployeeCount, TextTranslate.instance.getLanguage(TransKeyConst.building_businessBuildingEmployeeNum_num_num, _m_buildingInfo.employeeNum, _m_buildingInfo.maxEmployeeNum));
            ALUGUICommon.setLabelTxt(wnd.txtEarningsBonus, TextTranslate.instance.getLanguage(TransKeyConst.building_businessBuildingEarningsBonus_num, 100 + (_m_buildingInfo.levelRef?.earning_rate ?? 0) / 100f));
            ALUGUICommon.setLabelTxt(wnd.txtNextEarningsBonus, TextTranslate.instance.getLanguage(TransKeyConst.building_businessBuildingNextEarningsBonus_num, 100 + (_m_buildingInfo.nextLevelRef?.earning_rate ?? 0) / 100f));
            ALUGUICommon.setGameObjEnable(wnd.listLevelMaxShow, _m_buildingInfo.levelMax);
            ALUGUICommon.setGameObjEnable(wnd.listLevelMaxHide, !_m_buildingInfo.levelMax);
            ALUGUICommon.setGameObjEnable(wnd.listLevelEmployeeMaxShow, _m_buildingInfo.levelMax && _m_buildingInfo.employeeNum >= _m_buildingInfo.maxEmployeeNum);
            ALUGUICommon.setGameObjEnable(wnd.listLevelEmployeeMaxHide, !_m_buildingInfo.levelMax || _m_buildingInfo.employeeNum < _m_buildingInfo.maxEmployeeNum);
            _m_operatingHeroContainer?.refreshWnd(_m_buildingInfo.employeeNum, _m_buildingInfo.baseRef, _m_buildingInfo.heroSlotList, _m_buildingInfo.needShowSettleHeroTip());
            ALUGUICommon.setLabelTxt(wnd.txtNextHeroSlotUnlockRequire, _m_buildingInfo.getNextSlotUnlockRequireNum());
            ALUGUICommon.setLabelTxt(wnd.txtHeroSlotState, TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, _m_buildingInfo.heroSlotList.Count, _m_buildingInfo.baseRef.hero_slot_employee_num_list.Count));
            ALUGUICommon.setGameObjEnable(wnd.listHeroSlotAllUnlockHide, !_m_buildingInfo.isAllSlotUnlock());
            _m_tenTimesHire?.setSelected(AccountSettingMgr.instance.accountSetting.isBuildingHiringTenTimes);
            _m_videoShow?.refreshWnd(_m_buildingInfo.getCurrentVideoGroupRef(), _m_buildingInfo.id, _m_buildingInfo.level);
            BusinessBuildingInfo buildingInfo = NPPlayer.instance.buildingComp.getNextBusinessBuilding(_m_buildingInfo);
            ALUGUICommon.setGameObjEnable(wnd.listCanGotoShow, buildingInfo != null);
            _m_productGrid?.refreshWnd(_m_buildingInfo);
            ALUGUICommon.setGameObjEnable(wnd.listCanHeroSettleShow, _m_buildingInfo.needShowSettleHeroTip());
            ALUGUICommon.setGameObjEnable(wnd.listCanUnlockProductShow, _m_buildingInfo.needShowUnlockProductTip());
            if (wnd.animNextHeroSlotUnlockRequire != null)
                wnd.animNextHeroSlotUnlockRequire.ForcePlay(wnd.animNameNextHeroSlotUnlockRequire);
            _refreshCost();
            _refreshRnDProgress();
        }
        public void setVideoIndex(int _index, int _customLevel = -1)
        {
            _m_videoShow?.setVideoIndex(_index, _customLevel);
        }

        //刷新消耗
        private void _refreshCost()
        {
            if (_m_buildingInfo == null)
                return;

            _m_upgradeCost?.setItem(_m_buildingInfo.levelRef?.upgrade_cost_item);
            _m_hireCost?.setItem(_m_buildingInfo?.getHirCost(_m_tenTimesHire?.isOn ?? false ? 10 : 1));
        }

        //刷新研发进度
        private void _refreshRnDProgress()
        {
            if (_m_buildingInfo == null)
                return;

            List<BusinessBuildingProductRefObj> productList = GRefdataCoreMgr.instance.getBusinessBuildingProductRefList(_m_buildingInfo.id);
            if (productList == null)
                return;

            BusinessBuildingProductRefObj targetRef = null;
            for (int i = 0; i < productList.Count; i++)
            {
                // 找到第一个未解锁的产品
                if (productList[i] != null &&
                    productList[i].employee_required > _m_buildingInfo.employeeNum &&
                    !_m_buildingInfo.isProductUnlocked(productList[i].id))
                {
                    targetRef = productList[i];
                    break;
                }
            }

            //设置进度
            if (targetRef == null)
                _m_wRnDProgress?.hideWnd();
            else
            {
                _m_wRnDProgress?.showWnd();
                _m_wRnDProgress?.setProgress(_m_buildingInfo.employeeNum, targetRef.employee_required, EValueFormatType.NORMAL_NOT_LARGE_STR);
            }
        }

        // 处理展示新派遣的顾问气泡
        private void _dealShowHeroBubble(List<HeroInfo> _heroInfoList)
        {
            if (_heroInfoList == null || _m_wHeroBubble == null || _heroInfoList.Count <= 0)
            {
                _m_wHeroBubble?.hideWnd();
                return;
            }

            //开始逐个展示
            _m_lShowBubbleSerialize = ALSerializeOpMgr.next();
            long curSerialize = _m_lShowBubbleSerialize;
            _dealShowHeroBubble(curSerialize, _heroInfoList, 0);
        }
        private void _dealShowHeroBubble(long _serialize, List<HeroInfo> _heroInfoList, int _index)
        {
            if (wnd == null || !isShow || _m_lShowBubbleSerialize != _serialize || _m_wHeroBubble == null || _heroInfoList == null)
                return;

            if (_heroInfoList.Count <= _index)
            {
                _m_wHeroBubble?.hideWnd();
                return;
            }

            HeroInfo heroInfo = _heroInfoList[_index];
            if (heroInfo == null)
            {
                _dealShowHeroBubble(_serialize, _heroInfoList, _index + 1);
                return;
            }

            //展示气泡
            _m_wHeroBubble.hideWnd();
            _m_wHeroBubble.showWnd();
            _m_wHeroBubble.setInfo(heroInfo.id, EHeroVoiceType.APPOINT);

            //延时展示下一个
            ALCommonActionMonoTask.addMonoTask(() =>
            {
                _dealShowHeroBubble(_serialize, _heroInfoList, _index + 1);
            }, wnd.delayShowNextHeroBubbleSecond);
        }


        private void _onBtnBack(GameObject _)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_BUILDING_MAIN);
        }
        private void _onBtnHire()
        {
            if (wnd == null)
                return;
            
            _onBtnHire(wnd.btnHire);   
        }
        private void _onBtnHire(GameObject _)
        {
            if (_m_buildingInfo == null || wnd == null)
                return;

            // 人数是否达到上限
            if (_m_buildingInfo.employeeNum >= _m_buildingInfo.maxEmployeeNum)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.building_employeeNumReachLimitTip_none);
                return;
            }

            bool isTenTimesHire = false;
            void _hireComplete(bool _isSuc)
            {
                if (wnd == null || !_isSuc) return;

                // Limit the number of UI SFX to prevent audio spam
                if (_m_uiSfxObjList is { Count: >= _k_maxSfxCount })
                {
                    // Remove oldest SFX if we've reached the limit
                    for (int i = _m_uiSfxObjList.Count - 1; i >= _k_maxSfxCount - 1; i--)
                    {
                        _m_uiSfxObjList[i]?.forceDiscard();
                        _m_uiSfxObjList.RemoveAt(i);
                    }
                }

                CommonUISfxObj sfxObj = PlaySfxMgr.instance.playUISfx(isTenTimesHire ? wnd.tenTimesHireEffectSfxId : wnd.hireEffectSfxId, wnd.hireEffectParent);
                
                // Add the new SFX to the list for tracking
                if (sfxObj != null)
                {
                    _m_uiSfxObjList ??= new List<CommonUISfxObj>();
                    _m_uiSfxObjList.Insert(0, sfxObj);
                }

                if (wnd.animHire != null)
                {
                    int refreshSerialize = _m_refreshSerialize;
                    ALCommonActionMonoTask.addMonoTask(() =>
                    {
                        if (refreshSerialize != _m_refreshSerialize)
                            return;
                        
                        wnd.animHire.ForcePlay(isTenTimesHire ? wnd.animNameTenTimesHire : wnd.animNameHire);
                    }, wnd.animHireDelay);
                }
                
                WinMsg.SendMsg(WinMsgType.CUSTOM_RELOAD);
            }

            if (_m_tenTimesHire is null or { isOn: false })
                NPPlayer.instance.buildingComp.reqBusinessHireEmployee(_m_buildingInfo.id, _hireComplete);
            else
            {
                isTenTimesHire = true;
                NPPlayer.instance.buildingComp.reqBusinessHireTenEmployees(_m_buildingInfo.id, _hireComplete);
            }
        }
        private void _onBtnUpgrade()
        {
            if (wnd == null)
                return;
            
            _onBtnUpgrade(wnd.btnUpgrade);
        }
        private void _onBtnUpgrade(GameObject _)
        {
            GGUIWndBusinessBuildingUpgrade.instance.refreshWnd(_m_buildingInfo);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndBusinessBuildingUpgrade.instance, GGUIWndBusinessBuildingUpgrade.instance.showWnd, UINodeTagConst.C_BUSINESS_BUILDING_UPGRADE);
        }
        private void _onBtnEarningDetail(GameObject _)
        {
            GGUIWndBusinessBuildingDetail.instance.refreshWnd(_m_buildingInfo);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndBusinessBuildingDetail.instance, GGUIWndBusinessBuildingDetail.instance.showWnd);
        }
        private void _onBtnGoToNextBuilding(GameObject _)
        {
            if (_m_buildingInfo == null)
                return;

            HeroVoiceMgr.instance.stopAllVoice();
            BusinessBuildingInfo buildingInfo = NPPlayer.instance.buildingComp.getNextBusinessBuilding(_m_buildingInfo);
            if (buildingInfo != null)
                refreshWnd(buildingInfo);
        }
        private void _onBtnGoToPrevBuilding(GameObject _)
        {
            if (_m_buildingInfo == null)
                return;

            HeroVoiceMgr.instance.stopAllVoice();
            BusinessBuildingInfo buildingInfo = NPPlayer.instance.buildingComp.getNextBusinessBuilding(_m_buildingInfo, -1);
            if (buildingInfo != null)
                refreshWnd(buildingInfo);
        }

        private void _onBtnSetHeroClicked()
        {
            if (wnd == null)
                return;
            
            _onBtnSetHeroClicked(wnd.btnSetHero);
        }
        private void _onBtnSetHeroClicked(GameObject _)
        {
            if (_m_buildingInfo == null)
                return;
            
            _m_buildingInfo.setShowedSettleHeroTipIndex();
            refreshWnd();
            GGUIWndBusinessBuildingSelectOperatingHero.instance.refreshWnd(_m_buildingInfo, _dealShowHeroBubble);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndBusinessBuildingSelectOperatingHero.instance, GGUIWndBusinessBuildingSelectOperatingHero.instance.showWnd);   
        }
        private void _onBtnRnDClicked()
        {
            if (wnd == null)
                return;
            
            _onBtnRnDClicked(wnd.btnRnD);
        }
        private void _onBtnRnDClicked(GameObject _)
        {
            if (_m_buildingInfo == null)
                return;

            NPPlayer.instance.buildingComp.setCheckedCanUnlockProductEmployeeNum(_m_buildingInfo);
            GGUIWndBusinessBuildingRnD.instance.refreshWnd(_m_buildingInfo);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndBusinessBuildingRnD.instance, GGUIWndBusinessBuildingRnD.instance.showWnd, UINodeTagConst.C_BUSINESS_BUILDING_RND);
        }
        private void _onToggleTenTimesClicked()
        {
            _onToggleTenTimesClicked(_m_tenTimesHire);   
        }
        private void _onToggleTenTimesClicked(NPGGUIWndCommonToggleEx _toggle)
        {
            if (_toggle == null)
                return;
            
            _toggle.setSelected(!_toggle.isOn);            
            _m_hireCost?.setItem(_m_buildingInfo?.getHirCost(_toggle.isOn ? 10 : 1));
            AccountSettingMgr.instance.accountSetting.setBuildingHiringTenTimes(_toggle.isOn);
        }
        private void _onOperatingHeroSlotClick()
        {
            _onOperatingHeroSlotClick(_m_operatingHeroContainer?.getItem(0));
        }
        private void _onOperatingHeroSlotClick(GGUISubWndBusinessBuildingOperatingHeroContainerItem _item)
        {
            if (_item == null || _m_buildingInfo == null)
                return;

            if (!_item.isUnlock)
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.building_businessBuildingHeroSlotUnlockTip_num, _item.slotEmployeeNum));
                return;
            }
            
            _m_buildingInfo.setShowedSettleHeroTipIndex();
            refreshWnd();
            GGUIWndBusinessBuildingSelectOperatingHero.instance.refreshWnd(_m_buildingInfo, _dealShowHeroBubble);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndBusinessBuildingSelectOperatingHero.instance, GGUIWndBusinessBuildingSelectOperatingHero.instance.showWnd);   
        }
        private void _onBusinessBuildingChg(BusinessBuildingInfo _buildingInfo)
        {
            if (_buildingInfo == _m_buildingInfo)
                refreshWnd(true);
        }
        private void _onCostItemChg(params object[] _objects)
        {
            _refreshCost();
        }
    }
}
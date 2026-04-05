using System;
using System.Collections.Generic;
using ALPackage;
using Common.HeroObj;
using CommonEnum;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndBusinessBuildingSelectOperatingHero : _ANPGGUIBasicWnd<GGUIMonoBusinessBuildingSelectOperatingHero>
    {
        [NotNull] public static GGUIWndBusinessBuildingSelectOperatingHero instance { get { return _g_instance ??= new GGUIWndBusinessBuildingSelectOperatingHero(); } }
        private static GGUIWndBusinessBuildingSelectOperatingHero _g_instance;
        
        
        private GGUISubWndBusinessBuildingOperatingHeroContainer _m_selectedHeroContainer;
        private GGUISubWndBusinessBuildingOperatingHeroSelectGrid _m_selectHeroGrid;
        
        private BusinessBuildingInfo _m_buildingInfo;
        private List<HeroInfo> _m_curSelectHeroList;
        private Action<List<HeroInfo>> _m_onDispatchNewHero;


        public GGUIWndBusinessBuildingSelectOperatingHero() 
            : base(EALUIWndLayer.ADDITION)
        {
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoBusinessBuildingSelectOperatingHero.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoBusinessBuildingSelectOperatingHero.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_selectedHeroContainer?.showWnd();
            _m_selectHeroGrid?.showWnd();

            refreshWnd();
            
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_BUSINESS_BUILDING_HERO_SELECT_CONFIRM, _onClickConfirmBtn);
        }
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_BUSINESS_BUILDING_HERO_SELECT_CONFIRM, _onClickConfirmBtn);
            
            _m_selectedHeroContainer?.hideWnd();
            _m_selectHeroGrid?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_selectedHeroContainer?.resetWnd();
            _m_selectHeroGrid?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_selectedHeroContainer?.discard();
            _m_selectHeroGrid?.discard();
            
            _m_selectedHeroContainer = null;
            _m_selectHeroGrid = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickCloseBtn);
            ALUGUICommon.uncombineBtnClick(wnd.btnConfirm, _onClickConfirmBtn);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.monoHeroContainer != null)
                _m_selectedHeroContainer = new GGUISubWndBusinessBuildingOperatingHeroContainer(wnd.monoHeroContainer, _onHeroSlotClick);
            if (wnd.monoHeroSelectGrid != null)
                _m_selectHeroGrid = new GGUISubWndBusinessBuildingOperatingHeroSelectGrid(wnd.monoHeroSelectGrid, _onHeroSelect);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickCloseBtn);
            ALUGUICommon.combineBtnClick(wnd.btnConfirm, _onClickConfirmBtn);
        }


        public void refreshWnd(BusinessBuildingInfo _buildingInfo,Action<List<HeroInfo>> _onDispatchNewHero)
        {
            _m_buildingInfo = _buildingInfo;
            _m_onDispatchNewHero = _onDispatchNewHero;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_buildingInfo == null)
                return;

            _m_curSelectHeroList = new List<HeroInfo>(_m_buildingInfo.heroSlotList);
            _m_selectedHeroContainer?.refreshWnd(_m_buildingInfo.employeeNum, _m_buildingInfo.baseRef, _m_curSelectHeroList);
            _m_selectHeroGrid?.refreshWnd(_m_buildingInfo.baseRef, _m_curSelectHeroList);
            refreshTotalBonus();
        }
        public void refreshTotalBonus()
        {
            if (wnd == null)
                return;

            long totalBonus = 0;
            foreach (HeroInfo heroInfo in _m_curSelectHeroList)
                totalBonus += heroInfo.getBusinessSkillAddPropValue(_m_buildingInfo.baseRef, EBonusPropertyType.BUILDING_PROFIT_ADD_PER);
            
            ALUGUICommon.setLabelTxt(wnd.txtTotalBonus, TextTranslate.instance.getLanguage(TransKeyConst.building_businessBuildingHeroBonus_num, totalBonus / 100f));
        }
        
        
        private void _onClickCloseBtn(GameObject _)
        {
            QueueMgr.instance.DoUIRollBackByEsc();
        }
        private void _onClickConfirmBtn()
        {
            if (wnd == null)
                return;
            
            _onClickConfirmBtn(wnd.btnConfirm);
        }
        private void _onClickConfirmBtn(GameObject _)
        {
            if (_m_buildingInfo == null || _m_curSelectHeroList == null)
                return;
            
            List<HeroInfo> alreadyJoinsAnotherBuilding = new List<HeroInfo>();
            foreach (HeroInfo heroInfo in _m_curSelectHeroList)
            {
                if (heroInfo.placeData.isPlaced && heroInfo.placeData.buildingId != _m_buildingInfo.id)
                    alreadyJoinsAnotherBuilding.Add(heroInfo);
            }

            if (alreadyJoinsAnotherBuilding.Count > 0)
            {
                GGUIWndBusinessBuildingSelectOperatingHeroConfirm.instance.refreshWnd(alreadyJoinsAnotherBuilding, _m_buildingInfo.baseRef);
                GGUIWndBusinessBuildingSelectOperatingHeroConfirm.instance.setConfirmFunc(doConfirm);
                QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndBusinessBuildingSelectOperatingHeroConfirm.instance, GGUIWndBusinessBuildingSelectOperatingHeroConfirm.instance.showWnd);
            }
            else
                doConfirm();

            void doConfirm()
            {
                List<HeroInfo> newDispatchHeroList = new List<HeroInfo>();
                HeroInfo newDispatchHero = null;
                int serialize = MainCameraMono.selfInstance.openAllInputMask();
                ALStepCounter stepCounter = new ALStepCounter();
                stepCounter.chgTotalStepCount(1);
                stepCounter.regAllDoneDelegate(() =>
                {
                    MainCameraMono.selfInstance.closeAllInputMask(serialize);
                    _onClickCloseBtn(null);

                    if(newDispatchHero != null)
                        HeroVoiceMgr.instance.playVoice(newDispatchHero.id, EHeroVoiceType.APPOINT);

                    //确认派遣完成，执行回调
                    _m_onDispatchNewHero?.Invoke(newDispatchHeroList);
                });
                {
                    //记录原本派遣列表
                    List<HeroInfo> oriHeroInfoList = new List<HeroInfo>();
                    if(_m_buildingInfo.heroSlotList != null)
                        oriHeroInfoList.AddRange(_m_buildingInfo.heroSlotList);

                    List<HeroInfo> curSavedHeroList = new List<HeroInfo>(_m_buildingInfo.heroSlotList);
                    for (int i = 0; i < curSavedHeroList.Count; i++)
                    {
                        HeroInfo heroInfo = curSavedHeroList[i];
                        if (i >= _m_curSelectHeroList.Count ||
                            _m_curSelectHeroList[i] != heroInfo)
                        {
                            curSavedHeroList.RemoveAt(i);
                            i--;

                            stepCounter.chgTotalStepCount(1);
                            NPPlayer.instance.heroComponent.reqBuildingRemoveHero(heroInfo.id, stepCounter.addDoneStepCount);
                        }
                    }

                    for (int i = curSavedHeroList.Count; i < _m_curSelectHeroList.Count; i++)
                    {
                        HeroInfo heroInfo = _m_curSelectHeroList[i];
                        stepCounter.chgTotalStepCount(1);
                        NPPlayer.instance.heroComponent.reqBuildingPlaceHero(heroInfo.id, _m_buildingInfo.id, stepCounter.addDoneStepCount);

                        //记录加成最大的新派遣伙伴
                        if (newDispatchHero == null)
                            newDispatchHero = heroInfo;
                        else
                        {
                            long curValue = newDispatchHero.getBusinessSkillAddPropValue(_m_buildingInfo.baseRef, EBonusPropertyType.BUILDING_PROFIT_ADD_PER);
                            long newValue = heroInfo.getBusinessSkillAddPropValue(_m_buildingInfo.baseRef, EBonusPropertyType.BUILDING_PROFIT_ADD_PER);
                            if(curValue < newValue)
                                newDispatchHero = heroInfo;
                        }
                    }

                    //筛选新派遣的顾问列表
                    for (int i = 0; i < _m_curSelectHeroList.Count; i++)
                    {
                        bool isFind = false;
                        for (int j = 0; j < oriHeroInfoList.Count; j++)
                        {
                            if (_m_curSelectHeroList[i].id == oriHeroInfoList[j].id)
                            {
                                isFind = true;
                                break;
                            }
                        }

                        //记录新派遣的顾问
                        if(!isFind)
                            newDispatchHeroList.Add(_m_curSelectHeroList[i]);
                    }
                }
                stepCounter.addDoneStepCount();
            }
        }
        private void _onHeroSlotClick(GGUISubWndBusinessBuildingOperatingHeroContainerItem _item)
        {
            if (_item?.heroInfo == null || _m_curSelectHeroList == null)
                return;

            if (_m_curSelectHeroList.Remove(_item.heroInfo))
            {
                _m_selectHeroGrid?.refreshWnd();
                _m_selectedHeroContainer?.refreshWnd();
            }
        }
        private void _onHeroSelect(HeroInfo _heroInfo)
        {
            if (_m_curSelectHeroList == null || _m_selectHeroGrid == null || _m_buildingInfo == null)
                return;

            if (_m_curSelectHeroList.Remove(_heroInfo))
            {
                _m_selectHeroGrid.refreshWnd();
                _m_selectedHeroContainer?.refreshWnd();
                return;
            }

            if (_m_curSelectHeroList.Count >= _m_buildingInfo.baseRef.hero_slot_employee_num_list.Count ||
                _m_buildingInfo.baseRef.hero_slot_employee_num_list[_m_curSelectHeroList.Count] > _m_buildingInfo.employeeNum)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.building_businessBuildingHeroSlotFull_none);
                return;
            }
            
            _m_curSelectHeroList.Add(_heroInfo);
            _m_selectHeroGrid.refreshWnd();
            _m_selectedHeroContainer?.refreshWnd();
        }
    }
}
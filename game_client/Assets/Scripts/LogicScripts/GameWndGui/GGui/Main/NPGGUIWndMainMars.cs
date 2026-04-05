using UnityEngine;
using System.Collections;
using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 火星界面
    /// </summary>
    public class NPGGUIWndMainMars : _ANPGGUIBasicResBarWnd<NPGGUIMonoMainMars>
    {
        private static NPGGUIWndMainMars _g_instance = new NPGGUIWndMainMars();
        public static NPGGUIWndMainMars instance { get { return _g_instance ??= new NPGGUIWndMainMars(); } }
        

        private NPGGuiWndTexture _m_homeIconWnd;
        private NPGGUISubWndMiniChat _m_chatMiniWnd; // 聊天入口
        private GGUIWndMarsEventBuffItemContainer _m_marsEventBuffItemContainerWnd;
        private GGUISubWndMarsEventTip _m_marsEventTipWnd;
        private GGUISubWndMarsBuildQueue _m_marsBuildQueueWnd;


        private NPGGUIWndMainMars() 
            : base(EALUIWndLayer.NORMAL)
        {
        }
        

        protected override string _monoAssetPath { get { return NPGGUIMonoMainMars.assetPath; } }
        protected override string _monoObjName { get { return NPGGUIMonoMainMars.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_homeIconWnd?.showWnd();
            _m_chatMiniWnd?.showWnd();
            _m_marsEventBuffItemContainerWnd?.showWnd();
            _m_marsEventTipWnd?.showWnd();
            _m_marsBuildQueueWnd?.showWnd();

            refreshWnd();

            NPPlayer.instance.marsComp.onPowerChanged += _onPowerChanged;
            NPPlayer.instance.marsComp.onOxygenValueChanged += _onOxygenValueChanged;
            NPPlayer.instance.marsComp.buildingSubComponent.onBuildingLevelChg += _onBuildingLevelChanged;
            NPPlayer.instance.marsComp.onPeopleNumLimitChanged += _onPeopleNumDataChanged;
            WinMsg.RegisterMsgAct(WinMsgType.ON_MARS_PEOPLE_NUM_CHG, refreshPeopleNum);
        }
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_MARS_PEOPLE_NUM_CHG, refreshPeopleNum);
            NPPlayer.instance.marsComp.onPeopleNumLimitChanged -= _onPeopleNumDataChanged;
            NPPlayer.instance.marsComp.buildingSubComponent.onBuildingLevelChg -= _onBuildingLevelChanged;
            NPPlayer.instance.marsComp.onOxygenValueChanged -= _onOxygenValueChanged;
            NPPlayer.instance.marsComp.onPowerChanged -= _onPowerChanged;
            
            _m_marsEventBuffItemContainerWnd?.hideWnd();
            _m_marsEventTipWnd?.hideWnd();
            _m_homeIconWnd?.hideWnd();
            _m_marsBuildQueueWnd?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_marsEventBuffItemContainerWnd?.resetWnd();
            _m_marsEventTipWnd?.resetWnd();
            _m_homeIconWnd?.discardTexture();
            _m_marsBuildQueueWnd?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_chatMiniWnd?.discard();
            _m_marsEventBuffItemContainerWnd?.discard();
            _m_marsEventBuffItemContainerWnd = null;
            _m_marsEventTipWnd?.discard();
            _m_marsEventTipWnd = null;
            _m_homeIconWnd?.discard();
            _m_homeIconWnd = null;
            _m_marsBuildQueueWnd?.discard();
            _m_marsBuildQueueWnd = null;
            
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnEnergyYieldDetail, _onClickEnergyYieldDetail);
            ALUGUICommon.uncombineBtnClick(wnd.btnAIControl, _onAIControlBtnClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnPeopleDetail, _onPeopleDetailBtnClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnExplore, _onExploreBtnClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnPowerDetail, _onClickPowerDetail);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.chatMiniWndMono != null)
                _m_chatMiniWnd = new NPGGUISubWndMiniChat(wnd.chatMiniWndMono);
            if (wnd.monoMarsEventBuffItemContainer != null)
                _m_marsEventBuffItemContainerWnd = new GGUIWndMarsEventBuffItemContainer(wnd.monoMarsEventBuffItemContainer);
            if (wnd.monoMarsEventTip != null)
                _m_marsEventTipWnd = new GGUISubWndMarsEventTip(wnd.monoMarsEventTip);
            if (wnd.monoBuildQueue != null)
                _m_marsBuildQueueWnd = new GGUISubWndMarsBuildQueue(wnd.monoBuildQueue);
            
            ALUGUICommon.combineBtnClick(wnd.btnEnergyYieldDetail, _onClickEnergyYieldDetail);
            ALUGUICommon.combineBtnClick(wnd.btnAIControl, _onAIControlBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnPeopleDetail, _onPeopleDetailBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnExplore, _onExploreBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnPowerDetail, _onClickPowerDetail);
        }


        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            refreshPeopleNum();
            refreshMarsPower();
            refreshOxygenValue();
            refreshHomeData();
        }
        public void refreshPeopleNum()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            long totalPeopleNum = NPPlayer.instance.marsComp.totalPeopleNum;
            long peopleNumLimit = NPPlayer.instance.marsComp.peopleNumLimit;
            
            string txtPeopleNumKey = string.IsNullOrEmpty(wnd.txtPeopleNumKey) ? TransKeyConst.common_currentTotalNum_num_num : wnd.txtPeopleNumKey;
            ALUGUICommon.setLabelTxt(wnd.txtPeopleNum, TextTranslate.instance.getLanguage(txtPeopleNumKey, totalPeopleNum, peopleNumLimit));
        }
        public void refreshMarsPower()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtMarsPower, NPPlayer.instance.marsComp.power.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
        }
        public void refreshOxygenValue()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            long oxygenValue = NPPlayer.instance.marsComp.oxygenValue;
            wnd.setOxygenValue(oxygenValue);
        }
        public void refreshHomeData()
        {
            if (wnd == null || !_m_bIsShow)
                return;
            
            MarsBuildingInfo homeBuildingInfo = NPPlayer.instance.marsComp.buildingSubComponent.getBuildingInfoById(GRefdataCoreMgr.instance.npGeneral.mars_building_home_id);
            if (homeBuildingInfo == null || !homeBuildingInfo.homeData.isValid())
                return;

            ALUGUICommon.setLabelTxt(wnd.txtHomeLevel, homeBuildingInfo.level);
            _m_homeIconWnd?.setTexture(homeBuildingInfo.homeData.refObj.icon);
        }
        
        
        private void _onClickEnergyYieldDetail(GameObject _obj)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsBuildingEnergyYieldDetail.instance, GGUIWndMarsBuildingEnergyYieldDetail.instance.showWnd, 
                EUIQueueStageType.MAIN, UINodeTagConst_Mars.C_MARS_BUILDING_ENERGY_YIELD_DETAIL, false, false);   
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_obj"></param>
        private void _onAIControlBtnClick(GameObject _obj)
        {
            //打开AI控制界面
            // 火星智能控制主界面
            QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndMarsIntelligentControl.instance, UINodeTagConst.C_MARS_INTELLIGENT_CONTROL, null,
                () =>
                {
                    GGUIWndMarsIntelligentControl.instance.showWnd();
                },
                0);
        }

        private void _onExploreBtnClick(GameObject _obj)
        {
            GNodeMarsExplore.openOrQuitToNode(null);
        }

        private void _onClickPowerDetail(GameObject _obj)
        {
            QueueMgr.instance.AddNode(new GNodeMarsPowerDetailToolTip((RectTransform)_obj.transform, wnd.powerDetailToolTipInterval.x, wnd.powerDetailToolTipInterval.y));
        }

        /// <summary>
        /// 点击居民详情按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onPeopleDetailBtnClick(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd_OnlyCloseDiscard(GGUIWndMarsResident.instance, () =>
            {
                GGUIWndMarsResident.instance.showWnd();
            }, EUIQueueStageType.MAIN, UINodeTagConst.C_MARS_RESIDENT, false, false);
        }
        
        private void _onPowerChanged(long _)
        {
            refreshMarsPower();
        }

        private void _onOxygenValueChanged(long _)
        {
            refreshOxygenValue();
        }

        private void _onBuildingLevelChanged(long _buildingId, long _oldLevel)
        {
            if (_buildingId == GRefdataCoreMgr.instance.npGeneral.mars_building_home_id)
                refreshHomeData();
        }

        private void _onPeopleNumDataChanged(long _)
        {
            refreshPeopleNum();
        }
    }
}
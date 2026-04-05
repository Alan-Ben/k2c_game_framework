using ALPackage;
using GC2GS.p034_InnOp;
using GS2GC.p034_InnOp;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndInnStationLevelUp : _ATALBasicUIWnd<GGUIMonoInnStationLevelUp>
    {
        [NotNull] public static GGUIWndInnStationLevelUp instance { get { return _g_instance ??= new GGUIWndInnStationLevelUp(); } }
        private static GGUIWndInnStationLevelUp _g_instance;


        private InnStationInfo _m_stationInfo;
        private InnStationInfo _m_nextStationInfo;
        private InnStationInfo _m_prevStationInfo;

        private NPGGuiWndTexture _m_iconWnd;
        private TextUpgradePropertyShow<long> _m_levelChgShow;
        private TextUpgradePropertyShow<long> _m_popularityGainChgShow;
        private TextUpgradePropertyShow<long> _m_finesseGainChgShow;
        private NPGGUIWndCommonItem _m_costItemWnd;
        

        public GGUIWndInnStationLevelUp()
            : base(EALUIWndLayer.ADDITION)
        {
        }


        protected override string _monoAssetPath { get { return GGUIMonoInnStationLevelUp.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoInnStationLevelUp.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            _m_iconWnd?.showWnd();
            _m_costItemWnd?.showWnd();

            refreshWnd();

            NPPlayer.instance.innComp.onStationChg += _onStationChg;
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_INN_STATION_LEVEL_UP_BUTTON, _onSimulateClickLevelUp);
        }
        protected override void _onHideWnd()
        {
            NPPlayer.instance.innComp.onStationChg -= _onStationChg;
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_INN_STATION_LEVEL_UP_BUTTON, _onSimulateClickLevelUp);
            
            _m_iconWnd?.hideWnd();
            _m_costItemWnd?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_iconWnd?.discardTexture();
            _m_costItemWnd?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_iconWnd?.discard();
            _m_iconWnd = null;
            _m_costItemWnd?.discard();
            _m_costItemWnd = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnLevelUp, _onBtnLevelUpClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnNextBuilding, _onBtnNextBuildingClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnPrevBuilding, _onBtnPrevBuildingClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgIcon != null)
                _m_iconWnd = new NPGGuiWndTexture(wnd.imgIcon);
            if (wnd.txtLevelChg != null)
                _m_levelChgShow = new TextUpgradePropertyShow<long>(wnd.txtLevelChg, TransKeyConst.common_level_num);
            if (wnd.txtPopularityGainChg != null)
                _m_popularityGainChgShow = new TextUpgradePropertyShow<long>(wnd.txtPopularityGainChg, string.Empty);
            if (wnd.txtFinesseGainChg != null)
                _m_finesseGainChgShow = new TextUpgradePropertyShow<long>(wnd.txtFinesseGainChg, string.Empty);
            if (wnd.monoCostItem != null)
                _m_costItemWnd = new NPGGUIWndCommonItem(wnd.monoCostItem);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.combineBtnClick(wnd.btnLevelUp, _onBtnLevelUpClick);
            ALUGUICommon.combineBtnClick(wnd.btnNextBuilding, _onBtnNextBuildingClick);
            ALUGUICommon.combineBtnClick(wnd.btnPrevBuilding, _onBtnPrevBuildingClick);
        }
        
        
        public void refreshWnd(InnStationInfo _stationInfo)
        {
            _m_stationInfo = _stationInfo;
            _m_nextStationInfo = NPPlayer.instance.innComp.getNextBuiltStation(_m_stationInfo);
            _m_prevStationInfo = NPPlayer.instance.innComp.getPrevBuiltStation(_m_stationInfo);
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_stationInfo == null)
                return;

            InnStationLevelRefObj curLevelRefObj = _m_stationInfo.levelRefObj;
            InnStationLevelRefObj nextLevelRefObj = _m_stationInfo.nextLevelRefObj ?? curLevelRefObj;
            ALUGUICommon.setLabelTxt(wnd.txtLevelAndName, TextTranslate.instance.getLanguage(TransKeyConst.inn_stationLevelAndName_level_name, _m_stationInfo.level, _m_stationInfo.nameTranslated));
            _m_iconWnd?.setTexture(_m_stationInfo.refObj.icon);
            _m_levelChgShow?.setValue(curLevelRefObj?.level ?? 0, nextLevelRefObj?.level ?? 0);
            _m_popularityGainChgShow?.setValue(curLevelRefObj?.popularity_add ?? 0, nextLevelRefObj?.popularity_add ?? 0);
            _m_finesseGainChgShow?.setValue(curLevelRefObj?.finesse_add ?? 0, nextLevelRefObj?.finesse_add ?? 0);
            ALUGUICommon.setLabelTxt(wnd.txtDesc, _m_stationInfo.descTranslated);
            bool canUpgradeStation = NPPlayer.instance.innComp.canUpgradeStation();
            wnd.setIsUnlock(canUpgradeStation);
            wnd.setIsLevelMax(_m_stationInfo.isLevelMax);
            wnd.setHasNextBuilding(_m_nextStationInfo != null);
            wnd.setHasPrevBuilding(_m_prevStationInfo != null);
            _m_costItemWnd?.setItem(curLevelRefObj?.upgrade_cost);
        }


        private void _onBtnCloseClick(GameObject _)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_INN_STATION_LEVEL_UP);
        }
        private void _onBtnLevelUpClick(GameObject _)
        {
            if (_m_stationInfo == null)
                return;

            NPGSClientListener.sendRequestByLog(new GC2GS_034_003_ReqInnStationUpgrade(_m_stationInfo.stationId), 
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_034_003_RetInnStationUpgrade>((_isSuc, _msg) =>
                {
                    if (_isSuc)
                    {
                        GGUIWndInnStationLevelUpSuccess.instance.refreshWnd(_m_stationInfo);
                        QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndInnStationLevelUpSuccess.instance, GGUIWndInnStationLevelUpSuccess.instance.showWnd, UINodeTagConst.C_INN_STATION_LEVEL_UP_SUCCESS);
                    }
                }));
        }
        private void _onStationChg(InnStationInfo _stationInfo)
        {
            if (_stationInfo == null || _m_stationInfo == null || _m_stationInfo != _stationInfo)
                return;

            refreshWnd();
        }
        private void _onBtnNextBuildingClick(GameObject _)
        {
            if (_m_nextStationInfo == null)
                return;
            
            refreshWnd(_m_nextStationInfo);
        }
        private void _onBtnPrevBuildingClick(GameObject _)
        {
            if (_m_prevStationInfo == null)
                return;

            refreshWnd(_m_prevStationInfo);
        }
        private void _onSimulateClickLevelUp()
        {
            if (wnd == null) return;
            _onBtnLevelUpClick(wnd.btnLevelUp);
        }
    }
}
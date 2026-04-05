using ALPackage;
using CommonEnum;
using GC2GS.p034_InnOp;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUIWndInnStationBuild : _ATALBasicUIWnd<GGUIMonoInnStationBuild>
    {
        [NotNull] public static GGUIWndInnStationBuild instance { get { return _g_instance ??= new GGUIWndInnStationBuild(); } }
        private static GGUIWndInnStationBuild _g_instance;


        private InnStationInfo _m_stationInfo;

        private NPGGuiWndTexture _m_iconWnd;
        private NPGGUIWndCommonItem _m_costItemWnd;
        private InnViewMgr _m_viewMgr;
        
        
        public GGUIWndInnStationBuild() 
            : base(EALUIWndLayer.ADDITION)
        {
        }
        

        protected override string _monoAssetPath { get { return GGUIMonoInnStationBuild.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoInnStationBuild.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_iconWnd?.showWnd();
            _m_costItemWnd?.showWnd();
            
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_INN_STATION_BUILD_BUTTON, _onSimulateBuildClick);
            
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_iconWnd?.hideWnd();
            _m_costItemWnd?.hideWnd();
            
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_INN_STATION_BUILD_BUTTON, _onSimulateBuildClick);
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
            ALUGUICommon.uncombineBtnClick(wnd.btnBuild, _onBtnBuildClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgIcon != null)
                _m_iconWnd = new NPGGuiWndTexture(wnd.imgIcon);
            if (wnd.monoCostItem != null)
                _m_costItemWnd = new NPGGUIWndCommonItem(wnd.monoCostItem);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.combineBtnClick(wnd.btnBuild, _onBtnBuildClick);
        }


        public void refreshWnd(InnStationInfo _stationInfo, InnViewMgr _viewMgr)
        {
            _m_stationInfo = _stationInfo;
            _m_viewMgr = _viewMgr;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_stationInfo == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtName, _m_stationInfo.nameTranslated);
            _m_iconWnd?.setTexture(_m_stationInfo.refObj.icon);
            ALUGUICommon.setLabelTxt(wnd.txtPopularityGain, TextTranslate.instance.getLanguage(TransKeyConst.inn_stationPopularityGain_num, _m_stationInfo.levelRefObj?.popularity_add ?? 0));
            ALUGUICommon.setLabelTxt(wnd.txtFinesseGain, TextTranslate.instance.getLanguage(TransKeyConst.inn_stationFinesseGain_num, _m_stationInfo.levelRefObj?.finesse_add ?? 0));
            ALUGUICommon.setLabelTxt(wnd.txtDesc, _m_stationInfo.descTranslated);
            InnStationInfo requireStationInfo = NPPlayer.instance.innComp.getRequireStationInfo(_m_stationInfo);
            bool isNextBuilding = requireStationInfo is { isBuilt: true } or null;
            long currentGuestNum = _m_viewMgr.getHadSettleGuestsCount();
            long requireGuestNum = _m_stationInfo.refObj.need_receive_guest_num;
            ALUGUICommon.setLabelTxt(wnd.txtUnlockTip, TextTranslate.instance.getLanguage(TransKeyConst.inn_stationUnlockTip_name, requireStationInfo?.nameTranslated));
            if (currentGuestNum >= requireGuestNum)
                ALUGUICommon.setLabelTxt(wnd.txtGuestServeRequire, TextTranslate.instance.getLanguage(TransKeyConst.common_currentTotalNum_num_num, currentGuestNum, requireGuestNum));
            else
                ALUGUICommon.setLabelTxt(wnd.txtGuestServeRequire,
                    TextTranslate.instance.getLanguage(TransKeyConst.common_currentTotalNum_num_num, GCommon.addColorForRichText(currentGuestNum.ToString(), wnd.notEnoughColor), requireGuestNum));
            _m_costItemWnd?.setItem(_m_stationInfo.refObj.build_cost);
            wnd.setIsNextBuilding(isNextBuilding);
        }
        
        
        private void _onBtnCloseClick(GameObject _)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_INN_STATION_BUILD);
        }
        private void _onBtnBuildClick(GameObject _)
        {
            if (_m_stationInfo == null)
                return;

            if (!GCommon.isItemEnough(_m_stationInfo.refObj.build_cost, true))
                return;

            NPGSClientListener.sendRequestByLog(new GC2GS_034_002_ReqInnStationUnlock(_m_stationInfo.stationId), 
                new CommonErrCodeRequestCallbackDispatherTriggerDealer(() =>
                {
                    _onBtnCloseClick(null);
                    GGUIWndInnStationBuildSuccess.instance.refreshWnd(_m_stationInfo);
                    QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndInnStationBuildSuccess.instance, GGUIWndInnStationBuildSuccess.instance.showWnd, UINodeTagConst.C_INN_STATION_BUILD_SUCCESS);
                }));
        }
        
        private void _onSimulateBuildClick()
        {
            if (wnd == null) 
                return;
            
            _onBtnBuildClick(wnd.btnBuild);
        }
    }
}
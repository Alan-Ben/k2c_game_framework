using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndInnStationBuildSuccess : _ATALBasicUIWnd<GGUIMonoInnStationBuildSuccess>
    {
        [NotNull] public static GGUIWndInnStationBuildSuccess instance { get { return _g_instance ??= new GGUIWndInnStationBuildSuccess(); } }
        private static GGUIWndInnStationBuildSuccess _g_instance;


        private InnStationInfo _m_stationInfo;
        
        private NPGGuiWndTexture _m_iconWnd;
        

        public GGUIWndInnStationBuildSuccess()
            : base(EALUIWndLayer.ADDITION)
        {
        }


        protected override string _monoAssetPath { get { return GGUIMonoInnStationBuildSuccess.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoInnStationBuildSuccess.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            _m_iconWnd?.showWnd();

            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_iconWnd?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_iconWnd?.discardTexture();
        }
        protected override void _onDiscard()
        {
            _m_iconWnd?.discard();
            _m_iconWnd = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.imgIcon != null)
                _m_iconWnd = new NPGGuiWndTexture(wnd.imgIcon);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }


        public void refreshWnd(InnStationInfo _stationInfo)
        {
            _m_stationInfo = _stationInfo;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_stationInfo == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(TransKeyConst.inn_stationBuildSuccessTitle_name, _m_stationInfo.nameTranslated));
            _m_iconWnd?.setTexture(_m_stationInfo.refObj.station_tex);
            ALUGUICommon.setLabelTxt(wnd.txtDesc, _m_stationInfo.descTranslated);
        }


        private void _onBtnCloseClick(GameObject _)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_INN_STATION_BUILD_SUCCESS);
        }
    }
}
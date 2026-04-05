using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndInnStationLevelUpSuccess : _ATALBasicUIWnd<GGUIMonoInnStationLevelUpSuccess>
    {
        [NotNull] public static GGUIWndInnStationLevelUpSuccess instance { get { return _g_instance ??= new GGUIWndInnStationLevelUpSuccess(); } }
        private static GGUIWndInnStationLevelUpSuccess _g_instance;


        private InnStationInfo _m_stationInfo;

        private NPGGuiWndTexture _m_iconWnd;
        private TextUpgradePropertyShow<long> _m_levelUpgradeShow;
        private TextUpgradePropertyShow<long> _m_popularityUpgradeShow;
        private TextUpgradePropertyShow<long> _m_finesseUpgradeShow;
        

        public GGUIWndInnStationLevelUpSuccess()
            : base(EALUIWndLayer.ADDITION)
        {
        }


        protected override string _monoAssetPath { get { return GGUIMonoInnStationLevelUpSuccess.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoInnStationLevelUpSuccess.objName; } }
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
            if (wnd.levelUpgradeShow != null)
                _m_levelUpgradeShow = new TextUpgradePropertyShow<long>(wnd.levelUpgradeShow, string.Empty);
            if (wnd.popularityUpgradeShow != null)
                _m_popularityUpgradeShow = new TextUpgradePropertyShow<long>(wnd.popularityUpgradeShow, string.Empty);
            if (wnd.finesseUpgradeShow != null)
                _m_finesseUpgradeShow = new TextUpgradePropertyShow<long>(wnd.finesseUpgradeShow, string.Empty);
            
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

            InnStationLevelRefObj curLevelRef = _m_stationInfo.levelRefObj;
            InnStationLevelRefObj prevLevelRef = GRefdataCoreMgr.instance.getInnStationLevelRef(_m_stationInfo.stationId, _m_stationInfo.level - 1) ?? curLevelRef;
            _m_iconWnd?.setTexture(_m_stationInfo.refObj.icon);
            ALUGUICommon.setLabelTxt(wnd.txtName, _m_stationInfo.nameTranslated);
            _m_levelUpgradeShow?.setValue(prevLevelRef?.level ?? 0, curLevelRef?.level ?? 0);
            _m_popularityUpgradeShow?.setValue(prevLevelRef?.popularity_add ?? 0, curLevelRef?.popularity_add ?? 0);
            _m_finesseUpgradeShow?.setValue(prevLevelRef?.finesse_add ?? 0, curLevelRef?.finesse_add ?? 0);
        }


        private void _onBtnCloseClick(GameObject _)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_INN_STATION_LEVEL_UP_SUCCESS);
        }
    }
}
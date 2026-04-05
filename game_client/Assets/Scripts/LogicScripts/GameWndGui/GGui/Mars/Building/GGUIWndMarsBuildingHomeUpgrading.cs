using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndMarsBuildingHomeUpgrading : GGUIWndMarsBuildingUpgrading<GGUIMonoMarsBuildingHomeUpgrading>
    {
        [NotNull] public static GGUIWndMarsBuildingHomeUpgrading instance { get { return _g_instance ??= new GGUIWndMarsBuildingHomeUpgrading(); } }
        private static GGUIWndMarsBuildingHomeUpgrading _g_instance;


        private TextUpgradePropertyShow<long> _m_consumeUpgradeShow;
        private _IMarsBuildingView _m_homeUpgradingMarsBuildingView;


        public GGUIWndMarsBuildingHomeUpgrading()
            : base()
        {
        }


        protected override string _monoAssetPath { get { return GGUIMonoMarsBuildingHomeUpgrading.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsBuildingHomeUpgrading.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            base._onShowWnd();
            refreshHomeSpecificData();
        }
        protected override void _onDiscard()
        {
            base._onDiscard();
            _m_consumeUpgradeShow = null;
        }
        protected override void _onWndInitDone()
        {
            base._onWndInitDone();

            if (wnd == null)
                return;

            if (wnd.txtConsume != null)
                _m_consumeUpgradeShow = new TextUpgradePropertyShow<long>(wnd.txtConsume, TransKeyConst.mars_energyConsumePerMin_num);
        }

        public void refreshWnd(_IMarsBuildingView _buildingView)
        {
            base.refreshWnd(_buildingView);
            _m_homeUpgradingMarsBuildingView = _buildingView;
        }
        public void refreshHomeSpecificData()
        {
            if (wnd == null || !_m_bIsShow || _m_homeUpgradingMarsBuildingView == null)
                return;

            MarsBuildingInfo buildingInfo = _m_homeUpgradingMarsBuildingView.buildingInfo;

            MarsBuildingHomeLevelRefObj currentRefObj = buildingInfo.homeData.refObj;
            MarsBuildingHomeLevelRefObj nextLevelRefObj = buildingInfo.homeData.nextRefObj ?? currentRefObj;

            long currentConsume = currentRefObj?.energy_consume_per_min ?? 0;
            long nextConsume = nextLevelRefObj?.energy_consume_per_min ?? 0;

            _m_consumeUpgradeShow?.setValue(currentConsume, nextConsume);
        }

        protected override void _onBtnBackClick(GameObject _obj)
        {
            if (_m_homeUpgradingMarsBuildingView == null)
                return;

            _onBtnCloseClick(null);
            GGUIWndMarsBuildingHomeInfo.instance.refreshWnd(_m_homeUpgradingMarsBuildingView);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsBuildingHomeInfo.instance, GGUIWndMarsBuildingHomeInfo.instance.showWnd,
                EUIQueueStageType.MAIN, UINodeTagConst_Mars.C_MARS_BUILDING_HOME_INFO, false, false);
        }

        protected override void _onBtnCloseClick(GameObject _obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Mars.C_MARS_BUILDING_HOME_UPGRADING);
        }
    }
}
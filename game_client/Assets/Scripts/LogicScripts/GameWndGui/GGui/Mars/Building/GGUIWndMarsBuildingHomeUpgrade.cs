using ALPackage;
using CommonEnum;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndMarsBuildingHomeUpgrade : GGUIWndMarsBuildingUpgrade<GGUIMonoMarsBuildingHomeUpgrade>
    {
        [NotNull] public static GGUIWndMarsBuildingHomeUpgrade instance { get { return _g_instance ??= new GGUIWndMarsBuildingHomeUpgrade(); } }
        private static GGUIWndMarsBuildingHomeUpgrade _g_instance;


        private TextUpgradePropertyShow<long> _m_consumeUpgradeShow;
        private TextUpgradePropertyShow<long> _m_outputUpgradeShow;
        private TextUpgradePropertyShow<string> _m_powerUpgradeShow;
        private _IMarsBuildingView _m_homeNormalMarsBuildingView;
        

        public GGUIWndMarsBuildingHomeUpgrade() 
            : base()
        {
        }
        

        protected override string _monoAssetPath { get { return GGUIMonoMarsBuildingHomeUpgrade.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsBuildingHomeUpgrade.objName; } }
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
                _m_consumeUpgradeShow = new TextUpgradePropertyShow<long>(wnd.txtConsume, wnd.txtConsumeKey);
            if (wnd.txtOutput != null)
                _m_outputUpgradeShow = new TextUpgradePropertyShow<long>(wnd.txtOutput, wnd.txtOutputKey);
            if (wnd.txtPower != null)
                _m_powerUpgradeShow = new TextUpgradePropertyShow<string>(wnd.txtPower, wnd.txtPowerKey);
        }
        

        public void refreshWnd(_IMarsBuildingView _buildingView)
        {
            base.refreshWnd(_buildingView);
            _m_homeNormalMarsBuildingView = _buildingView;
        }
        public void refreshHomeSpecificData()
        {
            if (wnd == null || !_m_bIsShow || _m_homeNormalMarsBuildingView == null)
                return;

            MarsBuildingInfo buildingInfo = _m_homeNormalMarsBuildingView.buildingInfo;
            
            MarsBuildingHomeLevelRefObj currentHomeLevelRefObj = buildingInfo.homeData.refObj;
            MarsBuildingHomeLevelRefObj nextLevelHomeLevelRefObj = buildingInfo.homeData.nextRefObj ?? currentHomeLevelRefObj;
            
            //获取当前基地等级和下一个基地等级的能量消耗
            long currentConsume = currentHomeLevelRefObj?.energy_consume_per_min ?? 0;
            long nextConsume = nextLevelHomeLevelRefObj?.energy_consume_per_min ?? 0;
            _m_consumeUpgradeShow?.setValue(currentConsume, nextConsume);

            //获取当前基地等级和下一个基地等级
            NPCommonCostItem curOutput = currentHomeLevelRefObj?.output_per_min.getItem(NPEnum.ENPItemType.CURRENCY, (long)ECurrency.MARS_POINT);
            NPCommonCostItem nextOutput = nextLevelHomeLevelRefObj?.output_per_min.getItem(NPEnum.ENPItemType.CURRENCY, (long)ECurrency.MARS_POINT);
            _m_outputUpgradeShow?.setValue(curOutput?.count ?? 0, nextOutput?.count ?? 0);

            MarsBuildingLevelRefObj currentLevelRefObj = buildingInfo.levelData.refObj;
            MarsBuildingLevelRefObj nextLevelRefObj = buildingInfo.levelData.nextRefObj ?? currentLevelRefObj;
            
            //获取当前建筑等级和下一个建筑等级的火星实力
            long currentPower = currentLevelRefObj?.mars_power_value ?? 0;
            long nextPower = nextLevelRefObj?.mars_power_value ?? 0;
            _m_powerUpgradeShow?.setValue(currentPower.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT), nextPower.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
        }

        protected override void _onBtnBackClick(GameObject _obj)
        {
            if (_m_homeNormalMarsBuildingView == null)
                return;

            _onBtnCloseClick(null);
            GGUIWndMarsBuildingHomeInfo.instance.refreshWnd(_m_homeNormalMarsBuildingView);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsBuildingHomeInfo.instance, GGUIWndMarsBuildingHomeInfo.instance.showWnd,
                EUIQueueStageType.MAIN, UINodeTagConst_Mars.C_MARS_BUILDING_HOME_INFO, false, false);
        }
        
        protected override void _onBtnCloseClick(GameObject _obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Mars.C_MARS_BUILDING_HOME_UPGRADE);
        }

        /// <summary>
        /// 在处理升级请求时调用的处理函数，可以在子类强制处理一些其他操作
        /// </summary>
        protected override void _onDealUpgrade()
        {
            NPPlayer.instance.marsComp.buildingSubComponent.collectHome();
        }
    }
}
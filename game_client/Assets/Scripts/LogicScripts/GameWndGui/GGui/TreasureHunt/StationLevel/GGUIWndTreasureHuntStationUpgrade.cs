using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 太空舱升级弹窗
    /// </summary>
    public class GGUIWndTreasureHuntStationUpgrade : _ANPGGUIBasicWnd<GGUIMonoTreasureHuntStationUpgrade>
    {
        private static GGUIWndTreasureHuntStationUpgrade _g_instance;

        public static GGUIWndTreasureHuntStationUpgrade instance { get { return _g_instance ??= new GGUIWndTreasureHuntStationUpgrade(); } }

        private TreasureHuntStationLvlRefObj _m_nowStationLevelRefObj;
        private TreasureHuntStationLvlRefObj _m_preStationLevelRefObj;

        private NPGGuiWndTexture _m_wUnlockAreaIcon;
        private TextUpgradePropertyShow<string> _m_pickupExpUpgradeShow;
        private TextUpgradePropertyShow<string> _m_autoFlyDistanceUpgradeShow;
        private TextUpgradePropertyShow<string> _m_maxFlyDistanceUpgradeShow;
        private TextUpgradePropertyShow<string> _m_flyProtectTimesUpgradeShow;


        public GGUIWndTreasureHuntStationUpgrade() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoTreasureHuntStationUpgrade.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoTreasureHuntStationUpgrade.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            // 初始化解锁区域图标
            if (wnd.imgUnlockAreaIcon != null)
                _m_wUnlockAreaIcon = new NPGGuiWndTexture(wnd.imgUnlockAreaIcon);

            // 初始化属性升级显示组件
            if (wnd.pickupExpUpgradeShow != null)
                _m_pickupExpUpgradeShow = new TextUpgradePropertyShow<string>(wnd.pickupExpUpgradeShow, string.Empty);

            if (wnd.autoFlyDistanceUpgradeShow != null)
                _m_autoFlyDistanceUpgradeShow =
                    new TextUpgradePropertyShow<string>(wnd.autoFlyDistanceUpgradeShow, string.Empty);

            if (wnd.maxFlyDistanceUpgradeShow != null)
                _m_maxFlyDistanceUpgradeShow =
                    new TextUpgradePropertyShow<string>(wnd.maxFlyDistanceUpgradeShow, string.Empty);

            if (wnd.flyProtectTimesUpgradeShow != null)
                _m_flyProtectTimesUpgradeShow =
                    new TextUpgradePropertyShow<string>(wnd.flyProtectTimesUpgradeShow, string.Empty);

            // 绑定关闭按钮
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }


        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            }

            _m_wUnlockAreaIcon?.discard();
            _m_wUnlockAreaIcon = null;

            _m_pickupExpUpgradeShow = null;
            _m_autoFlyDistanceUpgradeShow = null;
            _m_maxFlyDistanceUpgradeShow = null;
            _m_flyProtectTimesUpgradeShow = null;

            _m_nowStationLevelRefObj = null;
            _m_preStationLevelRefObj = null;
        }


        protected override void _onShowWnd()
        {
            _refreshWnd();
        }


        protected override void _onHideWnd()
        {
            _m_wUnlockAreaIcon?.hideWnd();
        }


        protected override void _onReset()
        {
            _m_wUnlockAreaIcon?.discardTexture();
        }


        /// <summary>
        /// 设置数据并刷新界面
        /// </summary>
        public void setData(long _stationLevel)
        {
            _m_nowStationLevelRefObj = GRefdataCoreMgr.instance.treasureHuntStationLvlRefCore.getRef(_stationLevel);
            _m_preStationLevelRefObj = GRefdataCoreMgr.instance.treasureHuntStationLvlRefCore.getRef(_stationLevel - 1);
            if (_m_nowStationLevelRefObj == null || _m_preStationLevelRefObj == null)
                return;

            _refreshWnd();
        }

        /// <summary>
        /// 刷新窗口显示
        /// </summary>
        private void _refreshWnd()
        {
            if (wnd == null || !isShow || _m_nowStationLevelRefObj == null || _m_preStationLevelRefObj == null)
                return;

            // 设置太空舱等级文本
            ALUGUICommon.setLabelTxt(wnd.txtLevel,
                TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, _m_nowStationLevelRefObj.level));

            // 刷新每次拾取经验值显示
            _m_pickupExpUpgradeShow?.setValue(_m_preStationLevelRefObj.each_pickup_exp.ToString(), _m_nowStationLevelRefObj.each_pickup_exp.ToString());
            
            // 刷新自动飞行距离显示
            _m_autoFlyDistanceUpgradeShow?.setValue(_m_preStationLevelRefObj.auto_fly_distance.ToString(), _m_nowStationLevelRefObj.auto_fly_distance.ToString());

            // 刷新最大飞行距离显示
            _m_maxFlyDistanceUpgradeShow?.setValue(_m_preStationLevelRefObj.max_fly_distance.ToString(), _m_nowStationLevelRefObj.max_fly_distance.ToString());

            // 刷新飞行保护次数显示
            _m_flyProtectTimesUpgradeShow?.setValue(_m_preStationLevelRefObj.fly_protect_times.ToString(), _m_nowStationLevelRefObj.fly_protect_times.ToString());

            // 刷新解锁区域显示
            _refreshUnlockAreaShow();
        }

        /// <summary>
        /// 刷新解锁区域显示
        /// </summary>
        private void _refreshUnlockAreaShow()
        {
            if (wnd == null || _m_nowStationLevelRefObj == null)
                return;

            // 判断当前级是否会解锁新区域
            bool hasUnlockArea = _m_nowStationLevelRefObj.unlock_area_id > 0;

            // 显示或隐藏解锁区域相关显示
            ALUGUICommon.setGameObjEnable(wnd.hasUnlockAreaShowGoList, hasUnlockArea);

            if (!hasUnlockArea)
                return;

            // 获取解锁区域配表数据
            TreasureHuntAreaRefObj areaRefObj =
                GRefdataCoreMgr.instance.treasureHuntAreaRefCore.getRef(_m_nowStationLevelRefObj.unlock_area_id);
            if (areaRefObj == null)
                return;

            // 设置解锁区域图标
            if (_m_wUnlockAreaIcon != null)
            {
                _m_wUnlockAreaIcon.showWnd();
                _m_wUnlockAreaIcon.setTexture(areaRefObj.thumbnail_image);
            }

            // 设置解锁区域名称
            ALUGUICommon.setLabelTxt(wnd.txtUnlockAreaName, TextTranslate.instance.getLanguage(areaRefObj.name));
        }


        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TREASURE_HUNT_STATION_UPGRADE);
        }
    }
}

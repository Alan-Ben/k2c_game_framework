using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndTreasureHuntStationLevelDetail : _ANPGGUIBasicWnd<GGUIMonoTreasureHuntStationLevelDetail>
    {
        private static GGUIWndTreasureHuntStationLevelDetail _g_instance;
        public static GGUIWndTreasureHuntStationLevelDetail instance { get { return _g_instance ??= new GGUIWndTreasureHuntStationLevelDetail(); } }

        private TreasureHuntStationInfo _m_iStationInfo;
        
        private NPGGUIWndProgress _m_wLevelProgress;
        private TextUpgradePropertyShow<string> _m_levelUpgradeShow;
        private TextUpgradePropertyShow<string> _m_pickupExpUpgradeShow;
        private TextUpgradePropertyShow<string> _m_autoFlyDistanceUpgradeShow;
        private TextUpgradePropertyShow<string> _m_maxFlyDistanceUpgradeShow;
        private TextUpgradePropertyShow<string> _m_flyProtectTimesUpgradeShow;
        
        public GGUIWndTreasureHuntStationLevelDetail() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoTreasureHuntStationLevelDetail.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoTreasureHuntStationLevelDetail.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoLevelProgress != null)
                _m_wLevelProgress = new NPGGUIWndProgress(wnd.monoLevelProgress);
            
            if (wnd.levelUpgradeShow != null)
                _m_levelUpgradeShow = new TextUpgradePropertyShow<string>(wnd.levelUpgradeShow, string.Empty);

            if (wnd.pickupExpUpgradeShow != null)
                _m_pickupExpUpgradeShow = new TextUpgradePropertyShow<string>(wnd.pickupExpUpgradeShow, string.Empty);

            if (wnd.autoFlyDistanceUpgradeShow != null)
                _m_autoFlyDistanceUpgradeShow = new TextUpgradePropertyShow<string>(wnd.autoFlyDistanceUpgradeShow, string.Empty);

            if (wnd.maxFlyDistanceUpgradeShow != null)
                _m_maxFlyDistanceUpgradeShow = new TextUpgradePropertyShow<string>(wnd.maxFlyDistanceUpgradeShow, string.Empty);

            if (wnd.flyProtectTimesUpgradeShow != null)
                _m_flyProtectTimesUpgradeShow = new TextUpgradePropertyShow<string>(wnd.flyProtectTimesUpgradeShow, string.Empty);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            }

            _m_iStationInfo = null;
            
            _m_wLevelProgress?.discard();
            _m_wLevelProgress = null;

            _m_levelUpgradeShow = null;
            _m_pickupExpUpgradeShow = null;
            _m_autoFlyDistanceUpgradeShow = null;
            _m_maxFlyDistanceUpgradeShow = null;
            _m_flyProtectTimesUpgradeShow = null;
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wLevelProgress?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wLevelProgress?.resetWnd();
        }
        
        public void setData(TreasureHuntStationInfo _stationInfo)
        {
            _m_iStationInfo = _stationInfo;

            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if (wnd == null || !isShow || _m_iStationInfo == null || _m_iStationInfo.stationLvlRefObj == null)
                return;
            
            // 设置当前等级显示
            ALUGUICommon.setLabelTxt(wnd.txtCurLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, _m_iStationInfo.stationLevel));
            
            // 获取当前等级和下一等级的配表数据
            TreasureHuntStationLvlRefObj currentLevelRefObj = _m_iStationInfo.stationLvlRefObj;
            TreasureHuntStationLvlRefObj nextLevelRefObj = null;
            nextLevelRefObj = GRefdataCoreMgr.instance.treasureHuntStationLvlRefCore.getRef(_m_iStationInfo.stationLevel + 1);
            
            // 设置进度条
            if (_m_wLevelProgress != null)
            {
                _m_wLevelProgress.showWnd();
                
                if (nextLevelRefObj != null) // 非满级
                {
                    // 设置进度条显示
                    _m_wLevelProgress.setProgress(_m_iStationInfo.stationExp, currentLevelRefObj.level_up_need_exp, EValueFormatType.NORMAL);
                }
                else // 满级
                {
                    // 满级时显示满进度条
                    _m_wLevelProgress.setProgress(1.0f);
                }
            }
            
            // 设置满级状态显示
            ALUGUICommon.setGameObjEnable(wnd.maxLevelShow, nextLevelRefObj == null);
            ALUGUICommon.setGameObjEnable(wnd.maxLevelHide, nextLevelRefObj != null);
            
            // 设置等级提升显示
            if (_m_levelUpgradeShow != null)
            {
                // 当前等级和下一等级的等级对比
                string currentLevelStr = TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, _m_iStationInfo.stationLevel);
                string nextLevelStr = TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, nextLevelRefObj == null ? _m_iStationInfo.stationLevel : nextLevelRefObj.level);
                
                _m_levelUpgradeShow.setValue(currentLevelStr, nextLevelStr);
            }

            // 设置每次拾取经验值显示
            if (_m_pickupExpUpgradeShow != null)
            {
                string cur = currentLevelRefObj.each_pickup_exp.ToString();
                string nxt = (nextLevelRefObj == null ? currentLevelRefObj.each_pickup_exp : nextLevelRefObj.each_pickup_exp).ToString();
                _m_pickupExpUpgradeShow.setValue(cur, nxt);
            }

            // 设置自动飞行距离显示
            if (_m_autoFlyDistanceUpgradeShow != null)
            {
                string cur = currentLevelRefObj.auto_fly_distance.ToString();
                string nxt = (nextLevelRefObj == null ? currentLevelRefObj.auto_fly_distance : nextLevelRefObj.auto_fly_distance).ToString();
                _m_autoFlyDistanceUpgradeShow.setValue(cur, nxt);
            }

            // 设置最大飞行距离显示
            if (_m_maxFlyDistanceUpgradeShow != null)
            {
                string cur = currentLevelRefObj.max_fly_distance.ToString();
                string nxt = (nextLevelRefObj == null ? currentLevelRefObj.max_fly_distance : nextLevelRefObj.max_fly_distance).ToString();
                _m_maxFlyDistanceUpgradeShow.setValue(cur, nxt);
            }

            // 设置飞行保护次数显示
            if (_m_flyProtectTimesUpgradeShow != null)
            {
                string cur = currentLevelRefObj.fly_protect_times.ToString();
                string nxt = (nextLevelRefObj == null ? currentLevelRefObj.fly_protect_times : nextLevelRefObj.fly_protect_times).ToString();
                _m_flyProtectTimesUpgradeShow.setValue(cur, nxt);
            }
        }
        
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TREASURE_HUNT_STATION_LEVEL_DETAIL);
        }
    }
}
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndTreasureHuntStationLevel : _ANPGGUIBasicSubWnd<GGUISubMonoTreasureHuntStationLevel>
    {
        private TreasureHuntStationInfo _m_iStationInfo;
        private NPGGUIWndProgress _m_wLevelProgress;
        
        public GGUISubWndTreasureHuntStationLevel(GGUISubMonoTreasureHuntStationLevel _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoLevelProgress != null)
                _m_wLevelProgress = new NPGGUIWndProgress(wnd.monoLevelProgress);
            
            ALUGUICommon.combineBtnClick(wnd.btnDetail, _onClickDetail);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnDetail, _onClickDetail);
            }

            _m_iStationInfo = null;
            
            _m_wLevelProgress?.discard();
            _m_wLevelProgress = null;
        }
        
        protected override void _onShowWnd()
        {
            // 在窗口显示时添加消息监听
            WinMsg.RegisterMsgAct(WinMsgType.ON_TREASURE_HUNT_STATION_CHG, _onTreasureHuntStationUpdated);
            _refreshWnd();
        }
        
        protected override void _onHideWnd()
        {
            // 在窗口隐藏时移除消息监听
            WinMsg.UnregisterMsgAct(WinMsgType.ON_TREASURE_HUNT_STATION_CHG, _onTreasureHuntStationUpdated);
            
            // 同步隐藏进度条窗口
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
            if (wnd == null || !isShow || _m_iStationInfo == null)
                return;
            
            // 设置当前等级显示
            ALUGUICommon.setLabelTxt(wnd.txtCurLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, _m_iStationInfo.stationLevel));
            
            // 获取当前等级和下一等级的配表数据
            TreasureHuntStationLvlRefObj currentLevelRefObj = _m_iStationInfo.stationLvlRefObj;
            TreasureHuntStationLvlRefObj nextLevelRefObj = null;
            if (currentLevelRefObj != null)
            {
                nextLevelRefObj = GRefdataCoreMgr.instance.treasureHuntStationLvlRefCore.getRef(_m_iStationInfo.stationLevel + 1);
            }
            
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
                    // _m_wLevelProgress.setProgressTxt(TextTranslate.instance.getLanguage(TransKeyConst.common_max_none), true);
                }
            }
            
            // 刷新各种数值显示
            if (currentLevelRefObj != null)
            {
                // 设置助跑距离
                ALUGUICommon.setLabelTxt(wnd.txtAutoDistance, TextTranslate.instance.getLanguage(TransKeyConst.treasureHunt_autoDistance_value, currentLevelRefObj.auto_fly_distance));
                
                // 设置最大移动距离
                ALUGUICommon.setLabelTxt(wnd.txtMaxDistance, TextTranslate.instance.getLanguage(TransKeyConst.treasureHunt_maxDistance_value, currentLevelRefObj.max_fly_distance));
                
                // 设置保护时间
                ALUGUICommon.setLabelTxt(wnd.txtProtectTime, TextTranslate.instance.getLanguage(TransKeyConst.treasureHunt_protectTime_value, currentLevelRefObj.fly_protect_times));

                // 设置奖励数量
                TreasureHuntAreaDistanceRefObj distanceRef = GRefdataCoreMgr.instance.getTreasureHuntAreaDistanceRefObj(currentLevelRefObj.max_fly_distance);
                long rewardNum = 1;
                rewardNum += distanceRef?.capture_reward_num_add ?? 0;
                ALUGUICommon.setLabelTxt(wnd.txtRewardItem, TextTranslate.instance.getLanguage(TransKeyConst.treasureHunt_rewardNum_value, rewardNum));
            }
            
            // 处理满级显示/隐藏逻辑
            ALUGUICommon.setGameObjEnable(wnd.maxLevelShow, nextLevelRefObj != null);
            ALUGUICommon.setGameObjEnable(wnd.maxLevelHide, nextLevelRefObj == null);
        }
        
        /// <summary>
        /// TreasureHuntStationInfo更新消息处理
        /// </summary>
        private void _onTreasureHuntStationUpdated()
        {
            // 添加空值检查
            if (_m_iStationInfo == null)
                return;
            
            _refreshWnd();
        }
        
        /// <summary>
        /// 点击等级详情按钮的处理逻辑
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickDetail(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndTreasureHuntStationLevelDetail.instance, () =>
            {
                GGUIWndTreasureHuntStationLevelDetail.instance.setData(_m_iStationInfo);
                GGUIWndTreasureHuntStationLevelDetail.instance.showWnd();
            }, EUIQueueStageType.MAIN, UINodeTagConst.C_TREASURE_HUNT_STATION_LEVEL_DETAIL, false, false);
        }
    }
}
using System;
using System.Collections.Generic;
using ALPackage;
using Common.ActivityEnum;
using GOE;
using Hotfix.Common.TileMatchObj;
using Hotfix.GC2GS.p201_TileMatchOp;
using Hotfix.GS2GC.p201_TileMatchOp;
using Hotfix.TileMatchEnum;

namespace Hotfix
{
    public class TileMatchComponent : HotfixBaseComponent
    {
        private bool _m_bHasGetActivityInitData;//是否已经获取过活动初始化数据
        
        /// <summary>
        /// 阶段奖励信息
        /// </summary>
        private TileMatchStepRewardInfo _m_stepRewardInfo;

        /// <summary>
        /// 可领取的最小奖励阶段
        /// </summary>
        private int _m_iMinCanDrawRewardStep;

        /// <summary>
        /// 可领取的阶段奖励数量
        /// </summary>
        private int _m_iCanDrawStepRewardCount;

        /// <summary>
        /// 活动总积分
        /// </summary>
        private long _m_lActivityTotalScore;

        public bool hasGetActivityInitData => _m_bHasGetActivityInitData;
        
        public TileMatchStepRewardInfo stepRewardInfo => _m_stepRewardInfo;
        public int minCanDrawRewardStep => _m_iMinCanDrawRewardStep;
        public int canDrawStepRewardCount => _m_iCanDrawStepRewardCount;

        public long activityTotalScore => _m_lActivityTotalScore;
        
        protected override void _dealInit()
        {
            _getActivityInitData();
            
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, _onActivityStateChg);
            WinMsg.RegisterMsgAct(WinMsgType.ON_ACTIVITY_CRYSTAL_GIFT_PACK_REFRESH, _onActivityCrystalGiftPackRefresh);
            WinMsg.RegisterMsgAct(WinMsgType.ON_ACTIVITY_CRYSTAL_GIFT_PACK_BUY_RECORD_CHG, _onActivityCrystalGiftPackBuyRecordChg);
            ALMsgSys.RegisterMsg(HotfixMsgType.ON_TILEMATCH_CAN_DRAW_STEP_REWARD_INFO_CHG, _onCanDrawStepRewardInfoChg);
            WinMsg.RegisterMsg(WinMsgType.ON_LAZY_CD_CHG, _onLazyChg);
        }

        private void _getActivityInitData()
        {
            _ABaseActivityInfo activityInfo = NPPlayer.instance.commonActivityComp.getValidActivityInfoByActivityId(HotfixRefdataCoreMgr.instance.tileMatchOtherRefObj.tilematch_activity_id);
            if (activityInfo != null && activityInfo.isPlaying)
            {
                reqTileMatchInit((_msg) =>
                {
                    if (_msg != null)
                    {
                        _updateTileMatchInfo(_msg.getInfo());
                    }

                    _m_bHasGetActivityInitData = true;
                    
                    if(!isInitDone)
                        setInitDone();
                });
            }
            else
            {
                if(!isInitDone)
                    setInitDone();
            }
        }

        protected override void _onDiscard()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, _onActivityStateChg);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_ACTIVITY_CRYSTAL_GIFT_PACK_REFRESH, _onActivityCrystalGiftPackRefresh);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_ACTIVITY_CRYSTAL_GIFT_PACK_BUY_RECORD_CHG, _onActivityCrystalGiftPackBuyRecordChg);
            ALMsgSys.UnregisterMsg(HotfixMsgType.ON_TILEMATCH_CAN_DRAW_STEP_REWARD_INFO_CHG, _onCanDrawStepRewardInfoChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_LAZY_CD_CHG, _onLazyChg);

            _clearActivityInitData();
        }

        /// <summary>
        /// 清除活动初始化数据
        /// </summary>
        private void _clearActivityInitData()
        {
            _m_bHasGetActivityInitData = false;
            
            _m_stepRewardInfo = null;
        }
        
        protected override void _onInitDone()
        {
            //刷新所有红点
            _refreshRed();
        }

        protected override void _onInitFail()
        {
        }

        private void _updateTileMatchInfo(TileMatch_Info _tileMatchInfo)
        {
            if (_tileMatchInfo == null)
            {
                Debug.LogError("[TileMatchComponent _updateTileMatchInfo] tileMatchInfo is null");
                return;
            }
            
            _updateStepRewardInfo(_tileMatchInfo.getStepRewardInfo());
            _updateCanDrawStepRewardInfo(_tileMatchInfo.getCanDrawStepRewardList());
            _m_lActivityTotalScore = _tileMatchInfo.getTotalScore();
        }
        
        /// <summary>
        /// 更新当前所处阶段奖励阶段
        /// </summary>
        private void _updateStepRewardInfo(TileMatch_StepRewardInfo _serverStepRewardInfo)
        {
            if (_serverStepRewardInfo == null)
            {
                Debug.LogError($"[TileMatchComponent _updateStepRewardInfo] serverStepRewardInfo is null");
                return;
            }

            if (_m_stepRewardInfo == null)
                _m_stepRewardInfo = new TileMatchStepRewardInfo(_serverStepRewardInfo);
            else
                _m_stepRewardInfo.updateInfo(_serverStepRewardInfo);
            
            ALMsgSys.SendMsg(HotfixMsgType.ON_TILEMATCH_STEP_REWARD_INFO_CHG);
        }
        
        /// <summary>
        /// 更新可领取的阶段奖励信息
        /// </summary>
        private void _updateCanDrawStepRewardInfo(List<TileMatch_CanDrawStepReward> _canDrawStepRewardInfoList)
        {
            _m_iMinCanDrawRewardStep = 0;
            _m_iCanDrawStepRewardCount = 0;

            if (_canDrawStepRewardInfoList != null)
            {
                TileMatch_CanDrawStepReward canDrawStepRewardInfo = null;
                for (int i = 0, count = _canDrawStepRewardInfoList.Count; i < count; i++)
                {
                    canDrawStepRewardInfo = _canDrawStepRewardInfoList[i];
                    if(canDrawStepRewardInfo == null)
                        continue;

                    if (canDrawStepRewardInfo.getStep() < _m_iMinCanDrawRewardStep)
                        _m_iMinCanDrawRewardStep = canDrawStepRewardInfo.getStep();
                
                    _m_iCanDrawStepRewardCount += canDrawStepRewardInfo.getNum();
                }
            }
            
            ALMsgSys.SendMsg(HotfixMsgType.ON_TILEMATCH_CAN_DRAW_STEP_REWARD_INFO_CHG);
        }

        #region 红点

        /// <summary>
        /// 刷新红点
        /// </summary>
        private void _refreshRed()
        {
            _ABaseActivityInfo activityInfo = NPPlayer.instance.commonActivityComp.getValidActivityInfoByActivityId(HotfixRefdataCoreMgr.instance.tileMatchOtherRefObj.tilematch_activity_id);
            
            _refreshLazyCDRedTip(activityInfo);
            _refreshFreeBuyRedTip(activityInfo);
            _refreshStepRewardCanDrawRedTip(activityInfo);
        }
        
        /// <summary>
        /// 刷新三消体力红点(当前体力值达到体力上限的配置百分比时显示)
        /// </summary>
        private void _refreshLazyCDRedTip(_ABaseActivityInfo _activityInfo)
        {
            if(_activityInfo == null)
                _activityInfo = NPPlayer.instance.commonActivityComp.getValidActivityInfoByActivityId(HotfixRefdataCoreMgr.instance.tileMatchOtherRefObj.tilematch_activity_id);
            
            if (_activityInfo == null || !_activityInfo.isPlaying)
            {
                RedTipMgr.instance.setCountByRefRedTipId(HotfixRedTipConst.TILEMATCH_LAYZ_CD, 0);
                return;
            }
            
            long lazyCDId = HotfixRefdataCoreMgr.instance.tileMatchOtherRefObj.tilematch_lazy_cd_id;
            PlayerLazyCDInfo lazyCdInfo = NPPlayer.instance.lazyCdComp.getLazyCDInfo(lazyCDId);
            if (lazyCdInfo == null)
            {
                RedTipMgr.instance.setCountByRefRedTipId(HotfixRedTipConst.TILEMATCH_LAYZ_CD, 0);
                return;
            }

            float showRedTipPer = HotfixRefdataCoreMgr.instance.tileMatchOtherRefObj.tilematch_lazy_cd_red_tip_show_per / 100f;
            float nowLazyCdCountPer = 1f * lazyCdInfo.getCount() / lazyCdInfo.MaxCount;
            RedTipMgr.instance.setCountByRefRedTipId(HotfixRedTipConst.TILEMATCH_LAYZ_CD, nowLazyCdCountPer >= showRedTipPer ? 1 : 0);
        }
        
        /// <summary>
        /// 刷新三消礼包商店免费购买红点
        /// </summary>
        private void _refreshFreeBuyRedTip(_ABaseActivityInfo _activityInfo)
        {
            if(_activityInfo == null)
                _activityInfo = NPPlayer.instance.commonActivityComp.getValidActivityInfoByActivityId(HotfixRefdataCoreMgr.instance.tileMatchOtherRefObj.tilematch_activity_id);
            
            if (_activityInfo == null || !_activityInfo.isPlaying || _activityInfo.crystalGiftPackInfo == null)
            {
                RedTipMgr.instance.setCountByRefRedTipId(HotfixRedTipConst.TILEMATCH_FREE_BUY, 0);
                return;
            }
            
            RedTipMgr.instance.setCountByRefRedTipId(HotfixRedTipConst.TILEMATCH_FREE_BUY, _activityInfo.crystalGiftPackInfo.haveFreeGiftPackCanBuy() ? 1 : 0);
        }
        
        /// <summary>
        /// 刷新三消可领取阶段奖励红点
        /// </summary>
        private void _refreshStepRewardCanDrawRedTip(_ABaseActivityInfo _activityInfo)
        {
            if(_activityInfo == null)
                _activityInfo = NPPlayer.instance.commonActivityComp.getValidActivityInfoByActivityId(HotfixRefdataCoreMgr.instance.tileMatchOtherRefObj.tilematch_activity_id);
            
            if (_activityInfo == null || !_activityInfo.isPlaying)
            {
                RedTipMgr.instance.setCountByRefRedTipId(HotfixRedTipConst.TILEMATCH_STEP_REWARD_CAN_DRAW, 0);
                return;
            }
            
            RedTipMgr.instance.setCountByRefRedTipId(HotfixRedTipConst.TILEMATCH_STEP_REWARD_CAN_DRAW, _m_iCanDrawStepRewardCount);
        }

        #endregion
        
        #region S2C

        /// <summary>
        /// 三消逻辑处理
        /// </summary>
        /// <param name="_msg"></param>
        public void onTileMatchLogicProcess(GS2GC_201_051_OnTileMatchLogicProcess _msg)
        {
            if (_msg == null || _msg.getModeType() != HotfixAccountSettingMgr.instance.hotfixAccountSetting.getTileMatchModelType())
                return;

            long addScore = 0;
            for (int i = 0, count = _msg.getLogicList().Count; i < count; i++)
            {
                TileMatch_LogicInfo logicInfo = _msg.getLogicList()[i];
                if (logicInfo != null)
                    addScore += logicInfo.getScore();
            }
            
            // Debug.LogError($"===========[GS2GC_201_051_OnTileMatchLogicProcess] 增加积分:{addScore}");
            ALMsgSys.SendMsg(HotfixMsgType.GET_TILEMATCH_LOGIC_PROCESS, _msg.getLogicList());
        }

        /// <summary>
        /// 三消任务数据变更
        /// </summary>
        /// <param name="_msg"></param>
        public void onTileMatchTaskChg(GS2GC_201_052_OnTileMatchTaskChg _msg)
        {
            if (_msg == null || _msg.getModeType() != (int)HotfixAccountSettingMgr.instance.hotfixAccountSetting.getTileMatchModelType())
                return;
            
            ALMsgSys.SendMsg(HotfixMsgType.GET_TILEMATCH_TASK_CHG, _msg.getTaskInfo());
        }

        /// <summary>
        /// 三消阶段奖励数据变更
        /// </summary>
        /// <param name="_msg"></param>
        public void onTileMatchStepRewardChg(GS2GC_201_053_OnTileMatchStepRewardChg _msg)
        {
            if(_msg == null)
                return;
            
            _updateStepRewardInfo(_msg.getStepRewardInfo());
        }
        
        public void onTileMatchCanDrawStepRewardChg(GS2GC_201_054_OnTileMatchCanDrawStepRewardChg _msg)
        {
            if(_msg == null)
                return;
            
            _updateCanDrawStepRewardInfo(_msg.getCanDrawStepRewardList());
        }

        /// <summary>
        /// 三消总分数变更
        /// </summary>
        /// <param name="_msg"></param>
        public void onTileMatchTotalScoreChg(GS2GC_201_055_OnTileMatchTotalScoreChg _msg)
        {
            if(_msg == null)
                return;

            _m_lActivityTotalScore = _msg.getTotalScore();
            ALMsgSys.SendMsg(HotfixMsgType.ON_TILEMATCH_ACTIVITY_TOTAL_SCORE_CHG);
        }
        
        #endregion

        #region C2S

        /// <summary>
        /// 请求三消活动初始化数据
        /// </summary>
        public void reqTileMatchInit(Action<GS2GC_201_004_RetTileMatchInit> _reqDone)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_201_004_ReqTileMatchInit(),
                new HotfixCommonErrCodeRequestCallbackProtocolDealer<GS2GC_201_004_RetTileMatchInit>(_reqDone));
        }
        
        /// <summary>
        /// 请求棋盘数据初始化
        /// </summary>
        public void reqTileMatchBlockInit(Action<bool, GS2GC_201_001_RetTileMatchBlockInit> _reqDone)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_201_001_ReqTileMatchBlockInit(HotfixAccountSettingMgr.instance.hotfixAccountSetting.getTileMatchModelType()),
                new HotfixCommonRequestSucFailSameCallbackProtocolDealer<GS2GC_201_001_RetTileMatchBlockInit>(_reqDone));
        }

        /// <summary>
        /// 请求交换格子
        /// </summary>
        /// <param name="_gameMode"></param>
        /// <param name="_startIndex"></param>
        /// <param name="_endIndex"></param>
        /// <param name="_reqDone"></param>
        public void reqTileMatchSwitchItem(ETileMatch_ModeType _gameMode, int _startIndex, int _endIndex, Action<bool, GS2GC_201_002_RetTileMatchSwitch> _reqDone)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_201_002_ReqTileMatchSwitchItem(_gameMode, _startIndex, _endIndex),
                new HotfixCommonRequestSucFailSameCallbackProtocolDealer<GS2GC_201_002_RetTileMatchSwitch>(_reqDone));
        }

        /// <summary>
        /// 三消死局重开
        /// </summary>
        public void reqTileMatchGameOver(Action<bool, GS2GC_201_003_RetTileMatchGameOver> _reqDone)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_201_003_ReqTileMatchGameOver(HotfixAccountSettingMgr.instance.hotfixAccountSetting.getTileMatchModelType()),
                new HotfixCommonRequestSucFailSameCallbackProtocolDealer<GS2GC_201_003_RetTileMatchGameOver>(_reqDone));
        }

        /// <summary>
        /// 请求领取阶段奖励
        /// </summary>
        /// <param name="_reqDone"></param>
        public void reqTileMatchDrawStepReward(Action<GS2GC_201_005_RetTileMatchDrawStepReward> _reqDone)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_201_005_ReqTileMatchDrawStepReward(),
                new HotfixCommonErrCodeRequestCallbackProtocolDealer<GS2GC_201_005_RetTileMatchDrawStepReward>(_reqDone));
        }
        
        #endregion
        
        #region 消息事件

        //活动状态变更
        private void _onActivityStateChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length < 4)
                return;

            long activityId = (long)_objects[0];
            if (activityId != HotfixRefdataCoreMgr.instance.tileMatchOtherRefObj.tilematch_activity_id)
                return;

            EActivityState nowActivityState = (EActivityState)_objects[3];
            if (nowActivityState == EActivityState.PLAYING)//若活动开启, 请求活动初始化数据
            {
                _getActivityInitData();
            }
            else//其他活动状态, 清除活动初始化数据
            {
                _clearActivityInitData();
            }
            
            //刷新红点
            _refreshRed();
        }
        
        //可领取阶段奖励数据变更
        private void _onCanDrawStepRewardInfoChg(params object[] _objects)
        {
            _refreshStepRewardCanDrawRedTip(null);
        }

        /// <summary>
        /// 钻石礼包刷新消息监听
        /// </summary>
        private void _onActivityCrystalGiftPackRefresh()
        {
            _refreshFreeBuyRedTip(null);
        }

        /// <summary>
        /// 钻石礼包购买次数变化
        /// </summary>
        private void _onActivityCrystalGiftPackBuyRecordChg()
        {
            _refreshFreeBuyRedTip(null);
        }

        private void _onLazyChg(params object[] _objects)
        {
            if(_objects == null || _objects.Length < 1 || !(_objects[0] is long _lazyCdId) || _lazyCdId != HotfixRefdataCoreMgr.instance.tileMatchOtherRefObj.tilematch_lazy_cd_id)
                return;
            
            _refreshLazyCDRedTip(null);
        }
        
        #endregion
    }
}
using System;
using System.Collections.Generic;
using ALPackage;
using Common.ActivityEnum;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public abstract class _AGGUIWndStepTaskRushRankActivity<T_Mono> : _ATALBasicLoadPrefabSubUIWnd<T_Mono> where T_Mono : _AGGUIMonoStepTaskRushRankActivity
    {
        // 窗口对象的资源加载路径
        private string _m_sAssetPath;
        // 窗口对象的资源名字
        private string _m_sObjName;
        
        // 活动实例ID
        protected long _m_lActivityId;
        // 活动信息
        protected _ABaseActivityInfo _m_iActivityInfo;
        // 冲榜信息
        protected ActivityRankRushInfo _m_rankRushInfo;
        // 阶段奖励信息
        protected List<ActivityStepRewardInfo> _m_lStepRewardInfoList;
        
        // 当前选中的页签类型
        protected EStepTaskRushRankActivityViewType _m_eCurrentViewType = EStepTaskRushRankActivityViewType.STEP_REWARD;
        
        // 阶段奖励Tab
        private NPGGUIWndCommonTab _m_wStepRewardTab;
        // 任务列表Tab
        private NPGGUIWndCommonTab _m_wTaskListTab;
        
        // 阶段奖励Grid窗口
        private GGUIWndActivityStepRewardAllStepGrid _m_wStepRewardItemGrid;
        // 任务列表Grid窗口
        private GGUIWndStepTaskRushRankActivityTaskGrid _m_wTaskItemGrid;
        
        // 请求排行信息序列号
        private long _m_lGetRankInfoSerialize;

        public _AGGUIWndStepTaskRushRankActivity(long _activityId, NPCommonAssetPathInfo _assetPathInfo, Transform _parent) : base(_parent)
        {
            _m_lActivityId = _activityId;
            if (_assetPathInfo != null)
            {
                _m_sAssetPath = _assetPathInfo.asset_path;
                _m_sObjName = _assetPathInfo.obj_name;
            }
        }

        protected override string _monoAssetPath { get { return _m_sAssetPath; } }
        protected override string _monoObjName { get { return _m_sObjName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            // 注册消息监听
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, _onActivityStateChg);
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STEP_REWARD_SCORE_CHG, _onStepRewardScoreChg);
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STEP_REWARD_GET, _onStepRewardGet);
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STEP_REWARD_ONE_KEY_GET, _onStepRewardGet);
            WinMsg.RegisterMsg(WinMsgType.SIMULATE_SWITCH_STEP_TASK_RUSH_RANK_TAB, _onSimulateSwitchTab);

            _refreshWnd(true);
        }

        protected override void _onHideWnd()
        {
            _m_wStepRewardItemGrid?.hideWnd();
            _m_wTaskItemGrid?.hideWnd();

            // 解除消息监听
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, _onActivityStateChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STEP_REWARD_SCORE_CHG, _onStepRewardScoreChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STEP_REWARD_GET, _onStepRewardGet);
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STEP_REWARD_ONE_KEY_GET, _onStepRewardGet);
            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_SWITCH_STEP_TASK_RUSH_RANK_TAB, _onSimulateSwitchTab);

            _m_lGetRankInfoSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onReset()
        {
            _m_wStepRewardItemGrid?.resetWnd();
            _m_wTaskItemGrid?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wStepRewardTab?.discard();
            _m_wStepRewardTab = null;
            _m_wTaskListTab?.discard();
            _m_wTaskListTab = null;
            
            _m_wStepRewardItemGrid?.discard();
            _m_wStepRewardItemGrid = null;
            _m_wTaskItemGrid?.discard();
            _m_wTaskItemGrid = null;

            _m_iActivityInfo = null;
            _m_rankRushInfo = null;
            _m_lStepRewardInfoList?.Clear();
            _m_lStepRewardInfoList = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            // 构建阶段奖励Tab
            if (wnd.monoStepRewardTab != null)
            {
                _m_wStepRewardTab = new NPGGUIWndCommonTab(wnd.monoStepRewardTab);
                _m_wStepRewardTab.clickDelegate += _onClickStepRewardTab;
            }

            // 构建任务列表Tab
            if (wnd.monoTaskListTab != null)
            {
                _m_wTaskListTab = new NPGGUIWndCommonTab(wnd.monoTaskListTab);
                _m_wTaskListTab.clickDelegate += _onClickTaskListTab;
            }

            // 构建阶段奖励Grid
            if (wnd.monoStepRewardItemGrid != null)
            {
                _m_wStepRewardItemGrid = new GGUIWndActivityStepRewardAllStepGrid(wnd.monoStepRewardItemGrid);
            }

            // 构建任务列表Grid
            if (wnd.monoTaskItemGrid != null)
            {
                _m_wTaskItemGrid = new GGUIWndStepTaskRushRankActivityTaskGrid(wnd.monoTaskItemGrid);
            }
        }

        private void _updateActivityInfo()
        {
            _m_iActivityInfo = NPPlayer.instance.commonActivityComp.getValidActivityInfoByActivityId(_m_lActivityId);
            _m_rankRushInfo = _m_iActivityInfo?.rankRushInfoList?.SafeGet(0);
            if(_m_lStepRewardInfoList == null)
                _m_lStepRewardInfoList = new List<ActivityStepRewardInfo>();
            _m_lStepRewardInfoList.Clear();
            if(_m_iActivityInfo != null && _m_iActivityInfo.stepRewardInfoList != null)
                _m_lStepRewardInfoList.AddRange(_m_iActivityInfo.stepRewardInfoList);
        }

        /// <summary>
        /// 刷新窗口
        /// </summary>
        protected virtual void _refreshWnd(bool _resetInfo)
        {
            if(!isShow)
                return;
            
            if (_resetInfo)
                _updateActivityInfo();

            _refreshActivityStateShow();
            _refreshSelfRankAndScore();
            _refreshTabState();
            _refreshCurrentView(_resetInfo);
        }

        private void _refreshActivityStateShow()
        {
            if(wnd == null || !isShow || _m_iActivityInfo == null)
                return;
            
            NPCommonEnumStatMutexShowInfo<EActivityState>.setStat(wnd.activityStateShowInfo, _m_iActivityInfo.activityState);
        }
        
        /// <summary>
        /// 刷新页签状态
        /// </summary>
        private void _refreshTabState()
        {
            if(!isShow)
                return;
            
            _m_wStepRewardTab?.setSelected(_m_eCurrentViewType == EStepTaskRushRankActivityViewType.STEP_REWARD);
            _m_wTaskListTab?.setSelected(_m_eCurrentViewType == EStepTaskRushRankActivityViewType.TASK_LIST);
        }

        /// <summary>
        /// 刷新当前视图
        /// </summary>
        /// <param name="_resetInfo">是否重置数据</param>
        private void _refreshCurrentView(bool _resetInfo)
        {
            if(!isShow)
                return;
            
            switch (_m_eCurrentViewType)
            {
                case EStepTaskRushRankActivityViewType.STEP_REWARD:
                    _refreshStepRewardGrid(_resetInfo);
                    _m_wTaskItemGrid?.hideWnd();
                    break;
                case EStepTaskRushRankActivityViewType.TASK_LIST:
                    _refreshTaskItemGrid(_resetInfo);
                    _m_wStepRewardItemGrid?.hideWnd();
                    break;
            }
        }

        /// <summary>
        /// 切换页签
        /// </summary>
        /// <param name="_viewType">目标视图类型</param>
        private void _switchTab(EStepTaskRushRankActivityViewType _viewType)
        {
            if (_m_eCurrentViewType == _viewType)
                return;

            _m_eCurrentViewType = _viewType;
            _refreshTabState();
            _refreshCurrentView(true);
        }

        /// <summary>
        /// 刷新自己的排名和积分信息（参考GGUIWndRankRushDetail._refreshSelfInfo）
        /// </summary>
        private void _refreshSelfRankAndScore()
        {
            if (wnd == null || !isShow)
                return;

            // 设置默认显示状态
            string txtMyRankKey = string.IsNullOrEmpty(wnd.txtMyRankKey) ? TransKeyConst.rankRush_myRank_str : wnd.txtMyRankKey;
            ALUGUICommon.setLabelTxt(wnd.txtMyRank, TextTranslate.instance.getLanguage(txtMyRankKey, ""));
            
            string txtMyScoreKey = string.IsNullOrEmpty(wnd.txtMyScoreKey) ? TransKeyConst.rankRush_myScore_str_num : wnd.txtMyScoreKey;
            ALUGUICommon.setLabelTxt(wnd.txtMyScore, TextTranslate.instance.getLanguage(txtMyScoreKey, "", ""));

            // 检查数据有效性
            if (_m_rankRushInfo == null || _m_rankRushInfo.activityInfo == null || _m_rankRushInfo.activityRankRushRefObj == null || _m_rankRushInfo.rankRefObj == null)
                return;

            long serialize = _m_lGetRankInfoSerialize = ALSerializeOpMgr.next();

            // 根据活动状态请求排名信息
            switch (_m_rankRushInfo.activityInfo.activityState)
            {
                case EActivityState.PLAYING:
                case EActivityState.SETTLING:
                    _getRankInfo((_rank, _score) =>
                    {
                        // 回调校验：窗口已关闭或序列号不匹配则忽略
                        if (wnd == null || !isShow || _m_lGetRankInfoSerialize != serialize)
                            return;

                        _setSelfShowInfo(_rank, _score);
                    });
                    break;
                case EActivityState.REWARDING:
                    // 领奖期获取结算信息
                    _m_rankRushInfo.getSettleInfo(_settleInfo =>
                    {
                        if (wnd == null || !isShow || _m_lGetRankInfoSerialize != serialize)
                            return;

                        if(_settleInfo != null)
                            _setSelfShowInfo(_settleInfo.getRank(), _settleInfo.getScore());
                        else
                            _setSelfShowInfo(0, 0);
                    }, _m_rankRushInfo.isGuildRankRush);
                    break;
            }
        }

        /// <summary>
        /// 获取排行信息
        /// </summary>
        /// <param name="_onGetRankInfo">第一个参数: 排行, 第二个参数分数</param>
        protected abstract void _getRankInfo(Action<long, long> _onGetRankInfo);
        
        /// <summary>
        /// 设置自己的显示信息（参考GGUIWndRankRushDetail._setSelfShowInfo）
        /// </summary>
        /// <param name="_selfRank">排名</param>
        /// <param name="_score">积分</param>
        private void _setSelfShowInfo(long _selfRank, long _score)
        {
            if (wnd == null || _m_rankRushInfo == null || _m_rankRushInfo.rankRefObj == null)
                return;

            string txtMyRankKey = string.IsNullOrEmpty(wnd.txtMyRankKey) ? TransKeyConst.rankRush_myRank_str : wnd.txtMyRankKey;
            // 设置排名显示
            if (_selfRank > 0)
            {
                ALUGUICommon.setLabelTxt(wnd.txtMyRank, TextTranslate.instance.getLanguage(txtMyRankKey, _selfRank));
            }
            else
            {
                ALUGUICommon.setLabelTxt(wnd.txtMyRank, TextTranslate.instance.getLanguage(txtMyRankKey, TransKeyConst.rankRush_notOnTheRankingList_none));
            }

            string txtMyScoreKey = string.IsNullOrEmpty(wnd.txtMyScoreKey) ? TransKeyConst.rankRush_myScore_str_num : wnd.txtMyScoreKey;
            // 设置分数显示
            string scoreStr = GCommon.getValueFormatStr(_m_rankRushInfo.rankRefObj.process_num_format, _score);
            ALUGUICommon.setLabelTxt(wnd.txtMyScore, TextTranslate.instance.getLanguage(txtMyScoreKey, _m_rankRushInfo.rankRefObj.score_name, scoreStr));
        }

        /// <summary>
        /// 刷新阶段奖励列表
        /// </summary>
        /// <param name="_resetInfo">是否重置数据</param>
        private void _refreshStepRewardGrid(bool _resetInfo)
        {
            if (wnd == null || _m_wStepRewardItemGrid == null || !isShow)
                return;

            _m_wStepRewardItemGrid.showWnd();
            if (_resetInfo)
            {
                _m_wStepRewardItemGrid.setInfo(_m_lStepRewardInfoList);
            }
            else
            {
                _m_wStepRewardItemGrid.forceRefreshAllItem();
            }
        }

        /// <summary>
        /// 刷新任务列表
        /// </summary>
        /// <param name="_resetInfo">是否重置数据</param>
        private void _refreshTaskItemGrid(bool _resetInfo)
        {
            if (wnd == null || _m_wTaskItemGrid == null || !isShow)
                return;

            _m_wTaskItemGrid.showWnd();
            if (_resetInfo)
            {
                _m_wTaskItemGrid.setInfo(_m_lStepRewardInfoList);
            }
            else
            {
                _m_wTaskItemGrid.forceRefreshAllItem();
            }
        }


        #region 页签点击回调

        /// <summary>
        /// 点击阶段奖励Tab
        /// </summary>
        /// <param name="_isSelected">是否选中</param>
        private void _onClickStepRewardTab(bool _isSelected)
        {
            _switchTab(EStepTaskRushRankActivityViewType.STEP_REWARD);
        }

        /// <summary>
        /// 点击任务列表Tab
        /// </summary>
        /// <param name="_isSelected">是否选中</param>
        private void _onClickTaskListTab(bool _isSelected)
        {
            _switchTab(EStepTaskRushRankActivityViewType.TASK_LIST);
        }

        #endregion


        #region 消息监听回调

        /// <summary>
        /// 活动状态变化回调
        /// 参数：无
        /// </summary>
        private void _onActivityStateChg(params object[] _objs)
        {
            // 活动状态变化时, 刷新整个界面
            _refreshWnd(true);
        }

        /// <summary>
        /// 阶段奖励积分变化回调
        /// 参数：无
        /// </summary>
        private void _onStepRewardScoreChg(params object[] _objs)
        {
            _refreshCurrentView(false);
            _refreshSelfRankAndScore();//阶段积分变化可能会导致排名变化
        }

        /// <summary>
        /// 阶段奖励领取回调
        /// 参数：无
        /// </summary>
        private void _onStepRewardGet(params object[] _objs)
        {
            _refreshCurrentView(false);
        }

        /// <summary>
        /// 模拟切换页签回调
        /// 参数：_objs[0] 为 EStepTaskRushRankActivityViewType 目标页签类型
        /// </summary>
        private void _onSimulateSwitchTab(params object[] _objs)
        {
            if (_objs == null || _objs.Length < 1 || !(_objs[0] is EStepTaskRushRankActivityViewType _viewType))
                return;

            _switchTab(_viewType);
        }

        #endregion
    }
}
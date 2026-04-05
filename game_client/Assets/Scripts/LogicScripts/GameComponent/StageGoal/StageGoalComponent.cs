using System;
using System.Collections.Generic;
using ALPackage;
using Common.StageGoalObj;
using GS2GC.p002_InitOp;
using GS2GC.p007_CommOp;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

namespace GOE
{
    //阶段目标管理器
    public class StageGoalComponent : _ANPBasicPlayerComponent
    {
        //当前阶段id
        private long _m_curStageStepId;
        //当前大阶段配置
        private StageGoalBigStepRefObj _m_bigStepRefObj;
        //当前已经展示了建筑建造效果的大阶段 id
        private long _m_showBuildBigStepId;
        //阶段配表数据
        private StageGoalRefObj _m_stageRefObj;
        //已经领取的大阶段目标id列表
        [NotNull] private HashSet<long> _m_hasGetRewardBigGoalItemList = new HashSet<long>();
        //阶段子目标列表
        [NotNull] private List<StageGoalTaskItem> _m_taskItemList = new List<StageGoalTaskItem>();
        //所有阶段是否完成
        private bool _m_isAllDone;
        //定时任务
        private ALCommonEnableTaskController _m_iTickTask;

        #region ========大阶段首达信息========

        //已领取的大阶段首达奖励列表
        [NotNull] private HashSet<long> _m_hadDrawBigStepFirstReachList = new HashSet<long>();
        //可领取的大阶段首达奖励列表
        [NotNull] private HashSet<long> _m_canDrawBigStepFirstReachList = new HashSet<long>();
        //大阶段首达玩家信息列表
        [NotNull] private List<StageGoalFirstReachInfo> _m_lFirstReachInfoList = new List<StageGoalFirstReachInfo>();
        //到达指定阶段赚速第一玩家信息
        private StageGoalTopPlayerInfo _m_topPlayerInfo;

        #endregion

        public StageGoalComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
        }

        public override bool isMustInit { get { return true; } }

        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.STAGE_GOAL; } }

        public override ENPPlayerCompType[] dependCompList { get { return null; } }
        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return true; } }
        /// <summary>
        /// 当前阶段id
        /// </summary>
        public long curStageStepId { get { return _m_curStageStepId; } }
        /// <summary>
        /// 当前大阶段配置
        /// </summary>
        public StageGoalBigStepRefObj bigStepRefObj { get { return _m_bigStepRefObj; } }
        /// <summary>
        /// 阶段配表数据
        /// </summary>
        public StageGoalRefObj stageRefObj { get { return _m_stageRefObj; } }
        /// <summary>
        /// 所有阶段是否完成
        /// </summary>
        public bool isAllDone { get { return _m_isAllDone; } }
        /// <summary>
        /// 大阶段首达玩家信息列表
        /// </summary>
        public List<StageGoalFirstReachInfo> firstReachInfoList { get { return _m_lFirstReachInfoList; } }
        /// <summary>
        /// 到达指定阶段赚速第一玩家信息
        /// </summary>
        public StageGoalTopPlayerInfo topPlayerInfo { get { return _m_topPlayerInfo; } }

        /// <summary>
        /// 发送初始化协议提前申请内容
        /// </summary>
        public override void presendInitProtocol()
        {
            _reqStageGoalInit();
        }

        protected override void _dealInit()
        {
        }

        //组件加载完成时的调用
        protected override void _onInitDone()
        {
            WinMsg.RegisterMsgAct(WinMsgType.ON_NODE_CHG, _onNodeChg);
            WinMsg.RegisterMsgAct(WinMsgType.CUSTOM_RELOAD, _onNodeChg);
        }

        //组件初始化失败的处理
        protected override void _onInitFail()
        {
            ALLog.Error("StageGoalComponent init Fail!!!");
        }
        //释放资源函数
        protected override void _discard()
        {
            clear();
            _m_hasGetRewardBigGoalItemList.Clear();
            _m_hadDrawBigStepFirstReachList.Clear();
            _m_canDrawBigStepFirstReachList.Clear();
            _m_lFirstReachInfoList.Clear();
            _m_topPlayerInfo = null;
            _m_iTickTask.setDisable();
            WinMsg.UnregisterMsgAct(WinMsgType.ON_NODE_CHG, _onNodeChg);
            WinMsg.UnregisterMsgAct(WinMsgType.CUSTOM_RELOAD, _onNodeChg);
        }

        //所有组件初始化完以后
        public override void onAllCompInited()
        {
            //刷新红点
            refreshRed();
        }



        //数据清空
        public void clear()
        {
            StageGoalTaskItem temp = null;
            for (int i = 0; i < _m_taskItemList.Count; i++)
            {
                temp = _m_taskItemList[i];
                if (null == temp)
                    continue;

                temp.discard();
            }
            _m_taskItemList.Clear();
        }

        /// <summary>
        /// 刷新红点
        /// </summary>
        public void refreshRed()
        {
            refreshCurStageGoalRedTip();
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_STAGE_GOAL_BIG_STEP, bigGoalCanGetReward() ? 1 : 0);
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_STAGE_GOAL_PEAK, _m_canDrawBigStepFirstReachList.Count > 0 ? 1 : 0);

            //刷新阶段目标子任务红点
            long taskRedCount = 0;
            for (int i = 0; i < _m_taskItemList.Count; i++)
            {
                if(_m_taskItemList[i] != null && !_m_taskItemList[i].isReadRedTip && _m_taskItemList[i].isCompleted())
                    taskRedCount++;
            }
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_STAGE_GOAL_NOW_TASK, taskRedCount);
        }

        /// <summary>
        /// 刷新当前阶段目标奖励红点
        /// </summary>
        public void refreshCurStageGoalRedTip()
        {
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_STAGE_GOAL_NOW, subStepCanGetReward() ? 1 : 0);
        }

        /// <summary>
        /// 获取子任务列表
        /// </summary>
        public void getSubStageGoalTaskList(List<StageGoalTaskItem> _list)
        {
            if (null == _list)
                return;

            _list.AddRange(_m_taskItemList);
        }
        
        /// <summary>
        /// 获取可用的阶段任务数量（已解锁数量）
        /// </summary>
        /// <returns></returns>
        [Pure]
        public int getSubStageGoalAvailableTaskCount()
        {
            int count = 0;
            foreach (StageGoalTaskItem taskItem in _m_taskItemList)
            {
                //锁定的任务跳过
                if (taskItem == null || !taskItem.isUnlock)
                    continue;
                count++;
            }
            return count;
        }

        [Pure]
        public int getSubStageGoalCompleteCount()
        {
            int result = 0;
            foreach (StageGoalTaskItem taskItem in _m_taskItemList)
            {
                //锁定的任务跳过
                if (taskItem == null || !taskItem.isUnlock)
                    continue;
                
                if (taskItem.isCompleted())
                    result += 1;
            }

            return result;
        }

        [Pure]
        public float getStageGoalBigProgress()
        {
            if (null == _m_bigStepRefObj)
                return 0;

            float smallStepCount = _m_bigStepRefObj.end_small_step - _m_bigStepRefObj.begins_from_small_step + 1;
            float smallStepProcess = (_m_curStageStepId - _m_bigStepRefObj.begins_from_small_step) / smallStepCount;
            float subTaskProcess = getSubStageGoalCompleteCount() / (float)(getSubStageGoalAvailableTaskCount());
            
            return (float)Math.Round(smallStepProcess + (smallStepCount > 0 ? subTaskProcess / smallStepCount : 0), 2);
        }

        [Pure]
        public int getSubStageGoalTaskProgressPercent()
        {
            int subTaskProgressPercent = getSubStageGoalCompleteCount() * 100 / getSubStageGoalAvailableTaskCount();
            return subTaskProgressPercent;
        }

        /// <summary>
        /// 获取大阶段首达奖励状态
        /// </summary>
        /// <param name="_bigStepId"></param>
        /// <returns></returns>
        public ECommonRewardType getFirstReachBigStepRewardType(long _bigStepId)
        {
            if (_m_hadDrawBigStepFirstReachList.Contains(_bigStepId))
                return ECommonRewardType.HAS_GET_REWARD;

            if (_m_canDrawBigStepFirstReachList.Contains(_bigStepId))
                return ECommonRewardType.CAN_GET_REWARD;

            return ECommonRewardType.NOT_GET_REWARD;
        }

        /// <summary>
        /// 是否要展示建筑建造过程
        /// </summary>
        /// <remarks>
        /// 如果需要展示建筑建造过程，返回 true 的同时，内部会记录为已经展示过了，下次调用就会返回 false ，除非大阶段又变化了
        /// </remarks>
        public bool needShowBigStepBuildingBuild(bool _peak = false)
        {
            bool result = _m_showBuildBigStepId < _m_bigStepRefObj?.big_step;
            if (result && !_peak)
                _m_showBuildBigStepId = _m_bigStepRefObj?.big_step ?? 0;

            return result;
        }

        private void _updateStageGoal(StageGoal_Info _stageGoalInfo)
        {
            if (null == _stageGoalInfo)
                return;

            clear();
            _m_curStageStepId = _stageGoalInfo.getStep();
            _m_stageRefObj = GRefdataCoreMgr.instance.stageGoalRefCore.getRef(_m_curStageStepId);
            _m_bigStepRefObj = GRefdataCoreMgr.instance.getStageGoalBigStepRefObj((int)_m_curStageStepId);
            List<StageGoalTask_Info> list = _stageGoalInfo.getTaskList();
            if (null != list)
            {
                StageGoalTask_Info temp = null;
                for (int i = 0; i < list.Count; i++)
                {
                    temp = list[i];
                    if (null == temp)
                        continue;

                    StageGoalTaskItem taskItem = new StageGoalTaskItem(temp);
                    _m_taskItemList.Add(taskItem);
                }
            }

            _m_isAllDone = _stageGoalInfo.getIsDone();
            WinMsg.SendMsg(WinMsgType.ON_STAGE_GOAL_CHG);
        }
        private void _updateBigGoalRewardItemList(List<long> _hasGetRewardList)
        {
            _m_hasGetRewardBigGoalItemList.Clear();
            if (null == _hasGetRewardList || _hasGetRewardList.Count == 0)
                return;

            foreach (long id in _hasGetRewardList)
            {
                _m_hasGetRewardBigGoalItemList.Add(id);
            }
        }

        /// <summary>
        /// 更新已领取的大阶段首达奖励列表
        /// </summary>
        /// <param name="_hadDrawBigStepFirstReachList"></param>
        private void _updateHadDrawBigStepFirstReachList(List<long> _hadDrawBigStepFirstReachList)
        {
            _m_hadDrawBigStepFirstReachList.Clear();
            if (null == _hadDrawBigStepFirstReachList || _hadDrawBigStepFirstReachList.Count == 0)
                return;

            foreach (long id in _hadDrawBigStepFirstReachList)
            {
                _m_hadDrawBigStepFirstReachList.Add(id);
            }
        }

        /// <summary>
        /// 更新可领取的大阶段首达奖励列表
        /// </summary>
        /// <param name="_canDrawBigStepFirstReachList"></param>
        private void _updateCanDrawBigStepFirstReachList(List<long> _canDrawBigStepFirstReachList)
        {
            _m_canDrawBigStepFirstReachList.Clear();
            if (null == _canDrawBigStepFirstReachList || _canDrawBigStepFirstReachList.Count == 0)
                return;

            foreach (long id in _canDrawBigStepFirstReachList)
            {
                _m_canDrawBigStepFirstReachList.Add(id);
            }

            // 检查已领取列表，删除已领取的id
            foreach (long hadDrawId in _m_hadDrawBigStepFirstReachList)
            {
                if (_m_canDrawBigStepFirstReachList.Contains(hadDrawId))
                    _m_canDrawBigStepFirstReachList.Remove(hadDrawId);
            }
        }

        //主任务是否可以领取奖励
        public bool subStepCanGetReward()
        {
            if (isAllDone)
                return false;

            //如果当前阶段配置了需要服务器开服天数才能领取奖励，并且当前服务器开服天数不满足条件，则不能领取奖励
            if (_m_stageRefObj != null && 
                _m_stageRefObj.next_step_need_server_start_day > 0 && 
                _m_stageRefObj.next_step_need_server_start_day > NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.SERVER_START_DAYS))
                return false;

            //如果配置了simpleUnlock，并且没有解锁，则不能领取奖励
            if (_m_stageRefObj != null && _m_stageRefObj.next_step_simple_unlock_id > 0 && !GCommon.isSimpleUnlock(_m_stageRefObj.next_step_simple_unlock_id))
                return false;

            StageGoalTaskItem temp = null;
            for (int i = 0; i < _m_taskItemList.Count; i++)
            {
                temp = _m_taskItemList[i];
                //锁定的任务不需要检查
                if (null == temp || !temp.isUnlock)
                    continue;

                if (!temp.isCompleted())
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 每个子任务是否有可以领取奖励的
        /// </summary>
        /// <returns></returns>
        public bool subStepTaskCanGetRewrad()
        {
            StageGoalTaskItem temp = null;
            for (int i = 0; i < _m_taskItemList.Count; i++)
            {
                temp = _m_taskItemList[i];
                //锁定的任务不需要检查
                if (null == temp || !temp.isUnlock)
                    continue;

                if (temp.getCurRewardType() == EStageGoalTaskItemState.CAN_GET)
                    return true;
            }
            return false;
        }
        
        public bool bigGoalCanGetReward()
        {
            if (_m_bigStepRefObj == null)
                return false;

            //如果没有配置奖励，则当做已领取奖励
            if (_m_bigStepRefObj.reward_item_list == null || _m_bigStepRefObj.reward_item_list.Count <= 0)
                return false;

            int bigStepCheck = _m_bigStepRefObj.big_step;
            for (int i = 1; i <= bigStepCheck; i++)
            {
                if (_m_bigStepRefObj.big_step == i)
                {
                    //如果是当前大阶段，不需要先领小阶段奖励并且已经完成了所有小阶段任务
                    if (!_m_hasGetRewardBigGoalItemList.Contains(i) && !_m_bigStepRefObj.done_need_draw_all_step_reward && curIsLastSmallStepOfCurBigStep() && subStepCanGetReward())
                        return true;
                }
                else
                {
                    if (!_m_hasGetRewardBigGoalItemList.Contains(i))
                        return true;
                }
            }

            return false;
        }

        public bool bigGoalCanGetReward(long _bigStepId)
        {
            if (_bigStepId <= 0)
                return false;

            int bigStepCheck = _m_bigStepRefObj.big_step;
            if (_bigStepId > bigStepCheck)
                return false;

            //如果没有配置奖励，则当做已领取奖励
            StageGoalBigStepRefObj tempBigStepRefObj = GRefdataCoreMgr.instance.stageGoalBigStepRefCore.getRef(_bigStepId);
            if (tempBigStepRefObj != null && (tempBigStepRefObj.reward_item_list == null || tempBigStepRefObj.reward_item_list.Count <= 0))
                return false;

            if (_m_bigStepRefObj.big_step == _bigStepId)
            {
                //如果是当前大阶段并且是最后一个小阶段，不需要先领小阶段奖励并且已经完成了所有小阶段任务
                if (!_m_hasGetRewardBigGoalItemList.Contains(_bigStepId) && !_m_bigStepRefObj.done_need_draw_all_step_reward && curIsLastSmallStepOfCurBigStep() && subStepCanGetReward())
                    return true;
            }
            else
            {
                if (!_m_hasGetRewardBigGoalItemList.Contains(_bigStepId))
                    return true;
            }

            return false;
        }

        public ECommonRewardType bigStepRewardState(long _bigStepId)
        {
            if (_bigStepId <= 0)
                return ECommonRewardType.NONE;

            //如果没有配置奖励，则当做已领取奖励
            StageGoalBigStepRefObj tempBigStepRefObj = GRefdataCoreMgr.instance.stageGoalBigStepRefCore.getRef(_bigStepId);
            if (tempBigStepRefObj == null)
                return ECommonRewardType.NONE;
            else if(tempBigStepRefObj.reward_item_list == null || tempBigStepRefObj.reward_item_list.Count <= 0)
                return ECommonRewardType.HAS_GET_REWARD;

            int bigStepCheck = _m_bigStepRefObj.big_step;
            if (_bigStepId > bigStepCheck)
                return ECommonRewardType.NOT_GET_REWARD;
            
            if (_m_hasGetRewardBigGoalItemList.Contains(_bigStepId))
                return ECommonRewardType.HAS_GET_REWARD;

            if (_m_bigStepRefObj.big_step == _bigStepId)
            {
                //如果是当前大阶段并且是最后一个小阶段，不需要先领小阶段奖励并且已经完成了所有小阶段任务
                if (!_m_bigStepRefObj.done_need_draw_all_step_reward && curIsLastSmallStepOfCurBigStep() && subStepCanGetReward())
                    return ECommonRewardType.CAN_GET_REWARD;
                else
                    return ECommonRewardType.NOT_GET_REWARD;
            }
            else
                return ECommonRewardType.CAN_GET_REWARD;
        }

        /// <summary>
        /// 当前小阶段是否是当前大阶段的最后一个小阶段
        /// </summary>
        /// <returns></returns>
        public bool curIsLastSmallStepOfCurBigStep()
        {
            return _m_bigStepRefObj != null && _m_bigStepRefObj.end_small_step == _m_curStageStepId;
        }

        /// <summary>
        /// 当前小阶段是否是最后一个小阶段
        /// </summary>
        /// <param name="_stageGoalRef"></param>
        /// <returns></returns>
        public bool isLastSmallStep(StageGoalRefObj _stageGoalRef)
        {
            bool isLast = false;
            GRefdataCoreMgr.instance.stageGoalBigStepRefCore.dealAllRef(_bigStepRef =>
            {
                if (_stageGoalRef != null && _bigStepRef != null && _bigStepRef.end_small_step == _stageGoalRef.step)
                    isLast = true;
            });

            return isLast;
        }

        /// <summary>
        /// 获取有人到达的最后一个大阶段id
        /// </summary>
        /// <returns></returns>
        public long getLatestHadSomeoneReachBigStepId()
        {
            long latestId = 0;
            foreach (long bigStepId in _m_hadDrawBigStepFirstReachList)
            {
                if (bigStepId > latestId)
                    latestId = bigStepId;
            }

            foreach (long bigStepId in _m_canDrawBigStepFirstReachList)
            {
                if (bigStepId > latestId)
                    latestId = bigStepId;
            }
            return latestId;
        }

        //节点变化，刷新当前阶段奖励红点
        private void _onNodeChg()
        {
            if (_m_stageRefObj == null || _m_stageRefObj.next_step_simple_unlock_id <= 0)
                return;

            refreshCurStageGoalRedTip();
        }

        #region 检查开服天数

        /// <summary>
        /// 检查是否需要开启定时任务
        /// </summary>
        private void _checkNeedStartTickTask()
        {
            _m_iTickTask.setDisable();
            if (_m_stageRefObj == null)
                return;

            //当前阶段配置了需要服务器开服天数才能领取奖励，并且当前服务器开服天数不满足条件，则开启定时任务
            if (_m_stageRefObj.next_step_need_server_start_day > 0 && _m_stageRefObj.next_step_need_server_start_day > NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.SERVER_START_DAYS))
                _m_iTickTask = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_tickRefreshRedTip, 1.0f);
        }

        /// <summary>
        /// 检查刷新红点
        /// </summary>
        private void _tickRefreshRedTip()
        {
            refreshCurStageGoalRedTip();
        }

        #endregion

        #region S2C

        /// <summary>
        /// 初始化阶段目标
        /// </summary>
        /// <param name="_stageInfo"></param>
        public void retStageGoalInit(GS2GC_002_062_RetStageGoalInit _stageInfo)
        {
            if (null == _stageInfo)
            {
                setInitDone();
                return;
            }

            _updateStageGoal(_stageInfo.getCurStageGoal());
            _updateBigGoalRewardItemList(_stageInfo.getHadDrawBigStepList());
            _updateHadDrawBigStepFirstReachList(_stageInfo.getHadDrawBigStepFirstReachList());
            _updateCanDrawBigStepFirstReachList(_stageInfo.getCanDrawBigStepFirstReachList());
            _m_showBuildBigStepId = _m_bigStepRefObj?.big_step ?? 0;
            //检查是否需要开启定时任务
            _checkNeedStartTickTask();
            setInitDone();
        }

        /// <summary>
        /// 阶段目标变动
        /// </summary>
        /// <param name="_info"></param>
        public void onStageGoalChg(StageGoal_Info _info)
        {
            if (_info == null)
                return;

            //如果阶段变化，清空记录的已读任务完成红点
            if (_m_curStageStepId != _info.getStep())
                AccountSettingMgr.instance.accountSetting.clearStageTaskReadRedTipRecord();

            //更新数据
            _updateStageGoal(_info);
            //刷新红点
            refreshRed();
            //检查是否需要开启定时任务
            _checkNeedStartTickTask();
        }

        /// <summary>
        /// 阶段目标任务变动
        /// </summary>
        /// <param name="_taskInfo"></param>
        public void onStageGoalTaskChg(StageGoalTask_Info _taskInfo)
        {
            if (null == _taskInfo)
                return;

            for (int i = 0; i < _m_taskItemList.Count; i++)
            {
                StageGoalTaskItem item = _m_taskItemList[i];
                if (item.taskId == _taskInfo.getTaskId())
                {
                    item.updateTask(_taskInfo);
                    break;
                }
            }
            refreshRed();
        }

        /// <summary>
        /// 阶段目标大阶段奖励领取推送
        /// </summary>
        /// <param name="_bigStepId"></param>
        public void onStageGoalBigStepRewardDraw(long _bigStepId)
        {
            _m_hasGetRewardBigGoalItemList.Add(_bigStepId);
            refreshRed();
            WinMsg.SendMsg(WinMsgType.ON_STAGE_GOAL_BIG_STEP_REWARD_DRAW);
        }

        /// <summary>
        /// 可领取大阶段首达奖励推送
        /// </summary>
        /// <param name="_msg"></param>
        public void onStageGoalBigStepFirstReachAdd(GS2GC_007_077_OnStageGoalBigStepFirstReachAdd _msg)
        {
            if (_msg == null)
                return;

            //增加可领取
            if (!_m_canDrawBigStepFirstReachList.Contains(_msg.getBigStepId()))
                _m_canDrawBigStepFirstReachList.Add(_msg.getBigStepId());

            refreshRed();
            WinMsg.SendMsg(WinMsgType.ON_STAGE_GOAL_FIRST_REACH_ADD, _msg.getBigStepId());
        }

        /// <summary>
        /// 阶段目标大阶段首达奖励领取推送
        /// </summary>
        /// <param name="_msg"></param>
        public void onStageGoalBigStepFirstReachRewardDraw(GS2GC_007_078_OnStageGoalBigStepFirstReachRewardDraw _msg)
        {
            if (_msg == null)
                return;

            //增加已领取
            if (!_m_hadDrawBigStepFirstReachList.Contains(_msg.getBigStep()))
                _m_hadDrawBigStepFirstReachList.Add(_msg.getBigStep());

            //移除可领取
            if (_m_canDrawBigStepFirstReachList.Contains(_msg.getBigStep()))
                _m_canDrawBigStepFirstReachList.Remove(_msg.getBigStep());

            refreshRed();
            WinMsg.SendMsg(WinMsgType.ON_STAGE_GOAL_FIRST_REACH_DRAW, _msg.getBigStep());
        }

        #endregion

        #region C2S

        /// <summary>
        /// 阶段目标数据初始化
        /// </summary>
        private void _reqStageGoalInit()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_002_062_ReqStageGoalInit());
        }

        /// <summary>
        /// 领取阶段奖励
        /// </summary>
        public void reqTakeStageGoalTaskReward(Action<GS2GC_007_025_RetTakeStageGoalTaskReward> _doneAtion = null)
        {
            StageGoalBigStepRefObj curBigStageGoal = GRefdataCoreMgr.instance.getStageGoalBigStepRefObj((int)_m_curStageStepId);
            StageGoalBigStepRefObj nextBigStageGoal = GRefdataCoreMgr.instance.getStageGoalBigStepRefObj((int)_m_curStageStepId + 1);
            StageGoalRefObj nextSmallStageRefObj = GRefdataCoreMgr.instance.stageGoalRefCore.getRef(_m_curStageStepId + 1);

            NPGSClientListener.sendRequestByLog(GSWriter_007_CommOp.make_025_ReqTakeStageGoalTaskReward(_m_curStageStepId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_007_025_RetTakeStageGoalTaskReward>((_isSuc, _info) =>
                {
                    if (null == _info)
                        return;

                    if (_isSuc)
                    {
                        //完成大阶段，发送阶段完成埋点
                        if (nextBigStageGoal != null && curBigStageGoal != null && nextBigStageGoal.big_step != curBigStageGoal.big_step && curBigStageGoal.big_step == 2)
                            GCommon.sendAllThirdCustomEvent(EThirdCustomEventType.STAGE_2);
                        if((nextBigStageGoal == null || nextSmallStageRefObj == null) && curBigStageGoal != null && curBigStageGoal.big_step == 8)
                            GCommon.sendAllThirdCustomEvent(EThirdCustomEventType.STAGE_8);

                        if (_doneAtion != null)
                            _doneAtion(_info);
                    }
                }));
        }

        /// <summary>
        /// 领取阶段目标大阶段奖励
        /// </summary>
        /// <param name="_bigStep"></param>
        /// <param name="_doneAction"></param>
        public void reqTakeBigStepReward(long _bigStep, Action<GS2GC_007_019_RetTakeStageGoalBigStepReward> _doneAction = null)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_007_CommOp.make_019_ReqTakeStageGoalBigStepReward(_bigStep),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_007_019_RetTakeStageGoalBigStepReward>(_info =>
                {
                    if (null == _info)
                        return;
                    
                    _doneAction?.Invoke(_info);
                }));
        }

        /// <summary>
        /// 领取阶段目标子任务奖励
        /// </summary>
        /// <param name="_taskId"></param>
        /// <param name="_doneAction"></param>
        public void reqTakeStageGoalSubTaskReward(long _taskId, Action<GS2GC_007_018_RetTakeStageGoalSubTaskReward> _doneAction = null)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_007_CommOp.make_018_ReqTakeStageGoalSubTaskReward(_taskId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_007_018_RetTakeStageGoalSubTaskReward>(_info =>
                {
                    if (null == _info)
                        return;
                    
                    _doneAction?.Invoke(_info);
                }));
        }

        /// <summary>
        /// 领取阶段目标首达奖励
        /// </summary>
        /// <param name="_bigStepId">大阶段ID</param>
        public void reqDrawStageGoalFirstReachReward(long _bigStepId)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_007_CommOp.make_015_ReqDrawStageGoalFirstReachReward(_bigStepId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_007_015_RetDrawStageGoalFirstReachReward>(null));
        }
        
        /// <summary>
        /// 请求阶段目标首达详细信息
        /// </summary>
        public void reqStageGoalFirstReachDetailInfo(long _bigStepId, Action<GS2GC_007_016_RetStageGoalFirstReachDetailInfo> _doneAction = null)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_007_CommOp.make_016_ReqStageGoalFirstReachDetailInfo(_bigStepId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_007_016_RetStageGoalFirstReachDetailInfo>(_info =>
                {
                    if (_info == null)
                        return;
                    _doneAction?.Invoke(_info);
                }));
        }
        
        /// <summary>
        /// 请求阶段目标首达基础信息
        /// </summary>
        public void reqStageGoalFirstReachBaseInfo(Action<GS2GC_007_017_RetStageGoalFirstReachBaseInfo> _doneAction = null)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_007_CommOp.make_017_ReqStageGoalFirstReachBaseInfo(),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_007_017_RetStageGoalFirstReachBaseInfo>(_info =>
                {
                    if (_info == null)
                        return;

                    //更新数据
                    if (_info.getInfoList() != null)
                    {
                        _m_lFirstReachInfoList.Clear();
                        for (int i = 0; i < _info.getInfoList().Count; i++)
                        {
                            _m_lFirstReachInfoList.Add(new StageGoalFirstReachInfo(_info.getInfoList()[i]));
                        }
                    }
                    if(_m_topPlayerInfo == null)
                        _m_topPlayerInfo = new StageGoalTopPlayerInfo(_info.getTopInfo());
                    else
                        _m_topPlayerInfo.updateInfo(_info.getTopInfo());

                    _doneAction?.Invoke(_info);
                }));
        }

        #endregion
    }
}
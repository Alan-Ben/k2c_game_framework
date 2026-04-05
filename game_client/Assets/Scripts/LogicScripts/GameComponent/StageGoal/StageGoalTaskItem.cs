using ALPackage;
using Common.StageGoalObj;
using UnityEngine;

namespace GOE
{
    // 阶段目标任务数据结构 用于管理单个任务
    public class StageGoalTaskItem
    {
        //服务端给的值
        private long _m_sCount;

        //目标ID
        private long _m_taskId;

        //配表数据
        private StageGoalTaskRefObj _m_refObj;

        //是否已领取奖励
        private bool _m_bIsGetReward;

        //构造函数
        public StageGoalTaskItem(StageGoalTask_Info _info)
        {
            updateTask(_info);
        }

        /// <summary>
        /// 配表信息
        /// </summary>
        public StageGoalTaskRefObj refObj { get { return _m_refObj; } }

        /// <summary>
        /// 目标id
        /// </summary>
        public long taskId { get { return _m_taskId; } }

        /// <summary>
        /// 是否已读红点
        /// </summary>
        public bool isReadRedTip { get { return AccountSettingMgr.instance.accountSetting != null && AccountSettingMgr.instance.accountSetting.isReadStageTaskRedTip(_m_taskId); } set { if(value) AccountSettingMgr.instance.accountSetting?.recordReadStageTaskRedTipTaskId(_m_taskId); } }

        /// <summary>
        /// 是否已经解锁
        /// </summary>
        public bool isUnlock
        {
            get
            {
                if (_m_refObj == null)
                    return false;
                else if (_m_refObj.simple_unlock_id <= 0)
                    return true;
                else
                    return GCommon.isSimpleUnlock(_m_refObj.simple_unlock_id);
            }
        }

        public void discard()
        {
            _removeRegister();
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        public void updateTask(StageGoalTask_Info _info)
        {
            if (null == _info)
                return;

            _m_sCount = _info.getCounter();
            _m_bIsGetReward = _info.getHadDraw();
            if (_m_taskId != _info.getTaskId())
            {
                _m_taskId = _info.getTaskId();
                _removeRegister();
                _m_refObj = GRefdataCoreMgr.instance.stageGoalTaskRefCore.getRef(_m_taskId);
                _addRegister();
            }
            
            WinMsg.SendMsg(WinMsgType.ON_STAGE_GOAL_TASK_CHG, _m_taskId);
        }

        /// <summary>
        /// 监听消息变动
        /// </summary>
        private void _addRegister()
        {
            if (null == _m_refObj)
                return;

            //注册进度刷新事件
            if (null != _m_refObj.add_msg_type_list)
            {
                WinMsgType temp = 0;
                for (int i = 0; i < _m_refObj.add_msg_type_list.Count; i++)
                {
                    bool isParse = ALCommon.TryEnumParse(typeof(WinMsgType), _m_refObj.add_msg_type_list[i], out temp);
                    if (!isParse || temp == WinMsgType.NONE)
                        continue;

                    WinMsg.RegisterMsgAct(temp, _refreshTask);
                }
            }
        }

        /// <summary>
        /// 移除监听
        /// </summary>
        private void _removeRegister()
        {
            if (null == _m_refObj)
                return;

            //监听客户端目标数变动
            if (null != _m_refObj.add_msg_type_list)
            {
                WinMsgType temp = 0;
                for (int i = 0; i < _m_refObj.add_msg_type_list.Count; i++)
                {
                    bool isParse = ALCommon.TryEnumParse(typeof(WinMsgType), _m_refObj.add_msg_type_list[i], out temp);
                    if (!isParse || temp == WinMsgType.NONE)
                        continue;

                    WinMsg.UnregisterMsgAct(temp, _refreshTask);
                }
            }
        }

        /// <summary>
        /// 刷新阶段目标客户端计数
        /// </summary>
        private void _refreshTask()
        {
            if (null == _m_refObj)
                return;
            
            WinMsg.SendMsg(WinMsgType.ON_STAGE_GOAL_TASK_CHG, _m_taskId);
            //刷新红点
            NPPlayer.instance.stageGoalComp.refreshRed();
        }

        /// <summary>
        /// 获取当前进度
        /// </summary>
        public long getCurCount()
        {
            if (null == _m_refObj || null == _m_refObj.process_cur_count)
                return _m_sCount;

            return _m_sCount + _m_refObj.process_cur_count.CalculateVariableResult(null);
        }

        /// <summary>
        /// 获取当前进度值
        /// </summary>
        /// <returns></returns>
        public float getCurProgress()
        {
            if (null == _m_refObj || 0 == _m_refObj.process_count)
                return 0;

            return getCurCount() * 1.0f / _m_refObj.process_count;
        }

        public bool isCompleted()
        {
            if (null == _m_refObj || !isUnlock)
                return false;

            if (0 == _m_refObj.process_count)
                return true;
            
            return getCurCount() >= _m_refObj.process_count;
        }

        /// <summary>
        /// 获取当前任务领奖状态
        /// </summary>
        /// <returns></returns>
        public EStageGoalTaskItemState getCurRewardType()
        {
            if(!isUnlock)
                return EStageGoalTaskItemState.UNLOCK;
            else if (!isCompleted())
                return EStageGoalTaskItemState.CAN_NOT_GET;
            else if (_m_bIsGetReward)
                return EStageGoalTaskItemState.HAS_GET;
            else
                return EStageGoalTaskItemState.CAN_GET;
        }

        /// <summary>
        /// 获取当前进度值显示 
        /// </summary>
        public string getCurProgressFormat(Color _completeCurProcessColor, Color _notCompleteCurProcessColor)
        {
            if (null == _m_refObj)
                return "";

            string curCountStr = GCommon.getValueFormatStr(_m_refObj.process_num_format, getCurCount());
            string allCountStr = GCommon.getValueFormatStr(_m_refObj.process_num_format, _m_refObj.process_count);
            curCountStr = GCommon.addColorForRichText(curCountStr, isCompleted()?_completeCurProcessColor: _notCompleteCurProcessColor);
            return TextTranslate.instance.getLanguage(TransKeyConst.stage_goal_process_percent, curCountStr, allCountStr);
        }
    }
}
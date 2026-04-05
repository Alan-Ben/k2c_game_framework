using ALPackage;
using Common.QuestEnum;
using Common.QuestObj;
using NPCommon;
using NPEnum;
using System;
using System.Collections.Generic;

namespace GOE
{
    // 日常 周常任务数据结构 用于管理单个任务
    public class DailyQuestItem : _ISystemQuest
    {
        //任务类型
        private EDailyQuestType _m_type;
        //任务目标id 
        private long _m_lDailyQuestId;
        //任务数据对象
        private DailyQuestRefObj _m_dailyQuestRef;
        //当前计数
        private long _m_curCount;
        //是否已完成
        private bool _m_isFinish;
        //是否为随机任务
        private bool _m_isRandom;


        /// <summary>
        /// 每日任务id
        /// </summary>
        public long dailyQuestId { get { return _m_lDailyQuestId; } }
        /// <summary>
        /// 任务数据对象
        /// </summary>
        public DailyQuestRefObj dailyQuestRef { get { return _m_dailyQuestRef; } }
        /// <summary>
        /// 当前计数
        /// </summary>
        public long curFinallyCount { get { return _m_curCount + ((dailyQuestRef != null && dailyQuestRef.process_cur_count != null) ? dailyQuestRef.process_cur_count.CalculateVariableResult(null) : 0); } }
        /// <summary>
        /// 是否已经完成
        /// </summary>
        public bool isFinish { get { return _m_isFinish; } }
        /// <summary>
        /// 是否是随机任务
        /// </summary>
        public bool isRandom { get { return _m_isRandom; } }
        /// <summary>
        /// 是否可领取奖励
        /// </summary>
        public bool canFinish { get { if (isFinish) return false; return null == _m_dailyQuestRef ? false: _m_dailyQuestRef.process_count <= curFinallyCount; } }
        /// <summary>
        /// 任务名称
        /// </summary>
        public string questName { get { return _m_dailyQuestRef != null ? _m_dailyQuestRef.daily_quest_title : null; } }
        /// <summary>
        /// 展示名称
        /// </summary>
        public string showNameStr { get { return _m_dailyQuestRef != null ? TextTranslate.instance.getLanguage(_m_dailyQuestRef.daily_quest_title) : null; } }
        /// <summary>
        /// 当前计数
        /// </summary>
        public long curCount { get { return curFinallyCount; } }
        /// <summary>
        /// 进度值格式化显示方式
        /// </summary>
        public EValueFormatType processNumFormat { get { return _m_dailyQuestRef != null ? _m_dailyQuestRef.process_num_format : EValueFormatType.NORMAL; } }
        /// <summary>
        /// 目标计数
        /// </summary>
        public long targetCount { get { return _m_dailyQuestRef != null ? _m_dailyQuestRef.process_count : 0; } }
        /// <summary>
        /// 是否可以领取奖励
        /// </summary>
        public bool canGetReward { get { return getDailyQuestState() == EDailyQuestState.CAN_GET; } }
        /// <summary>
        /// 奖励列表
        /// </summary>
        public List<NPCommonCostItem> rewardItemList { get { return _m_dailyQuestRef != null ? _m_dailyQuestRef.reward_item_list : null; } }

        //构造函数
        public DailyQuestItem(DailyQuest_Info _info, EDailyQuestType _type, bool _isRandom = false)
        {
            if (null == _info)
                return;

            _m_type = _type;
            _m_lDailyQuestId = _info.getTargetId();
            _m_dailyQuestRef = GRefdataCoreMgr.instance.dailyQuestMap.getRef(_m_lDailyQuestId);
            if (null == _m_dailyQuestRef)
                ALLog.Error($"can not find {_m_lDailyQuestId}'s DailyQuestRefObj!");

            _m_curCount = _info.getCurCount();
            _m_isFinish = _info.getIsFinish();
            _m_isRandom = _isRandom;
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        public void update(DailyQuest_Info _info)
        {
            if (null == _info)
                return;

            long oldCount = curFinallyCount;
            _m_curCount = _info.getCurCount();
            _m_isFinish = _info.getIsFinish();
            
            //如果任务完成 需要侧边弹窗
            //每日任务
            if (_m_type == EDailyQuestType.DAY && oldCount < _m_dailyQuestRef.process_count && curFinallyCount >= _m_dailyQuestRef.process_count && GCommon.isFuncUnlock(ENPFunctionType.DAILY_QUEST))
            {
                NPGUIAddSceneCenterTip.instance.showIconTextTip(_m_dailyQuestRef.icon, _m_dailyQuestRef.daily_quest_title, GRefdataCoreMgr.instance.npGeneral.daily_quest_finish_center_tip_id);
            }
            // //每周任务
            // if (_m_type == EDailyQuestType.WEEK && oldCount < _m_dailyQuestRef.process_count && curFinallyCount >= _m_dailyQuestRef.process_count && GCommon.isFuncUnlock(ENPFunctionType.WEEKLY_QUEST))
            // {
            //     NPGUIAddSceneCenterTip.instance.showTextTip(TextTranslate.instance.getLanguage(TransKeyConst.quest_followSuc_str, _m_dailyQuestRef.daily_quest_title), NPConst.C_WEEK_QUEST_TEXT_TIP_REF_ID);
            // }
        }

        /// <summary>
        /// 获取每日任务状态
        /// </summary>
        /// <returns></returns>
        public EDailyQuestState getDailyQuestState()
        {
            if (_m_dailyQuestRef == null || !GCommon.isSimpleUnlock(_m_dailyQuestRef.simple_unlock_id))
                return EDailyQuestState.LOCK;

            if (_m_isFinish)
                return EDailyQuestState.ALREADY_GET;

            if (canFinish)
                return EDailyQuestState.CAN_GET;

            return EDailyQuestState.CAN_NOT_GET;
        }

        /// <summary>
        /// 处理领取奖励
        /// </summary>
        public void dealGetReward(Action<List<NPCommon_ItemInfo>> _callback)
        {
            if (!canGetReward)
                return;

            NPPlayer.instance.dailyQuestComp.reqFinishDailyQuest(_m_type, _m_lDailyQuestId, _callback, null);
        }

        /// <summary>
        /// 处理前往
        /// </summary>
        public void dealGoTo()
        {
            if (canGetReward || null == _m_dailyQuestRef)
                return;
            
            //执行跳转效果
            NPSimpleTutorialRefObj simpleTutorialRefObj = GRefdataCoreMgr.instance.simpleTutorialRefCore.getRef(_m_dailyQuestRef.go_to_simple_tutorial_id);
            if (null == simpleTutorialRefObj)
            {
                _m_dailyQuestRef.go_to?.dealEffect();
                return;
            }
            
            //设置简易引导
            SimpleTutorialController.instance.setCurSimpleGuide(simpleTutorialRefObj);
            //如果还不在引导中，触发简易引导
            if(Game.instance.isInTutorial)
                return;

            //如果没执行成功，执行默认跳转
            if (!SimpleTutorialController.instance.checkStartSimpleTutorial(QueueMgr.instance._lastNode.nodeTag))
            {
                _m_dailyQuestRef.go_to?.dealEffect();
            }
        }
    }
}

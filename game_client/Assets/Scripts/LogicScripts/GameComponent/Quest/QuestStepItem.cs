using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NPEnum;
using ALPackage;
using Common.QuestEnum;
using Common.QuestObj;

namespace GOE
{
    // 任务步骤数据结构
    public class QuestStepItem
    {
        //任务id
        private long _m_questId;

        //任务状态
        private EQuestStatus _m_questStatus;

        //任务步骤id 
        private long _m_questStepId;
        private QuestStepRefObj _m_srStepRef;

        //任务步骤超时时间
        private long _m_expireTimeTagS;

        //任务目标列表
        private List<QuestTargetItem> _m_questTargetItemList;

        //记录任务步骤之前是否就已经完成
        private bool _m_isFinishTag;

        //构造函数
        public QuestStepItem(Quest_info _info)
        {
            if (null == _info || null == _info.getQuestStep())
                return;

            _m_questId = _info.getQuestId();
            _m_questStatus = _info.getQuestStatus();
            _m_questStepId = _info.getQuestStep().getQuestStep();

            //如果是未接受 设置成第一个步骤
            if(_m_questStatus == EQuestStatus.WAITING)
            {
                QuestRefObj refObj = GRefdataCoreMgr.instance.questMap.getRef(_info.getQuestId());
                if (null != refObj)
                    _m_questStepId = refObj.first_step_id;
            }

            _m_srStepRef = GRefdataCoreMgr.instance.questStepMap.getRef(_m_questStepId);
            if (null == _m_srStepRef)
                ALLog.Error($"can not find {_m_questStepId}'s QuestStepRefObj!");

            _m_expireTimeTagS = _info.getQuestStep().getExpireTimeTagS();

            //初始化任务目标列表
            _m_questTargetItemList = new List<QuestTargetItem>();
            List<Quest_Target> targetList = _info.getQuestStep().getTargetList();
            Quest_Target temp = null;
            for (int i = 0; i < targetList.Count; i++)
            {
                temp = targetList[i];
                if (null == temp)
                    continue;

                QuestTargetItem item = new QuestTargetItem(temp,this);
                _m_questTargetItemList.Add(item);
            }

        }

        public long questId { get { return _m_questId; } }
        //任务步骤id
        public long questStepId { get { return _m_questStepId; } }

        //任务步骤超时时间
        public long expireTimeTagS { get { return _m_expireTimeTagS; } }

        //任务目标列表
        public List<QuestTargetItem> questTargetItemList { get { return _m_questTargetItemList; } }

        //是否为限时任务步骤
        public bool isTimeQuestStep { get { return _m_expireTimeTagS > 0; } }

        //剩余限时时长
        public long stepLeftTimeMs { get { return _m_expireTimeTagS*1000 - FpsAndPingMgr.instance.serverTimeTag; } }

        /// <summary>
        /// 获取配表信息
        /// </summary>
        public QuestStepRefObj questStepRefObj { get{ return _m_srStepRef; } }

        /// <summary>
        /// 获取当前步骤是否已经完成
        /// </summary>
        /// <returns></returns>
        public bool isFinish { get { return getQuestStepStatus() == ENPQuestStepStatusEnum.QUEST_CANGET; } }

        /// <summary>
        /// 获取当前步骤的状态
        /// </summary>
        public ENPQuestStepStatusEnum getQuestStepStatus()
        {
            //如果任务状态为等待中 则步骤状态为未接受
            if (_m_questStatus == EQuestStatus.WAITING)
                return ENPQuestStepStatusEnum.QUEST_NONE;

            //异常返回失败
            if (null == _m_questTargetItemList || _m_questTargetItemList.Count == 0)
                return ENPQuestStepStatusEnum.QUEST_FAIL;

            //如果为限时任务步骤 且步骤超时
            if (isTimeQuestStep && stepLeftTimeMs <= 0)
                return ENPQuestStepStatusEnum.QUEST_FAIL;

            QuestTargetItem item = null;
            for (int i = 0; i < _m_questTargetItemList.Count; i++)
            {
                item = _m_questTargetItemList[i];
                if (null == item)
                    continue;

                //如果有未完成表示进行中
                if (!item.getQuestTargetIsFinish())
                    return ENPQuestStepStatusEnum.QUEST_ISGOING;
            }

            return ENPQuestStepStatusEnum.QUEST_CANGET;
        }

        //初始化
        public void init()
        {
            _m_isFinishTag = false;
            QuestTargetItem temp = null;
            for (int i = 0; i < _m_questTargetItemList.Count; i++)
            {
                temp = _m_questTargetItemList[i];
                if (null == temp)
                    continue;

                temp.init();
            }
        }

        //刷新客户端计数
        public void reCalcCurCusCount()
        {
            if(null == _m_questTargetItemList)
                return;

            foreach (QuestTargetItem npQuestTargetItem in _m_questTargetItemList)
            {
                if(null == npQuestTargetItem)
                    continue;
                
                npQuestTargetItem.reCalcCurCusCount();
            }
        }
        
        /// <summary>
        /// 更新任务步骤 需要关联任务的状态
        /// </summary>
        public void update(Quest_Step _step, EQuestStatus _questStatus)
        {
            if (null == _step)
                return;

            _m_isFinishTag = false;

            _m_questStatus = _questStatus;

            //需要更新配表
            if(_m_questStepId != _step.getQuestStep())
            {
                _m_srStepRef = GRefdataCoreMgr.instance.questStepMap.getRef(_step.getQuestStep());
                if (null == _m_srStepRef)
                    ALLog.Error($"can not find {_m_questStepId}'s QuestStepRefObj!");
            }

            _m_questStepId = _step.getQuestStep();
            _m_expireTimeTagS = _step.getExpireTimeTagS();

            //清空目标
            _clearTarget();

            //更新任务目标列表
            List<Quest_Target> targetList = _step.getTargetList();
            if (null != targetList)
            {
                //构造目标数据
                Quest_Target temp = null;
                for (int i = 0; i < targetList.Count; i++)
                {
                    temp = targetList[i];
                    if (null == temp)
                        continue;

                    QuestTargetItem item = new QuestTargetItem(temp, this);
                    _m_questTargetItemList.Add(item);
                }

                //目标初始化
                init();
            }
        }

        /// <summary>
        /// 更新单个步骤目标计数器
        /// </summary>
        public void updateTarget(long _targetId,long _curCount)
        {
            QuestTargetItem item = null;
            for (int i = 0; i < _m_questTargetItemList.Count; i++)
            {
                item = _m_questTargetItemList[i];
                if (null == item)
                    continue;

                if (item.questTargetId == _targetId)
                    item.setCurCount(_curCount);
            }
        }

        /// <summary>
        /// 获取步骤当前的一个目标
        /// </summary>
        public QuestTargetItem getCurTarget()
        {
            QuestTargetItem temp = null;

            QuestTargetItem curItem = null;

            for (int i = 0; i < _m_questTargetItemList.Count; i++)
            {
                temp = _m_questTargetItemList[i];
                if (null == temp)
                    continue;

                if (i == 0)
                    curItem = temp;

                //返回未完成的目标
                if (!temp.getQuestTargetIsFinish())
                {
                    curItem = temp;
                    break;
                }

            }

            return curItem;
        }

        /// <summary>
        /// 获取任务目标
        /// </summary>
        /// <param name="_targetId"></param>
        /// <returns></returns>
        public QuestTargetItem getTarget(long _targetId)
        {
            for (int i = 0; i < _m_questTargetItemList.Count; i++)
            {
                QuestTargetItem item = _m_questTargetItemList[i];
                if (null == item)
                    continue;

                if (item.questTargetId == _targetId)
                    return item;
            }
            return null;
        }

        /// <summary>
        /// 判断任务步骤是否可以完成
        /// </summary>
        public void checkStepCanGet(bool _showTip)
        {
            if (_m_isFinishTag || !isFinish)
                return;

            _m_isFinishTag = isFinish;
            WinMsg.SendMsg(WinMsgType.QUEST_STEP_STATUS_CHG, this);
            
            if(null == _m_srStepRef)
                return;
            
            //如果该任务需要自动完成 则需要告知服务器
            if (_m_srStepRef.auto_done)
            {
                NPPlayer.instance.questComp.reqFinishQuest(_m_questId);
                
                //请求的时候不应该删除对应target，会导致回包之前任务目标文本为空
                // _clearTarget();
            }

            //如果需要 弹出侧边栏
            //按小方说的，这个只是告诉你任务已经做完的提示，真正完成需要玩家自己去点
            if (_showTip && !_m_srStepRef.is_not_show_tip)
            {
                // NPGUIAddSceneCenterTip.instance.showQuestCompleteTips(TextTranslate.instance.getLanguage(TransKeyConst.common_strDotStr, _m_srStepRef.sort_id, _m_srStepRef.quest_step_name_str));

                //可能有某些弹窗弹出，这里下一帧处理避免和弹窗一起出现
                ALCommonActionMonoTask.addNextFrameTask(() =>
                {
                    NPPlayer.instance.questComp.tryShowQuestEntryTip();
                });
            }
        }

        //释放
        public void discard()
        {
            _m_isFinishTag = false;

            _m_srStepRef = null;

            _clearTarget();
        }

        /// <summary>
        /// 清空任务所有的目标对象
        /// </summary>
        protected void _clearTarget()
        {
            QuestTargetItem temp = null;
            for (int i = 0; i < _m_questTargetItemList.Count; i++)
            {
                temp = _m_questTargetItemList[i];
                if (null == temp)
                    continue;

                temp.discard();
            }
            _m_questTargetItemList.Clear();
        }
    }
}

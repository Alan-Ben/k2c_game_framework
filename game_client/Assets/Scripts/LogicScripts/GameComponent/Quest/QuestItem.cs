using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NPEnum;
using ALPackage;
using Common.QuestEnum;
using Common.QuestObj;
using NPCommon;

namespace GOE
{
    // 任务数据结构 用于管理单个任务
    public class QuestItem : _IFollowableQuest
    {
        //任务id 
        private long _m_questId;

        //任务数据对象
        private QuestRefObj _m_qrQuestRef;

        //任务状态
        private EQuestStatus _m_questStatus;

        //任务当前的步骤
        private QuestStepItem _m_stepItem;

        public QuestItem()
        {
        }

        //构造函数
        public QuestItem(Quest_info _info)
        {
            if (null == _info)
                return;

            _m_questId = _info.getQuestId();
            _m_qrQuestRef = GRefdataCoreMgr.instance.questMap.getRef(_m_questId);
            if (null == _m_qrQuestRef)
                ALLog.Error($"can not find {_m_questId}'s QuestRefObj!");

            _m_questStatus = _info.getQuestStatus();

            _m_stepItem = new QuestStepItem(_info);

        }

        //任务id 
        public long questId { get { return _m_questId; } }

        //任务状态
        public EQuestStatus questStatus { get { return _m_questStatus; } }

        //任务当前的步骤
        public QuestStepItem stepItem { get { return _m_stepItem; } }

        /// <summary>
        /// 获取配表信息
        /// </summary>
        public QuestRefObj questRef { get { return _m_qrQuestRef; } }

        /// <summary>
        /// 更新数据
        /// </summary>
        public void update(Quest_info _info)
        {
            if (null == _info)
                return;

            _m_questId = _info.getQuestId();

            _m_questStatus = _info.getQuestStatus();

            //更新任务步骤
            if (null != _info.getQuestStep())
                _m_stepItem.update(_info.getQuestStep(), _m_questStatus);

        }

        public void init()
        {
            if(null != _m_stepItem)
                _m_stepItem.init();
        }
        public void discard()
        {
            _m_qrQuestRef = null;
            if (null != _m_stepItem)
                _m_stepItem.discard();
            _m_stepItem = null;
        }

        //重新计算客户端计数
        public void reCalcCurCusCount()
        {
            if(null != _m_stepItem)
                _m_stepItem.reCalcCurCusCount();
        }

        //步骤id是否完成
        public bool getStepIdIsDone(long _stepId)
        {
            if (null == _m_qrQuestRef)
                return false;
            
            //没有当前步骤说明都没完成
            if (null == _m_stepItem)
                return false;

            List<long> doneStepIdList = getDoneStepIdList();
            if (null == doneStepIdList || doneStepIdList.Count == 0)
                return false;

            return doneStepIdList.Contains(_stepId);
        }

        //获取已经完成的任务列表
        public List<long> getDoneStepIdList()
        {
            if (null == _m_qrQuestRef)
                return null;

            //没有当前步骤说明都没完成
            if (null == _m_stepItem)
                return null;

            List<long> doneIdList = new List<long>();
            
            //这个任务第一步
            QuestStepRefObj stepRefObj =  GRefdataCoreMgr.instance.questStepMap.getRef(_m_qrQuestRef.first_step_id);
            
            //还找到到这个步骤并且步骤id跟当前步骤不一致则继续循环
            while (null != stepRefObj && stepRefObj.step_id != _m_stepItem.questStepId)
            {
                //加入数据
                doneIdList.Add(stepRefObj.step_id);
                
                //找下一个步骤
                stepRefObj = GRefdataCoreMgr.instance.questStepMap.getRef(stepRefObj.next_step_id);
            }

            return doneIdList;
        }

        /// <summary>
        /// 是否全部步骤都完成
        /// </summary>
        /// <returns></returns>
        public bool isAllStepDone()
        {
            //没有当前步骤说明都没完成
            if (null == _m_stepItem)
                return false;

            //是最后一步并且已经完成
            if (_m_stepItem.questStepRefObj != null && _m_stepItem.questStepRefObj.next_step_id <= 0 &&
                _m_stepItem.getQuestStepStatus() == ENPQuestStepStatusEnum.QUEST_CANGET)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 当前步骤是否是最后一步
        /// </summary>
        /// <returns></returns>
        public bool curStepIsLastStep()
        {
            if (null == _m_stepItem)
                return false;

            if (_m_stepItem.questStepRefObj != null && _m_stepItem.questStepRefObj.next_step_id <= 0)
                return true;
            else
                return false;
        }

        #region 任务跟随接口

        public ENPFollowQuestType questType
        {
            get
            {
                if (_m_qrQuestRef == null)
                    return ENPFollowQuestType.BRANCH;

                switch (_m_qrQuestRef.quest_type)
                {
                    case EQuestType.MAIN:
                        return ENPFollowQuestType.MAIN;
                    case EQuestType.WISH:
                        return ENPFollowQuestType.WISH_QUEST;
                    default:
                        return ENPFollowQuestType.BRANCH;
                }
            }
        }

        public long id { get { return _m_questId; } }
        public long sortId { get { return _m_stepItem != null && _m_stepItem.questStepRefObj != null? _m_stepItem.questStepRefObj.sort_id:0; } }

        public ENPQuestStepStatusEnum questStepStatus { get { return _m_stepItem == null ? ENPQuestStepStatusEnum.QUEST_NONE : _m_stepItem.getQuestStepStatus(); } }

        public bool enableFollow { get { return NPPlayer.instance.questComp.questItemMgr.getQuestItem(_m_questId) != null && questStepStatus != ENPQuestStepStatusEnum.QUEST_NONE; } }
        public string title { get { return _m_stepItem == null || _m_stepItem.questStepRefObj == null ? string.Empty : TextTranslate.instance.getLanguage(_m_stepItem.questStepRefObj.quest_step_name_str); } }
        public string showStr
        {
            get
            {
                if (_m_stepItem == null || _m_stepItem.questStepRefObj == null)
                    return string.Empty;

                return TextTranslate.instance.getLanguage(TransKeyConst.common_strDotStr, _m_stepItem.questStepRefObj.sort_id, _m_stepItem.questStepRefObj.quest_step_name_str);
                // QuestTargetItem target = _m_stepItem.getCurTarget();
                // if (target == null || target.targetRefObj == null)
                //     return string.Empty;
                //
                // switch (_m_stepItem.getQuestStepStatus())
                // {
                //     case ENPQuestStepStatusEnum.QUEST_CANGET:
                //         return TextTranslate.instance.getLanguage(TransKeyConst.quest_followSuc_str, target.targetRefObj.getTargetStr());
                //
                //     case ENPQuestStepStatusEnum.QUEST_ISGOING:
                //         return TextTranslate.instance.getLanguage(TransKeyConst.quest_follow_str_num_num, target.targetRefObj.getTargetStr(), target.getQuestTargetRealCount(), target.targetRefObj.process_count);
                //
                //     case ENPQuestStepStatusEnum.QUEST_FAIL:
                //         return TextTranslate.instance.getLanguage(TransKeyConst.quest_followFail_str, target.targetRefObj.getTargetStr());
                // }
                // return string.Empty;
            }
        }

        public string curCountStr
        {
            get
            {
                if (stepItem == null || stepItem.questStepRefObj == null)
                    return "";

                QuestTargetItem questTargetItem = stepItem.getCurTarget();
                if (questTargetItem == null || questTargetItem.targetRefObj == null)
                    return "";

                long curCount = questTargetItem.getQuestTargetRealCount();
                string curCountStr = GCommon.getValueFormatStr(stepItem.questStepRefObj.process_num_format, curCount);
                return curCountStr;
            }
        }

        public string targetCountStr
        {
            get
            {
                if (stepItem == null || stepItem.questStepRefObj == null)
                    return "";

                QuestTargetItem questTargetItem = stepItem.getCurTarget();
                if (questTargetItem == null || questTargetItem.targetRefObj == null)
                    return "";

                long targetCount = questTargetItem.targetRefObj.process_count;
                string targetCountStr = GCommon.getValueFormatStr(stepItem.questStepRefObj.process_num_format, targetCount);
                return targetCountStr;
            }
        }

        public string progressStr
        {
            get
            {
                if (stepItem == null || stepItem.questStepRefObj == null)
                    return "";

                if (stepItem.questStepRefObj.process_num_format == EValueFormatType.PLAYER_CHAPTER)
                {
                    // 章节另外的展示格式
                    return TextTranslate.instance.getLanguage(TransKeyConst.chapter_currentTotalNum_num_num, curCountStr, targetCountStr);
                }
                else
                {
                    return TextTranslate.instance.getLanguage(TransKeyConst.common_currentTotalNum_num_num, curCountStr, targetCountStr);
                }
            }
        }

        public long expireTimeTagS
        {
            get
            {
                return _m_stepItem == null ? 0 : _m_stepItem.expireTimeTagS;
            }
        }

        public List<WinMsgType> getListenMsgList()
        {
            return NPPlayer.instance.questComp.followListenMsgList;
        }

        public void dealFinish(Action<List<NPCommon_ItemInfo>> _callback)
        {
            NPPlayer.instance.questComp.reqFinishQuest(_m_questId, _callback);
        }

        public void dealQuestGoto()
        {            
            QuestTargetItem targetItem = stepItem.getCurTarget();
            if (null == targetItem || null == targetItem.targetRefObj || null == targetItem.targetRefObj.go_to)
                return;
            
            //执行跳转效果
            NPSimpleTutorialRefObj simpleTutorialRefObj = GRefdataCoreMgr.instance.simpleTutorialRefCore.getRef(targetItem.targetRefObj.go_to_simple_tutorial_id);
            if (null == simpleTutorialRefObj)
            {
                targetItem.targetRefObj.go_to.dealEffect();
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
                targetItem.targetRefObj.go_to.dealEffect();
            }
        }

        #endregion
    }
}

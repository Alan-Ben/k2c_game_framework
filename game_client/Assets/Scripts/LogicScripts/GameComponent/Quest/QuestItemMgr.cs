using System;
using System.Collections.Generic;
using ALPackage;
using Common.QuestEnum;
using Common.QuestObj;
using GS2GC.p028_QuestOp;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 任务item管理类
    /// </summary>
    public class QuestItemMgr
    {
        //用于定时器开启达到解锁条件的任务
        private PlayerQuestComponent _m_questComp;

        //每秒检测数据状态的任务控制对象
        private ALCommonEnableTaskController _m_tcTickTaskController;

        //当前的任务列表
        [NotNull] private readonly List<QuestItem> _m_questList;
        //当前未接受的任务列表
        [NotNull] private readonly List<QuestItem> _m_waitQuestList;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_questComp"></param>
        public QuestItemMgr(PlayerQuestComponent _questComp)
        {
            //相关初始化
            _m_questList = new List<QuestItem>();
            _m_waitQuestList = new List<QuestItem>();

            _m_questComp = _questComp;
        }

        //重新计算客户端计数
        public void reCalcCurCusCount()
        {
            if (null != _m_questList)
            {
                foreach (QuestItem npQuestItem in _m_questList)
                {
                    if (npQuestItem != null) 
                        npQuestItem.reCalcCurCusCount();
                }
            }
            
            if (null != _m_waitQuestList)
            {
                foreach (QuestItem npQuestItem in _m_waitQuestList)
                {
                    if (npQuestItem != null) 
                        npQuestItem.reCalcCurCusCount();
                }
            }
        }
        

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="_questItem"></param>
        private void _addQuest(QuestItem _questItem)
        {
            if (null == _questItem || null == _questItem.stepItem || null == _questItem.questRef)
                return;

            _questItem.init();
            _m_questList.Add(_questItem);

            //如果是主线任务 判断当前是否需要弹窗章节开启弹窗
            // if(_questItem.questRef.quest_type == ENPQuestType.MAIN)
            // {
            //     NPGQuestWndCommon.showQuestMainOpen(_questItem.questRef.quest_group_id,_questItem.questId);
            // }

            //判断配表是否配置为自动追踪
            if (_questItem.questRef.is_auto_follow)
                QuestFollowMgr.instance.checkAndAutoFollow();
        }

        /// <summary>
        /// 更新任务目标的数量
        /// </summary>
        public void updateTargetCount(GS2GC_028_051_OnPlayerQuestStepCountChg _info)
        {
            if (null == _info)
                return;

            QuestItem item = getQuestItem(_info.getQuestId());
            if (null == item || null == item.stepItem)
                return;

            item.stepItem.updateTarget(_info.getQuestTargetId(), _info.getQuestTargetCount());
        }


        /// <summary>
        /// 新增 / 更新 一个任务的步骤
        /// </summary>
        public QuestItem updateQuest(Quest_info _info)
        {
            if (null == _info)
                return null;

            QuestItem item = getQuestItem(_info.getQuestId());
            if (null != item)
            {
                item.update(_info);
                WinMsg.SendMsg(WinMsgType.QUEST_UPDATE, item);
            }
            else if(_info.getQuestId() > 0 && _info.getQuestStatus() != EQuestStatus.NONE && _info.getQuestStep() != null && _info.getQuestStep().getQuestStep() > 0)
            {
                //新增
                item = new QuestItem(_info);
                _addQuest(item);

                if (item.questStatus == EQuestStatus.WAITING)
                    _m_waitQuestList.Add(item);

                WinMsg.SendMsg(WinMsgType.QUEST_ADD, item);
            }
            else
            {
                //数据无效已经到最后一个任务
                QuestFollowMgr.instance.setFollowQuest(null);
            }

            //尝试在提示都提示完成之后重新触发引导
            // NPUINoticeMgr.instance.dealTryPopNotice(()=> { WinMsg.SendMsg(WinMsgType.TRIGGER_TUTORIAL); });

            return item;
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        public void removeQuest(long _questId)
        {
            QuestItem item = null;
            for (int i = 0; i < _m_questList.Count; i++)
            {
                item = _m_questList[i];
                if (null == item)
                    continue;

                if (item.questId == _questId)
                {
                    item.discard();
                    _m_questList.RemoveAt(i);
                    WinMsg.SendMsg(WinMsgType.QUEST_REMOVE, item);
                    break;
                }
            }

        }

        /// <summary>
        /// 获取任务item
        /// </summary>
        /// <returns></returns>
        public QuestItem getQuestItem(long _questId)
        {
            QuestItem item = null;
            for (int i = 0; i < _m_questList.Count; i++)
            {
                item = _m_questList[i];
                if (null == item)
                    continue;

                if (item.questId == _questId)
                    return item;
            }

            return null;
        }

        /// <summary>
        /// 获取自动追踪的任务
        /// </summary>
        /// <returns></returns>
        public QuestItem getAutoFollowQuest()
        {
            QuestItem flagItem = null;
            QuestItem item = null;
            for (int i = 0; i < _m_questList.Count; i++)
            {
                item = _m_questList[i];
                if (null == item || null == item.questRef)
                    continue;
                if (i == 0)
                    flagItem = item;

                if (item.questRef.is_auto_follow)
                {
                    flagItem = item;
                    break;
                }
            }

            return flagItem;
        }

        /// <summary>
        /// 获取当前任务列表
        /// </summary>
        public void getCurQuestList(List<QuestItem> _list)
        {
            if (null == _list)
                return;

            _list.AddRange(_m_questList);
        }

        /// <summary>
        /// 获取当前获得的任务数量
        /// </summary>
        public int getCurQuestListCount()
        {
            if (null == _m_questList)
                return 0;

            return _m_questList.Count;
        }

        /// <summary>
        /// 当前是否已经拥有对应的任务
        /// </summary>
        /// <returns></returns>
        public bool hasQuest(long _questId)
        {
            QuestItem item = getQuestItem(_questId);
            if (null == item || null == item.stepItem || item.stepItem.getQuestStepStatus() == ENPQuestStepStatusEnum.QUEST_NONE)
                return false;

            return true;
        }

        private void _tickSec()
        {
            if (_m_waitQuestList.Count == 0)
                return;

            //处理每秒的检测
            _checkList();
        }

        /// <summary>
        /// 定时检测任务
        /// </summary>
        private void _checkList()
        {
            QuestItem temp = null;
            for (int i = _m_waitQuestList.Count - 1; i >= 0; i--)
            {
                temp = _m_waitQuestList[i];
                if (null == temp)
                    continue;

                if (null == _m_questComp)
                    return;

                //如果达到了开启条件
                if (_m_questComp.checkCanAcceptQuest(temp.questId))
                {
                    _m_questComp.reqStartQuest(temp.questId);
                    _m_waitQuestList.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// 开启定时器
        /// </summary>
        public void startTick()
        {
            stopTick();
            _m_tcTickTaskController = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_tickSec, 5f);
        }

        /// <summary>
        /// 关闭定时器
        /// </summary>
        private void stopTick()
        {
            //停止原先任务
            _m_tcTickTaskController.setDisable();
        }

        /// <summary>
        /// 初始化任务数据
        /// </summary>
        /// <param name="_list"></param>
        public void initQuestListData(List<Quest_info> _list)
        {
            if (null == _list)
                return;

            clear();

            Quest_info info = null;
            for (int i = 0; i < _list.Count; i++)
            {
                info = _list[i];

                //过滤none状态的任务
                if (null == info || info.getQuestStatus() == EQuestStatus.NONE)
                    continue;

                QuestItem item = new QuestItem(info);
                _addQuest(item);

                if (item.questStatus == EQuestStatus.WAITING)
                    _m_waitQuestList.Add(item);
            }
        }

        /// <summary>
        /// 清空任务列表
        /// </summary>
        private void _clearQuestList()
        {
            QuestItem temp = null;
            for (int i = 0; i < _m_questList.Count; i++)
            {
                temp = _m_questList[i];
                if (null == temp)
                    continue;

                temp.discard();
            }
            _m_questList.Clear();
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        public void clear()
        {
            stopTick();
            _clearQuestList();
            _m_waitQuestList.Clear();
        }
    }
}
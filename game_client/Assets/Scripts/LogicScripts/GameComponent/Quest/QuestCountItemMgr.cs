using System;
using System.Collections.Generic;
using ALPackage;
using Common.QuestEnum;
using Common.QuestObj;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 任务计数管理类
    /// </summary>
    public class QuestCountItemMgr
    {
        //任务计数数据
        [NotNull] private readonly List<QuestCountItem> _m_questCountItemList;

        /// <summary>
        /// 构造函数
        /// </summary>
        public QuestCountItemMgr()
        {
            _m_questCountItemList = new List<QuestCountItem>();
        }

        /// <summary>
        /// 初始化任务计数
        /// </summary>
        /// <param name="_list"></param>
        public void initQuestCountListData(List<Quest_Count> _list)
        {
            if (null == _list)
                return;

            Quest_Count count = null;
            for (int i = 0; i < _list.Count; i++)
            {
                count = _list[i];
                if (null == count)
                    continue;

                QuestCountItem countItem = new QuestCountItem(count);
                _m_questCountItemList.Add(countItem);
            }
        }

        /// <summary>
        /// 更新任务计数
        /// </summary>
        public void updateQuestCount(Quest_Count _count)
        {
            if (null == _count)
                return;

            QuestCountItem countItem = getQuestCountItem(_count.getQuestId());
            if (null != countItem)
                countItem.update(_count);
            else
            {
                countItem = new QuestCountItem(_count);
                _m_questCountItemList.Add(countItem);
            }
        }

        /// <summary>
        /// 获取任务计数item
        /// </summary>
        /// <param name="_questId"></param>
        /// <returns></returns>
        public QuestCountItem getQuestCountItem(long _questId)
        {
            QuestCountItem temp = null;
            for (int i = 0; i < _m_questCountItemList.Count; i++)
            {
                temp = _m_questCountItemList[i];
                if (null == temp)
                    continue;

                if (temp.questId == _questId)
                    return temp;
            }

            return null;
        }

        /// <summary>
        /// 判断任务完成次数是否达到上限
        /// </summary>
        /// <returns></returns>
        public bool checkQuestDoneCountIsMax(long _questId)
        {
            QuestCountItem item = getQuestCountItem(_questId);
            if (null == item)
                return false;

            return item.questDoneCountIsMax();
        }

        public void clear()
        {
            _m_questCountItemList.Clear();
        }

        /// <summary>
        /// 根据类型获取所有任务计数
        /// </summary>
        /// <param name="_countType"></param>
        /// <returns></returns>
        public long getAllQuestCount()
        {
            long totalCount = 0;
            QuestCountItem temp = null;
            for (int i = 0; i < _m_questCountItemList.Count; i++)
            {
                temp = _m_questCountItemList[i];
                if (null == temp)
                    continue;

                totalCount += temp.getCount();
            }

            return totalCount;
        }

        /// <summary>
        /// 获取主线任务完成数量
        /// </summary>
        /// <returns></returns>
        public long getMainQuestDoneCount()
        {
            long totalCount = 0;
            QuestCountItem temp = null;
            for (int i = 0; i < _m_questCountItemList.Count; i++)
            {
                temp = _m_questCountItemList[i];
                if (null == temp)
                    continue;

                QuestRefObj questRef = GRefdataCoreMgr.instance.questMap.getRef(temp.questId);
                if (temp.getCount() > 0 && questRef != null && questRef.quest_type == EQuestType.MAIN)
                    totalCount++;
            }

            return totalCount;
        }

        /// <summary>
        /// 判断任务组是否完成
        /// </summary>
        /// <param name="_groupId"></param>
        /// <returns></returns>
        public bool checkQuestGroupDone(long _groupId)
        {
            return checkQuestGroupDone(GRefdataCoreMgr.instance.questGroupMap.getRef(_groupId));
        }

        /// <summary>
        /// 判断任务组是否完成
        /// </summary>
        /// <param name="_groupRef"></param>
        /// <returns></returns>
        public bool checkQuestGroupDone(QuestGroupRefObj _groupRef)
        {
            if (_groupRef == null)
                return false;

            QuestCountItem temp = getQuestCountItem(_groupRef.last_quest_id);
            return temp != null && temp.getCount() > 0;
        }
    }
}
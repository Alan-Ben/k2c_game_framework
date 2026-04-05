using Common.QuestObj;

namespace GOE
{
    // 任务的计数对象
    public class QuestCountItem
    {
        //任务id 
        private long _m_questId;

        //完成的次数
        private long _m_doneCount;

        //构造函数
        public QuestCountItem(Quest_Count _info)
        {
            if (null == _info)
                return;

            _m_questId = _info.getQuestId();

            _m_doneCount = _info.getDoneCount();

        }

        //任务id 
        public long questId { get { return _m_questId; } }


        /// <summary>
        /// 更新数据
        /// </summary>
        public void update(Quest_Count _info)
        {
            if (null == _info)
                return;

            if (_m_doneCount != _info.getDoneCount())
            {
                _m_doneCount = _info.getDoneCount();
                WinMsg.SendMsg(WinMsgType.QUEST_DONE_COUNT_CHG);
            }
        }

        /// <summary>
        /// 判断任务完成次数是否达到上限
        /// </summary>
        public bool questDoneCountIsMax()
        {
            QuestRefObj questRef = GRefdataCoreMgr.instance.questMap.getRef(_m_questId);

            //完成次数配置小于等于0 表示无上限
            if (null == questRef || questRef.done_count <= 0)
                return false;

            return _m_doneCount >= questRef.done_count;
        }

        /// <summary>
        /// 根据类型获取任务计数
        /// </summary>
        /// <param name="_countType"></param>
        /// <returns></returns>
        public long getCount()
        {
            return _m_doneCount;
        }
    }
}

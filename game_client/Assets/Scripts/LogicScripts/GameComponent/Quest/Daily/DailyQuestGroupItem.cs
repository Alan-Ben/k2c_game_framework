using System.Collections.Generic;
using Common.QuestEnum;
using Common.QuestObj;
using JetBrains.Annotations;

namespace GOE
{
    // 日常 周常任务组
    public class DailyQuestGroupItem
    {

        //任务类型
        private EDailyQuestType _m_type;

        //刷新序列号
        private long _m_refreshSerial;

        //刷新时间戳
        private long _m_nextRefreshTimeMs;

        //任务列表 包括固定和随机
        [NotNull] private readonly List<DailyQuestItem> _m_questList;

        //已领取活跃度奖励id
        private List<long> _m_hasTakenActiveRewardRefIdList;

        //活跃度奖励列表
        [NotNull] private List<DailyQuestActiveRewardRefObj> _m_activeRewardList;

        //活跃度奖励最大配置
        [NotNull] private DailyQuestActiveRewardRefObj _m_maxActiveRewardRef;

        //构造函数
        public DailyQuestGroupItem(DailyQuest_Group _group)
        {
            if (null == _group)
                return;

            _m_type = _group.getType();
            _m_refreshSerial = _group.getRefreshSerial();
            _m_nextRefreshTimeMs = _group.getNextFreshTimeMs();
            _m_hasTakenActiveRewardRefIdList = _group.getHasTakenActiveRewardRefIdList();

            _m_activeRewardList = new List<DailyQuestActiveRewardRefObj>();

            //任务列表
            _m_questList = new List<DailyQuestItem>();

            List<DailyQuest_Info> questInfoList = _group.getQuestInfoList();
            if (null != questInfoList && questInfoList.Count > 0)
            {
                DailyQuest_Info temp = null;
                for (int i = 0; i < questInfoList.Count; i++)
                {
                    temp = questInfoList[i];
                    if (null == temp)
                        continue;

                    DailyQuestItem item = new DailyQuestItem(temp, _m_type);
                    _m_questList.Add(item);
                };
            }


            List<DailyQuest_Info> randomQuestInfoList = _group.getRandomQuestInfoList();
            if (null != randomQuestInfoList && randomQuestInfoList.Count > 0)
            {
                DailyQuest_Info temp = null;
                for (int i = 0; i < randomQuestInfoList.Count; i++)
                {
                    temp = randomQuestInfoList[i];
                    if (null == temp)
                        continue;

                    DailyQuestItem item = new DailyQuestItem(temp, _m_type, true);
                    _m_questList.Add(item);

                };
            }

            //根据id排序
            _m_questList.Sort((x, y) => x.dailyQuestId.CompareTo(y.dailyQuestId));

            //活跃度奖励配表数据
            List<DailyQuestActiveRewardRefObj> rewardList = GRefdataCoreMgr.instance.dailyQuestRewardListCore.refList;

            long maxId = 0;
            if (null != rewardList)
            {
                _m_activeRewardList.Clear();

                DailyQuestActiveRewardRefObj tempRef = null;
                for (int i = 0; i < rewardList.Count; i++)
                {
                    tempRef = rewardList[i];
                    if (null == tempRef)
                        continue;

                    if (tempRef.daily_quest_type == type)
                    {
                        _m_activeRewardList.Add(tempRef);
                        if (tempRef.id > maxId)
                        {
                            maxId = tempRef.id;
                            _m_maxActiveRewardRef = tempRef;
                        }
                    }
                }
                //排序
                _m_activeRewardList.Sort((x, y) => x.id.CompareTo(y.id));
            }

        }

        public EDailyQuestType type { get { return _m_type; } }

        public long refreshSerial { get { return _m_refreshSerial; } }

        public long nextRefreshTimeMs { get { return _m_nextRefreshTimeMs; } }

        public List<long> hasTakenActiveRewardRefIdList { get { return _m_hasTakenActiveRewardRefIdList; } }

        public List<DailyQuestActiveRewardRefObj> activeRewardList { get { return _m_activeRewardList; } }

        public DailyQuestActiveRewardRefObj maxActiveRewardRef { get { return _m_maxActiveRewardRef; } }

        public void setHasTakenActiveRewardRefIdList(List<long> _list)
        {
            if (null == _list)
                return;

            _m_hasTakenActiveRewardRefIdList = _list;
        }

        /// <summary>
        /// 获取所有任务列表
        /// </summary>
        public void getAllQuestList(List<DailyQuestItem> _list)
        {
            if (null == _list)
                return;

            _list.AddRange(_m_questList);
        }

        /// <summary>
        /// 获取所有可以展示的任务列表
        /// </summary>
        /// <param name="_list"></param>
        public void getAllShowQuestList(List<DailyQuestItem> _list)
        {
            if (null == _list)
                return;

            for (int i = 0; i < _m_questList.Count; i++)
            {
                if(_m_questList[i] != null && _m_questList[i].dailyQuestRef != null && (_m_questList[i].dailyQuestRef.show_cond == null || _m_questList[i].dailyQuestRef.show_cond.IsEnable(null)) )
                    _list.Add(_m_questList[i]);
            }
        }

        //更新任务
        public void updateQuestInfo(DailyQuest_Info _info)
        {
            if (_m_questList.Count > 0)
            {
                DailyQuestItem temp = null;
                for (int i = 0; i < _m_questList.Count; i++)
                {
                    temp = _m_questList[i];
                    if (null == temp)
                        continue;

                    if (temp.dailyQuestId == _info.getTargetId())
                    {
                        temp.update(_info);
                        return;
                    }
                }
            }
        }

        //更新任务组
        public void updateQuestGroup(DailyQuest_Group _group)
        {
            if (null == _group)
                return;

            _m_refreshSerial = _group.getRefreshSerial();
            _m_nextRefreshTimeMs = _group.getNextFreshTimeMs();
            _m_hasTakenActiveRewardRefIdList = _group.getHasTakenActiveRewardRefIdList();

            //更新任务列表
            _m_questList.Clear();

            List<DailyQuest_Info> questInfoList = _group.getQuestInfoList();
            if (null != questInfoList && questInfoList.Count > 0)
            {
                DailyQuest_Info temp = null;
                for (int i = 0; i < questInfoList.Count; i++)
                {
                    temp = questInfoList[i];
                    if (null == temp)
                        continue;

                    DailyQuestItem item = new DailyQuestItem(temp, _m_type);
                    _m_questList.Add(item);
                };
            }

            List<DailyQuest_Info> randomQuestInfoList = _group.getRandomQuestInfoList();
            if (null != randomQuestInfoList && randomQuestInfoList.Count > 0)
            {
                DailyQuest_Info temp = null;
                for (int i = 0; i < randomQuestInfoList.Count; i++)
                {
                    temp = randomQuestInfoList[i];
                    if (null == temp)
                        continue;

                    DailyQuestItem item = new DailyQuestItem(temp, _m_type, true);
                    _m_questList.Add(item);
                };
            }
        }

        /// <summary>
        /// 获取当前可以领取的任务id
        /// </summary>
        public void getCanFinishQuestList(List<long> _list)
        {
            DailyQuestItem temp = null;
            for (int i = 0; i < _m_questList.Count; i++)
            {
                temp = _m_questList[i];
                if (null == temp)
                    continue;

                if (temp.canFinish && GCommon.isSimpleUnlock(temp.dailyQuestRef.simple_unlock_id))
                    _list.Add(temp.dailyQuestId);
            }
        }

    }
}

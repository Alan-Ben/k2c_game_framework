using System;
using System.Collections.Generic;
using ALPackage;
using SQLite4Unity3d;

namespace GOE
{
    /// <summary>
    /// 通用事件-派遣事件实例表
    /// </summary>
    [Serializable]
    public class CommonEventDispatchRefObj : _ICommonEventInstanceSubRefObj
    {
        [Ignore]
        public Common.EventEnum.ECommonEventType eventType { get { return Common.EventEnum.ECommonEventType.DISPATCH; } }

        public long id;//唯一id
        public long Id { get { return id; } set { id = value; } }

        
        public long dispatch_show_id;//展示id
        public long dispatchShowId { get { return dispatch_show_id; } set { dispatch_show_id = value; } }

        
        public string condition_id_list;//条件列表
        public string conditionIdList { get { return condition_id_list; } set { condition_id_list = value; } }
        private List<long> _m_lConditionIdList;
        [Ignore]
        public List<long> lConditionIdList
        {
            get
            {
                if (_m_lConditionIdList == null)
                    _m_lConditionIdList = ALCommon.ParseLongList(condition_id_list, "condition_id_list");
                return _m_lConditionIdList;
            }
        }

        public string dispatch_result_id_list;//派遣结果id列表
        public string dispatchResultIdList { get { return dispatch_result_id_list; } set { dispatch_result_id_list = value; } }
        private List<long> _m_lDispatchResultIdList;
        [Ignore]
        public List<long> lDispatchResultIdList
        {
            get
            {
                if (_m_lDispatchResultIdList == null)
                    _m_lDispatchResultIdList = ALCommon.ParseLongList(dispatch_result_id_list, "dispatch_result_id_list");
                return _m_lDispatchResultIdList;
            }
        }
        
        public string event_reward_list;//事件奖励id列表
        public string eventRewardList { get { return event_reward_list; } set { event_reward_list = value; } }
        private List<long> _m_lEventRewardList;
        [Ignore]
        public List<long> lEventRewardList
        {
            get
            {
                if (_m_lEventRewardList == null)
                    _m_lEventRewardList = ALCommon.ParseLongList(event_reward_list, "event_reward_list");
                return _m_lEventRewardList;
            }
        }
        
        
        public int hero_num;//可派遣骑士数量
        public int heroNum { get { return hero_num; } set { hero_num = value; } }

        public static string assetPath { get { return "refdata_db/common_event.unity3d"; } }
        public static string objName { get { return "refdata_db/common_event_dispatch.txt"; } }
        public static string tableName { get { return "common_event_dispatch"; } }
    }
}
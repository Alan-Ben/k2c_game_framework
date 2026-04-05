using System;
using System.Collections.Generic;
using ALPackage;
using SQLite4Unity3d;

namespace GOE
{
    /// <summary>
    /// 通用事件-选择事件实例表
    /// </summary>
    [Serializable]
    public class CommonEventChoiceRefObj : _ICommonEventInstanceSubRefObj
    {
        [Ignore]
        public Common.EventEnum.ECommonEventType eventType { get { return Common.EventEnum.ECommonEventType.CHOICE; } }

        public long id;//唯一id
        public long Id { get { return id; } set { id = value; } }

        
        public long choice_show_id;//表现id
        public long choiceShowId { get { return choice_show_id; } set { choice_show_id = value; } }

        
        public string option_id_list;//选项列表
        public string optionIdList { get { return option_id_list; } set { option_id_list = value; } }
        private List<long> _m_lOptionIdList;//选项列表
        [Ignore]
        public List<long> lOptionIdList
        {
            get
            {
                if (_m_lOptionIdList == null)
                    _m_lOptionIdList = ALCommon.ParseLongList(option_id_list, "option_id_list");

                return _m_lOptionIdList;
            }
        }
        
        public string event_reward_id_list;//事件奖励id列表
        public string eventRewardIdList { get { return event_reward_id_list; } set { event_reward_id_list = value; } }
        private List<long> _m_lEventRewardIdList;
        [Ignore]
        public List<long> lEventRewardIdList
        {
            get
            {
                if (_m_lEventRewardIdList == null)
                    _m_lEventRewardIdList = ALCommon.ParseLongList(event_reward_id_list, "event_reward_id_list");

                return _m_lEventRewardIdList;
            }
        }
        
        public static string assetPath { get { return "refdata_db/common_event.unity3d"; } }
        public static string objName { get { return "refdata_db/common_event_choice.txt"; } }
        public static string tableName { get { return "common_event_choice"; } }
    }
}
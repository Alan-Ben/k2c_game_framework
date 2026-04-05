using System;
using System.Collections.Generic;
using ALPackage;
using SQLite4Unity3d;

namespace GOE
{
    /// <summary>
    /// 通用事件-对话事件实例表
    /// </summary>
    [Serializable]
    public class CommonEventDialogRefObj : _ICommonEventInstanceSubRefObj
    {
        [Ignore]
        public Common.EventEnum.ECommonEventType eventType { get { return Common.EventEnum.ECommonEventType.DIALOG; } }

        public long id;//唯一id
        public long Id { get { return id; } set { id = value; } }

        
        public long event_reward_id;//事件奖励id
        public long eventRewardId { get { return event_reward_id; } set { event_reward_id = value; } }
        

        public string event_name;//事件名称
        public string eventName { get { return event_name; } set { event_name = value; } }

        
        public string event_simple_desc;//事件简易描述
        public string eventSimpleDesc { get { return event_simple_desc; } set { event_simple_desc = value; } }

        
        public string event_list_icon;//在事件列表中icon
        public string eventListIcon { get { return event_list_icon; } set { event_list_icon = value; } }
        private NPGTextureIndex _m_lEventListIcon;//在事件列表中icon
        [Ignore]
        public NPGTextureIndex lEventListIcon
        {
            get
            {
                if (_m_lEventListIcon == null)
                    _m_lEventListIcon = NPGTextureIndex.readIndexInfo(event_list_icon);

                return _m_lEventListIcon;
            }
        }
        
        
        public string event_detail_desc;//事件详细描述
        public string eventDetailDesc { get { return event_detail_desc; } set { event_detail_desc = value; } }

        
        public string event_result_title;//事件处理结果标题
        public string eventResultTitle { get { return event_result_title; } set { event_result_title = value; } }

        
        public string event_result_desc;//事件处理结果描述
        public string eventResultDesc { get { return event_result_desc; } set { event_result_desc = value; } }

        
        public static string assetPath { get { return "refdata_db/common_event.unity3d"; } }
        public static string objName { get { return "refdata_db/common_event_dialog.txt"; } }
        public static string tableName { get { return "common_event_dialog"; } }
    }
}
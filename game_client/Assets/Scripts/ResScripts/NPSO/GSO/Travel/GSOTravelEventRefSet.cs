using System;
using System.Collections.Generic;
using ALPackage;
using Common.TravelEnum;
using NPEnum;
using SQLite4Unity3d;

namespace GOE
{
    /// <summary>
    /// 游历事件表
    /// </summary>
    [Serializable]
    public class TravelEventRefObj
    {
        public long event_id;//唯一id
        public long Id { get { return event_id; } set { event_id = value; } }
		
        public string event_type;//事件类型
        public string eventType { get { return event_type; } set { event_type = value; } }
        private bool _m_isInitEventType = false;
        private ETravelEventType _m_eEventType;
        [Ignore]
        public ETravelEventType eEventType
        {
            get
            {
                if (!_m_isInitEventType)
                {
                    ALCommon.TryEnumParse(typeof(ETravelEventType), event_type, out _m_eEventType);
                    _m_isInitEventType = true;
                }

                return _m_eEventType;
            }
        }

        public string dialog_list;//对话id列表(随机取一个生效)
        public string dialogList { get { return dialog_list; } set { dialog_list = value; } }
        private List<long> _m_lDialogList;
        [Ignore]
        public List<long> lDialogList
        {
            get
            {
                if (_m_lDialogList == null)
                    _m_lDialogList = ALCommon.ParseLongList(dialog_list, "dialog_list");

                return _m_lDialogList;
            }
        }
        
        public string target;//主体对象(类型:id)
        public string Target { get { return target; } set { target = value; } }
        private TravelEventRoleConfig _m_eventTarget;
        [Ignore]
        public TravelEventRoleConfig eventTarget
        {
            get
            {
                if (_m_eventTarget == null)
                    _m_eventTarget = TravelEventRoleConfig.readFromStr(target);

                return _m_eventTarget;
            }
        }

        public string effective_condition;//生效条件
        public string effectiveCondition { get { return effective_condition; } set { effective_condition = value; } }
        private _NPPlayerConditionSerializeInfo _m_effectiveCondition;//生效条件
        [Ignore]
        public _NPPlayerConditionSerializeInfo effectiveConditionInfo
        {
            get
            {
                if (_m_effectiveCondition == null)
                    _m_effectiveCondition = _NPPlayerConditionSerializeInfo.ReadFromString(effective_condition);
                return _m_effectiveCondition;
            }
        }

        public string pos_win_show_condition;//窗口展示条件
        public string posWinShowCondition { get { return pos_win_show_condition; } set { pos_win_show_condition = value; } }
        private _NPPlayerConditionSerializeInfo _m_posWinShowCondition;
        [Ignore]
        public _NPPlayerConditionSerializeInfo posWinShowConditionInfo
        {
            get
            {
                if (_m_posWinShowCondition == null)
                    _m_posWinShowCondition = _NPPlayerConditionSerializeInfo.ReadFromString(pos_win_show_condition);
                return _m_posWinShowCondition;
            }
        }
        
        public string event_info_desc;//事件信息描述
        public string eventInfoDesc { get { return event_info_desc; } set { event_info_desc = value; } }

        public string event_result_desc;//事件处理结果描述
        public string eventResultDesc { get { return event_result_desc; } set { event_result_desc = value; } } //事件处理结果描述

        public string event_result_desc_args;//事件处理结果描述参数
        public string eventResultDescArgs { get { return event_result_desc_args; } set { event_result_desc_args = value; } }
        private List<string> _m_lEventResultDescArgs;
        [Ignore]
        public List<string> lEventResultDescArgs
        {
            get
            {
                if (_m_lEventResultDescArgs == null)
                    _m_lEventResultDescArgs = ALCommon.ParseStringList(event_result_desc_args, "event_result_desc_args");

                return _m_lEventResultDescArgs;
            }
        }
        
        public static string assetPath { get { return "refdata_db/travel.unity3d"; } }
        public static string objName { get { return "refdata_db/travel_event.txt"; } }
        public static string tableName { get { return "travel_event"; } }
    }
}
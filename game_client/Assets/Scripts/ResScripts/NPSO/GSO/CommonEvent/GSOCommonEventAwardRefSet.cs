using System;
using ALPackage;
using Common.EventEnum;
using SQLite4Unity3d;

namespace GOE
{
    /// <summary>
    /// 通用事件-奖励事件实例表
    /// </summary>
    [Serializable]
    public class CommonEventAwardRefObj : _ICommonEventInstanceSubRefObj
    {
        [Ignore]
        public ECommonEventType eventType { get { return ECommonEventType.AWARD; } }

        public long id;//唯一id
        public long Id { get { return id; } set { id = value; } }

        
        public long event_reward_id;//事件奖励id
        public long eventRewardId { get { return event_reward_id; } set { event_reward_id = value; } }

        
        public long award_show_id;//表现id
        public long awardShowId { get { return award_show_id; } set { award_show_id = value; } }
        
        public static string assetPath { get { return "refdata_db/common_event.unity3d"; } }
        public static string objName { get { return "refdata_db/common_event_award.txt"; } }
        public static string tableName { get { return "common_event_award"; } }
    }
}
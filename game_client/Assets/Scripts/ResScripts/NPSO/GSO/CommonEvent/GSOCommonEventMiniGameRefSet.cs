using System;
using ALPackage;
using Common.EventEnum;
using SQLite4Unity3d;

namespace GOE
{
    /// <summary>
    /// 通用事件-小游戏事件实例表
    /// </summary>
    [Serializable]
    public class CommonEventMiniGameRefObj : _ICommonEventInstanceSubRefObj
    {
        [Ignore]
        public ECommonEventType eventType { get { return ECommonEventType.MINI_GAME; } }

        public long _refId { get { return id; } }

        public long id;//小游戏事件唯一id
        public long Id { get { return id; } set { id = value; } }

        public long event_reward_id;//事件奖励id
        public long eventRewardId { get { return event_reward_id; } set { event_reward_id = value; } }

        public long mini_game_show_id;//表现id
        public long miniGameShowId { get { return mini_game_show_id; } set { mini_game_show_id = value; } }
        
        public static string assetPath { get { return "refdata_db/common_event.unity3d"; } }
        public static string objName { get { return "refdata_db/common_event_mini_game.txt"; } }
        public static string tableName { get { return "common_event_mini_game"; } }
    }
}
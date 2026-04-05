namespace Hotfix
{
    /// <summary>
    /// 热更工程消息常量
    /// </summary>
    public class HotfixMsgType
    {
        public const int NONE = -1;
        public const int ON_REGULAR_EVENT_SHOP_ITEM_CHG = 1001;//万能活动商店购买记录变更
        public const int ON_REGULAR_EVENT_SHOP_ITEM_REFRESH = 1002;//万能活动商店刷新
        
        public const int GET_TILEMATCH_LOGIC_PROCESS = 2001; // 三消逻辑处理
        public const int GET_TILEMATCH_TASK_CHG = 2002; // 三消任务数据变更推送
        public const int ON_TILEMATCH_TASK_INFO_CHG = 2003; // 三消客户端任务数据变更
        public const int ON_NEW_TILEMATCH_TASK_INFO = 2004; // 三消客户端新任务数据
        public const int ON_A_TILEMATCH_LOGIC_PROCESS_DEAL_DONE = 2005; // 当一个三消逻辑过程处理完成
        public const int ON_TILEMATCH_STEP_REWARD_INFO_CHG = 2006;// 三消阶段奖励数据变更
        public const int ON_TILEMATCH_CAN_DRAW_STEP_REWARD_INFO_CHG = 2007;// 三消可领取阶段奖励数据变更
        public const int ON_TILEMATCH_SAME_SERIALIZE_LOGIC_PROCESS_START_DEAL = 2008; // 三消当一个同一序列化的逻辑过程开始处理
        public const int ON_TILEMATCH_SAME_SERIALIZE_LOGIC_PROCESS_DEAL_DONE = 2009; // 三消当一个同一序列化的逻辑过程处理完成
        public const int ON_TILEMATCH_ACTIVITY_TOTAL_SCORE_CHG = 2010; // 三消活动总分变更
        
        public const int SHOW_NUMMERGE_USE_ITEM_TIP = 3001; // 显示2048使用道具提示
    }
}
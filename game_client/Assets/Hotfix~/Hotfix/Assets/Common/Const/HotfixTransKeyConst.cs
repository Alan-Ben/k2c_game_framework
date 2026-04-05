namespace Hotfix
{
    /// <summary>
    /// 翻译KEY常量
    /// </summary>
    public class HotfixTransKeyConst
    {
        /*
         * 1、普通翻译KEY和主工程一样
         * 2、需要换皮的翻译KEY需要拼接活动id，格式为：#1_{0}_xxx_xxx_xxx，在调用翻译时，使用replaceActivity方法传入活动id
         *    例：
         *      定义翻译KEY常量
         *      public const string activity_test_none = "#1_{0}_activity_test_none";
         *      调用翻译
         *      TextTranslate.instance.getLanguage(HotfixTransKeyConst.activity_test_none.replaceActivity(_m_lActivityId));
         */

        #region 万能活动
        public const string regularEvent_myRank_num = "#1_{0}_regularEvent_myRank_num";//我的排名：{0}
        public const string regularEvent_addScore_num = "#1_{0}_regularEvent_addScore_num";//积分+{0}
        #endregion

        public const string tilematch_diedTipDesc_none = "#1_tilematch_diedTipDesc_none";
        public const string tilematch_diedTipTitle_none = "#1_tilematch_diedTipTitle_none";
        public const string tilematch_noGameModeCanPlatTip_none = "#1_tilematch_noGameModeCanPlatTip_none";//没有可玩的游戏模式
        public const string tilematch_taskLeftStep_num2 = "#1_tilematch_taskLeftStep_num2";//剩余步数：{0}/{1}
        public const string tilematch_taskAddScore_num = "#1_tilematch_taskAddScore_num";//分数+{0}
        public const string tilematch_stepRewardScoreProgress_num2 = "#1_tilematch_stepRewardScoreProgress_num2";//分数：{0}/{1}
        public const string tilematch_activityTotalScore_num = "#1_tilematch_activityTotalScore_num";//分数：{0}
        public const string tilematch_stepRewardCannotDrawTip_none = "#1_tilematch_stepRewardCannotDrawTip_none";//距离下一个奖励还有{0}积分

        public const string numMerge_modeLockByTotalScore_count = "#1_numMerge_modeLockByTotalScore_count"; //达到{0}积分解锁该模式
        public const string numMerge_boxScoreNotEnough_count = "#1_numMerge_boxScoreNotEnough_count"; //还差{0}积分可开启宝箱
        public const string numMerge_highestScore_value = "#1_numMerge_highestScore_value"; //最高分数：{0}
        public const string numMerge_totalScore_value = "#1_numMerge_totalScore_value"; //累计得分：{0}
        public const string numMerge_rank_value = "#1_numMerge_rank_value"; //排名：{0}
        public const string numMerge_scoreGap_value = "#1_numMerge_scoreGap_value"; //距离上一位排名：{0}
        public const string numMerge_boxSld_value2 = "#1_numMerge_boxSld_value2";
        public const string numMerge_boxRewardQualityNameAndProbability_name_percent = "#1_numMerge_boxRewardQualityNameAndProbability_name_percent";//{0} {1}
        public const string numMerge_notMove_tip = "#1_numMerge_notMove_tip";//无法移动，请尝试其他方向
        public const string numMerge_gameOverConfirm_title = "#1_numMerge_gameOverConfirm_title"; //友情提示
        public const string numMerge_gameOverConfirm_desc = "#1_numMerge_gameOverConfirm_desc"; //没有相邻的棋子可合成，是否需要使用道具或者重新开始？
        public const string numMerge_gameOverConfirm_useItem = "#1_numMerge_gameOverConfirm_useItem"; //使用道具
        public const string numMerge_gameOverConfirm_restart = "#1_numMerge_gameOverConfirm_restart"; //重新开始

        public const string numMerge_useRefresh_des = "#1_numMerge_useRefresh_des";//使用重排道具二次确认弹窗描述
        public const string numMerge_useRefresh_title = "#1_numMerge_useRefresh_title";//使用重排道具二次确认弹窗标题
        public const string numMerge_useRefreshItemSucc_tip = "#1_numMerge_useRefreshItemSucc_tip";//使用重排道具成功提示
    }
}
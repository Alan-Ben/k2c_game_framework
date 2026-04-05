using System;
using System.Collections.Generic;
using ALPackage;
using System.Text;
using UnityEngine;
using LitJson;


namespace GOE
{
    [System.Serializable]
    public class AsscountSettingData
    {
        public long viewChildMainTimeS;//设置查看子嗣主界面的时间戳
        public long followingQuestId;//跟踪的任务ID
        public int rankLikeTimeS;//玩家点赞的时间戳
        public int followingQuestType;//当前跟踪的任务类型（ENPFollowQuestType）
        public bool isMissionAutoBattle;//关卡是否自动战斗
        public int missionBattleSpeed;//关卡战斗速度
        public long dailyCheckShowTimeS;//签到弹窗的弹出时间戳
        /// <summary>
        /// 妃子是否选中一键约会
        /// </summary>
        public bool isAkeyGreeting;
        public List<long> collegeLastSelectHeroIdList; //大学上一次选择的骑士id列表
        public List<heroRecommendFinishDialogueData> heroRecommendFinishDialogueList;//骑士推荐的骑士是否看完了对话列表
        public long autoShowQuestSortId;//自动展示过的主线任务排序id
        public List<int> autoShowFuncTypeList;//自动展示过的系统功能类型(ENPFunctionType)
        public long friendApplyShowTimeS;//好友申请查看时间戳
        public int beProposeShowTimeS;//被求婚弹窗的弹出时间戳
        public long friendVisitPlayerId;//好友拜访玩家id
        public int weekCardAssignTipNoShowTimeS;//周卡委派未设置提醒不提示时间戳
        /// <summary>
        /// 是否选中一键游历
        /// </summary>
        public bool isAllTravel;
        public bool isShowAnecdoteUnlockAni;//是否已经展示过一键政务解锁动画
        public List<ESideBarType> spreadSideBarTypeList;//记录展开的sidebar类型列表
        public EHeroListSortTabType heroListSortType;//伙伴列表展示排序方式
        public bool isChapterQuickForward;//是否章节快进
        
        public bool isChildOneKeyPlusEducating;//是否开启了子嗣一键上课进阶功能
        public bool isChildOneKeyEducating;//是否开启了子嗣一键上课功能
        public bool isChildOneKeyNaming;//是否开启了子嗣一键起名功能
        public bool isChildOneKeyGraduating;//是否开启了子嗣一键毕业功能
        public bool isBuildingHiringTenTimes;//是否勾选十连招聘

        public bool arenaSetAutoGetRoundReward;//竞技场自动随机翻开连胜奖励
        public bool arenaSetSkipBattle;//竞技场跳过单场战斗动画
        public bool arenaSetAutoBuyBuffByCrystal;//竞技场自动购买水晶临时增益
        public bool arenaSetAutoBuyBuffByTwoCoin;//竞技场自动购买2银币临时增益
        public bool arenaSetAutoBuyBuffByOneCoin;//竞技场自动购买1银币临时增益
        public bool arenaSetNotToBuyBuff;//竞技场不购买增益
        public bool arenaOneKeySetAutoBuyBuffByCrystal;//竞技场一键谈判自动购买水晶临时增益
        public bool arenaOneKeySetAutoBuyBuffByTwoCoin;//竞技场一键谈判自动购买2银币临时增益
        public bool arenaOneKeySetAutoBuyBuffByOneCoin;//竞技场一键谈判自动购买1银币临时增益
        public bool arenaOneKeySetNotToBuyBuff;//竞技场一键谈判不购买增益
        public string arenaFightBotName;//竞技场战斗机器人名字
        public long arenaFightBotLevel;//竞技场战斗机器人等级
        public bool isInnOneKeyCreateGuest;//是否开启了一键迎宾功能

        public bool guildAutoDealEntrust;//公会自动处理委托
        
        public bool towerBattleSpeedTenTimes;//爬塔战斗速度10倍开关
        public bool towerSkipBattleShow;//爬塔跳过战斗过程开关
        public bool guildDungeonAutoBattle;//公会副本自动战斗开关

        public List<long> alreadyShowGainConsortList;//已经展示过的获得的妃子列表
        public List<RankRushNewTipRecordData> rankRushNewTipRecordList;//记录新冲榜数据列表

        public bool guildAnnouncementIsClose;//联盟公告是否收起

        public CDEventRecordData cdEventRecordData;//倒计时事件记录数据
        public StageGoalCompletedTaskRecord stageGoalCompletedTaskRecord;//已完成的阶段目标任务记录
        public List<long> alreadyReadRedTipStageTaskIdList;//已看过红点的阶段子任务id列表
        
        public long alreadyShowUnlockAniBigStageGoalId;//已经展示过解锁动画的大阶段目标id
        public List<long> alreadyShowFinishAniBigStageGoalIdList;//已经展示过完成动画的大阶段目标id列表 
        public bool isHeroTenUpgrade;//顾问是否开启十连升级功能
        public bool isHeroTalentTenUpgrade;//顾问是否使用十连升级资质
        public bool isEquipTenUpgrade;//藏品是否使用十连升级
        public bool isChapterQuickForwardSkipDialog;//关卡快速前进是否跳过对话
        public List<long> alreadyReadGuildRedTipIdList;//已读的联盟红点id列表
        public string guildCooperateAreaUnlockRedTipRecord;//联盟协作区域解锁红点记录
        public long readMarsExplorePvPLogTime;//已读火星探索PVP日志的时间戳
        public NewestSharedMineData readMarsExploreSharedMineData;//已读火星探索共享矿数据
        public long readMarsExploreGuildBattleReportId;//已读火星探索联盟战报ID
        public long readMarsExploreGuildBattleReportGuildId;//已读火星探索联盟战报时所在的联盟ID
        public List<long> clickPlayerGainHeroJumpIdList;//点击玩家解锁顾问前往按钮的id列表
        public bool hadShowNationalPowerTargetUnlockRedTip;//是否已经显示过国力目标解锁红点
        public string nationalPowerTargetPopWndShowDate;//国力目标弹窗显示日期
        public string sevenDayLoginMainCityPopWndShowDate;//七日登录主城弹窗显示日期
        public string firstRechargeMainCityPopWndShowDate;//首充主城弹窗显示日期
        public SevenDayGoalsRedTipRecord sevenDayGoalsRedTipRecord;//七日目标红点相关记录
        public int shopAutoRefreshPopWndDate;//已展示商店自动刷新弹窗时间
        public long lastShareMarsMineToGuildChatTime;//上次分享火星矿到联盟聊天的时间
        public List<PlayerReportRecordTime> playerReportRecordTimeList;//玩家举报记录时间列表

        public AsscountSettingData() {
        
        }
    }

    /// <summary>
    /// 骑士推荐的骑士是否看完了对话
    /// </summary>
    [System.Serializable]
    public class heroRecommendFinishDialogueData
    {
        public long heroId;//骑士id
        public long dialogueId;//看完的对话id（可获得和不可获得是两个对话id）
    }

    /// <summary>
    /// 新冲榜记录数据
    /// </summary>
    [System.Serializable]
    public class RankRushNewTipRecordData
    {
        public long rankRushId;
        public long instanceId;
    }

    /// <summary>
    /// 倒计时事件记录数据
    /// </summary>
    [System.Serializable]
    public class CDEventRecordData
    {
        public long dbId;
        public long lastResetTimeMs;
    }

    /// <summary>
    /// 已完成的阶段目标任务记录
    /// </summary>
    [System.Serializable]
    public class StageGoalCompletedTaskRecord
    {
        public long stageGoalRefId;//stage_goal表id
        public List<long> completedTaskIdList;//已完成的任务id列表
    }

    /// <summary>
    /// 七日目标红点相关记录
    /// </summary>
    [System.Serializable]
    public class SevenDayGoalsRedTipRecord
    {
        public long activityInstanceId;//活动实例id
        public List<long> readDayList;//已读的解锁天数列表
        public List<SevenDayGoalsGiftReadRecord> giftReadRecordList;//七日目标礼包已读记录
    }

    /// <summary>
    /// 七日目标礼包已读记录
    /// </summary>
    [System.Serializable]
    public class SevenDayGoalsGiftReadRecord
    {
        public long day;//天数
        public int readData;//已读日期
    }

    /// <summary>
    /// 已举报玩家时间记录
    /// </summary>
    [System.Serializable]
    public class PlayerReportRecordTime
    {
        public long cid;//举报玩家cid
        public long reportTime;//举报时间戳秒
    }
}

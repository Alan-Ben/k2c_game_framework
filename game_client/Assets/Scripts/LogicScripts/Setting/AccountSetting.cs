using System;
using ALPackage;
using System.Text;
using UnityEngine;
using LitJson;
using System.Collections.Generic;
using System.Linq;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 账号相关的本地保存,
    /// </summary>
    public class AccountSetting : _AALBasicSettingInfo
    {

        private AsscountSettingData _m_settingData;

        public AccountSetting(long _accountCID)
            : base(string.Format("{0}_cache_acctount_setting", _accountCID))
        {
            _m_settingData = new AsscountSettingData();
        }

        public const char strListSplit = ';';//列表分隔符

        //查看子嗣主界面时间戳
        public long viewChildMainTimeS { get { return _m_settingData.viewChildMainTimeS; } }

        //当前跟踪的任务类型
        public ENPFollowQuestType followingQuestType { get { return (ENPFollowQuestType)_m_settingData.followingQuestType; } }
        
        //当前跟踪的任务id
        public long followingQuestId { get { return _m_settingData.followingQuestId; } }

        //点赞时间戳
        public int rankLikeTimeS { get { return _m_settingData.rankLikeTimeS; } }

        //关卡是否自动战斗
        public bool isMissionAutoBattle { get { return _m_settingData.isMissionAutoBattle; } }
        //关卡战斗速度
        public int missionBattleSpeed { get { return _m_settingData.missionBattleSpeed; } }

        //自动展示过的主线任务排序id
        public long autoShowQuestSortId { get { return _m_settingData.autoShowQuestSortId; } }

        //大学上一次选择的骑士id列表
        public List<long> collegeLastSelectHeroIdList { get { return _m_settingData.collegeLastSelectHeroIdList; } }
        public bool isAkeyGreeting { get { return _m_settingData.isAkeyGreeting; } }
        public bool isAllTravel { get { return _m_settingData.isAllTravel; } }
        public long friendApplyShowTimeS { get { return _m_settingData.friendApplyShowTimeS; } }
        public bool isChapterQuickForward { get { return _m_settingData.isChapterQuickForward; } }

        //被求婚弹窗的弹出时间戳
        public int beProposeShowTimeS { get { return _m_settingData.beProposeShowTimeS; } }
        public int weekCardAssignTipNoShowTimeS { get { return _m_settingData.weekCardAssignTipNoShowTimeS; } }

        //好友拜访id
        public long friendVisitPlayerId { get { return _m_settingData.friendVisitPlayerId; } }
        //是否已经展示过一键政务解锁动画
        public bool isShowAnecdoteUnlockAni { get { return _m_settingData.isShowAnecdoteUnlockAni; } }
        //伙伴列表展示排序方式
        public EHeroListSortTabType heroListSortType { get { return _m_settingData.heroListSortType; } }
        /// <summary>
        /// 是否开启了子嗣一键上课进阶功能
        /// </summary>
        public bool isChildOneKeyPlusEducating { get { return _m_settingData.isChildOneKeyPlusEducating; } }
        /// <summary>
        /// 是否开启了子嗣一键上课功能
        /// </summary>
        public bool isChildOneKeyEducating { get { return _m_settingData.isChildOneKeyEducating; } }
        /// <summary>
        /// 是否开启了子嗣一键起名功能
        /// </summary>
        public bool isChildOneKeyNaming { get { return _m_settingData.isChildOneKeyNaming; } }
        /// <summary>
        /// 是否开启了子嗣一键毕业功能
        /// </summary>
        public bool isChildOneKeyGraduating { get { return _m_settingData.isChildOneKeyGraduating; } }
        
        public bool isBuildingHiringTenTimes { get { return _m_settingData.isBuildingHiringTenTimes; } }
        /// <summary>
        /// 竞技场自动随机翻开连胜奖励
        /// </summary>
        public bool arenaSetAutoGetRoundReward { get { return _m_settingData.arenaSetAutoGetRoundReward; } }
        /// <summary>
        /// 竞技场跳过单场战斗动画
        /// </summary>
        public bool arenaSetSkipBattle { get { return _m_settingData.arenaSetSkipBattle; } }
        /// <summary>
        /// 竞技场自动购买水晶临时增益
        /// </summary>
        public bool arenaSetAutoBuyBuffByCrystal { get { return _m_settingData.arenaSetAutoBuyBuffByCrystal; } }
        /// <summary>
        /// 竞技场自动购买2银币临时增益
        /// </summary>
        public bool arenaSetAutoBuyBuffByTwoCoin { get { return _m_settingData.arenaSetAutoBuyBuffByTwoCoin; } }
        /// <summary>
        /// 竞技场自动购买1银币临时增益
        /// </summary>
        public bool arenaSetAutoBuyBuffByOneCoin { get { return _m_settingData.arenaSetAutoBuyBuffByOneCoin; } }
        /// <summary>
        /// 竞技场不购买增益
        /// </summary>
        public bool arenaSetNotToBuyBuff { get { return _m_settingData.arenaSetNotToBuyBuff; } }
        /// <summary>
        /// 竞技场一键谈判自动购买水晶临时增益
        /// </summary>
        public bool arenaOneKeySetAutoBuyBuffByCrystal { get { return _m_settingData.arenaOneKeySetAutoBuyBuffByCrystal; } }
        /// <summary>
        /// 竞技场一键谈判自动购买2银币临时增益
        /// </summary>
        public bool arenaOneKeySetAutoBuyBuffByTwoCoin { get { return _m_settingData.arenaOneKeySetAutoBuyBuffByTwoCoin; } }
        /// <summary>
        /// 竞技场一键谈判自动购买1银币临时增益
        /// </summary>
        public bool arenaOneKeySetAutoBuyBuffByOneCoin { get { return _m_settingData.arenaOneKeySetAutoBuyBuffByOneCoin; } }
        /// <summary>
        /// 竞技场一键谈判不购买增益
        /// </summary>
        public bool arenaOneKeySetNotToBuyBuff { get { return _m_settingData.arenaOneKeySetNotToBuyBuff; } }
        /// <summary>
        /// 竞技场战斗机器人名字
        /// </summary>
        public string arenaFightBotName { get { return _m_settingData.arenaFightBotName; } }
        /// <summary>
        /// 竞技场战斗机器人等级
        /// </summary>
        public long arenaFightBotLevel { get { return _m_settingData.arenaFightBotLevel; } }
        /// <summary>
        /// 公会自动处理委托
        /// </summary>
        public bool guildAutoDealEntrust { get { return _m_settingData.guildAutoDealEntrust; } }
        /// <summary>
        /// 爬塔战斗速度10倍开关
        /// </summary>
        public bool towerBattleSpeedTenTimes { get { return _m_settingData.towerBattleSpeedTenTimes; } }
        /// <summary>
        /// 爬塔战斗跳过战斗展示
        /// </summary>
        public bool towerSkipBattleShow { get { return _m_settingData.towerSkipBattleShow; } }
        /// <summary>
        /// 公会副本自动战斗开关
        /// </summary>
        public bool guildDungeonAutoBattle { get { return _m_settingData.guildDungeonAutoBattle; } }
        /// <summary>
        /// 联盟公告是否收起
        /// </summary>
        public bool guildAnnouncementIsClose { get { return _m_settingData.guildAnnouncementIsClose; } }
        /// <summary>
        /// 已经展示过的获得的妃子列表
        /// </summary>
        public List<long> alreadyShowGainConsortList { get { return _m_settingData.alreadyShowGainConsortList; } }
        /// <summary>
        /// 是否开启了一键迎宾功能
        /// </summary>
        public bool isInnOneKeyCreateGuest { get { return _m_settingData.isInnOneKeyCreateGuest; } }
        
        /// <summary>
        /// 已经展示过解锁动画的大阶段目标id
        /// </summary>
        public long alreadyShowUnlockAniBigStageGoalId { get { return _m_settingData.alreadyShowUnlockAniBigStageGoalId; } }
        /// <summary>
        /// 是否开启顾问十连升级功能
        /// </summary>
        public bool isHeroTenUpgrade { get { return _m_settingData.isHeroTenUpgrade; } }
        /// <summary>
        /// 是否使用十连升级资质
        /// </summary>
        public bool isHeroTalentTenUpgrade { get { return _m_settingData.isHeroTalentTenUpgrade; } }
        /// <summary>
        /// 藏品是否使用十连升级
        /// </summary>
        public bool isEquipTenUpgrade { get { return _m_settingData.isEquipTenUpgrade; } }
        /// <summary>
        /// 关卡快速前进是否跳过对话
        /// </summary>
        public bool isChapterQuickForwardSkipDialog { get { return _m_settingData.isChapterQuickForwardSkipDialog; } }
        /// <summary>
        /// 火星探险PVP日志已读时间戳
        /// </summary>
        public long readMarsExplorePvPLogTime { get { return _m_settingData.readMarsExplorePvPLogTime; } }
        /// <summary>
        /// 已读的分享矿的数据
        /// </summary>
        public NewestSharedMineData readMarsExploreSharedMineData { get { return _m_settingData.readMarsExploreSharedMineData; } }
        /// <summary>
        /// 已读联盟火星矿战报ID
        /// </summary>
        public long readMarsExploreGuildBattleReportId { get { return _m_settingData.readMarsExploreGuildBattleReportId; } }
        /// <summary>
        /// 已读联盟火星矿战报时所在的联盟ID
        /// </summary>
        public long readMarsExploreGuildBattleReportGuildId { get { return _m_settingData.readMarsExploreGuildBattleReportGuildId; } }

        /*************
         * 构建需要保存的字符串
         *
         **/
        protected override string _makeSettingStr()
        {
            return JsonUtility.ToJson(_m_settingData);
        }

        /**************
       * 读取保存的字符串
       **/
        protected override void _initSettingStr(string _infoStr)
        {
            if(string.IsNullOrEmpty(_infoStr))
                return;
            try
            {
                _m_settingData = JsonUtility.FromJson<AsscountSettingData>(_infoStr);
            }
            catch(Exception e)
            {
#if UNITY_EDITOR
                Debug.LogError($"账号相关本地保存反序列化失败:{_infoStr};/n Exception:{e}");
#endif
            }

        }

        public AsscountSettingData settingData
        {
            get { return _m_settingData; }
        }

        //设置当前查看子嗣房间的时间戳
        public void setViewChildMainTimeS(long _itemS)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.viewChildMainTimeS = _itemS;

            //保存到本地
            saveSetting();
        }

        //设置当前跟踪任务
        public void setFollowingQuest(ENPFollowQuestType _questType, long _id)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.followingQuestType = (int)_questType;
            _m_settingData.followingQuestId = _id;

            //保存到本地
            saveSetting();
        }

        //清空当前跟踪任务
        public void clearFollowingQuest()
        {
            if (null == _m_settingData)
                return;

            _m_settingData.followingQuestType = (int)ENPFollowQuestType.NONE;
            _m_settingData.followingQuestId = 0;

            //保存到本地
            saveSetting();
        }

        //设置点赞时间戳
        public void setRankLikeTimeS(int _itemS)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.rankLikeTimeS = _itemS;

            //保存到本地
            saveSetting();
        }

        //设置关卡是否自动战斗
        public void setIsMissionAutoBattle(bool _isAutoBattle)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.isMissionAutoBattle = _isAutoBattle;

            //保存到本地
            saveSetting();
        }
        
        //设置关卡战斗速度
        public void setMissionBattleSpeed(int _speed)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.missionBattleSpeed = _speed;

            //保存到本地
            saveSetting();
        }

        //好友申请查看时间戳
        public void setFriendApplyShowTimeS(long _timeS)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.friendApplyShowTimeS = _timeS;

            //保存到本地
            saveSetting();
        }
        
        public void setCollegeLastSelectHeroIdList(List<long> _collegeLastSelectHeroIdList)
        {
            if (null == _m_settingData)
                return;

            if (null == _m_settingData.collegeLastSelectHeroIdList)
                _m_settingData.collegeLastSelectHeroIdList = new List<long>();
            _m_settingData.collegeLastSelectHeroIdList.Clear();

            _m_settingData.collegeLastSelectHeroIdList.AddRange(_collegeLastSelectHeroIdList);

            //保存到本地
            saveSetting();
        }
        
        /// <summary>
        /// 设置是否一键游历
        /// </summary>
        /// <param name="_isAllTravel"></param>
        public void setIsAllTravel(bool _isAllTravel)
        {
            if(null == _m_settingData)
                return;
            _m_settingData.isAllTravel = _isAllTravel;
            // //需求是不保存，下次登录还是还是默认不选中
            saveSetting();
        }
        
        /// <summary>
        /// 设置是否一键游历
        /// </summary>
        /// <param name="_isAllTravel"></param>
        public void setIsChapterQuickForward(bool _isChapterQuickForward)
        {
            if(null == _m_settingData)
                return;
            _m_settingData.isChapterQuickForward = _isChapterQuickForward;
            // //需求是不保存，下次登录还是还是默认不选中
            saveSetting();
        }

        /// <summary>
        /// 设置是否一键宠信
        /// </summary>
        /// <param name="_isAkeyGreeting"></param>
        public void setIsAkeyGreeting(bool _isAkeyGreeting)
        {
            if(null == _m_settingData)
                return;
            _m_settingData.isAkeyGreeting = _isAkeyGreeting;
         
            saveSetting();
        }

        /// <summary>
        /// 是否发送一键宠信
        /// </summary>
        /// <returns></returns>
        public bool getIsAkeyGreeting()
        {
            if (null == _m_settingData)
                return false;

            return _m_settingData.isAkeyGreeting;
        }

        /// <summary>
        /// 记录骑士推荐看完对话
        /// </summary>
        /// <param name="_heroId">骑士id</param>
        /// <param name="_dialogueId">对话id</param>
        public void setHeroRecommendFinishDialogue(long _heroId, long _dialogueId)
        {
            if (null == _m_settingData)
                return;

            if (_m_settingData.heroRecommendFinishDialogueList == null)
                _m_settingData.heroRecommendFinishDialogueList = new List<heroRecommendFinishDialogueData>();

            bool isFind = false;
            foreach (heroRecommendFinishDialogueData finishDialogueData in _m_settingData.heroRecommendFinishDialogueList)
            {
                if (finishDialogueData != null && finishDialogueData.heroId == _heroId)
                {
                    isFind = true;
                    finishDialogueData.dialogueId = _dialogueId;
                    break;
                }
            }

            if (!isFind)
            {
                heroRecommendFinishDialogueData data = new heroRecommendFinishDialogueData();
                data.heroId = _heroId;
                data.dialogueId = _dialogueId;
                _m_settingData.heroRecommendFinishDialogueList.Add(data);
            }
            //保存到本地
            saveSetting();
        }

        /// <summary>
        /// 骑士推荐是否已经看完对话
        /// </summary>
        /// <param name="_heroId">骑士id</param>
        /// <param name="_dialogueId">对话id</param>
        /// <returns></returns>
        public bool isHeroRecommendFinishDialogue(long _heroId, long _dialogueId)
        {
            if (null == _m_settingData || _m_settingData.heroRecommendFinishDialogueList == null)
                return false;

            foreach (heroRecommendFinishDialogueData finishDialogueData in _m_settingData.heroRecommendFinishDialogueList)
            {
                if (finishDialogueData != null && finishDialogueData.heroId == _heroId)
                    return finishDialogueData.dialogueId == _dialogueId;
            }
            
            return false;
        }

        //设置自动展示过的主线任务排序id
        public void setAutoShowQuestSortId(long _sortId)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.autoShowQuestSortId = _sortId;

            //保存到本地
            saveSetting();
        }

        //是否可以自动展示系统功能解锁领取奖励弹窗
        public bool canAutoShowFuncType(ENPFunctionType _type)
        {
            if (null == _m_settingData)
                return false;

            return _m_settingData.autoShowFuncTypeList != null && !_m_settingData.autoShowFuncTypeList.Contains((int)_type);
        }

        //设置自动展示过的系统功能类型
        public void addAutoShowFuncType(ENPFunctionType _type)
        {
            if (null == _m_settingData)
                return;

            if (_m_settingData.autoShowFuncTypeList == null)
                _m_settingData.autoShowFuncTypeList = new List<int>();

            _m_settingData.autoShowFuncTypeList.Add((int)_type);

            //保存到本地
            saveSetting();
        }

        //设置被求婚弹窗时间戳
        public void setBeProposeTimeS(int _timeS)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.beProposeShowTimeS = _timeS;

            //保存到本地
            saveSetting();
        }
        

        //设置周卡委派未设置提醒不提示时间戳
        public void setWeekCardAssignTipNoShowTimeS(int _timeS)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.weekCardAssignTipNoShowTimeS = _timeS;

            //保存到本地
            saveSetting();
        }

        //设置好友拜访玩家id
        public void setFriendVisitPlayerId(long _playerId)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.friendVisitPlayerId = _playerId;
            //保存到本地
            saveSetting();
        }

        //是否已经展示过一键政务解锁动画
        public void setIsShowAnecdoteUnlockAni(bool _isShow)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.isShowAnecdoteUnlockAni = _isShow;
            //保存到本地
            saveSetting();
        }

        //记录展开的SideBar类型
        public void setSpreadSideBarType(ESideBarType _type)
        {
            if (null == _m_settingData)
                return;

            if (_m_settingData.spreadSideBarTypeList == null)
                _m_settingData.spreadSideBarTypeList = new List<ESideBarType>();

            if (!_m_settingData.spreadSideBarTypeList.Contains(_type))
            {
                _m_settingData.spreadSideBarTypeList.Add(_type);
                //保存到本地
                saveSetting();
            }
        }

        //移除记录展开的SideBar类型
        public void removeSpreadSideBarType(ESideBarType _type)
        {
            if (null == _m_settingData)
                return;

            if (_m_settingData.spreadSideBarTypeList == null)
                return;

            if (_m_settingData.spreadSideBarTypeList.Contains(_type))
            {
                _m_settingData.spreadSideBarTypeList.Remove(_type);
                //保存到本地
                saveSetting();
            }
        }

        //该SideBar类型是否已经展开
        public bool isSideBarSpread(ESideBarType _type)
        {
            if (null == _m_settingData)
                return false;

            if (_m_settingData.spreadSideBarTypeList == null)
                return false;

            return _m_settingData.spreadSideBarTypeList.Contains(_type);
        }

        /// <summary>
        /// 设置伙伴列表展示排序方式
        /// </summary>
        /// <param name="_type"></param>
        public void setHeroListSortType(EHeroListSortTabType _type)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.heroListSortType = _type;
            //保存到本地
            saveSetting();
        }
        public void setBuildingHiringTenTimes(bool _isBuildingHiringTenTimes)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.isBuildingHiringTenTimes = _isBuildingHiringTenTimes;
            //保存到本地
            saveSetting();
        }
        public void setChildOneKeyPlusEducating(bool _isOneKeyPlusEducating)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.isChildOneKeyPlusEducating = _isOneKeyPlusEducating;
            //保存到本地
            saveSetting();
        }
        public void setChildOneKeyEducating(bool _isOneKeyEducating)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.isChildOneKeyEducating = _isOneKeyEducating;
            //保存到本地
            saveSetting();
        }
        public void setChildOneKeyNaming(bool _isOneKeyNaming)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.isChildOneKeyNaming = _isOneKeyNaming;
            //保存到本地
            saveSetting();
        }
        public void setChildOneKeyGraduating(bool _isOneKeyGraduating)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.isChildOneKeyGraduating = _isOneKeyGraduating;
            //保存到本地
            saveSetting();
        }

        /// <summary>
        /// 设置竞技场自动随机翻开连胜奖励
        /// </summary>
        /// <param name="_isOn"></param>
        public void setArenaSetAutoGetRoundReward(bool _isOn)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.arenaSetAutoGetRoundReward = _isOn;
            //保存到本地
            saveSetting();
        }

        /// <summary>
        /// 设置竞技场跳过单场战斗动画
        /// </summary>
        /// <param name="_isOn"></param>
        public void setArenaSetSkipBattle(bool _isOn)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.arenaSetSkipBattle = _isOn;
            //保存到本地
            saveSetting();
        }

        /// <summary>
        /// 设置竞技场自动购买水晶临时增益
        /// </summary>
        /// <param name="_isOn"></param>
        public void setArenaSetAutoBuyBuffByCrystal(bool _isOn)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.arenaSetAutoBuyBuffByCrystal = _isOn;
            //保存到本地
            saveSetting();
        }

        /// <summary>
        /// 设置竞技场自动购买2银币临时增益
        /// </summary>
        /// <param name="_isOn"></param>
        public void setArenaSetAutoBuyBuffByTwoCoin(bool _isOn)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.arenaSetAutoBuyBuffByTwoCoin = _isOn;
            //保存到本地
            saveSetting();
        }

        /// <summary>
        /// 设置竞技场自动购买1银币临时增益
        /// </summary>
        /// <param name="_isOn"></param>
        public void setArenaSetAutoBuyBuffByOneCoin(bool _isOn)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.arenaSetAutoBuyBuffByOneCoin = _isOn;
            //保存到本地
            saveSetting();
        }

        /// <summary>
        /// 竞技场不购买增益
        /// </summary>
        /// <param name="_isOn"></param>
        public void setArenaSetNotToBuyBuff(bool _isOn)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.arenaSetNotToBuyBuff = _isOn;
            //保存到本地
            saveSetting();
        }

        /// <summary>
        /// 设置竞技场一键谈判自动购买水晶临时增益
        /// </summary>
        /// <param name="_isOn"></param>
        public void setArenaOneKeySetAutoBuyBuffByCrystal(bool _isOn)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.arenaOneKeySetAutoBuyBuffByCrystal = _isOn;
            //保存到本地
            saveSetting();
        }

        /// <summary>
        /// 设置竞技场一键谈判自动购买2银币临时增益
        /// </summary>
        /// <param name="_isOn"></param>
        public void setArenaOneKeySetAutoBuyBuffByTwoCoin(bool _isOn)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.arenaOneKeySetAutoBuyBuffByTwoCoin = _isOn;
            //保存到本地
            saveSetting();
        }

        /// <summary>
        /// 设置竞技场一键谈判自动购买1银币临时增益
        /// </summary>
        /// <param name="_isOn"></param>
        public void setArenaOneKeySetAutoBuyBuffByOneCoin(bool _isOn)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.arenaOneKeySetAutoBuyBuffByOneCoin = _isOn;
            //保存到本地
            saveSetting();
        }

        /// <summary>
        /// 竞技场一键谈判不购买增益
        /// </summary>
        /// <param name="_isOn"></param>
        public void setArenaOneKeySetNotToBuyBuff(bool _isOn)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.arenaOneKeySetNotToBuyBuff = _isOn;
            //保存到本地
            saveSetting();
        }

        /// <summary>
        /// 竞技场战斗机器人名字
        /// </summary>
        /// <param name="_botName"></param>
        public void setArenaFightBotName(string _botName)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.arenaFightBotName = _botName;
            //保存到本地
            saveSetting();
        }

        /// <summary>
        /// 竞技场战斗机器人等级
        /// </summary>
        /// <param name="_botName"></param>
        public void setArenaFightBotLevel(long _level)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.arenaFightBotLevel = _level;
            //保存到本地
            saveSetting();
        }
        
        /// <summary>
        /// 设置公会自动处理委托
        /// </summary>
        /// <param name="_isOn"></param>
        public void setGuildAutoDealEntrust(bool _isOn)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.guildAutoDealEntrust = _isOn;
            //保存到本地
            saveSetting();
        }

        /// <summary>
        /// 设置联盟公告是否收起
        /// </summary>
        /// <param name="_isClose"></param>
        public void setGuildAnnouncementIsClose(bool _isClose)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.guildAnnouncementIsClose = _isClose;
            //保存到本地
            saveSetting();
        }
        
        /// <summary>
        /// 爬塔战斗速度10倍开关
        /// </summary>
        /// <param name="_isOn"></param>
        public void setTowerBattleSpeedTenTimes(bool _isOn)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.towerBattleSpeedTenTimes = _isOn;
            //保存到本地
            saveSetting();
        }
        
        /// <summary>
        /// 爬塔跳过战斗
        /// </summary>
        public void setTowerSkipBattleShow(bool _isOn)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.towerSkipBattleShow = _isOn;
            //保存到本地
            saveSetting();
        }

        /// <summary>
        /// 设置公会副本自动战斗
        /// </summary>
        public void setGuildDungeonAutoBattle(bool _isOn)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.guildDungeonAutoBattle = _isOn;
            //保存到本地
            saveSetting();
        }

        public void setInnOneKeyCreateGuest(bool _isOn)
        {
            if (null == _m_settingData)
                return;
            
            _m_settingData.isInnOneKeyCreateGuest = _isOn;
            //保存到本地
            saveSetting();
        }
        
        /// <summary>
        /// 添加已经展示过的获取妃子
        /// </summary>
        /// <param name="_consortId"></param>
        public void addAlreadyShowGainConsort(long _consortId)
        {
            if (null == _m_settingData)
                return;

            if (_m_settingData.alreadyShowGainConsortList == null)
                _m_settingData.alreadyShowGainConsortList = new List<long>();

            if (!_m_settingData.alreadyShowGainConsortList.Contains(_consortId))
            {
                _m_settingData.alreadyShowGainConsortList.Add(_consortId);
                //保存到本地
                saveSetting();
            }
        }

        /// <summary>
        /// 获取是否是新冲榜
        /// </summary>
        /// <param name="_rankRushId"></param>
        /// <param name="_instanceId"></param>
        public bool getIsNewRankRush(long _rankRushId,long _instanceId)
        {
            if (_m_settingData == null)
                return false;

            if (_m_settingData.rankRushNewTipRecordList == null)
                return true;

            bool isNew = true;
            for (int i = 0; i < _m_settingData.rankRushNewTipRecordList.Count; i++)
            {
                if (_m_settingData.rankRushNewTipRecordList[i] != null &&
                    _m_settingData.rankRushNewTipRecordList[i].rankRushId == _rankRushId &&
                    (_m_settingData.rankRushNewTipRecordList[i].instanceId == _instanceId ||
                     _m_settingData.rankRushNewTipRecordList[i].instanceId > _instanceId))
                    isNew = false;
            }

            return isNew;
        }

        /// <summary>
        /// 记录已看的冲榜
        /// </summary>
        /// <param name="_rankRushId"></param>
        /// <param name="_instanceId"></param>
        public void setRecordRankRush(long _rankRushId, long _instanceId)
        {
            if (_m_settingData == null)
                return;

            if (_m_settingData.rankRushNewTipRecordList == null)
                _m_settingData.rankRushNewTipRecordList = new List<RankRushNewTipRecordData>();

            bool isFind = false;
            for (int i = 0; i < _m_settingData.rankRushNewTipRecordList.Count; i++)
            {
                if (_m_settingData.rankRushNewTipRecordList[i] != null &&
                    _m_settingData.rankRushNewTipRecordList[i].rankRushId == _rankRushId)
                {
                    if(_m_settingData.rankRushNewTipRecordList[i].instanceId < _instanceId)
                        _m_settingData.rankRushNewTipRecordList[i].instanceId = _instanceId;
                    isFind = true;
                }
            }

            if (!isFind)
            {
                RankRushNewTipRecordData recordData = new RankRushNewTipRecordData();
                recordData.rankRushId = _rankRushId;
                recordData.instanceId = _instanceId;
                _m_settingData.rankRushNewTipRecordList.Add(recordData);
            }
            //保存到本地
            saveSetting();
        }

        /// <summary>
        /// 获取倒计时事件是否展示过重置对话
        /// </summary>
        /// <param name="_dbId"></param>
        /// <param name="_lastResetTimeMs"></param>
        /// <returns></returns>
        public bool getCDEventIsAlreadyShowResetDialogue(long _dbId, long _lastResetTimeMs)
        {
            if (_m_settingData == null || _m_settingData.cdEventRecordData == null || _m_settingData.cdEventRecordData.lastResetTimeMs <= 0)
                return true;

            if (_m_settingData.cdEventRecordData.dbId == _dbId)
                return _m_settingData.cdEventRecordData.lastResetTimeMs == _lastResetTimeMs;

            return true;
        }

        /// <summary>
        /// 获取倒计时事件是否是新的
        /// </summary>
        /// <param name="_dbId"></param>
        /// <returns></returns>
        public bool getCDEventIsNew(long _dbId)
        {
            if (_m_settingData == null || _m_settingData.cdEventRecordData == null)
                return true;

            return _m_settingData.cdEventRecordData.dbId != _dbId;
        }

        /// <summary>
        /// 记录倒计时事件
        /// </summary>
        /// <param name="_dbId"></param>
        /// <param name="_lastResetTimeMs"></param>
        public void setRecordCDEvent(long _dbId, long _lastResetTimeMs)
        {
            if (_m_settingData == null)
                return;

            if (_m_settingData.cdEventRecordData == null)
                _m_settingData.cdEventRecordData = new CDEventRecordData();

            _m_settingData.cdEventRecordData.dbId = _dbId;
            _m_settingData.cdEventRecordData.lastResetTimeMs = _lastResetTimeMs;

            //保存到本地
            saveSetting();
        }

        /// <summary>
        /// 是否是新完成的阶段目标任务
        /// </summary>
        /// <param name="_taskId"></param>
        /// <returns></returns>
        public bool isNewCompletedStageGoalTask(long _stageGoalId, long _taskId)
        {
            if (_m_settingData == null || _m_settingData.stageGoalCompletedTaskRecord == null)
                return true;

            if (_m_settingData.stageGoalCompletedTaskRecord.stageGoalRefId == _stageGoalId && _m_settingData.stageGoalCompletedTaskRecord.completedTaskIdList != null)
                return !_m_settingData.stageGoalCompletedTaskRecord.completedTaskIdList.Contains(_taskId);
            else
                return true;
        }

        /// <summary>
        /// 记录新完成阶段目标任务
        /// </summary>
        /// <param name="_taskId"></param>
        public void setNewCompletedStageGoalTask(long _stageGoalId, long _taskId)
        {
            if (_m_settingData == null)
                return;

            if (_m_settingData.stageGoalCompletedTaskRecord == null)
                _m_settingData.stageGoalCompletedTaskRecord = new StageGoalCompletedTaskRecord();

            if (_m_settingData.stageGoalCompletedTaskRecord.completedTaskIdList == null)
                _m_settingData.stageGoalCompletedTaskRecord.completedTaskIdList = new List<long>();

            //如果阶段目标id不一致，则清空之前的任务记录
            if (_m_settingData.stageGoalCompletedTaskRecord.stageGoalRefId != _stageGoalId)
            {
                _m_settingData.stageGoalCompletedTaskRecord.stageGoalRefId = _stageGoalId;
                _m_settingData.stageGoalCompletedTaskRecord.completedTaskIdList.Clear();
            }

            if(_taskId > 0 && !_m_settingData.stageGoalCompletedTaskRecord.completedTaskIdList.Contains(_taskId))
            {
                _m_settingData.stageGoalCompletedTaskRecord.completedTaskIdList.Add(_taskId);
                //保存到本地
                saveSetting();
            }
        }

        /// <summary>
        /// 获取阶段目标任务记录的任务数量
        /// </summary>
        /// <returns></returns>
        public long getStageGoalRecordTaskCount(long _stageGoalId)
        {
            if (_m_settingData == null || _m_settingData.stageGoalCompletedTaskRecord == null || _m_settingData.stageGoalCompletedTaskRecord.completedTaskIdList == null)
                return 0;

            if (_stageGoalId == _m_settingData.stageGoalCompletedTaskRecord.stageGoalRefId)
                return _m_settingData.stageGoalCompletedTaskRecord.completedTaskIdList.Count;
            else
                return 0;
        }

        /// <summary>
        /// 是否已经看过阶段目标任务红点
        /// </summary>
        /// <param name="_taskId"></param>
        /// <returns></returns>
        public bool isReadStageTaskRedTip(long _taskId)
        {
            if(_m_settingData == null || _m_settingData.alreadyReadRedTipStageTaskIdList  == null)
                return false;

            return _m_settingData.alreadyReadRedTipStageTaskIdList.Contains(_taskId);
        }

        /// <summary>
        /// 记录已读阶段目标任务红点
        /// </summary>
        /// <param name="_taskId"></param>
        public void recordReadStageTaskRedTipTaskId(long _taskId)
        {
            if (_m_settingData == null)
                return;

            if (_m_settingData.alreadyReadRedTipStageTaskIdList == null)
                _m_settingData.alreadyReadRedTipStageTaskIdList = new List<long>();

            if (_m_settingData.alreadyReadRedTipStageTaskIdList.Contains(_taskId))
                return;

            _m_settingData.alreadyReadRedTipStageTaskIdList.Add(_taskId);
            //保存到本地
            saveSetting();
        }

        /// <summary>
        /// 清空阶段目标任务已读红点记录
        /// </summary>
        public void clearStageTaskReadRedTipRecord()
        {
            if (_m_settingData == null)
                return;

            _m_settingData.alreadyReadRedTipStageTaskIdList?.Clear();
            //保存到本地
            saveSetting();
        }

        /// <summary>
        /// 设置已经展示过解锁动画的大阶段目标id
        /// </summary>
        /// <param name="_id"></param>
        public void setAlreadyShowUnlockAniBigStageGoalId(long _id)
        {
            if (null == _m_settingData || _id == 0)
                return;

            _m_settingData.alreadyShowUnlockAniBigStageGoalId = _id;
            //保存到本地
            saveSetting();
        }

        /// <summary>
        /// 获取大阶段目标是否已经展示过完成动画
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public bool getIsBigStageGoalAlreadyShowFinishAni(long _id)
        {
            if (null == _m_settingData || _id == 0)
                return false;

            if (_m_settingData.alreadyShowFinishAniBigStageGoalIdList == null)
                return false;

            return _m_settingData.alreadyShowFinishAniBigStageGoalIdList.Contains(_id);
        }

        /// <summary>
        /// 设置大阶段目标是否已经展示过完成动画
        /// </summary>
        /// <param name="_id"></param>
        public void setAlreadyShowFinishAniBigStageGoalId(long _id)
        {
            if (null == _m_settingData)
                return;

            if(_m_settingData.alreadyShowFinishAniBigStageGoalIdList == null)
                _m_settingData.alreadyShowFinishAniBigStageGoalIdList = new List<long>();

            _m_settingData.alreadyShowFinishAniBigStageGoalIdList.Add(_id);
            //保存到本地
            saveSetting();
        }

        /// <summary>
        /// 伙伴设置是否十连升级
        /// </summary>
        /// <param name="_isHeroTenUpgrade"></param>
        public void setIsHeroTenUpgrade(bool _isHeroTenUpgrade)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.isHeroTenUpgrade = _isHeroTenUpgrade;
            //保存到本地
            saveSetting();
        }

        /// <summary>
        /// 伙伴设置是否十连升级资质
        /// </summary>
        /// <param name="_isHeroTalentTenUpgrade"></param>
        public void setIsHeroTalentTenUpgrade(bool _isHeroTalentTenUpgrade)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.isHeroTalentTenUpgrade = _isHeroTalentTenUpgrade;
            //保存到本地
            saveSetting();
        }

        /// <summary>
        /// 藏品是否使用十连升级
        /// </summary>
        /// <param name="_isEquipTenUpgrade"></param>
        public void setIsEquipTenUpgrade(bool _isEquipTenUpgrade)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.isEquipTenUpgrade = _isEquipTenUpgrade;
            //保存到本地
            saveSetting();
        }
        
        /// <summary>
        /// 关卡快速前进是否跳过对话
        /// </summary>
        public void setIsChapterQuickForwardSkipDialog(bool _isChapterQuickForwardSkipDialog)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.isChapterQuickForwardSkipDialog = _isChapterQuickForwardSkipDialog;
            //保存到本地
            saveSetting();
        }

        /// <summary>
        /// 是否已读联盟红点
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public bool isReadGuildRedTip(long _id)
        {
            if (null == _m_settingData)
                return false;

            if (_m_settingData.alreadyReadGuildRedTipIdList == null)
                return false;

            return _m_settingData.alreadyReadGuildRedTipIdList.Contains(_id);
        }

        /// <summary>
        /// 添加已读联盟红点id
        /// </summary>
        /// <param name="_id"></param>
        public void addAlreadyReadGuildRedTip(long _id)
        {
            if (null == _m_settingData)
                return;

            if (_m_settingData.alreadyReadGuildRedTipIdList == null)
                _m_settingData.alreadyReadGuildRedTipIdList = new List<long>();

            if(!_m_settingData.alreadyReadGuildRedTipIdList.Contains(_id))
                _m_settingData.alreadyReadGuildRedTipIdList.Add(_id);
            //保存到本地
            saveSetting();
        }

        /// <summary>
        /// 移除已读联盟红点id
        /// </summary>
        /// <param name="_id"></param>
        public void removeAlreadyReadGuildRedTip(long _id)
        {
            if (null == _m_settingData)
                return;

            if (_m_settingData.alreadyReadGuildRedTipIdList == null)
                _m_settingData.alreadyReadGuildRedTipIdList = new List<long>();

            _m_settingData.alreadyReadGuildRedTipIdList.Remove(_id);
            //保存到本地
            saveSetting();
        }

        /// <summary>
        /// 是否已读公会协作区域解锁红点
        /// </summary>
        /// <param name="_tag"></param>
        /// <returns></returns>
        public bool isReadGuildCooperateAreaUnlockRedTip(string _tag)
        {
            if (null == _m_settingData)
                return false;

            return _m_settingData.guildCooperateAreaUnlockRedTipRecord == _tag;
        }

        /// <summary>
        /// 设置公会协作区域解锁红点已读
        /// </summary>
        /// <param name="_tag"></param>
        public void setGuildCooperateAreaUnlockRedTipRead(string _tag)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.guildCooperateAreaUnlockRedTipRecord = _tag;

            //保存到本地
            saveSetting();
        }

        /// <summary>
        /// 是否已点击玩家解锁顾问前往按钮
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public bool isClickPlayerGainHeroJump(long _id)
        {
            if (null == _m_settingData)
                return false;

            if (_m_settingData.clickPlayerGainHeroJumpIdList == null)
                return false;

            return _m_settingData.clickPlayerGainHeroJumpIdList.Contains(_id);
        }

        /// <summary>
        /// 添加点击玩家解锁顾问前往按钮id
        /// </summary>
        /// <param name="_id"></param>
        public void addClickPlayerGainHeroJumpId(long _id)
        {
            if (null == _m_settingData)
                return;

            if (_m_settingData.clickPlayerGainHeroJumpIdList == null)
                _m_settingData.clickPlayerGainHeroJumpIdList = new List<long>();

            if (!_m_settingData.clickPlayerGainHeroJumpIdList.Contains(_id))
            {
                _m_settingData.clickPlayerGainHeroJumpIdList.Add(_id);
                //保存到本地
                saveSetting();
            }
        }

         /// <summary>
        /// 设置火星探索PVP日志已读时间
        /// </summary>
        public void setReadMarsExplorePvPLogTime(long _time)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.readMarsExplorePvPLogTime = _time;
            //保存到本地
            saveSetting();
        }

        /// <summary>
        /// 设置火星探索共享矿数据已读
        /// </summary>
        public void setReadSharedMineData(NewestSharedMineData _data)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.readMarsExploreSharedMineData = _data;
            //保存到本地
            saveSetting();
        }

        /// <summary>
        /// 设置火星探索联盟战报已读ID
        /// </summary>
        public void setReadMarsExploreGuildBattleReportId(long _id, long _guildId)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.readMarsExploreGuildBattleReportId = _id;
            _m_settingData.readMarsExploreGuildBattleReportGuildId = _guildId;
            //保存到本地
            saveSetting();
        }

        #region 商店


        /// <summary>
        /// 设置商店自动更新提示已弹窗时间
        /// </summary>
        public void setShopAutoRefreshPopWndDate(int _date)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.shopAutoRefreshPopWndDate = _date;
            //保存到本地
            saveSetting();
        }

        /// <summary>
        /// 获取商店自动更新提示弹窗时间
        /// </summary>
        /// <returns></returns>
        public int getShopAutoRefreshPopWndDate()
        {
            if (null == _m_settingData)
                return 0;

            return _m_settingData.shopAutoRefreshPopWndDate;
        }

        #endregion

        #region 国力目标红点

        /// <summary>
        /// 是否已经显示过国力目标红点
        /// </summary>
        public bool hadShowNationalPowerTargetUnlockRedTip { get { return _m_settingData?.hadShowNationalPowerTargetUnlockRedTip ?? false; } }

        /// <summary>
        /// 设置已显示过国力目标红点
        /// </summary>
        /// <param name="_hadShow">是否已显示</param>
        public void setHadShowNationalPowerTargetUnlockRedTip(bool _hadShow)
        {
            if (null == _m_settingData)
                return;

            if (_m_settingData.hadShowNationalPowerTargetUnlockRedTip != _hadShow)
            {
                _m_settingData.hadShowNationalPowerTargetUnlockRedTip = _hadShow;
                //保存到本地
                saveSetting();
            }
        }

        #endregion

        #region 国力目标弹窗

        /// <summary>
        /// 国力目标弹窗显示日期
        /// </summary>
        public string nationalPowerTargetPopWndShowDate { get { return _m_settingData?.nationalPowerTargetPopWndShowDate ?? string.Empty; } }

        /// <summary>
        /// 设置国力目标弹窗显示日期
        /// </summary>
        public void setNationalPowerTargetPopWndShowDate()
        {
            if (null == _m_settingData)
                return;

            string dataStr = System.DateTime.Now.ToString("yyyyMMdd");
            if (_m_settingData.nationalPowerTargetPopWndShowDate != dataStr)
            {
                _m_settingData.nationalPowerTargetPopWndShowDate = dataStr;
                //保存到本地
                saveSetting();
            }
        }

        /// <summary>
        /// 今天是否已经显示过国力目标弹窗
        /// </summary>
        /// <returns></returns>
        public bool isTodayShowedNationalPowerTargetPopWnd()
        {
            if (null == _m_settingData)
                return false;

            string dataStr = System.DateTime.Now.ToString("yyyyMMdd");
            return _m_settingData.nationalPowerTargetPopWndShowDate == dataStr;
        }

        #endregion

        #region 七日登录主城弹窗

        /// <summary>
        /// 七日登录主城弹窗显示日期
        /// </summary>
        public string sevenDayLoginMainCityPopWndShowDate { get { return _m_settingData?.sevenDayLoginMainCityPopWndShowDate ?? string.Empty; } }

        /// <summary>
        /// 设置七日登录主城弹窗显示日期
        /// </summary>
        public void setSevenDayLoginMainCityPopWndShowDate()
        {
            if (null == _m_settingData)
                return;

            string dataStr = System.DateTime.Now.ToString("yyyyMMdd");
            if (_m_settingData.sevenDayLoginMainCityPopWndShowDate != dataStr)
            {
                _m_settingData.sevenDayLoginMainCityPopWndShowDate = dataStr;
                //保存到本地
                saveSetting();
            }
        }

        /// <summary>
        /// 今天是否已经显示过七日登录主城弹窗
        /// </summary>
        /// <returns></returns>
        public bool isTodayShowedSevenDayLoginMainCityPopWnd()
        {
            if (null == _m_settingData)
                return false;

            string dataStr = System.DateTime.Now.ToString("yyyyMMdd");
            return _m_settingData.sevenDayLoginMainCityPopWndShowDate == dataStr;
        }
        
        #endregion

        #region 首充主城弹窗

        /// <summary>
        /// 首充主城弹窗显示日期
        /// </summary>
        public string firstRechargeMainCityPopWndShowDate { get { return _m_settingData?.firstRechargeMainCityPopWndShowDate ?? string.Empty; } }
        public long lastShareMarsMineToGuildChatTime { get { return _m_settingData?.lastShareMarsMineToGuildChatTime ?? 0; } }

        public void setLastShareMarsMineToGuildChatTime(long _time)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.lastShareMarsMineToGuildChatTime = _time;
            saveSetting();
        }

        /// <summary>
        /// 设置首充主城弹窗显示日期
        /// </summary>
        public void setFirstRechargeMainCityPopWndShowDate()
        {
            if (null == _m_settingData)
                return;

            string dataStr = System.DateTime.Now.ToString("yyyyMMdd");
            if (_m_settingData.firstRechargeMainCityPopWndShowDate != dataStr)
            {
                _m_settingData.firstRechargeMainCityPopWndShowDate = dataStr;
                //保存到本地
                saveSetting();
            }
        }

        /// <summary>
        /// 今天是否已经显示过首充主城弹窗
        /// </summary>
        /// <returns></returns>
        public bool isTodayShowedFirstRechargeMainCityPopWnd()
        {
            if (null == _m_settingData)
                return false;

            string dataStr = System.DateTime.Now.ToString("yyyyMMdd");
            return _m_settingData.firstRechargeMainCityPopWndShowDate == dataStr;
        }

        #endregion

        #region 七日目标

        /// <summary>
        /// 获取七日目标某天是否是新解锁未读
        /// </summary>
        /// <param name="_instanceId"></param>
        /// <param name="_day"></param>
        public bool getSevenDayGoalDayIsNew(long _activityInstanceId, long _day)
        {
            if (null == _m_settingData)
                return false;

            if (_m_settingData.sevenDayGoalsRedTipRecord == null ||
                _m_settingData.sevenDayGoalsRedTipRecord.activityInstanceId != _activityInstanceId || 
                _m_settingData.sevenDayGoalsRedTipRecord.readDayList == null ||
                !_m_settingData.sevenDayGoalsRedTipRecord.readDayList.Contains(_day))
                return true;

            return false;
        }

        /// <summary>
        /// 设置七日目标某天新解锁的已读
        /// </summary>
        public void setNewSevenDayGoalDayRead(long _activityInstanceId, long _day)
        {
            if (null == _m_settingData)
                return;

            if(_m_settingData.sevenDayGoalsRedTipRecord == null)
                _m_settingData.sevenDayGoalsRedTipRecord = new SevenDayGoalsRedTipRecord();

            if (_m_settingData.sevenDayGoalsRedTipRecord.activityInstanceId != _activityInstanceId)
            {
                _m_settingData.sevenDayGoalsRedTipRecord.activityInstanceId = _activityInstanceId;
                _m_settingData.sevenDayGoalsRedTipRecord.readDayList = new List<long>();
                _m_settingData.sevenDayGoalsRedTipRecord.giftReadRecordList = new List<SevenDayGoalsGiftReadRecord>();
            }

            if (_m_settingData.sevenDayGoalsRedTipRecord.readDayList == null)
                _m_settingData.sevenDayGoalsRedTipRecord.readDayList = new List<long>();

            if (!_m_settingData.sevenDayGoalsRedTipRecord.readDayList.Contains(_day))
                _m_settingData.sevenDayGoalsRedTipRecord.readDayList.Add(_day);

            //保存到本地
            saveSetting();
        }

        /// <summary>
        /// 获取七日任务礼包每日红点是否已读
        /// </summary>
        /// <param name="_activityInstanceId"></param>
        /// <param name="_day"></param>
        /// <returns></returns>
        public bool getSevenDayGoalDayGiftIsRead(long _activityInstanceId, long _day)
        {
            if (null == _m_settingData)
                return true;

            if (_m_settingData.sevenDayGoalsRedTipRecord == null ||
                _m_settingData.sevenDayGoalsRedTipRecord.activityInstanceId != _activityInstanceId ||
                _m_settingData.sevenDayGoalsRedTipRecord.giftReadRecordList == null)
                return false;

            for (int i = 0; i < _m_settingData.sevenDayGoalsRedTipRecord.giftReadRecordList.Count; i++)
            {
                SevenDayGoalsGiftReadRecord record = _m_settingData.sevenDayGoalsRedTipRecord.giftReadRecordList[i];
                if (record == null) 
                    continue;

                if (record.day == _day)
                    return record.readData == TimeUtil.getTimeByYYYYMM(FpsAndPingMgr.instance.serverTimeTag);
            }

            return false;
        }

        /// <summary>
        /// 设置七日任务礼包每日红点已读
        /// </summary>
        /// <param name="_activityInstanceId"></param>
        /// <param name="_day"></param>
        public void setSevenDayGoalDayGiftRead(long _activityInstanceId, long _day)
        {
            if (null == _m_settingData)
                return;

            if (_m_settingData.sevenDayGoalsRedTipRecord == null)
                _m_settingData.sevenDayGoalsRedTipRecord = new SevenDayGoalsRedTipRecord();
            if (_m_settingData.sevenDayGoalsRedTipRecord.activityInstanceId != _activityInstanceId)
            {
                _m_settingData.sevenDayGoalsRedTipRecord.activityInstanceId = _activityInstanceId;
                _m_settingData.sevenDayGoalsRedTipRecord.readDayList = new List<long>();
                _m_settingData.sevenDayGoalsRedTipRecord.giftReadRecordList = new List<SevenDayGoalsGiftReadRecord>();
            }
            if (_m_settingData.sevenDayGoalsRedTipRecord.giftReadRecordList == null)
                _m_settingData.sevenDayGoalsRedTipRecord.giftReadRecordList = new List<SevenDayGoalsGiftReadRecord>();
            bool isFind = false;
            for (int i = 0; i < _m_settingData.sevenDayGoalsRedTipRecord.giftReadRecordList.Count; i++)
            {
                SevenDayGoalsGiftReadRecord record = _m_settingData.sevenDayGoalsRedTipRecord.giftReadRecordList[i];
                if (record == null)
                    continue;
                if (record.day == _day)
                {
                    record.readData = TimeUtil.getTimeByYYYYMM(FpsAndPingMgr.instance.serverTimeTag);
                    isFind = true;
                    break;
                }
            }
            if (!isFind)
            {
                SevenDayGoalsGiftReadRecord newRecord = new SevenDayGoalsGiftReadRecord();
                newRecord.day = _day;
                newRecord.readData = TimeUtil.getTimeByYYYYMM(FpsAndPingMgr.instance.serverTimeTag);
                _m_settingData.sevenDayGoalsRedTipRecord.giftReadRecordList.Add(newRecord);
            }
            //保存到本地
            saveSetting();
        }

        #endregion

        #region 聊天举报

        /// <summary>
        /// 获取玩家举报记录时间
        /// </summary>
        /// <param name="_cid"></param>
        /// <returns></returns>
        public long getPlayerReportTimeRecord(long _cid)
        {
            if (_m_settingData == null)
                return 0;

            if(_m_settingData.playerReportRecordTimeList == null)
                return 0;

            for (int i = 0; i < _m_settingData.playerReportRecordTimeList.Count; i++)
            {
                PlayerReportRecordTime record = _m_settingData.playerReportRecordTimeList[i];
                if (record == null)
                    continue;
                if (record.cid == _cid)
                    return record.reportTime;
            }

            return 0;
        }

        /// <summary>
        /// 设置记录举报玩家时间
        /// </summary>
        /// <param name="_cid"></param>
        /// <param name="_reportTime"></param>
        public void setPlayerReportTimeRecord(long _cid, long _reportTime)
        {
            if (_m_settingData == null)
                return;
            if (_m_settingData.playerReportRecordTimeList == null)
                _m_settingData.playerReportRecordTimeList = new List<PlayerReportRecordTime>();
            bool isFind = false;
            for (int i = 0; i < _m_settingData.playerReportRecordTimeList.Count; i++)
            {
                PlayerReportRecordTime record = _m_settingData.playerReportRecordTimeList[i];
                if (record == null)
                    continue;
                if (record.cid == _cid)
                {
                    record.reportTime = _reportTime;
                    isFind = true;
                    break;
                }
            }
            if (!isFind)
            {
                PlayerReportRecordTime newRecord = new PlayerReportRecordTime();
                newRecord.cid = _cid;
                newRecord.reportTime = _reportTime;
                _m_settingData.playerReportRecordTimeList.Add(newRecord);
            }
            //保存到本地
            saveSetting();
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using GC2GS.p002_InitOp;
using GC2GS.p004_PlayerOp;
using GC2GS.p037_GuildDungeonOp;
using GS2GC.p002_InitOp;
using GS2GC.p004_PlayerOp;
using GS2GC.p037_GuildDungeonOp;
using Common.GuildDungeonEnum;
using Common.GuildDungeonObj;
using JetBrains.Annotations;
using NPCommon;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟PVE管理组件
    /// </summary>
    public class GuildDungeonComponent : _ANPBasicPlayerComponent
    {
        private long _m_dungeonRefreshTimeTag; // 当前副本刷新时间戳（毫秒）
        private int _m_lastCheckFrameCount; // 上次检查副本信息的帧计数
        private List<GuildDungeonInfo> _m_dungeonList = new List<GuildDungeonInfo>(); //副本列表
        
       
        private Dictionary<long, GuildDungeon_FightHero> _m_fightHeroMap = new Dictionary<long, GuildDungeon_FightHero>(); // 保存每个大臣在公会副本中的出战/恢复次数

        public List<GuildDungeonInfo> dungeonList
        {
            get
            {
                _checkResetInfo();
                return _m_dungeonList;
            }
        }

        public GuildDungeonComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
        }

        //属性
        private static readonly ENPPlayerCompType[] _g_DependComp = { ENPPlayerCompType.GUILD };
        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.GUILD_DUNGEON; } }
        public override ENPPlayerCompType[] dependCompList => _g_DependComp;

        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return true; } }

        public override void presendInitProtocol()
        {
            //请求初始化
            _reqGuildDungeonInit();
        }

        protected override void _dealInit()
        {
        }

        protected override void _onInitDone()
        {
            WinMsg.RegisterMsgAct(WinMsgType.ON_LEAVE_GUILD, _onLeaveGuild);
            WinMsg.RegisterMsgAct(WinMsgType.ON_JOIN_GUILD, _onJoinGuild);
            WinMsg.RegisterMsg(WinMsgType.ON_HERO_GAIN, _onHeroGain);
        }

        public override void onAllCompInited()
        {
            base.onAllCompInited();
            _refreshUnlockSysRedTip();
        }

        protected override void _onInitFail()
        {
        }

        protected override void _discard()
        {
            _m_dungeonList?.Clear();
            _m_dungeonList = null;
            _m_fightHeroMap?.Clear();
            _m_fightHeroMap = null;
            WinMsg.UnregisterMsgAct(WinMsgType.ON_LEAVE_GUILD, _onLeaveGuild);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_JOIN_GUILD, _onJoinGuild);
            WinMsg.UnregisterMsg(WinMsgType.ON_HERO_GAIN, _onHeroGain);
        }

        /// <summary>
        /// 检查是否需要重置副本信息
        /// </summary>
        private void _checkResetInfo()
        {
            if(Time.frameCount == _m_lastCheckFrameCount)
                return; // 如果是同一帧，不再检查
            _m_lastCheckFrameCount = Time.frameCount;
            long freshTimeTag = GRefdataCoreMgr.instance.npGeneral.guild_dungeon_auto_settle_time.getPreviousFreshTimeTagMs();
            if (_m_dungeonRefreshTimeTag != freshTimeTag)
            {
                _resetGuildDungeonInfo(freshTimeTag);
            }
        }

        private void _resetGuildDungeonInfo(long _freshTimeTag)
        {
            _m_dungeonRefreshTimeTag = _freshTimeTag;
            if (_m_dungeonList != null)
                foreach (var dungeon in _m_dungeonList)
                {
                    dungeon?.nextDayResetInfo(_m_dungeonRefreshTimeTag);
                }
            _m_fightHeroMap?.Clear();
            WinMsg.SendMsg(WinMsgType.ON_GUILD_DUNGEON_RESET);
        }

        private void _refreshRedTip()
        {
            int count = 0;
            if (NPPlayer.instance.guildComp.isJoinGuild())
            {
                //剩余可出战的大臣战力
                long leftHeroTotalPower = getLeftHeroTotalPower();
                foreach (GuildDungeonInfo dungeonInfo in _m_dungeonList)
                {
                    if (dungeonInfo == null || dungeonInfo.instanceId <= 0)
                        continue; // 没有实例ID的副本不处理
                    if (dungeonInfo.hasRewardToGain)
                        count++; // 有奖励可领取的副本
                    if (leftHeroTotalPower > 0 && dungeonInfo.state == EGuildDungeonState.Started)
                        count++; // 有未击败的怪物
                }
            }
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_GUILD_DUNGEON, count);
        }

        //刷新解锁系统红点
        private void _refreshUnlockSysRedTip()
        {
            long count = 0;
            if (NPPlayer.instance.guildComp.isJoinGuild() && !AccountSettingMgr.instance.accountSetting.isReadGuildRedTip(RedTipConst.RED_GUILD_DUNGEON_UNLOCK))
                count = 1;

            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_GUILD_DUNGEON_UNLOCK, count);
        }
        
        public GuildDungeonInfo getDungeonInfoById(long _dungeonId)
        {
            _checkResetInfo();
            if (dungeonList == null)
                return null;
            foreach (GuildDungeonInfo dungeonInfo in dungeonList)
            {
                if (dungeonInfo != null && dungeonInfo.dungeonId == _dungeonId)
                    return dungeonInfo;
            }
            return null;
        }
        
                
        public GuildDungeonInfo getDungeonInfoByInstanceId(long _instanceId)
        {
            _checkResetInfo();
            if (dungeonList == null)
                return null;
            foreach (GuildDungeonInfo dungeonInfo in dungeonList)
            {
                if (dungeonInfo != null && dungeonInfo.instanceId == _instanceId)
                    return dungeonInfo;
            }
            return null;
        }

        /// <summary>
        /// 剩余可出战大臣战力总值
        /// </summary>
        /// <returns></returns>
        public long getLeftHeroTotalPower()
        {
            _checkResetInfo();
            
            long totalPower = 0;
            List<HeroInfo> heroList = new List<HeroInfo>();
            NPPlayer.instance.heroComponent.getAllList(heroList);
            
            foreach (var heroInfo in heroList)
            {
                if (heroInfo == null) continue;
                
                // 只计算还有出战次数的英雄战力
                if (getHeroLeftFightTimes(heroInfo.id) > 0)
                {
                    totalPower += heroInfo.power;
                }
            }
            
            return totalPower;
        }
        
        public int getHeroLeftFightTimes(long _heroId)
        {
            if (_m_fightHeroMap == null)
                return 0; 
            
            if (_m_fightHeroMap.TryGetValue(_heroId, out GuildDungeon_FightHero fightHero))
            {
                if (fightHero != null)
                    return 1 + fightHero.getRecoveredCount() - fightHero.getFightedCount(); // 1次出战+已恢复次数-已出战次数
            }
            return 1; // 默认1次
        }
        
        public HeroInfo getSelfMatchHero(long _bossPower)
        {
            _checkResetInfo();
            List<HeroInfo> heroList = new List<HeroInfo>();
            NPPlayer.instance.heroComponent.getAllList(heroList);
            heroList.Sort(sortHero);
            HeroInfo selectHero = null;
            foreach (var heroInfo in heroList)
            {
                if(heroInfo == null) continue;
                if(getHeroLeftFightTimes(heroInfo.id) <= 0)
                    continue;
                // 如果当前选中大臣为空，并且还有次数，则选中当前大臣
                if (selectHero == null)
                {
                    selectHero = heroInfo;
                }
                if(selectHero.power > heroInfo.power && heroInfo.power > _bossPower)
                    selectHero = heroInfo;
            }

            return selectHero;
        }
        
        /// <summary>
        /// 获取某个大臣的出战信息（出战次数/已恢复次数）。可能为null。
        /// </summary>
        public GuildDungeon_FightHero getFightHeroInfo(long heroId)
        {
            _checkResetInfo();
            if (_m_fightHeroMap != null && _m_fightHeroMap.TryGetValue(heroId, out var info))
                return info;
            return null;
        }
        
        public int sortHero(_IHeroCardShow a, _IHeroCardShow b)
        {
            if (b == null)
                return -1;
            if (a == null)
                return 1;
            
            // 获取英雄状态
            int stateA = _getHeroSortPriority(a.id);
            int stateB = _getHeroSortPriority(b.id);
            
            // 首先按状态优先级排序：出战次数(0) > 可恢复次数(1) > 不可出战(2)
            if (stateA != stateB)
                return stateA.CompareTo(stateB);
            
            // 状态相同时按战力降序排序
            return b.power.CompareTo(a.power);
        }
        
        /// <summary>
        /// 获取英雄排序优先级
        /// </summary>
        /// <param name="heroId">英雄ID</param>
        /// <returns>0=可出战, 1=可恢复, 2=不可出战</returns>
        private int _getHeroSortPriority(long heroId)
        {
            // 检查剩余出战次数
            int leftFightTimes = getHeroLeftFightTimes(heroId);
            if (leftFightTimes > 0)
                return 0; // 可出战，最高优先级
            
            // 检查是否可恢复
            if (canRecoverHero(heroId))
                return 1; // 可恢复，次优先级
            
            return 2; // 不可出战，最低优先级
        }
        
        /// <summary>
        /// 检查英雄是否可恢复
        /// </summary>
        /// <param name="heroId">英雄ID</param>
        /// <returns>是否可恢复</returns>
        public bool canRecoverHero(long heroId)
        {
            if (_m_fightHeroMap == null || !_m_fightHeroMap.TryGetValue(heroId, out GuildDungeon_FightHero fightHero))
                return false;
            
            if (fightHero == null)
                return false;
            
            int fighted = fightHero.getFightedCount();
            int recovered = fightHero.getRecoveredCount();
            
            // 如果没有出战过，则不需要恢复
            if (fighted <= 0)
                return false;
            
            // 检查恢复次数是否达到上限
            int limit = GRefdataCoreMgr.instance.npGeneral.guild_dungeon_recover_hero_limit;
            bool underLimit = recovered < limit;
            
            return underLimit;
        }

        public bool hasGainedRewardMonster(long _instanceId, long _monsterId)
        {
            _checkResetInfo();

            GuildDungeonInfo dungeonInfo = getDungeonInfoByInstanceId(_instanceId);
            return dungeonInfo != null && dungeonInfo.hasGainedRewardMonster(_monsterId);
        }

        #region 消息事件

        // 离开联盟
        private void _onLeaveGuild()
        {
            _resetGuildDungeonInfo(0);
            AccountSettingMgr.instance.accountSetting.removeAlreadyReadGuildRedTip(RedTipConst.RED_GUILD_DUNGEON_UNLOCK);
            _refreshUnlockSysRedTip();
            _refreshRedTip();
        }

        // 加入联盟
        private void _onJoinGuild()
        {
            _refreshUnlockSysRedTip();
        }

        // 大臣获得
        private void _onHeroGain(params object[] _objects)
        {
            _refreshRedTip();
        }

        #endregion

        #region S2C

        /// <summary>
        /// 初始化回包
        /// </summary>
        /// <param name="_info"></param>
        public void retGuildDungeonInit(GS2GC_002_071_RetGuildDungeonInit _info)
        {
            _checkResetInfo();
            
            // 记录大臣出战/恢复次数
            _m_fightHeroMap?.Clear();
            if (_info != null && _info.getFightHeroList() != null)
            {
                foreach (var fh in _info.getFightHeroList())
                {
                    if (fh == null) continue;
                    _m_fightHeroMap[fh.getHeroId()] = fh;
                }
            }

            if (_m_dungeonList == null)
                _m_dungeonList = new List<GuildDungeonInfo>();
            _m_dungeonList.Clear();
            List<GuildDungeon_SetInfo> setList = _info.getSetList();
            List<GuildDungeon_InstanceInfo> instanceList = _info.getInstanceList();
            List<GuildDungeon_DungeonMonster> rewardMonsterIdList = _info.getGainedRewardMonsterIdList();

            foreach (GuildDungeonRefObj dungeonRef in GRefdataCoreMgr.instance.guildDungeonRefCore.refList)
            {
                if (dungeonRef == null)
                    continue;
                GuildDungeon_SetInfo setInfo = null;
                GuildDungeon_InstanceInfo instanceInfo = null;

                if (_info != null)
                {
                    if (setList != null)    
                        foreach (GuildDungeon_SetInfo itemData in setList)
                        {
                            if (itemData != null && itemData.getDungeonId() == dungeonRef.id)
                            {
                                setInfo = itemData;
                                break;
                            }
                        }
                
                    if (instanceList != null)
                        foreach (GuildDungeon_InstanceInfo itemData in instanceList)
                        {
                            if (itemData != null && itemData.getDungeonId() == dungeonRef.id)
                            {
                                instanceInfo = itemData;
                                break;
                            }
                        }
                }

                GuildDungeonInfo dungeonInfo = null;


                if (setInfo != null)
                    dungeonInfo = new GuildDungeonInfo(_m_dungeonRefreshTimeTag, dungeonRef, setInfo);
                else
                    dungeonInfo = new GuildDungeonInfo(_m_dungeonRefreshTimeTag, dungeonRef);

                if (instanceInfo != null)
                {
                    dungeonInfo.updateInstanceInfo(_m_dungeonRefreshTimeTag, instanceInfo);
                    dungeonInfo.updateGainRewardMonster(rewardMonsterIdList);
                }
                _m_dungeonList.Add(dungeonInfo);
            }
            
            
            _refreshRedTip();
            setInitDone();
        }

        /// <summary>
        /// 副本信息推送
        /// </summary>
        /// <param name="_msg"></param>
        public void onDungeonPush(GS2GC_037_056_OnDungeonPush _msg)
        {
            if(_msg == null)
                return;
            _checkResetInfo();
            List<GuildDungeon_SetInfo> setList = _msg.getSetList();
            List<GuildDungeon_InstanceInfo> instanceList = _msg.getInstanceList();
            List<GuildDungeon_DungeonMonster> rewardMonsterIdList = _msg.getGainedRewardMonsterIdList();

            if (setList != null)
                foreach (GuildDungeon_SetInfo setInfo in setList)
                {
                    if (setInfo != null)
                    {
                        GuildDungeonInfo dungeonInfo = getDungeonInfoById(setInfo.getDungeonId());
                        if (dungeonInfo != null) dungeonInfo.updateSetInfo(_m_dungeonRefreshTimeTag, setInfo);
                    }
                }
            
            if (instanceList != null)
                foreach (GuildDungeon_InstanceInfo instanceInfo in instanceList)
                {
                    if (instanceInfo != null)
                    {
                        GuildDungeonInfo dungeonInfo = getDungeonInfoById(instanceInfo.getDungeonId());
                        if (dungeonInfo != null)
                        {
                            dungeonInfo.updateInstanceInfo(_m_dungeonRefreshTimeTag, instanceInfo);
                            dungeonInfo.updateGainRewardMonster(rewardMonsterIdList);
                        }
                    }
                }
        }

        /// <summary>
        /// 副本变更推送
        /// </summary>
        /// <param name="_msg"></param>
        public void onDungeonSetChg(GS2GC_037_050_OnDungeonSetChg _msg)
        {
            if (_msg == null)
                return;

            _checkResetInfo();
            // 根据副本ID查找对应的副本信息并更新实例数据
            GuildDungeon_SetInfo setInfo = _msg.getSetInfo();
            if (setInfo != null)
            {
                GuildDungeonInfo dungeonInfo = getDungeonInfoById(setInfo.getDungeonId());
                if (dungeonInfo != null)
                {
                    dungeonInfo.updateSetInfo(_m_dungeonRefreshTimeTag, setInfo);
                }
            }
            // 处理副本变更逻辑
            WinMsg.SendMsg(WinMsgType.ON_GUILD_DUNGEON_SET_CHG, setInfo?.getDungeonId());
        }

        /// <summary>
        /// 副本实例变更推送
        /// </summary>
        /// <param name="_msg"></param>
        public void onDungeonInstanceChg(GS2GC_037_051_OnDungeonInstanceChg _msg)
        {
            if (_msg == null)
                return;
            
            _checkResetInfo();
            
            // 根据副本ID查找对应的副本信息并更新实例数据
            GuildDungeon_InstanceInfo instanceInfo = _msg.getInstanceInfo();
            if (instanceInfo != null)
            {
                GuildDungeonInfo dungeonInfo = getDungeonInfoById(instanceInfo.getDungeonId());
                if (dungeonInfo != null)
                {
                    dungeonInfo.updateInstanceInfo(_m_dungeonRefreshTimeTag, instanceInfo);
                }
            }

            // 刷新红点提示
            _refreshRedTip();

            // 发送消息通知UI更新
            WinMsg.SendMsg(WinMsgType.ON_GUILD_DUNGEON_INSTANCE_CHG, instanceInfo?.getDungeonId());
        }

        /// <summary>
        /// 副本怪物变更推送
        /// </summary>
        /// <param name="_msg"></param>
        public void onDungeonMonsterChg(GS2GC_037_052_OnDungeonMonsterChg _msg)
        {
            if (_msg == null)
                return;

            _checkResetInfo();
            
            // 查找对应的副本信息，更新怪物血量
            foreach (GuildDungeonInfo dungeonInfo in _m_dungeonList)
            {
                if (dungeonInfo != null && _msg.getId() == dungeonInfo.instanceId)
                {
                    GuildDungeonMonster monster = dungeonInfo.getMonsterInfo(_msg.getMonsterId());
                    if (monster != null)
                    {
                        // 更新怪物血量
                        monster.updateHp(_msg.getHp());
                        break;
                    }
                }
            }

            // 发送消息通知UI更新
            WinMsg.SendMsg(WinMsgType.ON_GUILD_DUNGEON_MONSTER_CHG, _msg.getId(), _msg.getMonsterId());
        }

        /// <summary>
        /// 副本奖励变更推送
        /// </summary>
        /// <param name="_msg"></param>
        public void onDungeonGainedRewardChg(GS2GC_037_053_OnDungeonGainedRewardChg _msg)
        {
            if (_msg == null)
                return;

            _checkResetInfo();
            var gainedRewardMonsterIdList = _msg.getGainedRewardMonsterIdList();

            if (_m_dungeonList != null)
                foreach (GuildDungeonInfo dungeonInfo in _m_dungeonList)
                {
                    dungeonInfo?.updateGainRewardMonster(gainedRewardMonsterIdList);
                }

            // 刷新红点提示
            _refreshRedTip();

            // 发送消息通知UI更新
            WinMsg.SendMsg(WinMsgType.ON_GUILD_DUNGEON_REWARD_CHG, _msg);
        }
        
        public void onDungeonHeroFightChg(GS2GC_037_054_OnDungeonHeroFightChg _msg)
        {
            if (_msg == null)
                return;

            _checkResetInfo();
            
            GuildDungeon_FightHero info = _msg.getInfo();
            // 更新大臣出战信息
            if (_m_fightHeroMap != null && info != null)
            {
                _m_fightHeroMap[info.getHeroId()] = info;
            }
            
            // 刷新红点提示
            _refreshRedTip();
        }
        
        public void onDungeonTagMonsterChg(GS2GC_037_055_OnDungeonTagMonsterChg _msg)
        {
            if (_msg == null)
                return;

            _checkResetInfo();
            
            // 更新副本怪物标签状态
            GuildDungeonInfo dungeonInfo = getDungeonInfoByInstanceId(_msg.getId());
            if (dungeonInfo != null)
            {
                dungeonInfo.updateTagMonsterInfo(_msg.getTagMonsterIdList());
            }
            
            // 发送消息通知UI更新
            WinMsg.SendMsg(WinMsgType.ON_GUILD_DUNGEON_TAG_MONSTER_CHG, _msg.getId());
        }


        #endregion

        #region C2S

        /// <summary>
        /// 请求初始化协议
        /// </summary>
        private void _reqGuildDungeonInit()
        {
            NPGSClientListener.sendMsgByLog(new GC2GS_002_071_ReqGuildDungeonInit());
        }

        /// <summary>
        /// 请求获取副本全局配置
        /// </summary>
        /// <param name="_callback"></param>
        public void reqDungeonGlobalSet(Action<List<long>, int, int> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_037_001_ReqDungeontGlobalSet(),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_037_001_RetDungeontGlobalSet>((_msg) =>
                {
                    if (_msg != null)
                    {
                        _callback?.Invoke(_msg.getAutoStartDungeonIdList(), _msg.getAutoHour(), _msg.getAutoMin());
                    }
                    else
                    {
                        // 保持回调一致性
                        _callback?.Invoke(null, 0, 0);
                    }
                }));
        }

        /// <summary>
        /// 请求设置自动开启副本
        /// </summary>
        /// <param name="_dungeonId">副本ID</param>
        /// <param name="_isAutoStart">是否自动开启</param>
        /// <param name="_callback"></param>
        public void reqSetAutoStartDungeon(List<long> _dungeonIds, int _hour, int _min, Action<bool> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_037_002_ReqSetAutoStartDungeon(_dungeonIds, _hour, _min),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_037_002_RetSetAutoStartDungeon>((_succ, _msg) =>
                {
                    _callback?.Invoke(_succ);
                }));
        }

        /// <summary>
        /// 请求开启副本
        /// </summary>
        /// <param name="_dungeonId">副本ID</param>
        /// <param name="_startType">开启类型</param>
        /// <param name="_callback"></param>
        public void reqStartDungeon(long _dungeonId, EGuildDungeon_StartType _startType, Action<bool> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_037_003_ReqStartDungeon(_dungeonId, _startType),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_037_003_RetStartDungeon>((_succ, _msg) =>
                {
                    _callback?.Invoke(_succ);
                }));
        }

        /// <summary>
        /// 请求升级副本等级
        /// </summary>
        /// <param name="_dungeonId">副本ID</param>
        /// <param name="_callback"></param>
        public void reqUpgradeDungeonLvl(long _dungeonId,  int _curLevel,  Action<bool, GS2GC_037_004_RetUpgradeDungeonLvl> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_037_004_ReqUpgradeDungeonLvl(_dungeonId, _curLevel),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_037_004_RetUpgradeDungeonLvl>((_suc, _msg) =>
                {
                    _callback?.Invoke(_suc, _msg);
                }));
        }

        /// <summary>
        /// 请求攻击副本怪物
        /// </summary>
        /// <param name="_instanceId">副本ID</param>
        /// <param name="_monsterId">怪物ID</param>
        /// <param name="_heroId">英雄ID</param>
        /// <param name="_callback"></param>
        public void reqAttackDungeon(long _instanceId, long _monsterId, long _heroId, Action<bool, GS2GC_037_005_RetAttackDungeon> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_037_005_ReqAttackDungeon(_instanceId, _monsterId, _heroId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_037_005_RetAttackDungeon>((_succ, _msg) =>
                {
                    _callback?.Invoke(_succ, _msg);
                }));
        }
        
        /// <summary>
        /// 请求恢复出战次数
        /// </summary>
        public void reqRecoverHeroFight(long _heroId, Action<bool> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_037_006_ReqRecoverHeroFight(_heroId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_037_006_RetRecoverHeroFight>((_suc, _msg) =>
                {
                    _callback?.Invoke(_suc);
                }));
        }

        /// <summary>
        /// 请求攻击积分排行列表
        /// </summary>
        /// <param name="_callback"></param>
        public void reqDamageRank(int _page, int _num, Action<GS2GC_037_007_RetDamageRank> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_037_007_ReqDamageRank(_page, _num),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_037_007_RetDamageRank>((_msg) =>
                {
                    _callback?.Invoke(_msg);
                }));
        }

        /// <summary>
        /// 请求领取所有副本奖励
        /// </summary>
        /// <param name="_callback"></param>
        public void reqGainAllDungeonReward(Action<bool> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_037_008_ReqGainAllDungeonReward(),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_037_008_RetGainAllDungeonReward>((_suc, _msg) =>
                {
                    _callback?.Invoke(_suc);
                }));
        }
        
        public void reqGetDungeonLogList(long _dungeonId, Action<GS2GC_037_010_RetGetDungeonLogList> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_037_010_ReqGetDungeonLogList(_dungeonId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_037_010_RetGetDungeonLogList>((_suc, _msg) =>
                {
                    if(_suc)
                        _callback?.Invoke(_msg);
                }));
        }

        public void reqGainDungeonReward(long _instanceId, long _monsterId, Action<bool> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_037_011_ReqGainDungeonReward(_instanceId, _monsterId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_037_011_RetGainDungeonReward>((_suc, _msg) =>
                {
                    _callback?.Invoke(_suc);
                }));
        }
        
        public void reqSetTagMonsterList(long _instanceId, List<long> _tagMonsterIdList, Action<bool> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_037_012_ReqSetTagMonsterList(_instanceId, _tagMonsterIdList),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_037_012_RetSetTagMonsterList>((_suc, _msg) =>
                {
                    _callback?.Invoke(_suc);
                }));
        }
        #endregion
    }
}
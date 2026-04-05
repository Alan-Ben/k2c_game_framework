using System;
using System.Collections.Generic;
using ALPackage;
using Common.InnObj;
using GC2GS.p002_InitOp;
using GC2GS.p034_InnOp;
using GS2GC.p002_InitOp;
using GS2GC.p034_InnOp;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 旅店组件
    /// </summary>
    public partial class PlayerInnComponent : _ANPBasicPlayerComponent
    {
        // 当前接待的客人的相关信息
        [NotNull] private readonly InnReceiveInfo _m_receiveInfo;
        // 所有设施的数据，包括未建造的
        [ItemNotNull, NotNull] private readonly List<InnStationInfo> _m_stationList;
        // 所有菜品的数据，包括未解锁的
        [ItemNotNull, NotNull] private readonly List<InnDishInfo> _m_dishList;
        // 普通客人的图鉴数据
        [ItemNotNull, NotNull] private readonly List<InnNormalGuestHandbookInfo> _m_normalGuestHandbookInfoList;
        // 特殊客人的图鉴数据
        [ItemNotNull, NotNull] private readonly List<InnSpecialGuestHandbookInfo> _m_specialGuestHandbookInfoList;
        // 旅店的属性加成管理器
        [NotNull] private readonly CommonUnionBonusMgr _m_bonusMgr;
        
        // 旅店等级
        private InnLevelRefObj _m_levelRef;
        private InnLevelRefObj _m_nextLevelRef;
        // 旅店奖牌等级
        private InnMedalLevelRefObj _m_medalLevelRef;
        private InnMedalLevelRefObj _m_nextMedalLevelRef;
        // 旅店当前的人气值（用来升级）
        private long _m_popularity;
        // 第一次升级时的时间
        private long _m_firstTimeUpgradeTimeMs;
        // 下一个可建造的建筑
        private InnStationInfo _m_nextBuildableStation;
        // 客人相关的随机种子
        private CSSyncRandom _m_random;
        // 客人数据管理器
        [NotNull] private readonly InnGuestDataMgr _m_guestDataMgr;

        [NotNull] private readonly RedTipDealer _m_redTipDealer;
        
        private Action _m_onInnMainNodeShow;
        

        public PlayerInnComponent(NPPlayerComponentMgr _compMgr) 
            : base(_compMgr)
        {
            _m_stationList = new List<InnStationInfo>();
            _m_dishList = new List<InnDishInfo>();
            _m_normalGuestHandbookInfoList = new List<InnNormalGuestHandbookInfo>();
            _m_specialGuestHandbookInfoList = new List<InnSpecialGuestHandbookInfo>();
            _m_bonusMgr = new CommonUnionBonusMgr(EUnionBonusMgrTag.INN);
            _m_guestDataMgr = new InnGuestDataMgr();
            _m_receiveInfo = new InnReceiveInfo();
            _m_redTipDealer = new RedTipDealer(this);
        }


        /// <summary>
        /// 当接待信息发生了变化
        /// </summary>
        /// <remarks>
        /// 由服务端进行推送触发，一般在开始接待时推送，或是领取了奖励
        /// </remarks>
        public Action onReceiveChg;
        /// <summary>
        /// 当菜品信息发生变化
        /// </summary>
        public event Action<InnDishInfo> onDishChg;
        /// <summary>
        /// 当设施数据发生了变化
        /// </summary>
        public event Action<InnStationInfo> onStationChg;
        /// <summary>
        /// 当下一个可建设的设施发生了变化
        /// </summary>
        public event Action<InnStationInfo> onNextBuildableStationChg;
        /// <summary>
        /// 当旅店等级发生了变化
        /// </summary>
        public event Action onLevelChg;
        /// <summary>
        /// 当旅店奖牌等级发生了变化
        /// </summary>
        public event Action onMedalLevelChg;
        /// <summary>
        /// 当人气值发生了变化
        /// </summary>
        public event Action onPopularityChg;
        /// <summary>
        /// 当普通客人的图鉴数据变化了
        /// </summary>
        public event Action onNormalGuestHandbookInfoChg;
        /// <summary>
        /// 当特殊客人的图鉴数据变化了
        /// </summary>
        public event Action onSpecialGuestHandbookInfoChg;
        /// <summary>
        /// 当特殊客人的队列可能发生了变化
        /// </summary>
        public event Action onMaybeSpecialGuestListChg
        {
            add
            {
                _m_onInnMainNodeShow += value;
                onSpecialGuestHandbookInfoChg += value;
            }
            remove
            {
                onSpecialGuestHandbookInfoChg -= value;
                _m_onInnMainNodeShow -= value;
            }
        }

        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.INN; } }
        public override ENPPlayerCompType[] dependCompList { get { return null; } }
        public override bool canPreInit { get { return true; } }
        
        /// <summary>
        /// 当前的等级配置
        /// </summary>
        public InnLevelRefObj levelRef { get { return _m_levelRef; } }
        /// <summary>
        /// 下一级的等级配置
        /// </summary>
        public InnLevelRefObj nextLevelRef { get { return _m_nextLevelRef; } }
        /// <summary>
        /// 当前的奖牌等级
        /// </summary>
        public InnMedalLevelRefObj medalLevelRef { get { return _m_medalLevelRef; } }
        /// <summary>
        /// 下一级的奖牌等级
        /// </summary>
        public InnMedalLevelRefObj nextMedalLevelRef { get { return _m_nextMedalLevelRef; } }
        /// <summary>
        /// 人气值
        /// </summary>
        public long popularity { get { return _m_popularity; } }
        /// <summary>
        /// 下一个可建造的设施信息
        /// </summary>
        public InnStationInfo nextBuildableStationInfo { get { return _m_nextBuildableStation; } }
        /// <summary>
        /// 总菜品数量
        /// </summary>
        public int allDishCount { get { return _m_dishList.Count; } }
        public CommonUnionBonusMgr bonusMgr { get { return _m_bonusMgr; } }
        public long firstTimeUpgradeTimeMs { get { return _m_firstTimeUpgradeTimeMs; } }
        [NotNull] public InnReceiveInfo receiveInfo { get { return _m_receiveInfo; } }


        public override void presendInitProtocol()
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_002_057_ReqInnInit(), 
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_002_057_RetInnInit>((_isSuc, _msg) =>
                {
                    dealPreInitFunc(() =>
                    {
                        if (_isSuc)
                        {
                            _initData(_msg);
                            setInitDone();
                        }
                        else
                            setInitFail();
                    });
                }));
        }
        protected override void _dealInit()
        {
        }
        protected override void _onInitDone()
        {
            _m_bonusMgr.setParent(NPPlayer.instance.playerBonusMgr);
            _m_redTipDealer.init();
            
            WinMsg.RegisterMsgAct(WinMsgType.ON_NODE_CHG, _onNodeChg);
        }
        protected override void _onInitFail()
        {
            ALLog.Error("PlayerInnComponent init fail!");
        }
        protected override void _discard()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_NODE_CHG, _onNodeChg);
            
            _m_redTipDealer.clear();
            _m_bonusMgr.clear();
            
            _m_stationList.Clear();
            _m_dishList.Clear();
        }


        public InnGuestInfo getGuestInfoById(long _guestInstanceId)
        {
            if (_m_random == null)
                return null;
            
            // 计算解锁的菜品数量（基于客人ID）
            int unlockedDishCount = 0;
            foreach (InnDishInfo dishInfo in _m_dishList)
            {
                if (dishInfo.isUnlock && _guestInstanceId >= dishInfo.unlockGuestNum)
                    unlockedDishCount++;
                else
                    break;
            }

            int guestIndex = -1;
            // 计算解锁的普通客人图鉴数量（基于客人ID）
            int unlockedGuestHandbookCount = 0;
            foreach (InnNormalGuestHandbookInfo handbookInfo in _m_normalGuestHandbookInfoList)
            {
                if (handbookInfo.isUnlock && _guestInstanceId >= handbookInfo.unlockGuestNum)
                {
                    if (_guestInstanceId == handbookInfo.unlockGuestNum)
                        guestIndex = unlockedGuestHandbookCount;
                    unlockedGuestHandbookCount++;
                }
                else
                    break;
            }
            
            // 如果没有解锁的内容，返回null或默认值
            if (unlockedDishCount == 0 || unlockedGuestHandbookCount == 0)
                return null;
            
            // 使用客人ID作为随机种子，随机选择菜品和客人
            int dishIndex = _m_random.RandomInt(_guestInstanceId, unlockedDishCount);
            if (guestIndex < 0)
                guestIndex = _m_random.RandomInt(_guestInstanceId, unlockedGuestHandbookCount);
            
            // 获取选中的菜品和客人配置
            InnDishInfo selectedDish = _m_dishList[dishIndex];
            InnNormalGuestHandbookInfo selectedGuestHandbook = _m_normalGuestHandbookInfoList[guestIndex];
            
            return new InnGuestInfo(_guestInstanceId, selectedGuestHandbook.refObj, selectedDish);
        }
        /// <summary>
        /// 获取第一个特殊客人信息
        /// </summary>
        public InnSpecialGuestInfo tryGetFirstSpecialGuest()
        {
            foreach (InnSpecialGuestHandbookInfo handbookInfo in _m_specialGuestHandbookInfoList)
            {
                // 如果没有满足解锁条件，就跳过
                if (!handbookInfo.conditionEnable())
                    continue;

                // 如果解锁了，并且也领了博物馆奖励或者没有博物馆奖励，就也跳过
                if (handbookInfo.isUnlock)
                    continue;
                
                // 返回需要处理的特殊客人
                return new InnSpecialGuestInfo(handbookInfo);
            }
            
            return null;
        }
        public InnNormalGuestHandbookInfo tryGetFirstUnlockableNormalGuest()
        {
            foreach (InnNormalGuestHandbookInfo handbookInfo in _m_normalGuestHandbookInfoList)
            {
                if (handbookInfo.isUnlock)
                    continue;
                
                if (handbookInfo.checkCanUnlock())
                    return handbookInfo;
            }

            return null;
        }
        /// <summary>
        /// 获取所有设施列表
        /// </summary>
        /// <remarks>
        /// 返回的列表是新 new 的
        /// </remarks>
        [Pure, ItemNotNull, NotNull]
        public List<InnStationInfo> getStationList()
        {
            return new List<InnStationInfo>(_m_stationList);
        }
        /// <summary>
        /// 获取所有设施列表
        /// </summary>
        /// <remarks>
        /// 传入的列表会变成和组件内部的列表一模一样
        /// </remarks>
        public void getStationListNonAlloc(List<InnStationInfo> _list)
        {
            if (_list == null)
                return;
            
            _list.Clear();
            _list.AddRange(_m_stationList);
        }
        /// <summary>
        /// 根据设施 ID 获取设施信息
        /// </summary>
        public InnStationInfo getStationInfoById(long _stationId)
        {
            return _m_stationList.Find(_info => _info.stationId == _stationId);
        }
        /// <summary>
        /// 获取所有已建造的设施的总等级
        /// </summary>
        public int getStationTotalLevel()
        {
            int result = 0;
            foreach (InnStationInfo stationInfo in _m_stationList)
            {
                if (stationInfo.isBuilt)
                    result += stationInfo.level;
            }

            return result;
        }
        /// <summary>
        /// 获取指定建筑要建造时所需的建筑
        /// </summary>
        [Pure]
        public InnStationInfo getRequireStationInfo(InnStationInfo _stationInfo)
        {
            if (_stationInfo == null)
                return null;
            
            InnStationInfo requireStationInfo = null;
            foreach (InnStationInfo stationInfo in _m_stationList)
            {
                if (stationInfo == _stationInfo)
                    break;
                
                requireStationInfo = stationInfo;
            }
            
            return requireStationInfo;
        }
        /// <summary>
        /// 获取所有菜品列表
        /// </summary>
        /// <remarks>
        /// 返回的列表是新 new 的
        /// </remarks>
        [Pure, ItemNotNull, NotNull]
        public List<InnDishInfo> getDishList()
        {
            return new List<InnDishInfo>(_m_dishList);
        }
        /// <summary>
        /// 获取所有菜品列表
        /// </summary>
        /// <remarks>
        /// 传入的列表会变成和组件内部的列表一模一样
        /// </remarks>
        public void getDishListNonAlloc(List<InnDishInfo> _list)
        {
            if (_list == null)
                return;
            
            _list.Clear();
            _list.AddRange(_m_dishList);
        }
        /// <summary>
        /// 获取所有已解锁的菜品列表
        /// </summary>
        public void getUnlockedDishListNonAlloc(List<InnDishInfo> _list)
        {
            if (_list == null)
                return;
            
            _list.Clear();
            foreach (InnDishInfo dishInfo in _m_dishList)
            {
                if (dishInfo.isUnlock)
                    _list.Add(dishInfo);
            }
        }
        /// <summary>
        /// 获取已解锁的菜品的数量
        /// </summary>
        public int getUnlockedDishCount()
        {
            int result = 0;
            foreach (InnDishInfo dishInfo in _m_dishList)
            {
                if (dishInfo.isUnlock)
                    result++;
            }

            return result;
        }
        /// <summary>
        /// 获取所有未解锁的菜品列表
        /// </summary>
        public void getLockDishListNonAlloc(List<InnDishInfo> _list)
        {
            if (_list == null)
                return;
            
            _list.Clear();
            foreach (InnDishInfo dishInfo in _m_dishList)
            {
                if (!dishInfo.isUnlock)
                    _list.Add(dishInfo);
            }
        }
        /// <summary>
        /// 根据菜品 ID 获取菜品信息
        /// </summary>
        public InnDishInfo getDishInfoById(long _dishId)
        {
            return _m_dishList.Find(_info => _info.dishId == _dishId);
        }
        public InnDishInfo getNextUnlockedDish(InnDishInfo _dishInfo)
        {
            if (_dishInfo == null)
                return null;
            
            int index = _m_dishList.IndexOf(_dishInfo);
            if (index < 0 || index >= _m_dishList.Count - 1)
                return null;
            
            // 往下循环一遍，找到下一个已解锁的菜品
            for (int i = 0; i < _m_dishList.Count - 1; i++)
            {
                int checkIndex = NPGameUtility.intRepeat(index + i + 1, _m_dishList.Count);
                InnDishInfo checkDish = _m_dishList[checkIndex];
                if (checkDish.isUnlock)
                    return checkDish;
            }
            
            return null;
        }
        public InnDishInfo getPrevUnlockedDish(InnDishInfo _dishInfo)
        {
            if (_dishInfo == null)
                return null;
            
            int index = _m_dishList.IndexOf(_dishInfo);
            if (index <= 0 || index >= _m_dishList.Count)
                return null;
            
            // 往上循环一遍，找到上一个已解锁的菜品
            for (int i = 0; i < _m_dishList.Count - 1; i++)
            {
                int checkIndex = NPGameUtility.intRepeat(index - i - 1, _m_dishList.Count);
                InnDishInfo checkDish = _m_dishList[checkIndex];
                if (checkDish.isUnlock)
                    return checkDish;
            }
            
            return null;
        }
        public bool checkIsAllDishUnlocked()
        {
            foreach (InnDishInfo dishInfo in _m_dishList)
            {
                if (!dishInfo.isUnlock)
                    return false;
            }
            
            return true;
        }
        /// <summary>
        /// 获取所有普通客人的图鉴信息列表
        /// </summary>
        /// <remarks>
        /// 返回的列表是新 new 的
        /// </remarks>
        [Pure, ItemNotNull, NotNull]
        public List<InnNormalGuestHandbookInfo> getNormalGuestHandbookInfoList()
        {
            return new List<InnNormalGuestHandbookInfo>(_m_normalGuestHandbookInfoList);
        }
        /// <summary>
        /// 获取所有普通客人的图鉴信息列表
        /// </summary>
        /// <remarks>
        /// 传入的列表会变成和组件内部的列表一模一样
        /// </remarks>
        public void getNormalGuestHandbookInfoListNonAlloc(List<InnNormalGuestHandbookInfo> _list)
        {
            if (_list == null)
                return;
            
            _list.Clear();
            _list.AddRange(_m_normalGuestHandbookInfoList);
        }
        /// <summary>
        /// 根据客人 ID 获取普通客人的图鉴信息
        /// </summary>
        public InnNormalGuestHandbookInfo getNormalGuestHandbookInfoById(long _guestId)
        {
            return _m_normalGuestHandbookInfoList.Find(_info => _info.guestId == _guestId);
        }
        /// <summary>
        /// 获取所有特殊客人的图鉴信息列表
        /// </summary>
        /// <remarks>
        /// 返回的列表是新 new 的
        /// </remarks>
        public List<InnSpecialGuestHandbookInfo> getSpecialGuestHandbookInfoList()
        {
            return new List<InnSpecialGuestHandbookInfo>(_m_specialGuestHandbookInfoList);
        }
        /// <summary>
        /// 获取所有特殊客人的图鉴信息列表
        /// </summary>
        /// <remarks>
        /// 传入的列表会变成和组件内部的列表一模一样
        /// </remarks>
        public void getSpecialGuestHandbookInfoListNonAlloc(List<InnSpecialGuestHandbookInfo> _list)
        {
            if (_list == null)
                return;
            
            _list.Clear();
            _list.AddRange(_m_specialGuestHandbookInfoList);
        }
        /// <summary>
        /// 根据客人 ID 获取特殊客人的图鉴信息
        /// </summary>
        public InnSpecialGuestHandbookInfo getSpecialGuestHandbookInfoById(long _guestId)
        {
            return _m_specialGuestHandbookInfoList.Find(_info => _info.guestId == _guestId);
        }
        /// <summary>
        /// 获取下一个已建造的建筑
        /// </summary>
        [Pure]
        public InnStationInfo getNextBuiltStation(InnStationInfo _stationInfo)
        {
            if (_stationInfo == null)
                return null;
            
            int index = _m_stationList.IndexOf(_stationInfo);
            if (index < 0 || index >= _m_stationList.Count - 1)
                return null;
            
            // 往下循环一遍，找到下一个已建造的建筑
            for (int i = 0; i < _m_stationList.Count - 1; i++)
            {
                int checkIndex = NPGameUtility.intRepeat(index + i + 1, _m_stationList.Count);
                InnStationInfo checkStation = _m_stationList[checkIndex];
                if (checkStation.isBuilt)
                    return checkStation;
            }
            
            return null;
        }
        /// <summary>
        /// 获取上一个已建造的建筑
        /// </summary>
        [Pure]
        public InnStationInfo getPrevBuiltStation(InnStationInfo _stationInfo)
        {
            if (_stationInfo == null)
                return null;
            
            int index = _m_stationList.IndexOf(_stationInfo);
            if (index <= 0 || index >= _m_stationList.Count)
                return null;
            
            // 往上循环一遍，找到上一个已建造的建筑
            for (int i = 0; i < _m_stationList.Count - 1; i++)
            {
                int checkIndex = NPGameUtility.intRepeat(index - i - 1, _m_stationList.Count);
                InnStationInfo checkStation = _m_stationList[checkIndex];
                if (checkStation.isBuilt)
                    return checkStation;
            }
            
            return null;
        }
        /// <summary>
        /// 判断当前旅店设施是否可以升级
        /// </summary>
        [Pure]
        public bool canUpgradeStation()
        {
            foreach (InnStationInfo stationInfo in _m_stationList)
            {
                if (!stationInfo.isBuilt)
                    return false;
            }

            return true;
        }
        /// <summary>
        /// 获取奖牌可升级次数
        /// </summary>
        /// <returns>可升级次数</returns>
        [Pure]
        public int getMedalUpgradeCount()
        {
            if (_m_levelRef == null || _m_nextMedalLevelRef == null)
                return 0;
            
            long currentInnLevel = _m_levelRef.level;
            long checkLevel = _m_nextMedalLevelRef.level;
            int maxUpgradeCount = 0;
            
            // 从下一级的奖牌等级 +1 开始检查，看能升多少级
            while (true)
            {
                InnMedalLevelRefObj nextMedalRef = GRefdataCoreMgr.instance.getInnMedalLevelRef((int)checkLevel++);
                if (nextMedalRef == null)
                    break;
                
                // 检查是否满足旅店等级要求
                if (nextMedalRef.need_inn_level > currentInnLevel)
                    break;
                
                maxUpgradeCount++;
            }
            
            return maxUpgradeCount;
        }
        public int getUnlockedGuestNum(bool _includeSpecial = true)
        {
            int result = 0;
            foreach (InnNormalGuestHandbookInfo guestInfo in _m_normalGuestHandbookInfoList)
            {
                if (guestInfo.isUnlock)
                    result++;
            }
            if (_includeSpecial)
            {
                foreach (InnSpecialGuestHandbookInfo guestInfo in _m_specialGuestHandbookInfoList)
                {
                    if (guestInfo.isUnlock)
                        result++;
                }
            }
            return result;
        }
        
        public void getSettleReward(int _realRewardCount, Action<bool, GS2GC_034_006_RetInnSettle> _complete)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_034_006_ReqInnSettle(_realRewardCount), 
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_034_006_RetInnSettle>((_isSuc, _msg) =>
                {
                    _m_redTipDealer.refreshStationRedTip();
                    _m_redTipDealer.refreshDishRedTip();
                    WinMsg.SendMsg(WinMsgType.ON_INN_GET_SETTLE_REWARD);
                    _complete?.Invoke(_isSuc, _msg);
                }));
        }


        private void _initData(GS2GC_002_057_RetInnInit _msg)
        {
            // 先赋值上所有的配置数据
            _m_dishList.Clear();
            GRefdataCoreMgr.instance.innDishRefCore.dealAllRef(_dishRef =>
            {
                if (_dishRef == null)
                    return;
                
                InnDishInfo dishInfo = new InnDishInfo(_dishRef, _m_bonusMgr);
                _m_dishList.Add(dishInfo);
            });
            Dictionary<long, List<InnDishInfo>> uiDishDict = new Dictionary<long, List<InnDishInfo>>();
            foreach (InnDishInfo dishInfo in _m_dishList)
            {
                if (dishInfo == null)
                    continue;

                long stationId = dishInfo.refObj.station_id;
                if (!uiDishDict.TryGetValue(stationId, out List<InnDishInfo> uiList))
                {
                    uiList = new List<InnDishInfo>();
                    uiDishDict[stationId] = uiList;
                }
                uiList.Add(dishInfo);
            }
            _m_stationList.Clear();
            GRefdataCoreMgr.instance.innStationRefCore.dealAllRef(_stationRef =>
            {
                if (_stationRef == null)
                    return;

                List<InnDishInfo> dishInfos = new List<InnDishInfo>();
                foreach (long dishId in _stationRef.dish_id_list)
                {
                    InnDishInfo dishInfo = getDishInfoById(dishId);
                    if (dishInfo != null)
                        dishInfos.Add(dishInfo);
                }
                if (!uiDishDict.TryGetValue(_stationRef.id, out List<InnDishInfo> uiDishInfos))
                    uiDishInfos = new List<InnDishInfo>();
                InnStationInfo stationInfo = new InnStationInfo(_stationRef, dishInfos, uiDishInfos);
                _m_stationList.Add(stationInfo);
            });
            // 按照需要的人数排列
            _m_stationList.Sort((_a, _b) => _a.refObj.need_receive_guest_num.CompareTo(_b.refObj.need_receive_guest_num));
            _m_nextBuildableStation = _m_stationList.GetFirst();
            onNextBuildableStationChg?.Invoke(_m_nextBuildableStation);
            _m_normalGuestHandbookInfoList.Clear();
            GRefdataCoreMgr.instance.innGuestRefCore.dealAllRef(_guestRef =>
            {
                if (_guestRef == null)
                    return;
                
                InnNormalGuestHandbookInfo guestInfo = new InnNormalGuestHandbookInfo(_guestRef);
                _m_normalGuestHandbookInfoList.Add(guestInfo);
            });
            _m_specialGuestHandbookInfoList.Clear();
            GRefdataCoreMgr.instance.innSpecialGuestRefCore.dealAllRef(_specialGuestRef =>
            {
                if (_specialGuestRef == null)
                    return;
                
                InnSpecialGuestHandbookInfo guestInfo = new InnSpecialGuestHandbookInfo(_specialGuestRef);
                _m_specialGuestHandbookInfoList.Add(guestInfo);
            });

            // 开始初始化服务端的数值
            if (_msg != null)
            {
                Inn_Info serverInfo = _msg.getInnInfo();

                long seed = serverInfo.getRandomSeed();
                int level = serverInfo.getLevel();
                int medalLevel = serverInfo.getMedalLevel();
                long popularity = serverInfo.getPopularity();
                Inn_ReceiveList receiveList = serverInfo.getReceiveList();
                List<Inn_StationInfo> stationList = serverInfo.getStationList();
                List<Inn_DishInfo> dishList = serverInfo.getDishList();
                List<Inn_GuestInfo> normalGuestList = serverInfo.getGuestList();
                List<Inn_SpecialGuestInfo> specialGuestList = serverInfo.getSpecialGuestList();
                _m_firstTimeUpgradeTimeMs = serverInfo.getFirstTimeUpgradeTimeMs();

                _m_levelRef = GRefdataCoreMgr.instance.getInnLevelRef(level);
                _m_nextLevelRef = GRefdataCoreMgr.instance.getInnLevelRef(level + 1);
                _m_popularity = popularity;
                _m_random = new CSSyncRandom(seed);

                _onInnMedalLevelChg(medalLevel);

                _onInnReceiveChg(receiveList);

                // 添加建造完成的设施
                foreach (Inn_StationInfo stationInfo in stationList)
                {
                    _onInnStationAdd(stationInfo, false);
                }

                // 添加解锁的菜品
                foreach (Inn_DishInfo dishInfo in dishList)
                {
                    _onInnDishChg(dishInfo, false, true);
                }
                
                foreach (Inn_GuestInfo normalGuestInfo in normalGuestList)
                {
                    _onInnNormalGuestHandbookChg(normalGuestInfo, true);
                }
                
                foreach (Inn_SpecialGuestInfo specialGuestInfo in specialGuestList)
                {
                    _onInnSpecialGuestHandbookChg(specialGuestInfo);
                }
            }
            
            _m_dishList.Sort();
            _m_normalGuestHandbookInfoList.Sort();
        }


        internal void _onInnReceiveChg(GS2GC_034_050_OnInnReceiveChg _msg)
        {
            _onInnReceiveChg(_msg.getReceiveList());
        }
        internal void _onInnReceiveChg(Inn_ReceiveList _receiveInfo, bool _triggerEvent = true)
        {
            _m_receiveInfo._update(_receiveInfo);
            if (_triggerEvent)
                onReceiveChg?.Invoke();
        }
        internal void _onInnDishAdd(GS2GC_034_055_OnInnDishAdd _msg)
        {
            Inn_DishInfo serverInfo = _msg?.getDishInfo();
            _onInnDishChg(serverInfo, true, false);
        }
        internal void _onInnDishChg(GS2GC_034_052_OnInnDishChg _msg)
        {
            Inn_DishInfo serverInfo = _msg?.getDishInfo();
            _onInnDishChg(serverInfo, true, false);
        }
        internal void _onInnDishChg(Inn_DishInfo _serverInfo, bool _triggerEvent, bool _isInitial)
        {
            if (_serverInfo == null)
                return;
            
            InnDishInfo dishInfo = getDishInfoById(_serverInfo.getDishId());
            if (dishInfo == null)
            {
                ALLog.Error($"PlayerInnComponent._onInnDishChg: dishInfo is null, dishId={_serverInfo.getDishId()}");
                return;
            }
            
            bool isUnlock = !dishInfo.isUnlock && _serverInfo.getHadUnlock();
            bool isLevelChg = dishInfo.level != _serverInfo.getLevel();
            dishInfo._updateUnlock(_serverInfo.getHadUnlock(), _serverInfo.getStartLineUpId());
            dishInfo._updateHasRecipe(_serverInfo.getHadGainRecipe());
            dishInfo._updateLevel(_serverInfo.getLevel());
            dishInfo._updateFinesse(_serverInfo.getFinesse());
            if (!_isInitial)
                _m_dishList.Sort();
            if (_triggerEvent)
            {
                onDishChg?.Invoke(dishInfo);
                if (isUnlock)
                    WinMsg.SendMsg(WinMsgType.ON_INN_DISH_UNLOCK, dishInfo);
                if (isLevelChg)
                    WinMsg.SendMsg(WinMsgType.ON_INN_DISH_LEVEL_CHG, dishInfo);
            }
            
            _m_redTipDealer.refreshDishRedTip();
        }
        internal void _onInnStationChg(GS2GC_034_053_OnInnStationChg _msg)
        {
            Inn_StationInfo serverInfo = _msg?.getStationInfo();
            if (serverInfo == null)
                return;
            
            InnStationInfo stationInfo = getStationInfoById(serverInfo.getStationId());
            if (stationInfo == null)
            {
                ALLog.Error($"PlayerInnComponent._onInnStationChg: station is null, stationId={serverInfo.getStationId()}");
                return;
            }
            
            stationInfo._updateLevel(serverInfo.getLevel());
            onStationChg?.Invoke(stationInfo);
            WinMsg.SendMsg(WinMsgType.ON_INN_STATION_LEVEL_CHG, stationInfo);
            _m_redTipDealer.refreshStationRedTip();
            _m_redTipDealer.refreshDishRedTip();
        }
        internal void _onInnStationAdd(GS2GC_034_056_OnInnStationAdd _msg)
        {
            Inn_StationInfo serverInfo = _msg?.getStationInfo();
            _onInnStationAdd(serverInfo);
        }
        internal void _onInnStationAdd(Inn_StationInfo _serverInfo, bool _triggerEvent = true)
        {
            if (_serverInfo == null)
                return;
            
            InnStationInfo stationInfo = getStationInfoById(_serverInfo.getStationId());
            if (stationInfo == null)
            {
                ALLog.Error($"PlayerInnComponent._onInnStationAdd: stationInfo.refObj is null, stationId={_serverInfo.getStationId()}");
                return;
            }
            
            stationInfo._setBuilt();
            stationInfo._updateLevel(_serverInfo.getLevel());
            _refreshNextBuildableStation();
            if (_triggerEvent)
            {
                onStationChg?.Invoke(stationInfo);
                WinMsg.SendMsg(WinMsgType.ON_INN_STATION_LEVEL_CHG, stationInfo);
            }
            _m_redTipDealer.refreshStationRedTip();
            _m_redTipDealer.refreshDishRedTip();
        }
        internal void _onInnLevelChg(GS2GC_034_057_OnInnLevelChg _msg)
        {
            if (_msg == null)
                return;
            
            int newLevel = _msg.getNewLevel();
            if (_m_levelRef != null && newLevel == _m_levelRef.level)
                return;
            
            int oldLevel = _m_levelRef != null ? (int)_m_levelRef.level : 0;
            _m_levelRef = GRefdataCoreMgr.instance.getInnLevelRef(newLevel);
            _m_nextLevelRef = GRefdataCoreMgr.instance.getInnLevelRef(newLevel + 1);
            onLevelChg?.Invoke();
            WinMsg.SendMsg(WinMsgType.ON_INN_LEVEL_CHG, oldLevel, newLevel);
            _m_redTipDealer.refreshMedalLevelRedTip();
        }
        internal void _onInnPopularityChg(GS2GC_034_058_OnInnPopularityChg _msg)
        {
            if (_msg == null)
                return;
            
            long newPopularity = _msg.getNewPopularity();
            if (_m_popularity == newPopularity)
                return;
            
            long oldPopularity = _m_popularity;
            _m_popularity = newPopularity;
            onPopularityChg?.Invoke();
            WinMsg.SendMsg(WinMsgType.ON_INN_POPULARITY_CHG, oldPopularity, _m_popularity);
        }
        internal void _onInnMedalLevelChg(GS2GC_034_059_OnInnMedalLevelChg _msg)
        {
            if (_msg == null)
                return;
            
            int newMedalLevel = _msg.getMedalLevel();
            _onInnMedalLevelChg(newMedalLevel);
        }
        internal void _onInnMedalLevelChg(int _medalLevel)
        {
            if (_m_medalLevelRef != null && _m_medalLevelRef.level == _medalLevel)
                return;

            if (_m_medalLevelRef != null)
                _m_bonusMgr.removeTotalModifier(_m_medalLevelRef.bonus_prop_modifier);
            _m_medalLevelRef = GRefdataCoreMgr.instance.getInnMedalLevelRef(_medalLevel);
            _m_nextMedalLevelRef = GRefdataCoreMgr.instance.getInnMedalLevelRef(_medalLevel + 1);
            if (_m_medalLevelRef != null)
                _m_bonusMgr.addTotalModifier(_m_medalLevelRef.bonus_prop_modifier);
            onMedalLevelChg?.Invoke();
            _m_redTipDealer.refreshMedalLevelRedTip();
            
            WinMsg.SendMsg(WinMsgType.ON_INN_MEDAL_LEVEL_CHG);
        }
        internal void _onInnNormalGuestHandbookChg(GS2GC_034_060_OnInnGuestChg _msg)
        {
            Inn_GuestInfo serverInfo = _msg?.getGuestInfo();
            _onInnNormalGuestHandbookChg(serverInfo, false);
        }
        internal void _onInnNormalGuestHandbookChg(Inn_GuestInfo _serverInfo, bool _isInitial)
        {
            if (_serverInfo == null)
                return;
            
            InnNormalGuestHandbookInfo guestInfo = getNormalGuestHandbookInfoById(_serverInfo.getGuestId());
            if (guestInfo == null)
            {
                ALLog.Error($"PlayerInnComponent._onInnNormalGuestHandbookChg: guestInfo is null, guestId={_serverInfo.getGuestId()}");
                return;
            }
            
            guestInfo._updateIsUnlock(true, _serverInfo.getStartLineUpId());
            guestInfo._updateHadDrawReward(_serverInfo.getHadDrawHandbookReward());
            if (!_isInitial)
                _m_normalGuestHandbookInfoList.Sort();
            
            onNormalGuestHandbookInfoChg?.Invoke();
            _m_redTipDealer.refreshNormalGuestHandbookRewardRedTip();
        }
        internal void _onInnSpecialGuestHandbookChg(GS2GC_034_061_OnInnSpecialGuestChg _msg)
        {
            Inn_SpecialGuestInfo serverInfo = _msg?.getSpecialGuestInfo();
            _onInnSpecialGuestHandbookChg(serverInfo);
        }
        internal void _onInnSpecialGuestHandbookChg(Inn_SpecialGuestInfo _serverInfo)
        {
            if (_serverInfo == null)
                return;
            
            InnSpecialGuestHandbookInfo guestInfo = getSpecialGuestHandbookInfoById(_serverInfo.getSpecialGuestId());
            if (guestInfo == null)
            {
                ALLog.Error($"PlayerInnComponent._onInnSpecialGuestHandbookChg: guestInfo is null, guestId={_serverInfo.getSpecialGuestId()}");
                return;
            }
            
            guestInfo._updateIsUnlock(_serverInfo.getHadBeenServe());
            guestInfo._updateHadDrawReward(_serverInfo.getHadDrawHandbookReward());
            onSpecialGuestHandbookInfoChg?.Invoke();
            _m_redTipDealer.refreshSpecialGuestHandbookRewardRedTip();
        }
        internal void _onInnFirstTimeUpgradeTimeMsChg(GS2GC_034_063_OnInnFirstTimeUpgradeTimeMsChg _msg)
        {
            if (_msg == null)
                return;

            _m_firstTimeUpgradeTimeMs = _msg.getFirstTimeUpgradeTimeMs();
        }


        private void _refreshNextBuildableStation()
        {
            InnStationInfo nextStation = null;
            // _m_stationList 已经按照设施需要接待的客人数量从小到大排列了
            foreach (InnStationInfo stationInfo in _m_stationList)
            {
                if (stationInfo.isBuilt) 
                    continue;
                
                nextStation = stationInfo;
                break;
            }

            if (nextStation == _m_nextBuildableStation)
                return;

            _m_nextBuildableStation = nextStation;
            onNextBuildableStationChg?.Invoke(_m_nextBuildableStation);
        }
        private void _onNodeChg()
        {
            if (QueueMgr.instance._lastNode is GNodeInnMain)
                _m_onInnMainNodeShow?.Invoke();
        }
    }
}
using System;
using System.Collections.Generic;
using ALPackage;
using Common.TreasureHuntEnum;
using Common.TreasureHuntObj;
using GC2GS.p036_TreasureHuntOp;
using GS2GC.p002_InitOp;
using GS2GC.p036_TreasureHuntOp;
using JetBrains.Annotations;

namespace GOE
{
    public partial class TreasureHuntComponent : _ANPBasicPlayerComponent
    {
        private TreasureHuntStationInfo _m_iStationInfo;//太空舱信息
        
        /// <summary>
        /// 已获取的矿石信息列表
        /// </summary>
        [NotNull] private List<TreasureHuntGotOreInfo> _m_lGotOreInfoList = new List<TreasureHuntGotOreInfo>();
        
        /// <summary>
        /// 已获取的奇物信息列表
        /// </summary>
        [NotNull] private List<TreasureHuntGotTreasureInfo> _m_lGotTreasureInfoList = new List<TreasureHuntGotTreasureInfo>();

        /// <summary>
        /// 所有组合图鉴信息列表
        /// </summary>
        [NotNull] private List<TreasureHuntCompositeCatalogInfo> _m_lCompositeCatalogInfoList = new List<TreasureHuntCompositeCatalogInfo>();
        
        /// <summary>
        /// 奇物产出（可领取钻石）信息列表
        /// </summary>
        [NotNull] private List<TreasureHuntTreasureOutputInfo> _m_lTreasureOutputInfoList = new List<TreasureHuntTreasureOutputInfo>();

        private List<long> _m_lUpgradeStationLevelList;//太空舱升级等级列表
        
        private TreasureHuntBonusMgr _m_bonusMgr;//太空寻宝属性加成管理器
        private TreasureHuntSaver _m_saver;
        [NotNull] private RedTipDealer _m_redTipDealer;
        
        public TreasureHuntComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
            _m_bonusMgr = new TreasureHuntBonusMgr(this);
            _m_redTipDealer = new RedTipDealer(this);
        }

        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.TREASURE_HUNT; } }
        public override ENPPlayerCompType[] dependCompList { get { return null; } }
        public override bool canPreInit { get { return true; } }

        /// <summary>
        /// 太空舱信息
        /// </summary>
        public TreasureHuntStationInfo stationInfo { get { return _m_iStationInfo; } }
        
        /// <summary>
        /// 已获取的奇物信息列表
        /// </summary>
        [NotNull] public List<TreasureHuntGotTreasureInfo> gotTreasureInfoList { get { return _m_lGotTreasureInfoList; } }
        
        /// <summary>
        /// 已获取的矿石信息列表
        /// </summary>
        [NotNull] public List<TreasureHuntGotOreInfo> gotOreInfoList { get { return _m_lGotOreInfoList; } }

        /// <summary>
        /// 全部图鉴组合信息列表
        /// </summary>
        [NotNull] public List<TreasureHuntCompositeCatalogInfo> compositeCatalogInfoList { get { return _m_lCompositeCatalogInfoList; } }
        
        /// <summary>
        /// 太空舱升级等级列表
        /// </summary>
        public List<long> upgradeStationLevelList { get { return _m_lUpgradeStationLevelList; } }

        /// <summary>
        /// 太空寻宝 本地数据存储
        /// </summary>
        public TreasureHuntSaver saver { get { return _m_saver; } }
        
        /// <summary>
        /// 太空寻宝 - 红点处理器
        /// </summary>
        [NotNull] public RedTipDealer redTipDealer { get { return _m_redTipDealer; } }
        
        /// <summary>
        /// 奇物产出（可领取钻石）信息列表（直接引用，勿修改内部结构）
        /// </summary>
        [NotNull] public List<TreasureHuntTreasureOutputInfo> treasureOutputInfoList { get { return _m_lTreasureOutputInfoList; } }
        
        public override void presendInitProtocol()
        {
            reqTreasureHuntInit();
        }

        protected override void _dealInit()
        {
            try
            {
                _m_saver = new TreasureHuntSaver();
                _m_saver.init();
                
                _m_lCompositeCatalogInfoList.Clear();
                foreach (var compositeCatalogRefObj in GRefdataCoreMgr.instance.treasureHuntCompositeCatalogRefCore.refList)
                {
                    if(compositeCatalogRefObj == null)
                        continue;
                    
                    _m_lCompositeCatalogInfoList.Add(new TreasureHuntCompositeCatalogInfo(compositeCatalogRefObj));
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"TreasureHuntSaver init时出错：{e}");
                _m_saver = new TreasureHuntSaver();
                _m_saver.delete();
                _m_saver.init();
            }
        }

        protected override void _onInitDone()
        {
            _m_bonusMgr?.init();
            _m_redTipDealer.init();
        }

        protected override void _onInitFail()
        {
        }

        protected override void _discard()
        {
            _m_lGotOreInfoList.Clear();
            _m_lGotTreasureInfoList.Clear();
            _m_lCompositeCatalogInfoList.Clear();
            _m_lTreasureOutputInfoList.Clear();
            _m_lUpgradeStationLevelList?.Clear();
            
            _m_bonusMgr?.discard();
            _m_redTipDealer?.clear();
            
            _m_saver = null;
        }

        #region 奇物相关方法

        /// <summary>
        /// 获取已获取的
        /// </summary>
        /// <param name="_treasureId"></param>
        /// <returns></returns>
        public TreasureHuntGotTreasureInfo getGotTreasureInfo(long _treasureId)
        {
            foreach (var treasureInfo in _m_lGotTreasureInfoList)
            {
                if (treasureInfo != null && treasureInfo.treasureId == _treasureId)
                    return treasureInfo;
            }

            return null;
        }

        public List<TreasureHuntGotTreasureInfo> getGotTreasureInfoList()
        {
            List<TreasureHuntGotTreasureInfo> resList = new List<TreasureHuntGotTreasureInfo>();
            resList.AddRange(_m_lGotTreasureInfoList);
            return resList;
        }

        public List<TreasureHuntGotTreasureInfo> getGotTreasureInfoList(Func<TreasureHuntGotTreasureInfo, bool> _func)
        {
            if (_func == null)
                return null;
            
            List<TreasureHuntGotTreasureInfo> resList = new List<TreasureHuntGotTreasureInfo>();
            foreach (var gotTreasureInfo in _m_lGotTreasureInfoList)
            {
                if(_func(gotTreasureInfo))
                    resList.Add(gotTreasureInfo);
            }
            return resList;
        }

        public void getGotTreasureInfoList(List<TreasureHuntGotTreasureInfo> _resList)
        {
            if(_resList == null)
                return;
            _resList.Clear();

            _resList.AddRange(_m_lGotTreasureInfoList);
        }
        
        public void getGotTreasureInfoList(List<TreasureHuntGotTreasureInfo> _resList, Func<TreasureHuntGotTreasureInfo, bool> _func)
        {
            if(_resList == null || _func == null)
                return;
            _resList.Clear();

            foreach (var gotTreasureInfo in _m_lGotTreasureInfoList)
            {
                if(_func(gotTreasureInfo))
                    _resList.Add(gotTreasureInfo);
            }
        }

        public void dealAllGotTreasure(Action<TreasureHuntGotTreasureInfo> _action)
        {
            if(_action == null)
                return;

            foreach (var gotTreasureInfo in _m_lGotTreasureInfoList)
            {
                _action(gotTreasureInfo);
            }
        }

        #endregion
        
        #region 矿石相关方法

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TreasureHuntGotOreInfo getGotOreInfo(long _oreId)
        {
            foreach (var oreInfo in _m_lGotOreInfoList)
            {
                if (oreInfo != null && oreInfo.oreId == _oreId)
                    return oreInfo;
            }

            return null;
        }

        /// <summary>
        /// 获取所有已获取的矿石信息列表
        /// </summary>
        /// <returns></returns>
        public List<TreasureHuntGotOreInfo> getGotOreInfoList()
        {
            List<TreasureHuntGotOreInfo> resList = new List<TreasureHuntGotOreInfo>();
            resList.AddRange(_m_lGotOreInfoList);
            return resList;
        }
        
        /// <summary>
        /// 获取所有已获取的矿石信息列表
        /// </summary>
        /// <param name="_resList"></param>
        public void getGotOreInfoList(List<TreasureHuntGotOreInfo> _resList)
        {
            if(_resList == null)
                return;
            _resList.Clear();

            _resList.AddRange(_m_lGotOreInfoList);
        }

        /// <summary>
        /// 获取所有待处理矿石列表
        /// </summary>
        public List<TreasureHuntCommonOreInfo> getPendingOreList()
        {
            // 取出所有待处理矿石
            List<TreasureHuntCommonOreInfo> pendingOreList = new List<TreasureHuntCommonOreInfo>();
            foreach (var oreInfo in _m_lGotOreInfoList)
            {
                if (oreInfo == null)
                    continue;
                
                if(oreInfo.normalPendingOreInfo != null)
                    pendingOreList.Add(oreInfo.normalPendingOreInfo);
                
                if(oreInfo.advancePendingOreInfo != null)
                    pendingOreList.Add(oreInfo.advancePendingOreInfo);
            }

            return pendingOreList;
        }
        
        /// <summary>
        /// 获取所有待处理矿石列表
        /// </summary>
        public void getPendingOreList(List<_ITreasureHuntOreInfo> _pendingOreList)
        {
            if(_pendingOreList == null)
                return;
            _pendingOreList.Clear();
            
            // 取出所有待处理矿石
            foreach (var oreInfo in _m_lGotOreInfoList)
            {
                if (oreInfo == null)
                    continue;
                
                if(oreInfo.normalPendingOreInfo != null && oreInfo.normalPendingOreInfo.num > 0)
                    _pendingOreList.Add(oreInfo.normalPendingOreInfo);
                
                if(oreInfo.advancePendingOreInfo != null && oreInfo.advancePendingOreInfo.num > 0)
                    _pendingOreList.Add(oreInfo.advancePendingOreInfo);
            }

            return;
        }
        
        /// <summary>
        /// 获取待处理矿石数量
        /// </summary>
        /// <returns></returns>
        public int getPendingOreCount()
        {
            int count = 0;
            // 统计所有待处理矿石数量
            foreach (var oreInfo in _m_lGotOreInfoList)
            {
                if (oreInfo == null)
                    continue;

                if (oreInfo.normalPendingOreInfo != null)
                    count += oreInfo.normalPendingOreInfo.num;

                if (oreInfo.advancePendingOreInfo != null)
                    count += oreInfo.advancePendingOreInfo.num;
            }

            return count;
        }

        #endregion
        
        #region 组合图鉴相关方法

        /// <summary>
        /// 创建组合图鉴信息
        /// </summary>
        /// <param name="_compositeCatalogId"></param>
        /// <returns></returns>
        [NotNull] public TreasureHuntCompositeCatalogInfo createCompositeCatalogInfo(long _compositeCatalogId)
        {
            TreasureHuntCompositeCatalogInfo catalogInfo = new TreasureHuntCompositeCatalogInfo(_compositeCatalogId);
            _m_lCompositeCatalogInfoList.Add(catalogInfo);
            
            _m_bonusMgr?.addCompositeCatalog(catalogInfo);
            
            return catalogInfo;
        }
        
        /// <summary>
        /// 创建组合图鉴信息
        /// </summary>
        /// <param name="_compositeCatalogId"></param>
        /// <returns></returns>
        [NotNull] public TreasureHuntCompositeCatalogInfo createCompositeCatalogInfo(TreasureHuntCompositeCatalogRefObj _compositeCatalogRefObj)
        {
            TreasureHuntCompositeCatalogInfo catalogInfo = new TreasureHuntCompositeCatalogInfo(_compositeCatalogRefObj);
            _m_lCompositeCatalogInfoList.Add(catalogInfo);

            _m_bonusMgr?.addCompositeCatalog(catalogInfo);
            
            return catalogInfo;
        }
        
        /// <summary>
        /// 获取组合图鉴信息
        /// </summary>
        /// <returns></returns>
        public TreasureHuntCompositeCatalogInfo getCompositeCatalogInfo(long _compositeCatalogId)
        {
            foreach (var compositeCatalogInfo in _m_lCompositeCatalogInfoList)
            {
                if (compositeCatalogInfo != null && compositeCatalogInfo.compositeCatalogId == _compositeCatalogId)
                    return compositeCatalogInfo;
            }

            return null;
        }
        
        /// <summary>
        /// 获取所有组合图鉴信息列表
        /// </summary>
        /// <returns></returns>
        public List<TreasureHuntCompositeCatalogInfo> getCompositeCatalogInfoList()
        {
            List<TreasureHuntCompositeCatalogInfo> resList = new List<TreasureHuntCompositeCatalogInfo>();
            resList.AddRange(_m_lCompositeCatalogInfoList);
            return resList;
        }
        
        /// <summary>
        /// 获取所有组合图鉴信息列表
        /// </summary>
        /// <param name="_resList"></param>
        public void getCompositeCatalogInfoList(List<TreasureHuntCompositeCatalogInfo> _resList)
        {
            if(_resList == null)
                return;
            _resList.Clear();
                
            _resList.AddRange(_m_lCompositeCatalogInfoList);
        }
        
        /// <summary>
        /// 更新组合图鉴状态
        /// </summary>
        public void updateCompositeCatalogState(long _oreId)
        {
            foreach (var catalogInfo in _m_lCompositeCatalogInfoList)
            {
                if(catalogInfo == null || !catalogInfo.containsOre(_oreId))
                    return;
                
                ETreasureHuntCompositeCatalogState oldState = catalogInfo.state;
                catalogInfo.updateCompositeCatelogState();
                _checkCompositeCatalogStateChg(catalogInfo, oldState);
            }
        }

        private void _checkCompositeCatalogStateChg(TreasureHuntCompositeCatalogInfo _catalogInfo, ETreasureHuntCompositeCatalogState _oldState)
        {
            if(_catalogInfo == null)
                return;

            ETreasureHuntCompositeCatalogState newState = _catalogInfo.state;
            if ((_oldState is ETreasureHuntCompositeCatalogState.NONE or ETreasureHuntCompositeCatalogState.NOT_GET) &&
                newState is ETreasureHuntCompositeCatalogState.GOT_NORMAL)
            {
                WinMsg.SendMsg(WinMsgType.ON_TREASURE_HUNT_UNLOCK_NORMAL_COMPOSITE_CATALOG, _catalogInfo);
            }
            else if (!(_oldState is ETreasureHuntCompositeCatalogState.GOT_ADVANCED) &&
                     newState is ETreasureHuntCompositeCatalogState.GOT_ADVANCED)
            {
                WinMsg.SendMsg(WinMsgType.ON_TREASURE_HUNT_UNLOCK_ADVANCED_COMPOSITE_CATALOG, _catalogInfo);
            }
        }

        /// <summary>
        /// 获取已集齐的组合数量(不区分普通/高级)
        /// </summary>
        /// <returns></returns>
        public int getCollectedCompositeCount()
        {
            int count = 0;
            foreach (var catalogInfo in _m_lCompositeCatalogInfoList)
            {
                if (catalogInfo?.isCollected ?? false)
                {
                    count++;
                }
            }
            return count;
        }

        #endregion
        
        #region 奇物产出相关

        /// <summary>
        /// 获取指定奇物产出信息
        /// </summary>
        public TreasureHuntTreasureOutputInfo getTreasureOutputInfo(long _treasureId)
        {
            foreach (var outputInfo in _m_lTreasureOutputInfoList)
            {
                if (outputInfo != null && outputInfo.treasureId == _treasureId)
                    return outputInfo;
            }
            return null;
        }

        /// <summary>
        /// 获取奇物产出信息列表（返回拷贝）
        /// </summary>
        public List<TreasureHuntTreasureOutputInfo> getTreasureOutputInfoList()
        {
            List<TreasureHuntTreasureOutputInfo> resList = new List<TreasureHuntTreasureOutputInfo>();
            resList.AddRange(_m_lTreasureOutputInfoList);
            return resList;
        }
        
        /// <summary>
        /// 获取奇物产出信息列表（写入外部 list）
        /// </summary>
        public void getTreasureOutputInfoList(List<TreasureHuntTreasureOutputInfo> _resList)
        {
            if (_resList == null)
                return;
            _resList.Clear();
            _resList.AddRange(_m_lTreasureOutputInfoList);
        }

        /// <summary>
        /// 是否有可领取的奇物产出
        /// </summary>
        /// <returns></returns>
        public bool hasCanDrawTreasureOutput(long _labId)
        {
            foreach (var outputInfo in _m_lTreasureOutputInfoList)
            {
                if (outputInfo == null || !outputInfo.canDraw)
                    continue;

                TreasureHuntTreasureRefObj treasureHuntTreasureRefObj =
                    GRefdataCoreMgr.instance.treasureHuntTreasureRefCore.getRef(outputInfo.treasureId);
                if (treasureHuntTreasureRefObj != null && treasureHuntTreasureRefObj.related_lab_id == _labId)
                    return true;
            }
            
            return false;
        }
        
        /// <summary>
        /// 获取第一个有奇物产出可领取的实验室ID
        /// </summary>
        /// <returns></returns>
        public long getFirstCanDrawTreasureOutputLabId()
        {
            long labId = 0;
            foreach (var outputInfo in _m_lTreasureOutputInfoList)
            {
                if (outputInfo == null || !outputInfo.canDraw)
                    continue;
                
                TreasureHuntTreasureRefObj treasureHuntTreasureRefObj =
                    GRefdataCoreMgr.instance.treasureHuntTreasureRefCore.getRef(outputInfo.treasureId);
                if(treasureHuntTreasureRefObj == null)
                    continue;
                
                if(labId <= 0 || labId > treasureHuntTreasureRefObj.related_lab_id)
                    labId = treasureHuntTreasureRefObj.related_lab_id;
            }

            return labId;
        }
        
        #endregion
        
        /// <summary>
        /// 尝试显示太空舱升级弹窗
        /// </summary>
        public void tryShowStationUpgradePopWnd(Action _onShowDone = null)
        {
            if (_m_lUpgradeStationLevelList == null || _m_lUpgradeStationLevelList.Count <= 0)
            {
                _onShowDone?.Invoke();
                return;
            }

            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(_m_lUpgradeStationLevelList.Count);
            stepCounter.regAllDoneDelegate(_onShowDone);
            
            foreach (var stationLevel in _m_lUpgradeStationLevelList)
            {
                NPUINoticeMgr.instance.addDealer(new NoticeDealer_CustomAction((_setDealDone) =>
                {
                    GGUIWndTreasureHuntStationUpgrade.instance.load();
                    GGUIWndTreasureHuntStationUpgrade.instance.regLoadDoneDelegate(() =>
                    {
                        GGUIWndTreasureHuntStationUpgrade.instance.showWnd();
                        GGUIWndTreasureHuntStationUpgrade.instance.setData(stationLevel);
                    });
                }, NPNoticeType.g_AllTypeArr, ()=>
                {
                    GGUIWndTreasureHuntStationUpgrade.instance.discard();
                    stepCounter.addDoneStepCount();
                }, default, UINodeTagConst.C_TREASURE_HUNT_STATION_UPGRADE, true, true, true, true));
            }
            
            _m_lUpgradeStationLevelList?.Clear();
        }
        
        #region S2C

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_msg"></param>
        public void retTreasureHuntInit(GS2GC_002_066_RetTreasureHuntInit _msg)
        {
            if(_msg == null || _msg.getInfo() == null)
                return;

            _m_iStationInfo = null;
            if (_msg.getInfo().getStationInfo() != null)
            {
                _m_iStationInfo = new TreasureHuntStationInfo(_msg.getInfo().getStationInfo());
            }
            
            _m_lGotOreInfoList.Clear();
            if (_msg.getInfo().getOreList() != null)
            {
                foreach (var serverOreInfo in _msg.getInfo().getOreList())
                {
                    if(serverOreInfo == null)
                        continue;
                    
                    _m_lGotOreInfoList.Add(new TreasureHuntGotOreInfo(serverOreInfo));
                }
            }
            
            _m_lGotTreasureInfoList.Clear();
            if (_msg.getInfo().getTreasureList() != null)
            {
                foreach (var serverTreasureInfo in _msg.getInfo().getTreasureList())
                {
                    if(serverTreasureInfo == null)
                        continue;
                    
                    _m_lGotTreasureInfoList.Add(new TreasureHuntGotTreasureInfo(serverTreasureInfo));
                }
            }

            // 奇物产出列表
            _m_lTreasureOutputInfoList.Clear();
            if (_msg.getInfo().getTreasureOutputList() != null)
            {
                foreach (var serverTreasureOutput in _msg.getInfo().getTreasureOutputList())
                {
                    if (serverTreasureOutput == null)
                        continue;
                    _m_lTreasureOutputInfoList.Add(new TreasureHuntTreasureOutputInfo(serverTreasureOutput));
                }
            }

            TreasureHuntCompositeCatalogInfo compositeCatalogInfo = null;
            if (_msg.getInfo().getCompositeList() != null)
            {
                foreach (var serverCompositeInfo in _msg.getInfo().getCompositeList())
                {
                    if(serverCompositeInfo == null)
                        continue;

                    compositeCatalogInfo = getCompositeCatalogInfo(serverCompositeInfo.getRefId());
                    if (compositeCatalogInfo == null)
                    {
                        compositeCatalogInfo = new TreasureHuntCompositeCatalogInfo(serverCompositeInfo.getRefId());
                        _m_lCompositeCatalogInfoList.Add(compositeCatalogInfo);
                    }
                    
                    compositeCatalogInfo.updateFromServerInfo(serverCompositeInfo);
                }
            }
            
            setInitDone();
        }

        /// <summary>
        /// 太空寻宝-获得奇物
        /// </summary>
        /// <param name="_msg"></param>
        public void onTreasureHuntTreasureAdd(GS2GC_036_051_OnTreasureHuntTreasureAdd _msg)
        {
            if(_msg == null || _msg.getTreasureInfo() == null)
                return;

            TreasureHuntGotTreasureInfo treasureInfo = getGotTreasureInfo(_msg.getTreasureInfo().getTreasureId());
            if (treasureInfo == null)
            {
                treasureInfo = new TreasureHuntGotTreasureInfo(_msg.getTreasureInfo());
                _m_lGotTreasureInfoList.Add(treasureInfo);
                
                _m_bonusMgr?.addTreasure(treasureInfo);
                
                WinMsg.SendMsg(WinMsgType.ON_TREASURE_HUNT_UNLOCK_TREASURE, treasureInfo);
            }
            else
            {
                _m_bonusMgr?.removeTreasure(treasureInfo);
                
                treasureInfo.updateTreasureInfo(_msg.getTreasureInfo());
                
                _m_bonusMgr?.addTreasure(treasureInfo);
            }
        }
        
        /// <summary>
        /// 太空寻宝-奇物等级变更
        /// </summary>
        /// <param name="_msg"></param>
        public void onTreasureHuntTreasureLevelChg(GS2GC_036_052_OnTreasureHuntTreasureLevelChg _msg)
        {
            if(_msg == null)
                return;

            TreasureHuntGotTreasureInfo treasureInfo = getGotTreasureInfo(_msg.getTreasureId());
            if (treasureInfo == null)
                return;
         
            _m_bonusMgr?.removeSkill(treasureInfo.skillInfo);
            
            treasureInfo.updateTreasureSkillLevel(_msg.getLevel());
            
            _m_bonusMgr?.addSkill(treasureInfo.skillInfo);
            
            WinMsg.SendMsg(WinMsgType.ON_TREASURE_HUNT_TREASURE_LEVEL_CHG, treasureInfo);
        }

        /// <summary>
        /// 太空寻宝-矿石新增
        /// </summary>
        /// <param name="_msg"></param>
        public void onTreasureHuntOreAdd(GS2GC_036_053_OnTreasureHuntOreAdd _msg)
        {
            if(_msg == null || _msg.getOreInfo() == null)
                return;

            TreasureHuntGotOreInfo oreInfo = getGotOreInfo(_msg.getOreInfo().getOreId());
            if (oreInfo == null)
            {
                oreInfo = new TreasureHuntGotOreInfo(_msg.getOreInfo());
                _m_lGotOreInfoList.Add(oreInfo);
                
                _m_bonusMgr?.addOre(oreInfo);
                
                if(!oreInfo.isAdvancedOre)
                    WinMsg.SendMsg(WinMsgType.ON_TREASURE_HUNT_UNLOCK_NORMAL_ORE, oreInfo);
                else
                    WinMsg.SendMsg(WinMsgType.ON_TREASURE_HUNT_UNLOCK_ADVANCED_ORE, oreInfo);
            }
            else
            {
                _m_bonusMgr?.removeOre(oreInfo);
                
                oreInfo.updateInfo(_msg.getOreInfo());
                
                _m_bonusMgr?.addOre(oreInfo);
            }

            // 新增矿石时, 可能会导致组合图鉴状态变化, 这里要刷新下
            updateCompositeCatalogState(_msg.getOreInfo().getOreId());
            
            // 新增矿石后，刷新红点
            _m_redTipDealer?.refreshPendingOreRedTip();
        }

        /// <summary>
        /// 太空寻宝-矿石数量变化
        /// </summary>
        /// <param name="_msg"></param>
        public void onTreasureHuntOreNumChg(GS2GC_036_054_OnTreasureHuntOreNumChg _msg)
        {
            if(_msg == null || _msg.getNumInfo() == null)
                return;

            TreasureHuntGotOreInfo oreInfo = getGotOreInfo(_msg.getOreId());
            if (oreInfo == null)
                return;
                
            oreInfo.updateOreNum(_msg.getNumInfo());
            
            // 矿石数量变化后，刷新红点
            _m_redTipDealer?.refreshPendingOreRedTip();
        }

        /// <summary>
        /// 太空寻宝-矿石技能变化
        /// </summary>
        /// <param name="_msg"></param>
        public void onTreasureHuntOreSkillChg(GS2GC_036_055_OnTreasureHuntOreSkillChg _msg)
        {
            if(_msg == null || _msg.getSkillInfo() == null)
                return;

            TreasureHuntGotOreInfo oreInfo = getGotOreInfo(_msg.getOreId());
            if (oreInfo == null)
                return;
                
            if (_msg.getIsNormal())
            {
                _m_bonusMgr?.removeSkill(oreInfo.normalSkillInfo);
                
                oreInfo.updateNormalSkillInfo(_msg.getSkillInfo());
                // 发送一下技能点变化消息
                if(oreInfo.normalSkillInfo != null && oreInfo.normalSkillInfo.skillPointItem != null)
                    NPPlayer.instance.commonItemCountComp.sendCommonItemCountChgMsg(oreInfo.normalSkillInfo.skillPointItem, oreInfo.normalSkillInfo.skillPointNum);
                
                _m_bonusMgr?.addSkill(oreInfo.normalSkillInfo);
            }
            else
            {
                _m_bonusMgr?.removeSkill(oreInfo.advanceSkillInfo);

                oreInfo.updateAdvanceSkillInfo(_msg.getSkillInfo());
                // 发送一下技能点变化消息
                if(oreInfo.normalSkillInfo != null && oreInfo.normalSkillInfo.skillPointItem != null)
                    NPPlayer.instance.commonItemCountComp.sendCommonItemCountChgMsg(oreInfo.normalSkillInfo.skillPointItem, oreInfo.normalSkillInfo.skillPointNum);

                _m_bonusMgr?.addSkill(oreInfo.advanceSkillInfo);
            }
            
            WinMsg.SendMsg(WinMsgType.ON_TREASURE_HUNT_ORE_SKILL_CHG, oreInfo, _msg.getIsNormal());
        }

        /// <summary>
        /// 太空寻宝-组合图鉴变化
        /// </summary>
        /// <param name="_msg"></param>
        public void onTreasureCompositeChg(GS2GC_036_056_OnTreasureCompositeChg _msg)
        {
            if(_msg == null || _msg.getCompositeInfo() == null)
                return;
            
            TreasureHuntCompositeCatalogInfo compositeCatalogInfo = getCompositeCatalogInfo(_msg.getCompositeInfo().getRefId());
            if (compositeCatalogInfo == null)
                compositeCatalogInfo = createCompositeCatalogInfo(_msg.getCompositeInfo().getRefId());
            
            _m_bonusMgr?.removeCompositeCatalog(compositeCatalogInfo);
            ETreasureHuntCompositeCatalogState oldState = compositeCatalogInfo.state;
            
            compositeCatalogInfo.updateFromServerInfo(_msg.getCompositeInfo());
            
            _m_bonusMgr?.addCompositeCatalog(compositeCatalogInfo);
            _checkCompositeCatalogStateChg(compositeCatalogInfo, oldState);
            
            WinMsg.SendMsg(WinMsgType.ON_TREASURE_HUNT_COMPOSITE_CATALOG_CHG, compositeCatalogInfo);
        }

        /// <summary>
        /// 太空寻宝-太空舱变化
        /// </summary>
        /// <param name="_msg"></param>
        public void onTreasureStationChg(GS2GC_036_057_OnTreasureStationChg _msg)
        {
            if(_msg == null || _msg.getStationInfo() == null)
                return;

            int stationLevel = _m_iStationInfo?.stationLevel ?? 0;
            if (_m_iStationInfo == null)
                _m_iStationInfo = new TreasureHuntStationInfo(_msg.getStationInfo());
            else
                _m_iStationInfo.updateInfo(_msg.getStationInfo());

            if (stationLevel != _m_iStationInfo.stationLevel)
            {
                // 太空站等级变化时, 尝试刷新新区域解锁红点
                _m_redTipDealer.refreshUnlockNewAreaRedTip();

                if (_m_iStationInfo.stationLevel > stationLevel)
                {
                    if(_m_lUpgradeStationLevelList == null)
                        _m_lUpgradeStationLevelList = new List<long>();
                    _m_lUpgradeStationLevelList.Add(_m_iStationInfo.stationLevel);
                }
                
                WinMsg.SendMsg(WinMsgType.ON_TREASURE_HUNT_STATION_LVL_CHG);
            }
            
            WinMsg.SendMsg(WinMsgType.ON_TREASURE_HUNT_STATION_CHG);
        }

        /// <summary>
        /// 太空寻宝-矿石最大记录变化
        /// </summary>
        /// <param name="_msg"></param>
        public void onTreasureHuntOreMaxRecordChg(GS2GC_036_058_OnTreasureHuntOreMaxRecordChg _msg)
        {
            if(_msg == null)
                return;

            TreasureHuntGotOreInfo oreInfo = getGotOreInfo(_msg.getOreId());
            if (oreInfo == null)
                return;

            bool isAdvancedOre = oreInfo.isAdvancedOre;
            oreInfo.updateTreasureHuntOreMaxRecord(_msg.getMaxRecord());//进行数据变化
            WinMsg.SendMsg(WinMsgType.ON_TREASURE_HUNT_ORE_MAX_RECORD_CHG, oreInfo);
            // 若原来矿石不为高级矿石, 但是更新数据后为高级矿石
            if (!isAdvancedOre && oreInfo.isAdvancedOre)
            {
                WinMsg.SendMsg(WinMsgType.ON_TREASURE_HUNT_UNLOCK_ADVANCED_ORE, oreInfo);
            }
            
            // 获取矿石的最大记录更新时, 可能会导致组合图鉴状态变化, 这里要刷新下
            updateCompositeCatalogState(_msg.getOreId());
        }

        /// <summary>
        /// 太空寻宝-矿石已领取记录奖励变化
        /// </summary>
        /// <param name="_msg"></param>
        public void onTreasureHuntOreHadDrawRecordRewardChg(GS2GC_036_059_OnTreasureHuntOreHadDrawRecordRewardChg _msg)
        {
            if(_msg == null || _msg.getHadDrawRecordRewardList() == null)
                return;

            TreasureHuntGotOreInfo oreInfo = getGotOreInfo(_msg.getOreId());
            if (oreInfo == null)
                return;
                
            oreInfo.updateHadDrawRecordRewardList(_msg.getHadDrawRecordRewardList());
            WinMsg.SendMsg(WinMsgType.ON_TREASURE_HUNT_ORE_HAD_DRAW_RECORD_REWARD_CHG, oreInfo);
        }
        
        /// <summary>
        /// 太空寻宝-奇物产出变更（单个奇物）
        /// </summary>
        public void onTreasureHuntTreasureOutputChg(GS2GC_036_061_OnTreasureHuntTreasureOutputChg _msg)
        {
            if (_msg == null || _msg.getTreasureOutputInfo() == null)
                return;

            var proto = _msg.getTreasureOutputInfo();
            long treasureId = proto.getTreasureId();

            TreasureHuntTreasureOutputInfo outputInfo = getTreasureOutputInfo(treasureId);
            if (outputInfo == null)
            {
                outputInfo = new TreasureHuntTreasureOutputInfo(proto);
                _m_lTreasureOutputInfoList.Add(outputInfo);
            }
            else
            {
                outputInfo.updateTreasureOutputInfo(proto);
            }

            // 发送产出信息变化消息（带上数据）
            WinMsg.SendMsg(WinMsgType.ON_TREASURE_HUNT_TREASURE_OUTPUT_CHG, outputInfo);
        }
        
        #endregion

        #region C2S

        public void reqTreasureHuntInit()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_002_066_ReqTreasureHuntInit());
        }

        /// <summary>
        /// 请求捕获矿石
        /// </summary>
        public void reqTreasureHuntOreCapture(ETreasureHuntCaptureType _type, bool _isAdvance, long _areaId, long _distance, Action<bool, GS2GC_036_001_RetTreasureHuntOreCapture> _callBack)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_036_001_ReqTreasureHuntOreCapture(_type, _isAdvance, _areaId, _distance)
                , new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_036_001_RetTreasureHuntOreCapture>((_isSucc, _msg) =>
            {
                _callBack?.Invoke(_isSucc, _msg);
            }));
        }

        /// <summary>
        /// 太空寻宝-矿石技能激活
        /// </summary>
        /// <param name="_oreId">矿石ID</param>
        /// <param name="_isNormal">是否为普通技能</param>
        /// <param name="_callBack">回调</param>
        public void reqTreasureHuntOreSkillActive(long _oreId, bool _isNormal, Action<bool, GS2GC_036_002_RetTreasureHuntOreSkillActive> _callBack)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_036_002_ReqTreasureHuntOreSkillActive(_oreId, _isNormal)
                , new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_036_002_RetTreasureHuntOreSkillActive>((_isSucc, _msg) =>
            {
                _callBack?.Invoke(_isSucc, _msg);
            }));
        }

        /// <summary>
        /// 请求升级矿石技能
        /// </summary>
        /// <param name="_oreId">矿石ID</param>
        /// <param name="_isNormal">是否为普通技能</param>
        /// <param name="_callBack">回调</param>
        public void reqTreasureHuntOreSkillUpgrade(long _oreId, bool _isNormal, Action<bool, GS2GC_036_003_RetTreasureHuntOreSkillUpgrade> _callBack)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_036_003_ReqTreasureHuntOreSkillUpgrade(_oreId, _isNormal)
                , new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_036_003_RetTreasureHuntOreSkillUpgrade>((_isSucc, _msg) =>
            {
                _callBack?.Invoke(_isSucc, _msg);
            }));
        }

        /// <summary>
        /// 太空寻宝-奇物技能激活
        /// </summary>
        /// <param name="_treasureId">奇物id</param>
        /// <param name="_callBack">回调</param>
        public void reqTreasureHuntTreasureSkillActive(long _treasureId, Action<bool, GS2GC_036_004_RetTreasureHuntTreasureSkillActive> _callBack)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_036_004_ReqTreasureHuntTreasureSkillActive(_treasureId)
                , new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_036_004_RetTreasureHuntTreasureSkillActive>((_isSucc, _msg) =>
            {
                _callBack?.Invoke(_isSucc, _msg);
            }));
        }

        /// <summary>
        /// 请求升级奇物技能
        /// </summary>
        /// <param name="_treasureId">奇物ID</param>
        /// <param name="_callBack">回调</param>
        public void reqTreasureHuntTreasureSkillUpgrade(long _treasureId, Action<bool, GS2GC_036_005_RetTreasureHuntTreasureSkillUpgrade> _callBack)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_036_005_ReqTreasureHuntTreasureSkillUpgrade(_treasureId)
                , new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_036_005_RetTreasureHuntTreasureSkillUpgrade>((_isSucc, _msg) =>
            {
                _callBack?.Invoke(_isSucc, _msg);
            }));
        }

        /// <summary>
        /// 请求激活组合图鉴技能
        /// </summary>
        /// <param name="_compositeCatalogId">组合图鉴ID</param>
        /// <param name="_isNormal">是否为普通技能</param>
        /// <param name="_callBack">回调</param>
        public void reqTreasureHuntCompositeActive(long _compositeCatalogId, bool _isNormal, Action<bool, GS2GC_036_006_RetTreasureHuntCompositeActive> _callBack)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_036_006_ReqTreasureHuntCompositeActive(_compositeCatalogId, _isNormal)
                , new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_036_006_RetTreasureHuntCompositeActive>((_isSucc, _msg) =>
            {
                _callBack?.Invoke(_isSucc, _msg);
            }));
        }

        /// <summary>
        /// 请求领取矿石记录奖励
        /// </summary>
        /// <param name="_oreId">矿石ID</param>
        /// <param name="_recordIndex">记录档位索引</param>
        /// <param name="_callBack">回调</param>
        public void reqTreasureHuntDrawOreRecordReward(long _oreId, int _recordIndex, Action<bool, GS2GC_036_007_RetTreasureHuntDrawOreRecordReward> _callBack)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_036_007_ReqTreasureHuntDrawOreRecordReward(_oreId, _recordIndex)
                , new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_036_007_RetTreasureHuntDrawOreRecordReward>((_isSucc, _msg) =>
            {
                _callBack?.Invoke(_isSucc, _msg);
            }));
        }

        /// <summary>
        /// 太空寻宝-领取捕捉能量
        /// </summary>
        /// <param name="_callBack">回调</param>
        public void reqTreasureHuntDrawCapturePower(Action<bool, GS2GC_036_008_RetTreasureHuntDrawCapturePower> _callBack)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_036_008_ReqTreasureHuntDrawCapturePower()
                , new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_036_008_RetTreasureHuntDrawCapturePower>((_isSucc, _msg) =>
            {
                _callBack?.Invoke(_isSucc, _msg);
            }));
        }

        /// <summary>
        /// 太空寻宝-转换矿石
        /// </summary>
        /// <param name="_callBack">回调</param>
        public void reqTreasureHuntTransOre(Action<bool, GS2GC_036_009_RetTreasureHuntTransOre> _callBack)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_036_009_ReqTreasureHuntTransOre()
                , new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_036_009_RetTreasureHuntTransOre>((_isSucc, _msg) =>
            {
                _callBack?.Invoke(_isSucc, _msg);
            }));
        }

        /// <summary>
        /// 太空寻宝-矿石排行榜信息
        /// </summary>
        public void reqTreasureHuntOreRankInfo(long _oreId, Action<bool, GS2GC_036_010_RetTreasureHuntTransOreRankInfo> _callBack)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_036_010_ReqTreasureHuntOreRankInfo(_oreId)
                , new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_036_010_RetTreasureHuntTransOreRankInfo>((_isSucc, _msg) =>
                {
                    _callBack?.Invoke(_isSucc, _msg);
                }));
        }

        /// <summary>
        /// 太空寻宝-领取奇物产出
        /// </summary>
        /// <param name="_callBack">回调</param>
        public void reqTreasureHuntDrawTreasureOutput(long _treasureId, Action<bool, GS2GC_036_011_RetTreasureHuntDrawTreasureOutput> _callBack)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_036_011_ReqTreasureHuntDrawTreasureOutput(_treasureId)
                , new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_036_011_RetTreasureHuntDrawTreasureOutput>((_isSucc, _msg) =>
                {
                    _callBack?.Invoke(_isSucc, _msg);
                }));
        }

        #endregion
        
        #region 加成管理

        /// <summary>
        /// 获取玩家属性容器
        /// </summary>
        /// <returns></returns>
        public NPPlayerPropertyContainer getPlayerPropertyContainer()
        {
            return _m_bonusMgr?.playerPropertyContainer;
        }

        #endregion
    }
}

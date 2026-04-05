using System;
using System.Collections.Generic;
using Common.GuildEnum;
using Common.MarsEnum;
using Common.MarsObj;
using CommonEnum;
using NPEnum;

namespace GOE
{
    public class MarsTechnologyInfo : _IMarsTimeSpeedUpObject, _IMarsCompleteNowObject, _IGuildMarsHelp
    {
        public static NPCommonItem CompleteNowCommonItem = new NPCommonItem(ENPItemType.CURRENCY, (int) ECurrency.GEM);
        
        /// <summary>
        /// 科技ID
        /// </summary>
        private long _m_lTechnologyId;
        /// <summary>
        /// 科技配表数据
        /// </summary>
        private MarsTechnologyRefObj _m_rTechnologyRefObj;
        /// <summary>
        /// 科技等级
        /// </summary>
        private int _m_iLvl;
        /// <summary>
        /// 科技等级配表id
        /// </summary>
        private long _m_lTechnologyLvlRefId;
        /// <summary>
        /// 科技等级配表数据
        /// </summary>
        private MarsTechnologyLevelRefObj _m_rTechnologyLvlRefObj;
        /// <summary>
        /// 升级消耗物品列表
        /// </summary>
        private List<NPCommonCostItem> _m_lUpgradeConsumeList;
        /// <summary>
        /// 是否升级中 或 已升级完成
        /// </summary>
        private bool _m_bIsUpgradingOrUpgraded;
        /// <summary>
        /// 科技开始升级时间（毫秒）
        /// </summary>
        private long _m_lStartUpgradeLvlMs;
        /// <summary>
        /// 服务器给的 原始的科技结束升级时间（毫秒, 未经道具、盟友加速的结束时间, 但是经过玩家属性ENPPlayerPropertyType.MARS_TECH_SPEED_PER的减免）
        /// </summary>
        private long _m_lOriEndUpgradeLvlMs;

        private long _m_lItemHelpSecs; // 道具帮助时间
        
        private long _m_guildHelpId; // 公会帮助id
        private long _m_guildHelpSecs; // 公会帮助时间

        public MarsTechnologyInfo(long _technologyId)
        {
            _m_lTechnologyId = _technologyId;

            _m_iLvl = 0;
            _m_bIsUpgradingOrUpgraded = false;
        }

        public MarsTechnologyInfo(MarsTechnologyRefObj _technologyRefObj)
        {
            _m_lTechnologyId = _technologyRefObj?.id ?? 0;
            _m_rTechnologyRefObj = _technologyRefObj;
            
            _m_iLvl = 0;
            _m_bIsUpgradingOrUpgraded = false;
        }
        
        public MarsTechnologyInfo(Mars_Technology _serverData)
        {
            update(_serverData);
        }
        
        public long technologyId { get { return _m_lTechnologyId; } }
        public MarsTechnologyRefObj technologyRefObj
        {
            get
            {
                if (_m_rTechnologyRefObj == null || _m_rTechnologyRefObj.id != _m_lTechnologyId)
                {
                    _m_rTechnologyRefObj = GRefdataCoreMgr.instance.marsTechnologyRefCore.getRef(_m_lTechnologyId);
                }

                return _m_rTechnologyRefObj;
            }
        }
        public int lvl { get { return _m_iLvl; } }
        public long technologyLvlRefId { get { return _m_lTechnologyLvlRefId; } }
        public MarsTechnologyLevelRefObj technologyLvlRefObj
        {
            get
            {
                if (_m_rTechnologyLvlRefObj == null || _m_rTechnologyLvlRefObj.id != _m_lTechnologyLvlRefId)
                    _m_rTechnologyLvlRefObj = GRefdataCoreMgr.instance.marsTechnologyLevelRefCore.getRef(_m_lTechnologyLvlRefId);

                return _m_rTechnologyLvlRefObj;
            }
        }
        public List<NPCommonCostItem> upgradeConsumeList
        {
            get
            {
                if (_m_lUpgradeConsumeList == null)
                    _m_lUpgradeConsumeList = new List<NPCommonCostItem>();

                if (technologyLvlRefObj != null)
                {
                    technologyLvlRefObj.getRealConsumeList(_m_lUpgradeConsumeList);
                }
                else
                {
                    _m_lUpgradeConsumeList.Clear();
                }

                return _m_lUpgradeConsumeList;
            }
        }
        public bool isUpgradingOrUpgraded { get { return _m_bIsUpgradingOrUpgraded; } }//正在升级中 或 升级完成
        public bool isUpgrading { get { return _m_bIsUpgradingOrUpgraded && remainingUpgradeTimeMs > 0; } }// 正在升级中
        public bool isUpgraded { get { return _m_bIsUpgradingOrUpgraded && remainingUpgradeTimeMs <= 0; } }// 升级完成
        public long startUpgradeLvlMs { get { return _m_lStartUpgradeLvlMs; } }
        public long afterReductionEndUpgradeLvlMs { get { return _m_lOriEndUpgradeLvlMs - _m_guildHelpSecs * 1000 - _m_lItemHelpSecs * 1000; } }//经过道具、互助加速后的结束时间
        /// <summary>
        /// 剩余升级时间
        /// </summary>
        public long remainingUpgradeTimeMs
        {
            get
            {
                if (!_m_bIsUpgradingOrUpgraded)
                    return 0;

                long nowMs = FpsAndPingMgr.instance.serverTimeTag;
                long remainingMs = afterReductionEndUpgradeLvlMs - nowMs;
                return remainingMs > 0 ? remainingMs : 0;
            }
        }
        
        public EMarsTechnologyState state { get { return MarsUtil.getTechnologyState(this); } }
        
        
        public EGuildMarsHelpObjType helpObjType => EGuildMarsHelpObjType.TECH_UP;

        public long guildHelpObjId => _m_lTechnologyId;
        
        public long guildHelpId => _m_guildHelpId;

        public void onGuildHelpChg(long _guildHelpId, long _guildHelpSecs)
        {
            _m_guildHelpId = _guildHelpId;
            _m_guildHelpSecs = _guildHelpSecs;
        }
        
        public void onGuildHelpDel()
        { 
            
        }
        
        public void update(Mars_Technology _serverData)
        {
            if (_serverData == null)
                return;

            _m_lTechnologyId = _serverData.getTechnologyId();
            _m_iLvl = _serverData.getLvl();
            _m_lTechnologyLvlRefId = GRefdataCoreMgr.instance.getMarsTechnologyLevelRefId(_m_lTechnologyId, _m_iLvl);
            _m_bIsUpgradingOrUpgraded = _serverData.getIsUpgrading();
            _m_lStartUpgradeLvlMs = _serverData.getStartUpgradeLvlMs();
            _m_lOriEndUpgradeLvlMs = _serverData.getEndUpgradeLvlMs();
            _m_lItemHelpSecs = _serverData.getItemHelpSecs();
            _m_guildHelpId = _serverData.getGuildHelpId();
            _m_guildHelpSecs = _serverData.getGuildHelpSecs();
            if (state == EMarsTechnologyState.UPGRADEING)
            {
                NPPlayer.instance.guildMarsHelpComp.regMyMarsHelp(this);
            }
            else
            {
                NPPlayer.instance.guildMarsHelpComp.unregMyMarsHelp(this);
            }
        }

        /// <summary>
        /// 更新道具帮助时间变化
        /// </summary>
        /// <param name="_itemHelpSecs"></param>
        public void onItemHelpSecsChg(long _itemHelpSecs)
        {
            _m_lItemHelpSecs = _itemHelpSecs;
        }
        
        public void setTechnologyInUpgradingOrUpgraded(bool _isUpgradingOrUpgraded)
        {
            _m_bIsUpgradingOrUpgraded = _isUpgradingOrUpgraded;
        }
        
        /// <summary>
        /// 检查是否可以升级
        /// </summary>
        public bool checkCanUpgrade(bool _showTip = false)
        {
            if (technologyRefObj == null || technologyLvlRefObj == null)
                return false;

            // 判断科研所是否在升级中
            foreach (var buildingInfo in NPPlayer.instance.marsComp.buildingSubComponent._getBuildingInfos())
            {
                if(buildingInfo.refObj.building_type != EMarsBuildingType.TECHNOLOGY)
                    continue;
                        
                if (buildingInfo.state is MarsBuildingInfo.StateType.Upgrading)
                {
                    // 科研所升级中, 则不能进行升级
                    if(_showTip)
                        NPGUIAddSceneCenterTip.instance.showTextTip(TextTranslate.instance.getLanguage(TransKeyConst.mars_technology_buildingUpgradeingCannotResearchTip_none));
                    return false;
                }
            }
            
            switch (state)
            {
                case EMarsTechnologyState.NONE:
                case EMarsTechnologyState.LOCK://未解锁状态(自身0级 且 升级前置条件未达成)
                case EMarsTechnologyState.LEVEL_NOT0_CONDITION_NOTREACH://自身非0级 且 升级前置条件未达成
                case EMarsTechnologyState.UPGRADEING://升级中状态
                case EMarsTechnologyState.UPGRADED://升级完成待确认状态
                case EMarsTechnologyState.MAX_LEVEL://最高级状态
                    return false;
                
                case EMarsTechnologyState.LEVEL_0_CONDITION_REACH://自身0级 且 升级前置条件已达成
                case EMarsTechnologyState.NORMAL://普通状态(自身已经升级过 且 升级前置条件已经达成)
                    if (NPPlayer.instance.marsComp.technologySubComponent.nowdUpgradingOrUpgradedTechnologyInfo != null)
                    {
                        // 这里不判断升级中的科技是否是自己, 因为前面的状态判断已经排除了自己在升级中的情况(UPGRADEING状态)
                        if(_showTip)
                            NPGUIAddSceneCenterTip.instance.showTextTip(TextTranslate.instance.getLanguage(TransKeyConst.mars_technology_hasTechnologyResearchingTip_none));
                        return false;
                    }

                    // 判断消耗物品数量足够
                    return GCommon.isItemEnough(upgradeConsumeList, _showTip);
                default:
                    return false;
            }
        }
        
        #region _IMarsTimeSpeedUpObject接口

        public EMarsBagItemUseTimeType timeType { get { return EMarsBagItemUseTimeType.MARS_TECH; } }
        public long timeObjId { get { return _m_lTechnologyId; } }
        long _IMarsTimeSpeedUpObject.remainTimeMs { get { return remainingUpgradeTimeMs; } }

        public long beforeReductionTotalTimeMs { get { return _m_lOriEndUpgradeLvlMs - _m_lStartUpgradeLvlMs; } }
        public long afterReductionTotalTimeMs { get { return afterReductionEndUpgradeLvlMs - _m_lStartUpgradeLvlMs; } }

        #endregion

        #region _IMarsCompleteNowObject接口

        public bool checkCanCompleteNow(bool _checkCompleteNowCostEnough, bool _showUnableTip)
        {
            if (technologyRefObj == null || technologyLvlRefObj == null)
                return false;

            foreach (var buildingInfo in NPPlayer.instance.marsComp.buildingSubComponent._getBuildingInfos())
            {
                if(buildingInfo.refObj.building_type != EMarsBuildingType.TECHNOLOGY)
                    continue;
                        
                if (buildingInfo.state is MarsBuildingInfo.StateType.Upgrading)
                {
                    // 科研所升级中, 则不能进行升级
                    if(_showUnableTip)
                        NPGUIAddSceneCenterTip.instance.showTextTip(TextTranslate.instance.getLanguage(TransKeyConst.mars_technology_buildingUpgradeingCannotResearchTip_none));
                    return false;
                }
            }
            
            switch (state)
            {
                case EMarsTechnologyState.NONE:
                case EMarsTechnologyState.LOCK://未解锁状态(自身0级 且 升级前置条件未达成)
                case EMarsTechnologyState.LEVEL_NOT0_CONDITION_NOTREACH://自身非0级 且 升级前置条件未达成
                case EMarsTechnologyState.UPGRADED://升级完成待确认状态
                case EMarsTechnologyState.MAX_LEVEL://最高级状态
                    return false;
                
                case EMarsTechnologyState.UPGRADEING://升级中状态
                    if (_checkCompleteNowCostEnough)
                        // 判断立即完成消耗物品数量足够
                        return GCommon.isItemEnough(completeNowCostItem, _showUnableTip);
                    else
                        return true;
                
                case EMarsTechnologyState.LEVEL_0_CONDITION_REACH://自身0级 且 升级前置条件已达成
                case EMarsTechnologyState.NORMAL://普通状态(自身已经升级过 且 升级前置条件已经达成)
                    // 不在升级中时, 需要先判断升级消耗物品数量是否足够
                    if (!GCommon.isItemEnough(upgradeConsumeList, _showUnableTip))
                        return false;
                    
                    if (_checkCompleteNowCostEnough)
                        // 判断立即完成消耗物品数量足够
                        return GCommon.isItemEnough(completeNowCostItem, _showUnableTip);
                    else
                        return true;
                default:
                    return false;
            }
        }

        public bool hasRemainTime { get { return isUpgrading; } }//在升级中才有剩余时间
        long _IMarsCompleteNowObject.remainTimeMs { get { return remainingUpgradeTimeMs; } }

        private NPCommonCostItem _m_CompleteNowCostItem;
        public NPCommonCostItem completeNowCostItem
        {
            get
            {
                if (_m_CompleteNowCostItem == null)
                    _m_CompleteNowCostItem = new NPCommonCostItem(CompleteNowCommonItem, 0);

                long gemCost = 0;
                
                if(isUpgrading)
                    gemCost = MarsUtil.calculateGemCostForSpeedUpMs(remainingUpgradeTimeMs);
                else if(technologyLvlRefObj != null)
                    gemCost = MarsUtil.calculateGemCostForSpeedUpMs(technologyLvlRefObj.upgradeRealTimeMs);
                
                _m_CompleteNowCostItem.setCount(gemCost);
                
                return _m_CompleteNowCostItem;
            }
        }

        public Action<Action<bool>> dealCompleteNowAction { get { return reqCompleteNow; } }

        public void reqCompleteNow(Action<bool> _onComplete)
        {
            if (!checkCanCompleteNow(true, true))
            {
                _onComplete?.Invoke(false);
            }
            else
            {
                NPPlayer.instance.marsComp.technologySubComponent.reqSetTechnologyDone(_m_lTechnologyId, () =>
                {
                    _onComplete?.Invoke(true);
                });
            }
        }

        #endregion

    }
}
using Common.TreasureHuntObj;
using JetBrains.Annotations;

namespace GOE
{
    public class TreasureHuntCompositeCatalogInfo
    {
        private long _m_lCompositeCatalogId;
        private TreasureHuntCompositeCatalogRefObj _m_rCompositeCatalogRefObj;
        private long _m_lCollectedTimeMs;//集成时间
        private ETreasureHuntCompositeCatalogState _m_eCompositeCatalogState;//组合图鉴状态
        
        private TreasureHuntCommonSkillInfo _m_iNormalSkillInfo;
        private TreasureHuntCommonSkillInfo _m_iAdvancedSKillInfo;

        public TreasureHuntCompositeCatalogInfo(long _compositeCatalogId)
        {
            _m_lCompositeCatalogId = _compositeCatalogId;
            _m_eCompositeCatalogState = ETreasureHuntCompositeCatalogState.NONE;
            
            if (compositeCatalogRefObj != null)
            {
                _m_iNormalSkillInfo = new TreasureHuntCommonSkillInfo(compositeCatalogRefObj.normal_skill_id, false, 0, null, 0);
                _m_iAdvancedSKillInfo = new TreasureHuntCommonSkillInfo(compositeCatalogRefObj.advanced_skill_id, false, 0, null, 0);
            }

            updateCompositeCatelogState();
        }

        public TreasureHuntCompositeCatalogInfo(TreasureHuntCompositeCatalogRefObj _compositeCatalogRefObj)
        {
            _m_rCompositeCatalogRefObj = _compositeCatalogRefObj;
            _m_lCompositeCatalogId = _m_rCompositeCatalogRefObj?.id ?? 0;
            _m_eCompositeCatalogState = ETreasureHuntCompositeCatalogState.NONE;
            
            if (compositeCatalogRefObj != null)
            {
                _m_iNormalSkillInfo = new TreasureHuntCommonSkillInfo(compositeCatalogRefObj.normal_skill_id, false, 0, null, 0);
                _m_iAdvancedSKillInfo = new TreasureHuntCommonSkillInfo(compositeCatalogRefObj.advanced_skill_id, false, 0, null, 0);
            }
            
            updateCompositeCatelogState();
        }
        
        public long compositeCatalogId { get { return _m_lCompositeCatalogId; } }

        public TreasureHuntCompositeCatalogRefObj compositeCatalogRefObj
        {
            get
            {
                if (_m_rCompositeCatalogRefObj == null || _m_rCompositeCatalogRefObj.id != _m_lCompositeCatalogId)
                    _m_rCompositeCatalogRefObj = GRefdataCoreMgr.instance.treasureHuntCompositeCatalogRefCore.getRef(_m_lCompositeCatalogId);
                return _m_rCompositeCatalogRefObj;
            }
        }

        public ETreasureHuntCompositeCatalogState state { get { return _m_eCompositeCatalogState; } }
        public long collectedTimeMs { get { return _m_lCollectedTimeMs; } }
        public TreasureHuntCommonSkillInfo normalSkillInfo { get { return _m_iNormalSkillInfo; } }
        public TreasureHuntCommonSkillInfo advancedSkillInfo { get { return _m_iAdvancedSKillInfo; } }

        /// <summary>
        /// 是否已经集齐所需矿石(不管普通还是高级, 集齐就行)
        /// </summary>
        public bool isCollected { get { return state is ETreasureHuntCompositeCatalogState.GOT_NORMAL or ETreasureHuntCompositeCatalogState.GOT_ADVANCED; } }
        
        /// <summary>
        /// 是否集齐所需高级矿石
        /// </summary>
        public bool isCollectedAdvanced { get { return state == ETreasureHuntCompositeCatalogState.GOT_ADVANCED; } }

        /// <summary>
        /// 更新组合图鉴状态
        /// </summary>
        public void updateCompositeCatelogState()
        {
            if (compositeCatalogRefObj == null)
                return;

            if (compositeCatalogRefObj.ore_list == null)//若需要的矿石为空, 状态为直接获取高级图鉴状态
            {
                _m_eCompositeCatalogState = ETreasureHuntCompositeCatalogState.GOT_ADVANCED;
                _setNormalSkillUnlock(true);
                _setAdvanceSkillUnlock(true);
                return;
            }

            bool gotAllAdvanceOre = true;//是否获取到所有高级矿石
            bool gotAllNormalOre = true;//是否获取到所有普通矿石
            foreach (var oreId in compositeCatalogRefObj.ore_list)
            {
                TreasureHuntGotOreInfo oreInfo = NPPlayer.instance.treasureHuntComponent.getGotOreInfo(oreId);
                if (oreInfo == null || 
                    (oreInfo.oreState is ETreasureHuntOreState.NONE or ETreasureHuntOreState.NOT_GET))
                {
                    gotAllAdvanceOre = false;
                    gotAllNormalOre = false;
                    break;
                }

                if (oreInfo.oreState is ETreasureHuntOreState.NOT_ACTIVATE_NORMAL or ETreasureHuntOreState.ACTIVATED_NORMAL)
                {
                    gotAllAdvanceOre = false;
                }
            }

            if (!gotAllNormalOre)
            {
                // 若所有普通矿石都未获取到, 则图鉴状态为未获取
                _m_eCompositeCatalogState = ETreasureHuntCompositeCatalogState.NOT_GET;
                
                // 同步更新普通技能和高级技能状态未为解锁
                _setNormalSkillUnlock(false);
                _setAdvanceSkillUnlock(false);
            }
            else if(!gotAllAdvanceOre)
            {
                // 所有普通矿石都获取到, 但是没有获取到所有高级矿石, 图鉴状态为已获取普通图鉴
                _m_eCompositeCatalogState = ETreasureHuntCompositeCatalogState.GOT_NORMAL;
             
                _setNormalSkillUnlock(true);//设置普通技能解锁状态为已解锁
                _setAdvanceSkillUnlock(false);//设置高级技能解锁状态为未解锁
            }
            else//普通矿石和高级矿石都获取了
            {
                // 图鉴状态为已获取高级图鉴
                _m_eCompositeCatalogState = ETreasureHuntCompositeCatalogState.GOT_ADVANCED;
                
                _setNormalSkillUnlock(true);//设置普通技能解锁状态为已解锁
                _setAdvanceSkillUnlock(true);//设置高级技能解锁状态为已解锁
            }
        }
        
        /// <summary>
        /// 通过服务器数据更新
        /// </summary>
        /// <param name="_serverCompositeInfo"></param>
        public void updateFromServerInfo(TreasureHunt_CompositeInfo _serverCompositeInfo)
        {
            if(_serverCompositeInfo == null || _serverCompositeInfo.getRefId() != _m_lCompositeCatalogId)
                return;

            _m_lCollectedTimeMs = _serverCompositeInfo.getCollectTimeMs();

            _setNormalSkillActive(_serverCompositeInfo.getIsNormalActive());
            _setAdvanceSkillActive(_serverCompositeInfo.getIsAdvancedActive());

            updateCompositeCatelogState();
        }

        /// <summary>
        /// 设置普通技能解锁状态
        /// </summary>
        /// <param name="_isUnlock"></param>
        private void _setNormalSkillUnlock(bool _isUnlock)
        {
            _m_iNormalSkillInfo?.updateSkillUnlock(_isUnlock);
        }
        
        /// <summary>
        /// 设置普通技能激活状态
        /// </summary>
        private void _setNormalSkillActive(bool _isActive)
        {
            int normalSKillLevel = _isActive ? 1 : 0;
            _m_iNormalSkillInfo?.updateSkillLevel(normalSKillLevel);
        }

        /// <summary>
        /// 设置高级技能激活状态
        /// </summary>
        /// <param name="_isActive"></param>
        private void _setAdvanceSkillActive(bool _isActive)
        {
            int advanceSKillLevel = _isActive ? 1 : 0;
            _m_iAdvancedSKillInfo?.updateSkillLevel(advanceSKillLevel);
        }
        
        /// <summary>
        /// 设置高级技能解锁状态
        /// </summary>
        /// <param name="_isUnlock"></param>
        private void _setAdvanceSkillUnlock(bool _isUnlock)
        {
            _m_iAdvancedSKillInfo?.updateSkillUnlock(_isUnlock);
        }
        
        /// <summary>
        /// 是否包含矿石
        /// </summary>
        /// <param name="_oreId"></param>
        /// <returns></returns>
        public bool containsOre(long _oreId)
        {
            if (compositeCatalogRefObj == null || compositeCatalogRefObj.ore_list == null)
                return false;

            return compositeCatalogRefObj.ore_list.Contains(_oreId);
        }
    }
}
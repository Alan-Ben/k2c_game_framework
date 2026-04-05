using System.Collections.Generic;
using Common.TreasureHuntObj;
using GS2GC.p036_TreasureHuntOp;

namespace GOE
{
    public class TreasureHuntGotOreInfo : _ITreasureHuntOreInfo
    {
        private long _m_lOreId;//矿石id
        private TreasureHuntOreRefObj _m_oreRefObj;//矿石配表对象
        private long _m_lFirstGainTimeMs;//首次获取时间
        private int _m_iMaxMassRecord;//最大质量记录
        private List<int> _m_lHadDrawRecordRewardList;//已领取奖励档位列表
        private int _m_iTotalGainNum;//总获取数量
        private TreasureHuntCommonOreInfo _m_iNormalPendingOreInfo;//普通待处理矿石信息
        private TreasureHuntCommonOreInfo _m_iAdvancePendingOreInfo;//高级待处理矿石信息
        private TreasureHuntCommonSkillInfo _m_iNormalSkillInfo;//普通技能信息
        private TreasureHuntCommonSkillInfo _m_iAdvanceSkillInfo;//高级技能信息

        public TreasureHuntGotOreInfo(TreasureHunt_OreInfo _serverOreInfo)
        {
            updateInfo(_serverOreInfo);
        }
        
        public long oreId { get { return _m_lOreId; } }
        public TreasureHuntOreRefObj oreRefObj
        {
            get
            {
                if (_m_oreRefObj == null || _m_oreRefObj.id != _m_lOreId)
                    _m_oreRefObj = GRefdataCoreMgr.instance.treasureHuntOreRefCore.getRef(_m_lOreId);
                return _m_oreRefObj;
            }
        }
        public ETreasureHuntOreState oreState
        {
            get
            {
                if (oreRefObj == null)
                    return ETreasureHuntOreState.NONE;

                // 使用这个类, 矿石一定是已获取了, 所以不用判断是否为未获取矿石
                
                // 若普通技能未激活, 则矿石状态为未激活普通矿石
                if (_m_iNormalSkillInfo == null || 
                    (_m_iNormalSkillInfo.skillState is ETreasureHuntSkillState.LOCK or ETreasureHuntSkillState.UNLOCK_NOT_ACTIVATE) )
                    return ETreasureHuntOreState.NOT_ACTIVATE_NORMAL;

                // 普通技能已激活, 但是矿石最大质量记录未达到高级矿石, 则矿石状态为已激活普通矿石
                if (!oreRefObj.isReachAdvanceOreMass(_m_iMaxMassRecord))
                    return ETreasureHuntOreState.ACTIVATED_NORMAL;

                // 到这里说明获取的最大矿石记录已经达到高级矿石 ,若高级技能未激活, 则矿石状态为未激活高级矿石
                if (_m_iAdvanceSkillInfo == null ||
                    (_m_iAdvanceSkillInfo.skillState is ETreasureHuntSkillState.LOCK or ETreasureHuntSkillState.UNLOCK_NOT_ACTIVATE))
                    return ETreasureHuntOreState.NOT_ACTIVATE_ADVANCED;

                return ETreasureHuntOreState.ACTIVATED_ADVANCED;
            }
        }
        public int num { get { return _m_iTotalGainNum; } }
        public int mass { get { return _m_iMaxMassRecord; } }
        public long getTimeMs { get { return _m_lFirstGainTimeMs; } }
        public _ITreasureHuntSkillInfo normalSkillInfo { get { return _m_iNormalSkillInfo; } }
        public _ITreasureHuntSkillInfo advanceSkillInfo { get { return _m_iAdvanceSkillInfo; } }
        public TreasureHuntCommonOreInfo normalPendingOreInfo { get { return _m_iNormalPendingOreInfo; } }
        public TreasureHuntCommonOreInfo advancePendingOreInfo { get { return _m_iAdvancePendingOreInfo; } }
        // 是否是高级矿石(当前矿石是否达到高级矿石质量)
        public bool isAdvancedOre { get { return oreRefObj?.isReachAdvanceOreMass(_m_iMaxMassRecord) ?? false; } }

        public void updateInfo(TreasureHunt_OreInfo _serverOreInfo)
        {
            if(_serverOreInfo == null)
                return;
            
            _m_lOreId = _serverOreInfo.getOreId();
            _m_lFirstGainTimeMs = _serverOreInfo.getFirstGainTimeMs();
            _m_iMaxMassRecord = _serverOreInfo.getMaxRecord();
            _m_lHadDrawRecordRewardList = _serverOreInfo.getHadDrawRecordRewardList();

            _m_iTotalGainNum = 0;
            _m_iNormalPendingOreInfo = null;
            _m_iAdvancePendingOreInfo = null;
            updateOreNum(_serverOreInfo.getNumInfo());

            _m_iNormalSkillInfo = null;
            updateNormalSkillInfo(_serverOreInfo.getNormalSkillInfo());
            _m_iAdvanceSkillInfo = null;
            updateAdvanceSkillInfo(_serverOreInfo.getAdvancedSkillInfo());
        }

        /// <summary>
        /// 更新矿石数量
        /// </summary>
        public void updateOreNum(TreasureHunt_OreNumInfo _serverOreNumInfo)
        {
            if(_serverOreNumInfo == null)
                return;

            _m_iTotalGainNum = _serverOreNumInfo.getTotalGainNum();
            
            if (_m_iNormalPendingOreInfo != null)
            {
                _m_iNormalPendingOreInfo.updateNum(_serverOreNumInfo.getNormalPendingNum());
            }
            else
            {
                _m_iNormalPendingOreInfo = new TreasureHuntCommonOreInfo(_m_lOreId, ETreasureHuntOreState.ACTIVATED_NORMAL, _serverOreNumInfo.getNormalPendingNum(), 0, 0);
            }

            if (_m_iAdvancePendingOreInfo != null)
            {
                _m_iAdvancePendingOreInfo.updateNum(_serverOreNumInfo.getAdvancedPendingNum());
            }
            else
            {
                _m_iAdvancePendingOreInfo = new TreasureHuntCommonOreInfo(_m_lOreId, ETreasureHuntOreState.ACTIVATED_ADVANCED, _serverOreNumInfo.getAdvancedPendingNum(), 0, 0);
            }
        }

        /// <summary>
        /// 更新普通技能信息
        /// </summary>
        public void updateNormalSkillInfo(TreasureHunt_OreSkillInfo _serverSkillInfo)
        {
            if(_serverSkillInfo == null)
                return;
            
            if (_m_iNormalSkillInfo == null)
            {
                if (oreRefObj != null)
                {
                    _m_iNormalSkillInfo = new TreasureHuntCommonSkillInfo(oreRefObj.normal_skill_id, true,
                        _serverSkillInfo.getSkillLevel(),
                        GRefdataCoreMgr.instance.npGeneral.treasure_hunt_ore_normal_skill_point_item,
                        _serverSkillInfo.getSkillPoint());
                }
            }
            else
            {
                _m_iNormalSkillInfo.updateSkillLevel(_serverSkillInfo.getSkillLevel());
                _m_iNormalSkillInfo.updateSkillPointNum(_serverSkillInfo.getSkillPoint());
            }
        }

        /// <summary>
        /// 更新高级技能信息
        /// </summary>
        public void updateAdvanceSkillInfo(TreasureHunt_OreSkillInfo _serverSkillInfo)
        {
            if(_serverSkillInfo == null)
                return;
            
            if(_m_iAdvanceSkillInfo == null)
            {
                if (oreRefObj != null)
                {
                    _m_iAdvanceSkillInfo = new TreasureHuntCommonSkillInfo(oreRefObj.advanced_skill_id,
                        oreRefObj.isReachAdvanceOreMass(_m_iMaxMassRecord), _serverSkillInfo.getSkillLevel(),
                        GRefdataCoreMgr.instance.npGeneral.treasure_hunt_ore_advanced_skill_point_item,
                        _serverSkillInfo.getSkillPoint());
                }
            }
            else
            {
                _m_iAdvanceSkillInfo.updateSkillLevel(_serverSkillInfo.getSkillLevel());
                _m_iAdvanceSkillInfo.updateSkillPointNum(_serverSkillInfo.getSkillPoint());
            }
        }

        /// <summary>
        /// 更新矿石最大质量记录
        /// </summary>
        public void updateTreasureHuntOreMaxRecord(int _maxMassRecord)
        {
            if(_m_iMaxMassRecord == _maxMassRecord)
                return;
            
            _m_iMaxMassRecord = _maxMassRecord;

            // 当矿石最大质量变更时可能引起高级技能解锁, 所以这里需要刷新高级技能信息
            if (_m_iAdvanceSkillInfo == null)
            {
                if (oreRefObj != null)
                {
                    _m_iAdvanceSkillInfo = new TreasureHuntCommonSkillInfo(oreRefObj.advanced_skill_id,
                        oreRefObj.isReachAdvanceOreMass(_m_iMaxMassRecord), 0,
                        GRefdataCoreMgr.instance.npGeneral.treasure_hunt_ore_advanced_skill_point_item, 0);
                }
            }
            else
            {
                _m_iAdvanceSkillInfo.updateSkillUnlock(oreRefObj.isReachAdvanceOreMass(_m_iMaxMassRecord));
            }
        }

        public void updateHadDrawRecordRewardList(List<int> _hadDrawRecordRewardList)
        {
            _m_lHadDrawRecordRewardList = _hadDrawRecordRewardList;
        }
        
        /// <summary>
        /// 是否已领取档位奖励
        /// </summary>
        /// <param name="_index"></param>
        /// <returns></returns>
        public bool hadDrawRecordReward(int _index)
        {
            return _m_lHadDrawRecordRewardList != null && _m_lHadDrawRecordRewardList.Contains(_index);
        }

        /// <summary>
        /// 是否有可领取的档位奖励
        /// </summary>
        /// <returns></returns>
        public bool hasCanDrawRecordReward()
        {
            List<TreasureHuntOreMassRewardGradeInfo> oreMassRewardGradeInfoList = oreRefObj?.mass_reward_grade_list;
            if (oreMassRewardGradeInfoList == null || oreMassRewardGradeInfoList.Count <= 0)
                return false;
            
            for(int i = 0, count = oreMassRewardGradeInfoList.Count; i < count; i++)
            {
                TreasureHuntOreMassRewardGradeInfo gradeInfo = oreMassRewardGradeInfoList[i];
                if (gradeInfo == null)
                    continue;

                // 质量未达标退出循环(因为oreRefObj.mass_reward_grade_list在配表初始化完成后会按照质量从小到大排序一次, 所以这里判断质量未达标后直接退出循环)
                if (_m_iMaxMassRecord < gradeInfo.mass)
                    break;
                
                // 不存在奖励跳过
                if(gradeInfo.rewardItem == null || !gradeInfo.rewardItem.IsValid)
                    continue;
                
                // 已领取跳过
                if (hadDrawRecordReward(i))
                    continue;

                return true;
            }

            return false;
        }
    }
}
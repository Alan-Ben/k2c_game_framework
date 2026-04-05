using System.Collections.Generic;

namespace GOE
{
    public class TreasureHuntTreasureSkillInfo : _ITreasureHuntSkillInfo
    {
        private long _m_lSkillId;//技能id
        private TreasureHuntSkillRefObj _m_skillRefObj;//技能配表
        private int _m_iLevel;//技能等级
        private TreasureHuntSkillLevelRefObj _m_skillLevelRefObj;//技能等级配表
        private bool _m_bIsUnlockSkill;//技能是否已经解锁
        private ETreasureHuntSkillState _m_skillState;//技能状态
        private NPCommonItem _m_skillPointItem;//技能点物品
                
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_skillId"></param>
        /// <param name="_level"></param>
        /// <param name="_skillPointItem"></param>
        public TreasureHuntTreasureSkillInfo(long _skillId, bool _isUnlockSkill, int _level, NPCommonItem _skillPointItem)
        {
            updateSkillInfo(_skillId, _isUnlockSkill, _level, _skillPointItem);
        }
        
        public long skillId { get { return _m_lSkillId; } }
        public TreasureHuntSkillRefObj skillRefObj {
            get
            {
                if (_m_skillRefObj == null || _m_skillRefObj.id != _m_lSkillId)
                    _m_skillRefObj = GRefdataCoreMgr.instance.treasureHuntSkillRefCore.getRef(_m_lSkillId);
                return _m_skillRefObj;
            }
        }
        public int level { get { return _m_iLevel; } }
        public TreasureHuntSkillLevelRefObj skillLevelRefObj {
            get
            {
                if (_m_skillLevelRefObj == null || _m_skillLevelRefObj.id != _m_lSkillId ||
                    _m_skillLevelRefObj.level != _m_iLevel)
                    _m_skillLevelRefObj = GRefdataCoreMgr.instance.getTreasureHuntSkillLevelRefObj(_m_lSkillId, _m_iLevel);
                return _m_skillLevelRefObj;
            }
        }
        public ETreasureHuntSkillState skillState { get { return _m_skillState; } }
        public NPCommonItem skillPointItem { get { return _m_skillPointItem; } }
        public long skillPointNum { get { return GCommon.getItemCount(skillPointItem); } }

        public void updateSkillInfo(long _skillId, bool _isUnlockSkill, int _level, NPCommonItem _skillPointItem)
        {
            _m_lSkillId = _skillId;
            _m_bIsUnlockSkill = _isUnlockSkill;
            _m_iLevel = _level;
            _m_skillPointItem = _skillPointItem;

            updateSkillState();
        }

        public void updateSkillLevel(int _level)
        {
            if (_m_iLevel != _level)
            {
                _m_iLevel = _level;
             
                updateSkillState();
            }
        }

        public void updateSkillUnlock(bool _isUnlock)
        {
            if (_m_bIsUnlockSkill != _isUnlock)
            {
                _m_bIsUnlockSkill = _isUnlock;

                updateSkillState();       
            }
        }
        
        /// <summary>
        /// 更新技能状态
        /// </summary>
        public void updateSkillState()
        {
            if(!_m_bIsUnlockSkill)
            {
                _m_skillState = ETreasureHuntSkillState.LOCK;
            }
            else if(_m_iLevel <= 0)//已解锁但是等级为0时, 处于未激活状态
            {
                _m_skillState = ETreasureHuntSkillState.UNLOCK_NOT_ACTIVATE;
            }
            else
            {
                // 获取下一级技能等级配表数据
                TreasureHuntSkillLevelRefObj nextLevelRefObj =
                    GRefdataCoreMgr.instance.getTreasureHuntSkillLevelRefObj(_m_lSkillId, _m_iLevel + 1);
                if (nextLevelRefObj == null)//不存在下一级配表数据时, 当前为最高级
                    _m_skillState = ETreasureHuntSkillState.UNLOCK_ACTIVATE_MAX_LEVEL;
                else
                    _m_skillState = ETreasureHuntSkillState.UNLOCK_ACTIVATE;
            }
        }
    }
}
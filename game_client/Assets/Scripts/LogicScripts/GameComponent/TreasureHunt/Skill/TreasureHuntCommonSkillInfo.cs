using System.Collections.Generic;

namespace GOE
{
    public class TreasureHuntCommonSkillInfo : _ITreasureHuntSkillInfo
    {
        private long _m_lSkillId;
        private TreasureHuntSkillRefObj _m_skillRefObj;
        private int _m_iLevel;
        private TreasureHuntSkillLevelRefObj _m_skillLevelRefObj;
        private bool _m_bIsUnlockSkill;//技能是否已经解锁
        private ETreasureHuntSkillState _m_skillState;
        private NPCommonItem _m_skillPointItem;
        private long _m_lSkillPointNum;

        public TreasureHuntCommonSkillInfo(long _skillId, bool _isUnlockSkill, int _skillLevel, NPCommonItem _skillPointItem, long _skillPointNum)
        {
            updateSkillInfo(_skillId, _isUnlockSkill, _skillLevel, _skillPointItem, _skillPointNum);
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
                // 小于等于0的等级配表不存在
                if (_m_iLevel <= 0)
                    return null;
                
                if(_m_skillLevelRefObj == null || _m_skillLevelRefObj.skill_id != _m_lSkillId || _m_skillLevelRefObj.level != _m_iLevel)
                    _m_skillLevelRefObj = GRefdataCoreMgr.instance.getTreasureHuntSkillLevelRefObj(_m_lSkillId, _m_iLevel);
                return _m_skillLevelRefObj;
            }
        }
        public ETreasureHuntSkillState skillState { get { return _m_skillState; } }
        public NPCommonItem skillPointItem { get { return _m_skillPointItem; } }
        public long skillPointNum { get { return _m_lSkillPointNum; } }

        public void updateSkillInfo(long _skillId, bool _isUnlockSkill, int _skillLevel, NPCommonItem _skillPointItem, long _skillPointNum)
        {
            _m_lSkillId = _skillId;
            _m_bIsUnlockSkill = _isUnlockSkill;
            _m_iLevel = _skillLevel;
            _m_skillPointItem = _skillPointItem;
            _m_lSkillPointNum = _skillPointNum;
            
            _refreshSKillState();
        }
        
        /// <summary>
        /// 更新技能等级
        /// </summary>
        /// <param name="_skillLevel"></param>
        public void updateSkillLevel(int _skillLevel)
        {
            if (_m_iLevel != _skillLevel)
            {
                _m_iLevel = _skillLevel;
                //技能等级变化可能会引起技能状态变化, 所以需要刷新技能状态
                _refreshSKillState();
            }
        }

        /// <summary>
        /// 更新技能是否解锁
        /// </summary>
        /// <param name="_isUnlockSkill"></param>
        public void updateSkillUnlock(bool _isUnlockSkill)
        {
            if (_m_bIsUnlockSkill != _isUnlockSkill)
            {
                _m_bIsUnlockSkill = _isUnlockSkill;
                _refreshSKillState();
            }
        }

        /// <summary>
        /// 更新技能点数
        /// </summary>
        /// <param name="_skillPointNum"></param>
        public void updateSkillPointNum(long _skillPointNum)
        {
            _m_lSkillPointNum = _skillPointNum;
        }
        
        /// <summary>
        /// 刷新当前技能状态
        /// </summary>
        public void _refreshSKillState()
        {
            if (_m_iLevel > 0)//优先进行等级判断, 若有等级则一定已经激活了, 防止特殊情况客户端没有设置解锁但是服务端已经激活有技能等级数据
            {
                // 获取下一级技能等级配表数据
                TreasureHuntSkillLevelRefObj nextLevelRefObj =
                    GRefdataCoreMgr.instance.getTreasureHuntSkillLevelRefObj(_m_lSkillId, _m_iLevel + 1);
                if (nextLevelRefObj == null)//不存在下一级配表数据时, 当前为最高级
                    _m_skillState = ETreasureHuntSkillState.UNLOCK_ACTIVATE_MAX_LEVEL;
                else
                    _m_skillState = ETreasureHuntSkillState.UNLOCK_ACTIVATE;
            }
            else if (_m_bIsUnlockSkill)
            {
                _m_skillState = ETreasureHuntSkillState.UNLOCK_NOT_ACTIVATE;
            }
            else
            {
                _m_skillState = ETreasureHuntSkillState.LOCK;
            }
        }
    }
}
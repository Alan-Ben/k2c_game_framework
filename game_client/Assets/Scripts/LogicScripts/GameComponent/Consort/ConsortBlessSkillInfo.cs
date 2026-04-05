namespace GOE
{
    /// <summary>
    /// 妃子加护技能信息
    /// </summary>
    public class ConsortBlessSkillInfo
    {
        private long _m_lSkillId;//技能id
        private int _m_iLevel;//等级
        
        private ConsortBlessSkillRefObj _m_rConsortBlessSkillRef;//技能配置表
        private ConsortBlessSkillLvlRefObj _m_rConsortBlessSkillLvlRef;//技能等级配置表
        
        public ConsortBlessSkillInfo(long _skillId, int _level)
        {
            _m_lSkillId = _skillId;

            _m_iLevel = _level;
        }
        
        public long skillId => _m_lSkillId;
        public int level => _m_iLevel;
        
        public ConsortBlessSkillRefObj consortBlessSkillRef
        {
            get
            {
                if(_m_rConsortBlessSkillRef == null || _m_rConsortBlessSkillRef.bless_skill_id != _m_lSkillId)
                    _m_rConsortBlessSkillRef = GRefdataCoreMgr.instance.consortBlessSkillRefCore.getRef(_m_lSkillId);

                if(_m_rConsortBlessSkillRef == null)
                    Debug.LogError($"[ConsortBlessSkillInfo consortBlessSkillRef] 获取不到 _m_lSkillId:{_m_lSkillId} 对应的加护技能配置表数据, 请检查配置表");
                return _m_rConsortBlessSkillRef;
            }
        }

        public ConsortBlessSkillLvlRefObj consortBlessSkillLvlRefObj
        {
            get
            {
                if(_m_rConsortBlessSkillLvlRef == null || _m_rConsortBlessSkillLvlRef.bless_skill_id != _m_lSkillId || _m_rConsortBlessSkillLvlRef.lvl != _m_iLevel)
                    _m_rConsortBlessSkillLvlRef = GRefdataCoreMgr.instance.getConsortBlessSkillLvlRefObj(consortBlessSkillRef, _m_iLevel);

                if(_m_rConsortBlessSkillLvlRef == null)
                    Debug.LogError($"[ConsortBlessSkillInfo consortBlessSkillLvlRefObj] 获取不到 _m_lSkillId:{_m_lSkillId}, _m_iLevel:{_m_iLevel} 对应的加护技能等级配置表数据, 请检查配置表");
                return _m_rConsortBlessSkillLvlRef;
            }
        }
        
        public void updateSkillLevel(int _level)
        {
            _m_iLevel = _level;
        }

        /// <summary>
        /// 检查是否可以升级
        /// </summary>
        /// <returns></returns>
        public bool checkHasCanLevelUp(GGottenConsortInfo _consortInfo)
        {
            if (_consortInfo == null || _consortInfo.consortRefObj == null || !_consortInfo.consortRefObj.needShowBlessSkill() || consortBlessSkillLvlRefObj == null)
                return false;

            if(_m_rConsortBlessSkillRef?.skillLvlRefList?.GetLast()?.lvl <= _m_iLevel)
                return false;//已经满级了
            
            return consortBlessSkillLvlRefObj.cost_skill_point <= _consortInfo.charmPoint;
        }
    }
}
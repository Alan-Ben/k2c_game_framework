using System;

namespace GOE
{
    /// <summary>
    /// 妃子羁绊信息
    /// </summary>
    public class ConsortFetterInfo
    {
        private int _m_iFetterLevel;//羁绊等级
        
        private ConsortFettersLvlRefObj _m_rConsortFettersLvlRef;//羁绊等级配置表
        private ConsortFetterSkillInfo _m_iConsortFetterSkillInfo;//羁绊技能信息

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_level">羁绊等级</param>
        /// <param name="_skillId">羁绊技能id</param>
        public ConsortFetterInfo(int _level, long _skillId)
        {
            _m_iFetterLevel = _level;
            
            _m_iConsortFetterSkillInfo = new ConsortFetterSkillInfo(_skillId, consortFettersLvlRef?.consort_fetters_skill_lvl ?? 0);
        }
        
        public int fetterLevel => _m_iFetterLevel;
        
        public ConsortFettersLvlRefObj consortFettersLvlRef
        {
            get
            {
                if (_m_rConsortFettersLvlRef == null || _m_rConsortFettersLvlRef.lvl != _m_iFetterLevel)
                    _m_rConsortFettersLvlRef = GRefdataCoreMgr.instance.consortFettersLvlRefCore.getRef(_m_iFetterLevel);
                
                if(_m_rConsortFettersLvlRef == null)
                    Debug.LogError($"[ConsortFetterInfo consortFettersLvlRef] 获取不到 _m_iFetterLevel:{_m_iFetterLevel} 对应的羁绊等级配表数据, 请检查配置表");
                return _m_rConsortFettersLvlRef;
            }
        }
        
        public ConsortFetterSkillInfo consortFetterSkillInfo => _m_iConsortFetterSkillInfo;

        public long fetterSkillId { get { return _m_iConsortFetterSkillInfo?.skillId ?? 0; } }
        public int fetterSkillLevel { get { return _m_iConsortFetterSkillInfo?.skillLevel ?? 0; } }

        /// <summary>
        /// 更新羁绊等级
        /// </summary>
        public void updateFetterLvl(int _level)
        {
            _m_iFetterLevel = _level;
            
            _m_iConsortFetterSkillInfo?.updateSkillLevel(consortFettersLvlRef?.consort_fetters_skill_lvl ?? 0);
        }

        /// <summary>
        /// 更新羁绊技能
        /// </summary>
        /// <param name="_skillId">技能id</param>
        public void updateFetterSkill(long _skillId)
        {
            _m_iConsortFetterSkillInfo = new ConsortFetterSkillInfo(_skillId, consortFettersLvlRef?.consort_fetters_skill_lvl ?? 0);
        }

        public static bool checkCanLevelUp(GGottenConsortInfo _consortInfo, ConsortFetterInfo _consortFetterInfo , bool _needShowTip)
        {
            if (_consortInfo == null || _consortFetterInfo == null)
                return false;
                    
            if (_consortFetterInfo.consortFettersLvlRef == null || _consortFetterInfo.consortFettersLvlRef.isMaxLvl)
            {
                if(_needShowTip)
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.consort_fetter_levelUpMaxLevelTip_none);
                return false;
            }
            
            // long levelUpNeedPlayerLvl = _consortInfo.fetterInfo.consortFettersLvlRef?.need_player_lvl ?? 0;//升级需要玩家等级
            // https://www.teambition.com/task/67b6f358e42d3fd72e673aa8 【优化-0】家人羁绊技能升级需求修改
            int levelUpNeedConsortCount = _consortFetterInfo.consortFettersLvlRef.need_consort_num;//升级需要的妃子数量
            long levelUpNeedIntimacy = _consortFetterInfo.consortFettersLvlRef.need_consort_intimacy;//升级需要亲密度
            long levelUpNeedCharm = _consortFetterInfo.consortFettersLvlRef.need_consort_charm;//升级需要加护力

            // https://www.teambition.com/task/67b6f358e42d3fd72e673aa8 【优化-0】家人羁绊技能升级需求修改
            if (levelUpNeedConsortCount > NPPlayer.instance.consortComp.getConsortCount() ||
                levelUpNeedIntimacy > _consortInfo.intimacy || levelUpNeedCharm > _consortInfo.charm)
            {
                if(_needShowTip)
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.consort_fetter_cannotLevelUp_none);
                return false;
            }

            return true;
        }
    }

    /// <summary>
    /// 羁绊技能信息
    /// </summary>
    public class ConsortFetterSkillInfo
    {
        private long _m_lSkillId;//技能ID
        private int _m_iSkillLevel;//技能等级

        private ConsortFettersSkillRefObj _m_rConsortFettersSkillRef;//羁绊技能配置表
        private ConsortFettersSkillLvlRefObj _m_rConsortFettersSkillLvlRef;//羁绊等级配置表
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_skillId"></param>
        /// <param name="_skillLevel"></param>
        public ConsortFetterSkillInfo(long _skillId, int _skillLevel)
        {
            _m_lSkillId = _skillId;
            _m_iSkillLevel = _skillLevel;
        }

        public long skillId => _m_lSkillId;
        public int skillLevel => _m_iSkillLevel;
        
        public ConsortFettersSkillRefObj consortFettersSkillRefObj
        {
            get
            {
                if (_m_rConsortFettersSkillRef == null || _m_rConsortFettersSkillRef.fetters_skill_id != _m_lSkillId)
                    _m_rConsortFettersSkillRef = GRefdataCoreMgr.instance.consortFettersSkillRefCore.getRef(_m_lSkillId);

                if(_m_rConsortFettersSkillRef == null)
                    Debug.LogError($"[ConsortFetterSkillInfo consortFettersSkillRefObj] 获取不到 fetters_skill_id:{_m_lSkillId} 对应的羁绊技能配表数据, 请检查配置表");

                return _m_rConsortFettersSkillRef;
            }
        }
        
        public ConsortFettersSkillLvlRefObj consortFettersSkillLvlRefObj
        {
            get
            {
                if (_m_rConsortFettersSkillLvlRef == null ||
                    _m_rConsortFettersSkillLvlRef.fetters_skill_id != _m_lSkillId || _m_rConsortFettersSkillLvlRef.lvl != _m_iSkillLevel)
                {
                    _m_rConsortFettersSkillLvlRef = GRefdataCoreMgr.instance.getConsortFettersSkillLvlRefObj(consortFettersSkillRefObj, _m_iSkillLevel);
                }
                
                if(_m_rConsortFettersSkillLvlRef == null)
                    Debug.LogError($"[ConsortFetterSkillInfo consortFettersSkillLvlRefObj] 获取不到 fetters_skill_id:{_m_lSkillId} ,_m_iSkillLevel:{_m_iSkillLevel} 对应的羁绊技能等级配表数据, 请检查配置表");

                return _m_rConsortFettersSkillLvlRef;
            }
        }
        
        public void updateSkillLevel(int _skillLevel)
        {
            _m_iSkillLevel = _skillLevel;
        }
    }
}
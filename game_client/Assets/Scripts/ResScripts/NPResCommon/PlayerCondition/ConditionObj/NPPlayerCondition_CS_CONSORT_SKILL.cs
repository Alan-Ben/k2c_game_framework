using ALPackage;
using NPEnum;

namespace GOE
{
    public class NPPlayerCondition_CS_CONSORT_SKILL : _ANPBasicPlayerCondition
    {
        private long _m_consortId; // 妃子id
        private long _m_skillId; // 技能id
        private int _m_minLevel; // 最小等级
        private int _m_maxLevel; // 最大等级

        public override ENPPlayerConditionType conditionType { get { return ENPPlayerConditionType.CS_CONSORT_SKILL; } }

        public override bool isEnable(NPVarInfo _varVariableInfo)
        {
#if NP_GAME
            // if(_m_minLevel == 0)
            //     return NPPlayer.instance.consortComp.getConsortSkillUnlockType(_m_consortId, _m_skillId) == EGameCommonUnlockType.LOCK;                
            //
            // GGottenConsortInfo consortInfo = NPPlayer.instance.consortComp.getConsortInfo(_m_consortId);
            // if (null == consortInfo)
            //     return false;
            // GConsortSkillInfo skillInfo = consortInfo.getSkillInfo(_m_skillId);
            // if (null == skillInfo)
            //     return false;
            // if (skillInfo.skillLvl < _m_minLevel)
            //     return false;
            // if (_m_maxLevel != -1 && skillInfo.skillLvl > _m_maxLevel)
            //     return false;
            // return true;
            
            Debug.LogError("新项目GOM暂时没有这个条件判断");
#endif
            return false;
        }

        /// <summary>
        /// 读取条件信息
        /// </summary>
        public static NPPlayerCondition_CS_CONSORT_SKILL readStr(ALStringReader _reader)
        {
            string consortId = _reader.readItem(':');
            string skillId = _reader.readItem(':');
            string minLevel = _reader.readItem(':');
            string maxLevel = _reader.readItem(':');

            if (consortId == null || skillId == null || minLevel == null)
            {
                UnityEngine.Debug.LogError("Can not read str for ENPPlayerConditionType.CS_CONSORT_SKILL[" + _reader.srcString + "]");
                return null;
            }

            NPPlayerCondition_CS_CONSORT_SKILL cond = new NPPlayerCondition_CS_CONSORT_SKILL();
            cond._m_consortId = long.Parse(consortId);
            cond._m_skillId = long.Parse(skillId);
            cond._m_minLevel = int.Parse(minLevel);
            if (!int.TryParse(maxLevel, out cond._m_maxLevel))
            {
                cond._m_maxLevel = -1;
            }

            return cond;
        }
    }
}
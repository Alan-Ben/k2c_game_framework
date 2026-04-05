
using ALPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 妃子亲密度阶段是否达到
    /// </summary>
    public class NPPlayerCondition_C_CONSORT_INTIMACY_STEP_REACHED : _ANPBasicPlayerCondition
    {
        private long _m_lConsortId;//妃子id
        private int _m_lIntimacyStep;//亲密度阶段
        
        public override ENPPlayerConditionType conditionType { get { return ENPPlayerConditionType.C_CONSORT_INTIMACY_STEP_REACHED; } }

        /// <summary>
        /// 读取条件信息
        /// </summary>
        /// <param name="_reader"></param>
        /// <returns></returns>
        public static NPPlayerCondition_C_CONSORT_INTIMACY_STEP_REACHED readStr(ALStringReader _reader)
        {
            NPPlayerCondition_C_CONSORT_INTIMACY_STEP_REACHED cond = new NPPlayerCondition_C_CONSORT_INTIMACY_STEP_REACHED();

            //配置id
            string consortIdStr = _reader.readItem(':');
            if (string.IsNullOrEmpty(consortIdStr) || !long.TryParse(consortIdStr, out long consortId))
            {
                UnityEngine.Debug.LogError($"读取 ENPPlayerConditionType.C_CONSORT_INTIMACY_STEP_REACHED:[{_reader.srcString} 失败, 第一个参数应该为妃子consort_id, 正确格式:" +
                                           $"C_CONSORT_INTIMACY_STEP_REACHED:妃子consort_id:阶段intimacy_step");
                return null;
            }
            cond._m_lConsortId = consortId;

            string intimacyStepStr = _reader.readItem(':');
            if (string.IsNullOrEmpty(intimacyStepStr) || !int.TryParse(intimacyStepStr, out int intimacyStep))
            {
                UnityEngine.Debug.LogError($"读取 ENPPlayerConditionType.C_CONSORT_INTIMACY_STEP_REACHED:[{_reader.srcString} 失败, 第二个参数应该为阶段intimacy_step, 正确格式:" +
                                           $"C_CONSORT_INTIMACY_STEP_REACHED:妃子consort_id:阶段intimacy_step");
                return null;
            }
            cond._m_lIntimacyStep = intimacyStep;
            
            return cond;
        }
        public override bool isEnable(NPVarInfo _varVariableInfo)
        {
#if NP_GAME
            // GConsortRefObj consortRefObj = GRefdataCoreMgr.instance.consortRefCore.getRef(_m_lConsortId);
            // if (consortRefObj == null || consortRefObj.experienceList == null)
            //     return false;
            //
            // GConsortExperienceRefObj consortExperienceRefObj = consortRefObj.experienceList.Find((_refObj) =>
            // {
            //     return _refObj != null && _refObj.step_id == _m_lIntimacyStep;
            // });
            // if (consortExperienceRefObj == null)
            //     return false;
            //
            // GGottenConsortInfo consortInfo = NPPlayer.instance.consortComp.getConsortInfo(_m_lConsortId);
            // if (consortInfo == null)
            //     return false;
            //
            // return consortExperienceRefObj.need_consort_intimacy <= consortInfo.intimacy;
            Debug.LogError("新项目GOM删除了, 若还需此功能后续再开发");
            return false;
#else
            return false;
#endif
        }
    }
}

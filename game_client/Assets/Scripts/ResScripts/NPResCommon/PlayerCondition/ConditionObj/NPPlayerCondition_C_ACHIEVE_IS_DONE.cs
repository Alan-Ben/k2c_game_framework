
using ALPackage;
using NPEnum;

namespace GOE
{
    
    public class NPPlayerCondition_C_ACHIEVE_IS_DONE : _ANPBasicPlayerCondition
    {
        private long _m_lRefId = 0;

        public override ENPPlayerConditionType conditionType { get { return ENPPlayerConditionType.C_ACHIEVE_IS_DONE; } }

        /// <summary>
        /// 读取条件信息
        /// </summary>
        /// <param name="_reader"></param>
        /// <returns></returns>
        public static NPPlayerCondition_C_ACHIEVE_IS_DONE readStr(ALStringReader _reader)
        {
            NPPlayerCondition_C_ACHIEVE_IS_DONE cond = new NPPlayerCondition_C_ACHIEVE_IS_DONE();

            //配置id
            string refIdS = _reader.readItem(':');
            if (refIdS != null)
            {
                cond._m_lRefId = long.Parse(refIdS);
            }

            return cond;
        }
        public override bool isEnable(NPVarInfo _varVariableInfo)
        {
#if NP_GAME
            AchieveInfo achieveInfo = NPPlayer.instance.achieveComp.getAchimentInfo(_m_lRefId);
            if (null == achieveInfo)
                return false;
            return achieveInfo.isAllDone();
#else
            return false;
#endif
        }
    }
}

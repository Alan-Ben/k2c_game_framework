using ALPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 判断玩家Record是否在区间内
    /// </summary>
    public class NPPlayerCondition_CS_RECORD_PARAM : _ANPBasicPlayerCondition
    {
        private ENPPlayerRecordParam _m_eJudgeRecord;//判断的record类型
        private long _m_lMinCount = -1;//最小值
        private long _m_lMaxCount = -1;//最大值

        public override ENPPlayerConditionType conditionType { get { return ENPPlayerConditionType.CS_RECORD_PARAM; } }

        public override bool isEnable(NPVarInfo _varVariableInfo)
        {
            long count = 0;
#if NP_GAME
            count = NPPlayer.instance.recordComp.getValue(_m_eJudgeRecord);
#endif
            return isRange(count, _m_lMinCount, _m_lMaxCount);
        }

        /// <summary>
        /// 读取条件信息
        /// </summary>
        /// <param name="_reader"></param>
        /// <returns></returns>
        public static NPPlayerCondition_CS_RECORD_PARAM readStr(ALStringReader _reader)
        {
            string judgeRecord = _reader.readItem(':');
            string minCount = _reader.readItem(':');
            string maxCount = _reader.readItem(':');

            if (judgeRecord == null || minCount == null)
            {
                UnityEngine.Debug.LogError("Can not read str for ENPPlayerConditionType.CS_RECORD[" + _reader.srcString + "]");
                return null;
            }

            NPPlayerCondition_CS_RECORD_PARAM cond = new NPPlayerCondition_CS_RECORD_PARAM();

            cond._m_eJudgeRecord = (ENPPlayerRecordParam)ALCommon.EnumParse(typeof(ENPPlayerRecordParam), judgeRecord);
            cond._m_lMinCount = long.Parse(minCount);
            if(!string.IsNullOrEmpty(maxCount))
                cond._m_lMaxCount = long.Parse(maxCount);

            return cond;
        }
    }
}

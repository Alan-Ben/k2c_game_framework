using ALPackage;
using Common.PlayerEnum;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 判断玩家行为计数是否在区间内
    /// </summary>
    public class NPPlayerCondition_CS_EVENT_RECORD : _ANPBasicPlayerCondition
    {
        private EPlayerEventRecordType _m_eJudgeRecord;//判断的record类型
        private long _m_subId; // 具体id
        private long _m_lMinCount = -1;//最小值
        private long _m_lMaxCount = -1;//最大值

        public override ENPPlayerConditionType conditionType { get { return ENPPlayerConditionType.CS_EVENT_RECORD; } }

        public override bool isEnable(NPVarInfo _varVariableInfo)
        {
            long count = 0;
#if NP_GAME
            count = NPPlayer.instance.eventRecordComp.getValue(_m_eJudgeRecord,_m_subId);
#endif
            return isRange(count, _m_lMinCount, _m_lMaxCount);
        }

        /// <summary>
        /// 读取条件信息
        /// </summary>
        /// <param name="_reader"></param>
        /// <returns></returns>
        public static NPPlayerCondition_CS_EVENT_RECORD readStr(ALStringReader _reader)
        {
            string judgeRecord = _reader.readItem(':');
            string subId = _reader.readItem(':');
            string minCount = _reader.readItem(':');
            string maxCount = _reader.readItem(':');

            if (judgeRecord == null || subId == null || minCount == null)
            {
                UnityEngine.Debug.LogError("Can not read str for ENPPlayerConditionType.CS_EVENT_RECORD[" + _reader.srcString + "]");
                return null;
            }

            NPPlayerCondition_CS_EVENT_RECORD cond = new NPPlayerCondition_CS_EVENT_RECORD();

            cond._m_eJudgeRecord = (EPlayerEventRecordType)ALCommon.EnumParse(typeof(EPlayerEventRecordType), judgeRecord);
            cond._m_subId = long.Parse(subId);
            cond._m_lMinCount = long.Parse(minCount);
            if(!string.IsNullOrEmpty(maxCount))
                cond._m_lMaxCount = long.Parse(maxCount);

            return cond;
        }
    }
}

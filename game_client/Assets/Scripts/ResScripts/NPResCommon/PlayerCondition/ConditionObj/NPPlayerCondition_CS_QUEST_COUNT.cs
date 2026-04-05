using ALPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 判断某个任务计数是否在区间内，任务id为0代表全部任务
    /// </summary>
    public class NPPlayerCondition_CS_QUEST_COUNT : _ANPBasicPlayerCondition
    {
        private long _m_lQuestId;//判断的QuestId
        private long _m_lMinCount = -1;//最小值
        private long _m_lMaxCount = -1;//最大值

        public override ENPPlayerConditionType conditionType { get { return ENPPlayerConditionType.CS_QUEST_COUNT; } }

        public override bool isEnable(NPVarInfo _varVariableInfo)
        {
            long count = 0;
#if NP_GAME
            //任务id为0，计算全部计数
            if (_m_lQuestId == 0)
            {
                count = NPPlayer.instance.questComp.questCountItemMgr.getAllQuestCount();
            }
            else
            {
                QuestCountItem countInfo = NPPlayer.instance.questComp.questCountItemMgr.getQuestCountItem(_m_lQuestId);
                count = countInfo == null ? 0 : countInfo.getCount();
            }
#endif

            return isRange(count, _m_lMinCount, _m_lMaxCount);
        }

        /// <summary>
        /// 读取条件信息
        /// </summary>
        /// <param name="_reader"></param>
        /// <returns></returns>
        public static NPPlayerCondition_CS_QUEST_COUNT readStr(ALStringReader _reader)
        {
            string questId = _reader.readItem(':');
            string minCount = _reader.readItem(':');

            if (questId == null || minCount == null)
            {
                UnityEngine.Debug.LogError("Can not read str for ENPPlayerConditionType.CS_QUEST_COUNT[" + _reader.srcString + "]");
                return null;
            }

            NPPlayerCondition_CS_QUEST_COUNT cond = new NPPlayerCondition_CS_QUEST_COUNT();

            cond._m_lQuestId = long.Parse(questId);
            cond._m_lMinCount = long.Parse(minCount);

            string maxCount = _reader.readItem(':');
            if(null != maxCount)
                cond._m_lMaxCount = long.Parse(maxCount);

            return cond;
        }
    }
}

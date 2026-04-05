using ALPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 是否完成任务 任务完成次数 CS_QUEST_COUNT:quest_id:step_id
    /// </summary>
    public class NPPlayerConditionCS_QUEST_STEP_IS_DONE : _ANPBasicPlayerCondition
    {
        private long _m_lQuestId;//判断的QuestId
        private long _m_lStepId;//步骤id

        public override ENPPlayerConditionType conditionType { get { return ENPPlayerConditionType.CS_QUEST_STEP_IS_DONE; } }

        public override bool isEnable(NPVarInfo _varVariableInfo)
        {
#if NP_GAME
            long tmpQuestId = _m_lQuestId;
            QuestRefObj questRef = GRefdataCoreMgr.instance.questMap.getRef(tmpQuestId);
            //如果任务的静态数据不存在，说明可能配错，使用容错机制进行修正
            //GOB-6817【优化-0】针对修改主线任务配表数据，在任务完成条件判断增加容错机制
            //https://www.teambition.com/task/691c3479f0932419950f6512
            if (null == questRef)
                tmpQuestId = GRefdataCoreMgr.instance.npGeneral.default_quest_id;

            //这个任务当前进行中则根据链表判断
            QuestItem questItem = NPPlayer.instance.questComp.questItemMgr.getQuestItem(tmpQuestId);

            if (null != questItem && null != questItem.stepItem)
            {
                return questItem.getStepIdIsDone(_m_lStepId);
            }
            else
            {
                //不是当前任务则判断任务计数有就说明整个步骤都完成了
                QuestCountItem countInfo = NPPlayer.instance.questComp.questCountItemMgr.getQuestCountItem(tmpQuestId);
                if (null == countInfo)
                {
                    return false;
                }
                else
                {
                    return countInfo.getCount() > 0;
                }
            }
#endif

            return false;
        }

        /// <summary>
        /// 读取条件信息
        /// </summary>
        /// <param name="_reader"></param>
        /// <returns></returns>
        public static NPPlayerConditionCS_QUEST_STEP_IS_DONE readStr(ALStringReader _reader)
        {
            string questId = _reader.readItem(':');
            string stepId = _reader.readItem(':');

            if (questId == null || stepId == null)
            {
                UnityEngine.Debug.LogError("Can not read str for ENPPlayerConditionType.CS_QUEST_STEP_IS_DONE[" + _reader.srcString + "]");
                return null;
            }

            NPPlayerConditionCS_QUEST_STEP_IS_DONE cond = new NPPlayerConditionCS_QUEST_STEP_IS_DONE();

            cond._m_lQuestId = long.Parse(questId);
            cond._m_lStepId = long.Parse(stepId);

            return cond;
        }
    }
}

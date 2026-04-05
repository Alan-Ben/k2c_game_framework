using ALPackage;
using Common.QuestEnum;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 判断某个任务是否在进行中，参数可以精确到目标
    /// </summary>
    public class NPPlayerCondition_CS_QUEST_DOING : _ANPBasicPlayerCondition
    {
        private long _m_lQuestId;//任务id
        private long _m_lStepId = -1;//任务步骤id
        private long _m_lTargetId = -1;//目标id

        public override ENPPlayerConditionType conditionType { get { return ENPPlayerConditionType.CS_QUEST_DOING; } }

        public override bool isEnable(NPVarInfo _varVariableInfo)
        {
#if NP_GAME
            //获取任务
            QuestItem quest = NPPlayer.instance.questComp.questItemMgr.getQuestItem(_m_lQuestId);

            //获取不到任务或者任务不在进行中，为false
            if (quest == null || quest.questStatus != EQuestStatus.PROGRESSING)
                return false;

            //查找步骤
            if (_m_lStepId != -1)
            {
                //与当前步骤id不一致，为false
                if (quest.stepItem == null || quest.stepItem.questStepId != _m_lStepId)
                    return false;

                //查找目标
                if (_m_lTargetId != -1)
                {
                    //查找不到或者已经完成，为false
                    QuestTargetItem targetItem = quest.stepItem.getTarget(_m_lTargetId);
                    if (targetItem == null || targetItem.getQuestTargetIsFinish())
                        return false;
                }
            }

            return true;
#else
            return false;
#endif
        }

        /// <summary>
        /// 读取条件信息
        /// </summary>
        /// <param name="_reader"></param>
        /// <returns></returns>
        public static NPPlayerCondition_CS_QUEST_DOING readStr(ALStringReader _reader)
        {
            string questId = _reader.readItem(':');

            if (questId == null)
            {
                UnityEngine.Debug.LogError("Can not read str for ENPPlayerConditionType.CS_QUEST_DOING[" + _reader.srcString + "]");
                return null;
            }

            NPPlayerCondition_CS_QUEST_DOING cond = new NPPlayerCondition_CS_QUEST_DOING();
            cond._m_lQuestId = long.Parse(questId);

            //步骤
            string stepS = _reader.readItem(':');
            if (null != stepS)
                cond._m_lStepId = long.Parse(stepS);

            //目标
            string targetIdS = _reader.readItem(':');
            if (null != targetIdS)
                cond._m_lTargetId = long.Parse(targetIdS);

            return cond;
        }
    }
}

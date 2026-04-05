using System;
using System.Collections.Generic;

using ALPackage;
using NPEnum;

#if NP_GAME
using GOE;
#endif

namespace GOE
{
    public class NPPlayerCondition_CS_ID_JUDGE : _ANPBasicPlayerCondition
    {
        public override ENPPlayerConditionType conditionType { get { return ENPPlayerConditionType.CS_ID_JUDGE; } }

        private ENPPlayer_CS_IdJudgeFunc _m_eJudgeFunc;
        private long _m_lDataId;

        public ENPPlayer_CS_IdJudgeFunc judgeFunc { get { return _m_eJudgeFunc; } }
        public long dataId { get { return _m_lDataId; } }

        /// <summary>
        /// 读取条件信息
        /// </summary>
        /// <param name="_reader"></param>
        /// <returns></returns>
        public static NPPlayerCondition_CS_ID_JUDGE readStr(ALStringReader _reader)
        {
            string judgeFuncS = _reader.readItem(':');
            string dataIdS = _reader.readItem(':');

            if (null == judgeFuncS || null == dataIdS)
            {
                UnityEngine.Debug.LogError("Can not read str for ENPPlayerConditionType.C_ID_JUDGE[" + _reader.srcString + "]");
                return null;
            }

            NPPlayerCondition_CS_ID_JUDGE cond = new NPPlayerCondition_CS_ID_JUDGE();

            cond._m_eJudgeFunc = (ENPPlayer_CS_IdJudgeFunc)ALCommon.EnumParse(typeof(ENPPlayer_CS_IdJudgeFunc), judgeFuncS);
            cond._m_lDataId = long.Parse(dataIdS);

            return cond;
        }
        public override bool isEnable(NPVarInfo _varVariableInfo)
        {
#if NP_GAME
            switch (_m_eJudgeFunc)
            {
                case ENPPlayer_CS_IdJudgeFunc.STAGE_GOAL_STEP_IS_DONE:
                    StageGoalRefObj stageRefObj = NPPlayer.instance.stageGoalComp.stageRefObj;
                    return stageRefObj != null && stageRefObj.step > _m_lDataId;
                case ENPPlayer_CS_IdJudgeFunc.HAD_GAIN_TREASURE_HUNT_ORE:
                    TreasureHuntGotOreInfo oreInfo = NPPlayer.instance.treasureHuntComponent.getGotOreInfo(_m_lDataId);
                    return oreInfo != null;
                case ENPPlayer_CS_IdJudgeFunc.HAD_GAIN_TREASURE_HUNT_TREASURE:
                    TreasureHuntGotTreasureInfo treasureInfo = NPPlayer.instance.treasureHuntComponent.getGotTreasureInfo(_m_lDataId);
                    return treasureInfo != null;
                case ENPPlayer_CS_IdJudgeFunc.HAD_COLLECT_TREASURE_HUNT_COMPOSITE:
                    TreasureHuntCompositeCatalogInfo compositeCatalogInfo = NPPlayer.instance.treasureHuntComponent.getCompositeCatalogInfo(_m_lDataId);
                    return compositeCatalogInfo != null && compositeCatalogInfo.isCollected;
                case ENPPlayer_CS_IdJudgeFunc.HAD_DONE_SYSTEM_QUEST_TASK:
                    return NPPlayer.instance.systemQuestComp.hadDoneSystemQuest(_m_lDataId);
                default:
                    return true;
            }
#else
        return false;
#endif
        }
    }
}

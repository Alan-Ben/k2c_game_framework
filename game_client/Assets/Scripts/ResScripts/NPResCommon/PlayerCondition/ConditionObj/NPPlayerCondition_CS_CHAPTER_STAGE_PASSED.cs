using ALPackage;
using NPEnum;

namespace GOE
{
    public class NPPlayerCondition_CS_CHAPTER_STAGE_PASSED : _ANPBasicPlayerCondition
    {
        private long _m_stageId; // 关卡唯一 id

        public override ENPPlayerConditionType conditionType { get { return ENPPlayerConditionType.CS_CHAPTER_STAGE_PASSED; } }

        public override bool isEnable(NPVarInfo _varVariableInfo)
        {
#if NP_GAME
            return NPPlayer.instance.chapterComp.chapterIsPass(_m_stageId);
#endif
            return false;
        }

        /// <summary>
        /// 读取条件信息
        /// </summary>
        public static NPPlayerCondition_CS_CHAPTER_STAGE_PASSED readStr(ALStringReader _reader)
        {
            string stageId = _reader.readItem(':');

            if (stageId == null)
            {
                UnityEngine.Debug.LogError("Can not read str for ENPPlayerConditionType.CS_CHAPTER_PASSED[" + _reader.srcString + "]");
                return null;
            }

            NPPlayerCondition_CS_CHAPTER_STAGE_PASSED cond = new NPPlayerCondition_CS_CHAPTER_STAGE_PASSED();
            cond._m_stageId = long.Parse(stageId);

            return cond;
        }
    }
}
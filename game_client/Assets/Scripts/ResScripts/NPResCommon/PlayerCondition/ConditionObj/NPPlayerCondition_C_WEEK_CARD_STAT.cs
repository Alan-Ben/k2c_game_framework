
using ALPackage;
using NPEnum;

namespace GOE
{
    
    public class NPPlayerCondition_C_WEEK_CARD_STAT : _ANPBasicPlayerCondition
    {
        private EWeekCardStat _m_weekCardStat;

        public override ENPPlayerConditionType conditionType { get { return ENPPlayerConditionType.C_WEEK_CARD_STAT; } }

        /// <summary>
        /// 读取条件信息
        /// </summary>
        /// <param name="_reader"></param>
        /// <returns></returns>
        public static NPPlayerCondition_C_WEEK_CARD_STAT readStr(ALStringReader _reader)
        {
            NPPlayerCondition_C_WEEK_CARD_STAT cond = new NPPlayerCondition_C_WEEK_CARD_STAT();

            //配置id
            string refIdS = _reader.readItem(':');
            if (refIdS != null)
            {
                cond._m_weekCardStat = (EWeekCardStat)ALCommon.EnumParse(typeof(EWeekCardStat),refIdS);
            }

            return cond;
        }
        public override bool isEnable(NPVarInfo _varVariableInfo)
        {
#if NP_GAME
            EWeekCardStat weekCardStat = NPPlayer.instance.weekCardComp.getWeekCardStat();
            return (weekCardStat == _m_weekCardStat);
#else
            return false;
#endif
        }
    }
}

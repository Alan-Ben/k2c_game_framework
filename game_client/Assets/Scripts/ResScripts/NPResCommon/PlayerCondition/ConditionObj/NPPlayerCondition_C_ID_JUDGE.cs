using System;
using System.Collections.Generic;

using ALPackage;
using NPEnum;

#if NP_GAME
using GOE;
#endif

namespace GOE
{
    public class NPPlayerCondition_C_ID_JUDGE : _ANPBasicPlayerCondition
    {
        public override ENPPlayerConditionType conditionType { get { return ENPPlayerConditionType.C_ID_JUDGE; } }

        private EPlayer_C_IdJudgeFunc _m_eJudgeFunc;
        private long _m_lDataId;

        public EPlayer_C_IdJudgeFunc judgeFunc { get { return _m_eJudgeFunc; } }
        public long dataId { get { return _m_lDataId; } }

        /// <summary>
        /// 读取条件信息
        /// </summary>
        /// <param name="_reader"></param>
        /// <returns></returns>
        public static NPPlayerCondition_C_ID_JUDGE readStr(ALStringReader _reader)
        {
            string judgeFuncS = _reader.readItem(':');
            string dataIdS = _reader.readItem(':');

            if (null == judgeFuncS || null == dataIdS)
            {
                UnityEngine.Debug.LogError("Can not read str for ENPPlayerConditionType.C_ID_JUDGE[" + _reader.srcString + "]");
                return null;
            }

            NPPlayerCondition_C_ID_JUDGE cond = new NPPlayerCondition_C_ID_JUDGE();

            cond._m_eJudgeFunc = (EPlayer_C_IdJudgeFunc)ALCommon.EnumParse(typeof(EPlayer_C_IdJudgeFunc), judgeFuncS);
            cond._m_lDataId = long.Parse(dataIdS);

            return cond;
        }
        public override bool isEnable(NPVarInfo _varVariableInfo)
        {
#if NP_GAME
            switch (_m_eJudgeFunc)
            {
                case EPlayer_C_IdJudgeFunc.IS_TUTORIAL_DONE:
                    return NPPlayer.instance.tutorialComp.isTutorialDone(_m_lDataId);
                case EPlayer_C_IdJudgeFunc.IS_FUNC_UNLOCK_TIP_DONE:
                    return NPPlayer.instance.funcUnlockComp.isFuncUnlockTipDone((ENPFunctionType)_m_lDataId);
                case EPlayer_C_IdJudgeFunc.CUR_IS_IN_MISSION_WIN_WND:
                    return false;
                case EPlayer_C_IdJudgeFunc.IS_GENDER_TYPE_JUDGE:
                    ENPGenderType genderType = NPPlayer.instance.playerInfo.getCurrentGenderType();
                    return (int) genderType == _m_lDataId;
                default:
                    return true;
            }
#else
        return false;
#endif
        }
    }
}

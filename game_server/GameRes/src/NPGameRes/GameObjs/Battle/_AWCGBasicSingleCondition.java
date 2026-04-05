package NPGameRes.GameObjs.Battle;

import NPGameRes.GameObjs.Battle.Condition.*;
import WCGCommon.Enum.NPEnum.EWCGSingleConditionType;

public abstract class _AWCGBasicSingleCondition
{
    /******************
     * 获取条件类型
     *
     * @author alzq.z
     * @time Nov 12, 2013 10:56:58 PM
     */
    public abstract EWCGSingleConditionType conditionType();


    /********************
     * 从节点中读取相关信息
     *
     * @author alzq.z
     * @time Jun 27, 2013 12:29:59 AM
     */
    public static _AWCGBasicSingleCondition readCondition(EWCGSingleConditionType _conditionType, String _infoStr)
    {
        if (EWCGSingleConditionType.AI_V_RNG == _conditionType)
            return WCGConditionAIValueRange.read(_infoStr);
        if (EWCGSingleConditionType.RANGE == _conditionType)
            return WCGConditionRange.read(_infoStr);
        if (EWCGSingleConditionType.SK_OK == _conditionType)
            return WCGConditionSkillCD.read(_infoStr);
        if (EWCGSingleConditionType.SK_IDX_OK == _conditionType)
            return WCGConditionSkillIndexCD.read(_infoStr);
        if (EWCGSingleConditionType.RNG_S_COND == _conditionType)
            return WCGConditionRangeConditionS.read(_infoStr);
        if (EWCGSingleConditionType.RNG_NUM_S_COND == _conditionType)
            return WCGConditionRangeNumberS.read(_infoStr);
        if (EWCGSingleConditionType.RNG_D_COND == _conditionType)
            return WCGConditionRangeCondition.read(_infoStr);
        if (EWCGSingleConditionType.RNG_NUM_D_COND == _conditionType)
            return WCGConditionSkillIndexCD.read(_infoStr);

        if (EWCGSingleConditionType.RALL == _conditionType)
            return WCGConditionRangeAll.read(_infoStr);
        if (EWCGSingleConditionType.RALL_S_COND == _conditionType)
            return WCGConditionRangeAllConditionS.read(_infoStr);
        if (EWCGSingleConditionType.RALL_NUM_S_COND == _conditionType)
            return WCGConditionRangeAllNumberS.read(_infoStr);
        if (EWCGSingleConditionType.RALL_D_COND == _conditionType)
            return WCGConditionRangeAllCondition.read(_infoStr);
        if (EWCGSingleConditionType.RALL_NUM_D_COND == _conditionType)
            return WCGConditionRangeAllNumber.read(_infoStr);

        if (EWCGSingleConditionType.V_RNG == _conditionType)
            return WCGConditionValueRange.read(_infoStr);
        if (EWCGSingleConditionType.V_RNG_S == _conditionType)
            return WCGConditionSingleValueRangeSenior.read(_infoStr);
        if (EWCGSingleConditionType.P_RNG == _conditionType)
            return WCGConditionProRange.readCond(_infoStr);
        if (EWCGSingleConditionType.BUF_STK == _conditionType)
            return WCGConditionBuffStack.readCond(_infoStr);
        if (EWCGSingleConditionType.BUF_STK_S == _conditionType)
            return WCGConditionBuffStackSenior.readCond(_infoStr);

        if (EWCGSingleConditionType.BUF_T_STK == _conditionType)
            return WCGConditionBuffTypeStack.readCond(_infoStr);
        if (EWCGSingleConditionType.SPECIAL == _conditionType)
            return WCGConditionSpecial.read(_infoStr);
        if (EWCGSingleConditionType.SPE_TAG_T == _conditionType)
            return WCGConditionSpecialTag_T.readCond(_infoStr);
        if (EWCGSingleConditionType.SPE_TAG_F == _conditionType)
            return WCGConditionSpecialTag_F.readCond(_infoStr);
        if (EWCGSingleConditionType.RACE_ID == _conditionType)
            return WCGConditionRaceID.readCond(_infoStr);
        if (EWCGSingleConditionType.ACT_TYPE == _conditionType)
            return WCGConditionActorType.readCond(_infoStr);
        if (EWCGSingleConditionType.ACT_ID == _conditionType)
            return WCGConditionActorId.readCond(_infoStr);
        if (EWCGSingleConditionType.MOVE_TYPE == _conditionType)
            return WCGConditionMoveType.readCond(_infoStr);
        if (EWCGSingleConditionType.TEAM_P_R == _conditionType)
            return WCGConditionTeamProRange.readCond(_infoStr);
        if (EWCGSingleConditionType.TEAM_V_R == _conditionType)
            return WCGConditionTeamValueRange.readCond(_infoStr);
        if (EWCGSingleConditionType.SELF_CNT == _conditionType)
            return WCGConditionSelfCount.readCond(_infoStr);
        if (EWCGSingleConditionType.RNG_FT == _conditionType)
            return WCGConditionRangeFighting.readCond(_infoStr);
        if (EWCGSingleConditionType.UNIT_TYP_T == _conditionType)
            return WCGConditionUnitTyp_T.readCond(_infoStr);
        if (EWCGSingleConditionType.UNIT_TYP_F == _conditionType)
            return WCGConditionUnitTyp_F.readCond(_infoStr);
        if (EWCGSingleConditionType.BUF_MAX == _conditionType)
            return WCGConditionBuffMax.readCond(_infoStr);
        if (EWCGSingleConditionType.BUF_T_MAX == _conditionType)
            return WCGConditionBuffTypeMax.readCond(_infoStr);
        if (EWCGSingleConditionType.AREA_RELATION == _conditionType)
            return WCGConditionSingleAreaRelation.readCond(_infoStr);

        if (EWCGSingleConditionType.S_COMP == _conditionType)
            return WCGConditionSeniorCompare.readCond(_infoStr);
        if (EWCGSingleConditionType.S_RNG == _conditionType)
            return WCGConditionSRng.readCond(_infoStr);


        if (EWCGSingleConditionType.TEAM_COND == _conditionType)
            return WCGConditionTeamCond.readCond(_infoStr);
        if (EWCGSingleConditionType.SPE_TEAM_COND == _conditionType)
            return WCGConditionSpeTeamCond.readCond(_infoStr);

        if (EWCGSingleConditionType.OWNER_S_COND == _conditionType)
            return WCGConditionOwnerCond.readCond(_infoStr);

        if (EWCGSingleConditionType.ACT_SID == _conditionType)
            return WCGConditionActorSid.readCond(_infoStr);

        if (EWCGSingleConditionType.ACTOR_OPERATION_FEASIBILITY == _conditionType)
            return WCGConditionActorOperationFeasibility.readCond(_infoStr);
        if (EWCGSingleConditionType.RANGE_O == _conditionType)
            return WCGConditionRange_Owner.read(_infoStr);
        if (EWCGSingleConditionType.RNG_S_O_COND == _conditionType)
            return WCGConditionRangeConditionSOwner.read(_infoStr);
        if (EWCGSingleConditionType.RNG_NUM_S_O_COND == _conditionType)
            return WCGConditionRangeNumberSOwner.read(_infoStr);
        if (EWCGSingleConditionType.RNG_D_O_COND == _conditionType)
            return WCGConditionRangeDualConditionOwner.read(_infoStr);
        if (EWCGSingleConditionType.RNG_NUM_D_O_COND == _conditionType)
            return WCGConditionRangeDualNumberOwner.read(_infoStr);
        if (EWCGSingleConditionType.BUF_SPE_TAG == _conditionType)
            return WCGConditionBuffSpecialType.readCond(_infoStr);
        if (EWCGSingleConditionType.DNG_SPE_TAG == _conditionType)
            return WCGConditionDungeonSpeicalType.readCond(_infoStr);
        if (EWCGSingleConditionType.CTL_STAT == _conditionType)
            return WCGConditionControlState.readCond(_infoStr);


        return null;
    }
}
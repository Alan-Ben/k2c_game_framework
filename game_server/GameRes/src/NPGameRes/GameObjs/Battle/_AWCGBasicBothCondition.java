package NPGameRes.GameObjs.Battle;

import NPGameRes.GameObjs.Battle.Condition.*;
import WCGCommon.Enum.NPEnum.EWCGBothConditionType;

public abstract class _AWCGBasicBothCondition
{
    /******************
     * 获取条件类型
     *
     * @author alzq.z
     * @time Nov 12, 2013 10:56:58 PM
     */
    public abstract EWCGBothConditionType conditionType();


    /********************
     * 从节点中读取相关信息
     *
     * @author alzq.z
     * @time Jun 27, 2013 12:29:59 AM
     */
    public static _AWCGBasicBothCondition readCondition(EWCGBothConditionType _conditionType, String _infoStr)
    {
        if (EWCGBothConditionType.P_COMP == _conditionType)
            return WCGConditionProCompare.readCond(_infoStr);
        if (EWCGBothConditionType.V_COMP == _conditionType)
            return WCGConditionValueCompare.readCond(_infoStr);
        if (EWCGBothConditionType.A_COND == _conditionType)
            return WCGConditionActorCondition.read(_infoStr);
        if (EWCGBothConditionType.BUF_INS_STK == _conditionType)
            return WCGConditionBufInsStk.read(_infoStr);
        if (EWCGBothConditionType.BUF_INS_T_STK == _conditionType)
            return WCGConditionBufInsTypeStk.read(_infoStr);
        if (EWCGBothConditionType.BUF_INS_STK_S == _conditionType)
            return WCGConditionBufInsStkSenior.read(_infoStr);
        if (EWCGBothConditionType.BELONG == _conditionType)
            return WCGConditionBelong.read(_infoStr);
        if (EWCGBothConditionType.RELATION == _conditionType)
            return WCGConditionRelation.read(_infoStr);
        if (EWCGBothConditionType.S_COMP == _conditionType)
            return WCGConditionResCompare.readCond(_infoStr);
        if (EWCGBothConditionType.S_RNG == _conditionType)
            return WCGGroupConditionSRng.readCond(_infoStr);
        return null;
    }
}
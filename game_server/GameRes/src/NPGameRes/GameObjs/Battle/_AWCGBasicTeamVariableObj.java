package NPGameRes.GameObjs.Battle;

import WCGCommon.Enum.NPEnum.EWCGTeamVariableType;


/*******************
 * 变量结构体基类
 **/
public abstract class _AWCGBasicTeamVariableObj
{
    /******************
     * 获取条件类型
     */
    public abstract EWCGTeamVariableType variableType();


    /********************
     * 从节点中读取相关信息
     */
    public static _AWCGBasicTeamVariableObj readVariable(EWCGTeamVariableType _variableType, String _infoStr)
    {
        if (EWCGTeamVariableType.NUM == _variableType)
            return WCGTeamVariableNum.readVariable(_infoStr);
        if (EWCGTeamVariableType.TEAM_PRO == _variableType)
            return WCGTeamVariableTeamPro.readVariable(_infoStr);
        if (EWCGTeamVariableType.TEAM_VALUE == _variableType)
            return WCGTeamVariableTeamValue.readVariable(_infoStr);
        if (EWCGTeamVariableType.RND == _variableType)
            return WCGTeamVariableRnd.readVariable(_infoStr);
        if (EWCGTeamVariableType.SPE_GROUP_PRO == _variableType)
            return WCGTeamVariableSpecialGroupPro.readVariable(_infoStr);
        if (EWCGTeamVariableType.SPE_GROUP_VALUE == _variableType)
            return WCGTeamVariableSpecialGroupValue.readVariable(_infoStr);
        if (EWCGTeamVariableType.SPE_GROUP_RES == _variableType)
            return WCGTeamVariableTeamRes.readVariable(_infoStr);
        if (EWCGTeamVariableType.CAMP_PRO == _variableType)
            return WCGTeamVariableCampValue.readVariable(_infoStr);
        if (EWCGTeamVariableType.CAMP_RES == _variableType)
            return WCGTeamVariableCampRes.readVariable(_infoStr);
        if (EWCGTeamVariableType.ALL_TEAM_VALUE == _variableType)
            return WCGTeamVariableAllTeamValue.readVariable(_infoStr);
        if (EWCGTeamVariableType.AVG_TEAM_VALUE == _variableType)
            return WCGTeamVariableAvgTeamValue.readVariable(_infoStr);

        if (EWCGTeamVariableType.ALL_CAMP_VALUE == _variableType)
            return WCGTeamVariableAllCampValue.readVariable(_infoStr);
        if (EWCGTeamVariableType.AVG_CAMP_VALUE == _variableType)
            return WCGTeamVariableAvgCampValue.readVariable(_infoStr);
        if (EWCGTeamVariableType.ALL_TEAM_NUM == _variableType)
            return WCGTeamVariableAllCampTeamNum.readVariable(_infoStr);
        return null;
    }
}

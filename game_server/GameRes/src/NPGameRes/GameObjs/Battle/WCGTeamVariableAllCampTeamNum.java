package NPGameRes.GameObjs.Battle;

import WCGCommon.Enum.NPEnum.EWCGTeamVariableType;


public class WCGTeamVariableAllCampTeamNum extends _AWCGBasicTeamVariableObj
{
    protected WCGTeamVariableAllCampTeamNum()
    {
    }

    /******************
     * 获取条件类型
     */
    public EWCGTeamVariableType variableType()
    {
        return EWCGTeamVariableType.ALL_TEAM_NUM;
    }


    public static WCGTeamVariableAllCampTeamNum readVariable(String _str)
    {
        WCGTeamVariableAllCampTeamNum variableObj = new WCGTeamVariableAllCampTeamNum();

        return variableObj;
    }
}
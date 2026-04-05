package NPGameRes.GameObjs.Battle;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import WCGCommon.Enum.NPEnum.ENPResouceType;
import WCGCommon.Enum.NPEnum.EWCGTeamVariableType;

//指定类型的队伍资源
public class WCGTeamVariableTeamRes extends _AWCGBasicTeamVariableObj
{
    private int _m_lGroupId;
    private ENPResouceType _m_eResType;

    public int groupId()
    {
        return _m_lGroupId;
    }

    public ENPResouceType valueType()
    {
        return _m_eResType;
    }

    public EWCGTeamVariableType variableType()
    {
        return EWCGTeamVariableType.SPE_GROUP_RES;
    }


    public static WCGTeamVariableTeamRes readVariable(String _str)
    {
        WCGTeamVariableTeamRes variableObj = new WCGTeamVariableTeamRes();

        //解析字符串
        String[] strs = CommonFunc.charSplit(_str, '@');
        //逐个判断
        if (strs.length < 2)
        {
            CommLog.error("高级公式配置错误 - Cost example: enum:groupId:ResvalueType Error Str: " + _str);
            return null;
        }

        try
        {
            variableObj._m_lGroupId = Integer.parseInt(strs[0].trim());
            variableObj._m_eResType = ENPResouceType.valueOf(strs[1].toUpperCase().trim());

            return variableObj;
        } catch (Exception e)
        {
            CommLog.error("高级公式配置错误 - Cost example: enum:groupId:ResvalueType Error Str: " + _str, e);
            return null;
        }
    }

}
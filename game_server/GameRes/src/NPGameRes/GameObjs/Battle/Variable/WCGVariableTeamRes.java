package NPGameRes.GameObjs.Battle.Variable;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle._AWCGBasicVariableObj;
import WCGCommon.Enum.NPEnum.ENPResouceType;
import WCGCommon.Enum.NPEnum.EWCGVariableType;

//指定类型的队伍资源
public class WCGVariableTeamRes extends _AWCGBasicVariableObj
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

    public EWCGVariableType variableType()
    {
        return EWCGVariableType.SPE_GROUP_RES;
    }


    public static WCGVariableTeamRes readVariable(String _str)
    {
        WCGVariableTeamRes variableObj = new WCGVariableTeamRes();

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
            CommLog.error("高级公式配置错误 - Cost example: enum:groupId:ResvalueType Error Str: " + _str);
            return null;
        }
    }

}

package NPGameRes.GameObjs.Battle.Variable;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle._AWCGBasicVariableObj;
import WCGCommon.Enum.NPEnum.EWCGCampResouceType;
import WCGCommon.Enum.NPEnum.EWCGVariableType;

//指定类型的队伍资源
public class WCGVariableCampRes extends _AWCGBasicVariableObj
{
    private int _m_lCampId;
    private EWCGCampResouceType _m_eResType;

    public int campId()
    {
        return _m_lCampId;
    }

    public EWCGCampResouceType valueType()
    {
        return _m_eResType;
    }

    public EWCGVariableType variableType()
    {
        return EWCGVariableType.CAMP_RES;
    }


    public static WCGVariableCampRes readVariable(String _str)
    {
        WCGVariableCampRes variableObj = new WCGVariableCampRes();

        //解析字符串
        String[] strs = CommonFunc.charSplit(_str, '@');
        //逐个判断
        if (strs.length < 2)
        {
            CommLog.error("高级公式配置错误 - Cost example: enum:gampid:ResvalueType Error Str: " + _str);
            return null;
        }

        try
        {
            variableObj._m_lCampId = Integer.parseInt(strs[0].trim());
            variableObj._m_eResType = EWCGCampResouceType.valueOf(strs[1].toUpperCase().trim());

            return variableObj;
        } catch (Exception e)
        {
            CommLog.error("高级公式配置错误 - Cost example: enum:groupId:ResvalueType Error Str: " + _str, e);
            return null;
        }
    }

}

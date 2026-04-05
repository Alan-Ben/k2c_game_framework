package NPGameRes.GameObjs.Battle.Variable;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle._AWCGBasicVariableObj;
import WCGCommon.Enum.NPEnum.ENPTeamPropertyType;
import WCGCommon.Enum.NPEnum.EWCGVariableType;

//建造消耗值
public class WCGVariableSpecialGroupPro extends _AWCGBasicVariableObj
{
    private int _m_lGroupId;
    private ENPTeamPropertyType _m_eProType;

    public int groupId()
    {
        return _m_lGroupId;
    }

    public ENPTeamPropertyType propertyType()
    {
        return _m_eProType;
    }

    public EWCGVariableType variableType()
    {
        return EWCGVariableType.SPE_GROUP_PRO;
    }


    public static WCGVariableSpecialGroupPro readVariable(String _str)
    {
        WCGVariableSpecialGroupPro variableObj = new WCGVariableSpecialGroupPro();

        //解析字符串
        String[] strs = CommonFunc.charSplit(_str, '@');
        //逐个判断
        if (strs.length < 2)
        {
            CommLog.error("高级公式配置错误 - Cost example: enum:groupId:proType Error Str: " + _str);
            return null;
        }

        try
        {
            variableObj._m_lGroupId = Integer.parseInt(strs[0].trim());
            variableObj._m_eProType = ENPTeamPropertyType.valueOf(strs[1].toUpperCase().trim());

            return variableObj;
        } catch (Exception e)
        {
            CommLog.error("高级公式配置错误 - Cost example: enum:groupId:proType Error Str: " + _str, e);
            return null;
        }
    }

}

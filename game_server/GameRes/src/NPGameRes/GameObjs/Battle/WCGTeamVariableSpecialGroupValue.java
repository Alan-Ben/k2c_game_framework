package NPGameRes.GameObjs.Battle;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import WCGCommon.Enum.NPEnum.EWCGTeamValueType;
import WCGCommon.Enum.NPEnum.EWCGTeamVariableType;

//建造消耗值
public class WCGTeamVariableSpecialGroupValue extends _AWCGBasicTeamVariableObj
{
    private int _m_lGroupId;
    private EWCGTeamValueType _m_eValueType;

    public int groupId()
    {
        return _m_lGroupId;
    }

    public EWCGTeamValueType valueType()
    {
        return _m_eValueType;
    }

    public EWCGTeamVariableType variableType()
    {
        return EWCGTeamVariableType.SPE_GROUP_VALUE;
    }


    public static WCGTeamVariableSpecialGroupValue readVariable(String _str)
    {
        WCGTeamVariableSpecialGroupValue variableObj = new WCGTeamVariableSpecialGroupValue();

        //解析字符串
        String[] strs = CommonFunc.charSplit(_str, '@');
        //逐个判断
        if (strs.length < 2)
        {
            CommLog.error("高级公式配置错误 - Cost example: enum:groupId:valueType Error Str: " + _str);
            return null;
        }

        try
        {
            variableObj._m_lGroupId = Integer.parseInt(strs[0].trim());
            variableObj._m_eValueType = EWCGTeamValueType.valueOf(strs[1].toUpperCase().trim());

            return variableObj;
        } catch (Exception e)
        {
            CommLog.error("高级公式配置错误 - Cost example: enum:groupId:valueType Error Str: " + _str, e);
            return null;
        }
    }

}
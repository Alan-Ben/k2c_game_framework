package NPGameRes.GameObjs.Battle.Variable;

import ALServerLog.ALServerLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle._AWCGBasicVariableObj;
import WCGCommon.Enum.NPEnum.EWCGEffectTargetType;
import WCGCommon.Enum.NPEnum.EWCGVariableType;

public class WCGVariableBuffStack extends _AWCGBasicVariableObj
{

    private EWCGEffectTargetType _m_eTargetType;

    private long _m_lBuffId;

    public EWCGEffectTargetType TargetType()
    {
        return _m_eTargetType;
    }

    public long BuffID()
    {
        return _m_lBuffId;
    }

    @Override
    public WCGCommon.Enum.NPEnum.EWCGVariableType variableType()
    {
        return EWCGVariableType.BUF_STACK;
    }

    public static WCGVariableBuffStack readVariable(String _str)
    {

        WCGVariableBuffStack variableObj = new WCGVariableBuffStack();

        //解析字符串
        String[] strs = CommonFunc.charSplit(_str, '@');
        //逐个判断
        if (strs.length < 2)
        {
            ALServerLog.Error("Error Format for Variable - BUF_STACK example: enum:target:buff_id Error Str: " + _str);
            return null;
        }

        try
        {
            variableObj._m_eTargetType = EWCGEffectTargetType.valueOf(strs[0].toUpperCase().trim());
            variableObj._m_lBuffId = Long.parseLong(strs[1].trim());

            return variableObj;
        } catch (Exception e)
        {
            ALServerLog.Error("Error Format for Variable - BUF_STACK example: enum:target:buff_id Error Str: " + _str);
            return null;
        }
    }


}

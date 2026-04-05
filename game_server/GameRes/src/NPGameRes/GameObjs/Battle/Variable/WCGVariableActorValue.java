package NPGameRes.GameObjs.Battle.Variable;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle._AWCGBasicVariableObj;
import WCGCommon.Enum.NPEnum.EWCGEffectTargetType;
import WCGCommon.Enum.NPEnum.EWCGVariableType;


public class WCGVariableActorValue extends _AWCGBasicVariableObj
{
    private EWCGEffectTargetType _m_eTargetType;
    /**
     * 具体状态值索引
     */
    private int _m_iIndex;

    public EWCGEffectTargetType TargetType()
    {
        return _m_eTargetType;
    }

    public int index()
    {
        return _m_iIndex;
    }

    protected WCGVariableActorValue()
    {
        _m_iIndex = 0;
    }

    /******************
     * 获取条件类型
     *
     * @author alzq.z
     * @time Nov 12, 2013 10:56:58 PM
     */
    public EWCGVariableType variableType()
    {
        return EWCGVariableType.ACTOR_VALUE;
    }


    public static WCGVariableActorValue readVariable(String _str)
    {
        WCGVariableActorValue variableObj = new WCGVariableActorValue();

        //解析字符串
        String[] strs = CommonFunc.charSplit(_str, '@');
        //逐个判断
        if (strs.length < 2)
        {
            CommLog.error("高级公式配置错误 - ACTOR_VALUE  example: enum:target:index Error Str: " + _str);
            return null;
        }

        try
        {
            variableObj._m_eTargetType = EWCGEffectTargetType.valueOf(strs[0].trim().toUpperCase());
            variableObj._m_iIndex = Integer.parseInt(strs[1].trim());

            return variableObj;
        } catch (Exception e)
        {
            CommLog.error("高级公式配置错误 - ACTOR_VALUE  example: enum:target:index Error Str: " + _str, e);
            return null;
        }
    }
}

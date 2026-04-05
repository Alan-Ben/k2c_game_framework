package NPGameRes.GameObjs.Battle.Variable;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle._AWCGBasicVariableObj;
import WCGCommon.Enum.NPEnum.EWCGEffectTargetType;
import WCGCommon.Enum.NPEnum.EWCGVariableType;

public class WCGVariableSpActorValue extends _AWCGBasicVariableObj
{
    private EWCGEffectTargetType _m_eTargetType;
    /**
     * 索引值
     */
    private long _m_lActorId;
    /**
     * 具体状态值索引
     */
    private int _m_iIndex;

    public EWCGEffectTargetType TargetType()
    {
        return _m_eTargetType;
    }

    public long actorId()
    {
        return _m_lActorId;
    }

    public int index()
    {
        return _m_iIndex;
    }

    protected WCGVariableSpActorValue()
    {
        _m_lActorId = 0;
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
        return EWCGVariableType.SP_ACTOR_VALUE;
    }


    public static WCGVariableSpActorValue readVariable(String _str)
    {
        WCGVariableSpActorValue variableObj = new WCGVariableSpActorValue();

        //解析字符串
        String[] strs = CommonFunc.charSplit(_str, '@');
        //逐个判断
        if (strs.length < 3)
        {
            CommLog.error("高级公式配置错误 - SP_ACTOR_VALUE   example: enum:target:specialId:index Error Str: " + _str);
            return null;
        }

        try
        {
            variableObj._m_eTargetType = EWCGEffectTargetType.valueOf(strs[0].trim().toUpperCase());
            variableObj._m_lActorId = Long.parseLong(strs[1].trim());
            variableObj._m_iIndex = Integer.parseInt(strs[2].trim());

            return variableObj;
        } catch (Exception e)
        {
            CommLog.error("高级公式配置错误 - SP_ACTOR_VALUE   example: enum:target:specialId:index Error Str: " + _str, e);
            return null;
        }
    }
}
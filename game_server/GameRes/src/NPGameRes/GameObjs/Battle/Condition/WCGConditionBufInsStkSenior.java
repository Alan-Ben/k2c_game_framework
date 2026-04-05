package NPGameRes.GameObjs.Battle.Condition;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle.Variable.WCGVariableGroupObj;
import NPGameRes.GameObjs.Battle._AWCGBasicBothCondition;
import WCGCommon.Enum.NPEnum.EWCGBothConditionType;
import WCGCommon.Enum.NPEnum.EWCGEffectTargetType;


/// <summary>
/// V_COMP - 状态值比较   格式如下：枚举:用于比对的对象类型:比对的目标的对象类型:状态值类型:比较类型
/// </summary>
public class WCGConditionBufInsStkSenior extends _AWCGBasicBothCondition
{

    private EWCGEffectTargetType _m_eBufActorType;
    private EWCGEffectTargetType _m_eInsType;

    private WCGVariableGroupObj _m_voBufIdVar;

    private WCGVariableGroupObj _m_voBufStkMin;
    private WCGVariableGroupObj _m_voBufStkMax;

    public EWCGEffectTargetType bufActorType()
    {
        return _m_eBufActorType;
    }

    public EWCGEffectTargetType insType()
    {
        return _m_eInsType;
    }

    public WCGVariableGroupObj BuffID()
    {
        return _m_voBufIdVar;
    }

    public WCGVariableGroupObj min()
    {
        return _m_voBufStkMin;
    }

    public WCGVariableGroupObj max()
    {
        return _m_voBufStkMax;
    }


    public EWCGBothConditionType conditionType()
    {
        return EWCGBothConditionType.BUF_INS_STK_S;
    }

    public static WCGConditionBufInsStkSenior read(String _str)
    {
        WCGConditionBufInsStkSenior cond = new WCGConditionBufInsStkSenior();

        String[] strs = CommonFunc.charSplit(_str, ':');

        if (strs.length < 5)
        {
            CommLog.error("条件配置错误 - BUF_INS_STK_S example: enum:buf_actor_type:ins_type:buffid:min:max... Error Str: " + _str);
            return null;
        }

        try
        {
            //读取类型枚举
            cond._m_eBufActorType = EWCGEffectTargetType.valueOf(strs[0].trim().toUpperCase());
            if (strs[1].length() > 0)
                cond._m_eInsType = EWCGEffectTargetType.valueOf(strs[1].trim().toUpperCase());
            else
                cond._m_eInsType = EWCGEffectTargetType.NONE;
            cond._m_voBufIdVar = WCGVariableGroupObj.readVariableGroup(strs[2].trim(), "buf_stk_s1");
            cond._m_voBufStkMin = WCGVariableGroupObj.readVariableGroup(strs[3].trim(), "buf_stk_s2");
            cond._m_voBufStkMax = WCGVariableGroupObj.readVariableGroup(strs[4].trim(), "buf_stk_s3");

            return cond;
        } catch (Exception e)
        {
            CommLog.error("条件配置错误 - BUF_INS_STK_S example: enum:buf_actor_type:ins_type:buffid:min:max... Error Str: " + _str, e);
            return null;
        }
    }

    public static boolean inRange(int _value, long _min, long _max)
    {
        if (-1 == _min && -1 == _max)
            return true;

        if (-1 != _min && _value < _min)
            return false;

        if (-1 != _max && _value > _max)
            return false;

        return true;
    }
}
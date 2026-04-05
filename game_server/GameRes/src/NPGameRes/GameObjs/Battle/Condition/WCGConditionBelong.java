package NPGameRes.GameObjs.Battle.Condition;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle._AWCGBasicBothCondition;
import WCGCommon.Enum.NPEnum.EWCGBothConditionType;
import WCGCommon.Enum.NPEnum.EWCGEffectTargetType;

//后者是否是前者召唤的
public class WCGConditionBelong extends _AWCGBasicBothCondition
{

    /*比较对象*/
    private EWCGEffectTargetType _m_eCompareType;

    /*比较目标对象*/
    private EWCGEffectTargetType _m_eCompareTargetType;

    private boolean _m_bEnable;

    public EWCGEffectTargetType CompareType()
    {
        return _m_eCompareType;
    }

    public EWCGEffectTargetType CompareTargetType()
    {
        return _m_eCompareTargetType;
    }

    public boolean Enable()
    {
        return _m_bEnable;
    }

    @Override
    public EWCGBothConditionType conditionType()
    {
        return EWCGBothConditionType.BELONG;
    }


    public static WCGConditionBelong read(String _str)
    {
        WCGConditionBelong cond = new WCGConditionBelong();

        String[] strs = CommonFunc.charSplit(_str, ':');

        if (strs.length < 5)
        {
            CommLog.error("条件配置错误 - buf_ins_t_stk example: enum:buf_actor_type:ins_type:buffid:min:max... Error Str: " + _str);
            return null;
        }

        try
        {
            //读取类型枚举
            cond._m_eCompareType = EWCGEffectTargetType.valueOf(strs[0].toUpperCase().trim());
            cond._m_eCompareTargetType = EWCGEffectTargetType.valueOf(strs[1].toUpperCase().trim());
            cond._m_bEnable = Boolean.parseBoolean(strs[2]);

            return cond;
        } catch (Exception e)
        {
            CommLog.error("条件配置错误 - buf_ins_t_stk example: enum:buf_actor_type:ins_type:buffid:min:max... Error Str: " + _str, e);
            return null;
        }
    }
}
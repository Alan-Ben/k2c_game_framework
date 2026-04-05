package NPGameRes.GameObjs.Battle.Condition;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle.WCGIntRange;
import NPGameRes.GameObjs.Battle._AWCGBasicBothCondition;
import WCGCommon.Enum.NPEnum.EWCGBothConditionType;
import WCGCommon.Enum.NPEnum.EWCGEffectTargetType;

/// <summary>
/// V_COMP - 状态值比较   格式如下：枚举:用于比对的对象类型:比对的目标的对象类型:状态值类型:比较类型
/// </summary>
public class WCGConditionBufInsStk extends _AWCGBasicBothCondition
{

    private EWCGEffectTargetType _m_eBufActorType;
    private EWCGEffectTargetType _m_eInsType;
    private long _m_lBuffID;
    private WCGIntRange _m_iRangeStack;

    public EWCGEffectTargetType bufActorType()
    {
        return _m_eBufActorType;
    }

    public EWCGEffectTargetType insType()
    {
        return _m_eInsType;
    }

    public long BuffID()
    {
        return _m_lBuffID;
    }

    public WCGIntRange RangeStack()
    {
        return _m_iRangeStack;
    }


    @Override
    public EWCGBothConditionType conditionType()
    {
        return EWCGBothConditionType.BUF_INS_STK;
    }

    public static WCGConditionBufInsStk read(String _str)
    {
        WCGConditionBufInsStk cond = new WCGConditionBufInsStk();

        String[] strs = CommonFunc.charSplit(_str, ':');

        if (strs.length < 5)
        {
            CommLog.error("条件配置错误 - buf_ins_stk example: enum:buf_actor_type:ins_type:buffid:min:max... Error Str: " + _str);
            return null;
        }

        try
        {
            //读取类型枚举
            cond._m_eBufActorType = EWCGEffectTargetType.valueOf(strs[0].toUpperCase().trim());

            if (strs[1].length() > 0)
                cond._m_eInsType = EWCGEffectTargetType.valueOf(strs[1].trim().toUpperCase());
            else
                cond._m_eInsType = EWCGEffectTargetType.NONE;

            cond._m_lBuffID = Long.parseLong(strs[2].trim());
            cond._m_iRangeStack = new WCGIntRange(strs[3], strs[4]);

            return cond;
        } catch (Exception e)
        {
            CommLog.error("条件配置错误 - buf_ins_stk example: enum:buf_actor_type:ins_type:buffid:min:max... Error Str: " + _str, e);
            return null;
        }
    }
}

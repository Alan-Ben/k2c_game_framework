package NPGameRes.GameObjs.Battle.Variable;

import ALServerLog.ALServerLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle._AWCGBasicVariableObj;
import WCGCommon.Enum.NPEnum.EWCGEffectTargetType;
import WCGCommon.Enum.NPEnum.EWCGVariableType;

/// <summary>
/// 高级公式变量-buf类型的叠加层数   格式如下：枚举@对象类型@buf类型Id
/// </summary>
public class WCGVariableBuffTypeStack extends _AWCGBasicVariableObj
{
    /**
     * 条件筛选目标
     */
    private EWCGEffectTargetType _m_eTargetType;
    /**
     * buf类型Id
     */
    private long _m_lBuffTypeId;

    public EWCGEffectTargetType TargetType()
    {
        return _m_eTargetType;
    }

    public long BuffTypeId()
    {
        return _m_lBuffTypeId;
    }

    protected WCGVariableBuffTypeStack()
    {
        _m_eTargetType = EWCGEffectTargetType.NONE;
        _m_lBuffTypeId = 0;
    }

    @Override
    public WCGCommon.Enum.NPEnum.EWCGVariableType variableType()
    {
        return EWCGVariableType.BUF_TYPE_STACK;
    }

    public static WCGVariableBuffTypeStack readVariable(String _str)
    {
        WCGVariableBuffTypeStack variableObj = new WCGVariableBuffTypeStack();

        // 解析字符串
        String[] strs = CommonFunc.charSplit(_str, '@');
        // 逐个判断
        if (strs.length < 2)
        {
            ALServerLog.Error("Error Format for Variable - BUF_STACK example: enum:target:buff_id Error Str: " + _str);
            return null;
        }

        try
        {
            variableObj._m_eTargetType = EWCGEffectTargetType.valueOf(strs[0].toUpperCase().trim());
            variableObj._m_lBuffTypeId = Long.parseLong(strs[1].trim());

            return variableObj;
        } catch (Exception e)
        {
            ALServerLog.Error("Error Format for Variable - BUF_STACK example: enum:target:buff_id Error Str: " + _str);
            return null;
        }
    }

}

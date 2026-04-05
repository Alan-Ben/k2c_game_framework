package NPGameRes.GameObjs.Battle.AITrigger;

import ALServerLog.ALServerLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle.Variable.WCGVariableGroupObj;
import NPGameRes.GameObjs.Battle._AWCGBasicAITrigger;
import WCGCommon.Enum.NPEnum.EWCGAITriggerType;

public class WCGAITriggerChgIndexValue extends _AWCGBasicAITrigger
{

    private int _m_iIndex;

    private int _m_iValue;

    private WCGVariableGroupObj _m_sValueVariable;

    public int Index()
    {
        return _m_iIndex;
    }

    public int Value()
    {
        return _m_iValue;
    }

    public WCGVariableGroupObj ValueVariable()
    {
        return _m_sValueVariable;
    }

    @Override
    public WCGCommon.Enum.NPEnum.EWCGAITriggerType triggerType()
    {
        return EWCGAITriggerType.CHG_AI_V;
    }

    public static WCGAITriggerChgIndexValue read(String _infoStr)
    {
        WCGAITriggerChgIndexValue obj = new WCGAITriggerChgIndexValue();
        String[] strs = CommonFunc.charSplit(_infoStr, ':');
        if (strs.length < 2)
        {
            ALServerLog.Error("ai chg_ai_value配置错误 正确配置: 枚举:下标索引:值");
            return null;
        }
        try
        {
            obj._m_iIndex = Integer.parseInt(strs[0].trim());
            obj._m_iValue = Integer.parseInt(strs[1].trim());
            if (strs.length > 2)
                obj._m_sValueVariable = WCGVariableGroupObj.readVariableGroup(strs[2], "AI Trigger　Chg　Index高级公式错误：　");
            return obj;
        } catch (Exception e)
        {
            ALServerLog.Error("ai chg_ai_value配置错误 正确配置: 枚举:下标索引:值");
            return null;
        }
    }

}

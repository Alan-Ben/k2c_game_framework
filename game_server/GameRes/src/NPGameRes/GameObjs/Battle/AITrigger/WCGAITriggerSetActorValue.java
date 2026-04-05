package NPGameRes.GameObjs.Battle.AITrigger;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle.Variable.WCGVariableGroupObj;
import NPGameRes.GameObjs.Battle._AWCGBasicAITrigger;
import WCGCommon.Enum.NPEnum.EWCGAITriggerType;


/// <summary>
/// AI 触发效果： 设置ai中对应索引的值(idx索引暂定0-99)
/// </summary>
public class WCGAITriggerSetActorValue extends _AWCGBasicAITrigger
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

    public EWCGAITriggerType triggerType()
    {
        return EWCGAITriggerType.SET_ACTOR_V;
    }

    public static WCGAITriggerSetActorValue read(String _infoStr)
    {
        WCGAITriggerSetActorValue obj = new WCGAITriggerSetActorValue();
        String[] strs = CommonFunc.charSplit(_infoStr, ':');
        if (strs.length < 2)
        {
            CommLog.error("ai SET_ACTOR_V配置错误 正确配置: 枚举:下标索引:值");
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
            CommLog.error("ai SET_ACTOR_V配置错误 正确配置: 枚举:下标索引:值", e);
            return null;
        }
    }
}

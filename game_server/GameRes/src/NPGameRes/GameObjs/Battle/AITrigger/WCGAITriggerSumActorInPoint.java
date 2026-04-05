package NPGameRes.GameObjs.Battle.AITrigger;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle.Variable.WCGVariableGroupObj;
import NPGameRes.GameObjs.Battle._AWCGBasicAITrigger;
import WCGCommon.Enum.NPEnum.EWCGAITriggerType;
import WCGCommon.Enum.NPEnum.EWCGActorOpState;
import WCGCommon.Enum.NPEnum.EWCGUnitType;

/// <summary>
/// AI 触发效果： 在对应索引位置召唤对应队伍的对象
/// </summary>
public class WCGAITriggerSumActorInPoint extends _AWCGBasicAITrigger
{
    private int _m_iGroupId;

    private EWCGUnitType _m_eUnitType;

    private long _m_lActorID;

    private int _m_iLevel;

    private int _m_iPointId;

    private EWCGActorOpState _m_eInitState;

    private WCGVariableGroupObj _m_sPosX;

    private WCGVariableGroupObj _m_sPosZ;

    public int GroupId()
    {
        return _m_iGroupId;
    }

    public EWCGUnitType UnitType()
    {
        return _m_eUnitType;
    }

    public long ActorID()
    {
        return _m_lActorID;
    }

    public int Level()
    {
        return _m_iLevel;
    }

    public int PointId()
    {
        return _m_iPointId;
    }

    public EWCGActorOpState initState()
    {
        return _m_eInitState;
    }

    public WCGVariableGroupObj PosX()
    {
        return _m_sPosX;
    }

    public WCGVariableGroupObj PosZ()
    {
        return _m_sPosZ;
    }

    public EWCGAITriggerType triggerType()
    {
        return EWCGAITriggerType.SUM_T_POINT;
    }

    public static WCGAITriggerSumActorInPoint read(String _infoStr)
    {
        WCGAITriggerSumActorInPoint obj = new WCGAITriggerSumActorInPoint();
        //解析字符串
        String[] strs = CommonFunc.charSplit(_infoStr, ':');
        //逐个判断
        if (strs.length < 5)
        {
            CommLog.error("AI 触发效果配置错误 - SumActorInPoint example: enum:type:actorId:level:pos_index:pos_x:pos_z(:level_calc) Error Str: " + _infoStr);
            return null;
        }

        try
        {
            obj._m_iGroupId = Integer.parseInt(strs[0].trim());
            obj._m_eUnitType = EWCGUnitType.valueOf(strs[1].toUpperCase().trim());
            obj._m_lActorID = Long.parseLong(strs[2].trim());
            obj._m_iLevel = Integer.parseInt(strs[3].trim());
            obj._m_iPointId = Integer.parseInt(strs[4].trim());
            if (strs.length > 5)
                obj._m_eInitState = EWCGActorOpState.valueOf(strs[5].toUpperCase().trim());
            if (strs.length > 6)
                obj._m_sPosX = WCGVariableGroupObj.readVariableGroup(strs[6], "summon 坐标X高级公式错误： ");
            if (strs.length > 7)
                obj._m_sPosZ = WCGVariableGroupObj.readVariableGroup(strs[7], "summon 坐标Z高级公式错误： ");

            return obj;
        } catch (Exception e)
        {
            CommLog.error("AI 触发效果配置错误 - SumActorInPoint example: enum:type:actorId:level:pos_x:pos_z(:level_calc) Error Str: " + _infoStr, e);
            return null;
        }
    }
}
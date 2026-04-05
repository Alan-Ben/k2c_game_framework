package NPGameRes.GameObjs.Battle.AITrigger;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle.Variable.WCGVariableGroupObj;
import NPGameRes.GameObjs.Battle._AWCGBasicAITrigger;
import WCGCommon.Enum.NPEnum.EWCGAITriggerType;
import WCGCommon.Enum.NPEnum.EWCGActorOpState;
import WCGCommon.Enum.NPEnum.EWCGUnitType;


/// <summary>
/// AI 触发效果： 在对应位置召唤对应队伍的对象
/// </summary>
public class WCGAITriggerSumActorInPos extends _AWCGBasicAITrigger
{
    private int _m_iGroupId;

    private EWCGUnitType _m_eUnitType;

    private long _m_lActorID;

    private int _m_iLevel;

    private int _m_sPosX;

    private int _m_sPosZ;

    private EWCGActorOpState _m_eInitState;

    private WCGVariableGroupObj _m_sPosXV;

    private WCGVariableGroupObj _m_sPosZV;

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

    public int PosX()
    {
        return _m_sPosX;
    }

    public int PosZ()
    {
        return _m_sPosZ;
    }

    public EWCGActorOpState initState()
    {
        return _m_eInitState;
    }

    public WCGVariableGroupObj PosXV()
    {
        return _m_sPosXV;
    }

    public WCGVariableGroupObj PosZV()
    {
        return _m_sPosZV;
    }


    public EWCGAITriggerType triggerType()
    {
        return EWCGAITriggerType.SUM_T_POS;
    }

    public static WCGAITriggerSumActorInPos read(String _infoStr)
    {
        WCGAITriggerSumActorInPos obj = new WCGAITriggerSumActorInPos();
        //解析字符串
        String[] strs = CommonFunc.charSplit(_infoStr, ':');
        //逐个判断
        if (strs.length < 6)
        {
            CommLog.error("AI 触发效果配置错误 - SumActorInPos example: enum:type:actorId:level:pos_x:pos_z Error Str: " + _infoStr);
            return null;
        }

        try
        {
            obj._m_iGroupId = Integer.parseInt(strs[0].trim());
            obj._m_eUnitType = EWCGUnitType.valueOf(strs[1].toUpperCase().trim());
            obj._m_lActorID = Long.parseLong(strs[2].trim());
            obj._m_iLevel = Integer.parseInt(strs[3].trim());
            obj._m_sPosX = Integer.parseInt(strs[4].trim());
            obj._m_sPosZ = Integer.parseInt(strs[5].trim());
            if (strs.length > 6)
                obj._m_eInitState = EWCGActorOpState.valueOf(strs[6].toUpperCase().trim());
            if (strs.length > 7)
                obj._m_sPosXV = WCGVariableGroupObj.readVariableGroup(strs[7], "summon 坐标X高级公式错误： ");
            if (strs.length > 8)
                obj._m_sPosZV = WCGVariableGroupObj.readVariableGroup(strs[8], "summon 坐标Z高级公式错误： ");

            return obj;
        } catch (Exception e)
        {
            CommLog.error("AI 触发效果配置错误 - SumActorInPos example: enum:type:actorId:level:pos_x:pos_z Error Str: " + _infoStr, e);
            return null;
        }
    }
}
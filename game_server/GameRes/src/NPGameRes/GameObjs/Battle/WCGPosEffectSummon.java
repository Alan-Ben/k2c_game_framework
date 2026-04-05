package NPGameRes.GameObjs.Battle;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle.Variable.WCGVariableGroupObj;
import WCGCommon.Enum.NPEnum.EWCGPosEffectType;
import WCGCommon.Enum.NPEnum.EWCGUnitType;

//位置召唤效果
public class WCGPosEffectSummon extends _AWCGPosEffectInfo
{
    private EWCGUnitType _m_eUnitType;//单位类型

    private long _m_lActorID;//单位Id

    private int _m_iLevel;//单位等级

    private WCGVariableGroupObj _m_sPosX;//X坐标高级计算公式

    private WCGVariableGroupObj _m_sPosZ;//Y坐标高级计算公式

    private WCGVariableGroupObj _m_sLevelVariable; //等级高级计算公式

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

    public WCGVariableGroupObj PosX()
    {
        return _m_sPosX;
    }

    public WCGVariableGroupObj PosZ()
    {
        return _m_sPosZ;
    }

    public WCGVariableGroupObj LevelVariable()
    {
        return _m_sLevelVariable;
    }

    public EWCGPosEffectType effectType()
    {
        return EWCGPosEffectType.SUMMON;
    }

    public static WCGPosEffectSummon readVariable(String _str)
    {
        WCGPosEffectSummon effectObj = new WCGPosEffectSummon();
        //解析字符串
        String[] strs = CommonFunc.charSplit(_str, ':');
        int strsLength = strs.length;
        //逐个判断
        if (strsLength < 3)
        {
            CommLog.error("位置效果配置错误 - pos effect summon example: enum:type:actorId:level(:pos_x:pos_z:level_calc) Error Str: " + _str);
            return null;
        }

        try
        {
            effectObj._m_eUnitType = EWCGUnitType.valueOf(strs[0].toUpperCase().trim());
            effectObj._m_lActorID = Long.parseLong(strs[1].trim());
            effectObj._m_iLevel = Integer.parseInt(strs[2].trim());

            if (strsLength > 3)
                effectObj._m_sPosX = WCGVariableGroupObj.readVariableGroup(strs[3], "summon 坐标X高级公式错误： ");

            if (strsLength > 4)
                effectObj._m_sPosZ = WCGVariableGroupObj.readVariableGroup(strs[4], "summon 坐标Z高级公式错误： ");

            if (strsLength > 5)
                effectObj._m_sLevelVariable = WCGVariableGroupObj.readVariableGroup(strs[5], "pos effect summon 等级高级公式错误： ");

            return effectObj;
        } catch (Exception e)
        {
            CommLog.error("位置效果配置错误 - pos effect summon example: enum:type:actorId:level(:pos_x:pos_z:level_calc) Error Str: " + _str, e);
            return null;
        }


    }
}

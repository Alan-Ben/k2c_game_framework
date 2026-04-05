package NPGameRes.GameObjs.PlayerVariable.PlayerVariableObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerVariableType;
import NPGameRes.GameObjs.PlayerVariable._ANPBasicPlayerVariableObj;

public class NPPlayerVariable_CS_MARS_BUILDING_EQUIP_LVL extends _ANPBasicPlayerVariableObj
{
    private long _m_lId;
    private long _m_lEquipId;

    public long getId()
    {
        return _m_lId;
    }
    public long getEquipId()
    {
        return _m_lEquipId;
    }
    
    /******************
     * 获取条件类型
     */
    @Override
    public ENPPlayerVariableType variableType()
    {
        return ENPPlayerVariableType.CS_MARS_BUILDING_EQUIP_LVL;
    }

    public static NPPlayerVariable_CS_MARS_BUILDING_EQUIP_LVL readVariable(NPStringReader _reader)
    {
        NPPlayerVariable_CS_MARS_BUILDING_EQUIP_LVL variableObj = new NPPlayerVariable_CS_MARS_BUILDING_EQUIP_LVL();

        try
        {
            String param1S = _reader.readItem('@');
            variableObj._m_lId = Integer.valueOf(param1S);

            String param2S = _reader.readItem('@');
            variableObj._m_lEquipId = Integer.valueOf(param2S);

            return variableObj;
        } 
        catch (Exception e)
        {
            ALServerLog.Error("高级公式配置错误 - Value example: CS_MARS_BUILDING_EQUIP_LVL Error Str: " + _reader.getSrcString());
            return null;
        }
    }
}

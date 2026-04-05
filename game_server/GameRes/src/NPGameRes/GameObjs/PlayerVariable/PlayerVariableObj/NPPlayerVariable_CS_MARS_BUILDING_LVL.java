package NPGameRes.GameObjs.PlayerVariable.PlayerVariableObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerVariableType;
import NPGameRes.GameObjs.PlayerVariable._ANPBasicPlayerVariableObj;

public class NPPlayerVariable_CS_MARS_BUILDING_LVL extends _ANPBasicPlayerVariableObj
{
    private long _m_lId;

    public long getId()
    {
        return _m_lId;
    }
    
    /******************
     * 获取条件类型
     */
    @Override
    public ENPPlayerVariableType variableType()
    {
        return ENPPlayerVariableType.CS_MARS_BUILDING_LVL;
    }

    public static NPPlayerVariable_CS_MARS_BUILDING_LVL readVariable(NPStringReader _reader)
    {
        NPPlayerVariable_CS_MARS_BUILDING_LVL variableObj = new NPPlayerVariable_CS_MARS_BUILDING_LVL();

        try
        {
            String paramS = _reader.readItem('@');

            variableObj._m_lId = Integer.valueOf(paramS);

            return variableObj;
        } 
        catch (Exception e)
        {
            ALServerLog.Error("高级公式配置错误 - Value example: CS_MARS_BUILDING_LVL Error Str: " + _reader.getSrcString());
            return null;
        }
    }
}

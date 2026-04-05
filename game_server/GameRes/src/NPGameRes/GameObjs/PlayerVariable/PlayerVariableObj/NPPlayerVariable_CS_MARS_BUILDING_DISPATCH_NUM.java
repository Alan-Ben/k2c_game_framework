package NPGameRes.GameObjs.PlayerVariable.PlayerVariableObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerVariableType;
import NPGameRes.GameObjs.PlayerVariable._ANPBasicPlayerVariableObj;

public class NPPlayerVariable_CS_MARS_BUILDING_DISPATCH_NUM extends _ANPBasicPlayerVariableObj
{
	//表示全部建筑
    private long _m_lId = -1;

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
        return ENPPlayerVariableType.CS_MARS_BUILDING_DISPATCH_NUM;
    }

    public static NPPlayerVariable_CS_MARS_BUILDING_DISPATCH_NUM readVariable(NPStringReader _reader)
    {
        NPPlayerVariable_CS_MARS_BUILDING_DISPATCH_NUM variableObj = new NPPlayerVariable_CS_MARS_BUILDING_DISPATCH_NUM();

        try
        {
            String paramS = _reader.readItem('@');
            if(null != paramS)
            {
            	variableObj._m_lId = Integer.valueOf(paramS);
            }

            return variableObj;
        } 
        catch (Exception e)
        {
            ALServerLog.Error("高级公式配置错误 - Value example: CS_MARS_BUILDING_DISPATCH_NUM Error Str: " + _reader.getSrcString());
            return null;
        }
    }
}

package NPGameRes.GameObjs.PlayerVariable.PlayerVariableObj;

import ALServerLog.ALServerLog;
import Common.LevyEnum.ELevy_Type;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerVariableType;
import NPGameRes.GameObjs.PlayerVariable._ANPBasicPlayerVariableObj;

public class PlayerVariable_CS_LEVY_SUM extends _ANPBasicPlayerVariableObj
{
	private ELevy_Type _m_eType = ELevy_Type.NONE;

	public ELevy_Type levyType()
    {
        return _m_eType;
    }

    /******************
     * 获取条件类型
     */
    @Override
    public ENPPlayerVariableType variableType()
    {
        return ENPPlayerVariableType.CS_LEVY_SUM;
    }

    public static PlayerVariable_CS_LEVY_SUM readVariable(NPStringReader _reader)
    {
        PlayerVariable_CS_LEVY_SUM variableObj = new PlayerVariable_CS_LEVY_SUM();

        try
        {
            String typeS = _reader.readItem();
            
            variableObj._m_eType = ELevy_Type.valueOf(typeS.toUpperCase());

            return variableObj;
        } catch (Exception e)
        {
            ALServerLog.Error("高级公式——征收累计数量 CS_LEVY_SUM@ELevy_Type Error Str: " + _reader.getSrcString());
            return null;
        }
    }
}

package NPGameRes.GameObjs.PlayerVariable.PlayerVariableObj;

import Common.BagItemUseEnum.EBagItemUse_ConsortDrawShowType;
import NPCommon.CommonObj.NPStringReader;
import NPCommon.Log.CommLog;
import NPEnum.ENPPlayerVariableType;
import NPGameRes.GameObjs.PlayerVariable._ANPBasicPlayerVariableObj;

public class PlayerVariable_CS_CONSORT_RES_COUNT extends _ANPBasicPlayerVariableObj
{
	private long _m_lConsortId;
	private EBagItemUse_ConsortDrawShowType _m_eConsortResType = EBagItemUse_ConsortDrawShowType.NONE;

	public long getConsortId() {return _m_lConsortId;}
	public EBagItemUse_ConsortDrawShowType getConsortResType() {return _m_eConsortResType;}

    /******************
     * 获取条件类型
     */
    @Override
    public ENPPlayerVariableType variableType()
    {
        return ENPPlayerVariableType.CS_CONSORT_RES_COUNT;
    }

    public static PlayerVariable_CS_CONSORT_RES_COUNT readVariable(NPStringReader _reader)
    {
        PlayerVariable_CS_CONSORT_RES_COUNT variableObj = new PlayerVariable_CS_CONSORT_RES_COUNT();

        try
        {
            String idS = _reader.readItem('@');
            String typeS = _reader.readItem('@');
            
            if(null == idS || null == typeS)
            {
                CommLog.error("高级公式配置错误 - CS_CONSORT_RES_COUNT str:{}.", _reader.getSrcString());
                return null;
            }
            
            variableObj._m_lConsortId = Long.valueOf(idS);
            variableObj._m_eConsortResType = EBagItemUse_ConsortDrawShowType.valueOf(typeS.toUpperCase());

            return variableObj;
        } 
        catch (Exception e)
        {
        	CommLog.error("", e);
            return null;
        }
    }
}

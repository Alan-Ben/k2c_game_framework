package NPGameRes.GameObjs.PlayerVariable.PlayerVariableObj;

import NPCommon.CommonObj.NPStringReader;
import NPCommon.Log.CommLog;
import NPEnum.ENPPlayerVariableType;
import NPGameRes.GameObjs.PlayerVariable._ANPBasicPlayerVariableObj;

public class PlayerVariable_CS_HAS_BUILDING extends _ANPBasicPlayerVariableObj
{
    private long _m_lBuildingId;

    public long buildingId()
    {
        return _m_lBuildingId;
    }

    /******************
     * 获取条件类型
     */
    @Override
    public ENPPlayerVariableType variableType()
    {
        return ENPPlayerVariableType.CS_HAS_BUILDING;
    }

    public static PlayerVariable_CS_HAS_BUILDING readVariable(NPStringReader _reader)
    {
        PlayerVariable_CS_HAS_BUILDING variableObj = new PlayerVariable_CS_HAS_BUILDING();

        //解析字符串
        String rawBuildingId = _reader.readItem();
        //逐个判断
        if (null == rawBuildingId)
        {
            CommLog.error("CS_HAS_BUILDING read raw data error, str:{}", _reader.getSrcString());
            return null;
        }

        try
        {
            //解析建筑id
            variableObj._m_lBuildingId = Long.parseLong(rawBuildingId);
            return variableObj;
        } catch (Exception e)
        {
            CommLog.error("CS_HAS_BUILDING parse data error, str:{}", _reader.getSrcString());
            return null;
        }
    }
}

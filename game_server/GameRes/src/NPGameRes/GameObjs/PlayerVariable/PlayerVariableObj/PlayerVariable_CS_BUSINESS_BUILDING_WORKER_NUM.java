package NPGameRes.GameObjs.PlayerVariable.PlayerVariableObj;

import NPCommon.CommonObj.NPStringReader;
import NPCommon.Log.CommLog;
import NPEnum.ENPPlayerVariableType;
import NPGameRes.GameObjs.PlayerVariable._ANPBasicPlayerVariableObj;

public class PlayerVariable_CS_BUSINESS_BUILDING_WORKER_NUM extends _ANPBasicPlayerVariableObj
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
        return ENPPlayerVariableType.CS_BUSINESS_BUILDING_WORKER_NUM;
    }

    public static PlayerVariable_CS_BUSINESS_BUILDING_WORKER_NUM readVariable(NPStringReader _reader)
    {
        PlayerVariable_CS_BUSINESS_BUILDING_WORKER_NUM variableObj = new PlayerVariable_CS_BUSINESS_BUILDING_WORKER_NUM();

        //解析字符串
        String rawBuildingId = _reader.readItem();
        //逐个判断
        if (null == rawBuildingId)
        {
            return variableObj;
        }else
        {
            try
            {
                //解析建筑id
                variableObj._m_lBuildingId = Long.parseLong(rawBuildingId);
                return variableObj;
            } catch (Exception e)
            {
                CommLog.error("CS_BUSINESS_BUILDING_WORKER_NUM parse data error, str:{}", _reader.getSrcString());
                return null;
            }
        }
    }
}

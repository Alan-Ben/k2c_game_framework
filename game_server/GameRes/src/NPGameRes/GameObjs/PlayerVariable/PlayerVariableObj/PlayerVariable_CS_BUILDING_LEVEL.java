package NPGameRes.GameObjs.PlayerVariable.PlayerVariableObj;

import Common.BuildingEnum.EBuildingFuncEnum;
import NPCommon.CommonObj.NPStringReader;
import NPCommon.Log.CommLog;
import NPEnum.ENPPlayerVariableType;
import NPGameRes.GameObjs.PlayerVariable._ANPBasicPlayerVariableObj;

public class PlayerVariable_CS_BUILDING_LEVEL extends _ANPBasicPlayerVariableObj
{
    private EBuildingFuncEnum _m_buildingFuncType;
    private long _m_buildingId;

    public EBuildingFuncEnum buildingFuncType()
    {
        return _m_buildingFuncType;
    }

    public long buildingId()
    {
        return _m_buildingId;
    }

    /******************
     * 获取条件类型
     */
    @Override
    public ENPPlayerVariableType variableType()
    {
        return ENPPlayerVariableType.CS_BUILDING_LEVEL;
    }

    public static PlayerVariable_CS_BUILDING_LEVEL readVariable(NPStringReader _reader)
    {
        PlayerVariable_CS_BUILDING_LEVEL variableObj = new PlayerVariable_CS_BUILDING_LEVEL();

        //解析字符串
        String rawBuildingFuncType = _reader.readItem('@');
        String rawBuildingId = _reader.readItem();
        //逐个判断
        if (null == rawBuildingFuncType || null == rawBuildingId)
        {
            CommLog.error("CS_BUSINESS_BUILDING_WORKER_NUM read data error, str:{}", _reader.getSrcString());
            return null;
        }

        try
        {
            //解析
            variableObj._m_buildingFuncType = EBuildingFuncEnum.valueOf(rawBuildingFuncType);
            variableObj._m_buildingId = Long.parseLong(rawBuildingId);
            return variableObj;
        } catch (Exception e)
        {
            CommLog.error("CS_BUSINESS_BUILDING_WORKER_NUM parse data error, str:{}", _reader.getSrcString());
            return null;
        }
    }
}

package NPGameRes.GameObjs.CommonObj.BuildingCondition;

import Common.ConditionEnum.EBuildingConditionType;
import NPCommon.Log.CommLog;
import NPCommon.RefData._IParseFromStringable;
import NPGameRes.GameObjs.CommonObj.Condition._ATNPBasicConditionGroupObj;

public class BuildingConditionGroupObj extends _ATNPBasicConditionGroupObj<EBuildingConditionType, _ABasicBuildingCondition, BuildingConditionGroupObj> implements _IParseFromStringable
{
    /***************
     * 创建对应的条件集合对象
     * @param _str
     * @return
     */
    protected BuildingConditionGroupObj _createGroupObj()
    {
        return new BuildingConditionGroupObj();
    }

    /***************
     * 从字符串读取出对应的条件
     * @param _str
     * @return
     */
    @Override
    protected _ABasicBuildingCondition _readConditionStr(String _str)
    {
        return _ABasicBuildingCondition.readCondition(_str);
    }

    @Override
    public boolean parseFromString(String sValue)
    {
        try
        {
            readConditionGroupList(sValue, "");

            return true;
        } catch (Exception _e)
        {
            CommLog.error("can not parse HeroConditionGroupObj for str:{}", sValue, _e);
            return false;
        }
    }
}

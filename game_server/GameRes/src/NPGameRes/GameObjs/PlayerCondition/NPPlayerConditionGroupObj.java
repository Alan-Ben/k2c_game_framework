package NPGameRes.GameObjs.PlayerCondition;

import NPCommon.Log.CommLog;
import NPCommon.RefData._IParseFromStringable;
import NPEnum.ENPPlayerConditionType;
import NPGameRes.GameObjs.CommonObj.Condition._ATNPBasicConditionGroupObj;

public class NPPlayerConditionGroupObj extends _ATNPBasicConditionGroupObj<ENPPlayerConditionType, _ANPBasicPlayerCondition, NPPlayerConditionGroupObj> implements _IParseFromStringable
{
    /***************
     * 创建对应的条件集合对象
     * @param _str
     * @return
     */
    protected NPPlayerConditionGroupObj _createGroupObj()
    {
        return new NPPlayerConditionGroupObj();
    }

    /***************
     * 从字符串读取出对应的条件
     * @param _str
     * @return
     */
    @Override
    protected _ANPBasicPlayerCondition _readConditionStr(String _str)
    {
        return _ANPBasicPlayerCondition.readCondition(_str);
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
            CommLog.error("can not parse NPPlayerConditionGroupObj for str:{}", sValue, _e);
            return false;
        }
    }
}

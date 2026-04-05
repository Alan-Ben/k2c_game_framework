package NPGameRes.GameObjs.HeroCondition;

import Common.ConditionEnum.EHeroConditionType;
import NPCommon.Log.CommLog;
import NPCommon.RefData._IParseFromStringable;
import NPGameRes.GameObjs.CommonObj.Condition._ATNPBasicConditionGroupObj;

public class HeroConditionGroupObj extends _ATNPBasicConditionGroupObj<EHeroConditionType, _ABasicHeroCondition, HeroConditionGroupObj> implements _IParseFromStringable
{
    /***************
     * 创建对应的条件集合对象
     * @param _str
     * @return
     */
    protected HeroConditionGroupObj _createGroupObj()
    {
        return new HeroConditionGroupObj();
    }

    /***************
     * 从字符串读取出对应的条件
     * @param _str
     * @return
     */
    @Override
    protected _ABasicHeroCondition _readConditionStr(String _str)
    {
        return _ABasicHeroCondition.readCondition(_str);
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

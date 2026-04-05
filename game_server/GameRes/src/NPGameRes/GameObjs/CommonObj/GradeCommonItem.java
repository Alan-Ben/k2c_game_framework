package NPGameRes.GameObjs.CommonObj;

import NPCommon.CommonObj.NPCommonItem;
import NPCommon.RefData._IParseFromStringable;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPAttrGrade;

public class GradeCommonItem implements _IParseFromStringable
{
    private ENPAttrGrade _m_eAttrGrade = ENPAttrGrade.NONE;
    private NPCommonItem _m_ciCommonItem;

    public ENPAttrGrade attrGrade()
    {
        return _m_eAttrGrade;
    }

    public NPCommonItem commonItem()
    {
        return _m_ciCommonItem;
    }

    @Override
    public boolean parseFromString(String sValue)
    {
        try
        {
            String[] strs = CommonFunc.charSplit(sValue, ':', 2);

            //资质等级
            this._m_eAttrGrade = ENPAttrGrade.valueOf(strs[0].toUpperCase());
            //奖励物品
            this._m_ciCommonItem = new NPCommonItem();
            this._m_ciCommonItem.parseFromString(strs[1]);

            return true;
        } catch (Exception e)
        {
            return false;
        }
    }
}

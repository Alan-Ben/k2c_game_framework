package NPGameRes.GameObjs.CommonObj;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData._IParseFromStringable;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPAttrGrade;

public class GradeCommonCostItem implements _IParseFromStringable
{
    private ENPAttrGrade _m_eAttrGrade = ENPAttrGrade.NONE;
    private NPCommonCostItem _m_ciCostItem;

    public ENPAttrGrade attrGrade()
    {
        return _m_eAttrGrade;
    }

    public NPCommonCostItem commonCostItem()
    {
        return _m_ciCostItem;
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
            this._m_ciCostItem = new NPCommonCostItem();
            this._m_ciCostItem.parseFromString(strs[1]);

            return true;
        } catch (Exception e)
        {
            return false;
        }
    }
}

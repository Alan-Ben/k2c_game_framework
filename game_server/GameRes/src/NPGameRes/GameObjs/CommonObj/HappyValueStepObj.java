package NPGameRes.GameObjs.CommonObj;

import NPCommon.CommonObj.NPCommonCostItem;

public class HappyValueStepObj
{
    private int _m_iStep;
    private NPCommonCostItem _m_ciCommonCostItem;

    public HappyValueStepObj(int _step, NPCommonCostItem _costItem)
    {
        _m_iStep = _step;
        _m_ciCommonCostItem = _costItem;
    }

    public int getStep()
    {
        return _m_iStep;
    }

    public NPCommonCostItem getCommonCostItem()
    {
        return _m_ciCommonCostItem;
    }
}

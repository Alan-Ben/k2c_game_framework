package NPGameRes.Refs;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefListContainer;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;

@RefTable(tableName = "recruit")
public class RefRecruit extends RefBase
{
    private static RefListContainer<RefRecruit> _g_mgr = new RefListContainer<>();

    public static RefListContainer<RefRecruit> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefListContainer<RefRecruit> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefListContainer<RefRecruit>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefRecruit newRef = (RefRecruit) _newRef;
        id = newRef.id;
        cost_item = newRef.cost_item;
        gain_item = newRef.gain_item;
        condition = newRef.condition;
    }

    /**********
     * 获取对象数据Id，尽量唯一
     *
     * @author alzq.z
     * @time 2019年4月3日 下午11:35:24
     */
    @Override
    public long Id()
    {
        return id;
    }

    public long id;
    public NPCommonCostItem cost_item;//兑换消耗
    public NPCommonCostItem gain_item;//获得物品
    public NPPlayerConditionGroupObj condition;//条件

}

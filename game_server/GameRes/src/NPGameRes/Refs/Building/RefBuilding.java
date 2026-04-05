package NPGameRes.Refs.Building;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;


@RefTable(tableName = "building")
public class RefBuilding extends RefBase
{
    private static RefTableContainer<RefBuilding> _g_mgr = new RefTableContainer<RefBuilding>();
    public static RefTableContainer<RefBuilding> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefBuilding> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefBuilding>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefBuilding newRef = (RefBuilding) _newRef;
        id = newRef.id;
        build_cost = newRef.build_cost;
        build_condition = newRef.build_condition;
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

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long id;
    public NPCommonCostItem build_cost;//建造消耗
    public NPPlayerConditionGroupObj build_condition; //建造条件
}

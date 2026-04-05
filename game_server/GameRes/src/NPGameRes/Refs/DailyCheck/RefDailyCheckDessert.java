package NPGameRes.Refs.DailyCheck;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

@RefTable(tableName = "daily_check_dessert")
public class RefDailyCheckDessert extends RefBase
{
    private static RefTableContainer<RefDailyCheckDessert> _g_mgr = new RefTableContainer<>();

    public static RefTableContainer<RefDailyCheckDessert> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefDailyCheckDessert> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefDailyCheckDessert>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefDailyCheckDessert newRef = (RefDailyCheckDessert) _newRef;
        id = newRef.id;
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

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long id;//甜品id
}
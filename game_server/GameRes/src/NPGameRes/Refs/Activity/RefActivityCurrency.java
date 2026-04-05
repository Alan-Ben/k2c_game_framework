package NPGameRes.Refs.Activity;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

@RefTable(tableName = "activity_currency")
public class RefActivityCurrency extends RefBase
{
    private static RefActivityCurrencyMgr _g_mgr = new RefActivityCurrencyMgr();

    public static RefActivityCurrencyMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefActivityCurrencyMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefActivityCurrencyMgr) _mgr;
    }

    public static class RefActivityCurrencyMgr extends RefTableContainer<RefActivityCurrency>
    {
        @Override
        public void _onTableLoaded()
        {

        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefActivityCurrency newRef = (RefActivityCurrency) _newRef;
        id = newRef.id;
    }

    /**
     * 获取对象数据Id，尽量唯一
     */
    @Override
    public long Id()
    {
        return id;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public long id;
}
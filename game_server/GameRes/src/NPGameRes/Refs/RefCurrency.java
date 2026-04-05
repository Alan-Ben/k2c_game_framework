package NPGameRes.Refs;

import CommonEnum.ECurrency;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

@RefTable(tableName = "currency")
public class RefCurrency extends RefBase
{
    private static RefTableContainer<RefCurrency> _g_mgr = new RefTableContainer<RefCurrency>();

    public static RefTableContainer<RefCurrency> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefCurrency> getStaticContainer()
    {
        return _g_mgr;
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefCurrency>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefCurrency newRef = (RefCurrency) _newRef;
        type = newRef.type;
        is_spend_record = newRef.is_spend_record;
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
        return type.ordinal();
    }

    public ECurrency type;
    public boolean is_spend_record;//是否花费进入累计计数
}

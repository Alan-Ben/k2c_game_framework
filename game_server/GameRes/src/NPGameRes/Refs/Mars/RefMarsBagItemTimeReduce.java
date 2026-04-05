package NPGameRes.Refs.Mars;

import Common.MarsEnum.EMarsBagItemUseTimeType;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

@RefTable(tableName = "mars_bag_item_time_reduce")
public class RefMarsBagItemTimeReduce extends RefBase
{
    private static RefTableContainer<RefMarsBagItemTimeReduce> _g_mgr = new RefTableContainer<RefMarsBagItemTimeReduce>();

    public static RefTableContainer<RefMarsBagItemTimeReduce> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefMarsBagItemTimeReduce> getStaticContainer()
    {
        return _g_mgr;
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefMarsBagItemTimeReduce>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMarsBagItemTimeReduce newRef = (RefMarsBagItemTimeReduce) _newRef;
        id = newRef.id;
        time_type = newRef.time_type;
        reduce_sec = newRef.reduce_sec;
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
    public EMarsBagItemUseTimeType time_type = EMarsBagItemUseTimeType.NONE;
    public int reduce_sec;
}

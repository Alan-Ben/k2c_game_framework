package NPGameRes.Refs.Mars;

import Common.MarsEnum.EMarsBagItemUseTimeType;
import NPCommon.CommonObj.NPCommonItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

@RefTable(tableName = "mars_bag_item_time_type")
public class RefMarsBagItemTmeType extends RefBase
{
    private static RefTableContainer<RefMarsBagItemTmeType> _g_mgr = new RefTableContainer<RefMarsBagItemTmeType>();

    public static RefTableContainer<RefMarsBagItemTmeType> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefMarsBagItemTmeType> getStaticContainer()
    {
        return _g_mgr;
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefMarsBagItemTmeType>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMarsBagItemTmeType newRef = (RefMarsBagItemTmeType) _newRef;
        time_type = newRef.time_type;
        reduce_change_item = newRef.reduce_change_item;
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
        return time_type.ordinal();
    }

    public EMarsBagItemUseTimeType time_type = EMarsBagItemUseTimeType.NONE;
    public NPCommonItem reduce_change_item = new NPCommonItem();//火星加速道具溢出返还道具
}

package NPGameRes.Refs.Activity;

import CommonEnum.EActivityItemExpireTimeType;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.List;

@RefTable(tableName = "activity_bag_item")
public class RefActivityBagItem extends RefBase
{
    private static RefActivityBagItemMgr _g_mgr = new RefActivityBagItemMgr();

    public static RefActivityBagItemMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefActivityBagItemMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefActivityBagItemMgr) _mgr;
    }

    public static class RefActivityBagItemMgr extends RefTableContainer<RefActivityBagItem>
    {
        @Override
        public void _onTableLoaded()
        {

        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefActivityBagItem newRef = (RefActivityBagItem) _newRef;
        bag_item_id = newRef.bag_item_id;
        activity_id = newRef.activity_id;
        expire_type = newRef.expire_type;
        item_list = newRef.item_list;
    }

    /**
     * 获取对象数据Id，尽量唯一
     */
    @Override
    public long Id()
    {
        return bag_item_id;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public long bag_item_id;//背包道具id
    public long activity_id;//关联活动id
    public EActivityItemExpireTimeType expire_type;//过期类型
    public List<NPCommonCostItem> item_list;//过期后获得道具列表
}
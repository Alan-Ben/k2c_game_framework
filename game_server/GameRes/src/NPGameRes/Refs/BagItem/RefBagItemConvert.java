package NPGameRes.Refs.BagItem;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.CommonObj.NPCommonItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPItemType;

import java.util.ArrayList;
import java.util.List;

/*********************
 * 段位信息表
 *
 * @author spark
 *
 */
@RefTable(tableName = "item_convert")
public class RefBagItemConvert extends RefBase
{
    private static RefBagItemConvertMgr _g_mgr = new RefBagItemConvertMgr();

    public static RefBagItemConvertMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefBagItemConvertMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefBagItemConvertMgr) _mgr;
    }

    public static class RefBagItemConvertMgr extends RefTableContainer<RefBagItemConvert>
    {

    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefBagItemConvert newRef = (RefBagItemConvert) _newRef;
        bag_item_id = newRef.bag_item_id;
        ori_item_num = newRef.ori_item_num;
        cost_item_list = newRef.cost_item_list;
        target_item = newRef.target_item;
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
        return bag_item_id;
    }

    public long bag_item_id;
    public long ori_item_num;//原材料数量
    public ArrayList<NPCommonCostItem> cost_item_list = new ArrayList<>();//消耗列表
    public NPCommonItem target_item;//目标产物列表


    //////////////////////////////////////////////////////////////////////////

    /**
     * 计算总消耗(包括原道具)
     * @param _count 转换次数
     * @return
     */
    public List<NPCommonCostItem> calConvertTotalConsume(int _count)
    {
        List<NPCommonCostItem> items = new ArrayList<>();
        items.add(new NPCommonCostItem(ENPItemType.BAG_ITEM, bag_item_id, ori_item_num));
        for (NPCommonCostItem costItem : cost_item_list)
        {
            items.add(new NPCommonCostItem(costItem.getItemType(), costItem.getItemId(), costItem.getCount()));
        }

        return CommonFunc.itemMultiple(items, _count);
    }

    /**
     * 计算转换消耗(手续费)
     * @param _count 转换次数
     */
    public List<NPCommonCostItem> calConvertConsume(int _count)
    {
        List<NPCommonCostItem> items = new ArrayList<>();
        for (NPCommonCostItem costItem : cost_item_list)
        {
            items.add(new NPCommonCostItem(costItem.getItemType(), costItem.getItemId(), costItem.getCount()));
        }
        return CommonFunc.itemMultiple(items, _count);
    }
}

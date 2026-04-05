package NPGameRes.Refs.Shop;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;
import NPGameRes.GameObjs.PlayerVariable.NPPlayerVariableGroupObj;

import java.util.ArrayList;
import java.util.List;

/**
 * 商品表
 * @author mark
 */
@RefTable(tableName = "shop_item_group")
public class RefShopItemGroup extends RefBase
{
    private static RefShopItemGroupMgr _g_mgr = new RefShopItemGroupMgr();

    public static RefShopItemGroupMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefShopItemGroupMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefShopItemGroupMgr) _mgr;
    }

    public static class RefShopItemGroupMgr extends RefTableContainer<RefShopItemGroup>
    {
        @Override
        public void _onTableLoaded()
        {
        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefShopItemGroup newRef = (RefShopItemGroup) _newRef;
        group_id = newRef.group_id;
        enable_cond = newRef.enable_cond;
        ext_num = newRef.ext_num;
        discount_ext_num = newRef.discount_ext_num;
        buy_condition = newRef.buy_condition;
        pro_shop_item_id_list = newRef.pro_shop_item_id_list;
        pro_list = newRef.pro_list;
        shop_item_id_list = newRef.shop_item_id_list;
        wei_list = newRef.wei_list;
    }

    /**
     * 获取对象数据Id，尽量唯一
     */
    @Override
    public long Id()
    {
        return group_id;
    }

    //region 以下为配表字段

    public long group_id; //商品组
    public NPPlayerConditionGroupObj enable_cond; //商品组生效条件
    public NPPlayerVariableGroupObj ext_num;//商品额外数量(高级公式)
    public NPPlayerVariableGroupObj discount_ext_num;//打折商品额外数量（高级公式）
    public NPPlayerConditionGroupObj buy_condition;//可购买条件

    public List<Long> pro_shop_item_id_list = new ArrayList<>();//概率掉落-商品id列表
    public List<Long> pro_list = new ArrayList<>();//概率列表（万分比）,概率为空,则走权重计算 一个group中的概率是累加的，掉落失败则走权重掉落
    public List<Long> shop_item_id_list = new ArrayList<>();//权重掉落-商品id列表
    public List<Long> wei_list = new ArrayList<>(); //权重掉落-掉落权重列表

    //endregion
}

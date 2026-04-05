package NPGameRes.Refs.Dinner;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.Refs.RefPlayerFixedCd;

/**
 * @author mark
 */
@RefTable(tableName = "dinner_join_cost")
public class RefDinnerJoinCost extends RefBase
{
    private static RefDinnerCostMgr _g_mgr = new RefDinnerCostMgr();
    public static RefDinnerCostMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefDinnerCostMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefDinnerCostMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefDinnerJoinCost newRef = (RefDinnerJoinCost) _newRef;
        id = newRef.id;
        cost_item = newRef.cost_item;
        join_gain_score = newRef.join_gain_score;
        join_gain_coin = newRef.join_gain_coin;
        banquet_host_factor = newRef.banquet_host_factor;
        fixed_cd_id = newRef.fixed_cd_id;
    }
    
    public static class RefDinnerCostMgr extends RefTableContainer<RefDinnerJoinCost>
    {
    	@Override
    	protected void _onTableLoaded()
        {
        }
    }

    /**********
     * 获取对象数据Id，尽量唯一
     */
    @Override
    public long Id()
    {
        return id;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long id; //唯一id
    public NPCommonCostItem cost_item = new NPCommonCostItem();//消耗的物品CommonCostItem
    public int join_gain_score;//赴宴获得基础人气
    public int join_gain_coin;//赴宴获得基础宴会币
    public int banquet_host_factor;//开宴者宴会币结算系数
    public long fixed_cd_id;//每日使用最大次数
    
    @RefField(isIgnore = true)
    public RefPlayerFixedCd fixedCdRef;
}

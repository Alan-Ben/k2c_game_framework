package NPGameRes.Refs.Market;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;

/**
 * 香槟集市
 * @author mark
 */
@RefTable(tableName = "market_shop")
public class RefMarketShop extends RefBase
{
    private static RefMarketShopMgr _g_mgr = new RefMarketShopMgr();
    public static RefMarketShopMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefMarketShopMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefMarketShopMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMarketShop newRef = (RefMarketShop) _newRef;
        id = newRef.id;
        operate_reward_id = newRef.operate_reward_id;
        unlock_condition = newRef.unlock_condition;
    }
    
    public static class RefMarketShopMgr extends RefTableContainer<RefMarketShop>
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

    public long id;//唯一id
    public long operate_reward_id;//经营奖励reward_id
    public NPPlayerConditionGroupObj unlock_condition;//解锁条件
}

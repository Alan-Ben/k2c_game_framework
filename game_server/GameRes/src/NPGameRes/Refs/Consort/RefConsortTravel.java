package NPGameRes.Refs.Consort;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPCommon.Util.CommonFunc;

/**
 * @author mark
 */
@RefTable(tableName = "consort_travel")
public class RefConsortTravel extends RefBase
{
    private static RefConsortTravelMgr _g_mgr = new RefConsortTravelMgr();
    public static RefConsortTravelMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefConsortTravel> getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefConsortTravelMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefConsortTravel newRef = (RefConsortTravel) _newRef;
        id = newRef.id;
        add_bless = newRef.add_bless;
        bless_point_multiple = newRef.bless_point_multiple;
        travel_cost_item = newRef.travel_cost_item;
        time_price_id = newRef.time_price_id;
        giftde_probability = newRef.giftde_probability;
    }
    
    public static class RefConsortTravelMgr extends RefTableContainer<RefConsortTravel>
    {
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

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long id;
    public int add_bless;//add_bless
    public int bless_point_multiple;//加护点翻倍倍数
    public NPCommonCostItem travel_cost_item;//旅行消耗，目前字段已移除，但服务端先保留，原代码逻辑不变
    public int time_price_id;//旅行消耗的time_price
    public int giftde_probability;//出现卷王的概率（万分比）
    
    /**
     * 计算概率：是否可以获得卷王子嗣
     * @return
     */
    public boolean isGiftde()
    {
    	if(giftde_probability <= 0)
    		return false;
    	
    	int rnd = CommonFunc.randomInt(10000);
    	return rnd <= giftde_probability;
    }
}

package NPGameRes.Refs.Market;

import NPCommon.Log.CommLog;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.Market.MarketRewardCritWeiObj;
import NPGameRes.GameObjs.Market.MarketRewardObj;

import java.util.ArrayList;
import java.util.List;

/**
 * 香槟集市等级
 * @author mark
 */
@RefTable(tableName = "market_shop_lvl")
public class RefMarketShopLvl extends RefBase
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
        RefMarketShopLvl newRef = (RefMarketShopLvl) _newRef;
        lvl = newRef.lvl;
        up_cost = newRef.up_cost;
        crit_list = newRef.crit_list;
        crit_wei_list = newRef.crit_wei_list;
        big_crit = newRef.big_crit;
        big_crit_wei = newRef.big_crit_wei;
    }
    
    public static class RefMarketShopMgr extends RefTableContainer<RefMarketShopLvl>
    {
    	@Override
    	protected void _onTableLoaded()
        {
            List<RefMarketShopLvl> refList = getList();
            for(int i = 0; i < refList.size(); i++)
    		{
    			RefMarketShopLvl ref = refList.get(i);
    			if(null == ref)
    				continue;

    			MarketRewardCritWeiObj rewardWei = new MarketRewardCritWeiObj();
    			
    			//构造升级倍率
    			if(ref.crit_list.size() != ref.crit_wei_list.size())
    			{
    				CommLog.error("Table[market_shop_lvl] error, lvl:{} crit_list != crit_wei_list", ref.lvl);
    			}
    			else
    			{
    				for(int j = 0; j < ref.crit_list.size(); j++)
        			{
            			MarketRewardObj weiObj = new MarketRewardObj();
            			weiObj.multiple = ref.crit_list.get(j);
            			weiObj.weight = ref.crit_wei_list.get(j);
            			weiObj.isCrit = false;
            			
            			rewardWei.addWeiObj(weiObj);
        			}
    			}
    			
    			//构造暴击倍率
    			MarketRewardObj weiObj = new MarketRewardObj();
    			weiObj.multiple = ref.big_crit;
    			weiObj.weight = ref.big_crit_wei;
    			weiObj.isCrit = true;
    			
    			rewardWei.addWeiObj(weiObj);
    			
    			//更新配表数据
    			ref.rewardWei = rewardWei;
    		}
        }
    }

    /**********
     * 获取对象数据Id，尽量唯一
     */
    @Override
    public long Id()
    {
        return lvl;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long lvl;//唯一id
    public int up_cost;//升级消耗
    public ArrayList<Integer> crit_list = new ArrayList<>();//暴击倍数列表
    public ArrayList<Integer> crit_wei_list = new ArrayList<>();//暴击权重列表
    public int big_crit;//大暴击倍数
    public int big_crit_wei;//大暴击权重
    
    @RefField(isIgnore =  true)
    public MarketRewardCritWeiObj rewardWei = new MarketRewardCritWeiObj();
}

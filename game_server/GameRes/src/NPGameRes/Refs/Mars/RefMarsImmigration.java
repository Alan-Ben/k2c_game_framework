package NPGameRes.Refs.Mars;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

@RefTable(tableName = "mars_immigration")
public class RefMarsImmigration extends RefBase
{
    private static RefMarsImmigrationMgr _g_mgr = new RefMarsImmigrationMgr();

    public static RefMarsImmigrationMgr getMgr()
    {
        return _g_mgr;
    }
    
    @Override
    public RefMarsImmigrationMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefMarsImmigrationMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMarsImmigration newRef = (RefMarsImmigration) _newRef;
        times = newRef.times;
        cost_item = newRef.cost_item;
        reward_id = newRef.reward_id;
        duration = newRef.duration;
    }

    @Override
    public long Id()
    {
        return times;
    }

    public static class RefMarsImmigrationMgr extends RefTableContainer<RefMarsImmigration>
    {
        // 配表加载完成后的处理逻辑
        @Override
        protected void _onTableLoaded()
        {
        }
        
        /**
         * 根据次数查找移民配置
         * @param _times
         * @return
         */
        public RefMarsImmigration getRefByTimes(int _times)
        {
        	java.util.List<RefMarsImmigration> refList = getList();
        	if(null == refList || refList.isEmpty())
        		return null;
        	
        	//超过最大次数，直接选择最大次数
        	RefMarsImmigration maxRef = refList.get(refList.size() - 1);
        	if(_times >= maxRef.times)
        		return maxRef;
        	
        	//选择对应次数
        	return get(_times);
        }
    }

    public long times;//次数
    public NPCommonCostItem cost_item;//消耗
    public long reward_id;//奖励id
    public int duration;//移民时长（秒）
}
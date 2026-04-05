package NPGameRes.Refs.Mars;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.Parse.NPPlayerEffectListParse;

import java.util.ArrayList;

@RefTable(tableName = "mars_event")
public class RefMarsEvent extends RefBase
{
    private static RefMarsEventMgr _g_mgr = new RefMarsEventMgr();

    public static RefMarsEventMgr getMgr()
    {
        return _g_mgr;
    }
    
    @Override
    public RefMarsEventMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefMarsEventMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMarsEvent newRef = (RefMarsEvent) _newRef;
        id = newRef.id;
        daily_limit = newRef.daily_limit;
        trigger_wei = newRef.trigger_wei;
        trigger_rand_group_id = newRef.trigger_rand_group_id;
        trgger_effect = newRef.trgger_effect;
    }

    @Override
    public long Id()
    {
        return id;
    }

    public static class RefMarsEventMgr extends RefTableContainer<RefMarsEvent>
    {
        // 配表加载完成后的处理逻辑
        @Override
        protected void _onTableLoaded()
        {
        }
    }

    public long id;
    public int daily_limit;//每日上限
    public int trigger_wei;//触发权重
    public int trigger_rand_group_id;//触发概率组id
    public NPPlayerEffectListParse trgger_effect;//事件效果
    
    @RefField(isIgnore = true)
    public ArrayList<RefMarsEventTriggerPer> triggerPerRefList = new ArrayList<>();
    
    /**
     * 获取对应的概率
     * @param _healthIndex 健康指数
     * @param _happyIndex 幸福指数
     * @return
     */
    public int getPerByValue(long _healthIndex, long _happyIndex)
    {
    	ArrayList<RefMarsEventTriggerPer> list = this.triggerPerRefList;
    	for(int i = 0; i < list.size(); i++)
    	{
    		RefMarsEventTriggerPer ref = list.get(i);
    		if(null == ref)
    			continue;
    		
    		long value = ref.getValue(_healthIndex, _happyIndex);
    		if(CommonFunc.inRange(value, ref.value_range.first(), ref.value_range.second()))
    			return ref.per;
    	}
    	
    	return 0;
    }
}
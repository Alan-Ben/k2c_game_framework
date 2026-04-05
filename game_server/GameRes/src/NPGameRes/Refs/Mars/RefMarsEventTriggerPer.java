package NPGameRes.Refs.Mars;

import Common.MarsEnum.EMarsPeopleValueType;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPCommon.Util.Pair.WCGPairInt;

import java.util.ArrayList;

@RefTable(tableName = "mars_event_trigger_per")
public class RefMarsEventTriggerPer extends RefBase
{
    private static RefMarsEventTriggerPerMgr _g_mgr = new RefMarsEventTriggerPerMgr();

    public static RefMarsEventTriggerPerMgr getMgr()
    {
        return _g_mgr;
    }
    
    @Override
    public RefMarsEventTriggerPerMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefMarsEventTriggerPerMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMarsEventTriggerPer newRef = (RefMarsEventTriggerPer) _newRef;
        id = newRef.id;
        group_id = newRef.group_id;
        value_type_list = newRef.value_type_list;
        value_range = newRef.value_range;
        per = newRef.per;
    }

    @Override
    public long Id()
    {
        return id;
    }

    public static class RefMarsEventTriggerPerMgr extends RefTableContainer<RefMarsEventTriggerPer>
    {
        // 配表加载完成后的处理逻辑
        @Override
        protected void _onTableLoaded()
        {
        }
    }

    public long id;
    public int group_id;//组 ID
    public ArrayList<EMarsPeopleValueType> value_type_list = new ArrayList<>();//指数类型列表（最低值）
    public WCGPairInt value_range = new WCGPairInt();//数值区间
    public int per;//概率（万分比）
    
    /**
     * 获取对应的数值
     * @param _health
     * @param _happy
     * @return
     */
    public long getValue(long _health, long _happy)
    {
    	ArrayList<EMarsPeopleValueType> list = this.value_type_list;
    	
    	long value = 0;
    	for(int i = 0; i < list.size(); i++)
    	{
    		EMarsPeopleValueType type = list.get(i);
    		if(null == type)
    			continue;
    		
    		if(EMarsPeopleValueType.HEALTH == type)
    		{
    			if(0 == value || _health < value)
    				value = _health;
    		}
    		else if(EMarsPeopleValueType.HAPPY == type)
    		{
    			if(0 == value || _happy < value)
    				value = _happy;
    		}
    	}
    	
    	return value;
    }
}
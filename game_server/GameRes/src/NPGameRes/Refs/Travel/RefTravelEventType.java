package NPGameRes.Refs.Travel;

import Common.TravelEnum.ETravelEventType;
import NPCommon.Game.WeightValueList;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.ArrayList;

/**
 * @author mark
 */
@RefTable(tableName = "travel_event_type")
public class RefTravelEventType extends RefBase
{
    private static RefTravelEventTypeMgr _g_mgr = new RefTravelEventTypeMgr();
    public static RefTravelEventTypeMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTravelEventTypeMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTravelEventTypeMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefTravelEventType newRef = (RefTravelEventType) _newRef;
        event_type = newRef.event_type;
        rand_wei = newRef.rand_wei;
    }
    
    public static class RefTravelEventTypeMgr extends RefTableContainer<RefTravelEventType>
    {
    	//构造事件类型权重池
        private WeightValueList<RefTravelEventType> _m_refWeight = new WeightValueList<>();
        
    	@Override
    	protected void _onTableLoaded()
        {
    		WeightValueList<RefTravelEventType> refWeight = new WeightValueList<>();
    		for(int i = 0; i < getList().size(); i++)
        	{
        		RefTravelEventType ref = RefTravelEventType.getMgr().getList().get(i);
        		if(null == ref)
        			continue;
        		
        		refWeight.add(ref, ref.rand_wei);
        	}
    		_m_refWeight = refWeight;
        }
    	
    	/**
    	 * 获取随机事件类型
    	 * @return
    	 */
    	public RefTravelEventType randEventType()
    	{
    		return _m_refWeight.random();
    	}
    }

    /**********
     * 获取对象数据Id，尽量唯一
     */
    @Override
    public long Id()
    {
        return event_type.ordinal();
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public ETravelEventType event_type = ETravelEventType.NONE;//唯一id
    public int rand_wei;//事件权重
    
    @RefField(isIgnore = true)
    public ArrayList<RefTravelEvent> eventRefList = new ArrayList<>();
}

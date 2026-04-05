package NPGameRes.Refs.Dinner;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPCommon.Util.Pair.WCGPairInt;
import NPCommon.Util.Pair.WCGPairIntList;

import java.util.ArrayList;
import java.util.List;

/**
 * @author mark
 */
@RefTable(tableName = "dinner_type")
public class RefDinnerType extends RefBase
{
    private static RefDinnerTypeMgr _g_mgr = new RefDinnerTypeMgr();
    public static RefDinnerTypeMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefDinnerTypeMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefDinnerTypeMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefDinnerType newRef = (RefDinnerType) _newRef;
        dinner_id = newRef.dinner_id;
        sort_id = newRef.sort_id;
        open_cost = newRef.open_cost;
        is_permit_open = newRef.is_permit_open;
        default_seat_num = newRef.default_seat_num;
        duration_sec = newRef.duration_sec;
        time_people_list = newRef.time_people_list;
        name = newRef.name;
    }

    public static class RefDinnerTypeMgr extends RefTableContainer<RefDinnerType>
    {
    	@Override
    	protected void _onTableLoaded()
        {
        }
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
        return dinner_id;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long dinner_id;//宴会id
    public int sort_id;//排序（小->大）
	public List<NPCommonCostItem> open_cost = new ArrayList<>();//开宴会消耗道具
	public boolean is_permit_open;//是否凭证开启
    public int default_seat_num = 4;//默认席位数量
    public int duration_sec = 18000;//持续时长，秒
    public WCGPairIntList time_people_list = new WCGPairIntList();//宴会人数需求列表(秒:人数)
    public String name = "";//宴会名
    
    /**
     * 通过时间获取宴会此时赴宴人数
     * @param _secs
     * @return
     */
    public int getDinnerJoinerCount(int _secs)
    {
    	for(int i = time_people_list.getList().size() - 1; i >= 0; i--)
    	{
    		WCGPairInt obj = time_people_list.getList().get(i);
    		if(null == obj)
    			continue;
    		
    		if(_secs >= obj.first())
    			return obj.second();
    	}
    	
    	return 0;
    }
}

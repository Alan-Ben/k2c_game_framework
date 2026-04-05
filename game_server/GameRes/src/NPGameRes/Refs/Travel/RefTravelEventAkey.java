package NPGameRes.Refs.Travel;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.Refs.Parse.NPPlayerEffectListParse;

import java.util.ArrayList;

/**
 * @author mark
 */
@RefTable(tableName = "travel_event_akey")
public class RefTravelEventAkey extends RefBase
{
    private static RefTravelEventAkeyMgr _g_mgr = new RefTravelEventAkeyMgr();
    public static RefTravelEventAkeyMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTravelEventAkeyMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTravelEventAkeyMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefTravelEventAkey newRef = (RefTravelEventAkey) _newRef;
        event_id = newRef.event_id;
        event_item_list = newRef.event_item_list;
        event_effect = newRef.event_effect;
        gain_player_exp = newRef.gain_player_exp;
    }
    
    public static class RefTravelEventAkeyMgr extends RefTableContainer<RefTravelEventAkey>
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
        return event_id;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long event_id;//事件id
    public ArrayList<NPCommonCostItem> event_item_list = new ArrayList<>();
    public NPPlayerEffectListParse event_effect;//事件处理效果
    public long gain_player_exp;//获得玩家经验
}

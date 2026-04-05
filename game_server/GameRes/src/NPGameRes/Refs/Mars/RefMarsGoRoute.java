package NPGameRes.Refs.Mars;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.CommonObj.LevelObj._ILevelBasicObj;

import java.util.ArrayList;

@RefTable(tableName = "mars_go_route")
public class RefMarsGoRoute extends RefBase implements _ILevelBasicObj
{
    private static RefMarsGoRouteMgr _g_mgr = new RefMarsGoRouteMgr();

    public static RefMarsGoRouteMgr getMgr()
    {
        return _g_mgr;
    }
    
    @Override
    public RefMarsGoRouteMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefMarsGoRouteMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMarsGoRoute newRef = (RefMarsGoRoute) _newRef;
        id = newRef.id;
        stage_id = newRef.stage_id;
        continue_secs = newRef.continue_secs;
        allow_advance_secs = newRef.allow_advance_secs;
        arrive_item_list = newRef.arrive_item_list;
        done_simple_unlock_id = newRef.done_simple_unlock_id;
    }

    @Override
    public long Id()
    {
        return id;
    }

    public static class RefMarsGoRouteMgr extends RefTableContainer<RefMarsGoRoute>
    {
        // 配表加载完成后的处理逻辑
        @Override
        protected void _onTableLoaded()
        {
        }
    }

	@Override
	public int getLevel() 
	{
		return stage_id;
	}

    public long id;
    public int stage_id;//节点id
    public int continue_secs;//持续时间（秒）
    public int allow_advance_secs;//允许提前累积的秒数
    public ArrayList<NPCommonCostItem> arrive_item_list = new ArrayList<>();//到达奖励物品列表
    
    //GOB-7183【优化-0】火星基地mars_go_route表新增可以完成这一步的condition
    //https://www.teambition.com/task/692813b1f6a81b6e602c5dd9
    public long done_simple_unlock_id;//完成条件

}
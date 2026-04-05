package NPGameRes.Refs.Mars;

import NPCommon.CommonObj.NPCommonItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

@RefTable(tableName = "mars_explore_mine")
public class RefMarsExploreMine extends RefBase
{
    private static RefMarsExploreMineMgr _g_mgr = new RefMarsExploreMineMgr();

    public static RefMarsExploreMineMgr getMgr()
    {
        return _g_mgr;
    }
    
    @Override
    public RefMarsExploreMineMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefMarsExploreMineMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMarsExploreMine newRef = (RefMarsExploreMine) _newRef;
        id = newRef.id;
        mine_lvl = newRef.mine_lvl;
        res_type = newRef.res_type;
        res_type_refresh_wei = newRef.res_type_refresh_wei;
        res_num = newRef.res_num;
        mars_mine_collects_speed = newRef.mars_mine_collects_speed;
    }

    @Override
    public long Id()
    {
        return id;
    }

    public static class RefMarsExploreMineMgr extends RefTableContainer<RefMarsExploreMine>
    {
        // 配表加载完成后的处理逻辑
        @Override
        protected void _onTableLoaded()
        {
        }
    }

    public long id;//唯一 id
    public int mine_lvl;//矿场等级
    public NPCommonItem res_type = new NPCommonItem();
    public int res_type_refresh_wei;//类型刷新权重
    public long res_num;//资源总量
    public int mars_mine_collects_speed;//资源对应基础采集速度(秒)
}
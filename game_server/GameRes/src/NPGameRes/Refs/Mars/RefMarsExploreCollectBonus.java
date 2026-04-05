package NPGameRes.Refs.Mars;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.List;

@RefTable(tableName = "mars_explore_collect_bonus")
public class RefMarsExploreCollectBonus extends RefBase
{
    private static RefMarsExploreCollectBonusMgr _g_mgr = new RefMarsExploreCollectBonusMgr();

    public static RefMarsExploreCollectBonusMgr getMgr()
    {
        return _g_mgr;
    }
    
    @Override
    public RefMarsExploreCollectBonusMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefMarsExploreCollectBonusMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMarsExploreCollectBonus newRef = (RefMarsExploreCollectBonus) _newRef;
        id = newRef.id;
        need_team_power = newRef.need_team_power;
        add_collect_speed = newRef.add_collect_speed;
    }

    @Override
    public long Id()
    {
        return id;
    }

    public static class RefMarsExploreCollectBonusMgr extends RefTableContainer<RefMarsExploreCollectBonus>
    {
        // 配表加载完成后的处理逻辑
        @Override
        protected void _onTableLoaded()
        {
        }

        /**
         * 获取指定的加成万分比
         * @return
         */
        public int getAddPer(long _power)
        {
            int addPer = 0;

        	List<RefMarsExploreCollectBonus> refList = getList();
            for (RefMarsExploreCollectBonus ref : refList)
            {
                if (ref.need_team_power >= _power)
                    break;

                addPer = ref.add_collect_speed;
            }

            return addPer;
        }
    }

    public long id;//唯一ID
    public long need_team_power;//加成所需队伍最小实力	add_collect_speed
    public int add_collect_speed;//采集速度加成万分比
}
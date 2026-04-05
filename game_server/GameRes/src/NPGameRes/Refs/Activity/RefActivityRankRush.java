package NPGameRes.Refs.Activity;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

@RefTable(tableName = "activity_rank_rush")
public class RefActivityRankRush extends RefBase
{
    private static RefActivityRankRushMgr _g_mgr = new RefActivityRankRushMgr();

    public static RefActivityRankRushMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefActivityRankRushMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefActivityRankRushMgr) _mgr;
    }

    public static class RefActivityRankRushMgr extends RefTableContainer<RefActivityRankRush>
    {
        @Override
        public void _onTableLoaded()
        {

        }

        /**
         * 通过rankId获取配置
         * @param _rankId
         * @return
         */
        public RefActivityRankRush getByRankId(long _rankId)
        {
            for(RefActivityRankRush ref : this.getList())
            {
                if(ref.rank_id == _rankId)
                {
                    return ref;
                }
            }
            return null;
        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefActivityRankRush newRef = (RefActivityRankRush) _newRef;
        id = newRef.id;
        rank_id = newRef.rank_id;
        share_box = newRef.share_box;
    }

    /**
     * 获取对象数据Id，尽量唯一
     */
    @Override
    public long Id()
    {
        return id;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public long id;
    public long rank_id;//排行榜id
    public long share_box;//通用宝箱配表id
}
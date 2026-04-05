package NPGameRes.Refs.Rank;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.List;

@RefTable(tableName = "rank_fixed")
public class RefRankFixed extends RefBase
{
    private static RefTableContainer<RefRankFixed> _g_mgr = new RefTableContainer<>();

    public static RefTableContainer<RefRankFixed> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefRankFixed> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefRankFixed>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefRankFixed newRef = (RefRankFixed) _newRef;
        id = newRef.id;
        rank_id = newRef.rank_id;
        like_fixed_cd_id = newRef.like_fixed_cd_id;
        cross_like_fixed_cd_id = newRef.cross_like_fixed_cd_id;
        like_reward = newRef.like_reward;
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

    public long id; //常驻排行榜id
    public long rank_id; //排行榜id
    public long like_fixed_cd_id; //点赞消耗固定id
    public long cross_like_fixed_cd_id; //跨服点赞消耗固定id
    public List<NPCommonCostItem> like_reward; //点赞奖励

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
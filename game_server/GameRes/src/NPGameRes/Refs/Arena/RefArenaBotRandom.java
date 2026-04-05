package NPGameRes.Refs.Arena;

import NPCommon.Game.WeightLongValueList;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

@RefTable(tableName = "arena_bot_random")
public class RefArenaBotRandom extends RefBase
{
    private static RefArenaBotRandomMgr _g_mgr = new RefArenaBotRandomMgr();

    public static RefArenaBotRandomMgr getMgr()
    {
        return _g_mgr;
    }

    public static class RefArenaBotRandomMgr extends RefTableContainer<RefArenaBotRandom>
    {
        /**
         * 根据排名获取配置
         * @param _rank
         * @return
         */
        public RefArenaBotRandom getRefByRank(int _rank)
        {
            for (RefArenaBotRandom ref : getList())
            {
                if (null == ref)
                    continue;

                if (ref.rank_begin <= _rank && ref.rank_end >= _rank)
                    return ref;
            }

            return null;
        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefArenaBotRandom newRef = (RefArenaBotRandom) _newRef;
        id = newRef.id;
        rank_begin = newRef.rank_begin;
        rank_end = newRef.rank_end;
        weight = newRef.weight;
        template_weight_list = newRef.template_weight_list;
    }

    @Override
    public RefContainerBase<? extends RefBase> getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefArenaBotRandomMgr) _mgr;
    }

    @Override
    public long Id()
    {
        return id;
    }

    public long id;
    public int rank_begin;//排名起始
    public int rank_end;//排名结束
    public int weight;//匹配概率
    public WeightLongValueList template_weight_list;//机器人模板id权重
}

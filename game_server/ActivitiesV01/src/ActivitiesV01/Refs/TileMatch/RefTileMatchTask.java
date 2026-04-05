package ActivitiesV01.Refs.TileMatch;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.List;

@RefTable(tableName = "tilematch_task")
public class RefTileMatchTask extends RefBase
{
    private static RefTileMatchTaskMgr _g_mgr = new RefTileMatchTaskMgr();

    public static RefTileMatchTaskMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTileMatchTaskMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTileMatchTaskMgr) _mgr;
    }

    public static class RefTileMatchTaskMgr extends RefTableContainer<RefTileMatchTask>
    {
        @Override
        public void _onTableLoaded()
        {

        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefTileMatchTask newRef = (RefTileMatchTask) _newRef;
        id = newRef.id;
        npc_rand_list = newRef.npc_rand_list;
        gain_score = newRef.gain_score;
        step_limit = newRef.step_limit;
        chess_pieces_num = newRef.chess_pieces_num;
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
    public long id;//任务id
    public List<Long> npc_rand_list;//NPC形象随机列表
    public long gain_score;//获得的积分
    public int step_limit;//步数限制
    public List<Integer> chess_pieces_num;//任务要求(棋子数量)
}
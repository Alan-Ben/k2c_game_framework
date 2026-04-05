package NPGameRes.Refs.Anecdote;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

@RefTable(tableName = "anecdote_pos")
public class RefAnecdotePos extends RefBase
{
    private static RefAnecdotePosMgr _g_mgr = new RefAnecdotePosMgr();

    public static RefAnecdotePosMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefAnecdotePosMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefAnecdotePosMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefAnecdotePos newRef = (RefAnecdotePos) _newRef;
        id = newRef.id;
        refresh_group = newRef.refresh_group;
        event_id_list = newRef.event_id_list;
    }

    public static class RefAnecdotePosMgr extends RefTableContainer<RefAnecdotePos>
    {
        private Map<Integer, List<RefAnecdotePos>> _m_groupPosMap = new HashMap<>();

        @Override
        protected void _onTableLoaded()
        {
            Map<Integer, List<RefAnecdotePos>> map = new HashMap<>();
            for (RefAnecdotePos refAnecdotePos : getList())
            {
                map.computeIfAbsent(refAnecdotePos.refresh_group, k -> new ArrayList<>()).add(refAnecdotePos);
            }
            _m_groupPosMap = map;
        }

        /**
         * 获取刷新组的位置列表
         * @param _groupId
         * @return
         */
        public List<RefAnecdotePos> getPosListByGroupId(int _groupId)
        {
            return _m_groupPosMap.get(_groupId);
        }
    }

    /**********
     * 获取对象数据Id，尽量唯一
     */
    @Override
    public long Id()
    {
        return id;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long id;//唯一id
    public int refresh_group;//刷新组id
    public List<Long> event_id_list;//可刷新的事件列表
}

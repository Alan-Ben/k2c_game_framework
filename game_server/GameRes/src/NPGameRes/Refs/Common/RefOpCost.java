package NPGameRes.Refs.Common;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.CommonObj.LevelObj._ILevelBasicObj;
import NPGameRes.GameObjs.CommonObj.LevelObj._TLevelAreaMgr;

import java.util.HashMap;
import java.util.Map;

@RefTable(tableName = "op_cost")
public class RefOpCost extends RefBase implements _ILevelBasicObj
{
    private static RefOpCostMgr _g_mgr = new RefOpCostMgr();

    public static RefOpCostMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefOpCostMgr getStaticContainer()
    {
        return _g_mgr;
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefOpCostMgr) _mgr;
    }

    public static class RefOpCostMgr extends RefTableContainer<RefOpCost>
    {
        private Map<Integer, _TLevelAreaMgr<RefOpCost>> _m_map = new HashMap<>();

    	@Override
        public void _onTableLoaded()
        {
            Map<Integer, _TLevelAreaMgr<RefOpCost>> map = new HashMap<>();

            getList().forEach(refOpCost -> {
                map.computeIfAbsent(refOpCost.cost_group_id, k -> new _TLevelAreaMgr<>())._initAddLevelData(refOpCost);
            });

            _m_map = map;
        }

        /**
         * 获取等级map
         * @param _groupId
         * @return
         */
        public _TLevelAreaMgr<RefOpCost> getLevelMap(int _groupId)
        {
            return _m_map.get(_groupId);
        }

        /**
         * 获取消耗配置
         * @param _groupId
         * @param _count
         * @return
         */
        public RefOpCost getOpCostRef(int _groupId, int _count)
        {
            _TLevelAreaMgr<RefOpCost> levelMap = getLevelMap(_groupId);
            if (levelMap == null)
                return null;

            return levelMap.getLevelData(_count);
        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefOpCost newRef = (RefOpCost) _newRef;
        id = newRef.id;
        cost_group_id = newRef.cost_group_id;
        op_count = newRef.op_count;
        cost_item = newRef.cost_item;
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
        return id;
    }

    @Override
    public int getLevel()
    {
    	return op_count;
    }

    public long id;
    public int cost_group_id;//消耗组
    public int op_count; //操作次数
    public NPCommonCostItem cost_item;//消耗
}

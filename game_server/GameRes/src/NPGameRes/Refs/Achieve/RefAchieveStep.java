package NPGameRes.Refs.Achieve;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.Log.CommLog;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;

/**
 * @author mark 通用成就配置
 */
@RefTable(tableName = "achieve_step")
public class RefAchieveStep extends RefBase
{
    private static RefAchieveStepMgr _g_mgr = new RefAchieveStepMgr();

    public static RefAchieveStepMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefAchieveStepMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefAchieveStepMgr) _mgr;
    }

    public static class RefAchieveStepMgr extends RefTableContainer<RefAchieveStep>
    {
        private Map<Long, RefAchieveStep> _m_hmAchieveStepMap = new HashMap<>();

        @Override
        public void _onTableLoaded()
        {
            //整理成就步骤数据
            Map<Long, RefAchieveStep> map = new HashMap<>();
            for (RefAchieveStep ref : getList())
            {
                if (null == ref)
                    continue;

                long uniqueKey = getKey(ref.achieve_id, ref.step);
                if (map.containsKey(uniqueKey))
                    CommLog.warn("RefAchieveStepMgr _onTableLoaded sort map found duplicate key:{} refId:{}", uniqueKey, ref.id);

                map.put(uniqueKey, ref);
            }

            //最后直接替换原来的map, 保证线程安全
            _m_hmAchieveStepMap = map;
        }

        /**
         * 生成唯一key
         * @param _achieveId 成就id
         * @param _step      步骤
         * @return 唯一key
         */
        public static long getKey(long _achieveId, int _step)
        {
            return _achieveId * 10000L + _step;
        }

        /**
         * 通过成就步骤查找对应的配置数据
         * @param _achieveId 成就id
         * @param _step      步骤
         * @return 配置数据
         */
        public RefAchieveStep lookupByAchieveStep(long _achieveId, int _step)
        {
            return _m_hmAchieveStepMap.get(getKey(_achieveId, _step));
        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefAchieveStep newRef = (RefAchieveStep) _newRef;
        id = newRef.id;
        achieve_id = newRef.achieve_id;
        step = newRef.step;
        process_count = newRef.process_count;
        done_item_list = newRef.done_item_list;
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
    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long id;
    public long achieve_id; //成就ID
    public int step; //成就步骤
    public long process_count; //进度条计数目标值
    public ArrayList<NPCommonCostItem> done_item_list = new ArrayList<>(); //完成成就奖励物品列表

    // //////////////////////////////////////////////////////////////////////////////////////////////////////////////
}

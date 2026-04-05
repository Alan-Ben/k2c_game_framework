package NPGameRes.Refs.AvatarGacha;

import NPCommon.Log.CommLog;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

@RefTable(tableName = "gacha_pool_step")
public class RefGachaPoolStep extends RefBase
{
    private static RefGachaPoolStepMgr _g_mgr = new RefGachaPoolStepMgr();

    public static RefGachaPoolStepMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefGachaPoolStep> getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefGachaPoolStepMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefGachaPoolStep newRef = (RefGachaPoolStep) _newRef;
        id = newRef.id;
        pool_id = newRef.pool_id;
        step = newRef.step;
        upgrade_step_num = newRef.upgrade_step_num;
        guarantee_rule_list = newRef.guarantee_rule_list;
    }

    public static class RefGachaPoolStepMgr extends RefTableContainer<RefGachaPoolStep>
    {
        private Map<Long, RefGachaPoolStep> _m_poolStepMap = new HashMap<>();

        @Override
        public void _onTableLoaded()
        {
            //整理奖池步骤数据
            Map<Long, RefGachaPoolStep> map = new HashMap<>();
            for (RefGachaPoolStep ref : getList())
            {
                if (null == ref)
                    continue;

                long uniqueKey = getKey(ref.pool_id, ref.step);
                if (map.containsKey(uniqueKey))
                    CommLog.warn("RefGachaPoolStepMgr _onTableLoaded sort map found duplicate key:{} refId:{}", uniqueKey, ref.id);

                map.put(uniqueKey, ref);
            }

            //最后直接替换原来的map, 保证线程安全
            _m_poolStepMap = map;
        }

        /**
         * 生成唯一key
         * @param _poolId 奖池id
         * @param _step   步骤
         * @return 唯一key
         */
        public static long getKey(long _poolId, int _step)
        {
            return _poolId * 10000L + _step;
        }

        /**
         * 通过奖池步骤查找对应的配置数据
         * @param _poolId 奖池id
         * @param _step   步骤
         * @return 配置数据
         */
        public RefGachaPoolStep lookupByPoolStep(long _poolId, int _step)
        {
            return _m_poolStepMap.get(getKey(_poolId, _step));
        }
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

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long id;//id
    public long pool_id;//卡池id
    public int step;//阶段
    public int upgrade_step_num;//达到x次升阶
    public List<Long> guarantee_rule_list;//保底规则列表

    @RefField(isIgnore = true)
    public List<RefGachaGuarantee> guaranteeRuleRefList = new ArrayList<>();
}
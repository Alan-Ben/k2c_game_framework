package NPGameRes.Refs.ActivityFund;

import NPCommon.CommonObj.NPCommonItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.*;

/**
 * 活动基金等级配置表
 */
@RefTable(tableName = "activity_fund_level")
public class RefActivityFundLevel extends RefBase
{
    private static RefActivityFundLevelMgr _g_mgr = new RefActivityFundLevelMgr();

    public static RefActivityFundLevelMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefActivityFundLevelMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefActivityFundLevelMgr) _mgr;
    }

    public static class RefActivityFundLevelMgr extends RefTableContainer<RefActivityFundLevel>
    {
        // 按基金ID分组的等级列表
        private Map<Long, List<RefActivityFundLevel>> _m_levelMapByFundId = new HashMap<>();

        @Override
        public void _onTableLoaded()
        {
            // 按基金ID分组
            Map<Long, List<RefActivityFundLevel>> tempMap = new HashMap<>();

            for (RefActivityFundLevel ref : getList())
            {
                if (null == ref)
                    continue;

                tempMap.computeIfAbsent(ref.activity_fund_id, k -> new ArrayList<>()).add(ref);
            }

            // 按等级排序
            tempMap.forEach((key, value) -> value.sort(Comparator.comparingInt(o -> o.level)));

            _m_levelMapByFundId = tempMap;
        }

        /**
         * 根据基金ID和阶段号查找对应的等级配置
         * <p>
         * 通过 last_step 字段判断阶段属于哪个等级
         *
         * @param _fundId 基金ID
         * @param _step   阶段号
         * @return 等级配置，如果找不到返回null
         */
        public RefActivityFundLevel getLevelByStep(long _fundId, int _step)
        {
            List<RefActivityFundLevel> levelList = _m_levelMapByFundId.get(_fundId);
            if (null == levelList || levelList.isEmpty())
                return null;

            RefActivityFundLevel tarRef = null;
            for (RefActivityFundLevel ref : levelList)
            {
                if (ref.last_step < 0)
                {
                    // last_step 小于0表示没有上限，直接返回该等级
                    return ref;
                }

                if (ref.last_step < _step)
                {
                    tarRef = ref;
                }
                else
                {
                    // 找到第一个 last_step 大于等于 step 的等级，返回该等级
                    return ref;
                }

            }

            return tarRef;
        }

        /**
         * 根据基金ID和等级号查找等级配置
         *
         * @param _fundId 基金ID
         * @param _level  等级号
         * @return 等级配置，如果找不到返回null
         */
        public RefActivityFundLevel getLevelByLevelNum(long _fundId, int _level)
        {
            List<RefActivityFundLevel> levelList = _m_levelMapByFundId.get(_fundId);
            if (null == levelList)
                return null;

            for (RefActivityFundLevel ref : levelList)
            {
                if (ref.level == _level)
                    return ref;
            }

            return null;
        }

        /**
         * 根据基金ID获取所有等级配置列表
         *
         * @param _fundId 基金ID
         * @return 等级配置列表，如果找不到返回null
         */
        public List<RefActivityFundLevel> getLevelListByFundId(long _fundId)
        {
            return _m_levelMapByFundId.get(_fundId);
        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefActivityFundLevel newRef = (RefActivityFundLevel) _newRef;
        id = newRef.id;
        activity_fund_id = newRef.activity_fund_id;
        level = newRef.level;
        last_step = newRef.last_step;
        distinguish_item = newRef.distinguish_item;
        activate_exp_count = newRef.activate_exp_count;
        gift_pack_id = newRef.gift_pack_id;
    }

    @Override
    public long Id()
    {
        return id;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    // 唯一ID
    public long id;

    // 基金ID
    public long activity_fund_id;

    // 基金等级
    public int level;

    // 该等级的最后一个阶段（用于标识等级范围）
    public int last_step;

    // 凭证道具（拥有此道具可领取该等级的付费档）
    public NPCommonItem distinguish_item;

    // 激活该等级时获得的经验值（购买凭证道具时获得）
    public long activate_exp_count;

    // 付费礼包ID（该等级对应的付费礼包）
    public long gift_pack_id;
}

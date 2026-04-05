package NPGameRes.Refs.Travel;

import NPCommon.Game.MixedProbList;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;

import java.util.ArrayList;
import java.util.Set;

/**
 * @author mark
 */
@RefTable(tableName = "travel_pos")
public class RefTravelPos extends RefBase
{
    private static RefTravelPosMgr _g_mgr = new RefTravelPosMgr();
    public static RefTravelPosMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTravelPosMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTravelPosMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefTravelPos newRef = (RefTravelPos) _newRef;
        id = newRef.id;
        travel_event_list = newRef.travel_event_list;
        rand_pro = newRef.rand_pro;
        rand_wei = newRef.rand_wei;
        unlock_condition = newRef.unlock_condition;
    }
    
    public static class RefTravelPosMgr extends RefTableContainer<RefTravelPos>
    {
        private MixedProbList<RefTravelPos> _m_probList = new MixedProbList<>();

        @Override
        protected void _onTableLoaded()
        {
            MixedProbList<RefTravelPos> probList = new MixedProbList<>();
            for (RefTravelPos posRef : getList())
                probList.add(posRef, posRef.rand_pro, posRef.rand_wei);
            _m_probList = probList;
        }

        /**
         * 随机选取一个游历位置，传入上次位置ID则排除以避免连续重复，传入未解锁位置集合则额外排除这些位置
         * @param excludePosId 上次位置ID，0表示无需排除
         * @param lockedPosIds 未解锁的位置ID集合，null表示无额外排除
         */
        public RefTravelPos randPos(long excludePosId, Set<Long> lockedPosIds)
        {
            // 无需任何过滤时，直接使用预构建列表（最优路径）
            if ((lockedPosIds == null || lockedPosIds.isEmpty()) && (excludePosId == 0 || getList().size() <= 1))
                return _m_probList.random();

            MixedProbList<RefTravelPos> tempList = new MixedProbList<>();
            for (RefTravelPos posRef : getList())
            {
                if (posRef.id == excludePosId)
                    continue;
                if (lockedPosIds != null && lockedPosIds.contains(posRef.id))
                    continue;
                tempList.add(posRef, posRef.rand_pro, posRef.rand_wei);
            }
            return tempList.random();
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

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long id;//唯一id
    public ArrayList<Long> travel_event_list = new ArrayList<>();//出现的事件列表
    public int rand_pro;//随机绝对概率（万分比）
    public int rand_wei;//随机权重
    public NPPlayerConditionGroupObj unlock_condition;//解锁条件，满足条件才会加入随机权重列表

    // TravelInitDealer 填充，预解析的事件对象列表，供运行时直接使用
    @RefField(isIgnore = true)
    public ArrayList<RefTravelEvent> eventRefList = new ArrayList<>();
}

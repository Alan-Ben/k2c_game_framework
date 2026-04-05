package NPGameRes.Refs.RechargeRebate;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.ArrayList;
import java.util.List;

/**
 * 充值返利档位配置表
 * <p>
 * 对应表：recharge_rebate_step
 * 定义每个档位的目标计数和奖励列表
 */
@RefTable(tableName = "recharge_rebate_step")
public class RefRechargeRebateStep extends RefBase
{
    private static RefRechargeRebateStepMgr _g_mgr = new RefRechargeRebateStepMgr();

    public static RefRechargeRebateStepMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefRechargeRebateStepMgr getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefRechargeRebateStepMgr) _mgr;
    }

    public static class RefRechargeRebateStepMgr extends RefTableContainer<RefRechargeRebateStep>
    {

        /**
         * 根据档位ID查找档位配置
         */
        public RefRechargeRebateStep lookupStepRef(long _groupId, long _stepId)
        {
            List<RefRechargeRebateStep> list = getList();
            for (RefRechargeRebateStep refStep : list)
            {
                if (refStep.step == _stepId && refStep.group_id == _groupId)
                {

                    return refStep;
                }
            }
            return null;
        }

        /**
         * 获取某个组下的所有档位配置
         */
        public List<RefRechargeRebateStep> getAllSteps(long _groupId)
        {
            List<RefRechargeRebateStep> resList = new ArrayList<>();

            for (RefRechargeRebateStep refStep : getList())
            {
                if (refStep.group_id == _groupId)
                {
                    resList.add(refStep);
                }
            }
            return resList;
        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefRechargeRebateStep newRef = (RefRechargeRebateStep) _newRef;
        id = newRef.id;
        group_id = newRef.group_id;
        step = newRef.step;
        target_count = newRef.target_count;
        reward_list = newRef.reward_list;
    }

    @Override
    public long Id()
    {
        return id;
    }

    // 档位ID（主键）
    public long id;

    // 所属组ID（外键关联recharge_rebate_group.id）
    public long group_id;

    // 档位顺序
    public long step;

    // 目标计数（VIP点数或充值天数）
    public long target_count;

    // 奖励列表配置
    public List<NPCommonCostItem> reward_list;
}

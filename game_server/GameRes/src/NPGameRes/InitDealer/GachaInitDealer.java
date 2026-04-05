package NPGameRes.InitDealer;

import NPCommon.Log.CommLog;
import NPGameRes.Refs.AvatarGacha.*;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 * 抽卡组件配表初始化处理器
 */
public class GachaInitDealer extends _ABasicInitDealer
{
    @Override
    public void dealInit()
    {
        //处理奖池物品列表，按照配表的奖池id整理到map中
        Map<Long, List<RefGachaItem>> itemMap = new HashMap<>();
        for (RefGachaItem ref : RefGachaItem.getMgr().getList())
        {
            List<RefGachaItem> itemList = itemMap.computeIfAbsent(ref.pool_id, k -> new ArrayList<>());
            itemList.add(ref);
        }

        //处理奖池物品列表，按照配表的奖池id整理到map中
        Map<Long, List<RefGachaPoolWeight>> weightMap = new HashMap<>();
        for (RefGachaPoolWeight ref : RefGachaPoolWeight.getMgr().getList())
        {
            List<RefGachaPoolWeight> weightList = weightMap.computeIfAbsent(ref.pool_id, k -> new ArrayList<>());
            weightList.add(ref);
        }

        //处理奖池的保底规则，并将保底规则放置在奖池对象上
        for (RefGachaPool ref : RefGachaPool.getMgr().getList())
        {
            //获取奖池对应的物品列表
            List<RefGachaItem> itemRefList = itemMap.getOrDefault(ref.id, new ArrayList<>());
            if (itemRefList.isEmpty())
                CommLog.error("GachaInitDealer sort item ref list not find for pool, id:{}", ref.id);

            //获取奖池对应的权重列表
            List<RefGachaPoolWeight> weightRefList = weightMap.getOrDefault(ref.id, new ArrayList<>());
            if (weightRefList.isEmpty())
                CommLog.error("GachaInitDealer sort weight ref list not find for pool, id:{}", ref.id);

            ref.itemRefList = itemRefList;
            ref.weightRefList = weightRefList;
        }

        //处理奖池的保底规则，并将保底规则放置在奖池阶段对象上
        for (RefGachaPoolStep ref : RefGachaPoolStep.getMgr().getList())
        {
            //获取奖池对应的保底规则列表
            List<RefGachaGuarantee> guaranteeList = new ArrayList<>();
            for (Long ruleId : ref.guarantee_rule_list)
            {
                RefGachaGuarantee refGuarantee = RefGachaGuarantee.getMgr().get(ruleId);
                if (refGuarantee == null)
                {
                    CommLog.error("GachaInitDealer sort guarantee ref list not find for pool, id:{}", ruleId);
                    continue;
                }

                guaranteeList.add(refGuarantee);
            }

            ref.guaranteeRuleRefList = guaranteeList;
        }

    }
}

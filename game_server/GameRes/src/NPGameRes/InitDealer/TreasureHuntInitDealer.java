package NPGameRes.InitDealer;

import NPCommon.Game.WeightIndexList;
import NPCommon.Game.WeightValueList;
import NPCommon.Game.WeightValueList.WeightValue;
import NPCommon.Log.CommLog;
import NPEnum.EQuality;
import NPGameRes.Refs.RefGeneral;
import NPGameRes.Refs.TreasureHunt.*;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 * 寻宝系统初始化处理器
 * 负责在游戏启动时初始化寻宝相关的配置数据，包括：
 * 1. 矿石组合目录映射
 * 2. 矿石首次产出等级概率列表
 * 3. 高级能量对宝物品质权重的加成
 * 4. 区域矿石品质映射
 * 5. 技能等级列表排序
 * 6. 奇物产出信息关联
 */
public class TreasureHuntInitDealer extends _ABasicInitDealer
{
    /**
     * 寻宝系统数据初始化
     * 在游戏启动时执行，初始化所有寻宝相关的配置数据
     */
    @Override
    public void dealInit()
    {
        // 第一步：构建矿石与组合目录的映射关系
        List<RefTreasureHuntCompositeCatalog> refList = RefTreasureHuntCompositeCatalog.getMgr().getList();

        // 用于存储矿石ID到组合目录ID列表的映射
        Map<Long, List<Long>> refMap = new HashMap<>();

        // 遍历所有组合目录，建立矿石到组合目录的反向映射
        for (RefTreasureHuntCompositeCatalog ref : refList)
        {
            if (ref == null)
                continue;

            // 将每个矿石ID映射到包含它的组合目录ID列表中
            for (Long oreId : ref.ore_list)
            {
                refMap.computeIfAbsent(oreId, k -> new ArrayList<>()).add(ref.Id());
            }
        }

        // 为每个矿石设置组合目录ID列表和首次产出等级概率列表
        refMap.forEach((oreId, idList) ->
        {
            RefTreasureHuntOre refOre = RefTreasureHuntOre.getMgr().get(oreId);
            if (refOre == null)
            {
                CommLog.error("TreasureHuntInitDealer dealInit refOre is null, oreId:{}", oreId);
                return;
            }

            // 设置该矿石可以参与的组合目录ID列表
            refOre.setCompositeIdList(idList);
        });

        List<RefTreasureHuntOre> refOreList = RefTreasureHuntOre.getMgr().getList();
        for (RefTreasureHuntOre refOre : refOreList)
        {
            // 构建首次产出时的等级概率列表（仅包含低等级）
            WeightIndexList weightIndexList = new WeightIndexList();
            for (int i = 0; i < refOre.grade_probability_list.getList().getList().size(); i++)
            {
                WeightValue<Integer> weightValue = refOre.grade_probability_list.getList().getList().get(i);
                if (weightValue == null)
                    continue;

                // 只包含低于高级矿石最小等级的概率
                if (i >= RefGeneral.Ref().treasure_hunt_advanced_ore_min_grade)
                    break;

                weightIndexList.getList().add(weightValue.value, weightValue.weight);
            }
            // 设置首次产出等级概率列表
            refOre.setFirstTimeGradeProbabilityList(weightIndexList);
        }

        // 第二步：计算区域内奇物的权重总和
        for (RefTreasureHuntArea refArea : RefTreasureHuntArea.getMgr().getList())
        {
            if (refArea == null)
                continue;

            // 计算该区域内所有奇物的权重总和
            int totalWeight = 0;
            for (Long treasureId : refArea.treasure_list)
            {
                RefTreasureHuntTreasure refTreasure = RefTreasureHuntTreasure.getMgr().get(treasureId);
                if (refTreasure == null)
                    continue;

                totalWeight += refTreasure.gain_weight;
            }
            // 设置该区域的奇物权重总和
            refArea.setTreasureWeight(totalWeight);
        }

        // 第三步：构建区域内信息
        List<RefTreasureHuntArea> areaList = RefTreasureHuntArea.getMgr().getList();
        for (RefTreasureHuntArea refArea : areaList)
        {
            // 存储该区域内的奇物权重列表
            WeightValueList<RefTreasureHuntTreasure> treasureWeightList = new WeightValueList<>();
            // 用于存储该区域内不同品质的矿石列表
            Map<EQuality,List<RefTreasureHuntOre>> qualityOreMap = new HashMap<>();

            // 遍历该区域的所有矿石，按品质分类
            for (Long oreId : refArea.ore_list)
            {
                RefTreasureHuntOre refOre = RefTreasureHuntOre.getMgr().get(oreId);
                if (refOre == null)
                {
                    CommLog.error("TreasureHuntInitDealer dealInit refOre is null, oreId:{}", oreId);
                    continue;
                }

                // 将矿石按品质分组存储
                qualityOreMap.computeIfAbsent(refOre.quality, k -> new ArrayList<>()).add(refOre);
            }

            // 遍历该区域的所有奇物，构建权重列表
            for (Long treasureId : refArea.treasure_list)
            {
                RefTreasureHuntTreasure refTreasure = RefTreasureHuntTreasure.getMgr().get(treasureId);
                if (refTreasure == null)
                {
                    CommLog.error("TreasureHuntInitDealer dealInit refTreasure is null, treasureId:{}", treasureId);
                    continue;
                }

                // 将奇物添加到权重列表中
                treasureWeightList.add(refTreasure, refTreasure.gain_weight);
            }

            // 设置该区域的品质矿石映射
            refArea.setQualityOreMap(qualityOreMap);
            // 设置该区域的奇物权重列表
            refArea.setTreasureWeightList(treasureWeightList);
        }

        // 第四步：构建技能等级映射并排序
        Map<Long,List<RefTreasureHuntSkillLevel>> skillLevelMap = new HashMap<>();
        
        // 将所有技能等级按技能ID分组
        for (RefTreasureHuntSkillLevel refSkillLevel : RefTreasureHuntSkillLevel.getMgr().getList())
        {
            if (refSkillLevel == null)
                continue;

            skillLevelMap.computeIfAbsent(refSkillLevel.skill_id, k -> new ArrayList<>()).add(refSkillLevel);
        }
        
        // 为每个技能设置按等级排序的技能等级列表
        skillLevelMap.forEach((skillId, levelList) ->
        {
            RefTreasureHuntSkill refSkill = RefTreasureHuntSkill.getMgr().get(skillId);
            if (refSkill == null)
            {
                CommLog.error("TreasureHuntInitDealer dealInit refSkill is null, skillId:{}", skillId);
                return;
            }

            // 按等级从低到高排序
            levelList.sort((l1, l2) -> Integer.compare(l1.level, l2.level));
            // 设置该技能的等级列表
            refSkill.setSkillLevelList(levelList);
        });

        // 第五步：关联奇物与产出配置
        for (RefTreasureHuntTreasureOutput refTreasureOutput : RefTreasureHuntTreasureOutput.getMgr().getList())
        {
            if (refTreasureOutput == null)
                continue;

            // 通过treasure_id查找对应的奇物配置
            RefTreasureHuntTreasure refTreasure = RefTreasureHuntTreasure.getMgr().get(refTreasureOutput.treasure_id);
            if (refTreasure == null)
            {
                CommLog.error("TreasureHuntInitDealer dealInit refTreasure is null, treasureId:{}", refTreasureOutput.treasure_id);
                continue;
            }

            // 将产出配置关联到奇物上
            refTreasure.setTreasureOutput(refTreasureOutput);
        }
    }
}

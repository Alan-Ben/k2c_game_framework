package NPGameRes.Refs.TreasureHunt;

import NPCommon.Game.WeightIndexList;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPEnum.EQuality;
import NPGameRes.Refs.RefGeneral;

import java.util.ArrayList;
import java.util.List;

/**
 * id	矿石普通技能id		质量:奖励档位列表(下包上不包区间）	档位权重列表	品质
 * id	normal_skill_id	advanced_skill_id	mass_reward_grade_list	grade_probability_list	quality
 * @author mark
 */
@RefTable(tableName = "treasure_hunt_ore")
public class RefTreasureHuntOre extends RefBase
{
    private static RefTreasureHuntOreMgr _g_mgr = new RefTreasureHuntOreMgr();

    public static RefTreasureHuntOreMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTreasureHuntOreMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTreasureHuntOreMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefTreasureHuntOre newRef = (RefTreasureHuntOre) _newRef;
        id = newRef.id;
        normal_skill_id = newRef.normal_skill_id;
        advanced_skill_id = newRef.advanced_skill_id;
        mass_reward_grade_list = newRef.mass_reward_grade_list;
        grade_probability_list = newRef.grade_probability_list;
        quality = newRef.quality;
    }

    public static class RefTreasureHuntOreMgr extends RefTableContainer<RefTreasureHuntOre>
    {
        @Override
        protected void _onTableLoaded()
        {

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

    public long id;//id
    public long normal_skill_id;//普通技能id
    public long advanced_skill_id;//高级技能id
    public TreasureHuntMassGradeList mass_reward_grade_list = new TreasureHuntMassGradeList();//质量:奖励档位列表(下包上不包区间）
    public WeightIndexList grade_probability_list = new WeightIndexList();//档位权重列表
    public EQuality quality;//品质

    @RefField(isIgnore = true)
    public List<Long> compositeIdList = new ArrayList<>();
    public void setCompositeIdList(List<Long> _compositeIdList)
    {
        compositeIdList = _compositeIdList;
    }

    @RefField(isIgnore = true)
    public WeightIndexList firstTimeGradeProbabilityList = new WeightIndexList();
    public void setFirstTimeGradeProbabilityList(WeightIndexList _gradeProbabilityList)
    {
        firstTimeGradeProbabilityList = _gradeProbabilityList;
    }

    /**
     * 获取矿石重量对应的档位索引
     * @param _weight 矿石重量
     * @return
     */
    public int getGradeIndex(int _weight)
    {
        return mass_reward_grade_list.getGradeIndexByWeight(_weight);
    }

    /**
     * 判断矿石是否为高级矿石
     * @param _weight 矿石重量
     * @return
     */
    public boolean isAdvanced(int _weight)
    {
        return RefGeneral.Ref().treasure_hunt_advanced_ore_min_grade <= getGradeIndex(_weight);
    }
}

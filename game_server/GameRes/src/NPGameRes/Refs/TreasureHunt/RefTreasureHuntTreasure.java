package NPGameRes.Refs.TreasureHunt;

import NPCommon.CommonObj.NPCommonItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPEnum.EQuality;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;

/**
 * id	奇物技能id	升级消耗的物品	品质	解锁条件	满足解锁条件是否必定获得	获取权重
 * id	skill_id	upgrade_cost	quality	unlock_condition	met_condition_must_gain	gain_weight
 * @author mark
 */
@RefTable(tableName = "treasure_hunt_treasure")
public class RefTreasureHuntTreasure extends RefBase
{
    private static RefTreasureHuntTreasureMgr _g_mgr = new RefTreasureHuntTreasureMgr();

    public static RefTreasureHuntTreasureMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTreasureHuntTreasureMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTreasureHuntTreasureMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefTreasureHuntTreasure newRef = (RefTreasureHuntTreasure) _newRef;
        id = newRef.id;
        skill_id = newRef.skill_id;
        upgrade_cost = newRef.upgrade_cost;
        quality = newRef.quality;
        unlock_condition = newRef.unlock_condition;
        met_condition_must_gain = newRef.met_condition_must_gain;
        gain_weight = newRef.gain_weight;
    }

    public static class RefTreasureHuntTreasureMgr extends RefTableContainer<RefTreasureHuntTreasure>
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
    public long skill_id;//奇物技能id
    public NPCommonItem upgrade_cost;//升级消耗的物品
    public EQuality quality;//品质
    public NPPlayerConditionGroupObj unlock_condition;//解锁条件
    public boolean met_condition_must_gain;//满足解锁条件是否必定获得
    public int gain_weight;//获取权重

    // 动态设置的产出信息（由初始化器设置）
    @RefField(isIgnore = true)
    private RefTreasureHuntTreasureOutput treasureOutput;

    /**
     * 设置奇物的产出信息
     * 在初始化时由TreasureHuntInitDealer调用
     * 
     * @param _treasureOutput 奇物产出配置
     */
    public void setTreasureOutput(RefTreasureHuntTreasureOutput _treasureOutput)
    {
        this.treasureOutput = _treasureOutput;
    }

    /**
     * 获取奇物的产出信息
     * 
     * @return 奇物产出配置，可能为null
     */
    public RefTreasureHuntTreasureOutput getTreasureOutputRef()
    {
        return treasureOutput;
    }
}

package NPGameRes.Refs.Equip;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPCommon.Util.Pair.WCGPairInt;
import NPEnum.EQuality;

import java.util.List;

@RefTable(tableName = "equip")
public class RefEquip extends RefBase
{
    private static RefEquipMgr _g_mgr = new RefEquipMgr();

    public static RefEquipMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefContainerBase<? extends RefBase> getStaticContainer()
    {
        return _g_mgr;
    }

    /**
     * 获取觉醒技能数
     * @param _awakenLevel
     * @return
     */
    public int getAwakenSkillNum(int _awakenLevel)
    {
        int skillNum = 0;
        for (WCGPairInt pair : awaken_add_skill_num)
        {
            if (pair.first() > _awakenLevel)
                break;

            skillNum = pair.second();
        }

        return skillNum;
    }

    public static class RefEquipMgr extends RefTableContainer<RefEquip>
    {

    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefEquipMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefEquip newRef = (RefEquip) _newRef;
        id = newRef.id;
        quality = newRef.quality;
        num_limit = newRef.num_limit;
        level_limit = newRef.level_limit;
        initial_talent = newRef.initial_talent;
        initial_skill_num = newRef.initial_skill_num;
        cost_item = newRef.cost_item;
        upgrade_increase_talent = newRef.upgrade_increase_talent;
        awaken_add_skill_num = newRef.awaken_add_skill_num;
        disassemble_get_item_list = newRef.disassemble_get_item_list;
        awaken_group_id_list = newRef.awaken_group_id_list;
        name = newRef.name;
    }

    @Override
    public long Id()
    {
        return id;
    }

    public long id;
    public EQuality quality;
    public int num_limit;//数量上限
    public int level_limit;//等级上限
    public int initial_talent;//初始资质
    public int initial_skill_num;//初始技能数
    public NPCommonCostItem cost_item;//升级消耗
    public int upgrade_increase_talent;//升级增加资质点数
    public List<WCGPairInt> awaken_add_skill_num;//觉醒等级额外获得技能数
    public List<NPCommonCostItem> disassemble_get_item_list;//分解基础返还道具列表
    public List<Long> awaken_group_id_list;//觉醒消耗组列表
    public String name;//名字
}

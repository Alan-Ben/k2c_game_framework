package NPGameRes.Refs.Hero;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPCommon.Util.Pair.WCGPairLong;
import NPGameRes.GameObjs.CommonObj.LevelObj._ILevelBasicObj;
import NPGameRes.GameObjs.PlayerAttrProperty.PlayerAttrPropertyModifier;

import java.util.List;

@RefTable(tableName = "hero_halo_level")
public class RefHeroHaloLevel extends RefBase implements _ILevelBasicObj
{
    private static RefTableContainer<RefHeroHaloLevel> _g_mgr = new RefTableContainer<RefHeroHaloLevel>();

    public static RefTableContainer<RefHeroHaloLevel> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefHeroHaloLevel> getStaticContainer()
    {
        return _g_mgr;
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefHeroHaloLevel>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefHeroHaloLevel newRef = (RefHeroHaloLevel) _newRef;
        id = newRef.id;
        halo_id = newRef.halo_id;
        level = newRef.level;
        halo_suit_skill_level_list = newRef.halo_suit_skill_level_list;
        self_attr_prop_modifier = newRef.self_attr_prop_modifier;
        upgrade_cost = newRef.upgrade_cost;
    }

    /**
     * 获取等级
     * @return
     */
    public int getLevel(){return level;}

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

    public long id;//唯一id
    public long halo_id;//光环id
    public int level;//等级
    public List<WCGPairLong> halo_suit_skill_level_list;//套系技能加成等级
    public PlayerAttrPropertyModifier self_attr_prop_modifier;//玩家属性加成
    public NPCommonCostItem upgrade_cost;//升级到下一级消耗(解锁消耗配置在1级上）
}

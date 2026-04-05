package NPGameRes.Refs.Consort;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.CommonObj.LevelObj._ILevelBasicObj;
import NPGameRes.GameObjs.PlayerAttrProperty.PlayerAttrPropertyModifier;
import NPGameRes.GameObjs.RefUnionBonus.UnionBonus;

/**
 * @author mark
 */
@RefTable(tableName = "consort_halo_skill_lvl")
public class RefConsortHaloSkillLvl extends RefBase implements _ILevelBasicObj
{
    private static RefConsortHaloSkillLvlMgr _g_mgr = new RefConsortHaloSkillLvlMgr();
    public static RefConsortHaloSkillLvlMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefConsortHaloSkillLvl> getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefConsortHaloSkillLvlMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefConsortHaloSkillLvl newRef = (RefConsortHaloSkillLvl) _newRef;
        id = newRef.id;
        halo_skill_id = newRef.halo_skill_id;
        halo_skill_lvl = newRef.halo_skill_lvl;
        add_basic_attr = newRef.add_basic_attr;
        add_bonus_attr = newRef.add_bonus_attr;
    }
    
    public static class RefConsortHaloSkillLvlMgr extends RefTableContainer<RefConsortHaloSkillLvl>
    {
    	@Override
        public void _onTableLoaded()
        {
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
    
    @Override
    public int getLevel()
    {
    	return halo_skill_lvl;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long id;
    public long halo_skill_id;//星辉技能id
    public int halo_skill_lvl;//星辉技能等级
    public PlayerAttrPropertyModifier add_basic_attr;//加护伙伴加成（EBasicAttrType）
    public UnionBonus add_bonus_attr;//全局属性加成（EBonusPropertyType）
}

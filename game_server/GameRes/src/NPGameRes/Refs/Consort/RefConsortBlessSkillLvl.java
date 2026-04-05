package NPGameRes.Refs.Consort;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.CommonObj.LevelObj._ILevelBasicObj;
import NPGameRes.GameObjs.PlayerAttrProperty.PlayerAttrPropertyModifier;

/**
 * @author mark
 */
@RefTable(tableName = "consort_bless_skill_lvl")
public class RefConsortBlessSkillLvl extends RefBase implements _ILevelBasicObj
{
    private static RefConsortBlessSkillLvlMgr _g_mgr = new RefConsortBlessSkillLvlMgr();
    public static RefConsortBlessSkillLvlMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefConsortBlessSkillLvl> getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefConsortBlessSkillLvlMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefConsortBlessSkillLvl newRef = (RefConsortBlessSkillLvl) _newRef;
        id = newRef.id;
        bless_skill_id = newRef.bless_skill_id;
        lvl = newRef.lvl;
        cost_skill_point = newRef.cost_skill_point;
        add = newRef.add;
    }
    
    public static class RefConsortBlessSkillLvlMgr extends RefTableContainer<RefConsortBlessSkillLvl>
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
    	return lvl;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long id;
    public long bless_skill_id;//加护技能id
    public int lvl;//等级
    public int cost_skill_point;//升到下一级消耗加护点
    public PlayerAttrPropertyModifier add;//加成的属性（枚举:属性）
}

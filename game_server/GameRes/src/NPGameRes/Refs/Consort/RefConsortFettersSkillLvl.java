package NPGameRes.Refs.Consort;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.CommonObj.LevelObj._ILevelBasicObj;
import NPGameRes.GameObjs.NPPlayerProperty.NPPlayerPropertyModifier;
import NPGameRes.GameObjs.RefUnionBonus.UnionBonus;

/**
 * @author mark
 */
@RefTable(tableName = "consort_fetters_skill_lvl")
public class RefConsortFettersSkillLvl extends RefBase implements _ILevelBasicObj
{
    private static RefConsortFettersSkillLvlMgr _g_mgr = new RefConsortFettersSkillLvlMgr();
    public static RefConsortFettersSkillLvlMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefConsortFettersSkillLvl> getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefConsortFettersSkillLvlMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefConsortFettersSkillLvl newRef = (RefConsortFettersSkillLvl) _newRef;
        id = newRef.id;
        fetters_skill_id = newRef.fetters_skill_id;
        lvl = newRef.lvl;
        add_bonus = newRef.add_bonus;
        add_player = newRef.add_player;
    }
    
    public static class RefConsortFettersSkillLvlMgr extends RefTableContainer<RefConsortFettersSkillLvl>
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
    public long fetters_skill_id;
    public int lvl;
    public UnionBonus add_bonus;
    public NPPlayerPropertyModifier add_player; //玩家属性加成
}

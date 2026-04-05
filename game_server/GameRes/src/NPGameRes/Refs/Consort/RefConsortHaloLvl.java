package NPGameRes.Refs.Consort;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.CommonObj.LevelObj._ILevelBasicObj;

import java.util.ArrayList;

/**
 * @author mark
 */
@RefTable(tableName = "consort_halo_lvl")
public class RefConsortHaloLvl extends RefBase implements _ILevelBasicObj
{
    private static RefConsortSkinMgr _g_mgr = new RefConsortSkinMgr();
    public static RefConsortSkinMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefConsortHaloLvl> getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefConsortSkinMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefConsortHaloLvl newRef = (RefConsortHaloLvl) _newRef;
        id = newRef.id;
        halo_id = newRef.halo_id;
        level = newRef.level;
        cost_item = newRef.cost_item;
        add_intimacy = newRef.add_intimacy;
        add_charm = newRef.add_charm;
        reward_item_list = newRef.reward_item_list;
        halo_skill_list = newRef.halo_skill_list;
    }
    
    public static class RefConsortSkinMgr extends RefTableContainer<RefConsortHaloLvl>
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
    	return level;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long id;
    public long halo_id;
    public int level;
    public NPCommonCostItem cost_item;
    public int add_intimacy;//增加的亲密度
    public int add_charm;//增加的加护力
    public ArrayList<NPCommonCostItem> reward_item_list = new ArrayList<>();//星辉等级奖励
    public ArrayList<ConsortHaloSkillParseObj> halo_skill_list = new ArrayList<>();//星辉技能:等级

    @RefField(isIgnore = true)
    public ArrayList<ConsortHaloSkillObj> haloSkillList = new ArrayList<>();
    public ConsortHaloSkillObj lookupSkill(long _skillId)
    {
    	ArrayList<ConsortHaloSkillObj> list = this.haloSkillList;
    	for(int i = 0; i < list.size(); i++)
    	{
    		ConsortHaloSkillObj skillObj = list.get(i);
    		if(null == skillObj)
    			continue;
    		
    		if(skillObj.getSkilllRef().halo_skill_id == _skillId)
    			return skillObj;
    	}
    	
    	return null;
    }
}

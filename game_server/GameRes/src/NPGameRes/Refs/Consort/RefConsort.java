package NPGameRes.Refs.Consort;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPCommon.Util.CommonFunc;
import NPEnum.EQuality;
import NPGameRes.GameObjs.CommonObj.LevelObj._TLevelMapMgr;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.Set;

/**
 * @author mark
 */
@RefTable(tableName = "consort")
public class RefConsort extends RefBase
{
    private static RefConsortMgr _g_mgr = new RefConsortMgr();
    public static RefConsortMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefConsortMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefConsortMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefConsort newRef = (RefConsort) _newRef;
        id = newRef.id;
        relation_hero_id_list = newRef.relation_hero_id_list;
        unlock_gain_item_list = newRef.unlock_gain_item_list;
        init_intimacy = newRef.init_intimacy;
        init_charm = newRef.init_charm;
        init_fetters_lvl = newRef.init_fetters_lvl;
        default_skin_id = newRef.default_skin_id;
        consort_halo_id = newRef.consort_halo_id;
        consort_fetters_skill_id = newRef.consort_fetters_skill_id;
        bless_skill_id_list = newRef.bless_skill_id_list;
        quality = newRef.quality;
        name = newRef.name;
    }
    
    public static class RefConsortMgr extends RefTableContainer<RefConsort>
    {
    	@Override
    	protected void _onTableLoaded()
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

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long id;
    public ArrayList<Long> relation_hero_id_list = new ArrayList<>(); //关联大臣id列表
    public ArrayList<NPCommonCostItem> unlock_gain_item_list = new ArrayList<>(); //解锁同时获得道具列表
    public int init_intimacy; //初始亲密度
    public int init_charm; //初始魅力值
    public int init_fetters_lvl;//初始羁绊等级
    public long default_skin_id; //默认皮肤id
    public long consort_halo_id;//家人星辉id
    public long consort_fetters_skill_id;//家人羁绊技能id
    public ArrayList<Long> bless_skill_id_list = new ArrayList<>();//加护技能列表
    public EQuality quality;//品质
    public String name;//名称

    //家人加护技能列表
    @RefField(isIgnore = true)
    public ArrayList<RefConsortBlessSkill> blessSkillList = new ArrayList<>();
    
    //家人星辉技能列表
    @RefField(isIgnore = true)
    private _TLevelMapMgr<RefConsortHaloLvl> _m_lmHaloLevelMapMgr = new _TLevelMapMgr<RefConsortHaloLvl>();
    public void setHaloLevelMapMgr(_TLevelMapMgr<RefConsortHaloLvl> _mgr) {_m_lmHaloLevelMapMgr = _mgr;}
    public _TLevelMapMgr<RefConsortHaloLvl> getHaloLevelMapMgr() {return _m_lmHaloLevelMapMgr;}
    
    //家人事件列表，根据事件类型EConsortStoryType进行区分
    @RefField(isIgnore = true)
    public HashMap<Integer, ArrayList<RefConsortStory>>  callStoryRefListMap = new HashMap<>();

    /************
     * 随机获取邀约事件，但是优先排除已触发的事件
     * @param _unlockRefList
     * @param _triggeredStoryIdSet
     * @return
     */
    public RefConsortStory rndCallStory(ArrayList<RefConsortStory> _unlockRefList, Set<Long> _triggeredStoryIdSet)
    {
    	if(null == _unlockRefList || _unlockRefList.isEmpty())
    		return null;
    	
    	//随机先选取1个事件，如果该事件已经触发，则顺序检查下一个，
        //如果下一个已经到末尾，则从头开始检查，
        //如果检查了一圈都已经触发，则选取最初随机的事件
    	int idx = CommonFunc.randomInt(_unlockRefList.size() - 1);
    	for(int i = 0; i < _unlockRefList.size(); i++)
    	{
    		int curIdx = idx + i;
    		//当前超过了
    		if(curIdx >= _unlockRefList.size())
    			curIdx = curIdx % _unlockRefList.size();
    		
    		RefConsortStory ref = _unlockRefList.get(curIdx);
    		if(null == ref)
    			continue;
    		
    		//已经触发
    		if(_triggeredStoryIdSet.contains(ref.id))
    			continue;
    		
    		return ref;
    	}
    	
    	return _unlockRefList.get(idx);
    }
}

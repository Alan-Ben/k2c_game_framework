package NPGameRes.Refs.Hero;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.Log.CommLog;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.CommonObj.LevelObj._ILevelBasicObj;

import java.util.HashMap;
import java.util.Map;

@RefTable(tableName = "hero_business_skill_upgrade")
public class RefHeroBusinessSkillUpgrade extends RefBase implements _ILevelBasicObj
{
    private static RefHeroBusinessSkillUpgradeMgr _g_mgr = new RefHeroBusinessSkillUpgradeMgr();

    public static RefHeroBusinessSkillUpgradeMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefHeroBusinessSkillUpgrade> getStaticContainer()
    {
        return _g_mgr;
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefHeroBusinessSkillUpgradeMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefHeroBusinessSkillUpgrade newRef = (RefHeroBusinessSkillUpgrade) _newRef;
        id = newRef.id;
        group_id = newRef.group_id;
        business_skill_lvl = newRef.business_skill_lvl;
        upgrade_cost_item = newRef.upgrade_cost_item;
    }

    public static class RefHeroBusinessSkillUpgradeMgr extends RefTableContainer<RefHeroBusinessSkillUpgrade>
    {
        //速查map
        private Map<Long, RefHeroBusinessSkillUpgrade> _m_map;

        @Override
        public void _onTableLoaded()
        {
            HashMap<Long, RefHeroBusinessSkillUpgrade> newMap = new HashMap<>();
            for (RefHeroBusinessSkillUpgrade refLevel : getList())
            {
                //拼接一个key用于检索配置
                long key = getKey(refLevel.group_id, refLevel.business_skill_lvl);
                //判断key是否已经有了
                if (newMap.containsKey(key))
                    CommLog.warn("RefHeroBusinessSkillUpgrade repeat refLevel groupId:{} level:{}", refLevel.group_id, refLevel.business_skill_lvl);

                newMap.put(key, refLevel);
            }

            _m_map = newMap;
        }

        /**
         * 拼接检索使用的key
         * @param _groupId 分组id
         * @param _level   等级
         * @return
         */
        public static long getKey(long _groupId, int _level)
        {
            return _groupId * 10000L + _level;
        }

        /**
         * 查找升级配置
         * @param _haloId 光环id
         * @param _level  等级
         * @return
         */
        public RefHeroBusinessSkillUpgrade lookupLevelRef(long _haloId, int _level)
        {
            long key = getKey(_haloId, _level);
            return _m_map.get(key);
        }
    }

    /**
     * 获取等级
     * @return
     */
    public int getLevel()
    {
        return business_skill_lvl;
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

    public long id;//技能星级
    public int group_id;//组id
    public int business_skill_lvl;//技能等级
    public NPCommonCostItem upgrade_cost_item;//升级到下一级消耗
}

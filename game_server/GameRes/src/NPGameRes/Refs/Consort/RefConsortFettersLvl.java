package NPGameRes.Refs.Consort;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

/**
 * @author mark
 */
@RefTable(tableName = "consort_fetters_lvl")
public class RefConsortFettersLvl extends RefBase
{
    private static RefConsortDialogueMgr _g_mgr = new RefConsortDialogueMgr();
    public static RefConsortDialogueMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefConsortFettersLvl> getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefConsortDialogueMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefConsortFettersLvl newRef = (RefConsortFettersLvl) _newRef;
        lvl = newRef.lvl;
        consort_fetters_skill_lvl = newRef.consort_fetters_skill_lvl;
        need_consort_intimacy = newRef.need_consort_intimacy;
        need_consort_charm = newRef.need_consort_charm;
        need_consort_num = newRef.need_consort_num;
        adopt_child_quality_id = newRef.adopt_child_quality_id;
        graduate_get_item_num = newRef.graduate_get_item_num;
        income_bonus = newRef.income_bonus;
        study_bonus = newRef.study_bonus;
        caretaker_bonus_calculate_coefficient = newRef.caretaker_bonus_calculate_coefficient;
    }
    
    public static class RefConsortDialogueMgr extends RefTableContainer<RefConsortFettersLvl>
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
        return lvl;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public int lvl;
    public int consort_fetters_skill_lvl;//对应的羁绊技能等级
    public int need_consort_intimacy;//升到下一级需要的家人亲密度
    public int need_consort_charm;//升到下一级需要的家人加护力
    public int need_consort_num;//升到下一级需要的家人数量
    public long adopt_child_quality_id;//可收养学生资质(第一期不做子嗣，这里先配成string类型，后面要做了在改成id或者品质类型)
    public int graduate_get_item_num = 0;//获得的道具数量
    public long income_bonus;//收益天资加成（万分比）
    public int study_bonus;//教学经验加成（万分比）
    public int caretaker_bonus_calculate_coefficient;//监护者加成计算系数（万分比）
}

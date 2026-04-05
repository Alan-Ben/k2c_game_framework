package NPGameRes.Refs;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

@RefTable(tableName = "server_start_days")
public class RefServerStartDays extends RefBase
{
    private static RefServerStartDaysMgr _g_mgr = new RefServerStartDaysMgr();

    public static RefServerStartDaysMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefServerStartDaysMgr getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefServerStartDaysMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefServerStartDays newRef = (RefServerStartDays) _newRef;
        day = newRef.day;
        midday_dungeon_base_hero_exp = newRef.midday_dungeon_base_hero_exp;
        evening_dungeon_base_hero_exp = newRef.evening_dungeon_base_hero_exp;
        evening_dungeon_boss_blood_add_per = newRef.evening_dungeon_boss_blood_add_per;
    }

    public static class RefServerStartDaysMgr extends RefTableContainer<RefServerStartDays>
    {
        /**
         * 获取天数对应的配置对象，正序查找，如果输入天数大于配置天数则赋值，如果小于则跳出，返回数据
         * @param _day
         * @return
         */
        public RefServerStartDays getRefByDay(int _day)
        {
            RefServerStartDays lastRef = null;
            for (RefServerStartDays data : getList())
            {
                if (data.day == _day)
                {
                    return data;
                }
                if (data.day > _day)
                {
                    return lastRef;
                }
                lastRef = data;
            }
            return lastRef;
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
        return day;
    }

    //////////////////////////////

    public int day;//天数
    public long midday_dungeon_base_hero_exp;//午间副本基础伙伴经验
    public long evening_dungeon_base_hero_exp;//晚间副本基础伙伴经验
    public int evening_dungeon_boss_blood_add_per;//晚间副本boss血量增加万分比
}

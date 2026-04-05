package NPGameRes.Refs.Battle;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefListContainer;

/**
 * @author scott 地  图  信  息
 */
@RefTable(tableName = "actor_military_rank", isSingletonKey = false)
public class RefMilitaryRank extends RefBase
{
    private static RefListContainer<RefMilitaryRank> _g_mgr = new RefListContainer<RefMilitaryRank>();

    public static RefListContainer<RefMilitaryRank> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefListContainer<RefMilitaryRank> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefListContainer<RefMilitaryRank>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMilitaryRank newRef = (RefMilitaryRank) _newRef;
        id = newRef.id;
        level = newRef.level;
        military_lvl = newRef.military_lvl;
        is_show = newRef.is_show;
        icon = newRef.icon;
        property_list = newRef.property_list;
        skill_info_list = newRef.skill_info_list;
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
        return 0;
    }
    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long id;//卡牌ID
    public int level;//卡牌等级
    public int military_lvl;//前置建筑等级(没前置建筑的取自己的等级,等级为BUFFID为1000的层数)
    public boolean is_show;//是否显示
    public String icon;
    public String property_list;  //附加属性列表
    public String skill_info_list;//附加技能id


    // //////////////////////////////////////////////////////////////////////////////////////////////////////////////

}

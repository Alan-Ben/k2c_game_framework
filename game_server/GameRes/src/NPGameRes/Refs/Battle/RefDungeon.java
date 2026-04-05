package NPGameRes.Refs.Battle;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.ArrayList;

/**
 * @author scott 地  图  信  息
 */
@RefTable(tableName = "dungeon")
public class RefDungeon extends RefBase
{
    private static RefTableContainer<RefDungeon> _g_mgr = new RefTableContainer<RefDungeon>();

    public static RefTableContainer<RefDungeon> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefDungeon> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefDungeon>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefDungeon newRef = (RefDungeon) _newRef;
        id = newRef.id;
        map_id = newRef.map_id;
        time_scale = newRef.time_scale;
        select_actor = newRef.select_actor;
        select_hero = newRef.select_hero;
        check = newRef.check;
        can_use_house = newRef.can_use_house;
        battle_music = newRef.battle_music;
        race_sk_list = newRef.race_sk_list;
        add_race_sk_num = newRef.add_race_sk_num;
        dungeon_icon = newRef.dungeon_icon;
        dungeon_name = newRef.dungeon_name;
        special_type = newRef.special_type;
        quickBudilSearchRange = newRef.quickBudilSearchRange;
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

    public long map_id;

    public float time_scale;

    public String select_actor;

    public String select_hero;//BUff 增加的属性

    public boolean check;

    public boolean can_use_house;

    public long battle_music;

    public ArrayList<Long> race_sk_list;

    public int add_race_sk_num;

    public String dungeon_icon;

    public String dungeon_name;

    public String special_type;

    public int quickBudilSearchRange; //快速建造搜索半径

    // //////////////////////////////////////////////////////////////////////////////////////////////////////////////

}

package NPGameRes.Refs.Hero;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.ArrayList;
import java.util.List;

@RefTable(tableName = "hero_halo_suit")
public class RefHeroSuit extends RefBase
{
    private static RefTableContainer<RefHeroSuit> _g_mgr = new RefTableContainer<RefHeroSuit>();

    public static RefTableContainer<RefHeroSuit> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefHeroSuit> getStaticContainer()
    {
        return _g_mgr;
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefHeroSuit>) _mgr;
    }


    @Override
    public void resetRef(RefBase _newRef)
    {
        RefHeroSuit newRef = (RefHeroSuit) _newRef;
        id = newRef.id;
        halo_suit_skill_id_list = newRef.halo_suit_skill_id_list;
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

    public long id; // 唯一id
    public List<Long> halo_suit_skill_id_list; // 套系技能列表


    // 套件关联的大臣数据
    @RefField(isIgnore = true)
    private ArrayList<RefHero> _m_lHeroList = new ArrayList<>();
    public ArrayList<RefHero> getHeroList() { return _m_lHeroList; }
    public void setHeroList(ArrayList<RefHero> _list) { _m_lHeroList = _list; }
}

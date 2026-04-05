package NPGameRes.Refs.BagItem;

import Common.BagItemUseEnum.EBagItemUse_HeroType;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.PlayerVariable.NPPlayerVariableGroupObj;

@RefTable(tableName = "bag_item_hero")
public class RefBagItemHero extends RefBase
{
    private static RefTableContainer<RefBagItemHero> _g_mgr = new RefTableContainer<RefBagItemHero>();

    public static RefTableContainer<RefBagItemHero> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefBagItemHero> getStaticContainer()
    {
        return _g_mgr;
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefBagItemHero>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefBagItemHero newRef = (RefBagItemHero) _newRef;
        id = newRef.id;
        hero_id = newRef.hero_id;
        value = newRef.value;
        show_type = newRef.show_type;
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

    public long id;
    public NPPlayerVariableGroupObj hero_id;//大臣id
    public NPPlayerVariableGroupObj value;//值
    public EBagItemUse_HeroType show_type;//道具类型
}

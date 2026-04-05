package NPGameRes.Refs.Common;

import CommonEnum.ESpecAttrType;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.CommonObj.NPCommonItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

@RefTable(tableName = "basic_attr")
public class RefBasicAttr extends RefBase
{
    private static RefTableContainer<RefBasicAttr> _g_mgr = new RefTableContainer<RefBasicAttr>();

    public static RefTableContainer<RefBasicAttr> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefBasicAttr> getStaticContainer()
    {
        return _g_mgr;
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefBasicAttr>) _mgr;
    }


    @Override
    public void resetRef(RefBase _newRef)
    {
        RefBasicAttr newRef = (RefBasicAttr) _newRef;
        type = newRef.type;
        hero_aptitude_skill_upgrade_cost_item = newRef.hero_aptitude_skill_upgrade_cost_item;
        graduate_item = newRef.graduate_item;
    }

    @Override
    public long Id()
    {
        return type.ordinal();
    }

    public ESpecAttrType type = ESpecAttrType.NONE;//属性类型
    public NPCommonCostItem hero_aptitude_skill_upgrade_cost_item;//资质技能升级消耗(诏书)
    public NPCommonItem graduate_item = new NPCommonItem();//子嗣相性对应毕业奖励道具
}

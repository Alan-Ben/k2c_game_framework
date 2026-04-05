package NPGameRes.Refs.Hero;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.CommonObj.LevelObj._ILevelBasicObj;

@RefTable(tableName = "hero_skin_level")
public class RefHeroSkinLevel extends RefBase implements _ILevelBasicObj
{
    private static RefTableContainer<RefHeroSkinLevel> _g_mgr = new RefTableContainer<RefHeroSkinLevel>();

    public static RefTableContainer<RefHeroSkinLevel> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefHeroSkinLevel> getStaticContainer()
    {
        return _g_mgr;
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefHeroSkinLevel>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefHeroSkinLevel newRef = (RefHeroSkinLevel) _newRef;
        id = newRef.id;
        skin_id = newRef.skin_id;
        skin_level = newRef.skin_level;
        upgrade_cost = newRef.upgrade_cost;
    }

    /**
     * 获取等级
     * @return
     */
    public int getLevel(){return skin_level;}

    /**********
     * 获取对象数据Id，尽量唯一
     * @author scott
     * @time 2019年4月3日 下午11:35:24
     */
    @Override
    public long Id()
    {
        return id;
    }

    public long id; // 唯一id
    public long skin_id;//皮肤id
    public int skin_level;//皮肤等级
    public NPCommonCostItem upgrade_cost;//升级道具（CommonCostItem）
}

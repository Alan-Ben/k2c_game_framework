package NPGameRes.Refs.Hero;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.CommonObj.LevelObj._TLevelMapMgr;

import java.util.List;

@RefTable(tableName = "hero_skin")
public class RefHeroSkin extends RefBase
{
    private static RefTableContainer<RefHeroSkin> _g_mgr = new RefTableContainer<RefHeroSkin>();

    public static RefTableContainer<RefHeroSkin> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefHeroSkin> getStaticContainer()
    {
        return _g_mgr;
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefHeroSkin>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefHeroSkin newRef = (RefHeroSkin) _newRef;
        id = newRef.id;
        hero_id = newRef.hero_id;
        unlock_item = newRef.unlock_item;
        unlock_gain_item_list = newRef.unlock_gain_item_list;
        unlock_gain_talent_skill_id_list = newRef.unlock_gain_talent_skill_id_list;
    }

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
    public long hero_id;//大臣id
    public NPCommonCostItem unlock_item;//解锁道具（CommonCostItem）
    public List<NPCommonCostItem> unlock_gain_item_list; //解锁同时获得道具列表
    public List<Long> unlock_gain_talent_skill_id_list; //解锁时获得的资质技能id列表

    /**
     * 对应子数据等级的处理
     */
    @RefField(isIgnore = true)
    private _TLevelMapMgr<RefHeroSkinLevel> _m_lmLevelMapMgr = new _TLevelMapMgr<RefHeroSkinLevel>();

    public _TLevelMapMgr<RefHeroSkinLevel> getLevelMapMgr()
    {
        return _m_lmLevelMapMgr;
    }

    public void setLevelMapMgr(_TLevelMapMgr<RefHeroSkinLevel> _mgr)
    {
        _m_lmLevelMapMgr = _mgr;
    }
}

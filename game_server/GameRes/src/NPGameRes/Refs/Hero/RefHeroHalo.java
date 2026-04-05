package NPGameRes.Refs.Hero;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPCommon.Util.Pair.WCGPairLong;
import NPGameRes.GameObjs.CommonObj.LevelObj._TLevelMapMgr;

import java.util.List;

@RefTable(tableName = "hero_halo")
public class RefHeroHalo extends RefBase
{
    private static RefTableContainer<RefHeroHalo> _g_mgr = new RefTableContainer<RefHeroHalo>();

    public static RefTableContainer<RefHeroHalo> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefHeroHalo> getStaticContainer()
    {
        return _g_mgr;
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefHeroHalo>) _mgr;
    }


    @Override
    public void resetRef(RefBase _newRef)
    {
        RefHeroHalo newRef = (RefHeroHalo) _newRef;
        id = newRef.id;
        halo_suit_skill_extra_level = newRef.halo_suit_skill_extra_level;
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
    public List<WCGPairLong> halo_suit_skill_extra_level;//光环默认套系技能加成（未激活时也生效）

    /**
     * 对应子数据等级的处理
     */
    @RefField(isIgnore = true)
    private _TLevelMapMgr<RefHeroHaloLevel> _m_lmLevelMapMgr = new _TLevelMapMgr<RefHeroHaloLevel>();

    public _TLevelMapMgr<RefHeroHaloLevel> getLevelMapMgr()
    {
        return _m_lmLevelMapMgr;
    }

    public void setLevelMapMgr(_TLevelMapMgr<RefHeroHaloLevel> _mgr)
    {
        _m_lmLevelMapMgr = _mgr;
    }
}

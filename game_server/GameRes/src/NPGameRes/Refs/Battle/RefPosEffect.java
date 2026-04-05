package NPGameRes.Refs.Battle;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

/**
 * @author scott
 */
@RefTable(tableName = "pos_effect")
public class RefPosEffect extends RefBase
{
    private static RefTableContainer<RefPosEffect> _g_mgr = new RefTableContainer<RefPosEffect>();

    public static RefTableContainer<RefPosEffect> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefPosEffect> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefPosEffect>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefPosEffect newRef = (RefPosEffect) _newRef;
        id = newRef.id;
        sfx_id = newRef.sfx_id;
        single_cond_info = newRef.single_cond_info;
        effect_info_list = newRef.effect_info_list;
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

    public long sfx_id;

    public String single_cond_info;

    public String effect_info_list;
    // //////////////////////////////////////////////////////////////////////////////////////////////////////////////

}

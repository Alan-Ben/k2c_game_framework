package NPGameRes.Refs.AvatarGacha;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.Refs.AvatarGacha.Cond.GachaGuaranteeCondObj;

@RefTable(tableName = "gacha_guarantee")
public class RefGachaGuarantee extends RefBase
{
    private static RefTableContainer<RefGachaGuarantee> _g_mgr = new RefTableContainer<>();

    public static RefTableContainer<RefGachaGuarantee> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefGachaGuarantee> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefGachaGuarantee>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefGachaGuarantee newRef = (RefGachaGuarantee) _newRef;
        id = newRef.id;
        guarantee_times = newRef.guarantee_times;
        guarantee_reset_cond_list = newRef.guarantee_reset_cond_list;
        guarantee_roll_cond_list = newRef.guarantee_roll_cond_list;
        guarantee_need_new = newRef.guarantee_need_new;
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

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long id;//保底规则id
    public int guarantee_times;//保底次数
    public GachaGuaranteeCondObj guarantee_reset_cond_list;//刷新保底次数的道具条件(未满足就保底计数+1)
    public GachaGuaranteeCondObj guarantee_roll_cond_list;//获得道具过滤条件列表(1;2;3,条件3不满足则只过滤1和2条件,即条件有优先级,不满足则向上回退)
    public boolean guarantee_need_new;//保底是否必须随到未拥有的
}

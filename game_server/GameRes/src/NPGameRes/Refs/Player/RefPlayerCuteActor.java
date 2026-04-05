package NPGameRes.Refs.Player;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPEnum.ENPTimeAddType;
import NPEnum.EQuality;

/*********************
 * 段位信息表
 *
 * @author jeb
 *
 */
@RefTable(tableName = "cute_actor")
public class RefPlayerCuteActor extends _ARefExpiredItem
{
    private static RefTableContainer<RefPlayerCuteActor> _g_mgr = new RefTableContainer<RefPlayerCuteActor>();

    public static RefTableContainer<RefPlayerCuteActor> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefPlayerCuteActor> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefPlayerCuteActor>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefPlayerCuteActor newRef = (RefPlayerCuteActor) _newRef;
        id = newRef.id;
        quality = newRef.quality;
        add_type = newRef.add_type;
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
    ////////////////////////

    public long id;
    //品质
    public EQuality quality;
    //重复获得的时候，时间的叠加类型
    public ENPTimeAddType add_type;

    @Override
    public ENPTimeAddType getAddType()
    {
        return add_type;
    }
}

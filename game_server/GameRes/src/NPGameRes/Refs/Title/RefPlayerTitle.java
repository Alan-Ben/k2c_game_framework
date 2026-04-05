package NPGameRes.Refs.Title;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPEnum.ENPTimeAddType;
import NPGameRes.Refs.Player._ARefExpiredItem;


@RefTable(tableName = "player_title")
public class RefPlayerTitle extends _ARefExpiredItem
{
    private static RefTableContainer<RefPlayerTitle> _g_mgr = new RefTableContainer<RefPlayerTitle>();

    public static RefTableContainer<RefPlayerTitle> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefPlayerTitle> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefPlayerTitle>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefPlayerTitle newRef = (RefPlayerTitle) _newRef;
        id = newRef.id;
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
    //重复获得的时候，时间的叠加类型
    public ENPTimeAddType add_type;

    @Override
    public ENPTimeAddType getAddType()
    {
        return add_type;
    }
}

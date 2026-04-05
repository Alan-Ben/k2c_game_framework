package NPGameRes.Refs.Mail;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

@RefTable(tableName = "mail_type")
public class RefMailType extends RefBase
{
    private static RefTableContainer<RefMailType> _g_mgr = new RefTableContainer<RefMailType>();

    public static RefTableContainer<RefMailType> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefMailType> getStaticContainer()
    {
        return _g_mgr;
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefMailType>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMailType newRef = (RefMailType) _newRef;
        id = newRef.id;
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
}

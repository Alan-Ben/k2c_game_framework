package NPGameRes.Refs;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.NPPlayerProperty.NPPlayerPropertyModifier;


@RefTable(tableName = "player_forever_add")
public class RefPlayerForeverAdd extends RefBase
{
    private static RefTableContainer<RefPlayerForeverAdd> _g_mgr = new RefTableContainer<RefPlayerForeverAdd>();

    public static RefTableContainer<RefPlayerForeverAdd> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefPlayerForeverAdd> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefPlayerForeverAdd>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefPlayerForeverAdd newRef = (RefPlayerForeverAdd) _newRef;
        id = newRef.id;
        add_limit = newRef.add_limit;
        player_pro_add = newRef.player_pro_add;
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
    public int add_limit; //加成次数上限
    public NPPlayerPropertyModifier player_pro_add; //玩家属性加成
}

package NPGameRes.Refs;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.List;

@RefTable(tableName = "seven_day_login")
public class RefSevenDayLogin extends RefBase
{
    private static RefSevenDayLoginMgr _g_mgr = new RefSevenDayLoginMgr();

    public static RefSevenDayLoginMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefSevenDayLoginMgr getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefSevenDayLoginMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefSevenDayLogin newRef = (RefSevenDayLogin) _newRef;
        login_count = newRef.login_count;
        reward_item_list = newRef.reward_item_list;
    }

    public static class RefSevenDayLoginMgr extends RefTableContainer<RefSevenDayLogin>
    {

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
        return login_count;
    }

    //////////////////////////////

    public int login_count;//天数
    public List<NPCommonCostItem> reward_item_list;//奖励
}

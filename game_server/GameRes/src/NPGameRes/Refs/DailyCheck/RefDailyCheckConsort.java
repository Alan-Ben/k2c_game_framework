package NPGameRes.Refs.DailyCheck;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.List;

@RefTable(tableName = "daily_check_consort")
public class RefDailyCheckConsort extends RefBase
{
    private static RefTableContainer<RefDailyCheckConsort> _g_mgr = new RefTableContainer<>();

    public static RefTableContainer<RefDailyCheckConsort> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefDailyCheckConsort> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefDailyCheckConsort>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefDailyCheckConsort newRef = (RefDailyCheckConsort) _newRef;
        consort_id = newRef.consort_id;
        greeting_list = newRef.greeting_list;
        timeout_greeting_list = newRef.timeout_greeting_list;
        after_check_greeting_list = newRef.after_check_greeting_list;
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
        return consort_id;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long consort_id;//妃子id
    public List<String> greeting_list;//问候语
    public List<String> timeout_greeting_list;//长时间未登录问候语
    public List<String> after_check_greeting_list;//签到后文本
}
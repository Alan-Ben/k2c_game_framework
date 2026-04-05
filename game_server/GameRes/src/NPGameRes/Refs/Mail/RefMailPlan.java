package NPGameRes.Refs.Mail;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.ArrayList;

/**
 * RefMailPlan - 邮件计划配置表
 */
@RefTable(tableName = "mail_plan")
public class RefMailPlan extends RefBase
{
    private static RefTableContainer<RefMailPlan> _g_mgr = new RefTableContainer<RefMailPlan>();

    public static RefTableContainer<RefMailPlan> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefMailPlan> getStaticContainer()
    {
        return _g_mgr;
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefMailPlan>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMailPlan newRef = (RefMailPlan) _newRef;
        day = newRef.day;
        item_list = newRef.item_list;
    }

    /**
     * 获取对象数据Id，尽量唯一
     * @return 配置ID
     */
    @Override
    public long Id()
    {
        return day;
    }

    // 天数
    public int day;

    // 奖励物品列表
    public ArrayList<NPCommonCostItem> item_list;
}
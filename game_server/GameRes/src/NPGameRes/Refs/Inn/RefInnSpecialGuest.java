package NPGameRes.Refs.Inn;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;

import java.util.List;

@RefTable(tableName = "inn_special_guest")
public class RefInnSpecialGuest extends RefBase
{
    // 1. 静态管理器实例
    private static RefInnSpecialGuestMgr _g_mgr = new RefInnSpecialGuestMgr();

    // 2. 静态方法
    public static RefInnSpecialGuestMgr getMgr()
    {
        return _g_mgr;
    }

    // 3. 基类方法实现
    @Override
    public RefInnSpecialGuestMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefInnSpecialGuestMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefInnSpecialGuest newRef = (RefInnSpecialGuest) _newRef;
        id = newRef.id;
        unlock_condition_list = newRef.unlock_condition_list;
        handbook_reward = newRef.handbook_reward;
        first_gain_gift_id = newRef.first_gain_gift_id;
    }

    @Override
    public long Id()
    {
        return id;
    }

    // 4. 管理器内部类
    public static class RefInnSpecialGuestMgr extends RefTableContainer<RefInnSpecialGuest>
    {
        @Override
        protected void _onTableLoaded()
        {
            // 配表加载完成后的处理逻辑
        }
    }

    // 5. 配表字段定义
    public long id; // 客人ID（主键）
    public List<NPPlayerConditionGroupObj> unlock_condition_list; // 解锁条件
    public List<NPCommonCostItem> handbook_reward; // 图鉴奖励
    public long first_gain_gift_id; // 首次接待获得的珍宝奖励ID
}
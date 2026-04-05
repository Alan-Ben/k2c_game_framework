package NPGameRes.Refs.RechargeRebate;

import Common.RechargeRebateEnum.ERechargeRebateType;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

/**
 * 充值返利组配置表
 *
 * 对应表：recharge_rebate_group
 * 包含返利类型、关联活动ID、邮件模板ID等基础配置
 */
@RefTable(tableName = "recharge_rebate_group")
public class RefRechargeRebateGroup extends RefBase
{
    private static RefRechargeRebateGroupMgr _g_mgr = new RefRechargeRebateGroupMgr();

    public static RefRechargeRebateGroupMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefRechargeRebateGroupMgr getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefRechargeRebateGroupMgr) _mgr;
    }

    public static class RefRechargeRebateGroupMgr extends RefTableContainer<RefRechargeRebateGroup>
    {

    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefRechargeRebateGroup newRef = (RefRechargeRebateGroup) _newRef;
        id = newRef.id;
        type = newRef.type;
        mail_id = newRef.mail_id;
    }

    @Override
    public long Id()
    {
        return id;
    }

    // 组ID（主键）
    public long id;

    // 返利类型
    public ERechargeRebateType type;

    // 未领取奖励邮件模板ID
    public long mail_id;
}

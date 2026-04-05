package NPGameRes.Refs.Battle;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefListContainer;
import WCGCommon.Enum.NPEnum.EWCGOperationType;

/**
 * @author scott
 * 角  色  信  息
 */
@RefTable(tableName = "actor_lvl_operation", isSingletonKey = false)
public class RefActorLvlOperation extends RefBase
{
    private static RefListContainer<RefActorLvlOperation> _g_mgr = new RefListContainer<RefActorLvlOperation>();

    public static RefListContainer<RefActorLvlOperation> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefListContainer<RefActorLvlOperation> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefListContainer<RefActorLvlOperation>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefActorLvlOperation newRef = (RefActorLvlOperation) _newRef;
        id = newRef.id;
        level = newRef.level;
        operationid = newRef.operationid;
        icon = newRef.icon;
        actor_condition_list = newRef.actor_condition_list;
        actor_condition_list_show = newRef.actor_condition_list_show;
        choose_dis_isshow = newRef.choose_dis_isshow;
        cost_dis_isshow = newRef.cost_dis_isshow;
        delay_time = newRef.delay_time;
        delay_trigger_list = newRef.delay_trigger_list;
        cancel_trigger_list = newRef.cancel_trigger_list;
        can_cancel = newRef.can_cancel;
        cancel_return_list = newRef.cancel_return_list;
        cost_condition_list = newRef.cost_condition_list;
        click_trigger_list = newRef.click_trigger_list;
        share_team_show_condition = newRef.share_team_show_condition;
        operation_type = newRef.operation_type;
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
        return 0;
    }

    public long id;//actorId
    public int level;//等级
    public int operationid;  //操作id
    public String icon;//图标
    public String actor_condition_list;//操作角色条件
    public String actor_condition_list_show;

    public boolean choose_dis_isshow; //非选中状态是否显示
    public boolean cost_dis_isshow; //资源不满足是否显示
    public int delay_time;//操作效果延迟  毫秒
    public String delay_trigger_list;//效果列表
    public String cancel_trigger_list;//效果列表
    public boolean can_cancel;//可以取消
    public String cancel_return_list;
    public String cost_condition_list;
    public String click_trigger_list;
    public String share_team_show_condition;
    public EWCGOperationType operation_type;

    @Override
    public boolean Assert()
    {
        return true;
    }


}

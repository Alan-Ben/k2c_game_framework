package NPGameRes.GameObjs.Battle;

import NPGameRes.GameObjs.Battle.AITrigger.WCGAITriggerObj;
import NPGameRes.Refs.Battle.RefActorLvlOperation;
import WCGCommon.Enum.NPEnum.EWCGOperationType;

import java.util.List;

public class WCGActorOperation
{
    public int operationId;  //id
    public String icon;//图标
    public WCGSingleConditionGroupObj actor_condition_list;//生效条件 玩家点击以后才进行判定
    public WCGSingleConditionGroupObj actor_condition_list_show;//显示条件
    public WCGTeamConditionGroupObj share_team_show_condition;//显示的队伍条件
    public List<WCGActorCost> cost_condition_list;//资源条件
    public boolean choose_dis_isshow; //非选中状态是否显示
    public boolean cost_dis_isshow; //资源不满足是否显示
    public int delay_time;//效果延迟  毫秒
    public boolean can_cancel;//可以取消

    public List<WCGActorCost> cancel_return_list;//取消返还资源
    public List<WCGAITriggerObj> delay_trigger_list;//延迟触发事件列表
    public List<WCGAITriggerObj> click_trigger_list;//瞬间触发事件列表
    public List<WCGAITriggerObj> cancel_trigger_list;//触发事件列表

    public EWCGOperationType operation_type;//该操作类别----建造操作、3Doperation操作....
    //本地操作时，直接添加的效果Id，用于增加操作反馈感
    public long local_op_sfx;

    //条件不满足时提示的Key
    public String unmet_tip;


    public void adapt(RefActorLvlOperation refOpt)
    {
        this.operationId = refOpt.operationid;//id
        this.icon = refOpt.icon;//图标
        String errStr = String.format("3d 操作条件配置错误  actor_id:%d op_id:%d  str:%s"
                , this.operationId, this.operationId, refOpt.actor_condition_list);
        this.cost_condition_list = WCGActorCost.readCostList(refOpt.cost_condition_list);

        this.actor_condition_list = WCGSingleConditionGroupObj.readConditionGroupList(refOpt.actor_condition_list, errStr);//角色条件

        this.actor_condition_list_show = WCGSingleConditionGroupObj.readConditionGroupList(refOpt.actor_condition_list_show, errStr);
        this.share_team_show_condition = WCGTeamConditionGroupObj.readConditionGroupList(refOpt.share_team_show_condition, errStr);

        this.choose_dis_isshow = refOpt.choose_dis_isshow;
        this.cost_dis_isshow = refOpt.cost_dis_isshow;
        this.delay_time = refOpt.delay_time;//效果延迟毫秒
        this.can_cancel = refOpt.can_cancel;//可以取消

        this.cancel_return_list = WCGActorCost.readCostList(refOpt.cancel_return_list);
        this.delay_trigger_list = WCGAITriggerObj.readTriggerList(refOpt.delay_trigger_list);
        this.click_trigger_list = WCGAITriggerObj.readTriggerList(refOpt.click_trigger_list);
        this.cancel_trigger_list = WCGAITriggerObj.readTriggerList(refOpt.cancel_trigger_list);
        this.operation_type = refOpt.operation_type;


    }
}
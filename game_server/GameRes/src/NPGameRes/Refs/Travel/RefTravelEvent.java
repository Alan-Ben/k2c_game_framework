package NPGameRes.Refs.Travel;

import Common.TravelEnum.ETravelEventType;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;

import java.util.ArrayList;

/**
 * @author mark
 */
@RefTable(tableName = "travel_event")
public class RefTravelEvent extends RefBase
{
    private static RefTravelEventMgr _g_mgr = new RefTravelEventMgr();
    public static RefTravelEventMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTravelEventMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTravelEventMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefTravelEvent newRef = (RefTravelEvent) _newRef;
        event_id = newRef.event_id;
        effective_condition = newRef.effective_condition;
        event_type = newRef.event_type;
        rand_pro = newRef.rand_pro;
        rand_wei = newRef.rand_wei;
        event_item_list = newRef.event_item_list;
        gain_player_exp = newRef.gain_player_exp;
    }
    
    public static class RefTravelEventMgr extends RefTableContainer<RefTravelEvent>
    {
    	@Override
    	protected void _onTableLoaded()
        {
        }
    }

    /**********
     * 获取对象数据Id，尽量唯一
     */
    @Override
    public long Id()
    {
        return event_id;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long event_id;//事件id
    public NPPlayerConditionGroupObj effective_condition;
    public ETravelEventType event_type = ETravelEventType.NONE;//事件类型
    public int rand_pro;//事件绝对概率（万分比）
    public int rand_wei;//事件权重
    public ArrayList<NPCommonCostItem> event_item_list = new ArrayList<>();//事件奖励列表
    public long gain_player_exp;//获得玩家经验

    ////////////////////////// 子表对象
    @RefField(isIgnore = true)
    public RefTravelEventOnce onceEventRef;//一次性事件
    @RefField(isIgnore = true)
    public RefTravelEventAddPower addPowerEventRef;//大臣增加国力事件
    @RefField(isIgnore = true)
    public RefTravelEventChange changeEventRef;//交换事件
    @RefField(isIgnore = true)
    public RefTravelEventConsortBar consortBarEventRef;//妃子酒馆事件
    @RefField(isIgnore = true)
    public RefTravelEventConsortIntimacy consortIntimacyEventRef;//妃子亲密度事件
    @RefField(isIgnore = true)
    public RefTravelEventConsortLike consortLikeEventRef;//妃子好感度事件
    @RefField(isIgnore = true)
    public RefTravelEventGiftde giftdeEventRef;//必生卷娃事件
    @RefField(isIgnore = true)
    public RefTravelEventInvitation invitationEventRef;//妃子邀约事件
    @RefField(isIgnore = true)
    public RefTravelEventGamble gamblingEventRef;//博彩事件

    /**
     * 一键事件表：
     *   只有在玩家一键操作中才会检查本表，如果属于该表，则调用该表的奖励/效果，不处理事件表（及其子表）的奖励处理
     */
    @RefField(isIgnore = true) 
    public RefTravelEventAkey akeyEventRef;
}

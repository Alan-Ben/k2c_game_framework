package NPGameRes.InitDealer;

import NPCommon.Log.CommLog;
import NPGameRes.Refs.Travel.*;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

/**
 * 初始化游历配置
 */
public class TravelInitDealer extends _ABasicInitDealer
{
    @Override
    public void dealInit()
    {
        // ============== RefTravelPos
        for(int i = 0; i < RefTravelPos.getMgr().getList().size(); i++)
        {
            RefTravelPos posRef = RefTravelPos.getMgr().getList().get(i);
            if(null == posRef)
                continue;

            // 本地构建完整列表后原子替换，避免热更新时并发问题
            ArrayList<RefTravelEvent> newEventRefList = new ArrayList<>();
            for (Long eventId : posRef.travel_event_list)
            {
                RefTravelEvent eventRef = RefTravelEvent.getMgr().get(eventId);
                if(null == eventRef)
                {
                    CommLog.error("event:{} RefTravelPos init to RefTravelEvent fail, not find event ref.", eventId);
                    continue;
                }

                newEventRefList.add(eventRef);
            }
            posRef.eventRefList = newEventRefList;
        }

    	// ============== RefTravelEvent 游历事件
    	// 先构建 typeOrdinal -> 事件列表映射，再原子替换，避免热更新时并发问题
    	HashMap<Integer, ArrayList<RefTravelEvent>> typeEventMap = new HashMap<>();
    	for(int i = 0; i < RefTravelEvent.getMgr().getList().size(); i++)
    	{
    		RefTravelEvent ref = RefTravelEvent.getMgr().getList().get(i);
    		if(null == ref)
    			continue;

    		RefTravelEventType typeRef = RefTravelEventType.getMgr().get(ref.event_type.ordinal());
    		if(null == typeRef)
    		{
    			CommLog.error("event:{} type:{} RefTravelEvent init to RefTravelEventType fail, not find type ref.", ref.event_id, ref.event_type);
    			continue;
    		}

    		typeEventMap.computeIfAbsent(ref.event_type.ordinal(), k -> new ArrayList<>()).add(ref);
    	}
    	List<RefTravelEventType> typeRefList = RefTravelEventType.getMgr().getList();
    	for(int i = 0; i < typeRefList.size(); i++)
    	{
    		RefTravelEventType typeRef = typeRefList.get(i);
    		if(null == typeRef)
    			continue;
    		ArrayList<RefTravelEvent> newList = typeEventMap.get((int) typeRef.Id());
    		typeRef.eventRefList = newList != null ? newList : new ArrayList<>();
    	}
    	
    	// ============== RefTravelEventOnce
    	for(int i = 0; i < RefTravelEventOnce.getMgr().getList().size(); i++)
    	{
    		RefTravelEventOnce ref = RefTravelEventOnce.getMgr().getList().get(i);
    		if(null == ref)
    			continue;
    		
    		RefTravelEvent eventRef = RefTravelEvent.getMgr().get(ref.event_id);
    		if(null == eventRef)
    		{
    			CommLog.error("event:{} RefTravelEventOnce init to RefTravelEvent fail, not find event ref.", ref.event_id);
    			continue;
    		}
    		
    		eventRef.onceEventRef = ref;
    	}
    	
    	// ============== RefTravelEventAddPower
    	for(int i = 0; i < RefTravelEventAddPower.getMgr().getList().size(); i++)
    	{
    		RefTravelEventAddPower ref = RefTravelEventAddPower.getMgr().getList().get(i);
    		if(null == ref)
    			continue;
    		
    		RefTravelEvent eventRef = RefTravelEvent.getMgr().get(ref.event_id);
    		if(null == eventRef)
    		{
    			CommLog.error("event:{} RefTravelEventAddPower init to RefTravelEvent fail, not find event ref.", ref.event_id);
    			continue;
    		}
    		
    		eventRef.addPowerEventRef = ref;
    	}
    	
    	// ============== RefTravelEventChange
    	for(int i = 0; i < RefTravelEventChange.getMgr().getList().size(); i++)
    	{
    		RefTravelEventChange ref = RefTravelEventChange.getMgr().getList().get(i);
    		if(null == ref)
    			continue;
    		
    		RefTravelEvent eventRef = RefTravelEvent.getMgr().get(ref.event_id);
    		if(null == eventRef)
    		{
    			CommLog.error("event:{} RefTravelEventChange init to RefTravelEvent fail, not find event ref.", ref.event_id);
    			continue;
    		}
    		
    		eventRef.changeEventRef = ref;
    	}
    	
    	// ============== RefTravelEventConsortBar
    	for(int i = 0; i < RefTravelEventConsortBar.getMgr().getList().size(); i++)
    	{
    		RefTravelEventConsortBar ref = RefTravelEventConsortBar.getMgr().getList().get(i);
    		if(null == ref)
    			continue;
    		
    		RefTravelEvent eventRef = RefTravelEvent.getMgr().get(ref.event_id);
    		if(null == eventRef)
    		{
    			CommLog.error("event:{} RefTravelEventConsortBar init to RefTravelEvent fail, not find event ref.", ref.event_id);
    			continue;
    		}
    		
    		eventRef.consortBarEventRef = ref;
    	}
    	
    	// ============== RefTravelEventConsortIntimacy
    	for(int i = 0; i < RefTravelEventConsortIntimacy.getMgr().getList().size(); i++)
    	{
    		RefTravelEventConsortIntimacy ref = RefTravelEventConsortIntimacy.getMgr().getList().get(i);
    		if(null == ref)
    			continue;
    		
    		RefTravelEvent eventRef = RefTravelEvent.getMgr().get(ref.event_id);
    		if(null == eventRef)
    		{
    			CommLog.error("event:{} RefTravelEventConsortIntimacy init to RefTravelEvent fail, not find event ref.", ref.event_id);
    			continue;
    		}
    		
    		eventRef.consortIntimacyEventRef = ref;
    	}
    	
    	// ============== RefTravelEventConsortLike
    	for(int i = 0; i < RefTravelEventConsortLike.getMgr().getList().size(); i++)
    	{
    		RefTravelEventConsortLike ref = RefTravelEventConsortLike.getMgr().getList().get(i);
    		if(null == ref)
    			continue;
    		
    		RefTravelEvent eventRef = RefTravelEvent.getMgr().get(ref.event_id);
    		if(null == eventRef)
    		{
    			CommLog.error("event:{} RefTravelEventConsortLike init to RefTravelEvent fail, not find event ref.", ref.event_id);
    			continue;
    		}
    		
    		eventRef.consortLikeEventRef = ref;
    	}
    	
    	// ============== RefTravelEventGiftde
    	for(int i = 0; i < RefTravelEventGiftde.getMgr().getList().size(); i++)
    	{
    		RefTravelEventGiftde ref = RefTravelEventGiftde.getMgr().getList().get(i);
    		if(null == ref)
    			continue;
    		
    		RefTravelEvent eventRef = RefTravelEvent.getMgr().get(ref.event_id);
    		if(null == eventRef)
    		{
    			CommLog.error("event:{} RefTravelEventGiftde init to RefTravelEvent fail, not find event ref.", ref.event_id);
    			continue;
    		}
    		
    		eventRef.giftdeEventRef = ref;
    	}
    	
    	// ============== RefTravelEventInvitation
    	for(int i = 0; i < RefTravelEventInvitation.getMgr().getList().size(); i++)
    	{
    		RefTravelEventInvitation ref = RefTravelEventInvitation.getMgr().getList().get(i);
    		if(null == ref)
    			continue;
    		
    		RefTravelEvent eventRef = RefTravelEvent.getMgr().get(ref.event_id);
    		if(null == eventRef)
    		{
    			CommLog.error("event:{} RefTravelEventInvitation init to RefTravelEvent fail, not find event ref.", ref.event_id);
    			continue;
    		}
    		
    		eventRef.invitationEventRef = ref;
    	}
    	
    	// ============== RefTravelEventGamble
    	for(int i = 0; i < RefTravelEventGamble.getMgr().getList().size(); i++)
    	{
    		RefTravelEventGamble ref = RefTravelEventGamble.getMgr().getList().get(i);
    		if(null == ref)
    			continue;

    		RefTravelEvent eventRef = RefTravelEvent.getMgr().get(ref.event_id);
    		if(null == eventRef)
    		{
    			CommLog.error("event:{} RefTravelEventGamble init to RefTravelEvent fail, not find event ref.", ref.event_id);
    			continue;
    		}

    		eventRef.gamblingEventRef = ref;
    	}

    	// ============== RefTravelEventAkey
    	for(int i = 0; i < RefTravelEventAkey.getMgr().getList().size(); i++)
    	{
    		RefTravelEventAkey ref = RefTravelEventAkey.getMgr().getList().get(i);
    		if(null == ref)
    			continue;
    		
    		RefTravelEvent eventRef = RefTravelEvent.getMgr().get(ref.event_id);
    		if(null == eventRef)
    		{
    			CommLog.error("event:{} RefTravelEventAkey init to RefTravelEvent fail, not find event ref.", ref.event_id);
    			continue;
    		}
    		
    		eventRef.akeyEventRef = ref;
    	}
    }
}

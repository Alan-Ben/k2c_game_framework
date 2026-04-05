package NPGameRes.InitDealer;

import NPGameRes.LogicEvent.MetaData.EventMeta;
import NPGameRes.LogicEvent.MetaData.EventMetaMgr_Refdata;
import NPGameRes.Refs.Guild.RefGuildBoxEvent;
import NPGameRes.Refs.RefGeneral;

import java.util.HashMap;
import java.util.List;

public class GuildBoxInitDealer extends _ABasicInitDealer
{
    @Override
    public void dealInit()
    {
    	List<RefGuildBoxEvent> guildBoxEventRefList = RefGuildBoxEvent.getMgr().getList();
    	HashMap<Integer, RefGuildBoxEvent> guildBoxEventMap = new HashMap<>();
    	for(int i = 0; i < guildBoxEventRefList.size(); i++)
    	{
    		RefGuildBoxEvent ref = guildBoxEventRefList.get(i);
    		if(null == ref)
    			continue;
    		
    		String eventStr = ref.logic_event;
    		if(null == eventStr || eventStr.isEmpty())
    			continue;
    		
    		EventMeta eventMeta = EventMetaMgr_Refdata.getInstance().lookupMetaByEventName(eventStr.toUpperCase());
            if (eventMeta == null)
            	continue;
            
            guildBoxEventMap.put(eventMeta.getEventId(), ref);
    	}
    	RefGeneral.Ref().guildBoxEventMap = guildBoxEventMap;
    }
}

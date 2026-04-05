package NPGameRes.InitDealer;

import NPCommon.Log.CommLog;
import NPGameRes.Refs.ConsortChat.RefConsortChatAi;
import NPGameRes.Refs.ConsortChat.RefConsortChatAiCode;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class ConsortChatInitDealer extends _ABasicInitDealer
{
    @Override
    public void dealInit()
    {
        Map<Long, List<RefConsortChatAiCode>> chatCodeMap = new HashMap<>();

        List<RefConsortChatAiCode> list = RefConsortChatAiCode.getMgr().getList();
        for (RefConsortChatAiCode refConsortChatAiCode : list)
        {
            chatCodeMap.computeIfAbsent(refConsortChatAiCode.consort_id, k -> new ArrayList<>()).add(refConsortChatAiCode);
        }

        chatCodeMap.forEach((_consortId, _chatCodeList) ->
        {
            RefConsortChatAi refConsortChatAi = RefConsortChatAi.getMgr().get(_consortId);
            if (refConsortChatAi == null)
            {
                CommLog.error("ConsortChatInitDealer dealInit fail, not find consort chat ai ref, consortId:{}", _consortId);
                return;
            }

            refConsortChatAi.setChatCodeList(_chatCodeList);
        });
    }
}

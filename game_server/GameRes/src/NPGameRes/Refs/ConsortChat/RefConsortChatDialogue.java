package NPGameRes.Refs.ConsortChat;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.PlayerVariable.NPPlayerVariableGroupObj;

import java.util.List;

@RefTable(tableName = "consort_chat_dialogue")
public class RefConsortChatDialogue extends RefBase
{
    private static RefConsortChatDialogueMgr _g_mgr = new RefConsortChatDialogueMgr();
    public static RefConsortChatDialogueMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefConsortChatDialogue> getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefConsortChatDialogueMgr) _mgr;
    }

    public static class RefConsortChatDialogueMgr extends RefTableContainer<RefConsortChatDialogue>
    {
        @Override
        public void _onTableLoaded()
        {
        }
    }



    @Override
    public void resetRef(RefBase _newRef)
    {
        RefConsortChatDialogue newRef = (RefConsortChatDialogue) _newRef;
        id = newRef.id;
        consort_id = newRef.consort_id;
        process_cur_count = newRef.process_cur_count;
        goal_count = newRef.goal_count;
        event_list = newRef.event_list;
        reward_item_list = newRef.reward_item_list;
        reward_intimacy = newRef.reward_intimacy;
    }

    @Override
    public long Id()
    {
        return id;
    }

    public long id;
    public long consort_id;
    public NPPlayerVariableGroupObj process_cur_count;
    public long goal_count;
    public List<String> event_list;
    public List<NPCommonCostItem> reward_item_list;
    public long reward_intimacy;
}

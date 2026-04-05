package NPGameRes.Refs.ConsortChat;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

@RefTable(tableName = "consort_chat_ai_code")
public class RefConsortChatAiCode extends RefBase
{
    private static RefConsortChatAiCodeMgr _g_mgr = new RefConsortChatAiCodeMgr();
    public static RefConsortChatAiCodeMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefConsortChatAiCode> getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefConsortChatAiCodeMgr) _mgr;
    }

    public static class RefConsortChatAiCodeMgr extends RefTableContainer<RefConsortChatAiCode>
    {
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefConsortChatAiCode newRef = (RefConsortChatAiCode) _newRef;
        id = newRef.id;
        consort_id = newRef.consort_id;
        language = newRef.language;
        chat_code = newRef.chat_code;
        moment_content_code = newRef.moment_content_code;
        moment_respond_code = newRef.moment_respond_code;
        evaluate_reply_code = newRef.evaluate_reply_code;
    }

    @Override
    public long Id()
    {
        return id;
    }

    public long id;
    public long consort_id;
    public String language;
    public String chat_code;
    public String moment_content_code;
    public String moment_respond_code;
    public String evaluate_reply_code; // AI评价回复的chat code
}

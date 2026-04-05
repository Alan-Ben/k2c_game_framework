package NPGameRes.Refs.ConsortChat;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.Refs.RefGeneral;

import java.util.ArrayList;
import java.util.List;

@RefTable(tableName = "consort_chat_ai")
public class RefConsortChatAi extends RefBase
{
    private static RefConsortChatAiMgr _g_mgr = new RefConsortChatAiMgr();

    public static RefConsortChatAiMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefConsortChatAi> getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefConsortChatAiMgr) _mgr;
    }

    public static class RefConsortChatAiMgr extends RefTableContainer<RefConsortChatAi>
    {
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefConsortChatAi newRef = (RefConsortChatAi) _newRef;
        consort_id = newRef.consort_id;
        intimacy_count = newRef.intimacy_count;
    }

    @Override
    public long Id()
    {
        return consort_id;
    }

    public long consort_id;
    public long intimacy_count;//解锁ai对话的亲密度

    @RefField(isIgnore = true)
    private List<RefConsortChatAiCode> chatCodeList = new ArrayList<>();

    public void setChatCodeList(List<RefConsortChatAiCode> _chatCodeList)
    {
        chatCodeList = _chatCodeList;
    }

    /**
     * 获取聊天代码
     * @param _language
     * @return
     */
    public String getChatCode(String _language)
    {
        String chatCode = null;

        for (RefConsortChatAiCode refCode : chatCodeList)
        {
            // 当前还没有找到聊天代码（chatCode == null）且当前项语言匹配默认语言
            if ((chatCode == null && refCode.language.equals(RefGeneral.Ref().consort_ai_chat_default_language)))
                chatCode = refCode.chat_code;

            // 当前项语言匹配用户指定的语言
            if (refCode.language.equals(_language))
            {
                chatCode = refCode.chat_code;
                break;
            }
        }

        return chatCode;
    }

    /**
     * 获取聊天代码
     * @param _language
     * @return
     */
    public String getMomentCode(String _language, boolean _isMomentContent)
    {
        String chatCode = null;

        for (RefConsortChatAiCode refCode : chatCodeList)
        {
            // 当前还没有找到聊天代码（chatCode == null）且当前项语言匹配默认语言
            if ((chatCode == null && refCode.language.equals(RefGeneral.Ref().consort_ai_chat_default_language)))
                chatCode = _isMomentContent ? refCode.moment_content_code : refCode.moment_respond_code;

            // 当前项语言匹配用户指定的语言
            if (refCode.language.equals(_language))
            {
                chatCode = _isMomentContent ? refCode.moment_content_code : refCode.moment_respond_code;
                break;
            }
        }

        return chatCode;
    }

    /**
     * 获取评价回复的chat code
     *
     * 功能：根据用户语言获取用于AI评价朋友圈回复的chat code
     *
     * 执行流程：
     * 1. 遍历chatCodeList查找匹配的语言配置
     * 2. 优先返回用户指定语言的evaluate_reply_code
     * 3. 如果找不到则返回默认语言的evaluate_reply_code
     *
     * @param _language 用户语言代码（如"zh_CN"、"en_US"）
     * @return AI评价回复的chat code字符串，如果未配置则返回null
     */
    public String getEvaluateReplyCode(String _language)
    {
        String chatCode = null;

        for (RefConsortChatAiCode refCode : chatCodeList)
        {
            // 当前还没有找到聊天代码（chatCode == null）且当前项语言匹配默认语言
            if ((chatCode == null && refCode.language.equals(RefGeneral.Ref().consort_ai_chat_default_language)))
                chatCode = refCode.evaluate_reply_code;

            // 当前项语言匹配用户指定的语言
            if (refCode.language.equals(_language))
            {
                chatCode = refCode.evaluate_reply_code;
                break;
            }
        }

        return chatCode;
    }
}

package NPHttpServer.Http.Entity;

import NPHttpServer.Http.HttpService.AutoDecoder.JsonField;

/**
 * 通知AI消息添加
 */
public class EntityNotifyAiMsgAddData
{
    @JsonField(value = "content", required = true, description = "AI回复内容")
    private String _m_content;

    public String getContent()
    {
        return _m_content;
    }

    public void setContent(String _content)
    {
        _m_content = _content;
    }
}

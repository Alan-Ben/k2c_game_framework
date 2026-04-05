package NPHttpServer.Http.Entity;

import NPHttpServer.Http.HttpService.AutoDecoder.JsonField;

/**
 * 通知AI消息添加
 */
public class EntityNotifyAiMsgAdd
{
    @JsonField(value = "request_id", required = true, description = "请求id(唯一)")
    private long _m_requestId;

    @JsonField(value = "status", required = true, description = "状态：success:AI回复成功；fail:回复失败")
    private String _m_status;

    @JsonField(value = "code", required = true, description = "状态码")
    private int _m_code;

    @JsonField(value = "message", required = true, description = "失败时必填，失败原因描述。")
    private String _m_message;

    @JsonField(value = "data", required = true, description = "成功时：AI回复内容")
    private EntityNotifyAiMsgAddData _m_data;

    public long getRequestId()
    {
        return _m_requestId;
    }

    public void setRequestId(long _requestId)
    {
        _m_requestId = _requestId;
    }

    public String getStatus()
    {
        return _m_status;
    }

    public void setStatus(String _status)
    {
        _m_status = _status;
    }

    public int getCode()
    {
        return _m_code;
    }

    public void setCode(int _code)
    {
        _m_code = _code;
    }

    public String getMessage()
    {
        return _m_message;
    }

    public void setMessage(String _message)
    {
        _m_message = _message;
    }

    public EntityNotifyAiMsgAddData getData()
    {
        return _m_data;
    }

    public void setData(EntityNotifyAiMsgAddData _data)
    {
        _m_data = _data;
    }
}

package PayCenter.Http.Core.QuestNode;

import NPCommon.Http.HttpAsyncClient;
import PayCenter.Http.Core.EHttpContentType;
import PayCenter.Http.Core.PayCenterHttpServiceCore;
import PayCenter.Http.Core._IHttpCallBack;
import org.apache.http.message.BasicHeader;

import java.util.ArrayList;

/**
 * @description: 发送json类型数据
 * @author: ricci
 * @date: 2023-03-23 10:46:17
 */
public class NPOutBoundHttpQuestNode_PostJson extends _ANPOutBoundHttpQuestNode
{
    /**
     * 字符串内容
     */
    private final String _m_jsonStr;

    public NPOutBoundHttpQuestNode_PostJson(PayCenterHttpServiceCore _npHsHttpServiceCore, long _taskSerial,
                                            String _api, ArrayList<BasicHeader> _headerList, String _jsonObjStr,
                                            _IHttpCallBack _handler)
    {
        super(_npHsHttpServiceCore, _taskSerial, _api, _headerList, _handler);
        _m_jsonStr = _jsonObjStr;
    }

    @Override
    public EHttpContentType getContentType()
    {
        return EHttpContentType.JSON;
    }

    @Override
    public void send()
    {
        HttpAsyncClient.startHttpPostJson(getApi(), getHeaderParamList(), _m_jsonStr, this);
    }
}

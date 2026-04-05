package NPHttpServer.Http.Core.QuestNode;

import NPCommon.Http.HttpAsyncClient;
import NPHttpServer.Http.Core.ENPHttpContentType;
import NPHttpServer.Http.Core.NPHSHttpServiceCore;
import NPHttpServer.Http.Core._INPHttpCallBack;
import org.apache.http.message.BasicHeader;
import org.apache.http.message.BasicNameValuePair;

import java.util.ArrayList;

/**
 * @description: 发送 application/x-www-form-urlencoded 类型数据
 * @author: ricci
 * @date: 2023-03-23 10:46:17
 */
public class NPOutBoundHttpQuestNode_PostUrl extends _ANPOutBoundHttpQuestNode
{
    /**
     * 字符串内容
     */
    private final ArrayList<BasicNameValuePair> _m_basicNameValueList;

    public NPOutBoundHttpQuestNode_PostUrl(NPHSHttpServiceCore _npHsHttpServiceCore, long _taskSerial,
                                           String _api, ArrayList<BasicHeader> _headerList, ArrayList<BasicNameValuePair> _basicNameValue,
                                           _INPHttpCallBack _handler)
    {
        super(_npHsHttpServiceCore, _taskSerial, _api, _headerList, _handler);
        _m_basicNameValueList = _basicNameValue;
    }

    @Override
    public ENPHttpContentType getContentType()
    {
        return ENPHttpContentType.URL;
    }

    @Override
    public void send()
    {
        HttpAsyncClient.startHttpPost(getApi(), getHeaderParamList(), _m_basicNameValueList, this);
    }
}

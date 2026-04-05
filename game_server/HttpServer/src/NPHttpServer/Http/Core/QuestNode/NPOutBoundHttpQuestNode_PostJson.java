package NPHttpServer.Http.Core.QuestNode;

import NPCommon.Http.HttpAsyncClient;
import NPHttpServer.Http.Core.ENPHttpContentType;
import NPHttpServer.Http.Core.NPHSHttpServiceCore;
import NPHttpServer.Http.Core._INPHttpCallBack;
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

    public NPOutBoundHttpQuestNode_PostJson(NPHSHttpServiceCore _npHsHttpServiceCore, long _taskSerial,
                                            String _api, ArrayList<BasicHeader> _headerList, String _jsonObjStr,
                                            _INPHttpCallBack _handler)
    {
        super(_npHsHttpServiceCore, _taskSerial, _api, _headerList, _handler);
        _m_jsonStr = _jsonObjStr;
    }

    @Override
    public ENPHttpContentType getContentType()
    {
        return ENPHttpContentType.JSON;
    }

    @Override
    public void send()
    {
        HttpAsyncClient.startHttpPostJson(getApi(), getHeaderParamList(), _m_jsonStr, this);
    }
}

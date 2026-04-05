package PayCenter.Http.Core.QuestNode;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import NPCommon.ErrMain.HttpErr;
import NPCommon.Http._AResponseHandler;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import PayCenter.Http.Core.EHttpContentType;
import PayCenter.Http.Core.PayCenterHttpServiceCore;
import PayCenter.Http.Core.Task.NPHttpSynTask_QuestDone;
import PayCenter.Http.Core._IHttpCallBack;
import org.apache.http.Header;
import org.apache.http.message.BasicHeader;

import java.util.ArrayList;

/**
 * @description: 出站http请求任务管理节点
 * @author: ricci
 * @date: 2023-03-23 09:47:57
 */
public abstract class _ANPOutBoundHttpQuestNode extends _AResponseHandler
{
    private final PayCenterHttpServiceCore _m_npHsHttpServiceCore;
    /**
     * api路径
     */
    private final String _m_api;
    /**
     * 请求参数
     */
    private final ArrayList<BasicHeader> _m_headerParamList;

    /**
     * callback处理
     */
    private final _IHttpCallBack _m_httpCallBack;

    /**
     * 创建时间戳
     */
    private long _m_createTime;

    /**
     * 任务序列号
     */
    private long _m_taskSerial;

    public _ANPOutBoundHttpQuestNode(PayCenterHttpServiceCore _npHsHttpServiceCore, long _taskSerial, String _api,
                                     ArrayList<BasicHeader> _headerList, _IHttpCallBack _handler)
    {

        _m_npHsHttpServiceCore = _npHsHttpServiceCore;
        _m_taskSerial = _taskSerial;
        _m_api = _api;
        _m_headerParamList = _headerList;
        _m_httpCallBack = _handler;
        _m_createTime = CommonFunc.getNowTimeMS();
    }

    public PayCenterHttpServiceCore getNpHsHttpServiceCore()
    {
        return _m_npHsHttpServiceCore;
    }

    public String getApi()
    {
        return _m_api;
    }

    public ArrayList<BasicHeader> getHeaderParamList()
    {
        return _m_headerParamList;
    }

    public _IHttpCallBack getHttpCallBack()
    {
        return _m_httpCallBack;
    }

    public long getCreateTime()
    {
        return _m_createTime;
    }

    public void setCreateTime(long _m_createTime)
    {
        this._m_createTime = _m_createTime;
    }

    public Long getHttpQuestSerial()
    {
        return _m_taskSerial;
    }

    @Override
    public void onComplete(Header[] headers, int _code, String _response)
    {
        //通知
        ALSynTaskManager.getInstance().regTask(new NPHttpSynTask_QuestDone(getNpHsHttpServiceCore(), this));
        _m_httpCallBack.onSuc(headers, _code, _response);
    }

    @Override
    public void onFailed(Exception e)
    {
        CommLog.error("http deal fail Exception:{}", e);
        ALSynTaskManager.getInstance().regTask(new NPHttpSynTask_QuestDone(getNpHsHttpServiceCore(), this));
        _m_httpCallBack.onFail(HttpErr.HTTP_RESPONSE_ERROR);
    }

    /**
     * 发送请求的数据类型
     * @return ENPHttpContentType
     */
    public abstract EHttpContentType getContentType();

    /**
     * 执行发送
     */
    public abstract void send();


}

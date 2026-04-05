package NPHttpServer.Http.Core;

import ALBasicCommon.ALSerializeMaker;
import ALBasicServer.ALBasicMutex.MutexAtom;
import NPCommon.Http.HttpService.server.HttpDispather;
import NPCommon.Http.HttpService.server.NPHttpService;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPHttpServer.Http.Core.QuestNode.NPOutBoundHttpQuestNode_PostJson;
import NPHttpServer.Http.Core.QuestNode.NPOutBoundHttpQuestNode_PostUrl;
import NPHttpServer.Http.Core.QuestNode._ANPOutBoundHttpQuestNode;
import NPHttpServer.Http.Core.Task.NPHttpSynTask_Tick;
import NPHttpServer.Http.HttpContorller.NPHSHttpController;
import com.sun.net.httpserver.HttpServer;
import org.apache.http.message.BasicHeader;
import org.apache.http.message.BasicNameValuePair;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.LinkedList;

/**
 * @description: http 服务器启动器
 * @author: ricci
 * @date: 2023-03-22 17:36:12
 */
public class NPHSHttpServiceCore
{
    /**
     * http服务
     */
    private HttpServer _m_httpService;
    /**
     * http消息处理器
     */
    private HttpDispather dispather = new HttpDispather();

    /**
     * 出站Http请求待发送任务队列
     */
    private LinkedList<_ANPOutBoundHttpQuestNode> _m_outBoundQuestNodeList;
    /**
     * 等待回包的任务集合
     */
    private HashMap<Long, _ANPOutBoundHttpQuestNode> _m_waitResponseQuestNodeMap;

    /**
     * 任务队列锁
     */
    private MutexAtom _m_mutex;

    /**
     * 发送请求的处理任务
     */
    private NPHttpSynTask_Tick _m_dealTask;

    /**
     * 是否已经初始化
     */
    private boolean _m_hasInit;

    //////单例的//////
    private static final NPHSHttpServiceCore _s_instance = new NPHSHttpServiceCore();

    public static NPHSHttpServiceCore getInstance()
    {
        return _s_instance;
    }

    private NPHSHttpServiceCore()
    {
        _m_mutex = new MutexAtom();
        _m_outBoundQuestNodeList = new LinkedList<>();
        _m_waitResponseQuestNodeMap = new HashMap<>();
        _m_dealTask = new NPHttpSynTask_Tick(this);
    }

    protected void _lock()
    {
        _m_mutex.lock();
    }

    protected void _unlock()
    {
        _m_mutex.unlock();
    }

    public boolean isInit()
    {
        return _m_hasInit;
    }

    /**
     * 开启http服务
     * @param _port      指定对外端口
     * @param _dispather 消息处理器
     * @return 是否初始化成功
     */
    public boolean init(int _port)
    {
        //已经初始化不允许重复初始化
        if (_m_hasInit)
        {
            return false;
        }
        _m_hasInit = true;
        //创建新的service
        try
        {
            dispather = new HttpDispather();
            //获取指定包路径
            String pack = NPHSHttpController.class.getPackage().getName();
            if (!dispather.init(pack))
            {
                CommLog.error("HS Http init dispatcher failed:{}", pack);
                return false;
            }
            _m_httpService = NPHttpService.createServer(_port, dispather, "/");
        } catch (Exception e)
        {
            CommLog.error("HS Http Service init failed! ", e);
            return false;
        }
        //开启http服务
        CommLog.info("try start httpServer of HttpServer...");
        _m_httpService.start();
        CommLog.info("start over httpServer of HttpServer...");

        //开启处理任务
        _m_dealTask.start();

        return true;
    }

    private long __reqSerial()
    {
        return ALSerializeMaker.makeNewSerialize();
    }

    /**
     * 增加等待执行任务
     * @param _questNode 任务node
     */
    private void __addQuestNode(_ANPOutBoundHttpQuestNode _questNode)
    {
        if (_questNode == null)
        {
            return;
        }
        _lock();
        try
        {
            _m_outBoundQuestNodeList.push(_questNode);
        } finally
        {
            _unlock();
        }
    }

    /**
     * tick处理发送队列
     */
    public void tick()
    {
        _lock();
        try
        {
            if (_m_waitResponseQuestNodeMap.size() > 3000)
            {
                //等待回应的任务堆积超过 3000 报警
                CommLog.error("NPHSHttpServiceCore wait response http request task more than 3000...");
                //查找阻塞原因，开销时间最长的api

            }

            //首个出队
            while (!_m_outBoundQuestNodeList.isEmpty())
            {
                _ANPOutBoundHttpQuestNode pop = _m_outBoundQuestNodeList.pop();
                //将该任务存放到待回包任务组中
                _m_waitResponseQuestNodeMap.put(pop.getHttpQuestSerial(), pop);

                //调用任务的send方法，发送http请求
                pop.send();
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 通知 httpCore 任务完成,需要从待响应的队列中移除
     * @param _npOutBoundHttpQuestNode 已响应任务
     */
    public void questDone(_ANPOutBoundHttpQuestNode _npOutBoundHttpQuestNode)
    {
        _lock();
        try
        {
            if (_npOutBoundHttpQuestNode == null)
            {
                return;
            }
            _ANPOutBoundHttpQuestNode questNode = _m_waitResponseQuestNodeMap.remove(_npOutBoundHttpQuestNode.getHttpQuestSerial());
            if (questNode == null)
            {
                //不存在任务
                CommLog.error("NPHSHttpServiceCore questDone remove not found serial:{}"
                        , _npOutBoundHttpQuestNode.getHttpQuestSerial());
                return;
            }
            long nowTimeMS = CommonFunc.getNowTimeMS();
            long costTimeMs = nowTimeMS - questNode.getCreateTime();
            //花费10秒以上
            if (costTimeMs > 10000)
            {
                CommLog.info("NPHSHttpServiceCore questDone remove cost timeMs:{} api:{}"
                        , costTimeMs, questNode.getApi());
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 发送post请求
     * @param _api         发送api
     * @param _headerParamList   请求参数
     * @param _jsonObjStr 请求数据格式
     * @param _callBack    回包
     */
    public void httpPostJson(String _api, ArrayList<BasicHeader> _headerParamList, String _jsonObjStr, _INPHttpCallBack _callBack)
    {
        NPOutBoundHttpQuestNode_PostJson questNode = new NPOutBoundHttpQuestNode_PostJson(this,
                __reqSerial(), _api, _headerParamList, _jsonObjStr, _callBack);
        __addQuestNode(questNode);
    }

    public void httpPostUrlEntity(String _api, ArrayList<BasicHeader> _headerParamList,
                                  ArrayList<BasicNameValuePair> _paramList, _INPHttpCallBack _callBack)
    {
        NPOutBoundHttpQuestNode_PostUrl questNode = new NPOutBoundHttpQuestNode_PostUrl(this,
                __reqSerial(), _api, _headerParamList, _paramList, _callBack);
        __addQuestNode(questNode);
    }
}

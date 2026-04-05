package NPCommon.Http.HttpService.server;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import NPCommon.Http.HttpService.annotation.RequestMapping;
import NPCommon.Log.CommLog;

import java.lang.reflect.Method;

public class HttpAdapter
{

    private Object instance = null;
    private Method method = null;
    private String uri = "";

    public HttpAdapter(Object instance, Method method)
    {
        super();
        this.instance = instance;
        this.method = method;
        RequestMapping mapping = method.getAnnotation(RequestMapping.class);
        uri = mapping.uri();
    }

    public void invoke(HttpRequest request, HttpResponse response)
    {
        CommLog.info("{} {} {}", uri, request.getHttpMethod(), request.getPostBody());
        ALSynTaskManager.getInstance().regTask(new _IALSynTask()
        {

            @Override
            public void run()
            {
                try
                {
                    method.invoke(instance, request, response);
                } catch (Exception e)
                {
                    response.error(300001, "服务器发生未知错误，错误信息：%s", e.getMessage());
                    CommLog.error("handle request error", e);
                }
            }

        });

    }

    public String getUri()
    {
        return uri;
    }
}

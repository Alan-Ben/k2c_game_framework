package NPCommon.Http.HttpService.server;


import NPCommon.Http.HttpService.annotation.RequestMapping;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommClass;
import WCGCommon.Exception.RequestException;
import com.sun.net.httpserver.HttpExchange;
import com.sun.net.httpserver.HttpHandler;

import java.io.IOException;
import java.lang.reflect.Method;
import java.util.HashMap;
import java.util.Map;
import java.util.Set;

public class HttpDispather implements HttpHandler
{

    private final Map<HttpMethod, MethodAdapater> methodAdapters = new HashMap<>();

    public boolean init(String pack)
    {
        Set<Class<?>> dealers = CommClass.getClasses(pack);

        for (Class<?> cs : dealers)
        {
            Object instance = null;
            for (Method method : cs.getMethods())
            {
                RequestMapping mapping = method.getAnnotation(RequestMapping.class);
                if (mapping == null)
                {
                    continue;
                }
                Class<?>[] params = method.getParameterTypes();
                if (params.length != 2)
                {
                    CommLog.error("[" + method.getName() + "]不是固定2个参数");
                    return false;
                }
                if (params[0] != HttpRequest.class)
                {
                    CommLog.error("[" + method.getName() + "]第一个参数不是HttpRequest");
                    return false;
                }
                if (params[1] != HttpResponse.class)
                {
                    CommLog.error("[" + method.getName() + "]第二个参数不是HttpResponse");
                    return false;
                }
                if (instance == null)
                {
                    try
                    {
                        instance = CommClass.forName(cs.getName()).newInstance();
                    } catch (Exception e)
                    {
                        CommLog.error("create instance for ：{} error", cs.getName(), e);
                        return false;
                    }

                }
                for (HttpMethod httpMethod : mapping.method())
                {
                    if (methodAdapters.get(httpMethod) == null)
                    {
                        methodAdapters.put(httpMethod, new MethodAdapater());
                    }
                    methodAdapters.get(httpMethod).addAdapter(mapping.uri(), new HttpAdapter(instance, method));
                }
            }
        }
        return true;
    }

    @Override
    public void handle(HttpExchange exchange) throws IOException
    {
        HttpResponse response = new HttpResponse(exchange);
        try
        {
            HttpMethod method = null;
            try
            {
                String methodName = exchange.getRequestMethod().trim().toUpperCase();
                method = HttpMethod.valueOf(methodName);
            } catch (Exception e)
            {
                method = HttpMethod.GET;
            }
            MethodAdapater methodAdapater = methodAdapters.get(method);
            if (methodAdapater == null)
            {
                response.response(404, "File Not Found");
                return;
            }
            HttpAdapter adaperter = methodAdapater.getAdapter(exchange.getRequestURI().getPath());
            if (adaperter == null)
            {
                response.response(404, "File Not Found");
                return;
            }
            HttpRequest request = new HttpRequest(exchange);
            adaperter.invoke(request, response);
        } catch (Exception e)
        {
            Throwable cause = e.getCause();
            if (cause instanceof RequestException)
            {
                RequestException re = (RequestException) cause;
                response.error(re.getCode(), re.getMessage());
            } else
            {
                response.error(300001, "服务器发生未知错误，错误信息：%s", e.getMessage());
            }
        }
    }
}

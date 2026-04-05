package NPCommon.Http.HttpService.server;


import NPCommon.Log.CommLog;
import NPCommon.Util.HttpUtils;
import com.sun.net.httpserver.HttpExchange;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.URI;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class HttpRequest
{

    private final HttpExchange httpExchange;
    private Map<String, String> params = null;
    private final Map<String, List<String>> headMap = new HashMap<String, List<String>>();
    private String requestPostBody = "";
    private final HttpMethod _m_HttpMethod;

    public HttpRequest(HttpExchange httpExchange)
    {
        String methodname = httpExchange.getRequestMethod().trim().toUpperCase();
        _m_HttpMethod = HttpMethod.valueOf(methodname);

        this.httpExchange = httpExchange;
        headMap.putAll(httpExchange.getRequestHeaders());
        this.params = HttpUtils.abstractHttpParams(this.getReuestURI().getQuery());
        this.initRequestBody();

    }

    public Map<String, String> getParams()
    {
        return params;
    }

    public HttpMethod getHttpMethod()
    {
        return _m_HttpMethod;
    }

    public URI getReuestURI()
    {
        return httpExchange.getRequestURI();
    }

    public String getQueryParam(String key)
    {
        return params.get(key);
    }

    public String getQueryString()
    {
        return this.getReuestURI().getQuery();
    }


    /**
     * 一个 Head 会对应 <b>多个值</b>，该接口只取列表中的第一个返回
     * @return 如果head找不到为null, 有值的话，只取第一个
     */
    public String getHeader(String key)
    {
        List<String> head = httpExchange.getRequestHeaders().get(key);
        if (head == null || head.size() == 0)
        {
            return null;
        }
        return head.get(0);
    }

    /**
     * 一个 Head 会对应 <b>多个值</b>，该接口以列表的形式返回
     * @return 如果head找不到为null
     */
    public List<String> getHeaderList(String key)
    {
        return httpExchange.getRequestHeaders().get(key);
    }

    private void initRequestBody()
    {
        InputStream in = httpExchange.getRequestBody(); // 获得输入流
        StringBuilder builder = new StringBuilder();
        BufferedReader reader = null;
        try
        {
            String line = null;
            reader = new BufferedReader(new InputStreamReader(in));
            while ((line = reader.readLine()) != null)
            {
                builder.append(line);
            }
        } catch (IOException e)
        {
            CommLog.error("[HttpRequest]解析http协议body发生错误");
        } finally
        {
            if (reader != null)
            {
                try
                {
                    reader.close();
                } catch (IOException e)
                {
                    CommLog.error("[HttpRequest]解析http关闭输入流时错误");
                }
            }
        }
        requestPostBody = builder.toString();
    }

    public String getPostBody()
    {
        return requestPostBody;
    }
}

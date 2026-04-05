package NPCommon.Http;

import org.apache.http.HttpResponse;
import org.apache.http.client.config.RequestConfig;
import org.apache.http.client.entity.UrlEncodedFormEntity;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.client.methods.HttpRequestBase;
import org.apache.http.concurrent.FutureCallback;
import org.apache.http.entity.StringEntity;
import org.apache.http.entity.mime.MultipartEntityBuilder;
import org.apache.http.impl.nio.client.CloseableHttpAsyncClient;
import org.apache.http.impl.nio.client.HttpAsyncClients;
import org.apache.http.message.BasicHeader;
import org.apache.http.message.BasicNameValuePair;

import java.util.ArrayList;

/**
 * Http异步请求
 * @author colin
 */
public class HttpAsyncClient
{
    static CloseableHttpAsyncClient httpclient = null;

    public static void init()
    {
        if (httpclient == null)
        {
            RequestConfig requestConfig = RequestConfig.custom().setSocketTimeout(60000).setConnectTimeout(60000).build();
            httpclient = HttpAsyncClients.custom().setDefaultRequestConfig(requestConfig).build();
            httpclient.start();
        }
    }

    public static void start(HttpRequestBase request, FutureCallback<HttpResponse> response)
    {
        init();
        httpclient.execute(request, response);
    }

    public static void startHttpGet(String request, FutureCallback<HttpResponse> response)
    {
        init();
        httpclient.execute(new HttpGet(request), response);
    }

    public static void startHttpGet(String request, final _AResponseHandler response)
    {
        init();
        final HttpGet httpRequest = new HttpGet(request);
        httpclient.execute(httpRequest, response);
    }

    public static void startHttpPost(final HttpPost httpRequest, final _AResponseHandler response)
    {
        init();
        httpclient.execute(httpRequest, response);
    }

    /**
     * 发送 url格式数据
     * @param _url
     * @param _headers
     * @param _postParam
     * @param response
     */
    public static void startHttpPost(String _url, ArrayList<BasicHeader> _headers,
                                     ArrayList<BasicNameValuePair> _postParam,
                                     final _AResponseHandler response)
    {
        HttpPost httpPost = new HttpPost(_url);
        //设置请求头
        for (BasicHeader header : _headers)
        {
            httpPost.addHeader(header);
        }
        try
        {
            //创建请求信息
            UrlEncodedFormEntity urlEncodedFormEntity = new UrlEncodedFormEntity(_postParam);
            httpPost.setEntity(urlEncodedFormEntity);
        } catch (Exception e)
        {
            e.printStackTrace();
            return;
        }

        startHttpPost(httpPost, response);
    }

    /**
     * 发送formdata格式数据
     * @param _url
     * @param _headers
     * @param _postParam
     * @param response
     */
    public static void startHttpPostFromData(String _url,
                                             ArrayList<BasicNameValuePair> _postParam,
                                             final _AResponseHandler response)
    {
        HttpPost httpPost = new HttpPost(_url);
        //设置请求参数，生成请求实例
        MultipartEntityBuilder builder = MultipartEntityBuilder.create();
        for (BasicNameValuePair basicNameValuePair : _postParam)
        {
            builder.addTextBody(basicNameValuePair.getName(), basicNameValuePair.getValue());
        }
        httpPost.setEntity(builder.build());

        startHttpPost(httpPost, response);
    }

    /**
     * 发送json格式数据
     * @param _url
     * @param _headers
     * @param _bodyJson
     * @param response
     */
    public static void startHttpPostJson(String _url, ArrayList<BasicHeader> _headers,
                                         final String _bodyJson,
                                         final _AResponseHandler response)
    {
        HttpPost httpPost = new HttpPost(_url);
        //设置请求头
        for (BasicHeader header : _headers)
        {
            httpPost.addHeader(header);
        }
        //设置请求实例
        StringEntity reqStr = new StringEntity(_bodyJson, "UTF-8");
        httpPost.setEntity(reqStr);

        startHttpPost(httpPost, response);
    }
}

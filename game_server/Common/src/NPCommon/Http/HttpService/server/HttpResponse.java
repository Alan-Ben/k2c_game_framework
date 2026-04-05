package NPCommon.Http.HttpService.server;

import NPCommon.Log.CommLog;
import com.sun.net.httpserver.HttpExchange;

import java.io.*;

public class HttpResponse implements _IResponse
{
    private final HttpExchange httpExchange;

    public HttpResponse(HttpExchange httpExchange)
    {
        this.httpExchange = httpExchange;
    }

    @Override
    public void response(String result)
    {
        response(200, result);
    }

    @Override
    public void response(int code, String result)
    {
        try
        {
            httpExchange.sendResponseHeaders(code, result.getBytes().length);// 设置响应头属性及响应信息的长度
            OutputStream out = httpExchange.getResponseBody(); // 获得输出流
            out.write(result.getBytes());
            out.flush();
            httpExchange.close();
        } catch (IOException e)
        {
            CommLog.error("回写Http数据response时发生错误", e);
        }
    }

    public void response(File file)
    {
        try (InputStream input = new FileInputStream(file))
        {
            byte[] byt = new byte[input.available()];
            input.read(byt);

            httpExchange.sendResponseHeaders(200, byt.length);// 设置响应头属性及响应信息的长度
            OutputStream out = httpExchange.getResponseBody(); // 获得输出流
            out.write(byt);
            out.flush();
            httpExchange.close();
        } catch (IOException e)
        {
            CommLog.error("回写Http数据response时发生错误", e);
        }
    }

    @Override
    public void error(int code, String format, Object... param)
    {
        String msg = String.format(format, param);
        String rep = String.format("{\"state\":%d,\"msg\":\"%s\"}", code, this.encodeString(msg));
        this.response(rep);
        CommLog.error("{}请求处理失败,错误码:{},msg:{}", httpExchange.getRequestURI(), code, msg);
    }

    private String encodeString(String str)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < str.length(); i++)
        {
            char ch = str.charAt(i);
            switch (ch)
            {
                case '\\':
                    sb.append("\\\\");
                    break;
                case '/':
                    sb.append("\\/");
                    break;
                case '"':
                    sb.append("\\\"");
                    break;
                case '\t':
                    sb.append("\\t");
                    break;
                case '\f':
                    sb.append("\\f");
                    break;
                case '\b':
                    sb.append("\\b");
                    break;
                case '\n':
                    sb.append("\\n");
                    break;
                case '\r':
                    sb.append("\\r");
                    break;
                default:
                {
                    sb.append(ch);
                    break;
                }
            }
        }
        return sb.toString();
    }
}

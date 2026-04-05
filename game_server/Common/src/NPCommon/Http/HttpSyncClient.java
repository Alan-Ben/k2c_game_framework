package NPCommon.Http;

import NPCommon.Log.CommLog;

import java.io.*;
import java.net.*;
import java.util.Map;
import java.util.Map.Entry;

public class HttpSyncClient
{
    /***************************
     * 向指定URL发送GET方法的请求
     *
     * @param url
     *            发送请求的URL
     * @param param
     *            请求参数，请求参数应该是 name1=value1&name2=value2 的形式。
     * @return URL 所代表远程资源的响应结果
     */
    public static String sendGet(String url, String param)
    {
        String result = "";
        BufferedReader in = null;
        try
        {
            String urlNameString = url + "?" + param;
            URL realUrl = new URL(urlNameString);
            // 打开和URL之间的连接
            URLConnection connection = realUrl.openConnection();
            // 设置通用的请求属性
            connection.setRequestProperty("accept", "*/*");
            connection.setRequestProperty("connection", "Keep-Alive");
            connection.setRequestProperty("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1;SV1)");
            // 建立实际的连接
            connection.connect();
            // 定义 BufferedReader输入流来读取URL的响应
            in = new BufferedReader(new InputStreamReader(connection.getInputStream()));
            String line;
            while ((line = in.readLine()) != null)
            {
                result += line;
            }
        } catch (Exception e)
        {
            CommLog.error("发送GET请求[{}]出现异常！", url, e);
        } finally
        {
            try
            {
                if (in != null)
                {
                    in.close();
                }
            } catch (Exception e2)
            {
            }
        }
        return result;
    }

    /***************************
     * 向指定 URL 发送POST方法的请求
     *
     * @param url
     *            发送请求的 URL
     * @param param
     *            请求参数，请求参数应该是 name1=value1&name2=value2 的形式。
     * @return 所代表远程资源的响应结果
     */
    public static String sendPost(String url, String param)
    {
        PrintWriter out = null;
        BufferedReader in = null;
        String result = "";
        try
        {
            URL realUrl = new URL(url);
            // 打开和URL之间的连接
            URLConnection conn = realUrl.openConnection();
            // 设置通用的请求属性
            conn.setRequestProperty("accept", "*/*");
            conn.setRequestProperty("connection", "Keep-Alive");
            conn.setRequestProperty("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1;SV1)");
            // 发送POST请求必须设置如下两行
            conn.setDoOutput(true);
            conn.setDoInput(true);
            // 获取URLConnection对象对应的输出流
            out = new PrintWriter(conn.getOutputStream());
            // 发送请求参数
            out.print(param);
            // flush输出流的缓冲
            out.flush();
            // 定义BufferedReader输入流来读取URL的响应
            in = new BufferedReader(new InputStreamReader(conn.getInputStream()));
            String line;
            while ((line = in.readLine()) != null)
            {
                result += line;
            }
        } catch (Exception e)
        {
            CommLog.error("发送 POST请求[{}]出现异常！", url, e);
        } finally
        {
            try
            {
                if (out != null)
                {
                    out.close();
                }
                if (in != null)
                {
                    in.close();
                }
            } catch (IOException ex)
            {
            }
        }
        return result;
    }

    /****************************
     * 向指定 URL 发送POST方法的请求
     *
     * @param url
     *            发送请求的 URL
     * @param param
     *            请求参数
     * @return 所代表远程资源的响应结果
     */
    public static String sendPost(String url, Map<String, Object> param)
    {
        StringBuilder sBuilder = new StringBuilder();
        for (Entry<String, Object> pair : param.entrySet())
        {
            try
            {
                sBuilder.append(pair.getKey()).append('=').append(URLEncoder.encode(pair.getValue().toString(), "utf-8")).append('&');
            } catch (UnsupportedEncodingException e)
            {
            }
        }
        if (sBuilder.length() > 0)
        {
            sBuilder.deleteCharAt(sBuilder.length() - 1);
        }
        return sendPost(url, sBuilder.toString());
    }

    /****************************
     * 发送一个HTTP GET请求到指定的URL，并获取WEB返回的数据
     *
     * @param connTimeOutMs
     * @param readTimeOutMs
     * @param strGetUrl
     * @param strPostParam
     * @param exceptionRes
     * @return
     */
    public static String sendHttpGet2Web(int connTimeOutMs, int readTimeOutMs, String strGetUrl, String strPostParam, String exceptionRes)
    {
        String strRet = exceptionRes;

        HttpURLConnection connection = null;
        BufferedReader br = null;
        try
        {
            String strWholeUrl;
            if (strPostParam != null)
            {
                strWholeUrl = strGetUrl + "?" + strPostParam;
            } else
            {
                strWholeUrl = strGetUrl;
            }
            URL url = new URL(strWholeUrl);
            connection = (HttpURLConnection) url.openConnection();
            connection.setConnectTimeout(connTimeOutMs);
            connection.setReadTimeout(readTimeOutMs);
            connection.setRequestMethod("GET");
            connection.connect();
            br = new BufferedReader(new InputStreamReader(connection.getInputStream(), "UTF-8")); // 设置编码,否则中文乱码
            strRet = br.readLine();

            if (strRet != null)
            {
                strRet = _normReturn(strRet);
            }
            // 关闭连接
            br.close();
            connection.disconnect();
        } catch (Exception e)
        {
            CommLog.error(e.getMessage(), e);
            try
            {
                if (null != br)
                    br.close();
            } catch (Exception e1)
            {
            }
            try
            {
                if (null != connection)
                    connection.disconnect();
            } catch (Exception e2)
            {
            }
        }
        return strRet;
    }

    /****************************
     * 发送一个HTTP POST请求到指定的URL，并获取WEB返回的数据
     *
     * @param connTimeOutMs
     * @param readTimeOutMs
     * @param _strPostUrl
     * @param _strPostParam
     * @param exceptionRes
     * @return
     */
    public static String sendHttpPost2Web(int connTimeOutMs, int readTimeOutMs, String _strPostUrl, String _strPostParam, String exceptionRes)
    {
        String strRet = exceptionRes;

        HttpURLConnection connection = null;
        BufferedReader br = null;
        try
        {
            URL url = new URL(_strPostUrl);
            connection = (HttpURLConnection) url.openConnection();
            connection.setDoOutput(true);
            connection.setDoInput(true);
            connection.setRequestMethod("POST");
            connection.setUseCaches(false);
            connection.setInstanceFollowRedirects(true);
            connection.setRequestProperty("Content-Type", "application/json");
            connection.setConnectTimeout(connTimeOutMs);
            connection.setReadTimeout(readTimeOutMs);
            connection.connect();
            DataOutputStream out = new DataOutputStream(connection.getOutputStream());

            if (_strPostParam != null)
            {
                out.writeBytes(_strPostParam);
                out.flush();
            }
            out.close();

            InputStream isRet;
            isRet = connection.getInputStream();
            br = new BufferedReader(new InputStreamReader(isRet, "UTF-8"));

            strRet = "";
            String strRetTemp = null;
            while ((strRetTemp = br.readLine()) != null)
            {
                strRet += strRetTemp;
                strRetTemp = null;
            }

            br.close();
            connection.disconnect();
        } catch (SocketTimeoutException e)
        { // 已定义的超时错误直接返回
            CommLog.error("HttpPost time out for: [{} ?{}]", _strPostUrl, _strPostParam);
            try
            {
                if (null != br)
                    br.close();
            } catch (Exception e1)
            {
            }
            try
            {
                if (null != connection)
                    connection.disconnect();
            } catch (Exception e2)
            {
            }
        } catch (Exception e)
        {
            CommLog.error("HttpPost exception for: [{}?{}]", _strPostUrl, _strPostParam, e);

            try
            {
                if (null != br)
                    br.close();
            } catch (Exception e1)
            {
            }
            try
            {
                if (null != connection)
                    connection.disconnect();
            } catch (Exception e2)
            {
            }
        }

        return strRet;
    }

    /****************************
     * Web接口返回数据规范化
     *
     * @param strRetOrgi
     * @return
     */
    private static String _normReturn(String strRetOrgi)
    {
        String strRet = strRetOrgi;
        int iStartPos = 0;
        for (int i = 0; i < strRetOrgi.length(); i++)
        {
            char ch = strRetOrgi.charAt(i);
            if ((ch >= '0' && ch <= '9') || (ch >= 'a' && ch <= 'f') || (ch >= 'A' && ch <= 'F'))
            {
                break;
            }
            iStartPos++;
        }
        if (iStartPos != 0)
        {
            strRet = strRetOrgi.substring(iStartPos);
        }
        return strRet;
    }

    /****************************
     * HTTP请求获取全文
     *
     * @param strGetUrl
     * @param strPostParam
     * @return
     */
    public static byte[] GetAll(String strGetUrl)
    {
        return GetAll(15000, 15000, strGetUrl, "");
    }

    /****************************
     * HTTP请求获取全文
     *
     * @param strGetUrl
     * @param strPostParam
     * @return
     */
    public static byte[] GetAll(String strGetUrl, String strPostParam)
    {
        return GetAll(15000, 15000, strGetUrl, strPostParam);
    }

    /****************************
     * HTTP请求获取全文
     *
     * @param connTimeOutMs
     * @param readTimeOutMs
     * @param strGetUrl
     * @param strPostParam
     * @return
     */
    public static byte[] GetAll(int connTimeOutMs, int readTimeOutMs, String strGetUrl, String strPostParam)
    {
        HttpURLConnection connection = null;
        InputStream br = null;

        try
        {
            String strWholeUrl;
            if (strPostParam != null && !strPostParam.trim().isEmpty())
            {
                strWholeUrl = strGetUrl + "?" + strPostParam;
            } else
            {
                strWholeUrl = strGetUrl;
            }
            URL url = new URL(strWholeUrl);
            connection = (HttpURLConnection) url.openConnection();
            connection.setConnectTimeout(connTimeOutMs);
            connection.setReadTimeout(readTimeOutMs);
            connection.setRequestMethod("GET");
            connection.connect();
            br = connection.getInputStream();

            ByteArrayOutputStream swapStream = new ByteArrayOutputStream();
            byte[] buff = new byte[512];
            int rc = 0;
            while ((rc = br.read(buff, 0, 512)) > 0)
            {
                swapStream.write(buff, 0, rc);
            }
            br.close();

            // 关闭连接
            br.close();
            connection.disconnect();
            return swapStream.toByteArray();
        } catch (Exception e)
        {
            CommLog.error(e.getMessage(), e);
            try
            {
                if (null != br)
                    br.close();
            } catch (Exception e1)
            {
            }
            try
            {
                if (null != connection)
                    connection.disconnect();
            } catch (Exception e2)
            {
            }
        }
        return null;
    }
}

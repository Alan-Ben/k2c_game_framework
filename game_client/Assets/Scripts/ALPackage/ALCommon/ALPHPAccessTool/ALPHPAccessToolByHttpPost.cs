using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using UnityEngine;

namespace ALPackage
{
    /**************
     * Http Post
     **/
    public class ALPHPAccessToolByHttpPost
    {
        private const string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }

        public static HttpWebResponse CreatePostHttpResponse(string url, IDictionary<string, string> parameters, int? timeout, string userAgent, Encoding requestEncoding, CookieCollection cookies)
        {
#if UNITY_EDITOR
            Debug.Log("[Even] Http Post请求的链接 url:" + url);
#endif
            if(string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            if(requestEncoding == null)
            {
                throw new ArgumentNullException("requestEncoding");
            }
            Uri myUri = new Uri(url);
            HttpWebRequest request = null;
            try
            {
                //如果是发送HTTPS请求  
                if(url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                    request = WebRequest.Create(myUri) as HttpWebRequest;
                    request.ProtocolVersion = HttpVersion.Version10;
                }
                else
                {
                    request = (HttpWebRequest)WebRequest.Create(myUri);
                }
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";

                if(!string.IsNullOrEmpty(userAgent))
                {
                    request.UserAgent = userAgent;
                }
                else
                {
                    request.UserAgent = DefaultUserAgent;
                }

                if(timeout.HasValue)
                {
                    request.Timeout = timeout.Value;
                }
                if(cookies != null)
                {
                    request.CookieContainer = new CookieContainer();
                    request.CookieContainer.Add(cookies);
                }
                //如果需要POST数据  
                if(!(parameters == null || parameters.Count == 0))
                {
                    StringBuilder buffer = new StringBuilder();
                    int i = 0;
                    foreach(string key in parameters.Keys)
                    {
                        if(i > 0)
                        {
                            buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                        }
                        else
                        {
                            buffer.AppendFormat("{0}={1}", key, parameters[key]);
                        }
                        i++;
                    }
                    byte[] data = requestEncoding.GetBytes(buffer.ToString());
                    request.ContentLength = data.Length;
                    using(Stream stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }
                return request.GetResponse() as HttpWebResponse;
            }
            catch(Exception e)
            {
                Debug.LogError($"[Even] WCGPHPAccessToolByHttpPost 请求{url}失败 e:{e}");
                return null;
            }

        }

    }
}


using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

namespace ALPackage
{
    /**************
     * Http Get
     **/
    public class ALPHPAccessToolByHttpGet
    {
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }

        public static HttpWebResponse CreateGetHttpResponse(string _url, IDictionary<string, string> _parameters, bool _isApplicationJsonMode)
        {
            if(string.IsNullOrEmpty(_url))
                return null;

            string postDataStr = "?";
            if(null != _parameters)
            {
                foreach(string key in _parameters.Keys)
                {
                    postDataStr += key + "=" + _parameters[key] + "&";
                }
            }
            postDataStr = postDataStr.Substring(0, postDataStr.Length - 1);
            string url = _url + postDataStr;

            if(_AALMonoMain.instance.showDebugOutput)
                Debug.Log($"[CDN]Http Get 请求的链接 url:{url}");

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
                request.Method = "GET";

                if(_isApplicationJsonMode)
                {
                    request.ContentType = "application/json";
                }
                else
                {
                    request.ContentType = "text/html;charset=UTF-8";
                }

                request.Timeout = 20000;
                return request.GetResponse() as HttpWebResponse;
            }
            catch(Exception e)
            {
                Debug.Log($"[CDN]Http Get 本次请求未获取到数据，请求结束 e:{e}");
                return null;
            }

        }
    }
}


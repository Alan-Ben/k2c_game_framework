using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using System.IO;
using System.Threading;
using System.Text;

namespace ALPackage
{
    public class ALPHPAccessHttpGetDealer
    {
        //请求申请对象
        private HttpWebResponse _m_wrWebRequest;

        private string _m_sURL;
        private bool _m_bIsApplicationJson;

        public ALPHPAccessHttpGetDealer(string _url, IDictionary<string, string> _parameters, bool _isApplicationJsonMode)
        {
            _m_sURL = _url;
            _m_bIsApplicationJson = _isApplicationJsonMode;

            _m_wrWebRequest = _createGetHttpResponse(_url, _parameters, _isApplicationJsonMode);
        }

        private bool _checkValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }

        private HttpWebResponse _createGetHttpResponse(string _url, IDictionary<string, string> _parameters, bool _isApplicationJsonMode)
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
                Debug.Log($"Http Get 请求的链接 url:{url}");

            Uri myUri = new Uri(url);
            HttpWebRequest request = null;
            try
            {
                //如果是发送HTTPS请求  
                if(url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(_checkValidationResult);
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
                Debug.Log($"Http Get 本次请求未获取到数据，请求结束 e:{e}");
                return null;
            }
        }

        /// <summary>
        /// 线程访问并调用回调
        /// </summary>
        /// <param name="_onSuc"></param>
        /// <param name="_onFail"></param>
        public void threadGetURLStr(Action<string> _onSuc, Action _onFail)
        {
            ALThread requestThread = new ALThread(() =>
            {
                try
                {
                    if(null == _m_wrWebRequest)
                    {
                        if(_onFail != null)
                            ALCommonTaskController.CommonActionAddMonoTask(_onFail);
                        return;
                    }

                    // 如果请求出现问题就退出循环
                    int status = (int)_m_wrWebRequest.StatusCode;
                    if(status < 200 || status >= 300)
                    {
                        if(_onFail != null)
                            ALCommonTaskController.CommonActionAddMonoTask(_onFail);
                        return;
                    }

                    Stream myResponseStream = _m_wrWebRequest.GetResponseStream();
                    StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
                    string retString = myStreamReader.ReadToEnd();
                    myStreamReader.Close();
                    myResponseStream.Close();
                    if(string.IsNullOrEmpty(retString))
                    {
                        if(_onFail != null)
                            ALCommonTaskController.CommonActionAddMonoTask(_onFail);
                    }
                    else
                    {
                        if(_onSuc != null)
                            ALCommonTaskController.CommonActionAddMonoTask(() => { _onSuc(retString); });

                    }
                }
                catch(Exception )
                {
                    if(_onFail != null)
                        ALCommonTaskController.CommonActionAddMonoTask(_onFail);
                }
            });

            //开启线程
            requestThread.startThread();
        }
        public void threadGetAndDisposeURLStr(Action<string> _onSuc, Action _onFail)
        {
            ALThread requestThread = new ALThread(() =>
            {
                try
                {
                    if(null == _m_wrWebRequest)
                    {
                        if(_onFail != null)
                            ALCommonTaskController.CommonActionAddMonoTask(_onFail);
                        return;
                    }

                    // 如果请求出现问题就退出循环
                    int status = (int)_m_wrWebRequest.StatusCode;
                    if(status < 200 || status >= 300)
                    {
                        if(_onFail != null)
                            ALCommonTaskController.CommonActionAddMonoTask(_onFail);
                        return;
                    }

                    Stream myResponseStream = _m_wrWebRequest.GetResponseStream();
                    StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
                    string retString = myStreamReader.ReadToEnd();
                    myStreamReader.Close();
                    myResponseStream.Close();

                    if(_onSuc != null)
                        ALCommonTaskController.CommonActionAddMonoTask(() => { _onSuc(retString); });

                    //释放资源
                    dispose();
                }
                catch(Exception )
                {
                    if(_onFail != null)
                        ALCommonTaskController.CommonActionAddMonoTask(_onFail);

                    //释放资源
                    dispose();
                }
            });

            //开启线程
            requestThread.startThread();
        }
        public void gsyncRetStr(Action<string> _onSuc, Action _onFail)
        {
            try
            {
                if(null == _m_wrWebRequest)
                {
                    if(_onFail != null)
                        _onFail();
                    return;
                }

                // 如果请求出现问题就退出循环
                int status = (int)_m_wrWebRequest.StatusCode;
                if(status < 200 || status >= 300)
                {
                    if(_onFail != null)
                        _onFail();
                    return;
                }

                Stream myResponseStream = _m_wrWebRequest.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                if(string.IsNullOrEmpty(retString))
                {
                    if(_onFail != null)
                        _onFail();
                }
                else
                {
                    if(_onSuc != null)
                        _onSuc(retString);
                }
            }
            catch(Exception )
            {
                if(_onFail != null)
                    _onFail();
            }
        }

        /// <summary>
        /// 资源释放
        /// </summary>
        public void dispose()
        {
            if(null != _m_wrWebRequest)
                _m_wrWebRequest.Dispose();
            _m_wrWebRequest = null;
        }
    }
}

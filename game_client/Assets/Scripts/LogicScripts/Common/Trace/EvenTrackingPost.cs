using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 埋点数据发送类
    /// </summary>
    public class EvenTrackingPost
    {
        #region post请求返回码 是错误
        public const long EvenTrackingPostErrorCode_Exception = -1;//post请求返回码 - 返回数据转Json异常
        public const long EvenTrackingPostErrorCode_null = -2;//post请求返回码 - 返回数据为空
        public const long EvenTrackingPostErrorCode_failDelegate = -3;//post请求返回码 - 请求发送数据埋点回调失败

        #endregion
        
        //发送数据
        public static void dealPostMsg(string _url, string _subURL, IDictionary<string, string> _parameters, Action _onSucceed, Action<long> _onFailed, Action<long> _onDataError)
        {
            if(_parameters == null)
            {
                if(_onDataError != null)
                    _onDataError(-1);
                return;
            }

            ALURLPostDealer.reqPostMsg(ALCommon.urlEndInsure(_url) + _subURL, _parameters, (_str) =>
            {
                //成功处理
                if (_AALMonoMain.instance.showDebugOutput)
                    Debug.Log($"[Even] EvenTrackingPost:_dealPostMsg方法 Http post返回数据{_str}");

                //解析数据
                EvenTrackingReceiveData receiveData = null;
                try
                {
                    receiveData = JsonUtility.FromJson<EvenTrackingReceiveData>(_str);
                }
                catch (Exception e)
                {

                    Debug.LogError($"[Even] 埋点发送解析返回值失败，url:{_subURL}, ret:{_str} \n {e}");
                    if (_onFailed != null)
                        _onFailed(EvenTrackingPostErrorCode_Exception);
                    return;
                }

                //对数据结果进行处理
                if (receiveData == null)
                {
                    Debug.LogError($"[Even] EvenTrackingPost:_dealPostMsg方法 返回数据为空 receiveData = null");
                    if (_onFailed != null)
                        _onFailed(EvenTrackingPostErrorCode_null);
                }
                else
                {
                    //如果返回码不是1则说明发送失败
                    if (receiveData.code != 1)
                    {
                        switch (receiveData.code)
                        {
                            case 1001://参数缺失，无法请求
                            case 1002://参数格式异常
                            case 1003://请求时间与服务器时间相差过大
                            case 1004://缺少签名，无授权，无法请求
                            case 1005://签名错误，无法请求
                            case 1008://没有查到指定数据
                                if (_onDataError != null)
                                    _onDataError(receiveData.code);
                                break;
                            default:
                                if (_onFailed != null)
                                    _onFailed(receiveData.code);
                                break;
                        }
                    }
                    else
                    {
                        if (_onSucceed != null)
                            _onSucceed();
                    }
                }

            }, (_error, _errStatus) =>
            {
                //失败处理
                if (_onFailed != null)
                    _onFailed(EvenTrackingPostErrorCode_failDelegate);
            });
        }
    }

    /// <summary>
    /// post请求返回的json数据
    /// </summary>
    [Serializable]
    public class EvenTrackingReceiveData
    {
        public long code;//返回码
        public string msg;//返回状态文字描述说明
        public long time;//返回的时间戳

        public EvenTrackingReceiveData() {}
    }
}

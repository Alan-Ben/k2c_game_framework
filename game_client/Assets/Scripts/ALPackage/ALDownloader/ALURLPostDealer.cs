using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using ALPackage;
using UnityEngine;

namespace ALPackage
{
    /// <summary>
    /// 埋点数据发送类
    /// </summary>
    public class ALURLPostDealer
    {
        /// <summary>
        /// 请求发送埋点数据
        /// </summary>
        /// <param name="_url">埋点地址</param>
        /// <param name="_parameters">埋点数据</param>
        /// <param name="_onSucceed">成功回调</param>
        /// <param name="_onFailed">失败回调</param>
        /// <param name="_onDataError">数据错误回调</param>
        public static void reqPostMsg(string _url, IDictionary<string, string> _parameters, Action<string> _onSucceed, Action<string, int> _onFailed)
        {
            ALThread postThread = new ALThread(() => 
            {
                _dealPostMsg(
                    _url,
                    _parameters,
                    (_mes) =>
                    {
                        //成功
                        ALCommonTaskController.CommonActionAddMonoTask(() =>
                        {
                            if(_onSucceed != null)
                                _onSucceed(_mes);
                        });
                    }, 
                    (string _error, int _errStatus) =>
                    {
                        //失败
                        ALCommonTaskController.CommonActionAddMonoTask(() =>
                        {
                            if(_onFailed != null)
                                _onFailed(_error, _errStatus);
                        });
                    }); 
            });

            postThread.startThread();
        }
        
        //发送数据
        private static void _dealPostMsg(string _url, IDictionary<string, string> _parameters, Action<string> _onSucceed, Action<string, int> _onFailed)
        {
            ALHttpCommon.PostSync(_url, _parameters
                , (_statusCode, _msg) =>
                {
                    if (_statusCode != 200)
                    {
                        if (_onFailed != null)
                        {
                            _onFailed($"状态码不为200：{_statusCode},{_msg}", _statusCode);
                        }
                    }
                    else
                    {
                        if (_onSucceed != null)
                        {
                            _onSucceed(_msg);
                        }
                    }
                }, _onFailed);
        }
    }
}

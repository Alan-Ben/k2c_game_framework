using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 客户端自带埋点数据管理发送类
    /// </summary>
    public class EvenTrackingPostMgr
    {
        private static EvenTrackingPostMgr _g_instance;
        [NotNull]
        public static EvenTrackingPostMgr instance
        {
            get
            {
                if (_g_instance == null)
                {
                    _g_instance = new EvenTrackingPostMgr();
                }
                return _g_instance;
            }
        }

        [NotNull]private Queue<EvenTrackingData> _m_dataQueue = new Queue<EvenTrackingData>();//数据队列
        private bool _m_bIsPosting;//是否正在发送

        public EvenTrackingPostMgr()
        {
        }

        /// <summary>
        /// 埋点上报
        /// </summary>
        public void sendStepReport(TraceStepData _stepData)
        {
            if(_stepData == null)
            {
                Debug.LogError("埋点参数为空，无法发送");
                return;
            }

            //判断是否开启埋点
            if (!CDNSetting_ClientConfigInfo.instance.isOpenPHPAD)
                return;

            //判断是否有埋点地址
            if (string.IsNullOrEmpty(CDNSetting_ClientConfigInfo.instance.getPhpAdUrl()))
                return;

            //判断埋点优先级是否需要发送
            if (_stepData.sendLevel > CDNSetting_ClientConfigInfo.instance.phpADLevel)
                return;

            //构建数据
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("game_id", NPConst.GAME_ID);//游戏id===必填
            parameters.Add("platform_id", CDNSetting_ClientConfigInfo.instance.platformId.ToString());//平台自定义ID===必填
            parameters.Add("area_id", CDNSetting_AreaInfo.instance.areaId);//区域ID===必填
            parameters.Add("server_id", GameInit_SelectServer.instance.loginServerLogicId.ToString());//服务器ID
            parameters.Add("udid", "no_sdk_test");//设备ID===必填
            parameters.Add("system", _getSystem());//运行环境（aos,ios,pc）
            parameters.Add("os", SystemInfo.operatingSystem);//系统信息（具体版本号）
            parameters.Add("device", SystemInfo.deviceModel);//设备型号
            parameters.Add("login_tag", Application.version);//当时登录客户端版本号
            parameters.Add("language", Application.systemLanguage.ToString());//系统语言类型
            parameters.Add("runm", SystemInfo.systemMemorySize.ToString());//运行内存：手机的运行内存是多少(以MB为单位)
            parameters.Add("devres", Screen.currentResolution.ToString());//设备分辩率
            parameters.Add("cpunum", SystemInfo.processorCount.ToString());//cpu核数
            parameters.Add("cpumaxf", "");//cpu最大频率
            parameters.Add("cpuminf", "");//cpu最小频率
            parameters.Add("mac", "");//MAC地址
            parameters.Add("ip", _getIPAddress());//客户端IP
            parameters.Add("sdkId", "");//客户端接入afID的时候启动时会回调
            parameters.Add("step_info", _getStepInfo(_stepData));//json数组===必填
            parameters.Add("support_gpu_instancing", SystemInfo.supportsInstancing ? "1" : "0");//是否支持gpu instancing(0:否;1:是)
            parameters.Add("extend", _stepData.mark);//自定义扩展字段
            parameters.Add("mark2", GCommon.getStepReportMark2String());//埋点时间差及累计时间

            EvenTrackingData data = new EvenTrackingData();
            data.parameters = parameters;

            //加入队列
            _m_dataQueue.Enqueue(data);

            //是否正在发送数据
            if (!_m_bIsPosting)
            {
                _m_bIsPosting = true;
                ALMonoTaskMgr.instance.addNextFrameTask(new EvenTrackingSendMsgTask(() =>
                {
                    _m_bIsPosting = false;
                }));
            }

        }

        private string _getSystem()
        {
#if UNITY_IPHONE || UNITY_IOS
            return "ios";
#elif UNITY_ANDROID
            return "aos";
#elif UNITY_STANDALONE || UNITY_EDITOR
            return "pc";
#endif
        }

        private string _getIPAddress()
        {
            string curIp;
#if UNITY_EDITOR
                try
                {
                    string IPAddress = null;
                    IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

                    if (host == null || host.AddressList == null)
                        return "F:EditorNull";

                    foreach (var ip in host.AddressList)
                    {
                        if (ip.AddressFamily == AddressFamily.InterNetwork)
                        {
                            IPAddress = ip.ToString();
                            break;
                        }
                    }
                    curIp = IPAddress;
                }
                catch (Exception e)
                {
                    Debug.LogError($"hostname:{Dns.GetHostName()}, getIPAddress failed:{e}");
                    return "F:EditorError";
                }
#else
                curIp = "F:NoSDK";
#endif
            return curIp;
        }

        //获取json数组
        private string _getStepInfo(TraceStepData _stepData)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            builder.Append($"\"uid\":\"{(Game.instance.uid != null ? Game.instance.uid : "0")}\",");//平台用户id:刚开始默认0
            builder.Append($"\"cid\":\"{((NPPlayer.instance != null && NPPlayer.instance.playerInfo != null) ? NPPlayer.instance.playerInfo.CID.ToString() : "0")}\",");//角色ID
            builder.Append($"\"step_id\":\"{_stepData.ID.ToString()}\","); //事件ID
            builder.Append($"\"op_time\":\"{ALCommon.getNowTimeSec()}\",");//触发时间
            builder.Append($"\"netcode\":\"\",");//网络代码：移动网络代码
            builder.Append($"\"nettype\":\"\",");//网络类型：Wifi、3G、4G、5G
            builder.Append($"\"nation\":\"\"");//国家
            builder.Append("}");

            return builder.ToString();
        }




        /// <summary>
        /// 发送埋点数据任务
        /// </summary>
        private class EvenTrackingSendMsgTask : _IALBaseMonoTask
        {
            private const int retryCount = 3; //重试次数
            private EvenTrackingData _m_data;//需要发送的数据
            private int _m_iRetryCount; //发送失败重试次数
            private Action _m_aOnEnd; //结束

            public EvenTrackingSendMsgTask(Action _onEnd)
            {
                _m_iRetryCount = retryCount;
                _m_aOnEnd = _onEnd;
            }

            public void deal()
            {
                //是否还有数据
                if (EvenTrackingPostMgr.instance._m_dataQueue.Count == 0)
                {
                    if (_m_aOnEnd != null)
                        _m_aOnEnd();
                    return;
                }

                //获取下一个数据
                if(_m_data == null)
                    _m_data = EvenTrackingPostMgr.instance._m_dataQueue.Dequeue();

                if (null != _m_data)
                {
                    string url = CDNSetting_ClientConfigInfo.instance.getPhpAdUrl();
                    string subUrl = "api/game_trace/step_report";

                    if (string.IsNullOrEmpty(url))
                        return;

                    //数据签名
                    _m_data.sign();

                    if (_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
                    {
                        Debug.Log($"[Even] EvenTrackingSendMsgTask: deal方法 需要post发送的埋点：url:{url} subUrl:{subUrl}-----{_m_data.ToString()}");
                    }

                    //处理数据发送
                    EvenTrackingPost.dealPostMsg(url, subUrl, _m_data.parameters, () =>
                    {
                        //========================成功========================
                        if(_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
                        {
                            Debug.Log($"埋点数据发送成功！");
                        }

                        //重置重试次数
                        _m_iRetryCount = retryCount;
                        //重置数据
                        _m_data = null;

                        //下一帧继续检查是否还有数据发送
                        ALMonoTaskMgr.instance.addNextFrameTask(this);
                    }, (failCode) =>
                    {
                        //========================失败========================
                        if(_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
                        {
                            Debug.Log($"埋点数据发送失败！");
                        }

                        //失败后继续尝试能否发送
                        if (_m_iRetryCount > 0)
                            _m_iRetryCount--;
                        else
                        {
                            //重试之后还是发送失败放弃数据
                            //重置重试次数
                            _m_iRetryCount = retryCount;
                            //重置数据
                            _m_data = null;
                        }

                        //下一帧继续检查是否还有数据发送
                        ALMonoTaskMgr.instance.addNextFrameTask(this);

                    }, (errCode) =>
                    {
                        //=======================数据错误，此时数据其实已经传输=======================
                        if(_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
                        {
                            Debug.Log($"埋点数据错误发送失败！ post请求返回的返回码  errCode:  {errCode}");
                        }

                        //重置重试次数
                        _m_iRetryCount = retryCount;
                        //重置数据
                        _m_data = null;

                        //下一帧继续检查是否还有数据发送
                        ALMonoTaskMgr.instance.addNextFrameTask(this);
                    });
                }
                else
                {
                    //下一帧继续检查是否还有数据发送
                    ALMonoTaskMgr.instance.addNextFrameTask(this);
                }
            }
        }
    }


    /// <summary>
    /// 埋点数据
    /// </summary>
    public class EvenTrackingData
    {
        public IDictionary<string, string> parameters { get; set; }

        public EvenTrackingData()
        {
        }

        /// <summary>
        /// 设置请求时间戳并签名
        /// </summary>
        public void sign()
        {
            if (parameters == null)
                return;

            //请求时间戳（10位）===必填
            if (parameters.ContainsKey("timestamp"))
                parameters["timestamp"] = ALCommon.getNowTimeSec().ToString();
            else
                parameters.Add("timestamp", ALCommon.getNowTimeSec().ToString());

            //签名（MD5加密字符串后转小写）===必填
            if (parameters.ContainsKey("sign"))
                parameters.Remove("sign");
            parameters.Add("sign", getSign(parameters));
        }


        /// <summary>
        /// 获取签名
        /// 所有参数(除sign外)，根据参数名首字母自行排序后KEY-VALUE拼接，再加上签名key后进行MD5加密
        /// </summary>
        /// <param name="_data"></param>
        /// <returns></returns>
        public static string getSign(IDictionary<string, string> _data)
        {
            if (_data == null)
                return String.Empty;

            //按参数名首字母自行排序
            SortedDictionary<string, string> sortDic = new SortedDictionary<string, string>(_data);
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> kv in sortDic)
            {
                sb.Append(string.Format("{0}={1}", kv.Key, kv.Value));
            }
            sb.Append("key");
            try
            {
                string md5 = getMD5(sb.ToString());
                if (md5 != null)
                {
                    return md5.ToLowerInvariant();
                }
                Debug.LogError($"GetMD5Error:{sb}");
                return "GetMD5Error";
            }
            catch (Exception e)
            {
                Debug.LogError($"GetMD5Exception:{sb},{e}");
                return "GetMD5Exception";
            }
        }
        public static string getMD5(string input)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop thr
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("X2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (parameters != null)
            {
                foreach (string key in parameters.Keys)
                {
                    sb.AppendFormat("{0} = {1}", key, parameters[key]);
                    sb.AppendLine();
                }
            }
            return sb.ToString();
        }
    }
}

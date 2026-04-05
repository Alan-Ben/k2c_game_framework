using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using JetBrains.Annotations;
using LitJson;

namespace GOE
{
    /// <summary>
    /// 登录服务器的相关配置，此数据要求CDNLocalSetting_ClientConfigInfo先下载完成
    /// </summary>
    public class CDNSetting_LoginServerUrlInfo : _ATCDNConfigSetting<LoginServerUrlInfo>, _ILSLoginServerInfo
    {
        private static CDNSetting_LoginServerUrlInfo _g_instance;
        [NotNull]
        public static CDNSetting_LoginServerUrlInfo instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new CDNSetting_LoginServerUrlInfo();
                return _g_instance;
            }
        }

        private string _m_sLoginServerIp; //登陆服务器IP
        private int _m_iLoginServerPort; //登陆服务器端口
        private float _m_fLoginServerDelayTime; //登录服务器延迟登录时间

        //额外登录服务器的相关信息
        private List<string> _m_lLoopLSIpList = new List<string>();
        private List<int> _m_lLoopLSPortList = new List<int>();
        private List<float> _m_lLoopLSDelayTimeList = new List<float>();

        protected CDNSetting_LoginServerUrlInfo() : base("LoginServerUrlInfo")
        {
            //初始化默认数据
            _m_sLoginServerIp = Game.instance.mainCamera.platInfo.connectIp;
            _m_iLoginServerPort = Game.instance.mainCamera.platInfo.port;
            _m_fLoginServerDelayTime = 0f;
        }

        /// <summary>
        /// 获取下载的文件路径
        /// </summary>
        /// <returns></returns>
        protected override string _getDownloadFilePath()
        {
            return string.Format("/login_server/{0}/{1}", CDNSetting_ClientConfigInfo.instance.platformId, CDNSetting_AreaInfo.instance.areaId);
        }

        /// <summary>
        /// 获取下载操作的处理对象
        /// </summary>
        /// <returns></returns>
        protected override _ATALCDNDownloadProvider _getURLDownloaderProvider()
        {
            return CDNURLProvider_Client.instance;
        }

        /// <summary>
        /// 在数据加载完成后的事件
        /// </summary>
        protected override void _onDataLoaded()
        {
            if (!isValid)
            {
                Debug.LogError($"cdn 数据加载失败{this}");

                //发送埋点-CDN-初始化登录服务器地址失败
                GCommon.sendStepReport(TraceConst.INIT_LOGIN_SERVER_URL_FAIL);
            }
            
            if (null == data || null == data.config || !Game.instance.isUseCdn)
            {
                //无数据使用默认配置
                _m_sLoginServerIp = Game.instance.mainCamera.platInfo.connectIp;
                _m_iLoginServerPort = Game.instance.mainCamera.platInfo.port;
                _m_fLoginServerDelayTime = 0f;

                //发送埋点-CDN-初始化登录服务器地址成功,使用默认配置
                GCommon.sendStepReport(TraceConst.INIT_LOGIN_SERVER_URL_SUC_DEFAULT);

                return;
            }

            //从cdn配置获取对应的配置数据并存储
            _m_sLoginServerIp = data.config.loginServerIp;
            _m_iLoginServerPort = data.config.loginServerPort;
            if (!ALCommon.TryParseFloat(data.config.loginServerDelayTime, out _m_fLoginServerDelayTime))
            {
                //如果非空，说明填了值但是解析错误，才报错
                if (!string.IsNullOrEmpty(data.config.loginServerDelayTime))
                {
                    Debug.LogError($"[CDN]CDNSetting_AreaInfo----------CDN数据配置 loginServerDelayTime解析失败：{data.config.loginServerDelayTime}");
                }

                _m_fLoginServerDelayTime = 0f;
            }

            //读取循环登录和处理的Ip和端口队列
            _setLoginServerList(data.config.loopLSIpList, data.config.loopLSPortList, data.config.loopLSDelayTimeList);

            //发送埋点-CDN-初始化登录服务器地址成功,使用CDN配置
            GCommon.sendStepReport(TraceConst.INIT_LOGIN_SERVER_URL_SUC_CDN);
        }

        #region CDN用于登录的相关接口声明
        //默认登录信息
        public string defaultIP { get { return _m_sLoginServerIp; } }
        public int defaultPort { get { return _m_iLoginServerPort; } }
        //默认登录方式延迟登录时间
        public float defaultDelayLoginTime { get { return _m_fLoginServerDelayTime; } }

        //轮询地址的登录信息
        public List<string> loopIPList { get { return _m_lLoopLSIpList; } }
        public List<int> loopPortList { get { return _m_lLoopLSPortList; } }
        //轮询登录方式延迟登录时间
        public List<float> loopDelayLoginTimeList { get { return _m_lLoopLSDelayTimeList; } }
        
        //异步初始化获取数据
        public void reqDataDone(Action _action)
        {
            //没走cdn直接处理
            if (!Game.instance.isUseCdn)
            {
                if (null != _action)
                    _action();
                return;
            }
            
            //获取对应数据
            commonDownload(_action);
        }

        #endregion

        /// <summary>
        /// 设置服务器信息列表
        /// </summary>
        protected void _setLoginServerList(string _serverIpList, string _serverPortList, string _serverDelayTimeList)
        {
            //设置列表前，先清空之前设置的数据
            _m_lLoopLSIpList.Clear();
            _m_lLoopLSPortList.Clear();
            _m_lLoopLSDelayTimeList.Clear();
            if (string.IsNullOrEmpty(_serverIpList))
                return;

            string[] ipArr = _serverIpList.Split(';');
            if (null == ipArr)
                return;

            string[] portArr = _serverPortList.Split(';');
            if (null == portArr)
                return;

            string[] delayTimeArr = _serverDelayTimeList.Split(';');
            if (null == delayTimeArr)
                return;

            string tmpIp;
            int tmpPort = 5101;
            float delayTime = 0f;
            for (int i = 0; i < ipArr.Length; i++)
            {
                tmpIp = ipArr[i];
                if (null == tmpIp)
                    continue;

                if (i < portArr.Length)
                {
                    //在队列长度内，尝试读取端口
                    if (!int.TryParse(portArr[i], out tmpPort))
                    {
#if UNITY_EDITOR
                        UnityEngine.Debug.LogError($"端口数据读取错误：{portArr[i]}");
#endif
                        tmpPort = 5101;
                    }
                }
                else
                {
                    //数据无效则给默认值
#if UNITY_EDITOR
                    UnityEngine.Debug.LogError($"端口数据长度比Ip少");
#endif
                    tmpPort = 5101;
                }

                if (i < delayTimeArr.Length)
                {
                    //在队列长度内，尝试读取端口
                    if (!ALCommon.TryParseFloat(delayTimeArr[i], out delayTime))
                    {
                        UnityEngine.Debug.LogError($"延长时间数据读取错误：{delayTimeArr[i]}");
                        delayTime = 0f;
                    }
                }
                else
                {
                    //数据无效则给默认值
#if UNITY_EDITOR
                    UnityEngine.Debug.LogError($"延长时间数据长度比Ip少");
#endif
                    delayTime = 0f;
                }

                //添加到数据集
                _m_lLoopLSIpList.Add(tmpIp);
                _m_lLoopLSPortList.Add(tmpPort);
                _m_lLoopLSDelayTimeList.Add(delayTime);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using JetBrains.Annotations;
using LitJson;

namespace GOE
{
    /// <summary>
    /// 服务器列表相关配置，此数据要求CDNLocalSetting_ClientConfigInfo先下载完成
    /// </summary>
    public class CDNSetting_ServerListInfo : _ATCDNConfigSetting<List<ServerDataInfo>>
    {
        private static CDNSetting_ServerListInfo _g_instance;
        [NotNull]
        public static CDNSetting_ServerListInfo instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new CDNSetting_ServerListInfo();
                return _g_instance;
            }
        }

        protected CDNSetting_ServerListInfo() : base("ServerDataInfo")
        {
        }

        /// <summary>
        /// 获取下载的文件路径
        /// </summary>
        /// <returns></returns>
        protected override string _getDownloadFilePath()
        {
            return string.Format("/server_list/{0}/{1}", CDNSetting_ClientConfigInfo.instance.platformId, CDNSetting_AreaInfo.instance.areaId);
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

                //发送埋点-CDN-初始化服务器列表失败
                GCommon.sendStepReport(TraceConst.INIT_SERVER_LIST_FAIL);
            }
            else
            {
                //发送埋点-CDN-初始化服务器列表成功
                GCommon.sendStepReport(TraceConst.INIT_SERVER_LIST_SUC);
            }
        }

        /// <summary>
        /// 获取服务器数据
        /// </summary>
        /// <param name="_serverId"></param>
        /// <returns></returns>
        public ServerDataInfo getServerDataInfo(long _serverId)
        {
            if (null == data || null == data.config)
                return null;

            foreach (ServerDataInfo serverDataInfo in data.config)
            {
                if(null == serverDataInfo)
                    continue;

                if (serverDataInfo.serverId == _serverId)
                    return serverDataInfo;
            }

            return null;
        }
    }
}
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
    public class CDNSetting_ClientFunctionSwitchConfig : _ATCDNConfigSetting<List<ClientFunctionSwitchConfig>>
    {
        private static CDNSetting_ClientFunctionSwitchConfig _g_instance;
        [NotNull]
        public static CDNSetting_ClientFunctionSwitchConfig instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new CDNSetting_ClientFunctionSwitchConfig();
                return _g_instance;
            }
        }

        protected CDNSetting_ClientFunctionSwitchConfig() : base("ClientFunctionSwitchConfig")
        {
        }

        /// <summary>
        /// 获取下载的文件路径
        /// </summary>
        /// <returns></returns>
        protected override string _getDownloadFilePath()
        {
            return string.Format("/notice_in_maintain/{0}/{1}", CDNSetting_ClientConfigInfo.instance.platformId, CDNSetting_AreaInfo.instance.areaId);
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
            }
        }
    }
}
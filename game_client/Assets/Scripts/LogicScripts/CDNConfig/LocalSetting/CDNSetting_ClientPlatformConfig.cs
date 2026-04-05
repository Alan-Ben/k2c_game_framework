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
    public class CDNSetting_ClientPlatformConfig : _ATCDNConfigSetting<ClientPlatformConfig>
    {
        private static CDNSetting_ClientPlatformConfig _g_instance;
        [NotNull]
        public static CDNSetting_ClientPlatformConfig instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new CDNSetting_ClientPlatformConfig();
                return _g_instance;
            }
        }

        protected CDNSetting_ClientPlatformConfig() : base("ClientPlatformConfig")
        {
        }

        /// <summary>
        /// 获取下载的文件路径
        /// </summary>
        /// <returns></returns>
        protected override string _getDownloadFilePath()
        {
            int platformId = CDNSetting_ClientConfigInfo.instance.platformId;

#if UNITY_IPHONE
            return string.Format("/client_platform_config/{0}/{1}/{2}/{3}", platformId, Game.instance.mainCamera.platInfo.channelId.ToString(), CDNSetting_AreaInfo.instance.areaId, "ios");
#elif UNITY_ANDROID
            return string.Format("/client_platform_config/{0}/{1}/{2}/{3}", platformId, Game.instance.mainCamera.platInfo.channelId.ToString(), CDNSetting_AreaInfo.instance.areaId, "android");
#elif UNITY_STANDALONE || UNITY_EDITOR
            return string.Format("/client_platform_config/{0}/{1}/{2}/{3}", platformId, Game.instance.mainCamera.platInfo.channelId.ToString(), CDNSetting_AreaInfo.instance.areaId, "pc");
#endif
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
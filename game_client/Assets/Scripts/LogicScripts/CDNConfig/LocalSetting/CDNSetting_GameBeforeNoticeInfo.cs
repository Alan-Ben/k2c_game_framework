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
    public class CDNSetting_GameBeforeNoticeInfo : _ATCDNConfigSetting<List<GameBeforeNoticeInfo>>
    {
        private static CDNSetting_GameBeforeNoticeInfo _g_instance;
        [NotNull]
        public static CDNSetting_GameBeforeNoticeInfo instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new CDNSetting_GameBeforeNoticeInfo();
                return _g_instance;
            }
        }

        protected CDNSetting_GameBeforeNoticeInfo() : base("CDNSetting_GameBeforeNoticeInfo")
        {
        }

        /// <summary>
        /// 获取下载的文件路径
        /// </summary>
        /// <returns></returns>
        protected override string _getDownloadFilePath()
        {
            return string.Format("/notice/{0}/{1}/{2}"
                , CDNSetting_ClientConfigInfo.instance.platformId, CDNSetting_AreaInfo.instance.areaId, Game.instance.mainCamera.platInfo.channelId);
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
                //发送埋点-CDN-初始化登录前公告失败
                GCommon.sendStepReport(TraceConst.INIT_GAME_BEFORE_NOTICE_FAIL);
            }
            else
            {
                //发送埋点-CDN-初始化登录前公告成功
                GCommon.sendStepReport(TraceConst.INIT_GAME_BEFORE_NOTICE_SUC.setMarkParam(data?.config?.Count));
            }
        }
    }
}
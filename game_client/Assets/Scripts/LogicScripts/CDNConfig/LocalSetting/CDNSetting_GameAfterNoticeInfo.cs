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
    public class CDNSetting_GameAfterNoticeInfo : _ATCDNConfigSetting<List<GameAfterNoticeInfo>>
    {
        private static CDNSetting_GameAfterNoticeInfo _g_instance;
        [NotNull]
        public static CDNSetting_GameAfterNoticeInfo instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new CDNSetting_GameAfterNoticeInfo();
                return _g_instance;
            }
        }

        protected CDNSetting_GameAfterNoticeInfo() : base("CDNSetting_GameAfterNoticeInfo")
        {
        }

        /// <summary>
        /// 获取下载的文件路径
        /// </summary>
        /// <returns></returns>
        protected override string _getDownloadFilePath()
        {
            //先写死服务器Id为1，后续根据需求调整
            return string.Format("/notice_in_game/{0}/{1}/{2}", CDNSetting_ClientConfigInfo.instance.platformId, CDNSetting_AreaInfo.instance.areaId, 1);
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

                //发送埋点-CDN-初始化登录后公告失败
                GCommon.sendStepReport(TraceConst.INIT_GAME_BEFORE_AFTER_FAIL);
            }
            else
            {
                //发送埋点-CDN-初始化登录后公告成功
                GCommon.sendStepReport(TraceConst.INIT_GAME_BEFORE_AFTER_SUC);
            }
        }
    }
}
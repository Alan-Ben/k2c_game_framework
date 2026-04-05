using System.Collections.Generic;
using UnityEngine;
using System;
using ALPackage;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 登录部分配置的下载处理对象，同时用于检测对应的下载链接哪个可用
    /// 最快的CDN地址,用于获取客户端配置、游戏前公告、服务器列表、游戏内公告等
    /// </summary>
    /// 
    public class CDNURLProvider_Client : _ATALCDNDownloadProvider
    {
        private static CDNURLProvider_Client _g_instance = new CDNURLProvider_Client();
        public static CDNURLProvider_Client instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new CDNURLProvider_Client();
                return _g_instance;
            }
        }

        protected CDNURLProvider_Client()
        {
        }

        /// <summary>
        /// 处理下载对象初始化处理，处理完成需要调用setURLDownloader函数
        /// </summary>
        protected override void _doInitURLDownloader()
        {
            if (null == Game.instance.mainCamera.platInfo.phpUrlForLoginList || Game.instance.mainCamera.platInfo.phpUrlForLoginList.Count <= 0)
            {
                setURLDownloader(null);

                return;
            }

            //此操作直接调用构造函数初始化，其他不处理
            setURLDownloader(new ALURLDownloader(Game.instance.mainCamera.platInfo.phpUrlForLoginList));
        }
    }
}
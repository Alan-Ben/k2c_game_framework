using System.Collections.Generic;
using UnityEngine;
using System;
using ALPackage;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 游戏资源下载的下载数据构造对象
    /// </summary>
    /// 
    public class CDNURLProvider_Res : _ATALCDNDownloadProvider
    {
        private static CDNURLProvider_Res _g_instance = new CDNURLProvider_Res();
        public static CDNURLProvider_Res instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new CDNURLProvider_Res();
                return _g_instance;
            }
        }

        protected CDNURLProvider_Res()
        {
        }

        /// <summary>
        /// 处理下载对象初始化处理，处理完成需要调用setURLDownloader函数
        /// </summary>
        protected override void _doInitURLDownloader()
        {
            //没走cdn直接处理
            if (!Game.instance.isUseCdn)
            {
                setURLDownloader(null);
                return;
            }
            
            //依赖于ClientCOnfigInfo下载处理
            CDNSetting_ClientConfigInfo.instance.commonDownload(
                () =>
                {
                    //判断数据是否合法
                    if (null == CDNSetting_ClientConfigInfo.instance.data || null == CDNSetting_ClientConfigInfo.instance.data.config)
                    {
                        setURLDownloader(null);
                        return;
                    }

                    //初始化数据
                    string cdnRootUrl = CDNSetting_ClientConfigInfo.instance.data.config.resUpdateCDNUrlRootList;
                    string[] cdnUrlRootArray = cdnRootUrl.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);

                    setURLDownloader(new ALURLDownloader(cdnUrlRootArray));
                });
        }
    }
}
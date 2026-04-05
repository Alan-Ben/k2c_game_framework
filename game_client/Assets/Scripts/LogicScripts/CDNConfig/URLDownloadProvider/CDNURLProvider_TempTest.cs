using System.Collections.Generic;
using UnityEngine;
using System;
using ALPackage;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 临时测试的下载的下载数据构造对象
    /// </summary>
    /// 
    public class CDNURLProvider_TempTest : _ATALCDNDownloadProvider
    {
        private static CDNURLProvider_TempTest _g_instance = new CDNURLProvider_TempTest();
        public static CDNURLProvider_TempTest instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new CDNURLProvider_TempTest();
                return _g_instance;
            }
        }

        protected CDNURLProvider_TempTest()
        {
        }

        /// <summary>
        /// 处理下载对象初始化处理，处理完成需要调用setURLDownloader函数
        /// </summary>
        protected override void _doInitURLDownloader()
        {
            //此操作直接调用构造函数初始化，其他不处理
            setURLDownloader(new ALURLDownloader(Game.instance.mainCamera.platInfo.tempCdnRootUrl));
        }
    }
}
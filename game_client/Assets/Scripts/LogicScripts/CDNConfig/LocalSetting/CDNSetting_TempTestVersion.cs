using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using JetBrains.Annotations;
using LitJson;

namespace GOE
{
    /// <summary>
    /// 临时模拟cdn配置
    /// </summary>
    public class CDNSetting_TempTestVersion : _ATCDNConfigSetting<TempTestConfig>
    {
        private static CDNSetting_TempTestVersion _g_instance;
        [NotNull]
        public static CDNSetting_TempTestVersion instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new CDNSetting_TempTestVersion();
                return _g_instance;
            }
        }

        protected CDNSetting_TempTestVersion() : base("TempTestVersion")
        {
        }

        /// <summary>
        /// 获取下载的文件路径
        /// </summary>
        /// <returns></returns>
        protected override string _getDownloadFilePath()
        {
#if UNITY_IPHONE
            return "/version/ios";
#elif UNITY_ANDROID
            return "/version/android";
#elif UNITY_STANDALONE || UNITY_EDITOR
            return "/version/pc";
#endif
        }

        /// <summary>
        /// 获取下载操作的处理对象
        /// </summary>
        /// <returns></returns>
        protected override _ATALCDNDownloadProvider _getURLDownloaderProvider()
        {
            return CDNURLProvider_TempTest.instance;
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
        
        public string getGameResUpdateUrl(string _m_sResUpdateRootURL)
        {
            string gameResURL = Game.instance.mainCamera.platInfo.gameResURL;

            if (!string.IsNullOrEmpty(_m_sResUpdateRootURL)
                && null != data && null != data.config
                && !string.IsNullOrEmpty(data.config.gameUpdatePath))
                gameResURL = string.Format("{0}/game/{1}", _m_sResUpdateRootURL, data.config.gameUpdatePath);

            return getDeviceUrl(gameResURL);
        }
        
        public string getRefdataResUpdateUrl(string _m_sResUpdateRootURL)
        {
            string refdataUrl = Game.instance.mainCamera.platInfo.refdataResURL;
            
            if (!string.IsNullOrEmpty(_m_sResUpdateRootURL)
                && null != data && null != data.config
                && !string.IsNullOrEmpty(data.config.refdataUpdatePath))
                refdataUrl = string.Format("{0}/refdata/{1}", _m_sResUpdateRootURL, data.config.refdataUpdatePath);
            
            return getDeviceUrl(refdataUrl);
        }
        
        public string getVideoResUpdateUrl(string _m_sResUpdateRootURL)
        {
            string videoResURL = Game.instance.mainCamera.platInfo.videoResURL;
            
            if (!string.IsNullOrEmpty(_m_sResUpdateRootURL)
                && null != data && null != data.config
                && !string.IsNullOrEmpty(data.config.videoUpdatePath))
                videoResURL = string.Format("{0}/video/{1}", _m_sResUpdateRootURL, data.config.videoUpdatePath);
            
            return getDeviceUrl(videoResURL);
        }
        
         /// <summary>
        /// 获取hotfix的更新下载地址
        /// </summary>
        /// <param name="_m_sResUpdateRootURL"></param>
        /// <returns></returns>
        public string getHotfixUpdateUrl(string _m_sResUpdateRootURL)
        {
            string hotfixURL = Game.instance.mainCamera.platInfo.hotfixURL;
           
            //有cdn用cdn的
            if (!string.IsNullOrEmpty(_m_sResUpdateRootURL)
                && null != data && null != data.config
                && !string.IsNullOrEmpty(data.config.hotfixUpdatePath))
                hotfixURL = string.Format("{0}/hotfix/{1}", _m_sResUpdateRootURL, data.config.hotfixUpdatePath);
            
            return getDeviceUrl(hotfixURL);
        }
        
        /// <summary>
        /// 获取injectFix的更新下载地址
        /// </summary>
        /// <param name="_m_sResUpdateRootURL"></param>
        /// <returns></returns>
        public string getInjectFixUpdateUrl(string _m_sResUpdateRootURL)
        {
            string injectFixURL = Game.instance.mainCamera.platInfo.injectFixURL;
           
            //有cdn用cdn的
            if (!string.IsNullOrEmpty(_m_sResUpdateRootURL)
                && null != data && null != data.config
                && !string.IsNullOrEmpty(data.config.injectFixUpdatePath))
                injectFixURL = string.Format("{0}/injectfix/{1}", _m_sResUpdateRootURL, data.config.injectFixUpdatePath);
            
            return getDeviceUrl(injectFixURL);
        }
        
        /// <summary>
        /// 获取音效资源的更新下载地址
        /// </summary>
        /// <param name="_m_sResUpdateRootURL"></param>
        /// <returns></returns>
        public string getAudioResUpdateUrl(string _m_sResUpdateRootURL)
        {
            string audioResURL = Game.instance.mainCamera.platInfo.audioResURL;

            if (!string.IsNullOrEmpty(_m_sResUpdateRootURL)
                && null != data && null != data.config
                && !string.IsNullOrEmpty(data.config.audioUpdatePath))
                audioResURL = string.Format("{0}/audio/{1}", _m_sResUpdateRootURL, data.config.audioUpdatePath);

            return audioResURL;
        }
        
        public string getDeviceUrl(string _url)
        {
            if (string.IsNullOrEmpty(_url))
                return _url;

            string fianlUrl = string.Empty;
#if UNITY_IPHONE
        fianlUrl = _url + "/ios";
#elif UNITY_ANDROID
        fianlUrl = _url + "/android";
#elif UNITY_WEBPLAYER
        fianlUrl = _url + "/web";
#elif UNITY_STANDALONE_WIN
            fianlUrl = _url + "/pc";
#endif
            return fianlUrl;
        }
    }
}
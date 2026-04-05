using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using JetBrains.Annotations;
using LitJson;

namespace GOE
{
    /// <summary>
    /// 客户端版本基本配置的本地缓存管理对象
    /// </summary>
    public class CDNSetting_ClientConfigInfo : _ATCDNConfigSetting<ClientConfigInfo>
    {
        private static CDNSetting_ClientConfigInfo _g_instance;
        [NotNull]
        public static CDNSetting_ClientConfigInfo instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new CDNSetting_ClientConfigInfo();
                return _g_instance;
            }
        }

        protected CDNSetting_ClientConfigInfo() : base("ClientConfigInfo")
        {
            
        }

        //平台id
        public int platformId
        {
            get
            {
                if (null == data || null == data.config)
                    return 0;
                return data.config.platformId;
            }
        }

        /// <summary>
        /// 发送埋点优先级
        /// </summary>
        public int phpADLevel
        {
            get
            {
                if (null == data || null == data.config)
                    return 99;
                return data.config.phpADLevel;
            }
        }

        /// <summary>
        /// 是否开启埋点
        /// </summary>
        public bool isOpenPHPAD
        {
            get
            {
#if UNITY_EDITOR
                //Editor下，如果没有设置CDN地址，直接当作关闭埋点
                if (!Game.instance.isUseCdn)
                    return false;
#endif

                //如果CDN下载了的话，就用CDN上的埋点开关
                if (isInited && data != null && data.config != null)
                    return data.config.isOpenPHPAD;
                else 
                    return true; //默认开启埋点
            }
        }

        /// <summary>
        /// 是否正式服平台
        /// </summary>
        public bool isFormalPlatform
        {
            get
            {
#if UNITY_EDITOR
                //Editor下，如果没有设置CDN地址，不是正式服平台
                if (!Game.instance.isUseCdn)
                    return false;
#endif

                //如果CDN下载了的话，就用CDN判断是否正式服平台
                if (isInited && data != null && data.config != null)
                    return data.config.isFormalPlatform;
                else 
                    return true; //默认是正式服
            }
        }
        
        /// <summary>
        /// 获取下载的文件路径
        /// </summary>
        /// <returns></returns>
        protected override string _getDownloadFilePath()
        {
#if UNITY_IPHONE
            return string.Format("/client_config/common/{0}/{1}", ClientVersionSetting.instance.ClientVersionInfo.clientId, "ios");
#elif UNITY_ANDROID
            return string.Format("/client_config/common/{0}/{1}", ClientVersionSetting.instance.ClientVersionInfo.clientId, "android");
#elif UNITY_STANDALONE || UNITY_EDITOR
            return string.Format("/client_config/common/{0}/{1}", ClientVersionSetting.instance.ClientVersionInfo.clientId, "pc");
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

                //发送埋点-CDN-初始化ClientConfig失败
                GCommon.sendStepReport(TraceConst.INIT_CLIENT_CONFIG_FAIL);
            }
            else
            {
                //发送埋点-CDN-初始化ClientConfig成功
                GCommon.sendStepReport(TraceConst.INIT_CLIENT_CONFIG_SUC);
            }
        }

        //校验是否需要重新下载
        private void _checkNeedReset()
        {
            //保存的版本号跟当前游戏版本号一致，不需一致要清空数据
            if (ClientVersionSetting.instance.ClientVersionInfo.clientId ==
                GameSetting.instance.getLastCdnClientVersion())
            {
                return;
            }

            clearData();
            _resetAllCDNInfo();
        }

        /// <summary>
        /// 初始化客户端基本配置部分
        /// 此对象的初始化需要特殊处理，当在提审服的时候，需要重新进行全新的下载，以此确保配置一定是最新的
        /// 保证用户不会进入提审服
        /// </summary>
        public void initClientConfigInfo(Action _initDelegate)
        {
            //没走cdn直接处理
            if (!Game.instance.isUseCdn)
            {
                if (null != _initDelegate)
                    _initDelegate();
                return;
            }

            //发送埋点-CDN-开始初始化ClientConfig
            GCommon.sendStepReport(TraceConst.START_INIT_CLIENT_CONFIG);
            
            //判断是否需要先清除一些旧配置
            _checkNeedReset();
            
            //尝试初始化本地配置，这边不初始化本地配置的话oldPlatformId不存在
            init();
            //默认设置旧的平台Id，用于判断平台Id是否一致
            int oldPlatformId = -1;

            //记录平台id用于判断是否切换了平台，以清空cdn缓存
            if(null != data && null != data.config)
                oldPlatformId = data.config.platformId;
            commonDownload(
                () =>
                {
                    //找不到数据去重试
                    if (null == data)
                    {
                        //发送埋点-CDN-开始初始化ClientConfig-失败，弹窗对话框等待玩家确认重试
                        GCommon.sendStepReport(TraceConst.START_INIT_CLIENT_CONFIGF_FAIL_POP_WND);
                        
                        //找不到对应版本号的CDN
                        NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.login_loginFailedTipContent_cdn), 
                            TextTranslate.instance.getLanguage(TransKeyConst.ok)
                            , () =>
                            {
                                //CDN-开始初始化ClientConfig-失败，对话框点击确认重试
                                GCommon.sendStepReport(TraceConst.START_INIT_CLIENT_CONFIGF_FAILCLICK);
                                
                                initClientConfigInfo(_initDelegate);
                            });
                    }
                    else
                    {
                        //如果平台id发生了变化说明提审和正式平台切换了，需要清空本地cdn缓存数据
                        if (oldPlatformId != -1 && data.config != null && data.config.platformId != oldPlatformId)
                        {
                            //发送埋点-CDN-CDN-开始初始化platformid发生变化，需要清除其他cdn
                            GCommon.sendStepReport(TraceConst.CLIENT_CONFIG_PLATFORM_ID_CHG);
                            
                            //此时出现了平台切换，需要重置所有其他数据
                            _resetAllCDNInfo();
                        }
                        GameSetting.instance.setLastCdnClientVersion(ClientVersionSetting.instance.ClientVersionInfo.clientId);
                         
                        //注册回调处理，后注册可以确保数据先被处理了
                        regCDNInitDelegate(_initDelegate);   
                    }
                });
        }

        /// <summary>
        /// 重置所有CDN信息，重新下载处理
        /// </summary>
        protected void _resetAllCDNInfo()
        {
            //清除所有非client config的数据
            CDNSetting_LoginServerUrlInfo.instance.clearData();
            CDNSetting_GameBeforeNoticeInfo.instance.clearData();
            CDNSetting_AreaInfo.instance.clearData();
            CDNSetting_ClientPlatformConfig.instance.clearData();
            CDNSetting_GameAfterNoticeInfo.instance.clearData();
            CDNSetting_ClientFunctionSwitchConfig.instance.clearData();
            CDNSetting_ServerListInfo.instance.clearData();
            CDNSetting_AnnouncementInfo.instance.clearData();
        }

        /// <summary>
        /// 检测是否需要使用root下载处理
        /// 在一些配置，如提审服切换的时候需要根据当前数据判断是否需要进行强制下载
        /// 默认不进行
        /// </summary>
        /// <returns></returns>
        protected override bool _checkNeedRootLoad()
        {
            if (null == data || null == data.config)
                return false;

            //送审平台的情况下，要求强制取最新，确保不会因为意外进入送审
            if (!data.config.isFormalPlatform)
                return true;

            return false;
        }

        #region 获取相关下载地址接口
        
        /// <summary>
        /// 带入外部的下载URL头部分
        /// </summary>
        /// <param name="_m_sResUpdateRootURL"></param>
        /// <returns></returns>
        public string getAreaResUpdateURL(string _m_sResUpdateRootURL)
        {
            //先设置默认值
            string areaResURL = Game.instance.mainCamera.platInfo.areaResURL;

            if (!string.IsNullOrEmpty(_m_sResUpdateRootURL)
                && null != data && null != data.config 
                && !string.IsNullOrEmpty(data.config.areaUpdatePath))
                areaResURL = string.Format("{0}/area/{1}", _m_sResUpdateRootURL, data.config.areaUpdatePath);

            return getDeviceUrl(areaResURL);
        }

        public string getGameResUpdateUrl(string _m_sResUpdateRootURL)
        {
            string gameResURL = Game.instance.mainCamera.platInfo.gameResURL;

            if (!string.IsNullOrEmpty(_m_sResUpdateRootURL)
                && null != data && null != data.config
                && !string.IsNullOrEmpty(data.config.gameUpdatePath))
                gameResURL = string.Format("{0}/game/{1}", _m_sResUpdateRootURL, data.config.gameUpdatePath);

            //如果有配置debugcdn文件用文件的
            if (Game.instance.IsDebugCDN && null != Game.instance.debugCdnSetting && Game.instance.debugCdnSetting.ContainsKey(DebugCdnConst.GAMERES_URL))
            {
                gameResURL = Game.instance.debugCdnSetting[DebugCdnConst.GAMERES_URL];
            }
            return getDeviceUrl(gameResURL);
        }

        /***************
         * 获取特殊资源更新url
         **/
        public string getSpecialResUpdateURL(string _m_sResUpdateRootURL)
        {
            string specialResURL = Game.instance.mainCamera.platInfo.specialResURL;

            if (!string.IsNullOrEmpty(_m_sResUpdateRootURL)
                && null != data && null != data.config
                && !string.IsNullOrEmpty(data.config.noticeUpdatePath))
                specialResURL = string.Format("{0}/special/{1}", _m_sResUpdateRootURL, data.config.noticeUpdatePath);

            //如果有配置debugcdn文件用文件的
            if (Game.instance.IsDebugCDN && null != Game.instance.debugCdnSetting && Game.instance.debugCdnSetting.ContainsKey(DebugCdnConst.NOTICERES_URL))
            {
                specialResURL = Game.instance.debugCdnSetting[DebugCdnConst.NOTICERES_URL];
            }

            return getDeviceUrl(specialResURL);
        }
        
        /// <summary>
        /// 获取音效资源更新下载地址
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

            //如果有配置debugcdn文件用文件的
            if (Game.instance.IsDebugCDN && null != Game.instance.debugCdnSetting && Game.instance.debugCdnSetting.ContainsKey(DebugCdnConst.AUDIO_URL))
            {
                audioResURL = Game.instance.debugCdnSetting[DebugCdnConst.AUDIO_URL];
            }
            return audioResURL;
        }

        public string getRefdataResUpdateUrl(string _m_sResUpdateRootURL)
        {
            string refdataUrl = Game.instance.mainCamera.platInfo.refdataResURL;
            
            if (!string.IsNullOrEmpty(_m_sResUpdateRootURL)
                && null != data && null != data.config
                && !string.IsNullOrEmpty(data.config.refdataUpdatePath))
                refdataUrl = string.Format("{0}/refdata/{1}", _m_sResUpdateRootURL, data.config.refdataUpdatePath);

            //如果有配置debugcdn文件用文件的
            if (Game.instance.IsDebugCDN && null != Game.instance.debugCdnSetting && Game.instance.debugCdnSetting.ContainsKey(DebugCdnConst.REFDATA_URL))
            {
                return getDeviceUrl(Game.instance.debugCdnSetting[DebugCdnConst.REFDATA_URL]);
            }

            return getDeviceUrl(refdataUrl);
        }
        
        public string getVideoResUpdateUrl(string _m_sResUpdateRootURL)
        {
            string videoResURL = Game.instance.mainCamera.platInfo.videoResURL;
            
            if (!string.IsNullOrEmpty(_m_sResUpdateRootURL)
                && null != data && null != data.config
                && !string.IsNullOrEmpty(data.config.videoUpdatePath))
                videoResURL = string.Format("{0}/video/{1}", _m_sResUpdateRootURL, data.config.videoUpdatePath);

            //如果有配置debugcdn文件用文件的
            if (Game.instance.IsDebugCDN && null != Game.instance.debugCdnSetting && Game.instance.debugCdnSetting.ContainsKey(DebugCdnConst.VIDEO_URL))
            {
                return getDeviceUrl(Game.instance.debugCdnSetting[DebugCdnConst.VIDEO_URL]);
            }

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

            //如果有配置debugcdn文件用文件的
            if (Game.instance.IsDebugCDN && null != Game.instance.debugCdnSetting && Game.instance.debugCdnSetting.ContainsKey(DebugCdnConst.HOTFIX_URL))
            {
                hotfixURL = Game.instance.debugCdnSetting[DebugCdnConst.HOTFIX_URL];
            }

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

            //如果有配置debugcdn文件用文件的
            if (Game.instance.IsDebugCDN && null != Game.instance.debugCdnSetting && Game.instance.debugCdnSetting.ContainsKey(DebugCdnConst.INJECTFIX_URL))
            {
                injectFixURL = Game.instance.debugCdnSetting[DebugCdnConst.INJECTFIX_URL];
            }

            return getDeviceUrl(injectFixURL);
        }

        /// <summary>
        /// 获取埋点地址
        /// </summary>
        /// <returns></returns>
        public string getPhpAdUrl()
        {
            //先设置默认值
            string adURL = Game.instance.mainCamera.platInfo.phpADDefaultURL;

            //读取cdn配置
            if (null != data && null != data.config && !string.IsNullOrEmpty(data.config.phpADUrl))
                adURL = data.config.phpADUrl;

            return adURL;
        }
        
        /// <summary>
        /// 获取热更的路径标记，用于区分是否需要再次更新
        /// </summary>
        public string getHotfixPathTag()
        {
            if (null != data && null != data.config)
                return data.config.hotfixUpdatePath;
            else
                return string.Empty;
        }

        /// <summary>
        /// 获取Injectfix的路径标记，用于区分是否需要再次更新
        /// </summary>
        public string getInjectFixPathTag()
        {
            if (null != data && null != data.config)
                return data.config.injectFixUpdatePath;
            else
                return string.Empty;
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

        //获取设备字符
        private string _getDeviceStr()
        {
            string fianlUrl = string.Empty;
#if UNITY_IPHONE
        fianlUrl = "ios";
#elif UNITY_ANDROID
        fianlUrl = "android";
#elif UNITY_WEBPLAYER
        fianlUrl = "web";
#elif UNITY_STANDALONE_WIN
            fianlUrl = "pc";
#endif
            return fianlUrl;
        }
        #endregion

        #region 版本号变更检查

        /// <summary>
        /// CDN上的ClientConfig版本变更处理
        /// </summary>
        public void onCDNClientConfigVersionChg()
        {
            if (!Game.instance.isUseCdn || data == null || data.config == null)
                return;

            ClientConfigInfo oriClientConfigInfo = data.config;
            commonDownload(() =>
            {
                if (data == null || data.config == null)
                    return;

                //判断5个资源中是否有更新，有则按照clientHotfixUpdateType更新类型来提示
                if (oriClientConfigInfo.gameUpdatePath != data.config.gameUpdatePath
                    || oriClientConfigInfo.refdataUpdatePath != data.config.refdataUpdatePath
                    || oriClientConfigInfo.audioUpdatePath != data.config.audioUpdatePath
                    || oriClientConfigInfo.videoUpdatePath != data.config.videoUpdatePath
                    || oriClientConfigInfo.injectFixUpdatePath != data.config.injectFixUpdatePath
                    || oriClientConfigInfo.hotfixUpdatePath != data.config.hotfixUpdatePath
                    || oriClientConfigInfo.clientHotfixUpdateType != data.config.clientHotfixUpdateType)
                {
                    _onCDNClientConfigHasNewRes();
                    //如果injectFix有更新，重新加载
                    if (oriClientConfigInfo.injectFixUpdatePath != data.config.injectFixUpdatePath)
                    {
                        //TODO
                    }

                }
            });
        }

        //成功获取最新更新的CDNClientConfig，并且5个资源中，任意一个有新的版本
        //根据clientHotfixUpdateType，决定处理方式:
        private void _onCDNClientConfigHasNewRes()
        {
            //禁用esc
            QueueMgr.instance.CloseRollBack(NodeESC_Const.C_QUEUE_ESC_RES_HOTFIX_UPDATE);

            //客户端热更资源提示类型  1 强；2：否；3：提示
            switch (data.config.clientHotfixUpdateType)
            {
                //弹窗，并只能重启游戏
                case 1:
                    NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.cdn_forceUpdateClientHotfixTip_none)//发现游戏更新，请重启游戏
                        , TextTranslate.instance.getLanguage(TransKeyConst.cdn_updateImImmediately_none)//立即更新
                        , Application.Quit);
                    break;
                //不提示
                case 2:
                    QueueMgr.instance.OpenRollBack(NodeESC_Const.C_QUEUE_ESC_RES_HOTFIX_UPDATE);//开启esc
                    break;
                //弹窗提示，并可以选择是否重启游戏
                case 3:
                    NPMesMgr.instance.showTwoBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.cdn_promptUpdateClientHotfixTip_none)//发现游戏更新，是否立即重启
                        , TextTranslate.instance.getLanguage(TransKeyConst.cdn_promptUpdateHotfixCancel_none)
                        , ()=>
                        {
                            QueueMgr.instance.OpenRollBack(NodeESC_Const.C_QUEUE_ESC_RES_HOTFIX_UPDATE);//开启esc
                        }
                        , TextTranslate.instance.getLanguage(TransKeyConst.cdn_updateImImmediately_none)//立即更新
                        , Application.Quit);
                    break;
                default:
                    QueueMgr.instance.OpenRollBack(NodeESC_Const.C_QUEUE_ESC_RES_HOTFIX_UPDATE);//开启esc
                    break;
            }
        }

        #endregion
    }
}
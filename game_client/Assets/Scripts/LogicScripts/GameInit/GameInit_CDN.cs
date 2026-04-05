using UnityEngine;
using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using LitJson;

using ALPackage;
using NPEnum;


namespace GOE
{
    /// <summary>
    /// 游戏CDN部分初始化过程处理函数
    /// 此过程是无法进行第二次处理的
    /// </summary>
    public class GameInit_CDN : _AGameInitProcess
    {
        private static GameInit_CDN _g_instance = new GameInit_CDN();
        public static GameInit_CDN instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GameInit_CDN();
                return _g_instance;
            }
        }
        
        private static readonly string _m_gProcessName = "GameInit_CDN";
        public static string gProcessName { get { return _m_gProcessName; } }

        protected GameInit_CDN()
        {
        }


        /// <summary>
        /// 强制清除大区相关的cdn配置，用于切换大区时候调用
        /// </summary>
        public void forceClearAreaCdn()
        {
            CDNSetting_ClientPlatformConfig.instance.clearData();
            CDNSetting_AreaInfo.instance.clearData();
            CDNSetting_GameBeforeNoticeInfo.instance.clearData();
            CDNSetting_LoginServerUrlInfo.instance.clearData();
            CDNSetting_ServerListInfo.instance.clearData();
            CDNSetting_AnnouncementInfo.instance.clearData();

            _onReset();
        }

        /// <summary>
        /// 强制清除服务器相关的cdn配置，用于切换服务器时候调用
        /// </summary>
        public void forceClearServerCdn()
        {
            CDNSetting_AnnouncementInfo.instance.clearData();
        }

        /// <summary>
        /// 返回子进度队列
        /// </summary>
        /// <returns></returns>
        protected override _IALProgressnterface[] getChildProcess()
        {
            return new _IALProgressnterface[] {
                CDNSetting_ClientConfigInfo.instance.processObj
                , CDNSetting_AreaInfo.instance.processObj
                , CDNSetting_ClientPlatformConfig.instance.processObj
                // , CDNSetting_GameAfterNoticeInfo.instance.processObj
                , CDNSetting_GameBeforeNoticeInfo.instance.processObj
                , CDNSetting_LoginServerUrlInfo.instance.processObj
                , CDNSetting_ServerListInfo.instance.processObj
                , CDNSetting_AnnouncementInfo.instance.processObj
            };
        }

        /// <summary>
        /// 是否允许重置，默认允许。
        /// 子类可以重载
        /// 
        /// CDN资源加载不允许重置状态
        /// </summary>
        protected override bool canReset { get { return false; } }

        /// <summary>
        /// 重置数据部分
        /// </summary>
        protected override void _resetData()
        {
        }

        /// <summary>
        /// 加载处理函数
        /// </summary>
        /// <returns></returns>
        protected override void _dealInit(Action _doneDelegate)
        {
            //不走cdn直接返回
            if (!Game.instance.isUseCdn)
            {
                //发送激活埋点
                _sendStepActivateReport();

                if (_doneDelegate != null) 
                    _doneDelegate();
                return;
            }

            //发送埋点-开始初始化CDN
            GCommon.sendStepReport(TraceConst.START_INIT_CDN);
            
            //使用底层Process进行初始化流程
            ALProcess basicProcess = ALProcess.CreateProcess(_m_gProcessName);

            //首先进行初始化步骤处理
            basicProcess
                //初始化cdn的预处理
                //此处单独处理优先需要获取的信息
                .addDelegateProcess(CDNSetting_ClientConfigInfo.instance.initClientConfigInfo, "cdn_client_config_init")//初始化客户端参数（得到平台Id）
                .addDelegateProcess(_dealGetSDKIPInfo, "sdk_ip_info_init")//等待SDK初始化完成IP地区信息
                .addDelegateProcess(_dealCountryAreaInit, "country_area_info_init")//等待平台资源国家大区表初始化完
                .addDelegateProcess(CDNSetting_AreaInfo.instance.initArea, "cdn_client_area_init")//初始化大区列表
                
                .addProcess(_dealSubProcessAfterClientConfig_pre())

                //后续所有cdn下载都走并行处理，配置来源是一个地方
                .addProcess(_dealSubProcessAfterClientConfig_after())

                //进入正常视图
                .addProcess(() =>
                {
                    //发送埋点-初始化CDN完成
                    GCommon.sendStepReport(TraceConst.INIT_CDN_DONE);

                    //发送激活埋点
                    _sendStepActivateReport();

                    _doneDelegate?.Invoke();
                });

            //开始执行
            basicProcess.dealProcess(new GameInitMonitor(_m_gProcessName, 0));
        }

        /// <summary>
        /// 等待SDK初始化完成IP地区信息
        /// </summary>
        /// <param name="_onComplete"></param>
        protected void _dealGetSDKIPInfo(Action _onComplete)
        {
            SDKMgr.instance.regGetIPDoneDelegate(_info =>
            {
                _onComplete?.Invoke();
            });
        }

        /// <summary>
        /// 等待国家大区表初始化完
        /// </summary>
        /// <param name="_onComplete"></param>
        protected void _dealCountryAreaInit(Action _onComplete)
        {
            GameInit_PlatResInit.instance.init(() =>
            {
                //发送埋点-开始初始化国家配表
                GCommon.sendStepReport(TraceConst.INIT_PLAT_COUNTRY_REF);
                
                PCountryAreaInfo.instance.init(() =>
                {
                    //发送埋点-开始初始化国家配表完成
                    GCommon.sendStepReport(TraceConst.INIT_PLAT_COUNTRY_REF_DONE);
                    
                    if (_onComplete != null) 
                        _onComplete();
                },_onComplete);
            });
        }

        /// <summary>
        /// 在客户端版本信息下载完成之后的并行处理流程1,优先级高于后续处理，最好提前加载完毕
        /// </summary>
        /// <returns></returns>
        protected ALStepProcess _dealSubProcessAfterClientConfig_pre()
        {
            //使用底层Process进行初始化流程
            ALStepProcess stepProcess = ALStepProcess.CreateStepProcess("cdn_step_process_pre");

            //首先进行初始化步骤处理
            stepProcess
                //.addDelegateProcess(CDNSetting_VersionUpdateNoticeInfo.instance.showClientVersionUpdateNotic) //显示版本更新公告

                .addProcess(_onDone=>
                {
                    //发送埋点-CDN-开始初始化登录服务器地址
                    GCommon.sendStepReport(TraceConst.START_INIT_LOGIN_SERVER_URL);

                    CDNSetting_LoginServerUrlInfo.instance.commonDownload(_onDone);
                }) // 初始化登录服务器地址（依赖平台/分区）
                .addProcess(_onDone=>
                {
                    //发送埋点-CDN-开始初始化登录前公告
                    GCommon.sendStepReport(TraceConst.START_INIT_GAME_BEFORE_NOTICE);

                    CDNSetting_GameBeforeNoticeInfo.instance.commonDownload(_onDone);
                }); //下载登录前公告，（依赖平台/分区）


            return stepProcess;
        }
        /// <summary>
        /// 在客户端版本信息下载完成之后的并行处理流程
        /// </summary>
        /// <returns></returns>
        protected ALStepProcess _dealSubProcessAfterClientConfig_after()
        {
            //使用底层Process进行初始化流程
            ALStepProcess stepProcess = ALStepProcess.CreateStepProcess("cdn_step_process_after");

            //首先进行初始化步骤处理
            stepProcess
                .addProcess(CDNSetting_ClientPlatformConfig.instance.commonDownload) //初始化客户端平台信息地址，一般是登录和支付信息（依赖平台/渠道/分区）
                // .addProcess(_onDone=>
                // {
                //     //发送埋点-CDN-开始初始化登录后公告
                //     GCommon.sendStepReport(TraceConst.START_INIT_GAME_AFTER_NOTICE);
                //
                //     CDNSetting_GameAfterNoticeInfo.instance.commonDownload(_onDone);
                // }) //下载登录后公告，（依赖平台/分区）
                // .addProcess(CDNSetting_ClientFunctionSwitchConfig.instance.commonDownload) //功能开关
                // .addProcess(CDNSetting_IllegalWordInfo.instance.commonDownload) //屏蔽字库
                // .addProcess(CDNSetting_HorseLampInfo.instance.commonDownload) //跑马灯
                .addProcess(_onDone=>
                {
                    //服务器列表
                    //发送埋点-CDN-开始初始化服务器列表
                    GCommon.sendStepReport(TraceConst.START_INIT_SERVER_LIST);

                    CDNSetting_ServerListInfo.instance.commonDownload(_onDone);
                });

            return stepProcess;
        }

        /// <summary>
        /// 处理需要登录服务器之后下载的CDN，与serverID相关
        /// </summary>
        public void dealEnterServerCDN()
        {
            if (!Game.instance.isUseCdn)
                return;

            //发送埋点-CDN-开始初始化服务器id相关CDN
            GCommon.sendStepReport(TraceConst.START_INIT_SERVER_RELATE_CDN.setMarkParam(GameInit_SelectServer.instance.loginServerLogicId));
            //运营公告
            CDNSetting_AnnouncementInfo.instance.initAnnouncement();
        }

        //发送激活埋点
        private void _sendStepActivateReport()
        {
            //发送激活埋点
            if (!GameSetting.instance.getIsSendActivate())
            {
                SDKMgr.instance.trace_gameActivate();
                GameSetting.instance.setIsSendActivate(true);

                //发送系统信息埋点
                GCommon.sendStepReport(TraceConst.SYSTEM_INFO.setMark(GCommon.getSystemInfo()));
            }
        }
    }
}

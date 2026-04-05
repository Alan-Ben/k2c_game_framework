using UnityEngine;
using System;
using System.Diagnostics;
using System.Text;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 游戏初始化过程处理函数
    /// </summary>
    public class GameInit : _AGameInitProcess
    {
        private static GameInit _g_instance = new GameInit();
        public static GameInit instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GameInit();

                return _g_instance;
            }
        }

        //当前的处理过程对象
        private ALProcess _m_pProcessObj;
        //维护公告播放索引
        private int _m_iMaintainNoticeIndex = 0;
        //维护公告是否可以进入游戏
        private bool _m_bMaintainNoticeCanEnterGame = true;
        //初始化商品档位本地化信息重试次数
        private const int _m_iInitLocalProductReryCount = 5;

        protected GameInit()
        {
            _m_pProcessObj = null;
        }

        /// <summary>
        /// 返回子进度队列
        /// </summary>
        /// <returns></returns>
        protected override _IALProgressnterface[] getChildProcess()
        {
            return new _IALProgressnterface[] {
                GameInit_CDN.instance.processNode
                , GameInit_GameResInit.instance.processNode
                , GameInit_LoginProcess.instance.processNode
                , GameInit_PlatResInit.instance.processNode
                , GameInit_UpdateGameRes.instance.processNode};
        }

        /// <summary>
        /// 重置数据部分
        /// </summary>
        protected override void _resetData()
        {
            //重置维护公告相关变量
            _m_iMaintainNoticeIndex = 0;
            _m_bMaintainNoticeCanEnterGame = true;

            //中断过程对象
            if (null != _m_pProcessObj)
                _m_pProcessObj.stopProcess();
            _m_pProcessObj = null;

            //退出所有相关流程
            GameInit_CDN.instance.reset();
            GameInit_LoginProcess.instance.reset();
            GameInit_SelectServer.instance.reset();
            GameInit_Other.instance.reset();
            GameInit_LanguageResLoad.instance.reset();
        }

        /// <summary>
        /// 加载处理函数
        /// </summary>
        /// <returns></returns>
        protected override void _dealInit(Action _doneDelegate)
        {
            //使用底层Process进行初始化流程
            _m_pProcessObj = ALProcess.CreateProcess("main");

            //首先进行初始化步骤处理
            _m_pProcessObj
                .addProcess(_dealClientVersionCheck, "client_version_check")
                .addProcess(_initDebug, "init_debug")

                //初始化平台资源，要在cdn初始化之前，不然cdn失败的弹窗不会弹出来
                .addDelegateProcess(GameInit_PlatResInit.instance.init, GameInit_PlatResInit.gProcessName)
                
                //初始化cdn的预处理，此处不需要等待cdn加载完成，所以制作执行函数
                //后续每个需要使用cdn的地方会根据各自需要进行监听处理
                .addProcess(GameInit_CDN.instance.init, GameInit_CDN.gProcessName)
                
                //尝试进入界面
                //初始化平台数据资源，并进入界面
                .addDelegateProcess(_enterLoginNode, "init_login")
                .addProcess(_initVideoJudgerEx, "_initVideoJudgerEx")
                // .addDelegateProcess(_initVideoJudger, "_initVideoJudger")
                //初始化SDK
                .addDelegateProcess(_dealInitSDK, "init_sdk")
                
                //确保cdn初始化完毕
                .addDelegateProcess(GameInit_CDN.instance.init, GameInit_CDN.gProcessName)
                //判断是否强更，如果需要强更或推荐更新则需要弹出不同的对话框
                .addDelegateProcess(_judgeVersionUpdate, "judge_version_update")

                //判断是否展示隐私协议
                .addDelegateProcess(_showPolicy, "show_policy")

                //进入进度条显示节点
                .addDelegateProcess(_showLoadingBk, "show_loading")

                //开始其他资源部分初始化
                .addProcess(GameInit_UpdateGameRes.instance.init, GameInit_UpdateGameRes.gProcessName)

                //开始热更补丁加载
                .addProcess(GameInit_HotfixLoad.instance.init, GameInit_HotfixLoad.gProcessName)
                
                //设置刷新资源下载更新进度
                .addProcess(() => { NPPGUIWndLoadingBk.instance.setRefresher(GameInit_UpdateGameRes.instance); })
                
                //判断是否有维护公告，有则先弹出
                //TODO 这边维护公告应该只先判断全服的类型，这边拿不到当前要登入的服务器id，不好做单服判断
                //TODO 当前6月版本先判断有维护公告就拦着，不区分区服
                // .addDelegateProcess(_juegeGlobalMaintainNotice)
                //游戏前公告,等其他配置弹窗
                .addProcess(GameInit_Other.instance.init, GameInit_Other.gProcessName)

                //调用登录处理
                //自动登录，此操作会自动登录到GS，过程中的异常都应该重头开始登录处理
                .addProcess(GameInit_LoginProcess.instance.init, GameInit_LoginProcess.gProcessName)
                //初始化导出AI设置
                .addProcess(GameInit_GameResInit.instance.init, GameInit_GameResInit.gProcessName)

                //确保热更补丁加载完毕
                .addDelegateProcess(GameInit_HotfixLoad.instance.init, GameInit_HotfixLoad.gProcessName)
                
                //确保资源更新完毕
                .addDelegateProcess(GameInit_UpdateGameRes.instance.init, GameInit_UpdateGameRes.gProcessName)

                //确保资源更新完毕
                .addDelegateProcess(GameInit_LanguageResLoad.instance.init, GameInit_LanguageResLoad.gProcessName)
                
                //设置刷新资源初始化更新进度
                .addProcess(() => { NPPGUIWndLoadingBk.instance.setRefresher(GameInit_GameResInit.instance); })

                //初始化showcase
                .addDelegateProcess(GameInit_GameResInit.instance.init, GameInit_GameResInit.gProcessName)
                
                //hotfix 的资源初始化完成
                .addDelegateProcess(GameInit_HotfixLoad.instance.initHotfixRes, GameInit_HotfixLoad.gProcessName)
                
                //设置刷新进入游戏进度
                .addProcess(() => { NPPGUIWndLoadingBk.instance.setRefresher(GameInit_LoginProcess.instance); })

                //等待登录操作处理完毕后弹出相关的选服等操作界面
                .addDelegateProcess(GameInit_LoginProcess.instance.init, GameInit_LoginProcess.gProcessName)
                //初始化商品档位本地化信息
                .addProcess(_initLocalProduct, "_initLocalProduct")
                //进入选服操作界面
                .addDelegateProcess(GameInit_SelectServer.instance.init, GameInit_SelectServer.gProcessName)
                //结束流程
                .addProcess(()=> {
                    //重置变量
                    _m_pProcessObj = null;

                    if (null != _doneDelegate)
                        _doneDelegate();
                });

            //开始执行
            _m_pProcessObj.dealProcess(new GameInitMonitor("main", 0));
        }

        /// <summary>
        /// 客户端最新版本
        /// </summary>
        protected void _dealClientVersionCheck()
        {
            //EvenTrackingListener.instance.sendStepReport(EvenConst.INIT_CLIENT_VERSION,$"客户端版本:{GameSetting.instance.lastClientVersion}->{Application.version}");
            GameSetting.instance.setLastClientVersion(Application.version);
        }
        
        //直接设置值，不进行检测流程
        protected void _initVideoJudgerEx()
        {
            //设备基准参考值
            long judgePixels = (long)SystemInfo.graphicsMemorySize * 1024 * 1024 / 300;
            ALVideoLoadLegalJudger.instance.initMaxLoadPixels(judgePixels);
        }

        //获取视频校验上限
        protected void _initVideoJudger(Action _doneDelegate)
        {
            //当前记录值
            long videoMaxLoadPixels = GameSetting.instance.getVideoMaxLoadPixels();
            //设备基准参考值
            long judgePixels = (long)SystemInfo.graphicsMemorySize * 1024 * 1024 / 200;
            
            //如果校验过3次以上，或者当前存储的最大加载像素大于等于建议值则不需要再检测
            if ((videoMaxLoadPixels > 0 && videoMaxLoadPixels > judgePixels) || GameSetting.instance.getVideoMaxLoadPixelsJudgeCount() >= 3)
            {
                ALVideoLoadLegalJudger.instance.initMaxLoadPixels(videoMaxLoadPixels);
                if (_doneDelegate != null) 
                    _doneDelegate();
                return;
            }
            
            string streamingAssetsFullPath = String.Empty;
#if UNITY_ANDROID
    //新版本的unity不需要额外执行replace了
    #if UNITY_2021_3_OR_NEWER
            streamingAssetsFullPath =  (Application.streamingAssetsPath + "/");
    #else
            streamingAssetsFullPath =  (Application.streamingAssetsPath + "/").Replace("jar:file://", "").Replace("!/assets", "!assets");
    #endif
#else
            streamingAssetsFullPath = Application.streamingAssetsPath + "/";
#endif
            streamingAssetsFullPath = ALCommon.directoryInsure(streamingAssetsFullPath);
            streamingAssetsFullPath = $"{streamingAssetsFullPath}video_judge.mp4";
            
            //构造VideoResource
            _IALVideoResource resource = null;
#if AL_AVPRO_V2
#if UNITY_IOS
            resource = new ALVideoResourceURL(streamingAssetsFullPath);
#else
            resource = new ALVideoResourceAVPro2(streamingAssetsFullPath);
#endif
#endif            
            
            Stopwatch timeWatch = new Stopwatch();
            timeWatch.Start();
            
            ALVideoLoadLegalJudger.instance.tryLoadCount(resource, (_count) =>
            {
                //增加校验次数
                GameSetting.instance.addVideoMaxLoadPixelsJudgeCount(1);

                //取最大值存储
                _count = Math.Max(_count, GameSetting.instance.getVideoMaxLoadPixels());
                GameSetting.instance.setVideoMaxLoadPixels(_count);
                ALVideoLoadLegalJudger.instance.initMaxLoadPixels(_count);
                
                //发送埋点-获取视频校验上限
                long time = timeWatch.ElapsedMilliseconds;
                
                Debug.Log_EditorOnly($"初始化获取视频校验上限:{_count},检测时间：{time},设备基准值：{judgePixels}，当前测试次数：{GameSetting.instance.getVideoMaxLoadPixelsJudgeCount()}");
                
                GCommon.sendStepReport(TraceConst.VIDEO_MAX_LOAD_PIXELS.setMarkParam(_count, time, judgePixels, GameSetting.instance.getVideoMaxLoadPixelsJudgeCount()));
                timeWatch.Stop();

                if (_doneDelegate != null) 
                    _doneDelegate();
            }, 0.7f, 850, 1750, 32);
        }
        
        /// <summary>
        /// 初始化Debug相关东西
        /// </summary>
        protected void _initDebug()
        {

#if !UNITY_EDITOR
            //非Editor下，不打印协议
            MainCameraMono.selfInstance.gameSetting.printProtocol = false;
#endif
            //如果存在对应的文件，开启 打印协议的设置
            DebugFunctionFacade.instance.openDebugProtocol.readFileAndRefreshStatus(false);
            //如果存在调试文件，开始各种调试设置
            DebugFunctionFacade.instance.openDebugFunction.readFileAndRefreshStatus(false);
            //cdn额外配置
            DebugFunctionFacade.instance.openDebugCDN.readFileAndRefreshStatus();
            //GPM额外配置
            DebugFunctionFacade.instance.openDebugGPM.readFileAndRefreshStatus();

            //监听错误日志输出
            Application.logMessageReceived += _onLogCallbackHandler;
            //监听程序异常输出
            AppDomain.CurrentDomain.UnhandledException += _onUncaughtExceptionHandler;
        }

        /// <summary>
        /// 进入登录主界面
        /// </summary>
        protected void _enterLoginNode(Action _enterDoneDelegate)
        {
            //发送埋点-开始进入登录主界面
            GCommon.sendStepReport(TraceConst.START_ENTER_LOGIN_NODE);

            //进入加载场景节点
            QueueMgr.instance.AddNode(
                new PLoginMainNode(_enterDoneDelegate));
        }
        
        /// <summary>
        /// 初始化SDK
        /// </summary>
        /// <param name="_enterDoneDelegate"></param>
        protected void _dealInitSDK(Action _enterDoneDelegate)
        {
            //发送埋点-开始初始化SDK
            GCommon.sendStepReport(TraceConst.START_INIT_SDK);

            SDKMgr.instance.init(()=>
            {
                //初始化firebase自定义字符串
                _setFirebaseCrashlyticsDefaultValue();
                //设置AIHelp语言
                GCommon.setAIHelpLanguage();

                _enterDoneDelegate?.Invoke();
            });
        }

        /// <summary>
        /// 判断是否需要更新，如需要更新则弹出提示等待确认
        /// </summary>
        protected void _judgeVersionUpdate(Action _enterDoneDelegate)
        {
            //发送埋点-开始检查是否需要更新客户端
            GCommon.sendStepReport(TraceConst.START_JUDGE_VERSION_UPDATE);

            //没走cdn直接处理
            if (!Game.instance.isUseCdn)
            {
                if (null != _enterDoneDelegate)
                    _enterDoneDelegate();
                return;
            }

            //请求cdn的版本信息
            CDNSetting_ClientConfigInfo.instance.requestData(
                (_clientConfigInfo) => {
                    //判断是否强更
                    if(null == _clientConfigInfo)
                    {
                        //找不到对应版本号的CDN
                        NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.init_cdn_not_found_none), 
                            TextTranslate.instance.getLanguage(TransKeyConst.confirm)
                            , () =>
                            {
                                if (_enterDoneDelegate != null) 
                                    _enterDoneDelegate();
                            });
                        return;
                    }

                    //发送埋点-判断更新客户端方式
                    GCommon.sendStepReport(TraceConst.START_JUDGE_VERSION_UPDATE_STATE.setMarkParam(_clientConfigInfo.clientUpdateType));

                    //判断更新//客户端包更新方式  1 强；2：否；3：提示
                    if (_clientConfigInfo.clientUpdateType == 2)
                    {
                        if (null != _enterDoneDelegate)
                            _enterDoneDelegate();
                        return;
                    }

                    if(1 == _clientConfigInfo.clientUpdateType)
                    {
                        //强更
                        //当前运行的游戏版本过低，为了保障您的游戏体验，需要更新至版本 {0}，请立即前往更新。
                        NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.init_forceUpdateTip_versionStr, _clientConfigInfo.newClientVersion),
                            TextTranslate.instance.getLanguage(TransKeyConst.init_updateGoTo_none),//前往
                            () =>
                            {
                                //发送埋点-玩家选择更新客户端
                                GCommon.sendStepReport(TraceConst.START_JUDGE_VERSION_UPDATE_CONFIRM);
                                GCommon.openURLByBrowser(_clientConfigInfo.newClientUpdateUrl);
                            },
                            true,
                            TextTranslate.instance.getLanguage(TransKeyConst.init_forceUpdateTitle_none));//版本更新
                    }
                    else
                    {
                        //提示更新
                        //当前游戏已有可更新版本（版本号{0}），更新版本将会增强您的部分游戏体验，是否前往更新？
                        NPMesMgr.instance.showTwoBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.init_suggestUpdateTip_versionStr, _clientConfigInfo.newClientVersion),
                            TextTranslate.instance.getLanguage(TransKeyConst.cancel), //取消
                            ()=>
                            {
                                //发送埋点-玩家选择不更新客户端
                                GCommon.sendStepReport(TraceConst.START_JUDGE_VERSION_UPDATE_CANCEL);
                                _enterDoneDelegate?.Invoke();
                            },
                            TextTranslate.instance.getLanguage(TransKeyConst.init_updateGoTo_none),//前往
                            () =>
                            {
                                //发送埋点-玩家选择更新客户端
                                GCommon.sendStepReport(TraceConst.START_JUDGE_VERSION_UPDATE_CONFIRM);
                                GCommon.openURLByBrowser(_clientConfigInfo.newClientUpdateUrl);
                            },
                            true,
                            TextTranslate.instance.getLanguage(TransKeyConst.init_suggestUpdateTitle_none));//版本推荐更新
                    }
                });
        }
            
        /// <summary>
        /// 打开隐私协议界面
        /// </summary>
        protected void _showPolicy(Action _enterDoneDelegate)
        {
            //没走cdn直接处理
            if (!Game.instance.isUseCdn)
            {
                if (null != _enterDoneDelegate)
                    _enterDoneDelegate();
                return;
            }
            
            //请求cdn的版本信息
            CDNSetting_ClientConfigInfo.instance.requestData((_clientConfigInfo) => 
            {
                //判断是否强更
                if(null == _clientConfigInfo)
                {
                    //找不到对应版本号的CDN
                    NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.init_cdn_not_found_none), 
                        TextTranslate.instance.getLanguage(TransKeyConst.confirm),() =>
                    {
                        if (_enterDoneDelegate != null)
                            _enterDoneDelegate();
                    });
                    return;
                }

                //是否展示：后台开关、是否已经同意
                bool isShow = _clientConfigInfo.isShowPolicy &&
                              !GameSetting.instance.isAgreePolicy();

                //发送埋点-判断是否需要打开隐私协议界面
                GCommon.sendStepReport(TraceConst.SHOW_POLICY_WND.setMarkParam(isShow));

                if (isShow)
                {
                    //发送埋点-判断是否需要打开隐私协议界面
                    GCommon.sendStepReport(TraceConst.SHOW_POLICY_WND_OPEN.setMarkParam(isShow));
                    
                    NPGUIAddSceneSingleWndScene oneBtnScene = new NPGUIAddSceneSingleWndScene(NPPGUIWndEuroPolicy.instance, true);
                    NPPGUIWndEuroPolicy.instance.setInfo( () =>
                    {
                        oneBtnScene.quitScene();
                        GameSetting.instance.setIsAgreePolicy(true);
                        if (null != _enterDoneDelegate)
                            _enterDoneDelegate();
                    });
                    oneBtnScene.enterScene();
                }
                else
                {
                    if (null != _enterDoneDelegate)
                        _enterDoneDelegate();
                }

            });
        }

        // /// <summary>
        // /// 判断是否有全区维护公告，有则弹出公告
        // /// </summary>
        // /// <param name="_doneDelegate"></param>
        // protected void _juegeGlobalMaintainNotice(Action _doneDelegate)
        // {
        //     //没走cdn直接处理
        //     if (!Game.instance.isUseCdn)
        //     {
        //         if (null != _doneDelegate)
        //             _doneDelegate();
        //         return;
        //     }
        //     
        //     CDNSetting_MaintainNoticeInfo.instance.requestData(
        //         (_noticeInfo) => {
        //             //判断数据是否有效
        //             if (null == _noticeInfo || _noticeInfo.Count <= 0)
        //             {
        //                 if (null != _doneDelegate)
        //                     _doneDelegate();
        //                 return;
        //             }
        //             
        //             //显示索引
        //             if(_m_iMaintainNoticeIndex < _noticeInfo.Count)
        //             {
        //                 MaintainNoticeInfo tmpInfo = _noticeInfo[_m_iMaintainNoticeIndex];
        //                 //这边只判断有效的全区维护公告，单服的维护公告后面登us的时候再判断
        //                 if(null != tmpInfo && tmpInfo.inValid() && tmpInfo.maintainNoticeType == EMaintainType.GLOBAL_MAINTAIN)
        //                 {
        //                     _m_bMaintainNoticeCanEnterGame = false;
        //
        //                     //获取语言公告
        //                     GameNoticeContent content = tmpInfo.getLanguage(GameSetting.instance.getCurrentLanguage());
        //                     //展示公告
        //                     if(null != content)
        //                     {
        //                         NPMesMgr.instance.showOneBtnMes(content.content, TextTranslate.instance.getLanguage(TransKeyConst.confirm)
        //                             , () => {
        //                                 //增加索引
        //                                 _m_iMaintainNoticeIndex++;
        //
        //                                 _juegeGlobalMaintainNotice(_doneDelegate);
        //
        //                                 //进度条展示全服维护中
        //                                 NPPGUIWndLoadingBk.instance.setRefresher(null);
        //                                 NPPGUIWndLoadingBk.instance.setProcess(1,"");
        //                                 NPPGUIWndLoadingBk.instance.setLoadingTxt(TextTranslate.instance.getLanguage(TransKeyConst.loading_allServerMaintain_none));
        //
        //                             },true, content.title);
        //                     }
        //                     else
        //                     {
        //                         //增加索引
        //                         _m_iMaintainNoticeIndex++;
        //
        //                         _juegeGlobalMaintainNotice(_doneDelegate);
        //                     }
        //                 }
        //                 else
        //                 {
        //                     //增加索引
        //                     _m_iMaintainNoticeIndex++;
        //
        //                     _juegeGlobalMaintainNotice(_doneDelegate);
        //                 }
        //             }
        //             else
        //             {
        //                 //超出上限则根据是否可以进入游戏执行结束处理
        //                 if(_m_bMaintainNoticeCanEnterGame)
        //                 {
        //                     if(null != _doneDelegate)
        //                         _doneDelegate();
        //                 }
        //             }
        //         });
        // }

        /// <summary>
        /// 显示进度条
        /// </summary>
        protected void _showLoadingBk(Action _enterDoneDelegate)
        {
            //发送埋点-显示登入进度条界面
            GCommon.sendStepReport(TraceConst.SHOW_LOADING_BK_WND.setMarkParam());
            
            //对应节点
            QueueMgr.instance.addNode_Login_MainUIMainWnd(NPPGUIWndLoadingBk.instance, UINodeTagConst.C_Login_Loading, false, false, null, _enterDoneDelegate);
        }

        /// <summary>
        /// 初始化商品档位本地化信息
        /// </summary>
        /// <param name="_enterDoneDelegate"></param>
        private void _initLocalProduct()
        {
            //发送埋点-开始初始化商品档位本地化信息
            GCommon.sendStepReport(TraceConst.START_INIT_LOCAL_PRODUCT);

            //支持重试次数初始化商品档位本地化信息
            _initLocalProductWithRetry(_m_iInitLocalProductReryCount);
        }
        private void _initLocalProductWithRetry(int _retryCount)
        {
            if (_retryCount == 0)
                return;
            
            //未使用sdk不处理
            if (!SDKMgr.instance.isUseSDK)
                return;

            SDKMgr.instance.initLocalProduct(() =>
            {
                //发送埋点-初始化商品档位本地化信息成功
                GCommon.sendStepReport(TraceConst.INIT_LOCAL_PRODUCT_SUC);

            }, (_code, _str) =>
            {
                //发送埋点-初始化商品档位本地化信息失败
                GCommon.sendStepReport(TraceConst.INIT_LOCAL_PRODUCT_FAIL.setMarkParam(_retryCount - 1));
                //失败继续重试
                _initLocalProductWithRetry(_retryCount - 1);
            });
        }

        /// <summary>
        /// 监听错误日志输出
        /// </summary>
        /// <param name="_condition"></param>
        /// <param name="_stacktrace"></param>
        /// <param name="_type"></param>
        private void _onLogCallbackHandler(string _condition, string _stacktrace, LogType _type)
        {
            if (_type == LogType.Error || _type == LogType.Exception)
            {
                string stackTrace = _stacktrace != null ? _stacktrace : "null";
                string errorStr = $"{_condition}--{stackTrace}";
                if (errorStr.Length > 500)
                {
                    string trimErrorStr = errorStr.Substring(0, 500);//最多取500个字符
                    GCommon.sendStepReport(TraceConst.LOG_ERROR.setMark(trimErrorStr));
                    SDKMgr.instance.trace_gameErr(trimErrorStr);
                }
                else
                {
                    GCommon.sendStepReport(TraceConst.LOG_ERROR.setMark(errorStr));
                    SDKMgr.instance.trace_gameErr(errorStr);
                }

                //添加firebase错误日志
                Game.instance.setCrashlyticsCustomValue();
                SDKMgr.instance.firebase_customLog(errorStr,null,null);
                SDKMgr.instance.firebase_customExc(errorStr,null,null);
            }
        }

        /// <summary>
        /// 监听异常输出
        /// </summary>
        /// <param name="_sender"></param>
        /// <param name="_e"></param>
        private void _onUncaughtExceptionHandler(object _sender, UnhandledExceptionEventArgs _e)
        {
            string errorStr = _e != null ? _e.ToString() : "";
            if (errorStr.Length > 500)
            {
                string trimErrorStr = errorStr.Substring(0, 500);//最多取500个字符
                GCommon.sendStepReport(TraceConst.LOG_ERROR.setMark(trimErrorStr));
                SDKMgr.instance.trace_gameErr(trimErrorStr);
            }
            else
            {
                GCommon.sendStepReport(TraceConst.LOG_ERROR.setMark(errorStr));
                SDKMgr.instance.trace_gameErr(errorStr);
            }

            //添加firebase错误日志
            Game.instance.setCrashlyticsCustomValue();
            SDKMgr.instance.firebase_customLog(errorStr, null, null);
            SDKMgr.instance.firebase_customExc(errorStr, null, null);
        }

        //初始化Crashlytics参数
        private void _setFirebaseCrashlyticsDefaultValue()
        {
            if (SDKMgr.instance.isUseSDK)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("{");
                builder.Append($"\"platform_id\":\"{CDNSetting_ClientConfigInfo.instance.platformId}\",");
                builder.Append($"\"area_id\":\"{CDNSetting_AreaInfo.instance.areaId}\",");
                builder.Append($"\"server_id\":\"{GameInit_SelectServer.instance.loginServerLogicId}\",");
                builder.Append($"\"os\":\"{SystemInfo.operatingSystem}\",");
                builder.Append($"\"device\":\"{SystemInfo.deviceModel}\",");
                builder.Append($"\"login_tag\":\"{Application.version}\",");
                builder.Append($"\"language\":\"{Application.systemLanguage}\",");
                builder.Append($"\"runm\":\"{SystemInfo.systemMemorySize}\",");
                builder.Append($"\"devres\":\"{Screen.currentResolution}\"");
                builder.Append("}");
                SDKMgr.instance.firebase_setCustomKey(builder.ToString(), null, null);
            }
        }
    }
}

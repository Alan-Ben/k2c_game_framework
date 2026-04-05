using ALPackage;
using System;
using System.Text;
using UnityEngine;


namespace GOE
{
    /// <summary>
    /// 游戏资源下载更新处理流程
    /// </summary>
    public class GameInit_UpdateGameRes : _AGameInitProcess, _INPPGUILoadingBkProcessRefresher
    {
        private static GameInit_UpdateGameRes _g_instance = new GameInit_UpdateGameRes();
        public static GameInit_UpdateGameRes instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GameInit_UpdateGameRes();

                return _g_instance;
            }
        }

        //是否有进行资源更新
        private bool _m_bIsUpdatedRes = false;
        /// <summary>
        /// 更新容量监控管理对象
        /// </summary>
        private ResUpdateMonitor _m_RUmonitor;

        //全体资源加载的进度记录对象
        private ALProcessSubNode _m_pAllResProcess;

        //最后一次记录的下载信息
        private string _m_sLastDownloadProcessMsg;

        private static readonly string _m_gProcessName = "res_init";
        public static string gProcessName { get { return _m_gProcessName; } }
        
        protected GameInit_UpdateGameRes()
        {
            _m_pAllResProcess = new ALProcessSubNode();
            _m_sLastDownloadProcessMsg = string.Empty;
        }

        public bool isUpdateRes { get { return _m_bIsUpdatedRes; } }
        public string lastDownloadProcessMsg { get { return _m_sLastDownloadProcessMsg; } }

        /// <summary>
        /// 获取当前操作的文本
        /// </summary>
        public string curOPTxt { get { return _m_sLastDownloadProcessMsg; } }
        /// <summary>
        /// 刷新间隔
        /// </summary>
        public float refreshDuration { get { return 0.2f; } }

        /// <summary>
        /// 返回子进度队列
        /// </summary>
        /// <returns></returns>
        protected override _IALProgressnterface[] getChildProcess()
        {
            return new _IALProgressnterface[] {
                _m_pAllResProcess };
        }

        /// <summary>
        /// 是否允许重置，默认允许。
        /// 子类可以重载
        /// 
        /// 资源更新不允许重置状态
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
            //使用底层Process进行初始化流程
            ALProcess basicProcess = ALProcess.CreateProcess(_m_gProcessName);

            //首先进行初始化步骤处理
            basicProcess
                //初始化全部resCore
                .addProcess(
                ALProcess.CreateStepProcess("res_init")//"res_core_init")
                    .addProcess(AreaResCore.instance.init)
                    .addProcess(GameResCore.instance.init)
                    .addProcess(RefdataResCore.instance.init)
                    .addProcess(SpecialResCore.instance.init))

                //设置进度条
                .addProcess(() => { _setProcess(0.1f); })
                
                //初始化区域资源,就是检测资源更新
                .addDelegateProcess(_initRemoteRes, "init_remote_res")

                //设置进度条
                .addProcess(() => { _setProcess(0.2f); })

                /** 多个资源初始化完成后的统一更新处理 */
                .addDelegateProcess(_updateAllRes, "deal_update_all_res")

                //进入正常视图
                .addProcess(_finalUpdateOp, "_finalUpdateOp")

                //进入正常视图
                .addProcess(_doneDelegate);

            //开始执行
            basicProcess.dealProcess(new GameInitMonitor(_m_gProcessName, 0));
        }

        //开始更新资源操作
        //初始化游戏区域平台资源
        protected void _initRemoteRes(Action _doneDelegate)
        {
            //发送埋点-开始确认远端资源地址
            GCommon.sendStepReport(TraceConst.START_INIT_REMOTE_RES);
            
            //增加步骤统计
            ALStepCounter resInitCounter = new ALStepCounter();
            resInitCounter.resetAll();
            resInitCounter.chgTotalStepCount(6);
            resInitCounter.regAllDoneDelegate(() =>
            {
                //发送埋点-开始确认远端资源地址完成
                GCommon.sendStepReport(TraceConst.START_INIT_REMOTE_RES_DONE);
                
                if (_doneDelegate != null) 
                    _doneDelegate();
            });
            // resInitCounter.regAllDoneDelegate(() => {ALLog.Error($"------------_initAreaPlatRes"); });

            //临时的特殊模拟cdn处理
            if (Game.instance.mainCamera.platInfo.tempCdnRootUrl.Count > 0)
            {
                CDNURLProvider_TempTest.instance.checkResUpdateCdnAsync((_url =>
                {
                    CDNSetting_TempTestVersion.instance.requestData((data) =>
                    {
                        //初始化游戏区域平台资源
                        resInitCounter.addDoneStepCount();
                        // NPAreaResCore.instance.initRemoteResLoadMgr(CDNSetting_ClientConfigInfo.instance.getAreaResUpdateURL(_url), 3, 3, true, resInitCounter.addDoneStepCount);
                        //初始化游戏资源  资源初始化完成后增加一次登陆步骤
                        GameResCore.instance.initRemoteResLoadMgr(CDNSetting_TempTestVersion.instance.getGameResUpdateUrl(_url), 3, 3, true, resInitCounter.addDoneStepCount);
                        //初始化游戏资源  资源初始化完成后增加一次登陆步骤
                        RefdataResCore.instance.initRemoteResLoadMgr(CDNSetting_TempTestVersion.instance.getRefdataResUpdateUrl(_url), 3, 3, true, resInitCounter.addDoneStepCount);
                        //初始化视频资源
                        VideoResCore.instance.init(CDNSetting_TempTestVersion.instance.getVideoResUpdateUrl(_url), resInitCounter.addDoneStepCount, resInitCounter.addDoneStepCount);

                        //初始化特殊资源  资源初始化完成后增加一次登陆步骤
                        resInitCounter.addDoneStepCount();
                        
                        // 初始化音效远端资源(这个Core的init是相当于GameResCore的initRemoteResLoadMgr即初始化远端资源版本信息) 资源初始化完成后增加一次登陆步骤
                        GAudioWebResObjCore.instance.init(CDNSetting_TempTestVersion.instance.getAudioResUpdateUrl(_url), resInitCounter.addDoneStepCount, resInitCounter.addDoneStepCount);
                    });
                }));
            }
            else
            {
                //开始检测资源URL的操作，并使用检测之后的URL进行相关处理
                CDNURLProvider_Res.instance.checkResUpdateCdnAsync((_url) => {
                    //初始化游戏区域平台资源
                    resInitCounter.addDoneStepCount();
                    // NPAreaResCore.instance.initRemoteResLoadMgr(CDNSetting_ClientConfigInfo.instance.getAreaResUpdateURL(_url), 3, 3, true, resInitCounter.addDoneStepCount);
                    //初始化游戏资源  资源初始化完成后增加一次登陆步骤
                    GameResCore.instance.initRemoteResLoadMgr(CDNSetting_ClientConfigInfo.instance.getGameResUpdateUrl(_url), 3, 3, true, resInitCounter.addDoneStepCount);
                    //初始化游戏资源  资源初始化完成后增加一次登陆步骤
                    RefdataResCore.instance.initRemoteResLoadMgr(CDNSetting_ClientConfigInfo.instance.getRefdataResUpdateUrl(_url), 3, 3, true, resInitCounter.addDoneStepCount);
                    //初始化特殊资源  资源初始化完成后增加一次登陆步骤
                    resInitCounter.addDoneStepCount();
                    // NPSpecialResCore.instance.initRemoteResLoadMgr(CDNSetting_ClientConfigInfo.instance.getSpecialResUpdateURL(_url), 3, 3, true, resInitCounter.addDoneStepCount);
                    //初始化视频资源
                    VideoResCore.instance.init(CDNSetting_ClientConfigInfo.instance.getVideoResUpdateUrl(_url), resInitCounter.addDoneStepCount, resInitCounter.addDoneStepCount);
                    
                    // 初始化音效远端资源(这个Core的init是相当于GameResCore的initRemoteResLoadMgr即初始化远端资源版本信息) 资源初始化完成后增加一次登陆步骤
                    GAudioWebResObjCore.instance.init(CDNSetting_ClientConfigInfo.instance.getAudioResUpdateUrl(_url), resInitCounter.addDoneStepCount, resInitCounter.addDoneStepCount);
                });
            }
        }

        /** 多个资源初始化完成后的统一回调 */
        protected void _updateAllRes(Action _doneDelegate)
        {
            //如果已经更新过则直接执行完成函数
            if(_m_bIsUpdatedRes)
            {
                ALLog.Error("Already Updated The Resource!");
                _doneDelegate();
                return;
            }

            //发送埋点-开始更新资源
            GCommon.sendStepReport(TraceConst.START_UPDATE_RES);

            //完成资源更新的处理回调函数
            /// <summary>
            /// 初始化三块资源时的回调管理容器对象
            /// </summary>
            ActionContainer actionContainer = new ActionContainer();
            actionContainer.action += ()=>
            {
                //发送埋点-更新资源完成
                GCommon.sendStepReport(TraceConst.UPDATE_RES_DONE);
                _doneDelegate?.Invoke();
            };

            //创建总处理对象
            _m_RUmonitor = new ResUpdateMonitor();
            //设置处理回调
            _m_RUmonitor.setProcessDelegate(_onDownloadingGameRes, actionContainer.dealAction);

            //创建更新总步骤统计对象
            ALStepCounter updateAllAskCounter = new ALStepCounter();
            updateAllAskCounter.resetAll();
            updateAllAskCounter.chgTotalStepCount(3);
            //注册回调开启加载
            updateAllAskCounter.regAllDoneDelegate(_updateAllConuterDone);
            updateAllAskCounter.regAllDoneDelegate(updateAllAskCounter.resetAll);
            // updateAllCounter.regAllDoneDelegate(() => {ALLog.Error($"------------updateAllCounter"); });
            
            //注册区域资源处理对象
            // WCGResUpdateMonitor.WCGResUpdateCoreInfo areaUpdateInfo = _m_RUmonitor.regCoreInfo();
            ResUpdateMonitor.ResUpdateCoreInfo gameUpdateInfo = _m_RUmonitor.regCoreInfo();
            ResUpdateMonitor.ResUpdateCoreInfo refdataUpdateInfo = _m_RUmonitor.regCoreInfo();
            ResUpdateMonitor.ResUpdateCoreInfo videoUpdateInfo = _m_RUmonitor.regCoreInfo();

            // WCGResUpdateMonitor.WCGResUpdateCoreInfo spUpdateInfo = _m_RUmonitor.regCoreInfo();

//             //完成资源更新的处理回调函数
//             NPActionContainer areaActionContainer = new NPActionContainer();
//             areaActionContainer.action += areaUpdateInfo.onDownloadDone;
//             areaActionContainer.action += updateAllAskCounter.addDoneStepCount;
//             //获取当前的进度相关数据，带入3个参数分别是，已下载字节数，已解压字节数，总字节数，当前是否在下载中
//             NPAreaResCore.instance.updateAllRes(areaUpdateInfo.onDownloadingProcess
//                 , (int _failCount) =>
//                 {
// #if UNITY_EDITOR
//                     if (_failCount > 0)
//                         NPMesMgr.instance.showOneBtnMes("ResDownloadFail: " + _failCount, "Ok", null);
// #endif
//                     areaActionContainer.action();
//                 }
//                 , (long _totalSize, Action _dealFunc) =>
//                 {
//                     //设置尺寸
//                     areaUpdateInfo.setTotalSize(_totalSize);
//                     //设置处理函数
//                     areaUpdateInfo.setDealFunc(_dealFunc);
//
//                     //累加步骤对象
//                     updateAllAskCounter.addDoneStepCount();
//                 });

            ActionContainer gameActionContainer = new ActionContainer();
            gameActionContainer.action += gameUpdateInfo.onDownloadDone;
            //由于ask和done回调在逻辑上只会调用一个，因此在done回调增加了一个step调用避免流程卡住
            gameActionContainer.action += updateAllAskCounter.addDoneStepCount;
            GameResCore.instance.updateAllRes(gameUpdateInfo.onDownloadingProcess,
                _judgeGameResNeedUpdate
                , (int _failCount) =>
                {
#if UNITY_EDITOR
                    if (_failCount > 0)
                        NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.init_res_download_fail_num, _failCount), 
                            TextTranslate.instance.getLanguage(TransKeyConst.ok), null);
#endif
                    gameActionContainer.action();
                }
                , (long _totalSize, Action _dealFunc) =>
                {
                    //设置尺寸
                    gameUpdateInfo.setTotalSize(_totalSize);
                    //设置处理函数
                    gameUpdateInfo.setDealFunc(_dealFunc);

                    //累加步骤对象
                    updateAllAskCounter.addDoneStepCount();
                });

            ActionContainer refdataActionContainer = new ActionContainer();
            refdataActionContainer.action += refdataUpdateInfo.onDownloadDone;
            refdataActionContainer.action += updateAllAskCounter.addDoneStepCount;

            RefdataResCore.instance.updateAllRes(refdataUpdateInfo.onDownloadingProcess,
                _judgeRefdataResNeedUpdate
                , (int _failCount) =>
                {
#if UNITY_EDITOR
                    if (_failCount > 0)
                        NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.init_res_download_fail_num, _failCount), 
                            TextTranslate.instance.getLanguage(TransKeyConst.ok), null);
#endif
                    refdataActionContainer.action();
                }
                , (long _totalSize, Action _dealFunc) =>
                {
                    //设置尺寸
                    refdataUpdateInfo.setTotalSize(_totalSize);
                    //设置处理函数
                    refdataUpdateInfo.setDealFunc(_dealFunc);

                    //累加步骤对象
                    updateAllAskCounter.addDoneStepCount();
                });
            
            
            ActionContainer videoActionContainer = new ActionContainer();
            videoActionContainer.action += videoUpdateInfo.onDownloadDone;
            videoActionContainer.action += updateAllAskCounter.addDoneStepCount;
            VideoResCore.instance.updateAllRes(videoUpdateInfo.onDownloadingProcess
                , (int _failCount) =>
                {
#if UNITY_EDITOR
                    if (_failCount > 0)
                        NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.init_video_download_fail_num, _failCount), 
                            TextTranslate.instance.getLanguage(TransKeyConst.ok), null);
#endif
                    videoActionContainer.action();
                }
                , (long _totalSize, Action _dealFunc) =>
                {
                    //设置尺寸
                    videoUpdateInfo.setTotalSize(_totalSize);
                    //设置处理函数
                    videoUpdateInfo.setDealFunc(_dealFunc);

                    //累加步骤对象
                    updateAllAskCounter.addDoneStepCount();
                });
            
//             NPActionContainer spActionContainer = new NPActionContainer();
//             spActionContainer.action += spUpdateInfo.onDownloadDone;
//             spActionContainer.action += updateAllAskCounter.addDoneStepCount;
//             NPSpecialResCore.instance.updateAllRes(spUpdateInfo.onDownloadingProcess
//                 , (int _failCount) =>
//                 {
// #if UNITY_EDITOR
//                     if (_failCount > 0)
//                         NPMesMgr.instance.showOneBtnMes("ResDownloadFail: " + _failCount, "Ok", null);
// #endif
//                     spActionContainer.action();
//                 }
//                 , (long _totalSize, Action _dealFunc) =>
//                 {
//                     //设置尺寸
//                     spUpdateInfo.setTotalSize(_totalSize);
//                     //设置处理函数
//                     spUpdateInfo.setDealFunc(_dealFunc);
//
//                     //累加步骤对象
//                     updateAllAskCounter.addDoneStepCount();
//                 });
        }

        private bool _m_bIsDownload25 = false;
        private bool _m_bIsDownload50 = false;
        private bool _m_bIsDownload75 = false;
        //_m_dmDownloadMgr.loadedSize, _m_dmDownloadMgr.unzipSize, _m_dmDownloadMgr.totalSize, (_m_dmDownloadMgr.loadingSize != _m_dmDownloadMgr.loadingLoadedSize)
        /// <summary>
        /// 下载游戏资源时调用的事件函数
        /// </summary>
        /// <param name="_loadedSize"></param>
        /// <param name="_unzipSize"></param>
        /// <param name="_totalSize"></param>
        /// <param name="_isDownloading"></param>
        protected void _onDownloadingGameRes(long _loadedSize, long _unzipSize, long _totalSize, bool _isDownloading)
        {
            //计算进度
            double process = (double)_loadedSize / _totalSize;

            //设置进度
            _setProcess(0.2f + (float)(process * 0.8f));

            //拼凑显示文字
            StringBuilder txt = new StringBuilder();
            txt.Append(TextTranslate.instance.getLanguage(TransKeyConst.login_res_downloading));
            GCommon.appendFileSize(_loadedSize, txt);
            txt.Append("/");
            GCommon.appendFileSize(_totalSize, txt);

            //设置最后的下载进度信息
            _m_sLastDownloadProcessMsg = txt.ToString();

            //发送埋点--资源更新
            if ((int)(process * 100) >= 25 && !_m_bIsDownload25)
            {
                _m_bIsDownload25 = true;
                GCommon.sendStepReport(TraceConst.UPDATE_RES_25);
            }
            if ((int)(process * 100) >= 50 && !_m_bIsDownload50)
            {
                _m_bIsDownload50 = true;
                GCommon.sendStepReport(TraceConst.UPDATE_RES_50);
            }
            if ((int)(process * 100) >= 75 && !_m_bIsDownload75)
            {
                _m_bIsDownload75 = true;
                GCommon.sendStepReport(TraceConst.UPDATE_RES_75);
            }
        }

        /// <summary>
        /// 在计算完所有下载大小后的处理
        /// </summary>
        private void _updateAllConuterDone()
        {
            // ALLog.Error($"------------_updateAllConuterDone");
            //先带入假的容量数据
            _retStorageSize(false, 0);
        }

        /// <summary>
        /// 获取当前存储空间大小 回调
        /// </summary>
        /// <param name="_isGetStoreSizeSuc"></param>
        /// <param name="_size"></param>
        private void _retStorageSize(bool _isGetStoreSizeSuc, long _size)
        {
            if(_m_RUmonitor == null)
                return;

            //计算总数量
            long totalSize = _m_RUmonitor.calTotalSize();

            if(totalSize <= 0)
            {
                _m_RUmonitor.startDownload();
                return;
            }

            //在这里的时候判断本机空间是否小于totalSize的五倍，小于直接弹窗口提示玩家存储空间不足，然后点确认退出游戏
            //查询失败的时候不做提示处理，继续进行
            if(_isGetStoreSizeSuc && totalSize * 5 > _size * 1024)
            {
                //存储空间不足的处理 
                _showMemoryNotEnough();
                return;
            }
            
            //小于20m直接更新
            if (totalSize <= 1024 * 1024 * 20)
            {
                _m_RUmonitor.startDownload();
                return;
            }

            string tip = String.Empty;
            StringBuilder builder = new StringBuilder();
            GCommon.appendFileSize(totalSize, builder);

            //如果事wifi环境且大于50m则提示玩家是否更新资源
            if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork && totalSize >= 1024 * 1024 * 50)
            {
                tip = string.Format(TextTranslate.instance.getLanguage(TransKeyConst.update_resource), builder.ToString());
            }
            //非WIFI环境：大于20M弹出热更大小提示
            else if(Application.internetReachability != NetworkReachability.ReachableViaLocalAreaNetwork && totalSize >= 1024 * 1024 * 20)
            {
                tip = string.Format(TextTranslate.instance.getLanguage(TransKeyConst.update_resource_nonwifi), builder.ToString());
            }
            //其他情况无需要提示直接下载
            else
            {
                _m_RUmonitor.startDownload();
                return;
            }
            
            //询问是否更新资源
            NPMesMgr.instance.showOneBtnMes(
                tip
                , TextTranslate.instance.getLanguage(TransKeyConst.update_confirm), () =>
                {
                    //设置本次更新了资源
                    _m_bIsUpdatedRes = true;
                    
                    //开始下载
                    _m_RUmonitor.startDownload();
                });
        }

        /// <summary>
        /// 存储空间不足的处理，弹出提示框提示玩家控件不足请清理，在玩家点击确定后退出游戏
        /// </summary>
        private void _showMemoryNotEnough()
        {
            //告知玩家存储空间不足，请清理空间
            NPMesMgr.instance.showOneBtnMes(
                TextTranslate.instance.getLanguage(TransKeyConst.memory_not_enough_clear)
                , TextTranslate.instance.getLanguage(TransKeyConst.memory_confirm), Application.Quit);
        }
        
        /// <summary>
        /// 根据当前用户情况展示登录界面
        /// 最后登录操作的地方
        /// </summary>
        protected void _finalUpdateOp()
        {
            //设置进度
            _m_pAllResProcess.setProcess(1f);
        }

        /// <summary>
        /// 判断配表资源是否需要更新
        /// </summary>
        /// <param name="_assetPath"></param>
        /// <returns></returns>
        protected bool _judgeRefdataResNeedUpdate(string _assetPath)
        {
            if (string.IsNullOrEmpty(_assetPath))
                return false;
            
            //路径以language/开头的只加载当前语言的
            if(_assetPath.StartsWith("language/"))
            {
                // ENPLanguage curLanguage = NPGameSetting.instance.getCurrentLanguage();
                // string languageAssetPath = LanuageAsset.getLanguageAssetPath(curLanguage);
                // string playerNameAssetPath = MultiLanguageAsset.getLanguageAssetPath(curLanguage, NPGSOPlayerNameRefSet.objName);
                //
                // if (_assetPath == languageAssetPath || _assetPath == playerNameAssetPath)
                // {
                //     return true;
                // }
                return false;
            }

            return true;
        }
        /// <summary>
        /// 决定哪些资源要在游戏启动时下载
        /// </summary>
        /// <return> true 是要更新， false 是不要更新</return>
        protected bool _judgeGameResNeedUpdate(string _assetPath)
        {
            if (string.IsNullOrEmpty(_assetPath))
                return false;

            // 增量包的部分由别的模块负责另外下载
            if (_assetPath.StartsWith(ResIndexConst.addPackPathRoot))
                return false;

            return true;
        }
        
        /// <summary>
        /// 设置当前进度
        /// </summary>
        /// <param name="_process"></param>
        protected void _setProcess(float _process)
        {
            _m_pAllResProcess.setProcess(_process);
        }
    }
}

using ALPackage;
using System;
using System.Text;
using ILRuntime.Runtime.Generated;
using UnityEngine;


namespace GOE
{
    /// <summary>
    /// 游戏热更更新加载处理流程
    /// 这边只处理InjectFix,Ilruntime的加载过程，不处理具体业务逻辑
    /// </summary>
    public class GameInit_HotfixLoad : _AGameInitProcess, _INPPGUILoadingBkProcessRefresher
    {
        private static GameInit_HotfixLoad _g_instance = new GameInit_HotfixLoad();
        public static GameInit_HotfixLoad instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GameInit_HotfixLoad();

                return _g_instance;
            }
        }
        
        //全体资源加载的进度记录对象
        private ALProcessSubNode _m_pAllResProcess;

        //injectFix更新地址
        private string _m_injectFixRemoteUrl;
        //ilruntime更新地址
        private string _m_ilRuntimeRemoteUrl;

        private static readonly string _m_gProcessName = "hotfix_load";
        public static string gProcessName { get { return _m_gProcessName; } }
        
        protected GameInit_HotfixLoad()
        {
            _m_pAllResProcess = new ALProcessSubNode();
        }

        //当前操作文字
        public string curOPTxt { get {return String.Empty;} }

        //刷新间隔
        public float refreshDuration { get { return 0.2f; } }

        //返回子进度队列
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
            //发送埋点-开始热更补丁加载
            GCommon.sendStepReport(TraceConst.START_HOTFIX_LOAD);

            //使用底层Process进行初始化流程
            ALProcess basicProcess = ALProcess.CreateProcess(_m_gProcessName);

            //首先进行初始化步骤处理
            basicProcess
                //设置进度条
                .addProcess(() => { _setProcess(0.1f); })
                
                //初始化补丁更新地址
                .addDelegateProcess(_initRemoteRes, "init_remote_hotfix_url")

                //设置进度条
                .addProcess(() => { _setProcess(0.2f); })

                //初始化加载injectFix
                .addDelegateProcess(_initLoadInjectFix, "deal_load_injectFix")

                //设置进度条
                .addProcess(() => { _setProcess(0.6f); })
                
                //初始化加载hotfix
                .addDelegateProcess(_initLoadHotfixFix, "deal_load_ilRuntime")
                
                //初始化加载hotfix的绑定
                .addDelegateProcess(_initLoadHotfixBinding, "deal_ilRuntime_binding")
                
                //初始化加载hotfix的初始化
                .addDelegateProcess(_initILRuntime, "deal_ilRuntime_init")

                //ILruntime的二次版本号校验
                .addProcess(_checkILRuntimeVersion, "_checkILRuntimeVersion")
                
                //进入正常视图
                .addProcess(() => {_setProcess(1f);})
                //热更补丁加载完成
                .addProcess(() => {_fixLoadDone();}, "_fixLoadDone")
                //进入正常视图
                .addProcess(_doneDelegate);

            //开始执行
            basicProcess.dealProcess(new GameInitMonitor(_m_gProcessName, 0));
        }

        //初始化补丁更新地址
        private void _initRemoteRes(Action _doneDelegate)
        {
            //发送埋点-初始化补丁更新地址
            GCommon.sendStepReport(TraceConst.INIT_HOTFIX_URL);

            //临时的特殊模拟cdn处理
            if (Game.instance.mainCamera.platInfo.tempCdnRootUrl.Count > 0)
            {
                CDNURLProvider_TempTest.instance.checkResUpdateCdnAsync((_url =>
                {
                    CDNSetting_TempTestVersion.instance.requestData((data) =>
                    {
                        _m_injectFixRemoteUrl = CDNSetting_TempTestVersion.instance.getInjectFixUpdateUrl(_url);
                        _m_ilRuntimeRemoteUrl = CDNSetting_TempTestVersion.instance.getHotfixUpdateUrl(_url);

                        //发送埋点-初始化补丁更新地址成功
                        GCommon.sendStepReport(TraceConst.INIT_HOTFIX_URL_SUC.setMarkParam(_m_injectFixRemoteUrl, _m_ilRuntimeRemoteUrl));

                        if (_doneDelegate != null) 
                            _doneDelegate();
                    });
                }));
            }
            else
            {
                //开始检测资源URL的操作，并使用检测之后的URL进行相关处理
                CDNURLProvider_Res.instance.checkResUpdateCdnAsync((_url) =>
                {
                    _m_injectFixRemoteUrl = CDNSetting_ClientConfigInfo.instance.getInjectFixUpdateUrl(_url);
                    _m_ilRuntimeRemoteUrl = CDNSetting_ClientConfigInfo.instance.getHotfixUpdateUrl(_url);

                    //发送埋点-初始化补丁更新地址成功
                    GCommon.sendStepReport(TraceConst.INIT_HOTFIX_URL_SUC.setMarkParam(_m_injectFixRemoteUrl, _m_ilRuntimeRemoteUrl));

                    if (_doneDelegate != null) 
                        _doneDelegate();
                });
            }
        }

        #region injectFix 加载

        //初始化加载injectFix
        private void _initLoadInjectFix(Action _doneDelegate)
        {
            //发送埋点-初始化加载injectFix
            GCommon.sendStepReport(TraceConst.INIT_LOAD_INJECT_FIX);

            //无效cdn数据说明不走cdn，则每次都直接下载最新的，因为存在直接写死路径的情况
            if (!CDNSetting_ClientConfigInfo.instance.isValid)
            {
                _downloadInjectFix(_doneDelegate);
            }
            else
            {
                // 远程的版本信息标记
                string remoteTag = CDNSetting_ClientConfigInfo.instance.getInjectFixPathTag();

                //本地版本跟远程一致
                if (GameSetting.instance.getLastInjectFixVersion() == remoteTag)
                {
                    //如果和本地的相等，尝试直接使用本地
                    if (!InjectFixMgr.instance.LoadLocalSavedPatch())
                    {
                        //如果本地加载失败，还是要走下载
                        _downloadInjectFix(_doneDelegate);
                    }
                    else
                    {
                        //如果成功直接结束流程
                        if (null != _doneDelegate)
                            _doneDelegate();
                    }
                }
                else
                {
                    // 如果和本地不相等，就尝试下载远程资源来加载
                    _downloadInjectFix(_doneDelegate);
                }
            }
        }

        //走下载方式加载injectFix
        private void _downloadInjectFix(Action _doneDelegate)
        {
            //果地址为空则不需要加载，直接返回
            if (string.IsNullOrEmpty(_m_injectFixRemoteUrl))
            {
                //如果地址为空则执行一次清除操作，因为可能存在不配的情况是为了不加载补丁
                InjectFixMgr.instance.clearCurPatch();

                //发送埋点-不需要加载InjectFix
                GCommon.sendStepReport(TraceConst.SKIP_LOAD_INJECT_FIX);

                if (null != _doneDelegate)
                    _doneDelegate();
            }
            else
            {
                InjectFixMgr.instance.startDownloadAndLoad(_m_injectFixRemoteUrl + "/Assembly-CSharp.patch.bytes", null, () =>
                {
                    ALLog.Sys("injectFix加载成功");

                    //发送埋点-加载injectFix成功
                    GCommon.sendStepReport(TraceConst.LOAD_INJECT_FIX_SUC);

                    //设置当前最新标记
                    GameSetting.instance.setLastInjectFixVersion(CDNSetting_ClientConfigInfo.instance.getInjectFixPathTag());
                    
                    if (null != _doneDelegate)
                        _doneDelegate();
                }, () =>
                {
                    ALLog.Sys("injectFix加载失败");

                    //发送埋点-加载injectFix失败
                    GCommon.sendStepReport(TraceConst.LOAD_INJECT_FIX_FAIL);

                    if (null != _doneDelegate)
                        _doneDelegate();
                });

            }
        }
        
        #endregion

        #region Ilruntime 加载

        //初始化加载injectFix
        private void _initLoadHotfixFix(Action _doneDelegate)
        {
            //发送埋点-初始化加载HotFix
            GCommon.sendStepReport(TraceConst.INIT_LOAD_HOT_FIX);

            //初始化ILRuntime管理对象
            Game.instance.initILRuntimeMgr("Hotfix.dll", "Hotfix.pdb");

#if UNITY_EDITOR
            //仅Editor下判断，直接使用工程目录下的dll当作hotfix加载
            if (ALLocalResLoaderMgr.instance.useLocalHotfix)
            {
                string dllPath = Application.dataPath + "/Hotfix~/Hotfix/bin/Editor/Hotfix.dll";
                string pdbPath = Application.dataPath + "/Hotfix~/Hotfix/bin/Editor/Hotfix.pdb";
                
                if (Game.instance.ILRuntimeMgr.tryLoadDllByPath(dllPath, pdbPath, null, null))
                {
                    //发送埋点-本地加载HotFix成功
                    GCommon.sendStepReport(TraceConst.LOAD_HOT_FIX_LOCAL_SUC);
                    //如果成功直接结束流程
                    if (null != _doneDelegate)
                        _doneDelegate(); 
                    return;
                }
            }
#endif
            
            //无效cdn数据说明不走cdn，则每次都直接下载最新的，因为存在直接写死路径的情况
            if (!CDNSetting_ClientConfigInfo.instance.isValid)
            {
                _downloadIlRuntime(_doneDelegate);
            }
            else
            {
                // 远程的版本信息标记
                string remoteTag = CDNSetting_ClientConfigInfo.instance.getHotfixPathTag();

                //本地版本跟远程一致
                if (GameSetting.instance.getLastIlRuntimeVersion() == remoteTag)
                {
                    //如果和本地的相等，尝试直接使用本地，这边不加载随包的
                    if (!Game.instance.ILRuntimeMgr.tryLoadLocalSavedDll(null, null))
                    {
                        // 失败一次就清空存储
                        GameSetting.instance.setLastIlRuntimeVersion(String.Empty);
                        
                        //如果本地加载失败，还是要走下载
                        _downloadIlRuntime(_doneDelegate);
                    }
                    else
                    {
                        //发送埋点-本地加载HotFix成功
                        GCommon.sendStepReport(TraceConst.LOAD_HOT_FIX_LOCAL_SUC);
                        //如果成功直接结束流程
                        if (null != _doneDelegate)
                            _doneDelegate();
                    }
                }
                else
                {
                    // 如果和本地不相等，就尝试下载远程资源来加载
                    _downloadIlRuntime(_doneDelegate);
                }
            }
        }
        
        //走下载方式加载Ilruntime
        private void _downloadIlRuntime(Action _doneDelegate)
        {
            //果地址为空则加载本地的
            if (string.IsNullOrEmpty(_m_ilRuntimeRemoteUrl))
            {
                _loadLocalIlRuntimePatch(_doneDelegate);
            }
            else
            {
                //有地址尝试下载
                Game.instance.ILRuntimeMgr.startDownload(
                    _m_ilRuntimeRemoteUrl + "/Hotfix.dll",
                    _m_ilRuntimeRemoteUrl + "/Hotfix.pdb", 
                    null,
                    () =>
                {
                    ALLog.Sys("IlRuntime下载加载成功");

                    //发送埋点-下载加载HotFix成功
                    GCommon.sendStepReport(TraceConst.LOAD_HOT_FIX_SUC);

                    //设置当前最新标记
                    GameSetting.instance.setLastIlRuntimeVersion(CDNSetting_ClientConfigInfo.instance.getHotfixPathTag());
                    
                    if (null != _doneDelegate)
                        _doneDelegate();
                }, () =>
                {
                    ALLog.Sys("IlRuntime下载加载失败");

                    //发送埋点-下载加载HotFix失败
                    GCommon.sendStepReport(TraceConst.LOAD_HOT_FIX_FAIL);

                    //下载失败再尝试一次本地的加载
                    _loadLocalIlRuntimePatch(_doneDelegate);
                });

            }
        }

        /// <summary>
        /// 加载本地的ilruntime，如果本地失败用streamAsset里面的，理论上ilruntime要保证有
        /// </summary>
        /// <param name="_doneDelegate"></param>
        private void _loadLocalIlRuntimePatch(Action _doneDelegate)
        {
            if (Game.instance.ILRuntimeMgr.tryLoadLocalSavedDll(null, null))
            {
                //发送埋点-本地加载HotFix成功
                GCommon.sendStepReport(TraceConst.LOAD_HOT_FIX_LOCAL_SUC);

                //本地的补丁加载成功
                if (null != _doneDelegate)
                    _doneDelegate();
            }
            else
            {
                ALLog.Sys("IlRuntime  _loadLocalIlRuntimePatch  本地补丁加载失败");

                //本地补丁加载失败，尝试加载随包的
                if (Game.instance.ILRuntimeMgr.tryLoadLocalStreamingDll(null, null))
                {
                    //发送埋点-本地加载HotFix成功
                    GCommon.sendStepReport(TraceConst.LOAD_HOT_FIX_LOCAL_SUC);

                    //随包的补丁加载成功
                    if (null != _doneDelegate)
                        _doneDelegate();
                }
                else
                {
                    ALLog.Sys("IlRuntime  _loadLocalIlRuntimePatch  随包补丁加载失败");

                    //发送埋点-本地加载HotFix失败
                    GCommon.sendStepReport(TraceConst.LOAD_HOT_FIX_LOCAL_FAIL);

                    // 失败一次就清空存储
                    GameSetting.instance.setLastIlRuntimeVersion(String.Empty);

                    if (null != _doneDelegate)
                        _doneDelegate();
                }
            }
        }

        #endregion
        
        
        //hotfix的绑定注册等初始化相关
        private void _initLoadHotfixBinding(Action _doneDelegate)
        {
            try
            {
                //注册一些提供给dll继承的adapter注册处理
                if (ALHotfixMgr_ILRuntime_Global.g_AppDomain != null &&
                    ALHotfixMgr_ILRuntime_Global.g_AppDomain.DelegateManager != null)
                {
                    ILRuntimeBind.bindILRuntimeAdapter(ALHotfixMgr_ILRuntime_Global.g_AppDomain);
                }

                //注册一些热更工程调用主工程的函数绑定
                CLRBindings.Initialize(ALHotfixMgr_ILRuntime_Global.g_AppDomain);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                
                // 失败一次就清空存储
                GameSetting.instance.setLastIlRuntimeVersion(String.Empty);
            }
            finally
            {
                if (null != _doneDelegate)
                    _doneDelegate();
            }
        }
        
        //hotfix的初始化相关
        private void _initILRuntime(Action _doneDelegate)
        {
            try
            {
                HotfixStaticFunc.init();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                
                // 失败一次就清空存储
                GameSetting.instance.setLastIlRuntimeVersion(String.Empty);
            }
            finally
            {
                if (null != _doneDelegate)
                    _doneDelegate();
            }
        }
        
        //增加纠错机制，将已经下载的存储标记与加载成功的版本进行比对，如果两者不匹配，则重置下载标记，下回重新下载处理
        //这里会有一个潜规则，就是Hotfix一定要包含版本号才行，如果不包含，本地Hotfix缓存就会无效，每次都会去下载新的dll
        //这一步依赖Hotfix的初始化完成才能拿到hotfix的版本号
        private void _checkILRuntimeVersion()
        {
            string ILRuntimeTag = GameSetting.instance.getLastIlRuntimeVersion();
            
            if(!string.IsNullOrEmpty(ILRuntimeTag))
            {
                ALHotfixMgr_ILRuntime_Global.HotfixSCVersion dllVersion = ALHotfixMgr_ILRuntime_Global.instance.version;
                string dllVersionStr = $"{dllVersion.main}.{dllVersion.sub}.{dllVersion.patch}.{dllVersion.build}";
                if(!ILRuntimeTag.Contains(dllVersionStr))
                {
                    Debug.LogError($"客户端Setting里存的HotfixTag:{ILRuntimeTag}，不包含{dllVersionStr}，清除Setting");
                    GameSetting.instance.setLastIlRuntimeVersion(String.Empty);
                }
            }
        }

        /// <summary>
        /// 初始化热更资源
        /// </summary>
        /// <param name="_doneDelegate"></param>
        public void initHotfixRes(Action _action)
        {
            //发送埋点-初始化HotFix热更配表资源
            GCommon.sendStepReport(TraceConst.INIT_HOTFIX_RES);
            //初始化热更配表
            HotfixStaticFunc.initHotfixRefdata(_action);
        }

        /// <summary>
        /// 热更补丁加载完成
        /// </summary>
        private void _fixLoadDone()
        {
            
        }

        /// <summary>
        /// 设置当前进度
        /// </summary>
        /// <param name="_process"></param>
        private void _setProcess(float _process)
        {
            if (_m_pAllResProcess != null) 
                _m_pAllResProcess.setProcess(_process);
        }
    }
}

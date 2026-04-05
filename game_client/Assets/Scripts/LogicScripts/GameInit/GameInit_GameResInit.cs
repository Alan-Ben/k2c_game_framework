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
    /// 游戏初始化过程处理函数
    /// </summary>
    public class GameInit_GameResInit : _AGameInitProcess, _INPPGUILoadingBkProcessRefresher
    {
        private static GameInit_GameResInit _g_instance = new GameInit_GameResInit();
        public static GameInit_GameResInit instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GameInit_GameResInit();

                return _g_instance;
            }
        }

        //全体资源加载的进度记录对象
        private ALProcessSubNode _m_pAllResProcess;

        //最后一次记录的下载信息
        private string _m_sLastDownloadProcessMsg;

        private static readonly string _m_gProcessName = "game_res_init";
        public static string gProcessName { get { return _m_gProcessName; } }
        
        protected GameInit_GameResInit()
        {
            _m_pAllResProcess = new ALProcessSubNode();
            _m_sLastDownloadProcessMsg = string.Empty;
        }

        public string lastDownloadProcessMsg { get { return _m_sLastDownloadProcessMsg; } }

        /// <summary>
        /// 获取当前操作的文本
        /// </summary>
        public string curOPTxt { get { return _m_sLastDownloadProcessMsg; } }
        /// <summary>
        /// 刷新间隔
        /// </summary>
        public float refreshDuration { get { return 1f; } }

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
        /// 游戏内资源加载不允许重置状态
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
                //确保资源更新完毕
                .addDelegateProcess(GameInit_UpdateGameRes.instance.init, "update_all_res")
                //语言资源更新完毕
                .addDelegateProcess(GameInit_LanguageResLoad.instance.init, "update_language_res")

            #region 载入资源
                //初始化游戏相关资源导出
                .addDelegateProcess(_onGameResInited, "game_res_init")
                //初始化通用资源
                .addDelegateProcess(_loadCommonAB, "load_common_AB")
                //初始化其他通用资源
                .addDelegateProcess(_initCommonInfo, "init_common_info")
                //初始化视频音量配置信息
                .addDelegateProcess(_loadVideoVolume, "load_video_volume")
                //初始化增量包管理器
                .addDelegateProcess(_initAddPackMgr, "init_add_pack_mgr")
                //初始化基础ui，资源依赖配表
                .addDelegateProcess(_initBasicUI, "init_basic_ui")
                //初始化混音器
                .addProcess(_initAudioMixer)
                // //初始化puerts脚本
                // .addDelegateProcess(NPPuertsMgr.init, "init_puerts")
                // .addProcess(_initPuertsScripts)
                .addProcess(_onLocalResLoadOver)

            #endregion

                //初始化showcase
                .addDelegateProcess(_initShowCaseMgr)

                //进入正常视图
                .addProcess(_finalInitOp)
                .addProcess(_doneDelegate);

            //开始执行
            basicProcess.dealProcess(new GameInitMonitor(_m_gProcessName, 0));
        }
        
        /// <summary>
        /// 游戏资源初始化成功的处理
        /// </summary>
        protected void _onGameResInited(Action _doneDelegate)
        {
            //发送埋点-游戏资源初始化成功的处理
            GCommon.sendStepReport(TraceConst.GAME_RES_INITED);

            if (_doneDelegate != null) 
                _doneDelegate();
        }

        protected void _loadVideoVolume(Action _doneDelegate)
        {
            VideoVolumeMgr.instance.init(_doneDelegate);
        }

        /// <summary>
        /// 资源解压处理成功后的处理
        /// </summary>
        protected void _loadCommonAB(Action _doneDelegate)
        {
            //发送埋点-开始加载通用资源对象(common/shader.unity3d)
            GCommon.sendStepReport(TraceConst.START_LOAD_COMMON_AB);

            //加载通用资源对象
            GameResCore.instance.loadAsset("common/shader.unity3d"
                    , (bool _isSuc, ALAssetBundleObj _assetObj) =>
                    {
                        //发送埋点-加载通用资源对象完成
                        GCommon.sendStepReport(TraceConst.LOAD_COMMON_AB_DONE);

                        //判断是否加载成功
                        if (!_isSuc || null == _assetObj)
                        {
                            //初始化通用资源
                            GGameCommonInfo.instance.init(_doneDelegate);
                            return;
                        }

                        //固态化asset bundle
                        _assetObj.staticAssetBundle();
                        
                        // 预热shader
                        ShaderVariantCollection allShaderVariants = _assetObj.load<ShaderVariantCollection>("all_shader_variants");
                        if (allShaderVariants != null) allShaderVariants.WarmUp();
                        //初始化通用资源
                        GGameCommonInfo.instance.init(_doneDelegate);
                    }
                    , null);
        }

        /// <summary>
        /// 通用资源加载完毕后加载通用assetbundle
        /// </summary>
        protected void _initCommonInfo(Action _doneDelegate)
        {
            //发送埋点-开始加载通用assetbundle(配表、数据库等)
            GCommon.sendStepReport(TraceConst.START_INIT_COMMON_INFO);
#if UNITY_EDITOR
            Debug.Log("通用资源加载完毕后加载通用assetbundle");
#endif
            
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(GGameCommonInfo.instance.obj.commonAssetBundlePathList.Count + 4);
            stepCounter.regAllDoneDelegate(()=>
            {
                //发送埋点-加载通用assetbundle完成
                GCommon.sendStepReport(TraceConst.INIT_COMMON_INFO_DONE);
                _doneDelegate?.Invoke();
            });
            // stepCounter.regAllDoneDelegate(() => {ALLog.Error($"------------loadcommonab"); });

            //逐个加载通用Assetbundle
            for(int i = 0; i < GGameCommonInfo.instance.obj.commonAssetBundlePathList.Count; i++)
            {
                GameResCore.instance.loadAsset(GGameCommonInfo.instance.obj.commonAssetBundlePathList[i]
                    , (bool _isSuc, ALAssetBundleObj _assetObj) =>
                    {
                    //判断是否加载成功
                    if(!_isSuc || null == _assetObj)
                        {
                            UnityEngine.Debug.LogError("game_common_info 里的Common Asset: 第" + i + " 个配置init fail!!!需要检查资源");
                            stepCounter.addDoneStepCount();
                            return;
                        }

                    //固态化asset bundle
                    _assetObj.staticAssetBundle();

                    //判断是否字体文件
                    if(_assetObj.idx.Equals("common/font.unity3d", StringComparison.OrdinalIgnoreCase))
                        {
                        //加载字体
                        Game.instance.initCommonFont(_assetObj.load<Font>("Font_blod"));
                        }
                        else if(_assetObj.idx.Equals("common/sprite_atlas.unity3d", StringComparison.OrdinalIgnoreCase))
                        {
                        //如是图集则直接存储
                        Game.instance.initAtlasAB(_assetObj);
                        }

                    //增加完成步骤
                    stepCounter.addDoneStepCount();
                    }
                    , null);
            }

            //加载数据结构
            GRefdataCoreMgr.instance.InitAllRefCore(stepCounter.addDoneStepCount);
            GRefdataCoreMgr.instance.InitRefLanguage(GameSetting.instance.getCurrentLanguage(), stepCounter.addDoneStepCount);
            NPGGUILanguageSqlite.instance.initDB(stepCounter.addDoneStepCount, GameSetting.instance.getCurrentLanguage());
            // 更新文字资源
            _initFont(stepCounter.addDoneStepCount);

            //初始化置灰shader参数
            _initGrayGlobalShaderParam();

            //初始化URP设置
            _initURPSetting();
        }

        // 初始化增量包管理器
        private void _initAddPackMgr(Action _doneDelegate)
        {
            AddPackMgr.instance.init(_doneDelegate);
        }

        // 初始化gloabal shader置灰参数
        private void _initGrayGlobalShaderParam()
        {
            if(GGameCommonInfo.instance.obj == null)
            {
                Debug.LogError("_initGrayShaderParam GGameCommonInfo没有初始化成功");
                return;
            }
            Shader.SetGlobalFloat(ShaderPropertyMgr.g_GrayScale, GGameCommonInfo.instance.obj.grayGlobalScale);
            Shader.SetGlobalFloat(ShaderPropertyMgr.g_GrayColorScale, GGameCommonInfo.instance.obj.grayGlobalColorScale);
            Shader.SetGlobalFloat(ShaderPropertyMgr.g_GrayAlphaScale, GGameCommonInfo.instance.obj.grayGlobalAlphaScale);
            //用于对话变暗的参数
            // Shader.SetGlobalFloat(ShaderPropertyMgr.g_DarkScale, GGameCommonInfo.instance.obj.darkGlobalScale);

        }

        private void _initURPSetting()
        {
            // //如果CommonInfo里配置了urp替换配置，则替换掉
            // if (NPGGameCommonInfo.instance.obj.openReplaceUrpSetting)
            // {
            //     if(NPGGameCommonInfo.instance.obj.urpSetting.uiRenderData != null
            //     && NPGGameCommonInfo.instance.obj.urpSetting.additiveLightRenderData != null
            //     && NPGGameCommonInfo.instance.obj.urpSetting.defaultRenderData != null
            //     && NPGGameCommonInfo.instance.obj.urpSetting.outlineRenderFeature != null
            //     && NPGGameCommonInfo.instance.obj.urpSetting.highRenderPipelineAsset != null
            //     && NPGGameCommonInfo.instance.obj.urpSetting.mediumRenderPipelineAsset != null
            //     && NPGGameCommonInfo.instance.obj.urpSetting.lowRenderPipelineAsset != null)
            //         NPGame.instance.mainCamera.urpSetting = NPGGameCommonInfo.instance.obj.urpSetting;
            // }
            // //TODO 根据机型设置URP配置
        }

        private void _initFont(Action _onComplete)
        {
            GGameCommonInfo.instance.refreshFontAsset(_onComplete);
        }
        
        /// <summary>
        /// 初始化基础UI
        /// </summary>
        /// <param name="_doneDelegate"></param>
        protected void _initBasicUI(Action _doneDelegate)
        {
            //发送埋点-开始初始化基础UI
            GCommon.sendStepReport(TraceConst.START_INIT_BASIC_UI);
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(2);
            stepCounter.regAllDoneDelegate(()=>
            {
                //发送埋点-初始化基础UI完成
                GCommon.sendStepReport(TraceConst.INIT_BASIC_UI_DONE);
                _doneDelegate?.Invoke();
            });
            // stepCounter.regAllDoneDelegate(() => {ALLog.Error($"------------_initBasicUI"); });

            NPGUIAddSceneCenterTip.instance.enterScene();
            NPGUIAddSceneCenterTip.instance.regInitDelegate(stepCounter.addDoneStepCount);
            NPGUIAddSceneScreenSfx.instance.enterScene();
            NPGUIAddSceneScreenSfx.instance.regInitDelegate(stepCounter.addDoneStepCount);

            //初始化通用回退
            NPUICacheMgrCommonBack.instance.initCache();
        }
        
        /// <summary>
        /// 初始化混音器
        /// </summary>
        /// <param name="_doneDelegate"></param>
        protected void _initAudioMixer()
        {
            //发送埋点-开始初始化混音器
            GCommon.sendStepReport(TraceConst.START_INIT_AUDION_MIXER);
            //初始化混音器
            AudioMixerMgr.instance.init(() =>
            {
                //发送埋点-初始化混音器完成
                GCommon.sendStepReport(TraceConst.INIT_AUDION_MIXER_DONE);
                //根据设置再次调整音量信息
                GameSetting.instance.setAudioSettingAfterLoadData();
                
                //单独设置默认点击音效的混音器组，因为默认点击音效不走配表
                PlayAudioMgr.instance.setClickAudioMixerGroup(AudioMixerMgr.instance.getAudioMixerGroup(GRefdataCoreMgr.instance.npGeneral.default_click_audio_mixer_group_id));
            });
        }

        /// <summary>
        /// 判断附加Scene是否需要退出
        /// 一些资源初始化即创建的Scene不可退出
        /// 
        /// 在游戏重登时用于不退出通用Scene的判断函数
        /// </summary>
        /// <returns></returns>
        public bool judgeAddSceneNeedQuit(_AALBasicAdditionScene _addScene)
        {
            if (_addScene == NPGUIAddSceneCenterTip.instance ||
                _addScene == NPGUIAddSceneScreenSfx.instance)
                return false;

            return true;
        }

        /// <summary>
        /// 将指定资源写入指定目录
        /// </summary>
        /// <param name="_abName"></param>
        /// <param name="_folderName"></param>
        /// <param name="_doneDelegate"></param>
        protected void _writeRefdataRes(string _abName, string _folderName, Action<bool> _doneDelegate)
        {
            RefdataResCore.instance.loadAsset("refdata/" + _abName
                , (bool _isSuc, ALAssetBundleObj _assetObj) =>
                {
                //判断是否加载成功
                if(!_isSuc || null == _assetObj)
                    {
                    //报错
                    UnityEngine.Debug.LogError("Init res: " + _abName + " Error!!!");
                    //进行回调处理
                    if(null != _doneDelegate)
                            _doneDelegate(false);
                        return;
                    }

                //将数据中所有文件都导出到对应目录
                string dicPath = RefdataResCore.instance._patchInfo.patchLocalFullPath + "/" + _folderName;
                //检测文件夹是否存在
                if(Directory.Exists(dicPath))
                    {
                    //删除目录
                    Directory.Delete(dicPath, true);
                    }
                //创建文件夹
                Directory.CreateDirectory(dicPath);

                //逐个文件导出
                TextAsset[] objs = _assetObj.assetBundle.LoadAllAssets<TextAsset>();
                    TextAsset tmpObj = null;
                    for(int i = 0; i < objs.Length; i++)
                    {
                        tmpObj = objs[i];
                        if(null == tmpObj)
                            continue;

                    //先删除文件
                    string tmpPath = dicPath + "/" + tmpObj.name;
                        if(File.Exists(tmpPath))
                            File.Delete(tmpPath);

                    //写入文件
                    StreamWriter writer = new StreamWriter(tmpPath);
                        if(null == writer)
                        {
                            UnityEngine.Debug.LogError("res: " + _abName + " write error!" + tmpObj.name);
                        //进行回调处理
                        if(null != _doneDelegate)
                                _doneDelegate(false);
                            return;
                        }

                    //写入文件
                    writer.Write(tmpObj.text);

                    //关闭文件
                    writer.Close();
                        writer.Dispose();
                    }

                    if(null != _doneDelegate)
                        _doneDelegate(true);
                }
                , null);
        }

        /// <summary>
        /// 执行Puerts初始化脚本
        /// </summary>
        protected void _initPuertsScripts()
        {
#if AL_PUERTS
            Debug.Log_EditorOnly("start deal Puerts Global");
            
            //调用全局初始化函数
            NPPuertsMgr.instance.execGeneralScriptsFile("np_global_init");
            
            Debug.Log_EditorOnly("Puerts Global Init Done");      
#endif
        }

        /// <summary>
        /// 所有本地资源加载完成后的处理
        /// </summary>
        protected void _onLocalResLoadOver()
        {
            //发送埋点-所有本地资源加载完成
            GCommon.sendStepReport(TraceConst.LOCAL_RES_LOAD_OVER);

            //扩展字体
            if (null != Game.instance.commonFont)
            {
                Game.instance.commonFont.RequestCharactersInTexture(GGameCommonInfo.instance.obj.initFontTextStr);
            }

            //设置数据加载完成
            GameLoginResLoadController.instance.setResLoadDone();
        }
    
        /// <summary>
        /// 初始化showcase
        /// </summary>
        /// <param name="_action"></param>
        protected void _initShowCaseMgr(Action _action)
        {
            //发送埋点-初始化ShowCase
            GCommon.sendStepReport(TraceConst.INIT_SHOW_CASE);
            ShowCaseMgr.instance.init(_action);
        }

        /// <summary>
        /// 根据当前用户情况展示登录界面
        /// 最后登录操作的地方
        /// </summary>
        protected void _finalInitOp()
        {
            //设置进度
            _m_pAllResProcess.setProcess(1f);
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

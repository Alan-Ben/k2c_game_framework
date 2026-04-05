using UnityEngine;
using System;
using System.Collections;
using System.Text;
using System.IO;
using System.Collections.Generic;
using LitJson;

using ALPackage;
using ChatPackage.Internal;
using Common.ClientData;
using JetBrains.Annotations;
using NPEnum;
using static GOE.Loading;

namespace GOE
{
    //游戏总管理类
    public class Game
    {
        private static Game _g_instance = new Game();
        [NotNull]public static Game instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new Game();
                return _g_instance;
            }
        }
        

        public MainCameraMono mainCamera;

        private string _m_sUid;
        private int _m_iServerTimeZone; //服务器时区
        private int _m_iDstOffset;//夏令时时差
        private bool _m_bIsInited;//玩家状态是否初始化完成，如未完成则不会响应2-4的消息处理
        private ActionContainer _m_ACActionContainer;
        private Font _m_fCommonFont;//通用文字
        private long _m_lIsInTutorialTag = 0;//是否在引导中的标识
        private int _m_iIsSetCustomValue = 0;//是否已经设置完firebase crashlytics自定义参数(以二进制判断，每一位代表一个参数是否设置)
        private bool _m_bDialogueCanAutoPlay = false;//对话是否自动播放

        /// <summary>
        /// 游戏内用于管理ILRuntime热更的处理对象
        /// </summary>
        private ALHotfixMgr_ILRuntime _m_hmHotfixMgr;
        
        //展示的最大宽度和高度，用于判断对象是否展示的处理
        private float _m_fShowMaxWidth;
        private float _m_fShowMaxHeight;
        private float _m_fShowMinWidth;
        private float _m_fShowMinHeight;

#if UNITY_EDITOR
        private string _m_sApplicationDataPath;

        public string applicationDataPath_EditorOnly { get { return _m_sApplicationDataPath; } }
#endif

        //图集数据集合
        private ALAssetBundleObj _m_abAtlasAB;
        //已经加载的图集数据
        private Dictionary<string, UnityEngine.U2D.SpriteAtlas> _m_dicSpriteAtlasDic = new Dictionary<string, UnityEngine.U2D.SpriteAtlas>();

        private bool _m_bIsDebugCDN = false;//是否开启debugCDN
        public Dictionary<string, string> debugCdnSetting = new Dictionary<string, string>();//开启debugCdn的配置

        public bool isInited { get { return _m_bIsInited; } }

        public string uid { get { return _m_sUid; } }
        public int ServerTimeZone { get { return _m_iServerTimeZone; } set { _m_iServerTimeZone = value; } }
        public int DstOffset { get { return _m_iDstOffset; } set { _m_iDstOffset = value; } }

        public bool isInTutorial { get { return _m_lIsInTutorialTag != 0; } }

        public Font commonFont { get { return _m_fCommonFont; } }
        public ALHotfixMgr_ILRuntime ILRuntimeMgr { get { return _m_hmHotfixMgr; } }
        public bool IsDebugCDN { get { return _m_bIsDebugCDN; } set { _m_bIsDebugCDN = value; } }
        public bool dialogueCanAutoPlay { get { return _m_bDialogueCanAutoPlay; } set { _m_bDialogueCanAutoPlay = value; } }

        /// <summary>
        /// 是否走cdn
        /// </summary>
        public bool isUseCdn
        {
            get
            {
                if (null == mainCamera || null == mainCamera.platInfo || null == mainCamera.platInfo.phpUrlForLoginList)
                    return false;

                return mainCamera.platInfo.phpUrlForLoginList.Count > 0;
            }
        }

        public void Init()
        {
#if UNITY_EDITOR
            _m_sApplicationDataPath = Application.dataPath;
#endif

            //设置默认帧数
#if UNITY_EDITOR
            Application.targetFrameRate = 60;
#elif UNITY_ANDROID
        Application.targetFrameRate = 30;
#elif UNITY_IOS
        Application.targetFrameRate = 30;

        if(UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhone6
            || UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhone6Plus
            || UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhone6S
            || UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhone6SPlus
            || UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhone7
            || UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhone7Plus
            || UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhoneSE1Gen
            || UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPadPro1Gen
            || UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPadPro10Inch1Gen
            || UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPad5Gen)
        {
            Application.targetFrameRate = 30;
        }
#else
        Application.targetFrameRate = 30;
#endif

#if UNITY_EDITOR
            setEditorQuality();
#elif UNITY_ANDROID
        setAndroidQuality();
#elif UNITY_IOS
        setIosQuality();
#else
        setAndroidQuality();
#endif


            Application.runInBackground = true;

            //初始化默认语言
            TextEx.g_Lang = ENPLanguage.EN_US;

            GameSetting.instance.init();

            //根据开关设置高帧率
            if (GameSetting.instance.usingHighFrame)
                setTargetFrameRate(GameSetting.instance.usingHighFrame);

            //初始化屏幕范围判断距离
            _m_fShowMaxWidth = Screen.width * 1.1f;
            _m_fShowMaxHeight = Screen.height * 1.1f;
            _m_fShowMinWidth = Screen.width * -0.1f;
            _m_fShowMinHeight = Screen.height * -0.1f;

            //初始化根据设置调整音量信息
            GameSetting.instance.setAudioSettingAfterLoadData();

#if UNITY_EDITOR
            //初始化错误码列表
            ProtocolErrorCodeResult.InitRegistResults();      
#endif
        }

        /// <summary>
        /// 设置用户Id
        /// </summary>
        /// <param name="_uid"></param>
        public void setUid(string _uid)
        {
            _m_sUid = _uid;
        }

        //切换帧率模式
        public void setTargetFrameRate(bool _highFrame)
        {
            // 如果是高刷机器则改为当前屏幕刷新率，避免因为帧率和屏幕刷新率不是整数倍导致抖动
            Application.targetFrameRate = _highFrame ? (Screen.currentResolution.refreshRate > 60 ? Screen.currentResolution.refreshRate : 60) : 30;
        }

        /****************
         * 设置编辑器下的品质
         **/
        public void setEditorQuality()
        {
            //抗锯齿
            QualitySettings.antiAliasing = 2;
            //各向异性过滤
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
            //设置阴影参数
            QualitySettings.shadowResolution = ShadowResolution.Medium;
            QualitySettings.shadowProjection = ShadowProjection.CloseFit;
            QualitySettings.shadows = ShadowQuality.HardOnly;
            QualitySettings.shadowCascades = 2;
            QualitySettings.shadowDistance = 30;

            //骨骼动画权重
            QualitySettings.skinWeights = SkinWeights.Unlimited;
            //垂直同步
            QualitySettings.vSyncCount = 0;

            //设置显示级别
            QualitySettings.maximumLODLevel = 400;
        }

        /****************
         * 设置安卓下的品质
         **/
        public void setAndroidQuality()
        {
            //抗锯齿
            QualitySettings.antiAliasing = 0;
            //各向异性过滤
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
            //设置阴影参数
            QualitySettings.shadowResolution = ShadowResolution.Medium;
            QualitySettings.shadowProjection = ShadowProjection.CloseFit;
            QualitySettings.shadows = ShadowQuality.HardOnly;
            QualitySettings.shadowCascades = 2;
            QualitySettings.shadowDistance = 30;

            //骨骼动画权重
            QualitySettings.skinWeights = SkinWeights.Unlimited;
            //垂直同步
            QualitySettings.vSyncCount = 0;

            //设置显示级别
            QualitySettings.maximumLODLevel = 200;
        }

        /****************
         * 设置安卓下的品质
         **/
        public void setIosQuality()
        {
            //抗锯齿
            QualitySettings.antiAliasing = 2;
            //各向异性过滤
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
            //设置阴影参数
            QualitySettings.shadowResolution = ShadowResolution.Medium;
            QualitySettings.shadowProjection = ShadowProjection.CloseFit;
            QualitySettings.shadows = ShadowQuality.HardOnly;
            QualitySettings.shadowCascades = 2;
            QualitySettings.shadowDistance = 30;

            //骨骼动画权重
            QualitySettings.skinWeights = SkinWeights.Unlimited;
            //垂直同步
            QualitySettings.vSyncCount = 0;

            //设置显示级别
            QualitySettings.maximumLODLevel = 400;
        }

        /****************
         * 在进入战斗的时候触发的函数
         **/
        public void onEnterBattle()
        {
            //停止战斗外的背景音乐
            PlayAudioMgr.instance.stopBackgroundMusic();

            //释放无用大图
            GGUITextureCacheMgr.instance.releaseUnusedTexture();
            //释放自定义视图
            NPGGUICustomWndMgr.instance.discardAll();
        }

        /// <summary>
        /// 设置是否自动登入，目前是切换语言设置，后面可以参考别的游戏改成有个勾选，勾选后都自动登入不用进入stargame界面
        /// </summary>
        /// <param name="_isAutoLogin"></param>
        public void setIsAutoLogin(bool _isAutoLogin)
        {
            GameInit_SelectServer.instance.isAutoLogin = _isAutoLogin;
        }

        /************
         * 重新登录的操作
         *
         **/
        public void relogin()
        {
            //断开连接
            if(null != NPGSClientListener.instance)
                NPGSClientListener.instance.logout();
            
            //清空所有提示信息
            NPMesMgr.instance.reset();
            //可能切换服务器重登需要重置Crashlytics自定义参数
            _m_iIsSetCustomValue = 0;
            //重置一下对话自动播放选项
            _m_bDialogueCanAutoPlay = false;

            //TODO: 此处需要重新开始请求一次PHP信息，在返回后进行资源版本判断
            GameInit.instance.reset();
            //重置最后登录信息
            LoginTokenSetting.instance.clearLastLoginInfo();
            //清除缓存角色列表信息
            GameCDNServerListMgr.instance.clearCharacterList();

            //进入云层UI
            Loading.showLoading(_complete =>
            {
                //退出当前主ui scene
                NPGUISceneEmpty.instance.enterScene();
                //退出所有addtionScene
                ALSceneCore.instance.quitAllAdditionScene((int)ENPSceneType.UI_SCENE, GameInit_GameResInit.instance.judgeAddSceneNeedQuit);
                
                /** 重新回到登录界面 */
                PStageEmpty.instance.enterStage();
                PStageEmpty.instance.regInitDelegate(() =>
                {
                    //退出云层
                    _complete?.Invoke();
                    GStageLogin.instance.enterStage();
                    GStageLogin.instance.regInitDelegate(() =>
                    {
                        //清空缓存数据
                        _reloginClear();

                        //重新走登录流程
                        GameInit.instance.init();
                    });
                });

            });
        }
        /// <summary>
        /// 不重置登录消息重新登录
        /// </summary>
        public void reloginByDefault()
        {
            //断开连接
            if (null != NPGSClientListener.instance)
                NPGSClientListener.instance.logout();
            
            //清空所有提示信息
            NPMesMgr.instance.reset();
            //可能切换服务器重登需要重置Crashlytics自定义参数
            _m_iIsSetCustomValue = 0;
            //重置一下对话自动播放选项
            _m_bDialogueCanAutoPlay = false;

            //TODO: 此处需要重新开始请求一次PHP信息，在返回后进行资源版本判断
            GameInit.instance.reset();

            //声明调用函数，如果资源未初始化完成则不能调用Loading
            LoadFunction dealRelogin = (_complete) =>
                {
                    //退出当前主ui scene
                    NPGUISceneEmpty.instance.enterScene();
                    //退出所有addtionScene
                    ALSceneCore.instance.quitAllAdditionScene((int)ENPSceneType.UI_SCENE, GameInit_GameResInit.instance.judgeAddSceneNeedQuit);

                    /** 重新回到登录界面 */
                    PStageEmpty.instance.enterStage();
                    PStageEmpty.instance.regInitDelegate(() =>
                    {
                        //退出云层
                        _complete?.Invoke();
                        GStageLogin.instance.enterStage();
                        GStageLogin.instance.regInitDelegate(() =>
                        {
                            //清空缓存数据
                            _reloginClear();

                            //重新走登录流程
                            GameInit.instance.init();
                        });
                    });
                };

            //判断游戏资源是否初始化完成
            if(GameInit_UpdateGameRes.instance.isDone)
            {
                //进入云层UI
                Loading.showLoading(dealRelogin);
            }
            else
            {
                //如果资源未初始化，直接调用结果
                dealRelogin(null);
            }
        }

        /*****************
         * 设置屏幕分辨率
         **/
#if UNITY_ANDROID
    private int _m_iScaleWidth = 0, _m_iScaleHeight = 0;
#endif
        public void setDesignContentScale()
        {
#if UNITY_ANDROID
        //根据CPU主频处理
        if (SystemInfo.processorFrequency >= 1600)
            return;

        if (_m_iScaleWidth == 0 && _m_iScaleHeight == 0)
        {
            int width = Screen.currentResolution.width;
            int height = Screen.currentResolution.height;
            int designWidth = 960;
            int designHeight = 640;
            float s1 = (float)designWidth / (float)designHeight;
            float s2 = (float)width / (float)height;
            if (s1 < s2)
            {
                designWidth = (int)Mathf.FloorToInt(designHeight * s2);
            }
            else if (s1 > s2)
            {
                designHeight = (int)Mathf.FloorToInt(designWidth / s2);
            }
            float contentScale = (float)designWidth / (float)width;
            if (contentScale <= 0.8f)
            {
                _m_iScaleWidth = designWidth;
                _m_iScaleHeight = designHeight;
            }
        }
        if (_m_iScaleWidth > 0 && _m_iScaleHeight > 0)
        {
            if (_m_iScaleWidth % 2 == 0)
            {
                _m_iScaleWidth += 1;
            }
            else
            {
                _m_iScaleWidth -= 1;
            }
            Screen.SetResolution(_m_iScaleWidth, _m_iScaleHeight, true);
        }
#endif
        }

        /// <summary>
        /// 玩家初始登录游戏成功时的初始操作
        /// </summary>
        /// <param name="_enteredActoin"></param>
        public void loginGameInitUINode(Action _enteredAction)
        {
            //发送埋点-正式进入游戏界面
            GCommon.sendStepReport(TraceConst.ENTER_GAME);

            _m_bIsInited = true;

            //当前进入游戏了，不需要推送，清空本地推送
            LocalPushMgr.instance.clearLocalPush();
            //打开跑马灯
            NPGUIAddSceneMarquee.instance.enterScene();

            if (NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.IS_SET_DEFAULT) == 0) //未进行初始化
            {
                //进入预创角node，让引导去控制创角
                QueueMgr.instance.AddNode(new GNodePreCreatePlayer(_enteredAction));
                GCommon.sendStepReport(TraceConst.ENTER_GAME_PER_CREATE);
            }
            else
            {
                //不能的去主城
                QueueMgr.instance.AddNode(new GNodeBuilding(_enteredAction));
                GCommon.sendStepReport(TraceConst.ENTER_GAME_CITY);
            }
            
            // Action afterCheckIsReadStoryVideo = () =>
            // {
            //     if (NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.IS_SET_DEFAULT) == 0) //未进行初始化
            //     {
            //         //进入预创角node，让引导去控制创角
            //         QueueMgr.instance.AddNode(new GNodePreCreatePlayer(_enteredAction));
            //     }
            //     else
            //     {
            //         //不能的去主城
            //         QueueMgr.instance.AddNode(new GNodeBuilding(_enteredAction));
            //         GCommon.sendStepReport(TraceConst.ENTER_GAME_CITY);
            //     }
            // };
            
            // //如果剧情没播放优先去开篇剧情
            // if (!NPPlayer.instance.playerInfoComp.clientDataRemarkInfo.isReadStoryVideo())
            // {
            //     GStageMain.instance.enterStage();
            //     GStageMain.instance.regInitDelegate(() =>
            //     {
            //         QueueMgr.instance.AddNode(new GNodeIntroStoryNew(_enteredAction, () =>
            //         {
            //             NPPlayer.instance.playerInfoComp.clientDataRemarkInfo.setIsReadStoryVideo();
            //             afterCheckIsReadStoryVideo();
            //         }));
            //     });
            // }
            // else
            // {
            //     afterCheckIsReadStoryVideo();
            // }
        }

        /// <summary>
        /// 初始化通用字体
        /// </summary>
        /// <param name="_font"></param>
        public void initCommonFont(Font _font)
        {
            _m_fCommonFont = _font;
        }
        /// <summary>
        /// 初始化通用图集AB
        /// </summary>
        /// <param name="_font"></param>
        public void initAtlasAB(ALAssetBundleObj _assetObj)
        {
            //如是图集则直接存储
            _m_abAtlasAB = _assetObj;

            //注册事件
            UnityEngine.U2D.SpriteAtlasManager.atlasRequested += _onRequestSpriteAtlas;
        }

        /// <summary>
        /// 图集管理对象申请加载图集的响应函数
        /// </summary>
        /// <param name="_tag"></param>
        protected void _onRequestSpriteAtlas(string _tag, Action<UnityEngine.U2D.SpriteAtlas> _action)
        {
            UnityEngine.Debug.LogError("request: " + _tag);
            if(null == _action)
                return;

            _action(loadSpriteAtlas(_tag));
        }

        /// <summary>
        /// 根据标记加载对应的图集
        /// </summary>
        /// <param name="_tag"></param>
        public UnityEngine.U2D.SpriteAtlas loadSpriteAtlas(string _tag)
        {
            if(null == _m_abAtlasAB)
                return null;

            UnityEngine.U2D.SpriteAtlas res = null;
            //判断数据集是否有数据
            if(_m_dicSpriteAtlasDic.TryGetValue(_tag, out res))
                return res;

            //加载
            res = _m_abAtlasAB.load<UnityEngine.U2D.SpriteAtlas>(_tag);
            //放入集合
            _m_dicSpriteAtlasDic.Add(_tag, res);

            return res;
        }


        //切换账号时需要进行的数据清空相关操作
        private void _reloginClear()
        {
            if(!GRefdataCoreMgr.instance.isInited)
                return;

            _m_bIsInited = false;
            NPPlayer.resetNPPlayer();
            AccountSettingMgr.instance.discard();
            //红点数据清空
            RedTipMgr.instance.discard();
            
            //释放音频
            PlayAudioMgr.instance.reset();

            //重置运营公告管理类
            AnnouncementMgr.instance.reset();

            // 主城推送弹窗重置
            MainCityPushNoticeMgr.instance.reset();
            
            // 重置Notice
            NPUINoticeMgr.instance.reset();
        }

        //打开引导中的状态
        public void openIsInTutorial()
        {
            openIsInTutorial(0);
        }
        public void openIsInTutorial(int _sysIndex_64)
        {
            if (_sysIndex_64 > 64)
            {
                ALLog.Error("Open Is In Tutorial SysIndex is Bigger than 64");
                _sysIndex_64 = 64;
            }
            else if (_sysIndex_64 < 0)
            {
                ALLog.Error("Open Is In Tutorial SysIndex is Smaller than 0");
                _sysIndex_64 = 0;
            }

            _m_lIsInTutorialTag = _m_lIsInTutorialTag | (1L << _sysIndex_64);
            if (_AALMonoMain.instance.showDebugOutput)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[Tutorial] √ openIsInTutorial({_sysIndex_64}): isInTutorial:{isInTutorial}");
            }
            WinMsg.SendMsg(WinMsgType.ON_START_TUTORIAL);
            
            //发送消息刷新一下condition
            WinMsg.SendMsg(WinMsgType.CUSTOM_RELOAD);
        }

        //关闭引导中的状态
        public void closeIsInTutorial()
        {
            closeIsInTutorial(0);
        }
        public void closeIsInTutorial(int _sysIndex_64)
        {
            if (_sysIndex_64 > 64)
            {
                ALLog.Error("Open Is In Tutorial SysIndex is Bigger than 64");
                _sysIndex_64 = 64;
            }
            else if (_sysIndex_64 < 0)
            {
                ALLog.Error("Open Is In Tutorial SysIndex is Smaller than 0");
                _sysIndex_64 = 0;
            }

            _m_lIsInTutorialTag = _m_lIsInTutorialTag & ~(1L << _sysIndex_64);
            
            //发送消息刷新一下condition
            WinMsg.SendMsg(WinMsgType.CUSTOM_RELOAD);
            
            if (_AALMonoMain.instance.showDebugOutput)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[Tutorial] × openIsInTutorial({_sysIndex_64}): isInTutorial:{isInTutorial}");
            }
        }

        /// <summary>
        /// 统一设置游戏中camera的Rect,确保在开启HDR后功能可以正常，要求所有camera的大小是一致的
        /// </summary>
        public void setGameCameraRect(Rect _cameraRect)
        {
            if (null != CameraController.instance.controlCamera)
                CameraController.instance.controlCamera.rect = _cameraRect;
            
            if(null != Game.instance.mainCamera.uiCamera)
                Game.instance.mainCamera.uiCamera.rect = _cameraRect;

            if (RTMainCameraController.instance.controlCamera != null)
                RTMainCameraController.instance.controlCamera.rect = _cameraRect;
        }
        
        /// <summary>
        /// 初始化ILRuntime热更对象
        /// </summary>
        /// <param name="_dllUrl"></param>
        /// <param name="_dllSaveName"></param>
        /// <param name="_pdbUrl"></param>
        /// <param name="_pdbSaveName"></param>
        public void initILRuntimeMgr(string _dllSaveName, string _pdbSaveName)
        {
            _m_hmHotfixMgr = ALHotfixMgr_ILRuntime.createILRuntimeMgr(_dllSaveName, _pdbSaveName);
        }

        /// <summary>
        /// 释放hotfix，一般只在程序关闭时候调用
        /// </summary>
        public void discardRuntimeMgr()
        {
            if(null != _m_hmHotfixMgr)
                _m_hmHotfixMgr.discard();
            _m_hmHotfixMgr = null;
        }

        #region 按键处理部分
        public void dealBtn()
        {
            bool control = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
            bool alt = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
            bool shift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

            //返回键处理
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                QueueMgr.instance.DoUIRollBackByEsc();
            }

#if UNITY_EDITOR || UNITY_STANDALONE //PC包和editor下调用
            
            if(alt && shift && Input.GetKeyDown(KeyCode.K))
            {
                //进入作弊node
                QueueMgr.instance.AddNode(new MainUIAddSceneNode_Var_InGame(NPGGUIAddSceneCheat.instance, UINodeTagConst.C_Sys_CheatNode));
            }

#endif
            
#if UNITY_EDITOR
            //ctrl + [ - 强制设置所有引导完成
            if((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.LeftBracket))
            {
                CheatMgr.instance.dealCheat("player allpass");
                NPPlayer.instance.tutorialComp.forceSetAllDone();
                NPPlayer.instance.funcUnlockComp.forceSetAllShowDone();
                QueueMgr.instance.AddNode(new GNodeBuilding());
            }

            //ctrl + ] - 强制重置所有强制引导
            if((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.RightBracket))
            {
                NPPlayer.instance.tutorialComp.resetAllForce();
                //测试玩家经验收集表现
                //NPPlayerCityBuildingResGenerateInfo info = NPPlayer.instance.playerCityBuildingComp.resInfoMgr.getBuildResInfoByResType(NPEnum.ENPBuilding_Res.GOLD);
                //NPPlayer.instance.playerCityBuildingComp.resInfoMgr.onBldResHarvest(info, 10000);
            }

            //ctrl + P - 设置当前所在引导完成
            if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.PageDown))
            {
                NPPlayer.instance.tutorialComp.forceSetCurTutorialDone();
                //测试玩家经验收集表现
                //NPPlayerCityBuildingResGenerateInfo info = NPPlayer.instance.playerCityBuildingComp.resInfoMgr.getBuildResInfoByResType(NPEnum.ENPBuilding_Res.P_EXP);
                //NPPlayer.instance.playerCityBuildingComp.resInfoMgr.onBldResHarvest(info, 100);
            }

            //处理战斗重连
            if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                && Input.GetKeyDown(KeyCode.R))
            {
                NPGSClientListener.instance.dealClientLogout();
                //NPGame.instance.relogin();
            }
            
            //设置胜利场数，直接跳过和机器人匹配
            if((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.J))
            {
                MemoryMgr.instance.dealReleaseMemory();
            }

            if((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.F1))
            {
                CheatMgr.instance.reqGmCommand("player battle 1"); // 胜利
            }

            if((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.F2))
            {
                CheatMgr.instance.reqGmCommand("player battle 2"); // 失败
            }

            if((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.F3))
            {
                CheatMgr.instance.reqGmCommand("player battle 0"); // 平局
            }

            if((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.F4))
            {
                CheatMgr.instance.reqGmCommand("player adduni 12003 1");
            }

            if(Input.GetKey(KeyCode.RightControl) && Input.GetKeyDown(KeyCode.Slash))
            {
                CheatMgr.instance.reqGmCommand("card addall");
            }

            if(GCommon.isAltPressed())
            {
                if(Input.GetKeyDown(KeyCode.Alpha1))
                {
                    CheatMgr.instance.reqGmCommand("task finish 0");
                }
            }

            if(Input.GetKeyDown(KeyCode.M))
            {
                CheatMgr.instance.reqGmCommand("soldier unlockall");
                CheatMgr.instance.reqGmCommand("hero unlockall");

            }
            
            //重登
            if (control && alt && Input.GetKeyDown(KeyCode.R))
            {
                relogin();
            }

            //打开联盟
            if (control && Input.GetKeyDown(KeyCode.G))
            {
                QueueMgr.instance.AddNode(new GNodeEveningDungeonRankAndReward(EEveningDungeonRankAndRewardDetailTabType.REWARD));
            }

            if (control && Input.GetKeyDown(KeyCode.T))
            {
                GCommon.enterUIMainNodeShow(ESysSceneType.EVENING_DUNGEON_ENTRANCE);
            }
#endif
        }
        #endregion

        #region 通用UI部分函数
        /// <summary>
        /// 展示对应的信息
        /// </summary>
        /// <param name="_info"></param>
        public void UIC_showTextInfo(string _info)
        {
            NPGUIAddSceneCenterTip.instance.showTextInfo(_info);
        }

        /// <summary>
        /// 展示对应的错误提示
        /// </summary>
        /// <param name="_errCode"></param>
        public void UIC_showErrorInfo(int _errCode)
        {
            NPGUIAddSceneCenterTip.instance.showErrorInfo(_errCode);
        }

        //判断对应的坐标是否在屏幕内
        public bool checkInScreen(Vector2 _vec)
        {
            //判断是否超出一定范围
            return !(_vec.x > _m_fShowMaxWidth || _vec.x < _m_fShowMinWidth
                || _vec.y > _m_fShowMaxHeight || _vec.y < _m_fShowMinHeight);
        }
        public bool checkInScreen(Vector3 _vec)
        {
            //判断是否超出一定范围
            return !(_vec.x > _m_fShowMaxWidth || _vec.x < _m_fShowMinWidth
                || _vec.y > _m_fShowMaxHeight || _vec.y < _m_fShowMinHeight);
        }

        #endregion


        /// <summary>
        /// 重新设置部分Crashlytics参数
        /// </summary>
        public void setCrashlyticsCustomValue()
        {
            if (SDKMgr.instance.isUseSDK)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("{");
                //用二进制记录某个参数已经设置或者未设置
                if (CDNSetting_ClientConfigInfo.instance.platformId != 0 && ((_m_iIsSetCustomValue & (1 << 0)) == 0))
                {
                    builder.Append($"\"platform_id\":\"{CDNSetting_ClientConfigInfo.instance.platformId}\"");
                    _m_iIsSetCustomValue |= (1 << 0);
                }

                if (CDNSetting_AreaInfo.instance.areaId != "-1" && ((_m_iIsSetCustomValue & (1 << 1)) == 0))
                {
                    if (builder.Length > 1)
                        builder.Append(",");
                    builder.Append($"\"area_id\":\"{CDNSetting_AreaInfo.instance.areaId}\"");
                    _m_iIsSetCustomValue |= (1 << 1);
                }

                if (GameInit_SelectServer.instance.loginServerLogicId != 0 && ((_m_iIsSetCustomValue & (1 << 2)) == 0))
                {
                    if (builder.Length > 1)
                        builder.Append(",");
                    builder.Append($"\"server_id\":\"{GameInit_SelectServer.instance.loginServerLogicId}\"");
                    _m_iIsSetCustomValue |= (1 << 2);
                }

                if (!string.IsNullOrEmpty(Game.instance.uid) && Game.instance.uid != "0" && ((_m_iIsSetCustomValue & (1 << 3)) == 0))
                {
                    if (builder.Length > 1)
                        builder.Append(",");
                    builder.Append($"\"uid\":\"{Game.instance.uid}\"");
                    _m_iIsSetCustomValue |= (1 << 3);
                }

                if (NPPlayer.instance != null && NPPlayer.instance.playerInfo != null && NPPlayer.instance.playerInfo.CID != 0 && ((_m_iIsSetCustomValue & (1 << 4)) == 0))
                {
                    if (builder.Length > 1)
                        builder.Append(",");
                    builder.Append($"\"cid\":\"{NPPlayer.instance.playerInfo.CID}\"");//角色ID
                    _m_iIsSetCustomValue |= (1 << 4);
                }
                builder.Append("}");

                if(builder.Length > 2)
                    SDKMgr.instance.firebase_setCustomKey(builder.ToString(), null, null);
            }
        }

        /// <summary>
        /// 传回LS服务器的Json数据 IF可能会有坑要注意！data["language"]这样访问会报错
        /// </summary>
        /// <returns></returns>
        public string initLSJsonData()
        {
            JsonData data = new JsonData();

            data["clientIp"] = GCommon.getClientIp();

            string jsonString = JsonMapper.ToJson(data);
            if (_AALMonoMain.instance.showDebugOutput)
            {
                Debug.Log($"LS CustomMsg：{jsonString}");
            }
            return jsonString;
        }

        /// <summary>
        /// 传回GS服务器的Json数据 IF可能会有坑要注意！data["language"]这样访问会报错
        /// </summary>
        /// <returns></returns>
        public string initGSJsonData()
        {
            JsonData data = new JsonData();

            ENPLanguage currentLanguage = GameSetting.instance.getCurrentLanguage();
            data["language"] = currentLanguage.toPHPLanguageCode();//当前语言
            data["deviceType"] = _getdeviceType();//操作系统
            data["clientPackageName"] = Application.identifier;//客户端包名
            data["version"] = Application.version;//客户端版本
            data["adid"] = SDKMgr.instance.deviceId;//设备id
            data["nation"] = SDKMgr.instance.sysCountry;//国家
            data["adfrom"] = _getdeviceType();//一级渠道：无渠道时，苹果默认ios，安卓默认aos
            data["adfrom2"] = SDKLoginSetting.instance.getTokenData()?.user?.adfrom2;//二级渠道名称：无渠道时，默认使用default
            data["afid"] = SDKMgr.instance.appsflyerId;//广告afid


            string jsonString = JsonMapper.ToJson(data);
            if (_AALMonoMain.instance.showDebugOutput)
            {
                Debug.Log($"GS CustomMsg：{jsonString}");
            }
            return jsonString;
        }

        //获取设备类型
        private string _getdeviceType()
        {
#if UNITY_IOS
            return "ios";
#elif UNITY_ANDROID
            return "aos";
#elif UNITY_STANDALONE || UNITY_EDITOR
            return "pc";
#endif
        }
    }
}

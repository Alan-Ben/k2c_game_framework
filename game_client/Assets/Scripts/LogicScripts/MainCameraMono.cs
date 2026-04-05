using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using ALPackage;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


namespace GOE
{
    public enum ENPSpaceDebugDrawType
    {
        DONT_DRAW,
        DRAW_SHOW_UNIT_DEALER,
        DRAW_SHOW_UNIT_TREE_DEALER,
        DRAW_DATA_TREE_DEALER,
        DRAW_LOGIC_UNIT_TREE_DEALER,
        DRAW_LOGIC_UNIT_DEALER
    }

    /***********
     * 连接对应平台的登录信息
     **/
    [Serializable]
    public class WCGPlatLoginInfo
    {
        public string editorShowTag;//仅用于在列表中显示对应的枚举标签
        public EWCGPlatType platType;
        public string connectIp;
        [ALHeader("连接哪一台用户服务器，填0表示使用本地记录的Id")]
        public int serverId;
        public int port;
        public string areaResURL;
        public string gameResURL;
        public string refdataResURL;
        public string specialResURL;
        public string videoResURL;

        [ALHeader("Ilruntime的下载地址")]
        public string hotfixURL;
        [ALHeader("injectFIx的下载地址")]
        public string injectFixURL;
        [ALHeader("远端音效资源下载地址")]
        public string audioResURL;

        [ALHeader("如果有配置地址，会走模拟自定义的cdn逻辑，一般用于封测没接平台")]
        public List<string> tempCdnRootUrl;
        
        [ALHeader("最开始的下载配置信息的URL列表，会进行遍历检测")]
        public List<string> phpUrlForLoginList;
        public string phpADDefaultURL;          //埋点默认地址，判断如果获取PHP后台配置的地砖就用默认地址
                                                //本平台累加的构建版本号
        public int channelId;
    }

    /***********
     * WCG游戏设置信息结构体
     **/
    [Serializable]
    public class GameSettingInfo
    {
        [ALHeader("是否打印协议")]
        public bool printProtocol;//是否打印协议
        [ALHeader("是否打印001_021协议")]
        public bool printProtocol_001_021 = true;
        [ALHeader("是否发送协议请求时模拟弱网环境，只在editor环境下有用")]
        public bool isSendMsgDelay;
        [ALHeader("协议尺寸显示的控制大小")]
        public int protocolPrintMinSize = 2048;
        [ALHeader("协议尺寸超出报错的控制大小")]
        public int protocolErrorMinSize = 20480;
        [ALHeader("是否打印语言表找不到")]
        public bool printLanguageKeyError;
        [ALHeader("是否打印UI节点信息")]
        public bool printNodeMessage;
        [ALHeader("是否绘制辅助信息")]
        public bool drawDebug;
        [ALHeader("战斗逻辑时间是否使用unity本地时间，下面的多线程模式勾选去掉的话在editor下可以实现一帧帧播放战斗，仅editor有效，注意只能1倍速测试")]
        public bool battleUsingUnityTime = true;
        [ALHeader("是否开启战斗多线程")]
        public bool usingThreadDoBattleLogic = true;
        [ALHeader("绘制大地图的调试信息设置")]
        public ENPSpaceDebugDrawType drawSpaceDebug;
        [ALHeader("是否输出战斗日志用于与服务器比对")]
        public bool showDebug;//是否输出log
        [ALHeader("是否强制使用sdk及cdn方式登录和处理业务，一般用于功能验收")]
        public bool forceUseCDN = false;
        public GameObject debug;//输出log对应的对象


        public bool RecordPlayerOperation;//是否记录玩家的操作信息
        public bool RecordPlayerRes;//是否记录玩家的资源获取信息
        public string outputPlayerOperationRecordPath;//输出玩家操作记录信息的默认路径，为空则取默认值

        public bool outputReconnectFile;//输出断线重连的信息
        public string outputReconnectFilePath;//输出断线重连信息的默认路径，为空则去默认值
        public string outputPlayerResFilePath;//输出玩家资源获取信息的默认路径，为空则取默认值

        public bool useOpDeltaPos;//是否使用操作偏差
        public bool forceTutorialDelay;//强制引导延迟
    }

    /**************
     * 客户端版本号信息
     * 主版本号 . 子版本号 . 修正版本号 . 编译版本号
     **/
    [Serializable]
    public class WCGClientInfo
    {
        public int majorVersion;    //主版本号
        public int minorVersion;    //子版本号
        public int revisionVersion; //修正版本号
        public int _buildVersion;    //编译版本号
        public int _dateVersion;    //日期版本号

        public string clientId { get { return $"{majorVersion}.{minorVersion}.{revisionVersion}.{buildVersion}"; } }
        public int clientVersion { get { return (majorVersion * 1000000) + (minorVersion * 10000) + (revisionVersion * 100) + _buildVersion; } }
        public int buildVersion { get { return _buildVersion + Game.instance.mainCamera.platInfo.channelId; } }
        public string clientShowId { get { return $"{majorVersion}.{minorVersion}.{revisionVersion}.{buildVersion}.{_dateVersion}"; } }
    }

    /**************
     * 游戏主摄像头脚本对象
     **/
    public class MainCameraMono : _AALMonoMain
    {
        public static MainCameraMono selfInstance { get { return (MainCameraMono) instance; } }
        
        //当前客户端版本号
        [NotNull] public ClientVersionSetting npClientVersionSetting;
        //当前客户端平台
        private EWCGClientPlat _m_eClientPlat = EWCGClientPlat.NONE;
        public EWCGClientPlat clientPlat { get { return _m_eClientPlat; } }
        //当前使用的平台类型
        public EWCGPlatType platType;
        //是否走SDK登录
        public bool isUseSDK;

        //所有可连接平台的信息列表
        public List<WCGPlatLoginInfo> platInfoList;
        //自定义扩展模式下的平台名称
        public string extraPlatName;
        //扩展模式的设置信息
        public WCGPlatLoginInfo extraPlatInfo;

        //设置信息结构体
        public GameSettingInfo gameSetting;

        /** 跟随的摄像头，用于显示战斗对象，需要一直显示在地表之上 */
        public List<Camera> followCamera;
        /// <summary>
        /// ShowCase主相机
        /// </summary>
        public Camera rtMainCamera;
        /** 全局的方向光 */
        public Light directionLight;
        /** 全局的方向光 */
        public Light showcaseDirectionLight;
        /** 全局的后效 */
        public Volume globalVolume;
        /// <summary>
        /// 用于修改主相机的URP配置的mono
        /// </summary>
        public UniversalAdditionalCameraData mainCameraData;
        public UniversalAdditionalCameraData uiCameraData;
        public UniversalAdditionalCameraData rtMainCameraData;
        /// <summary>
        /// URP相关的配置，可通过ab加载来替换
        /// </summary>
        public NPURPSetting urpSetting;
        /** UI缩放处理对象 */
        public CanvasScaler fullCanvasScaler;

        [Header("自适应大小Canvas列表")]
        public List<Canvas> adjustScreenCanvas;
        [Header("在editor下, 屏幕左边距占比"), Range(0, 1)]
        public float inEditorScreenLeftMarginPercent = 0f;
        [Header("在editor下, 屏幕右边距占比"), Range(0, 1)]
        public float inEditorScreenRightMarginPercent = 0f;
        [Header("在editor下, 屏幕上边距占比"), Range(0, 1)]
        public float inEditorScreenTopMarginPercent = 0.05f;
        [Header("在editor下, 屏幕下边距占比"), Range(0, 1)]
        public float inEditorScreenBotMarginPercent = 0.02f;
        
        //用于专门渲染特殊单位的摄像头
        public Camera specialActorCamera;

        [ALHeader("所有输入的遮罩对象，可以屏蔽所有屏幕输入，一般用于特殊展示流程的时候控制输入使用")]
        public GameObject allInputMask;

        public GameObject audioListenerGo;
        
        [ALHeader("四个遮罩，用于挡住空白区域")]
        public GameObject maskTop;
        public GameObject maskBot;
        public GameObject maskLeft;
        public GameObject maskRight;
        
        [ALHeader("视频使用VideoPlayer播放器，和ios一样")]
        public bool useVideoPlayerPlay = false;
        
        /** 渲染的阴影图片 */
        private RenderTexture _m_tShadowRenderTexture;

        private Camera _m_cMainCamera;
        private float _m_fPreCameraOrSize;
        private float _m_fPreCameraView;

        private ALStepCounter stepCounter = new ALStepCounter();

        public RenderTexture shadowRenderTexture { get { return _m_tShadowRenderTexture; } }
        //获取当前平台名称
        public string curPlatName { get { if(platType == EWCGPlatType.EXTRA_PLAT) return extraPlatName; else return platType.ToString(); } }

        public Action<bool> onApplicationFocus;//当程序获得或者是去焦点时

        public Action<bool> onApplicationPause;//当程序切后台时

        /**************
        * 获取充值功能开关
        **/
        public bool isEnableRechargeFunction { get { return false; } }
        /********************
         * 获取当前的资源对应整形
         **/
        public override int curVersionNum { get { return ClientVersionSetting.instance.ClientVersionInfo.clientVersion; } }
        /********************
         * 获取当前的服务器对应整形
         **/
        public override int curServerVersionNum
        {
            get
            {
                return GameResCore.instance.serverVersionNum;
            }
        }

        /**************
         * 判断是否需要强更的处理
         **/
        public override void onClickVersionLowerErr()
        {
            //string newClientVersion = WCGPHPConfigMgr.instance.phpUrlForLoginConfig.newClientVersion;  //0_16_0_5 需要解析
            //string newClientVersionNum = null == newClientVersion ? string.Empty : newClientVersion.Replace('_', '.');
            //string updateURL = WCGPHPConfigMgr.instance.phpUrlForLoginConfig.newClientUpdateUrl;

            ////弹出跳转提示
            //NPGUIQueueMgr.instance.AddNode(
            //    new NPGUIAddQueueWndMes_TwoBtnNode(TextTranslate.instance.getLanguage("#*promptupdateclientversiontip", newClientVersionNum)
            //            , TextTranslate.instance.getLanguage("#*promptupdatecancel")
            //            , null
            //             , TextTranslate.instance.getLanguage("#*promptupdateok")
            //             , () =>
            //             {
            //                     //Debug.Log("chuck-----php-点击OK，跳转最新客户端版本页面");
            //                     _OpenNewClientVersionUrl(updateURL);
            //             }, null));
        }
        /// <summary>
        /// 当服务器版本过低的时候的触发效果
        /// </summary>
        public override void onClickServerVersionLowerErr()
        {

        }

        /// <summary>
        /// 当Hotfix版本过低的时候的触发效果
        /// </summary>
        public override void onClickHotfixVersionLowerErr()
        {

        }

        /****************
         * 当发生了未知错误时
         **/
        public override void onUnknowErrorOccurred(Exception _e)
        {

        }

        /********************
         * 当执行窗口大小更改时
         **/
        public override void onClientScreenOnSize(int _newWidth, int _newHeight)
        {
            //设置屏幕自适应
            _adjustScreen();
        }

        /********************
         * 在ALGUI响应了鼠标操作时触发的事件，一般不做处理
         **/
        //protected override void _onALGUICatchMouse() { }
        /********************
         * 子类用于在GUI未处理鼠标操作时进行的鼠标操作处理函数，一般不做处理
         **/
        //protected override void _dealMouseActionWhenALGUIDidNotCatchMouse() { }
        //是否使用刘海
        public override bool forceLiuHai
        {
            get
            {
                return base.forceLiuHai;
            }
        }

        // Use this for initialization
        protected internal void Awake()
        {
            //设置平台
#if UNITY_ANDROID
        _m_eClientPlat = EWCGClientPlat.ANDROID;
#elif UNITY_IOS || UNITY_IPHONE
        _m_eClientPlat = EWCGClientPlat.IOS;
#elif UNITY_STANDALONE
            _m_eClientPlat = EWCGClientPlat.PC;
#endif

#if UNITY_STANDALONE && !UNITY_EDITOR
            //非editor环境则设置分辨率
            Screen.SetResolution(450, 800, false);
#endif
#if UNITY_STANDALONE
            setIsPcPlat(true);
#endif
#if UNITY_EDITOR
            setIsPcPlat(true);
#endif
            //初始化埋点开始时间
            GCommon.initStepReportStartTime();

            _m_cMainCamera = GetComponent<Camera>();
            if (rtMainCamera != null) 
                GameObject.DontDestroyOnLoad(rtMainCamera);
            
            //初始化NPGame摄像头对象
            Game.instance.mainCamera = this;
            if (directionLight != null) 
                GameObject.DontDestroyOnLoad(directionLight);
            if (showcaseDirectionLight != null) 
                GameObject.DontDestroyOnLoad(showcaseDirectionLight);

        }

        private bool _m_bTempMainCameraEnable = true;
        private bool _m_bTempRTMainCameraEnable = false;
        
        /// <summary>
        /// 管线渲染之前
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cameras"></param>
        void OnBeginFrameRendering(ScriptableRenderContext context, Camera[] cameras)
        {
            // if (MJUniversalRenderAPI.uiBlurState == UIBlurState.OpenBlur)
            // {
            //     _m_bTempMainCameraEnable = CameraController.instance.cameraIsOpenRender;
            //     CameraController.instance.cameraIsOpenRender = false;
            //     _m_bTempRTMainCameraEnable = RTMainCameraController.instance.cameraIsOpenRender;
            //     RTMainCameraController.instance.cameraIsOpenRender = false;
            // }
        }
        
        /// <summary>
        /// 管线渲染之后
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cameras"></param>
        void OnEndFrameRendering(ScriptableRenderContext context, Camera[] cameras)
        {
            // if (MJUniversalRenderAPI.uiBlurState == UIBlurState.OpenBlur)
            // {
            //     CameraController.instance.cameraIsOpenRender = _m_bTempMainCameraEnable;
            //     RTMainCameraController.instance.cameraIsOpenRender = _m_bTempRTMainCameraEnable;
            // }
        }
        

        /********************
         * 在脚本开始运行的时候调用的函数
         **/
        protected override void _onStart()
        {
            //初始化视频播放管理对象
#if AL_AVPRO_V2
            // bool isEmulator = EmulatorCheck.IsProbablyEmulator();
            // RenderHeads.Media.AVProVideo.Android.VideoApi androidVideoApi = isEmulator
            //     ? RenderHeads.Media.AVProVideo.Android.VideoApi.MediaPlayer   // 模拟器：更稳
            //     : RenderHeads.Media.AVProVideo.Android.VideoApi.ExoPlayer;    // 真机：功能更全

            RenderHeads.Media.AVProVideo.Android.VideoApi androidVideoApi = RenderHeads.Media.AVProVideo.Android.VideoApi.ExoPlayer;    // 真机：功能更全
#endif
            ALVideoPlayerMgr.instance.init(
#if AL_AVPRO_V2
                androidVideoApi
#endif
                );

            // 初始化全部相机的管理对象
            URPCameraManager.instance.init(_m_cMainCamera, uiCamera, rtMainCamera, mainCameraData, uiCameraData , rtMainCameraData);
            //初始化摄像头控制对象
            CameraController.instance.setControlCamera(_m_cMainCamera);
            RTMainCameraController.instance.setControlCamera(rtMainCamera);
            RTMainCameraController.instance.setUniversalAdditionalCameraData(rtMainCameraData);
            
            // 增加事件，用于处理根据UI状态关闭相机的操作
            RenderPipelineManager.beginFrameRendering += OnBeginFrameRendering;
            RenderPipelineManager.endFrameRendering += OnEndFrameRendering;
            //发送埋点-开始onStart
            GCommon.sendStepReport(TraceConst.ON_START);

            //默认关闭全输入遮罩
            forceCloseAllInputMask();

            QualityMgr.instance.init();
            MemoryMgr.instance.init();

            //判断UI并处理切换
            _judgeAndDealScreenScale();

            //设置屏幕自适应
            _adjustScreen();
            
            //设置切换Scene不被删除的对象

            //初始化相关设置信息
            Game.instance.Init();

            // Disable screen dimming  
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            //初始化配置相关
            _initSetting();
            
            //这边先执行一次本地补丁加载，保证后面的代码可以热更到
            InjectFixMgr.instance.LoadLocalSavedPatch();

            //开始记录游戏运行时间
            ALMonoTaskMgr.instance.addMonoTask(new RecordGameRunningTimeMonoTask());

            //执行初始化函数
            GameInit.instance.init();

            //初始化操作控制对象的屏蔽3D点击的UI管理器
            InputListener.instance.initIgnoreClick3DUI(uiEventSystem);
        }

        //屏幕宽高比
        private float _m_fScreenRate;
        private static float _g_fMaxScreenRate = 1920f / 1080;
        private void _judgeAndDealScreenScale()
        {
            _m_fScreenRate = (float)Screen.width / Screen.height;

            if(_m_fScreenRate - _g_fMaxScreenRate > 0.1f)
            {
                //超出极限值，此时需要对屏幕比例进行修正
                if(null != fullCanvasScaler)
                    fullCanvasScaler.referenceResolution = new Vector2(1080 * _m_fScreenRate, 1080);
            }
        }

        /// <summary>
        /// 根据初始化配置处理
        /// </summary>
        private void _initSetting()
        {
            //debug信息处理
            if(gameSetting.showDebug && gameSetting.debug != null)
                GameObject.Instantiate(gameSetting.debug);
        }

        /********************
         * 子类用于处理GUI判断前的操作处理
         **/
        protected override void _onPreGuiUpdate()
        {

        }

        /********************
         * 子类用于处理正常帧处理操作
         **/
        protected override void _onUpdate()
        {
            /** 同步跟随摄像头 */
            _syncFollowCamera();

            //刷新渲染相关的信息
            GraphicMgr.instance.update();
            
            InputListener.instance.Update();

            //处理按键信息
            Game.instance.dealBtn();

            if(gameSetting.printNodeMessage)
                Debug.Log($"【Node】【{Time.frameCount}】NodeList : {QueueMgr.instance.ToString()}");
        }

        public void LateUpdate()
        {
            //先处理摄像头控制对象的操作
            CameraController.instance.frameCheck();
            // //刷新阴影相机位置
            // PlanarShadowMgr.instance.laterUpdateCameraPos(NPCameraController.instance.controlCamera);
        }

        /****************
         * 当程序退出时执行的相关函数
         **/
        public void OnApplicationQuit()
        {
            //设置本地推送
            if (Game.instance.isInited)
            {
                LocalPushMgr.instance.clearLocalPush();
                LocalPushMgr.instance.setLocalPush();
            }
            
            //需要退出相关连接对象
            LSMgr.instance.resetLoginState();
            if(NPGSClientListener.instance != null)
                NPGSClientListener.instance.logout();

            //退出聊天
            if (null != NPPlayer.instance)
                NPPlayer.instance.discard();

            NPGGUILanguageSqlite.instance.clear();
            NPPGUILanguageSqlite.instance.clear();
            ScreenBlurMgr.instance.discard();
            //释放音频
            PlayAudioMgr.instance.discard();
            
            VideoResCore.instance.discard();

            GameLanguageMgr.instance.discard();
            
            //断开数据库链接
            GGameSqliteMgr.instance.discard();
            
            //退出自定义线程
            ALThreadMgr.instance.clearAllThread();
            
            //释放Ilruntime
            Game.instance.discardRuntimeMgr();
        }

        /**************
         * 进程停止时
         **/
        void OnApplicationPause(bool paused)
        {
            //发送消息
            if(paused)
                WinMsg.SendMsg(WinMsgType.APPLICATION_PAUSE);
            else
                WinMsg.SendMsg(WinMsgType.APPLICATION_PAUSE_RESUME);

            if(paused)
            {
                //游戏切到后台
                //设置本地推送
                if (Game.instance.isInited)
                {
                    LocalPushMgr.instance.clearLocalPush();
                    LocalPushMgr.instance.setLocalPush();
                }

                //发送埋点-游戏切到后台
                GCommon.sendStepReport(TraceConst.GAME_PAUSE);
            }
            else
            {
                //后台回到游戏
                //清空推送
                if (Game.instance.isInited)
                    LocalPushMgr.instance.clearLocalPush();

                Game.instance.setDesignContentScale();

                //发送埋点-后台切回游戏
                GCommon.sendStepReport(TraceConst.GAME_RESUME);
            }

            if(onApplicationPause != null)
                onApplicationPause(paused);
        }

        /**************
         * 当程序获得或者是去焦点时
         **/
        void OnApplicationFocus(bool focus)
        {
            if(onApplicationFocus != null)
                onApplicationFocus(focus);
        }


        /*****************
         * 获取当前客户端选择登录的平台信息
         **/
        public WCGPlatLoginInfo platInfo
        {
            get
            {
                if(platType == EWCGPlatType.EXTRA_PLAT)
                    return extraPlatInfo;

#if UNITY_EDITOR
                //自定义配置，只在Editor下生效
                if(GGameCustomInfo.instance.isEnable)
                {        
                    for(int i = 0; i < GGameCustomInfo.instance.obj.platInfoList.Count; i++)
                    {
                        WCGPlatLoginInfo info = GGameCustomInfo.instance.obj.platInfoList[i];
                        if(null == info)
                            continue;
                
                        if(info.platType == platType)
                            return info;
                    }
                }
#endif 
                
                for(int i = 0; i < platInfoList.Count; i++)
                {
                    WCGPlatLoginInfo info = platInfoList[i];
                    if(null == info)
                        continue;

                    if(info.platType == platType)
                        return info;
                }

                return null;
            }
        }

        /****************
         * 设置音频接受对象的相对位置
         **/
        public void setAudioListenerPos(Vector3 _pos)
        {
            if(null == audioListenerGo || null == audioListenerGo.transform)
                return;

            audioListenerGo.transform.position = transform.position + _pos;
        }
        public void setAudioListenerPos(float _zDis)
        {
            if(null == audioListenerGo || null == audioListenerGo.transform)
                return;

            audioListenerGo.transform.localPosition = Vector3.forward * _zDis;
        }
        public void resetAudioListenerPos()
        {
            if(null == audioListenerGo || null == audioListenerGo.transform)
                return;

            audioListenerGo.transform.localPosition = Vector3.zero;
        }

        #region 总输入遮罩控制
        // 总输入遮罩的计数，要求 open 和 close 一一对应才行，好处是支持了多套逻辑同时需要屏蔽操作
        private int _m_iInputMaskCounter = 0;
        // 当前 open close 方法的有效序列号，forceClose 会使序列号自增，废弃旧的 close 将不再影响新的 open
        private int _m_iInputMaskSerialize = 0;

        /// <summary>
        /// 是否正在屏蔽玩家当前的所有输入
        /// </summary>
        public bool isOpenAllInputMask { get { return _m_iInputMaskCounter > 0; } }
        /// <summary>
        /// 屏蔽玩家当前的所有输入
        /// </summary>
        public int openAllInputMask()
        {
            // 增加计数
            _m_iInputMaskCounter++;
            // 计数为 0 时才通过
            if (_m_iInputMaskCounter > 1)
                return _m_iInputMaskSerialize;
            
            // 屏蔽所有输入
            ALUGUICommon.setGameObjEnable(allInputMask);
            QueueMgr.instance.CloseRollBack(NodeESC_Const.C_QUEUE_ESC_ALL_INPUT_MASK);
            ALInputControl.instance.CloseInput(NodeESC_Const.C_QUEUE_ESC_ALL_INPUT_MASK);
            
            // 返回当前的序列号
            return _m_iInputMaskSerialize;
        }
        /// <summary>
        /// 恢复玩家的正常输入
        /// </summary>
        /// <param name="_serialize">需要带入 open 给到的序列号</param>
        public void closeAllInputMask(int _serialize)
        {
            // 如果序列号不同，说明当前屏蔽系统已经被 forceClose 过一次，旧序列号的操作不再被响应
            if (_m_iInputMaskSerialize != _serialize)
                return;
            
            // 减少计数
            _m_iInputMaskCounter--;
            // 计数小于等于 0 才进行处理
            if (_m_iInputMaskCounter > 0)
                return;

            // 强行赋值 0 ，防止奇怪的情况出现
            _m_iInputMaskCounter = 0;
            
            // 恢复所有玩家的输入权限
            ALUGUICommon.setGameObjDisable(allInputMask);
            QueueMgr.instance.OpenRollBack(NodeESC_Const.C_QUEUE_ESC_ALL_INPUT_MASK);
            ALInputControl.instance.OpenInput(NodeESC_Const.C_QUEUE_ESC_ALL_INPUT_MASK);
        }
        public void forceCloseAllInputMask()
        {
            // 自增序列号，让旧序列号的 open 和 close 不再响应
            _m_iInputMaskSerialize = ALSerializeOpMgr.next();
            // 强行赋值 0 ，防止奇怪的情况出现
            _m_iInputMaskCounter = 0;
            
            // 恢复所有玩家的输入权限
            ALUGUICommon.setGameObjDisable(allInputMask);
            QueueMgr.instance.OpenRollBack(NodeESC_Const.C_QUEUE_ESC_ALL_INPUT_MASK);
            ALInputControl.instance.OpenInput(NodeESC_Const.C_QUEUE_ESC_ALL_INPUT_MASK);
        }
        #endregion

        /** 同步跟随的摄像头的视角信息 */
        protected void _syncFollowCamera()
        {
            if(null != followCamera && null != _m_cMainCamera)
            {
                //尝试同步参数
                if(!Mathf.Approximately(_m_fPreCameraView, _m_cMainCamera.fieldOfView))
                {
                    _m_fPreCameraView = _m_cMainCamera.fieldOfView;
                    Camera cam = null;
                    for(int i = 0; i < followCamera.Count; i++)
                    {
                        cam = followCamera[i];
                        if(cam == null)
                            continue;
                        cam.fieldOfView = _m_fPreCameraView;
                    }
                }

                //尝试同步参数
                if(!Mathf.Approximately(_m_fPreCameraOrSize, _m_cMainCamera.orthographicSize))
                {
                    _m_fPreCameraOrSize = _m_cMainCamera.orthographicSize;
                    Camera cam = null;
                    for(int i = 0; i < followCamera.Count; i++)
                    {
                        cam = followCamera[i];
                        if(cam == null)
                            continue;
                        cam.orthographicSize = _m_fPreCameraOrSize;
                    }
                }
            }
        }


        /**************
         * 检查客户端与服务端资源版本是否一致
         **/
        private bool _checkClientResVersion()
        {
            //如果ClientVersionSetting为空说明整个工程不太对
            WCGClientInfo clientVersionInfo = ClientVersionSetting.instance.ClientVersionInfo;
            
            //进行版本判断操作
            if(ClientSCVersion.main != ResSCVersion.main
            || ClientSCVersion.sub != ResSCVersion.sub
            || ClientSCVersion.patch != ResSCVersion.patch
            || ClientSCVersion.build != ResSCVersion.build
            || ClientSCVersion.date != ResSCVersion.date
            || ResSCVersion.main != clientVersionInfo.majorVersion
            || ResSCVersion.sub != clientVersionInfo.minorVersion
            || ResSCVersion.patch != clientVersionInfo.revisionVersion
            || ResSCVersion.build != clientVersionInfo._buildVersion
            || ResSCVersion.date != clientVersionInfo._dateVersion
            )
            {
                //弹出提示
                NPMesMgr.instance.showOneBtnMes(
                    "客户端打包代码版本不一致！"
                    , TextTranslate.instance.getLanguage(TransKeyConst.confirm), null);
            }

            gameSetting.printProtocol = false;

            return true;
        }

        protected void _onPlatInitFail()
        {
            //初始化失败，则调用本函数
            Debug.LogError("Plat Init Fail!");
        }
        
        
        //设置屏幕自适应
        private void _adjustScreen()
        {
#if UNITY_IPHONE
            switch (UnityEngine.iOS.Device.generation)
            {
                case UnityEngine.iOS.DeviceGeneration.iPhoneX:
                case UnityEngine.iOS.DeviceGeneration.iPhoneXS:
                case UnityEngine.iOS.DeviceGeneration.iPhoneXSMax:
                case UnityEngine.iOS.DeviceGeneration.iPhoneXR:
                    float minWidth = 375f;
                    float minHeight = 812f;
                    float leftMarginPixel = 0;
                    float rightMarginPixel = 0;
                    float botMarginPixel = 21f;
                    float topMarginPixel = 30f;
                    _adjustScreen((minWidth - leftMarginPixel - rightMarginPixel) / (minHeight - botMarginPixel - topMarginPixel), 1080f / 1920f, 
                        leftMarginPixel / minWidth, rightMarginPixel / minWidth, botMarginPixel / minHeight, topMarginPixel / minHeight, 0, 0);
                    break;
                case UnityEngine.iOS.DeviceGeneration.iPhone14:
                case UnityEngine.iOS.DeviceGeneration.iPhone14Plus:
                case UnityEngine.iOS.DeviceGeneration.iPhone14Pro:
                case UnityEngine.iOS.DeviceGeneration.iPhone14ProMax:
                    minWidth = 375f;
                    minHeight = 812f;
                    leftMarginPixel = 0;
                    rightMarginPixel = 0;
                    botMarginPixel = 21f;
                    topMarginPixel = 48f;
                    _adjustScreen((minWidth - leftMarginPixel - rightMarginPixel) / (minHeight - botMarginPixel - topMarginPixel), 1080f / 1920f, 
                        leftMarginPixel / minWidth, rightMarginPixel / minWidth, botMarginPixel / minHeight, topMarginPixel / minHeight, 0, 0);
                    break;
                default://没有手动配置大小的, 统一走安全区域
                    _adjustScreenBySafeArea();
                    // Debug.Log($"Screen.safeArea:{Screen.safeArea} Screen.width:{Screen.width} Screen.height:{Screen.height}");
                    // _adjustScreen(1080f / 2200f, 1080f / 1920f, 
                    //     0, 0, 0, 0, 0, 0);
                    break;
            }
#elif UNITY_EDITOR
            {
                _adjustScreen(1080f / 2300f, 1080f / 1920f,
                    inEditorScreenLeftMarginPercent, inEditorScreenRightMarginPercent, inEditorScreenBotMarginPercent, inEditorScreenTopMarginPercent, 0, 0);
            }
#else
            {
                //安卓直接按照安全区域来适配
                _adjustScreenBySafeArea();
            }
#endif
        }

        //按照安全区域来适配
        private void _adjustScreenBySafeArea()
        {
            Rect safeArea = Screen.safeArea;//获取安全区域
            float minWidth = Screen.width;
            float minHeight = Screen.height;
            float leftMarginPixel = safeArea.xMin;
            float rightMarginPixel = minWidth - safeArea.width - leftMarginPixel;
            float botMarginPixel = safeArea.yMin;
            float topMarginPixel = minHeight - safeArea.height - botMarginPixel;
            _adjustScreen(1080f / 2300f, 1080f / 1920f,
                leftMarginPixel / minWidth, rightMarginPixel / minWidth, botMarginPixel / minHeight, topMarginPixel / minHeight, 0, 0);
        }


        /// <summary>
        ///  通用的计算屏幕适配的方法，不同的适配方案，传递不同参数
        ///  目前先以UGUI宽度为定值来计算其他的值
        /// 
        ///  程序定义了支持的最小宽高比（最瘦长），最大宽高比（最宽，就是pad那种）
        ///  四边分别有4个margin，margin大小定义为物理屏幕宽/高的百分比
        ///  屏幕物理像素宽高是 Screen.width / Screen.height
        ///  去掉margin后，宽高是：fixedScreenWidth / fixedScreenHeight
        /// 
        ///  现在要算出屏幕的显示区域，叫做showWidth / showHeight，算法是：
        ///  在去掉margin后，有3种情况：
        ///  一种是fixedScreen太长了，那就把宽度撑满，高度尽量拉伸到最小宽高比，就得到显示区域
        ///  一种是fixedScreen太宽了，那就把高度撑满，宽度尽量拉伸到最大宽高比，就得到显示区域
        ///  还有一种是fixedScreen在能支持的宽高比内，那就宽高都撑满，就得到显示区域
        /// 
        ///  得到屏幕的显示区域之后，就把Canvas设置到这个区域，Canvas的宽度设置为1080、高度按照显示区域的宽高比计算得到。
        ///  之后再计算3D摄像机的适配，保持宽度看到的范围和设计时候一致
        /// </summary>
        /// <param name="_minRatio">最小宽高比（最瘦长）</param>
        /// <param name="_maxRatio">最大宽高比（最宽——pad）</param>
        /// <param name="_screenLeftMarginP">屏幕左边margin百分比</param>
        /// <param name="_screenRightMarginP">屏幕右边margin百分比</param>
        /// <param name="_screenBotMarginP">屏幕下面margin百分比</param>
        /// <param name="_screenTopMarginP">屏幕上面margin百分比</param>
        /// <param name="_botTopPercent">上下如何对齐，-1是完全靠下对齐（上面留黑），1是完全靠上对齐（下面留黑）</param>
        /// <param name="_leftRightPercent">左右如何对齐，-1是完全靠左对齐（右边留黑），1是完全靠右对齐（左边留黑）</param>
        /// <param name="_showUGUIWidth"></param>
        private void _adjustScreen(float _minRatio, float _maxRatio, float _screenLeftMarginP, float _screenRightMarginP, float _screenBotMarginP, float _screenTopMarginP, 
            float _botTopPercent, float _leftRightPercent, float _showUGUIWidth = 1080f)
        {
            if(fullCanvasScaler == null)
            {
                return;
            }

            if (null == maskTop || null == maskBot || null == maskLeft || null == maskRight)
            {
                return;
            }
            
            if(CameraController.instance.controlCamera == null)
            {
                Debug.LogError("controlCamera == null");
                return;
            }
            
            //处理参数
            if(_minRatio > _maxRatio)
            {
                float temp = _minRatio;
                _minRatio = _maxRatio;
                _maxRatio = temp;
            }
            _botTopPercent = Mathf.Clamp(_botTopPercent, -1, 1);
            _leftRightPercent = Mathf.Clamp(_leftRightPercent, -1, 1);

            //除掉Margin后的屏幕区域宽和高（主要是有些设备需要去掉上面的刘海区域，还有下面的虚拟按键区域）
            int fixedScreenWidth = (int) ((1 - _screenLeftMarginP - _screenRightMarginP) * Screen.width);
            int fixedScreenHeight = (int) ((1 - _screenBotMarginP - _screenTopMarginP) * Screen.height);
            //算出修正后屏幕区域的宽高比
            float fixedScreenRatio = (float)fixedScreenWidth / fixedScreenHeight;

            float fixedScreenWidth_UGUI;//***除掉Margin后的屏幕UGUI宽度，需要算出来的
            float fixedScreenHeight_UGUI;//***除掉Margin后的屏幕UGUI高度，需要算出来的

            float showWidth_UGUI = _showUGUIWidth;//显示区域的UGUI像素宽度，策划定死用1080来适配的
            float showHeight_UGUI;//***显示区域的UGUI像素高度，需要算出来的

            float gapWidth_UGUI;//***show区域到screen区域，宽度总的差多少，需要算出来的
            float gapHeight_UGUI;//***show区域到screen区域，高度总的差多少，需要算出来的

            //如果修正后的屏幕比能支持的最长的还长（宽度顶满，高度按照_minRatio算出来），显示区域宽高比使用_minRatio
            if(fixedScreenRatio < _minRatio)
            {
                //match width 让宽度等于1080
                fullCanvasScaler.matchWidthOrHeight = 0;

                //计算显示区域的UGUI像素高度
                showHeight_UGUI = showWidth_UGUI / _minRatio;
            
                //计算屏幕UGUI宽度和高度
                fixedScreenWidth_UGUI = showWidth_UGUI;
                fixedScreenHeight_UGUI = showWidth_UGUI / fixedScreenRatio;
            }
            else if(fixedScreenRatio < _maxRatio) //如果修正后的屏幕宽高比在能支持的范围
            {
                //match height 让宽度等于1080
                fullCanvasScaler.matchWidthOrHeight = 0;

                //计算显示区域的UGUI像素高度
                showHeight_UGUI = showWidth_UGUI / fixedScreenRatio;
            
                //计算屏幕UGUI宽度和高度
                fixedScreenWidth_UGUI = showWidth_UGUI;
                fixedScreenHeight_UGUI = showHeight_UGUI;
            }
            else//如果修正后的屏幕比能支持的最矮的还矮（高度顶满，宽度按照_maxRatio算出来），显示区域宽高比使用_maxRatio
            {
                //match height 让宽度等于1080
                fullCanvasScaler.matchWidthOrHeight = 1;

                //计算显示区域的UGUI像素高度
                showHeight_UGUI = showWidth_UGUI / _maxRatio;
            
                //计算屏幕UGUI宽度和高度
                fixedScreenWidth_UGUI = fixedScreenRatio * showHeight_UGUI;
                fixedScreenHeight_UGUI = showHeight_UGUI;
            }
        
            //计算宽高总差距
            gapWidth_UGUI = fixedScreenWidth_UGUI - showWidth_UGUI;
            gapHeight_UGUI = fixedScreenHeight_UGUI - showHeight_UGUI;
        
            float screenWidth_UGUI = fixedScreenWidth_UGUI / (1 - _screenLeftMarginP - _screenRightMarginP); //***屏幕UGUI宽度，需要算出来的
            float screenHeight_UGUI = fixedScreenHeight_UGUI / (1 - _screenBotMarginP - _screenTopMarginP); //***屏幕UGUI高度，需要算出来的
        
            fullCanvasScaler.referenceResolution = new Vector2(screenWidth_UGUI, screenHeight_UGUI);

            float screenLeftMarginUGUI = screenWidth_UGUI * _screenLeftMarginP;
            float screenRightMarginUGUI = screenWidth_UGUI * _screenRightMarginP;
            float screenBotMarginUGUI = screenHeight_UGUI * _screenBotMarginP;
            float screenTopMarginUGUI = screenHeight_UGUI * _screenTopMarginP;

            float halfGapUGUIWidth = gapWidth_UGUI / 2;
            float halfGapUGUIHeight = gapHeight_UGUI / 2;

            // Debug.Log($"fixedScreenWidth:{fixedScreenWidth}");
            // Debug.Log($"fixedScreenHeight:{fixedScreenHeight}");
            // Debug.Log($"fixedScreenRatio:{fixedScreenRatio}");
            //
            // Debug.Log($"fixedScreenWidth_UGUI:{fixedScreenWidth_UGUI}");
            // Debug.Log($"fixedScreenHeight_UGUI:{fixedScreenHeight_UGUI}");
            // Debug.Log($"showWidth_UGUI:{showWidth_UGUI}");
            // Debug.Log($"showHeight_UGUI:{showHeight_UGUI}");
            // Debug.Log($"gapWidth_UGUI:{gapWidth_UGUI}");
            // Debug.Log($"gapHeight_UGUI:{gapHeight_UGUI}");
            //
            // Debug.Log($"screenWidth_UGUI:{screenWidth_UGUI}");
            // Debug.Log($"screenHeight_UGUI:{screenHeight_UGUI}");
            //
            // Debug.Log($"screenLeftMarginUGUI:{screenLeftMarginUGUI}");
            // Debug.Log($"screenRightMarginUGUI:{screenRightMarginUGUI}");
            // Debug.Log($"screenBotMarginUGUI:{screenBotMarginUGUI}");
            // Debug.Log($"screenTopMarginUGUI:{screenTopMarginUGUI}");

            
            //调整全部canvas的大小，保证视图正常
            //2022-08-16 alzq 这里不需要调整，使用rect保证显示区域即可
            //for (var i = 0; i < canvasList.Count; i++)
            //{
            //    Canvas canvas = canvasList[i];
            //    if(canvas != null)
            //    {
            //        RectTransform canvasTransform = canvas.transform as RectTransform;
            //        if(canvasTransform != null)
            //        {
            //            canvasTransform.anchoredPosition =
            //                new Vector2(_leftRightPercent * halfGapUGUIWidth + (screenLeftMarginUGUI - screenRightMarginUGUI) / 2,
            //                    _botTopPercent * halfGapUGUIHeight + (screenBotMarginUGUI - screenTopMarginUGUI) / 2);
            //            canvasTransform.sizeDelta = new Vector2(
            //                -gapWidth_UGUI - screenLeftMarginUGUI - screenRightMarginUGUI,
            //                -gapHeight_UGUI - screenBotMarginUGUI - screenTopMarginUGUI);
            //        }
            //    }
            //    else
            //    {
            //        Debug.LogError("Canvas is null");
            //    }
            //}

            //调整4个mask大小位置，保证遮挡空白区域
            //2022-08-16 alzq 强制关闭mask，通过uicamera的rect进行显示区域控制即可
            if (maskTop != null)
                ALUGUICommon.setGameObjDisable(maskTop);
            if (maskBot != null)
                ALUGUICommon.setGameObjDisable(maskBot);
            if (maskLeft != null)
                ALUGUICommon.setGameObjDisable(maskLeft);
            if (maskRight != null)
                ALUGUICommon.setGameObjDisable(maskRight);
            
            //if(maskTop != null)
            //{
            //    (maskTop.transform as RectTransform).gameObject.SetActive(_botTopPercent < 1 || _screenTopMarginP > 0); //只要不是全部靠上，或者上面有margin，就要显示mask
            //    (maskTop.transform as RectTransform).sizeDelta = new Vector2(0, screenTopMarginUGUI + _botTopPercent.Remap(-1, 1, 1, 0) * gapHeight_UGUI);
            //}
            //if(maskBot != null)
            //{
            //    (maskBot.transform as RectTransform).gameObject.SetActive(_botTopPercent > -1 || _screenBotMarginP > 0); //只要不是全部靠下，或者下面有margin，就要显示mask
            //    (maskBot.transform as RectTransform).sizeDelta = new Vector2(0,  screenBotMarginUGUI + _botTopPercent.Remap(-1, 1, 0, 1) * gapHeight_UGUI);
            //}
            //if(maskLeft != null)
            //{
            //    (maskLeft.transform as RectTransform).gameObject.SetActive(_leftRightPercent > -1 || _screenLeftMarginP > 0); //只要不是全部靠左，或者左边有margin，就要显示mask
            //    (maskLeft.transform as RectTransform).sizeDelta = new Vector2(screenLeftMarginUGUI + _leftRightPercent.Remap(-1, 1, 0, 1) * gapWidth_UGUI, 0);
            //}
            //if(maskRight != null)
            //{
            //    (maskRight.transform as RectTransform).gameObject.SetActive(_leftRightPercent < 1 || _screenRightMarginP > 0); //只要不是全部靠右，或者右边有margin，就要显示mask
            //    (maskRight.transform as RectTransform).sizeDelta = new Vector2(screenRightMarginUGUI + _leftRightPercent.Remap(-1, 1, 1, 0) * gapWidth_UGUI, 0);
            //}

            //设置场景摄像机视野范围
            float viewRectWidthPercent = showWidth_UGUI / screenWidth_UGUI;
            float viewRectHeightPercent = showHeight_UGUI / screenHeight_UGUI;
            // Rect cameraRect = new Rect(
            //     _screenLeftMarginP + _leftRightPercent.Remap(-1, 1, 0, 1) * (1 - _screenLeftMarginP - _screenRightMarginP - viewRectWidthPercent),
            //     _screenBotMarginP + _botTopPercent.Remap(-1, 1, 0, 1) * (1 - _screenBotMarginP - _screenTopMarginP - viewRectHeightPercent),
            //     viewRectWidthPercent,
            //     viewRectHeightPercent);
            Rect cameraRect = new Rect(
                _screenLeftMarginP + _leftRightPercent.Remap(-1, 1, 0, 1) * (1 - _screenLeftMarginP - _screenRightMarginP - viewRectWidthPercent),
                0,//https://www.teambition.com/task/64843b8578bb2e1ee5c9f56f 只裁剪宽度(避免平板出问题), 高度不裁剪
                viewRectWidthPercent,
                1);//https://www.teambition.com/task/64843b8578bb2e1ee5c9f56f 只裁剪宽度(避免平板出问题), 高度不裁剪
            
            //统一设置摄像头Rect
            Game.instance.setGameCameraRect(cameraRect);

            // https://www.teambition.com/task/64843b8578bb2e1ee5c9f56f新增需要进行缩放的Canvas
			if(adjustScreenCanvas != null)
			{
				for (var i = 0; i < adjustScreenCanvas.Count; i++)
	            {
	                Canvas canvas = adjustScreenCanvas[i];
	                if(canvas != null)
	                {
	                    RectTransform canvasTransform = canvas.transform as RectTransform;
	                    if(canvasTransform != null)
	                    {
	                        canvasTransform.anchoredPosition = new Vector2(_leftRightPercent * halfGapUGUIWidth + (screenLeftMarginUGUI - screenRightMarginUGUI) / 2, _botTopPercent * halfGapUGUIHeight + (screenBotMarginUGUI - screenTopMarginUGUI) / 2);
	                        canvasTransform.sizeDelta = new Vector2(0,
	                             - screenBotMarginUGUI - screenTopMarginUGUI);//因为宽度直接裁剪摄像机渲染范围, 所以宽度跟Canvas一样赋值0, 高度摄像机不裁剪, 单独移除bot和top Margin
	                    }
	                }
	                else
	                {
	                    Debug.LogError("Canvas is null");
	                }
	            }
			}
            
            _m_fAdjustScreenCanvasLeftMarginUGUI = 0;//自适应时Canvas的宽度没有变化
            _m_fAdjustScreenCanvasRightMarginUGUI = 0;//自适应时Canvas的宽度没有变化
            _m_fAdjustScreenCanvasBotMarginUGUI = screenBotMarginUGUI;//自适应的Canvas高度上移screenBotMarginUGUI
            _m_fAdjustScreenCanvasTopMarginUGUI = screenTopMarginUGUI;
            _m_fAdjustScreenCanvasWidthUGUI = showWidth_UGUI;
            _m_fAdjustScreenCanvasHeightUGUI = showHeight_UGUI;
            
            //打开宽度适配，调整orthographicSize，让屏幕显示的宽度，总是和设计时看到的宽度一致，而看到的高度则根据适配而不同，打开适配后，调整orthographicSize，都应该通过CameraController
            CameraController.instance.openNormalRateWidthFit(new Vector2(9, 16), true, true);
            
            //一并调整rt主摄像机的orthographicSize，保证showcase正常
            RTMainCameraController.instance.openNormalRateWidthFit(new Vector2(9, 16), true, true);
            
            //手动刷新一次，确保相机参数有更新到，有位置会根据视野范围有变化的功能
            RTMainCameraController.instance.frameCheck();
        }

        private float _m_fAdjustScreenCanvasLeftMarginUGUI = 0f;//自适应Canvas的距离FullCanvas左边UGUI宽度
        private float _m_fAdjustScreenCanvasRightMarginUGUI = 0f;//自适应Canvas的距离FullCanvas右边UGUI宽度
        private float _m_fAdjustScreenCanvasBotMarginUGUI = 0f;//自适应Canvas的距离FullCanvas下边UGUI宽度
        private float _m_fAdjustScreenCanvasTopMarginUGUI = 0f;//自适应Canvas的距离FullCanvas上边UGUI宽度
        private float _m_fAdjustScreenCanvasWidthUGUI = 0f;//自适应Canvas Rect UGUI宽度
        private float _m_fAdjustScreenCanvasHeightUGUI = 0f;//自适应Canvas Rect UGUI高度

        public float adjustScreenCanvasLeftMarginUGUI { get { return _m_fAdjustScreenCanvasLeftMarginUGUI; } }
        public float adjustScreenCanvasRightMarginUGUI { get { return _m_fAdjustScreenCanvasRightMarginUGUI; } }
        public float adjustScreenCanvasBotMarginUGUI { get { return _m_fAdjustScreenCanvasBotMarginUGUI; } }
        public float adjustScreenCanvasTopMarginUGUI { get { return _m_fAdjustScreenCanvasTopMarginUGUI; } }
        public float adjustScreenCanvasWidthUGUI { get { return _m_fAdjustScreenCanvasWidthUGUI; } }
        public float adjustScreenCanvasHeightUGUI { get { return _m_fAdjustScreenCanvasHeightUGUI; } }


		/// <summary>
        /// 判断对象是否是自适应屏幕Canvas下的对象
        /// </summary>
        public bool isAdjustScreenCanvasChildRect(Transform _transform)
        {
            if (adjustScreenCanvas == null || _transform == null)
                return false;

            foreach (var canvas in adjustScreenCanvas)
            {
                if (canvas != null && _transform.IsChildOf(canvas.transform))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 判断对象是否是自适应屏幕Canvas下的对象
        /// </summary>
        public bool isAdjustScreenCanvasChildRect(Transform _transform, out Canvas _canvas)
        {
            _canvas = null;
            if (adjustScreenCanvas == null || _transform == null)
                return false;

            foreach (var canvas in adjustScreenCanvas)
            {
                if (canvas != null && _transform.IsChildOf(canvas.transform))
                {
                    _canvas = canvas;
                    return true;
                }
            }

            return false;
        }
        
		/// <summary>
        /// 将2DUI的屏幕位置转化为做了屏幕自适应的Canvas的UI坐标
        /// </summary>
        public Vector2 swapUIVector_InAdjustScreenCanvas(Vector2 _screenPos)
        {
            if (null == uiRootRectTrans)
                return _screenPos;

            Rect rect = uiCamera.rect;

            return new Vector2(uiRootRectTrans.rect.width * ((_screenPos.x / Screen.width) - rect.x) / rect.width - _m_fAdjustScreenCanvasLeftMarginUGUI, uiRootRectTrans.rect.height * ((_screenPos.y / Screen.height) - rect.y) / rect.height - _m_fAdjustScreenCanvasBotMarginUGUI);
        }

        #region URP Method

        /// <summary>
        /// 把相机组合方式改为Base
        /// </summary>
        public void setToBaseCamera()
        {
            mainCameraData.cameraStack.Clear();
            uiCameraData.renderType = CameraRenderType.Base;
            rtMainCameraData.renderType = CameraRenderType.Base;
        }
        /// <summary>
        /// 把相机组合方式改为Overlay
        /// </summary>
        public void setToOverlayCamera()
        {
            uiCameraData.renderType = CameraRenderType.Overlay;
            rtMainCameraData.renderType = CameraRenderType.Overlay;
            mainCameraData.cameraStack.Clear();
            mainCameraData.cameraStack.Add(rtMainCamera);
            mainCameraData.cameraStack.Add(uiCamera);
        }

        #endregion
    }
}

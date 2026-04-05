using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/*****************
 * 基本的主摄像头脚本对象
 **/
namespace ALPackage
{
    /******************
     * UI层级对象的节点信息结构体
     **/
    [System.Serializable]
    public class ALUILayerInfo
    {
        /** 窗口层级 */
        public EALUIWndLayer layer;
        /** 对应的根节点对象 */
        public Transform layerRootGo;
        /** ugui的根节点对象 */
        public Canvas cavasObj;

        [ALHeader("是否每个子窗口对象都会创建一个Canvas用于分层")]
        public bool isAllChildCreateCanvas = false;
        [ALHeader("每个子窗口分层之间的间隔")]
        public int layerInterval = 0;
    }

    /***********
     * AL设置部分信息
     **/
    [System.Serializable]
    public class ALGameSettingInfo
    {
        public bool isDebugIphoneXWnd;//是否在调试iphoneX窗口
    }

    public abstract class _AALMonoMain : MonoBehaviour
    {
        private static _AALMonoMain _g_instance = null;
        public static _AALMonoMain instance { get { return _g_instance; } }

        private static long _g_lSerialize;
        public static long newObjSerialzie() { return _g_lSerialize++; }

        /** 是否在运行界面中展示调试信息（一般是打印关键信息，关键流程函数） */
        public bool showDebugOutput;

        /** 是否Pc平台 */
        public bool isPcPlat;
        /** 是否进行任务超时监控 */
        public bool isMonitorTaskTime;

        /** UI渲染摄像头 */
        public Camera uiCamera;
        /** ugui的根节点对象 */
        public Canvas fullCanvas;
        /** ugui可能的不同ui根节点对象 */
        [NotNull] public List<Canvas> canvasList;
        /** 事件系统管理对象 */
        public EventSystem uiEventSystem;
        /** ui控制相关对象 */
        [NotNull] public List<GraphicRaycaster> uiGraphicRaycasterList;
        /** ui中分层结构的相关信息存储队列 */
        [NotNull] public List<ALUILayerInfo> uiLayerInfoList;

        //设置信息结构体
        public ALGameSettingInfo alSetting;
        public virtual bool forceLiuHai { get { return alSetting.isDebugIphoneXWnd; } }

        /** 根节点UI的区域对象 */
        private RectTransform _m_rtUIRootRectTrans;

        /** 测试用列表，避免重复创建 */
        private List<RaycastResult> _m_lTestUIRaycastList = new List<RaycastResult>();

        /** ui控制相关对象 */
        private List<GraphicRaycaster> _m_lDynamicGraphicRaycaster = new List<GraphicRaycaster>();
        
        /** ui控制相关对象 */
        private PointerEventData _m_tmpPointEventData;

        public RectTransform uiRootRectTrans { get { return _m_rtUIRootRectTrans; } }

        public void regGraphic(GraphicRaycaster _raycaster)
        {
            if (null == _raycaster)
                return;

            _m_lDynamicGraphicRaycaster.Add(_raycaster);
        }
        public void unregGraphic(GraphicRaycaster _raycaster)
        {
            if (null == _raycaster)
                return;

            _m_lDynamicGraphicRaycaster.Remove(_raycaster);
        }

        // Use this for initialization
        protected internal void Start()
        {
            //输出版本号
            ALVersion.printVersion();

            try
            {
#if AL_UNITY_GUI
                //初始化基础UI
                ALGUIMain.instance.init();
#endif

                //初始化UI节点
                ALUILayerMgr.instance.initUIRootNodes(uiLayerInfoList);
                //先进行mono task初始化
                ALMonoTaskMgr.instance.addMonoTask(null);

                //设置处理对象
                if (null == _g_instance)
                {
                    _g_instance = this;
                }
                else
                {
                    UnityEngine.Debug.LogError("Multiple _AALMonoMain Mono!!!");
                    return;
                }
                
                //注册屏幕变动回调
                ALGUIMain.instance.screenSizeChgDelegate += onClientScreenOnSize;

                //注册输入对象的按键处理函数
                ALInputControl.instance.ALGUIRegKeyDownDelegate(_onKeyDown);

                //判断本对象是否有加载ALMonoTaskMono脚本，无则自动添加
                if (null == gameObject.GetComponent<ALMonoTaskMono>())
                    gameObject.AddComponent<ALMonoTaskMono>();
                //判断本对象是否有加载ALMonoCoroutineDealer脚本，无则自动添加
                if (null == gameObject.GetComponent<ALMonoCoroutineDealer>())
                    gameObject.AddComponent<ALMonoCoroutineDealer>();

                if (null != fullCanvas)
                {
                    _m_rtUIRootRectTrans = fullCanvas.GetComponent<RectTransform>();
                }

#if AL_UNITY_GUI
                if (showDebugOutput)
                {
                    ALDebugStage.instance.enterStage();
                    Application.logMessageReceived += (string condition, string stackTrace, LogType type) => { ALDebugWnd.appendInfo("ALLog - [ " + type.ToString() + " ] " + condition); };
                }
#endif

                //设置对象不被删除
                GameObject.DontDestroyOnLoad(this);
                //设置ui摄像头不被移除
                GameObject.DontDestroyOnLoad(uiCamera);
                GameObject.DontDestroyOnLoad(fullCanvas);
                GameObject.DontDestroyOnLoad(uiEventSystem);
                //设置所有canvas不被删除
                for(int i = 0; i < canvasList.Count; i++)
                {
                    if(null != canvasList[i] && canvasList[i].isRootCanvas)
                        GameObject.DontDestroyOnLoad(canvasList[i]);
                }
            
                if (isPcPlat)
                    ALInputControl.instance.regInputListener(new ALPCInputListener());
                else
                    ALInputControl.instance.regInputListener(new ALIOSInputListener());

                //调用事件函数
                _onStart();
                //初始化顶点事件系统数据
                _m_tmpPointEventData  = new PointerEventData(uiEventSystem);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError("MonoMain Start has Exception:\n" + e);
                onUnknowErrorOccurred(e);
            }
        }

        // Update is called once per frame
        protected internal void Update()
        {
            try
            {
                _onPreGuiUpdate();

#if AL_UNITY_GUI
                //调用事件处理函数
                if (!ALGUIMain.instance.ALGUIExec())
                {
                    //返回false表示GUI部分未拦截鼠标等事件
                    _dealMouseActionWhenALGUIDidNotCatchMouse();
                }
                else
                {
                    _onALGUICatchMouse();
                }
#else
                ALGUIMain.instance.ALGUIExec();
#endif

                _onUpdate();
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError("MonoMain Update has Exception:\n" + e);
                onUnknowErrorOccurred(e);
            }
        }

#if AL_UNITY_GUI
        protected internal void OnGUI()
        {
            //处理输入控制器的处理
            ALInputControl.instance.guiCheckKeyEvent();

            //调用GUI库中的绘制函数
            ALGUIMain.instance.ALGUIPain();

            //查询是否正在拖拽，是则进行拖拽操作的绘制
            if(ALGUIDragItemMgr.instance.isDragging)
            {
                //调用拖拽管理对象的绘制函数
                ALGUIDragItemMgr.instance.painDragWnd();
            }
        }
#endif

        /** 键盘按键事件，返回处理结果 */
        protected bool _onKeyDown(KeyCode _key)
        {
#if AL_UNITY_GUI
            if (_key == KeyCode.Tab)
            {
                if (null != ALGUIMain.instance.focusWnd)
                {
                    //切换到下一个窗口
                    if (null != ALGUIMain.instance.focusWnd.tabNextWnd)
                    {
                        ALGUIMain.instance.focusWnd.tabNextWnd.focus();

                        //返回处理成功
                        return true;
                    }
                }
            }

            //tab没处理则进入正常处理
            if (null != ALGUIMain.instance.focusWnd)
            {
                //返回处理结果
                return ALGUIMain.instance.focusWnd.OnPressKey(_key);
            }
#endif

            return false;
        }

        /**************************
         * 加载资源中对应的ui对象，并将对象实例化到对应ui层级中
         **/
        public _AALBasicUIWndMono loadUI(ALAssetBundleObj _assetObj, string _objName, Transform _parent)
        {
            if (null == _assetObj)
            {
                Debug.LogError("load ui: " + _objName + " _assetObj is null!");
                return null;
            }

            if (null == _parent)
            {
                Debug.LogError("load ui: " + _objName + " _parent is null!");
                return null;
            }

            //尝试加载对象
            GameObject resObj = _assetObj.load<GameObject>(_objName);
            return loadUI(resObj, _objName, _parent);
        }
        public _AALBasicUIWndMono loadUI(GameObject _go, string _objName, Transform _parent)
        {
            if (null == _go)
            {
                Debug.LogError("load ui: " + _objName + " error 2!");
                return null;
            }
            
            if (null == _parent)
            {
                Debug.LogError("load ui: " + _objName + ", _parent is null 2!");
                return null;
            }

            //实例化加载的对象
            GameObject newGo = GameObject.Instantiate(_go, _parent.position, Quaternion.identity) as GameObject;

            //设置父节点
            newGo.transform.SetParent(_parent, false);
            newGo.transform.localPosition = Vector3.zero;
            //newGo.transform.localScale = Vector2.zero;

            //重置尺寸
            newGo.transform.localScale = new Vector3(1, 1, 1);

            //查询对应窗口中的脚本对象
            _AALBasicUIWndMono wndMono = newGo.GetComponent<_AALBasicUIWndMono>();
            if (null == wndMono)
            {
                Debug.LogError("ui: " + _objName + " don't have Basic UI Mono: _AALBasicUIWndMono");
                //释放ui对象
                ALUnityCommon.releaseGameObj(newGo);

                return null;
            }

            //进入层级管理，如UI对象在根层级上，需要根据根层级的配置进行处理
            //查询匹配的根节点UI对象
            ALUILayerRootNode rootNode = ALUILayerMgr.instance.fixRootNode(_parent);
            //如有匹配根节点，则针对根节点进行添加子对象的修正处理
            if (null != rootNode)
            {
                rootNode.checkChildUIWnd(wndMono);
            }

            return wndMono;
        }

        /// <summary>
        /// 获取对应窗口层级对应的父对象节点
        /// </summary>
        /// <param name="_wndLayer"></param>
        /// <returns></returns>
        public Transform getUILayerRootTrans(EALUIWndLayer _wndLayer)
        {
            //判断ui根节点是否有效
            if(null == uiLayerInfoList)
            {
                Debug.LogError("ugui (root object/ui layer info list) is null!");
                return null;
            }

            //查询对应层级的节点对象
            for(int i = 0; i < uiLayerInfoList.Count; i++)
            {
                ALUILayerInfo layerInfo = uiLayerInfoList[i];
                if(null == layerInfo)
                    continue;

                if(layerInfo.layer == _wndLayer)
                {
                    if(null != layerInfo.layerRootGo)
                        return layerInfo.layerRootGo;

                    if(null != layerInfo.cavasObj)
                        return layerInfo.cavasObj.transform;
                }
            }

            return fullCanvas.transform;
        }

        /**************
         * 将2DUI的位置转化为当前视图中的UI坐标
         **/
        public Vector2 swapUIVector(Vector2 _screenPos)
        {
            if (null == _m_rtUIRootRectTrans)
                return _screenPos;

            Rect rect = uiCamera.rect;

            return new Vector2(_m_rtUIRootRectTrans.rect.width * ((_screenPos.x / Screen.width) - rect.x) / rect.width, _m_rtUIRootRectTrans.rect.height * ((_screenPos.y / Screen.height) - rect.y) / rect.height);
        }
        public Vector2 getScreenPosFromUI(Vector2 _uiVec)
        {
            if (null == _m_rtUIRootRectTrans)
                return _uiVec;

            Rect rect = uiCamera.rect;

            return new Vector2(Screen.width * (((_uiVec.x / _m_rtUIRootRectTrans.rect.width) * rect.width) + rect.x), Screen.height * (((_uiVec.y / _m_rtUIRootRectTrans.rect.height) * rect.height) + rect.y));
        }
        
        /// <summary>
        /// UI坐标到屏幕中心的距离平方
        /// </summary>
        /// <param name="_uiVec"></param>
        /// <returns></returns>
        public float getSqrMagnitudeFromScreenCenter(Vector2 _uiVec)
        {
            if (null == _m_rtUIRootRectTrans)
                return Vector2.SqrMagnitude(_uiVec);
            return Vector2.SqrMagnitude(_uiVec - new Vector2(_m_rtUIRootRectTrans.rect.width/ 2, _m_rtUIRootRectTrans.rect.height / 2));
        }

        /// <summary>
        /// 判断坐标是否在屏幕内
        /// </summary>
        /// <returns></returns>
        public bool isUIVecInScreen(Vector2 _uiVec)
        {
            if (_uiVec.x < 0 || _uiVec.y < 0
                || _uiVec.x > _m_rtUIRootRectTrans.rect.width
                || _uiVec.y > _m_rtUIRootRectTrans.rect.height)
                return false;

            return true;
        }

        /*****************
         * 判断对应位置是否在ugui中
         **/
        public bool isPointInUgui(Vector2 _pos)
        {
            if (null == uiEventSystem || ((null == uiGraphicRaycasterList || uiGraphicRaycasterList.Count <= 0) && (null == _m_lDynamicGraphicRaycaster || _m_lDynamicGraphicRaycaster.Count <= 0)))
                return false;

            if (null == _m_tmpPointEventData)
                return false;

            //进行射线判断
            _m_tmpPointEventData.pressPosition = _pos;
            _m_tmpPointEventData.position = _pos;

            //进行射线检测
            for (int i = 0; i < uiGraphicRaycasterList.Count; i++)
            {
                if (null == uiGraphicRaycasterList[i])
                    continue;

                uiGraphicRaycasterList[i].Raycast(_m_tmpPointEventData, _m_lTestUIRaycastList);

                //遍历判断是否有在非屏蔽层级中的UI对象
                for (int n = 0; n < _m_lTestUIRaycastList.Count; n++)
                {
                    RaycastResult tmpItem = _m_lTestUIRaycastList[n];
                    if (null == tmpItem.gameObject)
                        continue;

                    //判断layer是否是ingore Raycast，是的过滤在ugui中, 2 == ingore Raycast
                    if (tmpItem.gameObject.layer != 2)
                    {
                        _m_lTestUIRaycastList.Clear();
                        return true;
                    }
                }

                _m_lTestUIRaycastList.Clear();
            }
            for (int i = 0; i < _m_lDynamicGraphicRaycaster.Count; i++)
            {
                if (null == _m_lDynamicGraphicRaycaster[i])
                    continue;

                _m_lDynamicGraphicRaycaster[i].Raycast(_m_tmpPointEventData, _m_lTestUIRaycastList);

                //遍历判断是否有在非屏蔽层级中的UI对象
                for (int n = 0; n < _m_lTestUIRaycastList.Count; n++)
                {
                    RaycastResult tmpItem = _m_lTestUIRaycastList[n];
                    if (null == tmpItem.gameObject)
                        continue;

                    //判断layer是否是ingore Raycast，是的过滤在ugui中, 2 == ingore Raycast
                    if (tmpItem.gameObject.layer != 2)
                    {
                        _m_lTestUIRaycastList.Clear();
                        return true;
                    }
                }

                _m_lTestUIRaycastList.Clear();
            }

            return false;
        }

        /**************
         * 设置是否pc平台
         **/
        public void setIsPcPlat(bool _isPcPlat)
        {
            isPcPlat = _isPcPlat;

            if (isPcPlat)
                ALInputControl.instance.regInputListener(new ALPCInputListener());
            else
                ALInputControl.instance.regInputListener(new ALIOSInputListener());
        }

        /****************
         * 获取对应层级的canvas对象
         **/
        public Canvas getLayerCanvas(EALUIWndLayer _layer)
        {
            ALUILayerInfo tmpInfo = null;
            for (int i = 0; i < uiLayerInfoList.Count; i++)
            {
                tmpInfo = uiLayerInfoList[i];
                if (null == tmpInfo)
                    continue;

                if (tmpInfo.layer == _layer)
                    return tmpInfo.cavasObj;
            }

            return null;
        }

        /********************
         * 获取当前的客户端版本对应整形
         **/
        public abstract int curVersionNum { get; }
        /********************
         * 获取当前的服务器对应整形
         **/
        public abstract int curServerVersionNum { get; }

        /// <summary>
        /// 当版本过低的时候的触发效果
        /// </summary>
        public abstract void onClickVersionLowerErr();
        /// <summary>
        /// 当服务器版本过低的时候的触发效果
        /// </summary>
        public abstract void onClickServerVersionLowerErr();
        /// <summary>
        /// 当Hotfix版本过低的时候的触发效果
        /// </summary>
        public abstract void onClickHotfixVersionLowerErr();
        /****************
         * 当发生了未知错误时
         **/
        public abstract void onUnknowErrorOccurred(Exception _e);
        /********************
         * 当执行窗口大小更改时
         **/
        public abstract void onClientScreenOnSize(int _newWidth, int _newHeight);

#if AL_UNITY_GUI
        /********************
         * 在GUI响应了鼠标操作时触发的事件
         **/
        protected abstract void _onALGUICatchMouse();
        /********************
         * 子类用于在GUI未处理鼠标操作时进行的鼠标操作处理函数
         **/
        protected abstract void _dealMouseActionWhenALGUIDidNotCatchMouse();
#endif

        /********************
         * 在脚本开始运行的时候调用的函数
         **/
        protected abstract void _onStart();

        /********************
         * 子类用于处理GUI判断前的操作处理
         **/
        protected abstract void _onPreGuiUpdate();

        /********************
         * 子类用于处理正常帧处理操作
         **/
        protected abstract void _onUpdate();
    }
}
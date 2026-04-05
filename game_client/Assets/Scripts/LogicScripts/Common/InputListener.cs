using ALPackage;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GOE
{
    public class TouchInfo
    {
        private static float _g_fHoldingTime = 1f;

        public EALGUIOpButtonType _m_eBtnType;

        private int _m_iOpSerialize;

        private Vector2 _m_vBeginPos;
        private Vector2 _m_vPos;             // Current position of the mouse or touch event
        private Vector2 _m_vDelta;           // Delta since last update

        private GameObject _m_goPressGo;         // 点下的对象

        private float _m_fPressStartTime = 0f;

        private bool _m_bIsPress = false;
        private bool _m_bIsDragging = false;
        private bool _m_bIsHolding = false;

        //是否按在UI上，非UI还是要做行为监控
        private bool _m_bIsUIPress = false;

        public TouchInfo(EALGUIOpButtonType _type)
        {
            _m_eBtnType = _type;

            _m_iOpSerialize = 0;

            _m_vBeginPos = Vector2.zero;
            _m_vPos = Vector2.zero;
            _m_vDelta = Vector2.zero;

            _m_goPressGo = null;

            _m_fPressStartTime = 0f;

            _m_bIsPress = false;
            _m_bIsDragging = false;
            _m_bIsHolding = false;

            //是否按在UI上，非UI还是要做行为监控
            _m_bIsUIPress = false;
        }

        public EALGUIOpButtonType btnType { get { return _m_eBtnType; } }
        public int opSerialize { get { return _m_iOpSerialize; } }
        public Vector2 curPos { get { return _m_vPos; } }
        public GameObject pressGo { get { return _m_goPressGo; } set { _m_goPressGo = value; } }
        public bool isPress { get { return _m_bIsPress; } }
        public bool isDragging { get { return _m_bIsDragging; } }
        public bool isHolding { get { return _m_bIsHolding; } }
        public bool isUIPress { get { return _m_bIsUIPress; } }

        /** 设置释放处理，返回是否处理成功 */
        public bool setUnPress()
        {
            if(!_m_bIsPress)
                return false;

            _m_iOpSerialize = 0;

            //设置按下状态
            _m_bIsPress = false;
            _m_bIsDragging = false;
            _m_bIsHolding = false;

            _m_bIsUIPress = false;

            _m_vBeginPos = Vector2.zero;
            _m_vDelta = Vector2.zero;

            //_m_goPressGo = null;

            _m_fPressStartTime = Time.unscaledTime;

            return true;
        }

        /** 设置按下处理，返回设置是否成功 */
        public bool setPress(int _opSerialize, Vector2 _pressPos, GameObject _pressGo, bool _isUIPress)
        {
            if(_m_bIsPress)
                return false;

            _m_iOpSerialize = _opSerialize;

            //设置按下状态
            _m_bIsPress = true;
            _m_bIsDragging = false;
            _m_bIsHolding = false;

            _m_bIsUIPress = _isUIPress;

            _m_vBeginPos = _pressPos;
            _m_vPos = _pressPos;
            _m_vDelta = Vector2.zero;

            _m_goPressGo = _pressGo;

            _m_fPressStartTime = Time.unscaledTime;

            return true;
        }

        /** 在按下状态下的tick处理 */
        public void pressTick(float _dragDis, _AInput _inputObj)
        {
            if(!_m_bIsPress)
                return;

            Vector2 curPos = ALInputControl.instance.getBtnUnityPos(_m_eBtnType);
            //根据是否已经在拖拽状态进行判断
            if(!_m_bIsDragging)
            {
                //判断移动距离
                if(Math.Abs(curPos.x - _m_vBeginPos.x) >= _dragDis
                    || Math.Abs(curPos.y - _m_vBeginPos.y) >= _dragDis)
                {
                    //进入拖拽操作
                    _m_bIsDragging = true;
                    //处理开始拖拽
                    if(null != _inputObj && !_m_bIsUIPress)
                    {
                        //处理开始拖拽操作
                        _inputObj.OnDragStart(_m_goPressGo, this, Time.unscaledTime - _m_fPressStartTime);
                    }
                }
                else
                {
                    float holdingTime = Time.unscaledTime - _m_fPressStartTime;
                    if(holdingTime > _g_fHoldingTime)
                    {
                        _m_bIsHolding = true;

                        //处理长按
                        if(null != _inputObj && !_m_bIsUIPress)
                        {
                            //处理开始拖拽操作
                            _inputObj.OnHold(_m_goPressGo, this, holdingTime);
                        }
                    }
                }
            }

            //判断是否需要进行拖拽过程处理
            if(_m_bIsDragging)
            {
                //计算移动距离
                _m_vDelta = curPos - _m_vPos;

                //进行拖拽移动处理操作
                if(null != _inputObj && !_m_bIsUIPress)
                {
                    _inputObj.OnDrag(_m_goPressGo, _m_vDelta, this);
                }

                //设置当前位置
                _m_vPos = curPos;
            }
        }
    }

    public class InputListener : WCGSingleton<InputListener>
    {
        /// <summary>
        /// 在3D场景点击操作中屏蔽点击得ui控制相关对象
        /// 单独增加此控制是由于部分UI元素为了避免屏蔽3D场景拖拽，需要允许拖拽穿透
        /// 因此在本列表中的UI捕捉对象会屏蔽3D操作响应的点击与holding操作
        /// </summary>
        private List<GraphicRaycaster> _m_lIgnoreClickUIGraphicRaycasterList;
        /// <summary>
        /// 用于检测UI操作位置的数据对象，由于需要反复使用因此直接常驻变量
        /// </summary>
        private PointerEventData _m_tmpPointEventData;
        private EventSystem _m_esUIEventSystem;
        /** 测试用列表，避免重复创建 */
        private List<RaycastResult> _m_lTestUIRaycastList = new List<RaycastResult>();

        //类型列表
        protected EALGUIOpButtonType[] _m_lTypeList = new EALGUIOpButtonType[] { EALGUIOpButtonType.NONE, EALGUIOpButtonType.OP_BTN, EALGUIOpButtonType.ASSIST_BTN };

        //每个触点的处理操作
        protected TouchInfo[] _m_arrTouchArr = new TouchInfo[ALCommon.getEnumCount(typeof(EALGUIOpButtonType))];

        //开启拖拽的判断范围
#if UNITY_EDITOR || UNITY_STANDALONE
        protected float _m_fDragDistance = Mathf.Min(Screen.height, Screen.width) / 100;
#else
    protected float _m_fDragDistance = Mathf.Min(Screen.height, Screen.width) / 50;
#endif

        //操作点的数量
        private int _m_iOpCount;

        //射线结果
        private RaycastHit _m_rLastHit;

        private bool _m_bIsLockInput = false;
        public void lockInput() { _m_bIsLockInput = true; _m_iCurrentInput = null; }
        public void unlockInput() { _m_bIsLockInput = false; }

        //处理操作的对象
        public _AInput _m_iCurrentInput;
        public _AInput currentInput { get { return _m_iCurrentInput; } }

        public int touchCount { get { return _m_iOpCount; } }

        public InputListener()
        {
            for(int i = 0; i < _m_arrTouchArr.Length; i++)
            {
                _m_arrTouchArr[i] = new TouchInfo((EALGUIOpButtonType)i);
            }
        }

        /// <summary>
        /// 初始化用于判断UI是否需要屏蔽3D点击操作的相关变量初始化函数
        /// </summary>
        /// <param name="_eventSystem"></param>
        public void initIgnoreClick3DUI(EventSystem _eventSystem)
        {
            _m_esUIEventSystem = _eventSystem;
            //初始化顶点事件系统数据
            _m_tmpPointEventData = new PointerEventData(_m_esUIEventSystem);

            _m_lIgnoreClickUIGraphicRaycasterList = new List<GraphicRaycaster>();
        }

        /// <summary>
        /// 注册和注销屏蔽3D点击效果的UI GraphicRaycaster对象
        /// </summary>
        public void regIgnoreClickUIGraphics(GraphicRaycaster _obj)
        {
            if (null == _obj)
                return;

            //不重复添加
            if (_m_lIgnoreClickUIGraphicRaycasterList.Contains(_obj))
                return;

            _m_lIgnoreClickUIGraphicRaycasterList.Add(_obj);
        }
        public void unregIgnoreClickUIGraphics(GraphicRaycaster _obj)
        {
            if (null == _obj)
                return;

            _m_lIgnoreClickUIGraphicRaycasterList.Remove(_obj);
        }

        /************
         * 设置输入监听对象
         **/
        public void setInputObj(_AInput _newObj)
        {
            if(_m_bIsLockInput)
                return;

            if(_newObj == _m_iCurrentInput)
                return;

            if(null != _m_iCurrentInput)
                _m_iCurrentInput.OnExit();

            _m_iCurrentInput = _newObj;

            if(null != _m_iCurrentInput)
                _m_iCurrentInput.OnEnter();
        }

        /** 返回第一个按下的触点信息 */
        public TouchInfo getFirstPressTouchInfo()
        {
            for(int i = 0; i < _m_arrTouchArr.Length; i++)
            {
                if(_m_arrTouchArr[i].isPress)
                    return _m_arrTouchArr[i];
            }

            return null;
        }

        //每帧处理
        public void Update()
        {
            //进行触点的判断和处理
            bool isPress = _processBtnType(EALGUIOpButtonType.OP_BTN);
            isPress |=_processBtnType(EALGUIOpButtonType.ASSIST_BTN);

            if (isPress)
            {
                WinMsg.SendMsg(WinMsgType.SCREEN_PRESS);
            }
            
            if (currentInput != null)
            {
                //进行缩放处理
                if(ALInputControl.instance.getScaleChg() != 0)
                    _m_iCurrentInput.OnScaleChg(ALInputControl.instance.getScaleChg());

                //进行输入对象的处理
                currentInput.Update();
            }
        }

        /**********
         * 射线处理，返回射到的物体
         **/
        private GameObject _raycast(Vector3 _screenPos)
        {
            if(CameraController.instance.controlCamera == null)
            {
                return null;
            }

            // Convert to view space
            Vector3 pos = CameraController.instance.controlCamera.ScreenToViewportPoint(_screenPos);
            if(float.IsNaN(pos.x) || float.IsNaN(pos.y))
                return null;

            // If it's outside the camera's viewport, do nothing
            if(pos.x < 0f || pos.x > 1f || pos.y < 0f || pos.y > 1f)
                return null;

            // Cast a ray into the screen
            Ray ray = CameraController.instance.controlCamera.ScreenPointToRay(_screenPos);

            if(Physics.Raycast(ray, out _m_rLastHit, 200f))
            {
                return _m_rLastHit.collider.gameObject;
            }

            return null;
        }

        public TouchInfo GetTouch(EALGUIOpButtonType _type)
        {
            return _m_arrTouchArr[(int)_type];
        }

        /**********
         * 进行不同的操作处理的函数
         **/
        private void _unPressBtn(TouchInfo _touchInfo)
        {
            if(null == _touchInfo || !_touchInfo.isPress)
                return;

            bool isDragging = _touchInfo.isDragging;
            bool isHolding = _touchInfo.isHolding;
            bool isUIPress = _touchInfo.isUIPress;
            //进行未按下处理
            if (_touchInfo.setUnPress())
            {
                //减少操作数量
                _m_iOpCount--;

                //判断按键是否进行了拖拽
                if(!isDragging && !isHolding)
                {
                    //进行点击处理
                    if (null != _m_iCurrentInput && !isUIPress
                        //判断是否在需要屏蔽点击的UI范围内
                        && !isPointInIgnoreClickUgui(_touchInfo.curPos))
                        _m_iCurrentInput.OnClick(_touchInfo.pressGo, _touchInfo);

                    //发送一个点击事件
                    WinMsg.SendMsg(WinMsgType.SCREEN_CLICK, _touchInfo);
                }
                else if(isDragging)
                {
                    //拖拽过程则进行拖拽结束处理
                    if(null != _m_iCurrentInput && !isUIPress)
                        _m_iCurrentInput.OnDragEnd(_touchInfo.pressGo, _touchInfo);
                }

                //进行释放的事件处理
                if(null != _m_iCurrentInput && !isUIPress)
                    _m_iCurrentInput.OnPress(_touchInfo.pressGo, false, _touchInfo);

                _touchInfo.pressGo = null;
            }
        }

        //进行指定类型的处理，返回是否按下
        private bool _processBtnType(EALGUIOpButtonType _btnType)
        {
            //获取数据对象
            TouchInfo touchInfo = GetTouch(_btnType);

            //判断是否按下
            bool btnPress = ALInputControl.instance.isBtnPress(_btnType);
            if(!btnPress)
            {
                //进行未按下处理
                _unPressBtn(touchInfo);
                //不进行其他处理
                return false;
            }
            else
            {
                //当按下状态且序列号不同时进行如下处理
                if(touchInfo.isPress && ALInputControl.instance.getOpSerialize(_btnType) != touchInfo.opSerialize)
                {
                    //进行放开处理
                    _unPressBtn(touchInfo);
                }

                //当已经在按下的时候，且操作序列号一致的时候进行如下处理
                if(!touchInfo.isPress)
                {
                    //进行射线判断
                    GameObject castGo = _raycast(ALInputControl.instance.getBtnUnityPos(_btnType));
                    //尝试进行点击处理
                    if (touchInfo.setPress(ALInputControl.instance.getOpSerialize(_btnType), ALInputControl.instance.getBtnUnityPos(_btnType), castGo, ALInputControl.instance.isBtnPressUGUI(_btnType)))
                    {
                        _m_iOpCount++;

                        //设置按下则进行按下处理
                        if (null != _m_iCurrentInput && !touchInfo.isUIPress)
                            _m_iCurrentInput.OnPress(castGo, true, touchInfo);
                    }
                }
                else
                {
                    //已经在按下状态则进行按下的处理
                    touchInfo.pressTick(_m_fDragDistance, _m_iCurrentInput);
                }

                return true;
            }
        }

        /// <summary>
        /// 判断点击位置是否在需要屏蔽3D操作点击、holding操作的范围内
        /// </summary>
        /// <param name="_pos"></param>
        /// <returns></returns>
        private bool isPointInIgnoreClickUgui(Vector2 _pos)
        {
            if (null == _m_esUIEventSystem || ((null == _m_lIgnoreClickUIGraphicRaycasterList || _m_lIgnoreClickUIGraphicRaycasterList.Count <= 0)))
                return false;

            //进行射线判断
            _m_tmpPointEventData.pressPosition = _pos;
            _m_tmpPointEventData.position = _pos;

            //进行射线检测
            for (int i = 0; i < _m_lIgnoreClickUIGraphicRaycasterList.Count; i++)
            {
                if (null == _m_lIgnoreClickUIGraphicRaycasterList[i])
                    continue;

                _m_lIgnoreClickUIGraphicRaycasterList[i].Raycast(_m_tmpPointEventData, _m_lTestUIRaycastList);

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
            }

            return false;
        }
    }
}

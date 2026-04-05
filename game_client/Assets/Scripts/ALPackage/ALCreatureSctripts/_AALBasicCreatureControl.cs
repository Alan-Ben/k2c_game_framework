using System;
using System.Collections.Generic;

using UnityEngine;

#if AL_CREATURE_SYS
namespace ALPackage
{
    /******************
     * 在角色控制对象中执行的当前帧动作触发事件对象的later执行信息
     **/
    public class ALCreatureDealLaterAniEventInfo
    {
        public _AALSOBaseEvent eventObj;
        public ALCreatureAnimationSession animationSession;
    }

    public abstract class _AALBasicCreatureControl
        : MonoBehaviour
    {
        /***********
         * 使用2进制方式判断角色移动方向
         **/
        public static int MOVE_FORWARD = 1;
        public static int MOVE_BACK = 2;
        public static int MOVE_LEFT = 4;
        public static int MOVE_RIGHT = 8;
        public static int MOVE_DESTINATION = 16;

        /** 骨骼管理对象 */
        protected ALCreatureBoneMgr _m_bmBoneMgr = null;

        /** Unity 自带脚本部分 */
        protected CharacterController _m_ccCharacterControl = null;

        /** 移动状态机对象 */
        protected ALCharacterMoveStateMachine _m_smMoveStateMachine = null;

        /** 移动状态机对象 */
        protected _AALBasicActionStateMachine _m_smActionStateMachine = null;

        /** 动作控制对象 */
        protected ALCreatureAnimationControler _m_acAnimationControl = null;

        /** action 控制对象 */
        protected ALCreatureActionControler _m_acActionControl = null;

        /** 添加物的控制对象 */
        protected ALCreatureAdditionObjMgr _m_omAdditionObjMgr = null;

        /** 每帧检测对象的管理对象 */
        protected ALCreatureFrameCheckerMgr _m_fcFrameCheckerMgr = null;

        /** 角色显示属性控制对象 */
        protected ALCreatureDisplayController _m_dcDisplayController = null;

        /** 当前帧需要处理的later事件对象 */
        protected List<ALCreatureDealLaterAniEventInfo> _m_lLaterEventInfoList = new List<ALCreatureDealLaterAniEventInfo>();

        /** 本对象所关联到的父对象 */
        protected _AALBasicCreatureControl _m_ccParentMoveCreatureControl = null;
        /** 本对象关联的子对象控制对象 */
        protected List<_AALBasicCreatureControl> _m_lMoveChildCreatureControlList = new List<_AALBasicCreatureControl>(1);

        /** 是否已经初始化完成 */
        protected bool _m_bIsInited = false;
        /** 在完成初始化时调用的回调函数 */
        protected Action _m_dOnInited = null;

        /** 角色移动速度 */
        protected float _m_fMoveSpeed = 4.5f;
        protected Vector3 _m_vMoveForward;

        //角色转向参数部分
        protected float _m_fSlerpSpeed = 0f;
        protected Vector3 _m_vForward = new Vector3();
        protected Quaternion _m_qTargetRotation = Quaternion.identity;

        /** 角色相关事件处理对象 */
        protected _AALCharacterEventDealer _m_edEventDealer;

        /** 提取出行为位移操作的时间创造对象 */
        protected _IActionMovementFactory _m_amfMovementFactory = null;

        /** 角色缩放比例参数，默认都为1，当缩放时根据缩放比例进行处理 */
        protected Vector3 _m_vScaleSize = new Vector3(1, 1, 1);

        /** 判断是否显示的标记数字 */
        private int _m_iVisibleJudgeInt = 0;
        private int _m_iPreVisibleJudgeInt = 0;
        /** 对象是否可见 */
        private bool _m_bIsVisible = false;

        void Awake()
        {
            //动作对象需要提前创建
            _m_acAnimationControl = new ALCreatureAnimationControler(this);

            //create action controler
            _m_acActionControl = new ALCreatureActionControler(this);

            //添加物控制
            _m_omAdditionObjMgr = new ALCreatureAdditionObjMgr(this);

            //创建显示控制对象
            _m_dcDisplayController = new ALCreatureDisplayController(this);

            //添加脚本对象
            Animation aniScript = transform.GetComponent<Animation>();
            if (null == aniScript)
                aniScript = transform.gameObject.AddComponent<Animation>();

            //判断是否有render脚本，如无则添加，需要用于显示判断
            SkinnedMeshRenderer skinedRender = transform.GetComponent<SkinnedMeshRenderer>();
            if (null == skinedRender)
                skinedRender = transform.gameObject.AddComponent<SkinnedMeshRenderer>();

            //停止所有动作，避免有自带动作在播放
            aniScript.Stop();

            //设置动画播放方式
            aniScript.cullingType = AnimationCullingType.AlwaysAnimate;
        }

        // Use this for initialization
        void Start()
        {
            _m_ccCharacterControl = transform.GetComponent<CharacterController>();
            if (null == _m_ccCharacterControl)
            {
                _m_ccCharacterControl = transform.gameObject.AddComponent<CharacterController>();

                //设置角色控制对象默认属性
                _m_ccCharacterControl.enabled = true;
                _m_ccCharacterControl.isTrigger = false;
                _m_ccCharacterControl.radius = 0.3f;
                _m_ccCharacterControl.height = 2;
                _m_ccCharacterControl.center = new Vector3(0, 1, 0);
            }

            //create move state machine
            _m_smMoveStateMachine = new ALCharacterMoveStateMachine(this);

            //create action state machine
            _m_smActionStateMachine = _createCharacterActionStateMachine();

            //create the frame checker manager
            _m_fcFrameCheckerMgr = new ALCreatureFrameCheckerMgr(this);

            //init move state
            _m_smMoveStateMachine.init();

            //init bone mgr
            _m_bmBoneMgr = new ALCreatureBoneMgr(this);
            _m_bmBoneMgr.resetBones();

            //调用回调，并重置回调对象
            if (null != _m_dOnInited)
            {
                _m_bIsInited = true;
                _m_dOnInited();
                _m_dOnInited = null;
            }
        }

        /************
         * 动作事件处理函数
         **/
        public void onALAnimationEvent(AnimationEvent _event)
        {
            if (null == _event.animationState)
                return;

            //获取事件的信息对象
            _AALSOBaseEvent ev = _event.objectReferenceParameter as _AALSOBaseEvent;
            if (null == ev)
                return;

            //获取动作片段名，根据片段名获取SessionID
            string animationName = _event.animationState.name;
            string[] splitStrs = animationName.Split('_');
            int sessionID = 0;

            try
            {
                sessionID = Int32.Parse(splitStrs[0]);
            }
            catch (Exception)
            {
                Debug.LogError("ALAnimationEvent err, get sesion id err! animationState Name; " + animationName);
                return;
            }

            //获取对应AnimationSession
            ALCreatureAnimationSession animationSession = _m_acAnimationControl.getAnimationSession(sessionID);

            if (null == animationSession)
                return;

            //处理事件
            ev.activeEvent(animationSession, this);

            //判断事件是否需要进入later处理
            if (ev.needLaterActive())
            {
                //创建信息对象
                ALCreatureDealLaterAniEventInfo info = new ALCreatureDealLaterAniEventInfo();
                info.animationSession = animationSession;
                info.eventObj = ev;

                _m_lLaterEventInfoList.Add(info);
            }
        }

        /********************
         * 注册一个初始化完成后的回调函数
         **/
        public void regInitedDelegate(Action _delegate)
        {
            if (null == _delegate)
                return;

            if (_m_bIsInited)
            {
                //直接调用回调函数
                _delegate();
                return;
            }

            //加入回调对象
            if (null == _m_dOnInited)
                _m_dOnInited = _delegate;
            else
                _m_dOnInited += _delegate;
        }

        /****************************
         * 设置一个事件处理对象
         **/
        public void regEventDealer(_AALCharacterEventDealer _eventDealer)
        {
            _m_edEventDealer = _eventDealer;
        }

        /***********************
         * 获取当前事件处理对象
         **/
        public _AALCharacterEventDealer getEventDealer()
        {
            return _m_edEventDealer;
        }

        /**********************
         * 注册移动的子控制对象
         **/
        public void regChildMoveCreatureControl(_AALBasicCreatureControl _childCreatureControl)
        {
            if (null == _childCreatureControl)
                return;

            if (_childCreatureControl._m_ccParentMoveCreatureControl == this)
                return;

            if (_childCreatureControl._m_ccParentMoveCreatureControl != null)
                _childCreatureControl._m_ccParentMoveCreatureControl.unregChildMoveCreatureControl(_childCreatureControl);

            _m_lMoveChildCreatureControlList.Add(_childCreatureControl);
        }
        public void unregChildMoveCreatureControl(_AALBasicCreatureControl _childCreatureControl)
        {
            if (null == _childCreatureControl)
                return;

            _m_lMoveChildCreatureControlList.Remove(_childCreatureControl);
            _childCreatureControl._m_ccParentMoveCreatureControl = null;
        }
        public void unregMoveFromParentControl()
        {
            if (null == _m_ccParentMoveCreatureControl)
                return;

            _m_ccParentMoveCreatureControl.unregChildMoveCreatureControl(this);
        }

        // Update is called once per frame
        void Update()
        {
            //判断对象是否显示
            _m_bIsVisible = (_m_iPreVisibleJudgeInt != _m_iVisibleJudgeInt);
            _m_iPreVisibleJudgeInt = _m_iVisibleJudgeInt;
            
            /******************
             * 处理显示数据控制对象的每帧处理函数
             **/
            displayController.frameCheck();

            /****************
             * 检查动作控制对象中的无效队列
             **/
            _m_acAnimationControl.checkAnimationDisableList();

            /***************
             * 检查角色行为状态机状态是否切换
             **/
            _m_smActionStateMachine.checkCurState();

            /****************
             * 处理每帧的事件检测
             **/
            _m_fcFrameCheckerMgr.dealCheck();

            /*****************
             * 进行移动操作
             **/
            _dealMoveDirection();

            /****************
             * 执行事件处理对象的每帧事件
             **/
            _AALCharacterEventDealer eventDealer = getEventDealer();
            if (null != eventDealer)
                eventDealer.onUpdate();
        }

        // LateUpdate is called once per frame after update
        void LateUpdate()
        {
            //处理由animation触发的需要later处理的事件对象
            for (int i = 0; i < _m_lLaterEventInfoList.Count; i++)
            {
                ALCreatureDealLaterAniEventInfo eventInfo = _m_lLaterEventInfoList[i];
                if(null == eventInfo)
                    continue;

                eventInfo.eventObj.lateActiveEvent(eventInfo.animationSession, this);
            }
            //清空列表
            _m_lLaterEventInfoList.Clear();

            /****************
             * 处理每帧需要在Update之后处理的函数，如Action，Operation的释放
             **/
            _m_fcFrameCheckerMgr.dealLaterCheck();
        }

        /**************
         * 在本对象在摄像头中显示的时候将调用本函数，在调用本函数的时候将数据进行刷新，以此判断数据是否显示
         **/
        void OnWillRenderObject()
        {
            _m_iVisibleJudgeInt++;
        }

        public CharacterController getCharacterController()
        {
            return _m_ccCharacterControl;
        }

        public bool isVisible { get { return _m_bIsVisible; } }
        public ALCreatureAdditionObjMgr additionObjMgr { get { return _m_omAdditionObjMgr; } }
        public ALCreatureDisplayController displayController { get { return _m_dcDisplayController; } }

        public ALCreatureAnimationControler getAnimationControler() { return _m_acAnimationControl; }
        public ALCreatureActionControler getActionControler() { return _m_acActionControl; }
        public ALCreatureFrameCheckerMgr getFrameCheckerMgr() { return _m_fcFrameCheckerMgr; }
        public ALCharacterMoveStateMachine getMoveStateMachine() { return _m_smMoveStateMachine; }
        public _AALBasicActionStateMachine getActionStateMachine() { return _m_smActionStateMachine; }
        public ALCreatureBoneMgr getBoneMgr() { return _m_bmBoneMgr; }

        /***********
         * 设置角色移动速度
         **/
        public float moveSpeed
        {
            get
            {
                return _m_fMoveSpeed;
            }
            set
            {
                if (_m_fMoveSpeed == value)
                    return;

                _m_fMoveSpeed = value;

                //设置时需要刷新移动状态机的所有动作信息
                getMoveStateMachine().getCurState().refreshAnimationSession();

                //子对象进入对应状态
                for (int i = 0; i < _m_lMoveChildCreatureControlList.Count; i++)
                    _m_lMoveChildCreatureControlList[i].moveSpeed = value;
            }
        }

        public Vector3 moveForward { get { return _m_vMoveForward; } }
        public Vector3 forward { get { return _m_vForward; } }

        /** 缩放本对象的尺寸比例 */
        public Vector3 scaleSize 
        { 
            get 
            { 
                return _m_vScaleSize; 
            }
            set
            {
                //获取当前比例，将本对象的比例根据缩放比例处理
                Vector3 dealScale = new Vector3(
                                            value.x / _m_vScaleSize.x
                                            , value.y / _m_vScaleSize.y
                                            , value.z / _m_vScaleSize.z
                                            );

                //计算实际应该缩放的比例
                Vector3 targetLocalScale = new Vector3(
                                            dealScale.x * transform.localScale.x
                                            , dealScale.y * transform.localScale.y
                                            , dealScale.z * transform.localScale.z
                                            );

                //设置对象的比例
                transform.localScale = targetLocalScale;
                _m_vScaleSize = value;
            }
        }

        /***********
         * 设置角色朝向
         **/
        public void setTargetPos(Vector3 _targetPos)
        {
            //获取朝向
            Vector3 forward = _targetPos - transform.position;
            forward.y = 0;

            //设置朝向
            setForward(forward);
        }
        public void setForward(Vector3 _forward)
        {
            //有移动方向则计算需要旋转的角度
            if (_forward.sqrMagnitude != 0)
            {
                _m_vForward = _forward;
                //设置旋转参数
                _m_qTargetRotation = ALUnityCommon.getForwardQuaternion(_forward);
                float needRotateDig = Quaternion.Angle(transform.rotation, _m_qTargetRotation);

                //根据需要旋转角度计算球形插值的参数，夹角越小相对需要参数越高，此处计算根据测试取一个效果不错的计算方式
                _m_fSlerpSpeed = 2 * 360 / needRotateDig;
            }
        }
        /***********
         * 设置角色移动方式
         **/
        public void setMoveForward(Vector3 _cameraForward, int _direction)
        {
            //设置移动方向并且移动
            _m_vMoveForward = ALUnityCommon.getMoveDirection(_cameraForward, _direction);

            //有移动方向则计算需要旋转的角度
            if (_m_vMoveForward.sqrMagnitude != 0)
            {
                //设置移动状态
                getMoveStateMachine().IsMoving = true;

                //子对象进入对应状态
                for (int i = 0; i < _m_lMoveChildCreatureControlList.Count; i++)
                    _m_lMoveChildCreatureControlList[i].getMoveStateMachine().IsMoving = true;
            }
            else
            {
                getMoveStateMachine().IsMoving = false;

                //子对象进入对应状态
                for (int i = 0; i < _m_lMoveChildCreatureControlList.Count; i++)
                    _m_lMoveChildCreatureControlList[i].getMoveStateMachine().IsMoving = false;
            }
        }
        public void setMoveForward(Vector3 _cameraForward, Vector2 _direction)
        {
            //设置移动方向并且移动
            _m_vMoveForward = ALUnityCommon.getMoveDirection(_cameraForward, _direction);

            //有移动方向则计算需要旋转的角度
            if (_m_vMoveForward.sqrMagnitude != 0)
            {
                //设置移动状态
                getMoveStateMachine().IsMoving = true;

                //子对象进入对应状态
                for (int i = 0; i < _m_lMoveChildCreatureControlList.Count; i++)
                    _m_lMoveChildCreatureControlList[i].getMoveStateMachine().IsMoving = true;
            }
            else
            {
                getMoveStateMachine().IsMoving = false;

                //子对象进入对应状态
                for (int i = 0; i < _m_lMoveChildCreatureControlList.Count; i++)
                    _m_lMoveChildCreatureControlList[i].getMoveStateMachine().IsMoving = false;
            }
        }
        public void setMoveForward(Vector3 _moveForward)
        {
            //设置移动方向并且移动
            _m_vMoveForward = _moveForward.normalized;

            //有移动方向则计算需要旋转的角度
            if (_m_vMoveForward.sqrMagnitude != 0)
            {
                //设置移动状态
                getMoveStateMachine().IsMoving = true;

                //子对象进入对应状态
                for (int i = 0; i < _m_lMoveChildCreatureControlList.Count; i++)
                    _m_lMoveChildCreatureControlList[i].getMoveStateMachine().IsMoving = true;
            }
            else
            {
                getMoveStateMachine().IsMoving = false;

                //子对象进入对应状态
                for (int i = 0; i < _m_lMoveChildCreatureControlList.Count; i++)
                    _m_lMoveChildCreatureControlList[i].getMoveStateMachine().IsMoving = false;
            }
        }

        /*******************
         * 判断角色是否在地面上
         **/
        public float getFlowHeight(int _layerMask)
        {
            if (getCharacterController().isGrounded)
                return 0;

            //使用射线判断角色离地面距离，抬高0.1米进行计算，保证不穿透，同时需要将角色控制对象的偏移计算进去
            RaycastHit rayHit = new RaycastHit();
            if (Physics.Raycast(
                new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z) + (transform.forward * getCharacterController().center.z) + (transform.right * getCharacterController().center.x)
                , new Vector3(0, -1, 0)
                , out rayHit
                , 10f
                , _layerMask))
            {
                return (rayHit.distance - 0.1f);
            }
            else
            {
                //无碰撞表示肯定悬空
                return 10f;
            }
        }

        /*********************
         * 角色开始跳跃，在动作不允许移动的情况下不允许进入跳跃状态
         **/
        public void jump()
        {
            if (!getActionStateMachine().canMove())
                return;

            //尝试进入跳跃状态
            getMoveStateMachine().tryChgToState(new ALJumpCharacterMoveState(this));

            //子对象进入对应状态
            for (int i = 0; i < _m_lMoveChildCreatureControlList.Count; i++)
                _m_lMoveChildCreatureControlList[i].getMoveStateMachine().tryChgToState(new ALJumpCharacterMoveState(this));
        }

        /*********************
         * 角色不接触地面时进行的掉落操作，根据角色当前移动方向掉落，此操作为被动，因此不进行合法性判断
         **/
        public void drop()
        {
            //尝试进入跳跃状态
            getMoveStateMachine().tryChgToState(new ALDropCharacterMoveState(this));

            //子对象进入对应状态
            for (int i = 0; i < _m_lMoveChildCreatureControlList.Count; i++)
                _m_lMoveChildCreatureControlList[i].getMoveStateMachine().tryChgToState(new ALDropCharacterMoveState(this));
        }
        public void drop(Vector3 _direction, float _moveSpeed)
        {
            //尝试进入跳跃状态
            getMoveStateMachine().tryChgToState(new ALDropCharacterMoveState(this, _direction, _moveSpeed));

            //子对象进入对应状态
            for (int i = 0; i < _m_lMoveChildCreatureControlList.Count; i++)
                _m_lMoveChildCreatureControlList[i].getMoveStateMachine().tryChgToState(new ALDropCharacterMoveState(this, _direction, _moveSpeed));
        }

        /*******************
         * 重置对象位置坐标
         **/
        public void resetPosition(Vector3 _pos)
        {
            Vector3 groundPos = ALUnityCommon.getInstanceGroundPoint(_pos);

            transform.position = groundPos;
        }

        /*********************
         * 注册对应行为位移事件的创建对象
         **/
        public void regMovementFactory(_IActionMovementFactory _factory)
        {
            _m_amfMovementFactory = _factory;
        }

        /******************
         * 提取出当前的行为位移操作目标位置
         **/
        public ActionMovementInfo popMovementFactory()
        {
            if (null == _m_amfMovementFactory)
                return null;

            return _m_amfMovementFactory.popMovementInfo();
        }

        /************************
         * 释放所有本对象的相关资源，如动作，状态，附加物等
         **/
        public void discard()
        {
            //子对象进入对应状态
            for (int i = 0; i < _m_lMoveChildCreatureControlList.Count; i++)
            {
                _m_lMoveChildCreatureControlList[i].discard();
            }
            _m_lMoveChildCreatureControlList.Clear();

            //将本对象从移动关联父对象注销
            unregMoveFromParentControl();

            //重置行为与移动状态机到无效状态
            getMoveStateMachine().resetState(new ALNoneCharacterMoveState(this));
            getActionStateMachine().resetState(new ALNoneActionState(this));
            //释放所有行为对象
            getActionControler().discard();
            //删除所有其他动作
            getAnimationControler().discard();
            //删除所有添加物
            additionObjMgr.discard();

            //释放每帧确认检查对象
            getFrameCheckerMgr().discard();

            //清空骨骼管理对象
            getBoneMgr().discard();

            //清空附加队列
            _m_lLaterEventInfoList.Clear();
        }

        /*************************
         * 根据角色当前移动状态设置角色移动方向以及移动角色
         **/
        protected void _dealMoveDirection()
        {
            /***********
             * 处理旋转操作
             **/
            if (_m_qTargetRotation != transform.rotation)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, _m_qTargetRotation, _m_fSlerpSpeed * Time.deltaTime);
            }

            //处理每帧操作函数
            getMoveStateMachine().onUpdate();

            //处理子对象
            for (int i = 0; i < _m_lMoveChildCreatureControlList.Count; i++)
                _m_lMoveChildCreatureControlList[i].getMoveStateMachine().onUpdate();
        }

        /************************
         * 创建对象的行为控制状态机
         **/
        protected abstract _AALBasicActionStateMachine _createCharacterActionStateMachine();
    }
}
#endif

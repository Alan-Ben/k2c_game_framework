using System;

/************************
 * 游戏架构下的一个附加ui场景对象
 * 附加ui场景的意思是不存在互斥的状态，本ui场景可多次加载
 **/
namespace ALPackage
{
    public abstract class _AALBasicAdditionScene
    {
        /** scene的类型id */
        private int _m_iSceneTypeId;

        /** 进入本场景的次数统计信息 */
        private int _m_iEnterCount;
        /** 表示本场景是否已经初始化完成 */
        private bool _m_bIsInited;

        /** 在进入本场景后调用的进入回调函数 */
        private Action _m_dEnterSceneDelegate;
        private Action _m_dQuitSceneDelegate;
        private Action _m_dInitedDelegate;

        public _AALBasicAdditionScene(int _sceneTypeId)
        {
            _m_iSceneTypeId = _sceneTypeId;

            //默认为未进入场景
            _m_bIsInited = false;

            _m_dEnterSceneDelegate = null;
            _m_dQuitSceneDelegate = null;
            _m_dInitedDelegate = null;

            _m_iEnterCount = 0;
        }

        public int sceneTypeId { get { return _m_iSceneTypeId; } }
        public bool isEntered { get { return (_m_iEnterCount > 0); } }
        public bool isInited { get { return _m_bIsInited; } }
        protected int enterCount{ get { return _m_iEnterCount; } }

        /******************
         * 进入本场景的处理操作
         **/
        public void enterScene()
        {
            if (_AALMonoMain.instance.showDebugOutput)
            {
                if(isEntered)
                {
                    UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[Add_Scene] enterScene fake {this.ToString()}");
                }
                else
                {
                    UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[Add_Scene] enterScene real {this.ToString()}");
                }
            }

            //判断已经进入的次数是否超出0，是则表示已经进入场景，此时累加进入次数
            if (isEntered)
            {
                _m_iEnterCount++;
                return;
            }

            //设置进入次数为1
            _m_iEnterCount = 1;

            //向ui scene管理对象中注册本对象
            ALSceneCore.instance._regAdditionScene(this);

            //调用事件函数
            _onEnterScene();

            //调用回调函数
            if (null != _m_dEnterSceneDelegate)
            {
                _m_dEnterSceneDelegate();
                //重置回调
                _m_dEnterSceneDelegate = null;
            }

           
        }

        /*****************
         * 尝试退出本场景
         **/
        public void quitScene()
        {
            //减少进入次数
            _m_iEnterCount--;

            if(_m_iEnterCount > 0)
            {
                if (_AALMonoMain.instance.showDebugOutput)
                {
                    UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[Add_Scene] quitScene fake, count {_m_iEnterCount}");
                }
                return;
            }

            if (_m_iEnterCount < 0)
            {
                //不加类名太难找了，报了错但是不知道是哪里的
#if UNITY_EDITOR
                UnityEngine.Debug.LogError("[EDITOR]Quit an addition scene when it is not entered!" + this);
#endif
                _m_iEnterCount = 0;
                return;
            }

            //调用退出场景的操作
            _quitScene();
        }

        /*****************
         * 尝试退出本场景
         **/
        public void forceQuitAddScene()
        {
            if(_m_iEnterCount <= 0)
            {
                return;
            }

            //减少进入次数
            _m_iEnterCount = 0;

            //调用退出场景的操作
            _quitScene();
        }

        /******************
         * 设置场景已经初始化完成
         **/
        public void setSceneInited()
        {
            if (_AALMonoMain.instance.showDebugOutput)
            {
                if(_m_bIsInited || !isEntered)
                {
                    UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[Add_Scene] setSceneInited fake {this.ToString()}");
                }
                else
                {
                    UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[Add_Scene] setSceneInited real {this.ToString()}");
                }
            }

            //已经初始化完成或还未进入则不进行处理
            if (_m_bIsInited || !isEntered)
                return;

            //设置初始化完成
            _m_bIsInited = true;

            //调用事件函数
            _onSceneInited();

            Action initAct = _m_dInitedDelegate;
            //重置回调
            _m_dInitedDelegate = null;
            //调用回调处理
            if (null != initAct)
            {
                initAct();
                initAct = null;
            }
        }

        /*****************
         * 退出本场景，本函数不对外开放，只需要进入下一个游戏场景，前一个场景自然会退出
         **/
        protected internal void _quitScene()
        {
            if (_AALMonoMain.instance.showDebugOutput)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[Add_Scene] quitScene real {this.ToString()}");
            }

            //从ui scene管理对象中删除本对象
            ALSceneCore.instance._unregAdditionScene(this);

            //调用回调函数
            Action quitDelegate = _m_dQuitSceneDelegate;
            //重置所有回调
            _m_dQuitSceneDelegate = null;
            _m_dEnterSceneDelegate = null;
            _m_dInitedDelegate = null;
            
            //调用事件函数
            _onQuitScene();

            //设置初始化状态
            _m_bIsInited = false;
            _m_iEnterCount = 0;

            //调用外部注册事件函数
            if (null != quitDelegate)
            {
                quitDelegate();
            }

            //处理资源回收
            //ALMemCollectMgr.instance.collect();
        }

        /*******************
         * 注册回调函数
         **/
        public void regEnterDelegate(Action _enterDelegate)
        {
            if (null == _enterDelegate)
                return;

            if (isEntered)
            {
                //如已经完成则直接处理
                _enterDelegate();
            }
            else
            {
                //未完成则注册回调
                if (null == _m_dEnterSceneDelegate)
                {
                    _m_dEnterSceneDelegate = _enterDelegate;
                }
                else
                {
                    //添加回调对象
                    _m_dEnterSceneDelegate += _enterDelegate;
                }
            }
        }
        public void regQuitDelegate(Action _quitDelegate)
        {
            if (null == _quitDelegate)
                return;

            if (!isEntered)
            {
                //如已经完成则直接处理
                _quitDelegate();
            }
            else
            {
                //未完成则注册回调
                if (null == _m_dQuitSceneDelegate)
                {
                    _m_dQuitSceneDelegate = _quitDelegate;
                }
                else
                {
                    //添加回调对象
                    _m_dQuitSceneDelegate += _quitDelegate;
                }
            }
        }
        public void regInitDelegate(Action _initDelegate)
        {
            if (null == _initDelegate)
                return;

            if (_m_bIsInited)
            {
                //如已经完成则直接处理
                _initDelegate();
            }
            else
            {
                //未完成则注册回调
                if (null == _m_dInitedDelegate)
                {
                    _m_dInitedDelegate = _initDelegate;
                }
                else
                {
                    //添加回调对象
                    _m_dInitedDelegate += _initDelegate;
                }
            }
        }

        /** 在进入本场景时调用的事件函数 */
        protected abstract void _onEnterScene();

        /** 在初始化本场景完成后调用的事件函数 */
        protected abstract void _onSceneInited();

        /** 在退出本场景时调用的事件函数 */
        protected abstract void _onQuitScene();
    }
}

using System;

/************************
 * 游戏架构下的一个场景对象（根据带入的id进行区分，每个id的scene都只有一个独立对象和多个附加对象
 * 一个场景对象用于控制对应窗口的显隐，并在退出本场景时对窗口进行操作，重置和释放必要的对象
 **/
namespace ALPackage
{
    public abstract class _AALBasicScene
    {
        /** scene的类型id */
        private int _m_iSceneTypeId;

        /** 是否已经进入了本场景 */
        private bool _m_bIsEntered;
        /** 表示本场景是否已经初始化完成 */
        private bool _m_bIsInited;

        /** 在进入本场景后调用的进入回调函数 */
        private Action _m_dEnterSceneDelegate;
        private Action _m_dQuitSceneDelegate;
        private Action _m_dInitedDelegate;

        public _AALBasicScene(int _sceneTypeId)
        {
            _m_iSceneTypeId = _sceneTypeId;

            //默认为未进入场景
            _m_bIsEntered = false;
            _m_bIsInited = false;

            _m_dEnterSceneDelegate = null;
            _m_dQuitSceneDelegate = null;
            _m_dInitedDelegate = null;
        }

        public int sceneTypeId { get { return _m_iSceneTypeId; } }
        public bool isEntered { get { return _m_bIsEntered; } }
        public bool isInited { get { return _m_bIsInited; } }

        /******************
         * 进入本场景的处理操作
         **/
        public void enterScene()
        {
            if (_AALMonoMain.instance.showDebugOutput)
            {
                if(_m_bIsEntered)
                {
                    UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[{this.GetType().Name}] enterScene fake");
                }
                else
                {
                    UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[{this.GetType().Name}] enterScene real");
                }
            }

            //已经进入则不再次进入
            if (_m_bIsEntered)
                return;
    
            _m_bIsEntered = true;

            //设置当前ui场景对象
            ALSceneCore.instance._setCurScene(_m_iSceneTypeId, this);

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

        /******************
         * 设置场景已经初始化完成
         **/
        public virtual void setSceneInited()
        {
            if (_AALMonoMain.instance.showDebugOutput)
            {
                if(_m_bIsInited || !_m_bIsEntered)
                {
                    UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[{this.GetType().Name}] setInitDone fake");
                }
                else
                {
                    UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[{this.GetType().Name}] setInitDone real");
                }
            }

            //已经初始化完成或还未进入则不进行处理
            if (_m_bIsInited || !_m_bIsEntered)
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
                if(!_m_bIsEntered)
                {
                    UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[{this.GetType().Name}] _quitScene fake");
                }
                else
                {
                    UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[{this.GetType().Name}] _quitScene real");
                }
            }

            //还未进入场景则不处理
            if (!_m_bIsEntered)
                return;

            Action initAct = _m_dInitedDelegate;
            //重置回调
            _m_dInitedDelegate = null;
            //调用一次 init done 回调，避免流程断开
            if (null != initAct)
            {
                initAct();
                initAct = null;
            }

            //设置为未进入场景
            _m_bIsEntered = false;
            //设置初始化状态
            _m_bIsInited = false;

            //调用事件函数
            _onQuitScene();

            //调用回调函数
            if (null != _m_dQuitSceneDelegate)
            {
                _m_dQuitSceneDelegate();
                //重置回调
                _m_dQuitSceneDelegate = null;
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

            if (_m_bIsEntered)
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

            if (!_m_bIsEntered)
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

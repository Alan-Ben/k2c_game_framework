using System;

/************************
 * 游戏架构下的一个场景对象
 * 一个场景对象一般由游戏场景部分和ui部分构成
 * 本对象作为场景的基类对象
 **/
namespace ALPackage
{
    public abstract class _AALBasicGameStage
    {
        /** 是否已经进入了本场景 */
        private bool _m_bIsEntered;
        /** 表示本场景是否已经初始化完成 */
        private bool _m_bIsInited;

        /** 在进入本场景后调用的进入回调函数 */
        private Action _m_dEnterStageDelegate;
        private Action _m_dQuitStageDelegate;
        private Action _m_dInitedDelegate;

        public _AALBasicGameStage()
        {
            //默认为未进入场景
            _m_bIsEntered = false;
            _m_bIsInited = false;

            _m_dEnterStageDelegate = null;
            _m_dQuitStageDelegate = null;
            _m_dInitedDelegate = null;
        }

        public bool isEntered { get { return _m_bIsEntered; } }
        public bool isInited { get { return _m_bIsInited; } }

        /******************
         * 进入本场景的处理操作
         **/
        public void enterStage()
        {
            //已经进入则不再次进入
            if (_m_bIsEntered)
                return;

            _m_bIsEntered = true;

            //设置当前场景对象
            ALGameStageCore.instance._setCurGameStage(this);

            //调用事件函数
            _onEnterStage();

            //调用回调函数
            if (null != _m_dEnterStageDelegate)
            {
                _m_dEnterStageDelegate();
                //重置回调
                _m_dEnterStageDelegate = null;
            }
        }

        /******************
         * 设置场景已经初始化完成
         **/
        public virtual void setStageInited()
        {
            //已经初始化完成或还未进入则不进行处理
            if (_m_bIsInited || !_m_bIsEntered)
                return;

            //设置初始化完成
            _m_bIsInited = true;

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
        protected internal void _quitStage()
        {
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

            //调用事件函数
            _onQuitStage();

            //设置为未进入场景
            _m_bIsEntered = false;
            //设置初始化状态
            _m_bIsInited = false;

            //调用回调函数
            if (null != _m_dQuitStageDelegate)
            {
                _m_dQuitStageDelegate();
                //重置回调
                _m_dQuitStageDelegate = null;
            }

            //处理资源回收
            ALMemCollectMgr.instance.collect();
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
                if (null == _m_dEnterStageDelegate)
                {
                    _m_dEnterStageDelegate = _enterDelegate;
                }
                else
                {
                    //添加回调对象
                    _m_dEnterStageDelegate += _enterDelegate;
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
                if (null == _m_dQuitStageDelegate)
                {
                    _m_dQuitStageDelegate = _quitDelegate;
                }
                else
                {
                    //添加回调对象
                    _m_dQuitStageDelegate += _quitDelegate;
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
        protected abstract void _onEnterStage();

        /** 在退出本场景时调用的事件函数 */
        protected abstract void _onQuitStage();
    }
}

using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

/*****************
 * 单个scene的相关信息
 **/
namespace ALPackage
{
    public class ALSceneInfo
    {
        /** 场景本地序列号 */
        private int _m_iSceneSerialize;

        /** 进入本场景的次数统计信息 */
        private int _m_iEnterCount;

        /** 具体的场景对象 */
        private Scene _m_sSceneObj;
        /** 场景名称 */
        private string _m_sSceneName;
        /** 当前场景是否加载完成 */
        private bool _m_bIsSceneLoaded;
        /** 当前场景是否有效 */
        private bool _m_bIsSceneEnable;

        /** 场景加载类型 */
        private LoadSceneMode _m_eSceneLoadMode;

        /** 在进入本场景后调用的进入回调函数 */
        private Action<float> _m_dEnterProcessDelegate;
        private Action<ALSceneInfo> _m_dInitSceneDelegate;
        private Action _m_dQuitSceneDelegate;

        public ALSceneInfo(int _serialize, string _name, LoadSceneMode _sceneLoadMode)
        {
            _m_iSceneSerialize = _serialize;
            _m_sSceneName = _name;
            
            _m_bIsSceneLoaded = false;
            _m_bIsSceneEnable = true;

            _m_eSceneLoadMode = _sceneLoadMode;

            _m_iEnterCount = 0;
            _m_dEnterProcessDelegate = null;
            _m_dInitSceneDelegate = null;
            _m_dQuitSceneDelegate = null;
        }

        public int sceneSerialize { get { return _m_iSceneSerialize; } }
        public Scene sceneObj { get { return _m_sSceneObj; } }
        public string sceneName { get { return _m_sSceneName; } }
        public bool isSceneLoaded { get { return _m_bIsSceneLoaded; } }
        public bool isSceneEnable { get { return _m_bIsSceneEnable; } }

        public bool isEntered { get { return _m_iEnterCount > 0; } }

        #region 回调处理部分
        /// <summary>
        /// 回调注册处理
        /// </summary>
        public void regEnterProcessDelegate(Action<float> _enterProcessDelegate)
        {
            if(null == _enterProcessDelegate)
                return;

            if(isSceneLoaded)
            {
                //如已经完成则直接处理
                _enterProcessDelegate(1f);
            }
            else
            {
                //未完成则注册回调
                if(null == _m_dEnterProcessDelegate)
                {
                    _m_dEnterProcessDelegate = _enterProcessDelegate;
                }
                else
                {
                    //添加回调对象
                    _m_dEnterProcessDelegate += _enterProcessDelegate;
                }
            }
        }
        public void unregEnterProcessDelegate(Action<float> _enterProcessDelegate)
        {
            if(null == _enterProcessDelegate)
                return;

            //未完成则注册回调
            if(null == _m_dEnterProcessDelegate)
                return;

            _m_dEnterProcessDelegate -= _enterProcessDelegate;
        }

        public void regInitedDelegate(Action<ALSceneInfo> _initedDelegate)
        {
            if(null == _initedDelegate)
                return;

            if(isSceneLoaded)
            {
                //如已经完成则直接处理
                _initedDelegate(this);
            }
            else
            {
                //未完成则注册回调
                if(null == _m_dInitSceneDelegate)
                {
                    _m_dInitSceneDelegate = _initedDelegate;
                }
                else
                {
                    //添加回调对象
                    _m_dInitSceneDelegate += _initedDelegate;
                }
            }
        }
        public void unregInitedDelegate(Action<ALSceneInfo> _initedDelegate)
        {
            if(null == _initedDelegate)
                return;

            if(null == _m_dInitSceneDelegate)
                return;

            _m_dInitSceneDelegate -= _initedDelegate;
        }

        public void regQuitDelegate(Action _quitDelegate)
        {
            if(null == _quitDelegate)
                return;

            if(!isEntered)
            {
                //如已经完成则直接处理
                _quitDelegate();
            }
            else
            {
                //未完成则注册回调
                if(null == _m_dQuitSceneDelegate)
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
        public void unregQuitDelegate(Action _quitDelegate)
        {
            if(null == _quitDelegate)
                return;

            if(null == _m_dQuitSceneDelegate)
                return;

            _m_dQuitSceneDelegate -= _quitDelegate;
        }
        #endregion

        /// <summary>
        /// 进行加载操作，带入对应的监控过程对象
        /// </summary>
        /// <param name="_asyncOp"></param>
        /// <param name="_processDelegate"></param>
        /// <param name="_doneDelegate"></param>
        public void loadScene(AsyncOperation _asyncOp, Action<float> _processDelegate = null, Action<ALSceneInfo> _doneDelegate = null)
        {
            //判断是否已经进入，是则累加
            if(isEntered)
            {
                _m_iEnterCount++;
                //注册回调
                regEnterProcessDelegate(_processDelegate);
                regInitedDelegate(_doneDelegate);

                return;
            }

            if(null == _asyncOp)
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogError("加载AddTdScene的操作对象异常！");
#endif
                return;
            }

            //设置进入次数1
            _m_iEnterCount = 1;

            //注册回调
            regEnterProcessDelegate(_processDelegate);
            regInitedDelegate(_doneDelegate);

            //开启新场景加载监控
            ALCoroutineDealerMgr.instance.addCoroutine(new ALCoroutineAsynLoadSceneMonitor(this, _asyncOp));
        }

        /// <summary>
        /// 在同步加载scene之后调用的加载处理
        /// </summary>
        public void loadScene(Scene _scene, Action<ALSceneInfo> _doneDelegate = null)
        {
            //判断是否已经进入，是则累加
            if(isEntered)
            {
                _m_iEnterCount++;
                //注册回调
                regInitedDelegate(_doneDelegate);

                return;
            }

            if(null == _scene)
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogError("加载AddTdScene的操作对象异常！");
#endif
                return;
            }

            //设置进入次数1
            _m_iEnterCount = 1;

            //注册回调
            regInitedDelegate(_doneDelegate);

            //开启新场景加载监控
            ALCoroutineDealerMgr.instance.addCoroutine(new ALCoroutineSynLoadSceneMonitor(this, _scene));
        }

        /// <summary>
        /// 加载过程处理
        /// </summary>
        public void onLoadProcess(float _process)
        {
            if(null != _m_dEnterProcessDelegate)
                _m_dEnterProcessDelegate(_process);
        }

        /**********************
         * 设置场景对象
         **/
        public bool setSceneObj(Scene _sceneObj)
        {
            //判断是否已经设置，是则返回
            if (_m_bIsSceneLoaded)
            {
                Debug.LogError("set a scene when it is loaded!");
                return false;
            }

            if(null == _sceneObj)
                return false;

            if(null != _m_dEnterProcessDelegate)
                _m_dEnterProcessDelegate(1f);
            _m_dEnterProcessDelegate = null;

            //判断是否有效，无效则返回
            if (!_m_bIsSceneEnable)
                return false;

            _m_bIsSceneLoaded = true;
            _m_sSceneObj = _sceneObj;

            Action<ALSceneInfo> initAct = _m_dInitSceneDelegate;
            //重置回调
            _m_dInitSceneDelegate = null; ;
            //调用回调处理
            if(null != initAct)
            {
                initAct(this);
                initAct = null;
            }

            return true;
        }

        /// <summary>
        /// 加载场景失败的处理
        /// </summary>
        public void onLoadFail()
        {
            //判断是否已经设置，是则返回
            if(_m_bIsSceneLoaded)
            {
                Debug.LogError("set a scene when it is loaded!");
                return ;
            }

            if(null != _m_dEnterProcessDelegate)
                _m_dEnterProcessDelegate(1f);
            _m_dEnterProcessDelegate = null;

            Action<ALSceneInfo> initAct = _m_dInitSceneDelegate;
            //重置回调
            _m_dInitSceneDelegate = null; ;
            //调用回调处理
            if(null != initAct)
            {
                initAct(this);
                initAct = null;
            }

            //判断是否有效，无效则返回
            if(!_m_bIsSceneEnable)
                return ;

            _m_bIsSceneLoaded = true;
        }

        /// <summary>
        /// 释放相关资源
        /// </summary>
        public void discard()
        {
            //减少进入次数
            _m_iEnterCount--;

            if(_m_iEnterCount > 0)
                return;

            if(_m_iEnterCount < 0)
            {
                //不加类名太难找了，报了错但是不知道是哪里的
#if UNITY_EDITOR
                UnityEngine.Debug.LogError("退出一个TD场景超出进入次数!" + this);
#endif
                _m_iEnterCount = 0;
                return;
            }

            //处理实际的退出操作
            _dealDiscard();
        }
        /// <summary>
        /// 不论进入几次，都强制退出场景
        /// </summary>
        public void forceDiscard()
        {
            if(_m_iEnterCount < 0)
                return;

            _m_iEnterCount = 0;
            //处理实际的退出操作
            _dealDiscard();
        }

        /// <summary>
        /// 处理实际的退出操作行为
        /// </summary>
        protected void _dealDiscard()
        {
            _m_bIsSceneEnable = false;

            if(_m_bIsSceneLoaded)
            {
                //卸载场景，只有附加模式才强制卸载，如是single的则等待另外一个加载的时候会自动进行处理
                if(_m_eSceneLoadMode == LoadSceneMode.Additive)
                {
                    AsyncOperation asyncOp = SceneManager.UnloadSceneAsync(_m_sSceneObj);
                    if(_AALMonoMain.instance.showDebugOutput && ALSOGlobalSetting.Instance.logLevel <= ALLogLevel.DEBUG)
                    {
                        asyncOp.completed += _operation =>
                        {
                            UnityEngine.Debug.Log($"【{Time.frameCount}】[Asset]Scene unloaded: {_m_sSceneName}");
                        };
                    }
                }
                //SceneManager.UnloadScene(_m_sSceneObj);
                _m_bIsSceneLoaded = false;
            }

            //从场景中删除本信息对象
            ALSceneMgr.instance._removeSceneInfo(this);

            //调用回调函数
            Action quitDelegate = _m_dQuitSceneDelegate;
            //重置所有回调
            _m_dQuitSceneDelegate = null;
            _m_dEnterProcessDelegate = null;
            _m_dInitSceneDelegate = null;

            if(null != quitDelegate)
            {
                quitDelegate();
            }
        }
    }
}

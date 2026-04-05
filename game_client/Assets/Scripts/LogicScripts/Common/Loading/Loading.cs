
using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 管理 NP 的加载画面的接口
    /// </summary>
    public static class Loading
    {
        // 加载任务的 delegate 类型
        public delegate void LoadFunction(Action _complete);
        
        // 当前的加载展示对象和当前是否有展示
        private static _ILoadingShow _g_curLoadingShow;
        private static bool _g_isShow;
        private static bool _g_isShowDone;
        //是否还在表现种，只要显示过程，或者隐藏动画没播放完，都算显示过程
        private static bool _g_isInShowing;
        private static int _g_loadingCount;

        // 每次 force 暂停后的序列号变化
        private static int _g_serialize;

        // 当前需要加载的任务
        [NotNull] private static LoadTasks _g_loadTasks = new LoadTasks();
        // 完全隐藏加载界面的回调
        private static Action _m_hideDoneDelegate = default(Action);
        
        /// <summary>
        /// 显示加载界面
        /// </summary>
        public static void showLoading(LoadFunction _loadFunc)
        {
            showLoading(NPGGUIWndLoading.instance, _loadFunc);
        }
        /// <summary>
        /// 显示加载界面
        /// </summary>
        public static void showLoading(_ILoadingShow _loadingShow, LoadFunction _loadFunc)
        {
            // _loadFunc 为 null 就什么都不用做
            if (_loadFunc == null)
                return;
            
            // 如果当前有正在显示的接口就不赋值，如果传入的是 null 就使用默认的加载窗口
            _g_curLoadingShow ??= _loadingShow ?? NPGGUIWndLoading.instance;
            // 把要加载的任务加入到
            _g_loadTasks.addLoadFunction(_loadFunc);
            // 判断当前的
            _checkAndShowOrHide();
        }
        
        public static void forceHideLoading()
        {
            if (!_g_isShow)
                return;

            _g_curLoadingShow?.hide(() =>
            {
                _g_isInShowing = false;
                _dealHideDoneDelegate();
            });
            _g_curLoadingShow = null;
        
            _g_isShow = false;
            _g_isShowDone = false;
            _g_loadingCount = 0;
            
            QueueMgr.instance.OpenRollBack(NodeESC_Const.C_QUEUE_ESC_LOADING);
        
            _g_serialize = ALSerializeOpMgr.next();
            
            // 把还屯着的加载任务都直接加载了
            _g_loadTasks.deal(null);
        }
        
        public static void regHideDoneDelegate(Action _hideDoneDelegate)
        {
            //当前不在显示并且没有再表现过程，隐藏动画也完成了，直接执行回调
            if (!_g_isShow && !_g_isInShowing)
            {
                _hideDoneDelegate?.Invoke();
                return;
            }
            
            _m_hideDoneDelegate += _hideDoneDelegate;
        }
        
        private static void _dealHideDoneDelegate()
        {
            //处理回调，先赋值后处理，避免过程中回调被删除
            Action doneDelegate = _m_hideDoneDelegate;
            _m_hideDoneDelegate = null;
            if (null != doneDelegate)
                doneDelegate();
            doneDelegate = null;
        }
        
        private static void _checkAndShowOrHide()
        {
            int serialize = _g_serialize;
            // 如果当前有加载任务，并且未显示
            if (_g_loadTasks.dealFunctionCount > 0 && !_g_isShow)
            {
                // 表示当前已经进入显示流程了
                _g_isShow = true;
                _g_isInShowing = true;

                Action dealAction = () =>
                    {
                        if (serialize != _g_serialize)
                            return;

                        _g_isShowDone = true;
                        _g_loadingCount++;
                        _g_loadTasks.deal(() =>
                        {
                            if (serialize != _g_serialize)
                                return;

                            _g_loadingCount--;
                            _checkAndShowOrHide();
                        });
                    };

                //判断游戏资源是否初始化完成
                if (GameInit_UpdateGameRes.instance.isDone)
                {
                    // 开始处理流程，等加载展示展示结束之后再开始加载任务
                    _g_curLoadingShow.show(dealAction);
                }
                else
                {
                    //如果资源未初始化，直接调用结果
                    dealAction();
                }
                
                QueueMgr.instance.CloseRollBack(NodeESC_Const.C_QUEUE_ESC_LOADING);
            }
            // 如果当前还有加载任务，并且已经进入了显示状态，就直接执行加载任务
            else if (_g_loadTasks.dealFunctionCount > 0 && _g_isShowDone)
            {
                _g_loadingCount++;
                _g_loadTasks.deal(() =>
                {
                    if (serialize != _g_serialize)
                        return;

                    _g_loadingCount--;
                    _checkAndShowOrHide();
                });
            }
            // 如果没有任务要加载了，并且当前正在显示中，就进入隐藏流程
            else if (_g_loadingCount == 0 && _g_loadTasks.dealFunctionCount <= 0 && _g_isShow)
            {
                _g_isShow = false;
                _g_isShowDone = false;

                Action dealAction = () =>
                    {
                        if (serialize != _g_serialize)
                            return;

                        _g_isInShowing = false;
                        _dealHideDoneDelegate();

                        _checkAndShowOrHide();
                    };

                //判断游戏资源是否初始化完成
                if (GameInit_UpdateGameRes.instance.isDone)
                {
                    _g_curLoadingShow.hide(dealAction);
                    _g_curLoadingShow = null;
                }
                else
                {
                    //如果资源未初始化，直接调用结果
                    dealAction();
                }
                
                QueueMgr.instance.OpenRollBack(NodeESC_Const.C_QUEUE_ESC_LOADING);
            }
        }

        private class LoadTasks
        {
            [ItemNotNull][NotNull] private readonly List<LoadFunction> _m_loadFunctions;

            public LoadTasks()
            {
                _m_loadFunctions = new List<LoadFunction>();
            }
            
            public int dealFunctionCount { get { return _m_loadFunctions.Count; } }

            public void addLoadFunction(LoadFunction _loadFunction)
            {
                if (_loadFunction == null)
                    return;
                
                _m_loadFunctions.Add(_loadFunction);
            }

            public void deal(Action _complete)
            {
                if (_m_loadFunctions.Count <= 0)
                {
                    _complete?.Invoke();
                    return;
                }

                List<LoadFunction> loadFunctions = new List<LoadFunction>(_m_loadFunctions);
                _m_loadFunctions.Clear();

                ALStepCounter stepCounter = new ALStepCounter();
                stepCounter.chgTotalStepCount(loadFunctions.Count);
                stepCounter.regAllDoneDelegate(_complete);

                foreach (LoadFunction function in loadFunctions)
                {
                    function.Invoke(stepCounter.addDoneStepCount);
                }
            }
        }
    }
}
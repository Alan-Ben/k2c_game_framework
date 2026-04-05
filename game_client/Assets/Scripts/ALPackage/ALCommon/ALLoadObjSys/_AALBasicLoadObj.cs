using System;

using UnityEngine;

/*******************
 * 加载系统的基本加载对象，实现了加载对象与管理对象之间的关联部分
 **/
namespace ALPackage
{
    public abstract class _AALBasicLoadObj
    {
        /** 加载完成时调用的函数 */
        private Action _m_dOnLoadDone = null;
        /** 是否开始初始化 */
        private bool _m_bIsInited = false;
        /** 是否已经加载完成 */
        private bool _m_bIsLoaded = false;
        /** 加载请求的统计数量，避免多次加载后多次卸载产生的问题 */
        private int _m_iLoadCount = 0;

        public bool isLoaded { get { return _m_bIsLoaded; } }
        
        /// <summary>
        /// 是否开始初始化
        /// </summary>
        public bool isInited { get { return _m_bIsInited; } }

        /****************
         * 加载窗口的操作
         **/
        public void load()
        {
            load(null);
        }
        public void load(Action _delegate)
        {
            if (_AALMonoMain.instance.showDebugOutput && ALSOGlobalSetting.Instance.logLevel <= ALLogLevel.VERBOSE)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[Load][{this.GetType().Name}] load.");
            }
            
            //增加加载请求
            _m_iLoadCount++;

            //判断是否已经初始化完成
            if (_m_bIsLoaded)
            {
                if (null != _delegate)
                    _delegate();
                return;
            }

            //注册回调
            regLoadDoneDelegate(_delegate);

            if (!_m_bIsInited)
            {
                //进行加载操作
                _loadOp();

                //设置开始加载
                _m_bIsInited = true;
            }
        }

        /*****************
         * 注册初始化完成的回调函数
         **/
        public void regLoadDoneDelegate(Action _delegate)
        {
            if(null == _delegate)
                return;

            if(_m_bIsLoaded)
                _delegate();
            else if(null == _m_dOnLoadDone)
                _m_dOnLoadDone = _delegate;
            else
                _m_dOnLoadDone += _delegate;
        }

        /*****************
         * 注销初始化完成的回调函数
         **/
        public void unregLoadDoneDelegate(Action _delegate)
        {
            if(null == _delegate)
                return;

            //已经加载就直接返回
            if(_m_bIsLoaded)
                return;

            if(null == _m_dOnLoadDone)
                return;
            
            //删除回调
            _m_dOnLoadDone -= _delegate;
        }


        //强制释放窗口资源相关对象
        public void forceDiscard()
        {
            if (_AALMonoMain.instance.showDebugOutput && ALSOGlobalSetting.Instance.logLevel <= ALLogLevel.VERBOSE)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[Load][{this.GetType().Name}] forceDiscard.");
            }
            
            // 强制卸载，计数直接清空
            _m_iLoadCount = 0;

            //判断是否已经加载，还未加载不进行后续处理
            if (!_m_bIsLoaded)
                return;

            //调用事件函数
            _discard();

            //设置未加载完成
            _m_bIsLoaded = false;
            _m_bIsInited = false;
        }

        /***************
         * 释放窗口资源相关对象
         **/
        public void discard()
        {
            if (_AALMonoMain.instance.showDebugOutput && ALSOGlobalSetting.Instance.logLevel <= ALLogLevel.VERBOSE)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[Load][{this.GetType().Name}] discard.");
            }
            
            //减少加载统计
            _m_iLoadCount--;
            //判断数量是否仍然大于-0
            if (_m_iLoadCount > 0)
                return;
            //判断是否非法数值
            if (_m_iLoadCount < 0)
                _m_iLoadCount = 0;

            //判断是否已经加载，还未加载不进行后续处理
            if (!_m_bIsLoaded)
                return;

            //调用事件函数
            _discard();

            //设置未加载完成
            _m_bIsLoaded = false;
            _m_bIsInited = false;
        }

        /*****************
         * 设置窗口初始化完成
         **/
        protected void _setLoadDone()
        {
            //如果已经加载完毕则提示错误
            if (_m_bIsLoaded)
            {
                Debug.LogError("Set Load Done Multi times!");
                return;
            }

            //设置已经加载完成
            _m_bIsLoaded = true;

            //调用回调
            if (null != _m_dOnLoadDone)
                _m_dOnLoadDone();
            //清空回调对象
            _m_dOnLoadDone = null;

            //判断是否需要继续加载
            if (_m_iLoadCount <= 0)
            {
                //无效则释放内容
                _discard();

                //设置为未加载
                _m_bIsInited = false;
                _m_bIsLoaded = false;
                return;
            }
        }

        /******************
         * 显示窗口的事件函数
         **/
        protected abstract void _loadOp();
        /**********************
        * 实际的释放资源函数
        **/
        protected abstract void _discard();
    }
}

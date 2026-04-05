using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/********************
 * 对异步操作的监控对象
 **/
namespace ALPackage
{
    public class ALAssetBundleSceneAsynMonoitor : _IALBaseMonoTask
    {
        //结束时的回调对象
        private Action _m_dDoneDelegate;
        //过程中的进度回调
        private Action<float> _m_dProcessDelegate;

        /** 对应加载的对象 */
        private ALAssetBundleObj _m_aoAssetBundleObj;
        /** 异步处理对象的监控对象 */
        private AsyncOperation _m_aoAsyncOperationObj;

        public ALAssetBundleSceneAsynMonoitor(ALAssetBundleObj _assetbundleObj, AsyncOperation _opObj, Action<float> _processDelegate, Action _doneDelegate)
        {
            _m_aoAssetBundleObj = _assetbundleObj;
            _m_aoAsyncOperationObj = _opObj;
            _m_dProcessDelegate = _processDelegate;
            _m_dDoneDelegate = _doneDelegate;
        }

        /**************
         * 开启监控
         **/
        public void startMonitor()
        {
            //加载对象无效，直接当作已经完成
            if (null == _m_aoAsyncOperationObj)
            {
                if (null != _m_dDoneDelegate)
                    _m_dDoneDelegate();
                _m_dDoneDelegate = null;

                //先进行事件触发函数，避免回调出现错误
                //调用加载操作完成函数，减少操作，等待系统释放资源
                _m_aoAssetBundleObj.onLoadingOpDone();

                return;
            }

            ALMonoTaskMgr.instance.addMonoTask(this);
        }

        /*******************
         * 任务的执行体函数
         **/
        public void deal()
        {
            if (null == _m_aoAsyncOperationObj)
            {
                _m_aoAssetBundleObj.onLoadingOpDone();
                return;
            }

            //判断任务是否完成，是则调用完成函数，并不继续检测
            if (_m_aoAsyncOperationObj.isDone)
            {
                //最后一次调用进度函数
                if (null != _m_dProcessDelegate)
                    _m_dProcessDelegate(_m_aoAsyncOperationObj.progress);

                if (null != _m_dDoneDelegate)
                    _m_dDoneDelegate();
                _m_dDoneDelegate = null;

                //先进行事件触发函数，避免回调出现错误
                //调用加载操作完成函数，减少操作，等待系统释放资源
                _m_aoAssetBundleObj.onLoadingOpDone();

                return;
            }

            //未加载完成，调用过程函数
            if (null != _m_dProcessDelegate)
                _m_dProcessDelegate(_m_aoAsyncOperationObj.progress);
            //将任务加入到下一帧
            ALMonoTaskMgr.instance.addNextFrameTask(this);
        }
    }
}

using System;
using System.Threading;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

/*****************
 * 下载完补丁的资源文件并写入对应位置的处理对象
 **/
namespace ALPackage
{
    public abstract class _AALResourcePatchUpdateDelegate : _AALHttpDownloadDelegate
    {
        /** 对应资源管理对象 */
        protected _AALResourceCore _m_rcResourceCore;
        /** 资源对应的版本信息 */
        protected ALAssetBundleVersionInfo _m_viVersionInfo;

        /** 下载下来的文件本地路径 */
        private string _m_sDownloadlocalPath;

        public _AALResourcePatchUpdateDelegate(_AALResourceCore _resourceCore, ALAssetBundleVersionInfo _versionInfo)
            : base()
        {
            _m_rcResourceCore = _resourceCore;
            _m_viVersionInfo = _versionInfo;

            _m_sDownloadlocalPath = null;
        }

        public long totalUnzipSize { get { return _m_viVersionInfo.fileSize; } }

        /***************
         * 在资源下载完成时触发的回调处理
         **/
        protected override internal void _onHttpLoaded(string _localPath)
        {
            if (string.IsNullOrEmpty(_localPath))
            {
                //数据非法则直接完成
                _dealDelegateFail();
                return;
            }

            //开启线程写入文件,写入完成后调用完成函数
            _m_sDownloadlocalPath = _localPath;

            //开启线程进行处理
            ALThread writeThread = new ALThread(_writeFileThread);
            writeThread.startThread();
        }
        /***************
         * 在资源下载失败时触发的回调处理
         **/
        protected override internal void _onHttpFail()
        {
            //直接做完成处理
            _dealDelegateDone();
        }

        /*******************
         * 写入文件的线程记录
         **/
        protected void _writeFileThread()
        {
            try
            {
                //增加补丁对象的信息，带入设置进度的回调对象
                //使用resource core锁定，避免一个文件可能被多开线程的时候访问，导致文件IO出错
                lock(_m_rcResourceCore)
                {
                    _writeFileFunc(_m_viVersionInfo, _m_sDownloadlocalPath);
                }

                //删除字节信息
                _m_sDownloadlocalPath = null;

                //设置操作完成
                ALCommonActionMonoTask.addMonoTask(_dealDelegateDone);
            }
            catch(Exception _ex)
            {
                //增加文件写入报错
                UnityEngine.Debug.LogError($"_AALResourcePatchUpdateDelegate:{_ex.Message}");
                //报错则表示失败，注册任务进行后续处理
                ALCommonActionMonoTask.addMonoTask(_dealDelegateDone);
            }
        }

        /// <summary>
        /// 处理本回调处理完成的操作
        /// </summary>
        protected virtual void _dealDelegateDone()
        {
            _setDealDone();
        }

        /// <summary>
        /// 处理本回调处理失败的操作
        /// </summary>
        protected virtual void _dealDelegateFail()
        {
            _setDealFail();
        }

        /// <summary>
        /// 写入文件的处理函数
        /// </summary>
        /// <param name="_resourceCore"></param>
        /// <param name="_versionInfo"></param>
        protected abstract void _writeFileFunc(ALAssetBundleVersionInfo _versionInfo, string _localPath);
    }
}

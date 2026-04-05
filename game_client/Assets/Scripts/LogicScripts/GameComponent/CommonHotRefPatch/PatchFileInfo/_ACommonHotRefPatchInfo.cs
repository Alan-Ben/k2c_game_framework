using System;
using System.Collections.Generic;
using System.IO;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 基础的
    /// </summary>
    public abstract partial class _ACommonHotRefPatchInfo
    {
        public static readonly string g_sHotRefdataPath = $"{Application.persistentDataPath}/hot_refdata/";

        private long _m_serialId;//序列号

        //文件名
        private string _m_fileName;
        //文件md5
        private string _m_fileMd5;
        //文件路径
        private string _m_filePath;

        //是否已经打过补丁
        private bool _m_hasPatch;
        //是否正在下载
        private bool _m_isDownloading;
        //是否已生效
        private bool _m_isActivated;
        //补丁完成回调
        private Action<bool> _onPatchDoneDelegate;

        //获取本地文件路径
        private string patchFilePath { get { return $"{g_sHotRefdataPath}{_m_fileName}"; } }
        //url链接路径
        private string patchUrlPath { get { return $"{_m_filePath}{_m_fileName}"; } }

        protected _ACommonHotRefPatchInfo(string _fileName, string _fileMd5, string _filePath)
        {
            _m_fileName = _fileName;
            _m_fileMd5 = _fileMd5;
            _m_filePath = _filePath;
            _m_hasPatch = false;
            _m_serialId = ALSerializeOpMgr.next();
        }
        
        /// <summary>
        /// 下载校验文件
        /// </summary>
        public void startPatch(Action<bool> _onDone)
        {
            //判断是否需要打补丁，一般活动相关是等活动开了再打补丁
            if (!_checkCanPatch())
            {
                if (_onDone != null) 
                    _onDone(false);
            }
            
            //直接注册回调
            regDelegate(_onDone);
            
            //已经处理完了不处理
            if(_m_hasPatch)
                return;
            
            //如果本地文件有效则直接去激活
            if (_checkLocalFileValid())
            {
                _startActivated();
            }
            //本地文件无效则删除后重新下载
            else
            {
                //判断是否已经下载完成
                if (_m_isDownloading)
                    return;
                
                //删除本地文件
                _deleteLocalFile();
                //开始下载
                _startDownload(() =>
                {
                    //成功处理
                    //开始激活文件
                    _startActivated();
                    
                }, () =>
                {
                    _m_isActivated = false;
                    //失败处理
                    _setPatchDone();
                });
            }
        }
        
        /// <summary>
        /// 重置数据
        /// </summary>
        public void reset()
        {
            _m_serialId = ALSerializeOpMgr.next();
            _m_hasPatch = false;
            _m_isDownloading = false;
            _m_isActivated = false;
        }
        
        //开始激活打补丁
        private void _startActivated()
        {
            if(_m_isActivated && _m_hasPatch)
                return;
            
            //文件无效直接不处理
            if (!_checkLocalFileValid())
            {
                //删除本地文件
                _deleteLocalFile();
                _m_isActivated = false;
                //设置补丁完成，但是不成功
                _setPatchDone();
            }
            else
            {
                //文件有效开始打补丁
                bool _isSuc = _dealPatchFile();
                if(!_isSuc)
                {
                    UnityEngine.Debug.LogError($"配表补丁要补丁的表，失败，请检查，文件名：{_m_fileName}");
                }
                
                _m_isActivated = _isSuc;
                //设置补丁完成
                _setPatchDone();
            }
        }
        
        /// <summary>
        /// 注册回调
        /// </summary>
        /// <param name="_delegate"></param>
        public void regDelegate(Action<bool> _delegate)
        {
            if (null == _delegate)
                return;

            //判断是否已经完成，已完成直接设置回调
            if(_m_hasPatch)
            {
                if (null != _delegate)
                    _delegate(_m_isActivated);
                return;
            }

            if (null == _onPatchDoneDelegate)
                _onPatchDoneDelegate = _delegate;
            else
                _onPatchDoneDelegate += _delegate;
        }

        /// <summary>
        /// 设置补丁完成，此时将调用回调
        /// </summary>
        private void _setPatchDone()
        {
            _m_hasPatch = true;

            Action<bool> doneDelegate = _onPatchDoneDelegate;
            _onPatchDoneDelegate = null;
            if (null != doneDelegate)
                doneDelegate(_m_isActivated);
        }
        
        //检查本地文件有效性
        private bool _checkLocalFileValid()
        {
            if (string.IsNullOrEmpty(patchFilePath))
                return false;
            
            //文件不存在则为空
            FileInfo fileInfo = new FileInfo(patchFilePath);
            if (!fileInfo.Exists)
                return false;

            if (fileInfo.Length <= 0)//若文件长度小于等于0, 删除
                return false;

            if (!string.Equals(ALCommon.generateMD5(fileInfo), _m_fileMd5, StringComparison.InvariantCultureIgnoreCase))
                return false;
            
            return true;
        }
        
        //删除本地文件
        private void _deleteLocalFile()
        {
            try
            {
                //删除文件
                ALFile.Delete(patchFilePath);
            }
            catch(Exception)
            {
                //错误这里暂不处理
            }
        }

        //子类判断是否可以打补丁
        protected abstract bool _checkCanPatch();
    }
}
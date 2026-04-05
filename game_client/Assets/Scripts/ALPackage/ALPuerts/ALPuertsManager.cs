using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

#if AL_PUERTS
using Puerts;
#endif

namespace ALPackage
{
#if AL_PUERTS
    /// <summary>
    /// 需要通过本对象使用puerts，需要先使用本对象initPuertsAsset函数初始化puerts脚本相关
    /// 为了避免每次启动都进行初始化，需要在资源变更或更新的时候做此处理
    /// 
    /// 本管理类重载了ILoader对象，对puerts脚本进行加载，由此可以通过ResourceCore进行资源脚本的管理和调用
    /// </summary>
    public class ALPuertsManager : ILoader
    {
        private _AALResourceCore _m_rcResCore;
        /// <summary>
        /// 根节点目录名
        /// </summary>
        private string _m_sRootPath;
        /// <summary>
        /// 全局的Puerts环境
        /// </summary>
        private ALPuertsInstance _m_peGeneralPuertsInstance;
        /// <summary>
        /// puerts执行的环境集合
        /// </summary>
        private List<ALPuertsInstance> _m_pdPuertsDictionary;

        /// <summary>
        /// 子目录的版本管理对象
        /// </summary>
        private ALPuertsSubFolderVersionMgr _m_vmFolderVersionMgr;

        /// <summary>
        /// 每帧释放资源的处理任务
        /// </summary>
        private ALCommonEnableTaskController _m_tmTickTask;

        /// <summary>
        /// 基础构造函数
        /// </summary>
        protected ALPuertsManager(_AALResourceCore _resCore, string _rootPath)
        {
            _m_rcResCore = _resCore;
            _m_sRootPath = _rootPath;
            _m_peGeneralPuertsInstance = new ALPuertsInstance(string.Empty, this);

            _m_pdPuertsDictionary = new List<ALPuertsInstance>();

            _m_vmFolderVersionMgr = new ALPuertsSubFolderVersionMgr(this);
            _m_vmFolderVersionMgr.init();

            //开启每帧任务执行tick
            _m_tmTickTask = ALCommonEnableTickActionMonoTask.addMonoTask(tick);
        }

        public string rootPath { get { return _m_sRootPath; } }

        /// <summary>
        /// 强制系统更新对应目录的脚本
        /// </summary>
        /// <param name="_abName"></param>
        /// <param name="_subFolderName"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void forceUpdatePuertsScriptAsset(string _abName, string _subFolderName, Action _sucDelegate, Action _failDelegate)
        {
            //删除版本信息
            _m_vmFolderVersionMgr.removeFolderInfo(_subFolderName);

            //进行更新操作
            addPuertsScriptAsset(_abName, _subFolderName, _sucDelegate, _failDelegate);
        }

        /// <summary>
        /// 初始化puerts的asset，并将里面的puerts脚本写入对应位置
        /// </summary>
        /// <param name="_abName"></param>
        /// <param name="_doneDelegate"></param>
        public void addPuertsScriptAsset(string _abName, string _subFolderName, Action _sucDelegate, Action _failDelegate)
        {
            if(null == _m_rcResCore)
                return;

            //获取对应版本信息
            ALAssetBundleVersionInfo versionInfo = _m_rcResCore.getVersionInfo(_abName);
            //判断版本是否一致，如一致则不做后续处理
            if (null != versionInfo && _m_vmFolderVersionMgr.checkFolderVersion(_subFolderName, versionInfo.versionNum))
            {
#if UNITY_EDITOR
                ALLog.Warning($"Puerts [{_abName}] - Folder[{_subFolderName}] version is the newest!");
#endif
                //进行回调处理
                if (null != _sucDelegate)
                    _sucDelegate();
                return;
            }

#if UNITY_EDITOR
            ALLog.Sys($"Puerts [{_abName}] - Folder[{_subFolderName}] start update Scripts!");
#endif

            _m_rcResCore.loadAsset(_abName
                , (bool _isSuc, ALAssetBundleObj _assetObj) =>
                {
                    //判断是否加载成功
                    if(!_isSuc || null == _assetObj)
                    {
                        //报错
                        ALLog.Error($"Puerts [{_abName}] - Folder[{_subFolderName}] Update Fail! - Assetbundle load fail");
                        //进行回调处理
                        if(null != _sucDelegate)
                            _sucDelegate();
                        return;
                    }

                    //将数据中所有文件都导出到对应目录
                    string dicPath = $"{_m_rcResCore._patchInfo.patchLocalFullPath}/{_m_sRootPath}";
                    if(!string.IsNullOrEmpty(_subFolderName))
                    {
                        dicPath = dicPath + "/" + _subFolderName;
                    }
                    //检测文件夹是否存在，是则需要删除目录。
                    if(Directory.Exists(dicPath))
                    {
                        //删除目录下所有文件即可，不删子文件夹，避免嵌套目录被删除
                        ALCommon.DelectDirectoryFiles(dicPath);
                        //删除目录及子文件
                        //Directory.Delete(dicPath, true);
                    }
                    //创建文件夹
                    Directory.CreateDirectory(dicPath);

                    //逐个文件导出
                    TextAsset[] objs = _assetObj.assetBundle.LoadAllAssets<TextAsset>();
                    TextAsset tmpObj = null;
                    for(int i = 0; i < objs.Length; i++)
                    {
                        tmpObj = objs[i];
                        if(null == tmpObj)
                            continue;

                        //先删除文件
                        string tmpPath = dicPath + "/" + tmpObj.name;
                        if(File.Exists(tmpPath))
                            File.Delete(tmpPath);

                        //写入文件
                        StreamWriter writer = new StreamWriter(tmpPath);
                        if(null == writer)
                        {
                            ALLog.Error($"Puerts [{_abName}] - Folder[{_subFolderName}] Update Fail! - write file[{tmpObj.name}] fail");
                            //进行回调处理
                            if(null != _failDelegate)
                                _failDelegate();
                            return;
                        }

                        //写入文件
                        writer.Write(tmpObj.text);

                        //关闭文件
                        writer.Close();
                        writer.Dispose();
                    }

#if UNITY_EDITOR
                    ALLog.Sys($"Puerts [{_abName}] - Folder[{_subFolderName}] Update Success Done!");
#endif
                    //添加数据
                    _m_vmFolderVersionMgr.setFolderVersion(_subFolderName, versionInfo.versionNum);

                    if (null != _sucDelegate)
                        _sucDelegate();
                }
                , null);
        }

        /// <summary>
        /// 每帧执行的释放函数
        /// </summary>
        public void tick()
        {
            if(null != _m_peGeneralPuertsInstance)
                _m_peGeneralPuertsInstance.tick();

            //遍历每个实例对象执行 tick
            for(int i = 0; i < _m_pdPuertsDictionary.Count; i++)
            {
                _m_pdPuertsDictionary[i].tick();
            }
        }

        /// <summary>
        /// 释放本对象的所有环境
        /// </summary>
        public void discard()
        {
            _m_tmTickTask.setDisable();

            if(null != _m_peGeneralPuertsInstance)
                _m_peGeneralPuertsInstance.discard();
            _m_peGeneralPuertsInstance = null;

            //遍历每个实例对象执行 tick
            for (int i = 0; i < _m_pdPuertsDictionary.Count; i++)
            {
                _m_pdPuertsDictionary[i].discard();
            }
            _m_pdPuertsDictionary.Clear();
        }

        /// <summary>
        /// 在全局环境中执行带入的puerts脚本处理
        /// </summary>
        /// <param name="_puertsName"></param>
        public void execGeneralScripts(string _puertsStr)
        {
            if(null == _m_rcResCore)
                return ;

            //执行对应脚本
            if(null != _m_peGeneralPuertsInstance)
                _m_peGeneralPuertsInstance.doScripts(_puertsStr);
        }
        public TResult execGeneralScripts<TResult>(string _puertsStr, string _chunkName = "chunk")
        {
            if(null == _m_rcResCore)
                return default(TResult);

            //执行对应脚本
            if (null != _m_peGeneralPuertsInstance)
                return _m_peGeneralPuertsInstance.doScripts<TResult>(_puertsStr, _chunkName);
            else
                return default(TResult);
        }
        public void execGeneralScriptsFile(string _puertsFileName)
        {
            if (null == _m_rcResCore)
                return;

            //执行对应脚本
            if (null != _m_peGeneralPuertsInstance)
                _m_peGeneralPuertsInstance.doScripts($@"require('{_puertsFileName}')");
        }
        public TResult execGeneralScriptsFile<TResult>(string _puertsFileName, string _chunkName = "chunk")
        {
            if (null == _m_rcResCore)
                return default(TResult);

            //执行对应脚本
            if (null != _m_peGeneralPuertsInstance)
                return _m_peGeneralPuertsInstance.doScripts<TResult>($@"require('{_puertsFileName}')", _chunkName);
            else
                return default(TResult);
        }

        /** 暂不建议使用instance处理，先行屏蔽
        /// <summary>
        /// 在指定的puerts名称环境下执行语句
        /// </summary>
        /// <param name="_puertsName"></param>
        public void execInstanceScripts(string _instanceName, string _puertsStr)
        {
            if (null == _m_rcResCore)
                return;

            ALPuertsInstance puertsInstance = _getAndCreatePuertsInstance(_instanceName);
            //执行对应脚本
            if (null != puertsInstance)
                puertsInstance.doScripts(_puertsStr);
        }
        public TResult execInstanceScripts<TResult>(string _instanceName, string _puertsStr, string _chunkName = "chunk")
        {
            if (null == _m_rcResCore)
                return default(TResult);

            ALPuertsInstance puertsInstance = _getAndCreatePuertsInstance(_instanceName);
            //执行对应脚本
            if (null != puertsInstance)
                return puertsInstance.doScripts<TResult>(_puertsStr, _chunkName);
            else
                return default(TResult);
        }
        public void execInstanceScriptsFile(string _instanceName, string _puertsFileName)
        {
            if (null == _m_rcResCore)
                return;

            ALPuertsInstance puertsInstance = _getAndCreatePuertsInstance(_instanceName);
            //执行对应脚本
            if (null != puertsInstance)
                puertsInstance.doScripts($@"require('{_puertsFileName}')");
        }
        public TResult execInstanceScriptsFile<TResult>(string _instanceName, string _puertsFileName, string _chunkName = "chunk")
        {
            if (null == _m_rcResCore)
                return default(TResult);

            ALPuertsInstance puertsInstance = _getAndCreatePuertsInstance(_instanceName);
            //执行对应脚本
            if (null != puertsInstance)
                return puertsInstance.doScripts<TResult>($@"require('{_puertsFileName}')", _chunkName);
            else
                return default(TResult);
        } **/

        /// <summary>
        /// 重载接口，判断文件是否存在
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        protected string _getLocalFilePath(string _filePath)
        {
#if UNITY_EDITOR
            //本地加载资源时从本地路径加载AI
            if (ALLocalResLoaderMgr.instance.isLoadRefdataFromLocal)
            {
                //获取数据路径
                return string.Format(@"{0}/" + ALLocalResLoaderMgr.instance.localResRootPath + "/Refdata/__DLExport/Puerts/{1}.txt", Application.dataPath, _filePath);
            }
#endif

            return $"{_m_rcResCore._patchInfo.patchLocalFullPath}/{_m_sRootPath}/{_filePath}";
        }

        private string PathToUse(string filepath)
        {
            return filepath.EndsWith(".cjs") ?
                filepath.Substring(0, filepath.Length - 4) :
                filepath;
        }
        public bool FileExists(string _filepath)
        {
            //puerts系统文件直接通过随包的处理
            if(_filepath.StartsWith("puerts/"))
            {
                string pathToUse = this.PathToUse(_filepath);
                return UnityEngine.Resources.Load(pathToUse) != null;
            }

            string filePath = _getLocalFilePath(_filepath);

            return File.Exists(filePath);
        }
        /// <summary>
        /// 加载puerts的脚本对象
        /// </summary>
        /// <param name="_filepath"></param>
        public string ReadFile(string _filepath, out string _debugpath)
        {
            //puerts系统文件直接通过随包的处理
            if (_filepath.StartsWith("puerts/"))
            {
                string pathToUse = this.PathToUse(_filepath);
                UnityEngine.TextAsset file = (UnityEngine.TextAsset)UnityEngine.Resources.Load(pathToUse);
                _debugpath = System.IO.Path.Combine("", _filepath);
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
                _debugpath = _debugpath.Replace("/", "\\");
#endif
                return file == null ? null : file.text;
            }

            if (null == _m_rcResCore)
            {
                _debugpath = "NoResCore/" + _filepath;
                return null;
            }

            string filePath = _getLocalFilePath(_filepath);

            if(!File.Exists(filePath))
            {
                _debugpath = "FileNotExists/" + _filepath;
                return null;
            }

            //直接读取ai
            StreamReader stream = new StreamReader(filePath);
            if(null == stream)
            {
                _debugpath = "ReadFileErr/" + _filepath;
                return null;
            }

            //读取文件
            string allText = stream.ReadToEnd();

            //关闭
            stream.Close();
            stream.Dispose();

            _debugpath = filePath;
            //返回结果
            return allText;
        }

        /// <summary>
        /// 释放puerts实例相关资源
        /// </summary>
        /// <param name="_instanceName"></param>
        public void disposePuertsEnv(string _instanceName)
        {
            ALPuertsInstance puertsInstance = _getPuertsInstance(_instanceName);
            if(null == puertsInstance)
                return;

            puertsInstance.discard();
            //从数据集删除
            _m_pdPuertsDictionary.Remove(puertsInstance);
        }

        /// <summary>
        /// 获取对应名称的puerts执行实例对象
        /// </summary>
        /// <param name="_instanceName"></param>
        /// <returns></returns>
        protected ALPuertsInstance _getPuertsInstance(string _instanceName)
        {
            string realName = _instanceName.Trim().ToLowerInvariant();
            for(int i = 0; i < _m_pdPuertsDictionary.Count; i++)
            {
                if(_m_pdPuertsDictionary[i].tag.Equals(realName, StringComparison.OrdinalIgnoreCase))
                    return _m_pdPuertsDictionary[i];
            }

            return null;
        }
        protected ALPuertsInstance _getAndCreatePuertsInstance(string _instanceName)
        {
            ALPuertsInstance instance = _getPuertsInstance(_instanceName);
            if(null == instance)
            {
                //不存在则创建
                string realName = _instanceName.Trim().ToLowerInvariant();
                instance = new ALPuertsInstance(realName, this);
                //放入集合
                _m_pdPuertsDictionary.Add(instance);
            }

            return instance;
        }
    }
#endif
}

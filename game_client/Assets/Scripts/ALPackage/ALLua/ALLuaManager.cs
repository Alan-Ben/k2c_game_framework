using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace ALPackage
{
#if AL_XLUA
    /// <summary>
    /// 需要通过本对象使用lua，需要先使用本对象initLuaAsset函数初始化lua脚本相关
    /// 为了避免每次启动都进行初始化，需要在资源变更或更新的时候做此处理
    /// </summary>
    public class ALLuaManager
    {
        private _AALResourceCore _m_rcResCore;
        /// <summary>
        /// 根节点目录名
        /// </summary>
        private string _m_sRootPath;
        /// <summary>
        /// 全局的Lua环境
        /// </summary>
        private ALLuaInstance _m_leGeneralLuaInstance;
        /// <summary>
        /// lua执行的环境集合
        /// </summary>
        private List<ALLuaInstance> _m_ldLuaDictionary;

        /// <summary>
        /// 子目录的版本管理对象
        /// </summary>
        private ALLuaSubFolderVersionMgr _m_vmFolderVersionMgr;

        /// <summary>
        /// 每帧释放资源的处理任务
        /// </summary>
        private ALCommonEnableTaskController _m_tmTickTask;

        /// <summary>
        /// 基础构造函数
        /// </summary>
        protected ALLuaManager(_AALResourceCore _resCore, string _rootPath)
        {
            _m_rcResCore = _resCore;
            _m_sRootPath = _rootPath;
            _m_leGeneralLuaInstance = new ALLuaInstance(string.Empty, this);

            _m_ldLuaDictionary = new List<ALLuaInstance>();

            _m_vmFolderVersionMgr = new ALLuaSubFolderVersionMgr(this);
            _m_vmFolderVersionMgr.init();

            //开启每帧任务执行tick
            _m_tmTickTask = ALCommonEnableTickActionMonoTask.addMonoTask(tick);
        }

        public string rootPath { get { return _m_sRootPath; } }

        /// <summary>
        /// 创建一个全局lua对应的数据table
        /// </summary>
        /// <returns></returns>
        public XLua.LuaTable createGenearlLuaTable()
        {
            return _m_leGeneralLuaInstance.createEnvTable();
        }

        /// <summary>
        /// 强制系统更新对应目录的脚本
        /// </summary>
        /// <param name="_abName"></param>
        /// <param name="_subFolderName"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void forceUpdateLuaAsset(string _abName, string _subFolderName, Action _sucDelegate, Action _failDelegate)
        {
            //删除版本信息
            _m_vmFolderVersionMgr.removeFolderInfo(_subFolderName);

            //进行更新操作
            addLuaAsset(_abName, _subFolderName, _sucDelegate, _failDelegate);
        }

        /// <summary>
        /// 初始化lua的asset，并将里面的lua脚本写入对应位置
        /// </summary>
        /// <param name="_abName"></param>
        /// <param name="_doneDelegate"></param>
        public void addLuaAsset(string _abName, string _subFolderName, Action _sucDelegate, Action _failDelegate)
        {
            if(null == _m_rcResCore)
                return;

            //获取对应版本信息
            ALAssetBundleVersionInfo versionInfo = _m_rcResCore.getVersionInfo(_abName);
            //判断版本是否一致，如一致则不做后续处理
            if (null != versionInfo && _m_vmFolderVersionMgr.checkFolderVersion(_subFolderName, versionInfo.versionNum))
            {
#if UNITY_EDITOR
                ALLog.Warning($"Lua [{_abName}] - Folder[{_subFolderName}] version is the newest!");
#endif
                //进行回调处理
                if (null != _sucDelegate)
                    _sucDelegate();
                return;
            }

#if UNITY_EDITOR
            ALLog.Sys($"Lua [{_abName}] - Folder[{_subFolderName}] start update Scripts!");
#endif

            _m_rcResCore.loadAsset(_abName
                , (bool _isSuc, ALAssetBundleObj _assetObj) =>
                {
                    //判断是否加载成功
                    if(!_isSuc || null == _assetObj)
                    {
                        //报错
                        ALLog.Error($"Lua [{_abName}] - Folder[{_subFolderName}] Update Fail! - Assetbundle load fail");
                        //进行回调处理
                        if (null != _sucDelegate)
                            _sucDelegate();
                        return;
                    }

                    //将数据中所有文件都导出到对应目录
                    string dicPath = $"{_m_rcResCore._patchInfo.patchLocalFullPath}/{_m_sRootPath}";
                    if (!string.IsNullOrEmpty(_subFolderName))
                    {
                        dicPath = dicPath + "/" + _subFolderName;
                    }
                    //检测文件夹是否存在
                    if (Directory.Exists(dicPath))
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
                            ALLog.Error($"Lua [{_abName}] - Folder[{_subFolderName}] Update Fail! - write file[{tmpObj.name}] fail");
                            //进行回调处理
                            if (null != _failDelegate)
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
                    ALLog.Sys($"Lua [{_abName}] - Folder[{_subFolderName}] Update Success Done!");
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
            if(null != _m_leGeneralLuaInstance)
                _m_leGeneralLuaInstance.tick();

            //遍历每个实例对象执行 tick
            for(int i = 0; i < _m_ldLuaDictionary.Count; i++)
            {
                _m_ldLuaDictionary[i].tick();
            }
        }

        /// <summary>
        /// 释放本对象的所有环境
        /// </summary>
        public void discard()
        {
            _m_tmTickTask.setDisable();

            if(null != _m_leGeneralLuaInstance)
                _m_leGeneralLuaInstance.discard();
            _m_leGeneralLuaInstance = null;

            //遍历每个实例对象执行 tick
            for(int i = 0; i < _m_ldLuaDictionary.Count; i++)
            {
                _m_ldLuaDictionary[i].discard();
            }
            _m_ldLuaDictionary.Clear();
        }

        /// <summary>
        /// 在全局环境中执行带入的lua脚本处理
        /// </summary>
        /// <param name="_luaName"></param>
        public object[] execGeneralLua(string _luaStr)
        {
            if(null == _m_rcResCore)
                return null;

            //执行对应脚本
            if(null != _m_leGeneralLuaInstance)
                return _m_leGeneralLuaInstance.doLua(_luaStr);
            else
                return null;
        }
        public object[] execGeneralLua(string _luaStr, string _chunkName = "chunk", XLua.LuaTable _env = null)
        {
            if(null == _m_rcResCore)
                return null;

            //执行对应脚本
            if(null != _m_leGeneralLuaInstance)
                return _m_leGeneralLuaInstance.doLua(_luaStr, _chunkName, _env);
            else
                return null;
        }

        /// <summary>
        /// 在全局环境中执行指定的Lua脚本对象
        /// </summary>
        /// <param name="_luaName"></param>
        public void doGeneralLua(string _luaName, bool _isNoExistWarning = true, Action<object[]> _doneDelegate = null)
        {
            if(null == _m_rcResCore)
                return;

            //加载lua脚本
            _loadLua(_luaName
                , (string _luaStr) =>
                {
                    object[] res = null;
                    //根据读取到的lua字符串进行处理
                    if(null != _m_leGeneralLuaInstance)
                        res = _m_leGeneralLuaInstance.doLua(_luaStr);

                    if(null != _doneDelegate)
                        _doneDelegate(res);
                }
                , () =>
                {
                    //加载失败的处理
                    if(_isNoExistWarning)
                        ALLog.Error("Load Lua: " + _luaName + " Fail!");

                    if(null != _doneDelegate)
                        _doneDelegate(null);
                }
                , _isNoExistWarning);
        }
        public void doGeneralLua(string _luaName, bool _isNoExistWarning = true, Action<object[]> _doneDelegate = null, string _chunkName = "chunk", XLua.LuaTable _env = null)
        {
            if(null == _m_rcResCore)
                return;

            //加载lua脚本
            _loadLua(_luaName
                , (string _luaStr) =>
                {
                    object[] res = null;
                    //根据读取到的lua字符串进行处理
                    if(null != _m_leGeneralLuaInstance)
                        res = _m_leGeneralLuaInstance.doLua(_luaStr, _chunkName, _env);

                    if(null != _doneDelegate)
                        _doneDelegate(res);
                }
                , () =>
                {
                    //加载失败的处理
                    if(_isNoExistWarning)
                        ALLog.Error("Load Lua: " + _luaName + " Fail!");

                    if(null != _doneDelegate)
                        _doneDelegate(null);
                }
                , _isNoExistWarning);
        }

        /** 暂不建议使用instance处理，先行屏蔽
        /// <summary>
        /// 在指定的lua名称环境下执行语句
        /// </summary>
        /// <param name="_luaName"></param>
        public void doInstanceLua(string _instanceName, string _luaName, bool _isNoExistWarning = true, Action _doneDelegate = null)
        {
            if(null == _m_rcResCore)
                return;

            //加载lua脚本
            _loadLua(_luaName
                , (string _luaStr) =>
                {
                    ALLuaInstance luaInstance = _getAndCreateLuaInstance(_instanceName);
                    //根据读取到的lua字符串进行处理
                    if(null != luaInstance)
                        luaInstance.doLua(_luaStr);

                    if(null != _doneDelegate)
                        _doneDelegate();
                }
                , () =>
                {
                    //加载失败的处理
                    if(_isNoExistWarning)
                        ALLog.Error("Load Lua: " + _luaName + " Fail!");

                    if(null != _doneDelegate)
                        _doneDelegate();
                }
                , _isNoExistWarning);
        }
        public void doInstanceLua(string _instanceName, string _luaName, bool _isNoExistWarning = true, Action _doneDelegate = null, string _chunkName = "chunk", XLua.LuaTable _env = null)
        {
            if(null == _m_rcResCore)
                return;

            //加载lua脚本
            _loadLua(_luaName
                , (string _luaStr) =>
                {
                    ALLuaInstance luaInstance = _getAndCreateLuaInstance(_instanceName);
                    //根据读取到的lua字符串进行处理
                    if(null != luaInstance)
                        luaInstance.doLua(_luaStr, _chunkName, _env);

                    if(null != _doneDelegate)
                        _doneDelegate();
                }
                , () =>
                {
                    //加载失败的处理
                    if(_isNoExistWarning)
                        ALLog.Error("Load Lua: " + _luaName + " Fail!");

                    if(null != _doneDelegate)
                        _doneDelegate();
                }
                , _isNoExistWarning);
        } **/

        /// <summary>
        /// 加载lua的脚本对象
        /// </summary>
        /// <param name="_luaName"></param>
        protected void _loadLua(string _luaName, Action<string> _loadedDelegate, Action _failDelegate, bool _isNoExistWarning = true)
        {
            if(null == _m_rcResCore)
            {
                if(null != _failDelegate)
                    _failDelegate();
                return;
            }

            string filePath = $"{_m_rcResCore._patchInfo.patchLocalFullPath}/{_m_sRootPath}/{_luaName}";

#if UNITY_EDITOR
            //本地加载资源时从本地路径加载AI
            if(ALLocalResLoaderMgr.instance.isLoadRefdataFromLocal)
            {
                //获取数据路径
                filePath = string.Format(@"{0}/" + ALLocalResLoaderMgr.instance.localResRootPath + "/Refdata/__DLExport/Lua/{1}.txt", Application.dataPath, _luaName);
            }
#endif

            if(!File.Exists(filePath))
            {
                if(null != _failDelegate)
                    _failDelegate();

                if(_isNoExistWarning)
                    UnityEngine.Debug.LogError("lua file not exist" + _luaName + " Error!");
                return;
            }

            //直接读取ai
            StreamReader stream = new StreamReader(filePath);
            if(null == stream)
            {
                if(null != _failDelegate)
                    _failDelegate();

                if(_isNoExistWarning)
                    UnityEngine.Debug.LogError("read lua file" + _luaName + " Error!");
                return;
            }

            //读取文件
            string aiAllTxt = stream.ReadToEnd();

            //关闭
            stream.Close();
            stream.Dispose();

            //调用回调
            if(null != _loadedDelegate)
                _loadedDelegate(aiAllTxt);
        }

        /// <summary>
        /// 释放lua实例相关资源
        /// </summary>
        /// <param name="_instanceName"></param>
        public void disposeLuaEnv(string _instanceName)
        {
            ALLuaInstance luaInstance = _getLuaInstance(_instanceName);
            if(null == luaInstance)
                return;

            luaInstance.discard();
            //从数据集删除
            _m_ldLuaDictionary.Remove(luaInstance);
        }

        /// <summary>
        /// 获取对应名称的lua执行实例对象
        /// </summary>
        /// <param name="_instanceName"></param>
        /// <returns></returns>
        protected ALLuaInstance _getLuaInstance(string _instanceName)
        {
            string realName = _instanceName.Trim().ToLowerInvariant();
            for(int i = 0; i < _m_ldLuaDictionary.Count; i++)
            {
                if(_m_ldLuaDictionary[i].tag.Equals(realName, StringComparison.OrdinalIgnoreCase))
                    return _m_ldLuaDictionary[i];
            }

            return null;
        }
        protected ALLuaInstance _getAndCreateLuaInstance(string _instanceName)
        {
            ALLuaInstance instance = _getLuaInstance(_instanceName);
            if(null == instance)
            {
                //不存在则创建
                string realName = _instanceName.Trim().ToLowerInvariant();
                instance = new ALLuaInstance(realName, this);
                //放入集合
                _m_ldLuaDictionary.Add(instance);
            }

            return instance;
        }
    }
#endif
}

using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace ALPackage
{
#if AL_ILRUNTIME
    /// <summary>
    /// 需要通过本对象使用lua，需要先使用本对象initLuaAsset函数初始化lua脚本相关
    /// 为了避免每次启动都进行初始化，需要在资源变更或更新的时候做此处理
    /// 
    /// 本对象是单独通过字节调用的处理类
    /// </summary>
    public class ALILRuntimeManager
    {
        private static ALILRuntimeManager _g_instance = new ALILRuntimeManager();
        public static ALILRuntimeManager instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new ALILRuntimeManager();
                return _g_instance;
            }
        }

        private ILRuntime.Runtime.Enviorment.AppDomain _m_adAppDomain;

        //文件流dll
        private FileStream _m_fs_dll;
        //文件流的pdb，只再editor下
        private FileStream _m_fs_pdb;


        //内存流dll
        private MemoryStream _m_st_dll;
        //内存流的dll，只再editor下
        private MemoryStream _m_st_pdb;

        
        protected ALILRuntimeManager()
        {
            //获取运行环境
            _m_adAppDomain = new ILRuntime.Runtime.Enviorment.AppDomain();
        }

        public ILRuntime.Runtime.Enviorment.AppDomain appDomain { get { return _m_adAppDomain; } }

        /// <summary>
        /// 释放
        /// </summary>
        public void discard()
        {
            _discardFileStream();

            _discardMemoryStream();
        }

        //释放当前文件流
        private void _discardFileStream()
        {
            if (null != _m_fs_dll)
            {
                _m_fs_dll.Close();
                _m_fs_dll.Dispose();
                _m_fs_dll = null;
            }
            
       
            if (null != _m_fs_pdb)
            {
                _m_fs_pdb.Close();
                _m_fs_pdb.Dispose();
                _m_fs_pdb = null;
            }
        }
        
        //释放当前内存流
        private void _discardMemoryStream()
        {
            if (null != _m_st_dll)
            {
                _m_st_dll.Close();
                _m_st_dll.Dispose();
                _m_st_dll = null;
            }
      
            if (null != _m_st_pdb)
            {
                _m_st_pdb.Close();
                _m_st_pdb.Dispose();
                _m_st_pdb = null;
            }
        }
        
        /// <summary>
        /// release方式加载dll文件
        /// </summary>
        /// <param name="_dllPath"></param>
        public void loadDll(byte[] _dllBytes)
        {
            if(null == _dllBytes)
                return;

            if(null == _m_adAppDomain)
            {
                ALLog.Crush("Load Dll when appdomain is null!!");
                return;
            }

            try
            {
                //先释放当前
                _discardMemoryStream();
                
                _m_st_dll = new MemoryStream(_dllBytes);
                
                _m_adAppDomain.LoadAssembly(_m_st_dll);
            }
            catch(Exception _ex)
            {
                //报错直接释放
                _discardMemoryStream();
                
                ALLog.Crush("Load Dll Fail!! " + _ex.Message);
            }
        }
        public void loadDll(byte[] _dllBytes, byte[] _pdbBytes)
        {
            if(null == _dllBytes)
                return;

            if(null == _m_adAppDomain)
            {
                ALLog.Crush("Load Dll when appdomain is null!!");
                return;
            }

            try
            {
                //先释放当前
                _discardMemoryStream();
                _m_st_dll = new MemoryStream(_dllBytes);
                _m_st_pdb = new MemoryStream(_dllBytes);

#if AL_ILRUNTIME_V2
                _m_adAppDomain.LoadAssembly(_m_st_dll, _m_st_pdb, new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());
#else
                _m_adAppDomain.LoadAssembly(_m_st_dll, _m_st_pdb, new Mono.Cecil.Pdb.PdbReaderProvider());
#endif
            }
            catch(Exception _ex)
            {
                //先释放当前
                _discardMemoryStream();
                
                ALLog.Crush("Load Dll Fail!! " + _ex.Message);
            }
        }
        public void loadDllFile(string _dllFilePath)
        {
            if(null == _dllFilePath)
                return;

            if(null == _m_adAppDomain)
            {
                ALLog.Crush("Load Dll when appdomain is null!!");
                return;
            }

            try
            {
                _m_adAppDomain.LoadAssemblyFile(_dllFilePath);
            }
            catch(Exception _ex)
            {
                ALLog.Crush("Load Dll: [" + _dllFilePath + "] Fail!! " + _ex.Message);
            }
        }

        /// <summary>
        /// 调用对应类的对应函数
        /// </summary>
        /// <param name="_classPath"></param>
        /// <param name="_actionnName"></param>
        public void dealAction(string _classPath, string _actionName)
        {
            if(null == _classPath || null == _actionName)
                return;

            if(null == _m_adAppDomain)
            {
                ALLog.Crush("deal action when appdomain is null!!");
                return;
            }

            try
            {
                _m_adAppDomain.Invoke(_classPath, _actionName, null, null);
            }
            catch(Exception _ex)
            {
                ALLog.Crush("Deal Action: [" + _classPath + "." + _actionName + "] Fail!! " + _ex.Message);
            }
        }
    }
#endif
                    }

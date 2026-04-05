using UnityEngine;
using UnityEngine.Networking;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Reflection;

using ALPackage;
using System.Threading;
using JetBrains.Annotations;

namespace ALPackage
{
    //游戏总管理类
    public class ALHotfixMgr_ILRuntime
    {
#if AL_ILRUNTIME
        public static ILRuntime.Runtime.Enviorment.AppDomain g_AppDomain
        {
            get
            {
                return ALHotfixMgr_ILRuntime_Global.g_AppDomain;
            }
        }
        /// <summary>
        /// 根据下载路径，加载名称等创建对应的ILRuntime的Dll管理对象
        /// </summary>
        /// <param name="_dllUrl"></param>
        /// <param name="_dllSaveName"></param>
        /// <param name="_pdbUrl"></param>
        /// <param name="_pdbSaveName"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        /// <returns></returns>
        public static ALHotfixMgr_ILRuntime createILRuntimeMgr(string _dllSaveName, string _pdbSaveName)
        {
            return new ALHotfixMgr_ILRuntime(_dllSaveName, _pdbSaveName);
        }

        private string _m_sDllSavePath;
        private string _m_sDllTempSavePath;//文件先存到临时目录
        private string _m_sDllStreamingAssetSavePath;//文件在StreamingAsset里的路径
        private string _m_sPdbSavePath;
        private string _m_sPdbTempSavePath;//文件先存到临时目录
        private string _m_sPdbStreamingAssetSavePath;//文件在StreamingAsset里的路径

        /// <summary>
        /// 是否正在处理
        /// </summary>
        private bool _m_bIsDealing;

        /// <summary>
        /// 处理结果，默认为true，有失败则设置
        /// </summary>
        private bool _m_bSuc;
        /// <summary>
        /// 是否已经加载完毕
        /// </summary>
        private bool _m_isLoadDone;
        private Action _m_dLoadDoneDelegate;

        /// <summary>
        /// 加载的处理对象
        /// </summary>
        private ALStepCounter _m_scLoadCounter;
        
        /**
         * 加载dll的流被关闭了。新版要求流不能关闭，也不能用using写法。新的用法请参见观法Demo示例
         */
        
        //文件流dll
        private FileStream _m_fs_dll;
#if UNITY_EDITOR
        //文件流的pdb，只再editor下
        private FileStream _m_fs_pdb;
#endif

        //内存流dll
        private MemoryStream _m_st_dll;
#if UNITY_EDITOR
        //内存流的dll，只再editor下
        private MemoryStream _m_st_pdb;
#endif

        protected ALHotfixMgr_ILRuntime(string _dllSaveName, string _pdbSaveName)
        {
            _m_bIsDealing = false;

            _m_sDllSavePath = _makeLocalSavePath(_dllSaveName);
            _m_sDllTempSavePath = _makeLocalSavePath("temp_" + _dllSaveName);
            _m_sDllStreamingAssetSavePath = Application.streamingAssetsPath + "/hotfix/" + _dllSaveName;
            _m_sPdbSavePath = _makeLocalSavePath(_pdbSaveName);
            _m_sPdbTempSavePath = _makeLocalSavePath("temp_" + _pdbSaveName);
            _m_sPdbStreamingAssetSavePath = Application.streamingAssetsPath + "/hotfix/" + _pdbSaveName;

            _m_isLoadDone = false;
            _m_bSuc = true;

            _m_dLoadDoneDelegate = default(Action);
        }

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
            
#if UNITY_EDITOR          
            if (null != _m_fs_pdb)
            {
                _m_fs_pdb.Close();
                _m_fs_pdb.Dispose();
                _m_fs_pdb = null;
            }
#endif
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
            
#if UNITY_EDITOR          
            if (null != _m_st_pdb)
            {
                _m_st_pdb.Close();
                _m_st_pdb.Dispose();
                _m_st_pdb = null;
            }
#endif
        }

        /// <summary>
        /// 开启任务执行
        /// </summary>
        /// <param name="_dllUrl"></param>
        /// <param name="_dllSaveName"></param>
        /// <param name="_pdbUrl"></param>
        /// <param name="_pdbSaveName"></param>
        /// <param name="_isLocal">是否直接使用本地hotfix</param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void startDownload(string _dllUrl, string _pdbUrl, Action<float> _processDelegate, Action _sucDelegate, Action _failDelegate)
        {
            //如已经加载完成则报错
            if(_m_isLoadDone)
            {
                UnityEngine.Debug.LogError($"已经加载完成Dll的时候尝试加载本地保存的dll！");
                if (_sucDelegate != null) 
                    _sucDelegate();
                return ;
            }

            if(_m_bIsDealing)
            {
                UnityEngine.Debug.LogError($"在加载过程中尝试进行加载操作！");
                _m_scLoadCounter.regAllDoneDelegate(() =>
                {
                    if (_m_bSuc)
                    {
                        if (_sucDelegate != null) 
                            _sucDelegate();
                    }
                    else
                    {
                        if (_failDelegate != null) 
                            _failDelegate();
                    }
                });
                return;
            }

            if(string.IsNullOrEmpty(_dllUrl))
            {
                if(_failDelegate != null)
                {
                    _failDelegate();
                }
                return;
            }

            _m_bIsDealing = true;

            _m_scLoadCounter = new ALStepCounter();
#if UNITY_EDITOR
            _m_scLoadCounter.chgTotalStepCount(2);
#else
            _m_scLoadCounter.chgTotalStepCount(1);
#endif
            _m_scLoadCounter.regAllDoneDelegate(
                ()=>
                {
                    //不先设置不在加载是因为在tryload操作中还是属于加载行为
                    if(_tryLoadDownloadDll())
                    {
                        _m_bIsDealing = false;

                        //处理成果调用结果
                        if(null != _sucDelegate)
                            _sucDelegate();

                        //设置加载完成
                        setLoadDone();
                    }
                    else
                    {
                        _m_bIsDealing = false;

                        if(null != _failDelegate)
                            _failDelegate();
                    }
                });

            //分别创建下载对象
            ALHttpSingleDownloadMd5Dealer dllDownloadDealer = new ALHttpSingleDownloadMd5Dealer(_dllUrl, _m_sDllTempSavePath, _processDelegate, _onDealerSuc, _onDealerFail);
            dllDownloadDealer.startLoad();
            
#if UNITY_EDITOR
            ALHttpSingleDownloadMd5Dealer pdbDownloadDealer = new ALHttpSingleDownloadMd5Dealer(_pdbUrl, _m_sPdbTempSavePath, null, _onDealerSuc, _onDealerFail);
            pdbDownloadDealer.startLoad();
#endif
        }
        
        /// <summary>
        /// 直接根据路径加载dll
        /// </summary>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        /// <returns></returns>
        public bool tryLoadDllByPath(string _dllPath, string _pdbPath, Action _sucDelegate, Action _failDelegate)
        {
            //如已经加载完成则报错
            if(_m_isLoadDone)
            {
                UnityEngine.Debug.LogError($"已经加载完成Dll的时候尝试加载本地保存的dll！");
                return false;
            }

            if(_m_bIsDealing)
            {
                UnityEngine.Debug.LogError($"在加载过程中尝试尝试加载本地保存的dll！");
                return false;
            }

            //加载成功
            if(_onDllLoaded(_dllPath, _pdbPath))
            {
                if(null != _sucDelegate)
                    _sucDelegate();
                
                //设置加载完成
                setLoadDone();

                return true;
            }
            else
            {
                if(null != _failDelegate)
                    _failDelegate();
                
                return false;
            }
        }

        /// <summary>
        /// 加载本地dll的处理
        /// </summary>
        /// <param name="_dllUrl"></param>
        /// <param name="_dllSaveName"></param>
        /// <param name="_pdbUrl"></param>
        /// <param name="_pdbSaveName"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public bool tryLoadLocalSavedDll(Action _sucDelegate, Action _failDelegate)
        {
            //如已经加载完成则报错
            if(_m_isLoadDone)
            {
                UnityEngine.Debug.LogError($"已经加载完成Dll的时候尝试加载本地保存的dll！");
                return false;
            }

            if(_m_bIsDealing)
            {
                UnityEngine.Debug.LogError($"在加载过程中尝试尝试加载本地保存的dll！");
                return false;
            }

            // 本地加载成功
            if(_onDllLoaded(_m_sDllSavePath, _m_sPdbSavePath))
            {
                if(null != _sucDelegate)
                    _sucDelegate();

                //设置加载完成
                setLoadDone();

                return true;
            }
            else
            {
                if(null != _failDelegate)
                    _failDelegate();

                return false;
            }
        }
        
        public bool tryLoadLocalStreamingDll(Action _sucDelegate, Action _failDelegate)
        {
            //如已经加载完成则报错
            if(_m_isLoadDone)
            {
                UnityEngine.Debug.LogError($"已经加载完成Dll的时候尝试加载本地Streaming的dll！");
                return false;
            }

            if(_m_bIsDealing)
            {
                UnityEngine.Debug.LogError($"在加载过程中尝试尝试加载本地Streaming的dll！");
                return false;
            }

            // 本地加载成功
            if(_onDllLoadStreaming(_m_sDllStreamingAssetSavePath, _m_sPdbStreamingAssetSavePath))
            {
                if(null != _sucDelegate)
                    _sucDelegate();

                //设置加载完成
                setLoadDone();

                return true;
            }
            else
            {
                if(null != _failDelegate)
                    _failDelegate();

                return false;
            }
        }
        
        

        /// <summary>
        /// 注册加载完成的触发函数
        /// </summary>
        /// <param name="_onLoadDone"></param>
        public void regLoadDone(Action _onLoadDone)
        {
            if(_onLoadDone == null)
            {
                return;
            }

            if(_m_isLoadDone)
            {
                if(_onLoadDone != null)
                {
                    _onLoadDone();
                }
            }
            else
            {
                _m_dLoadDoneDelegate += _onLoadDone;
            }
        }

        /// <summary>
        /// 设置加载完成
        /// </summary>
        public void setLoadDone()
        {
            if(_m_isLoadDone)
                return;

            _m_isLoadDone = true;

            //触发对应完成的响应函数
            if(null != _m_dLoadDoneDelegate)
                _m_dLoadDoneDelegate();
            _m_dLoadDoneDelegate = null;
        }

        public void clearILRuntimeLocalSavedDll()
        {
            if (null != _m_sDllSavePath && File.Exists(_m_sDllSavePath))
                File.Delete(_m_sDllSavePath);

            if (null != _m_sDllTempSavePath && File.Exists(_m_sDllTempSavePath))
                File.Delete(_m_sDllTempSavePath);

            if (null != _m_sPdbSavePath && File.Exists(_m_sPdbSavePath))
                File.Delete(_m_sPdbSavePath);

            if (null != _m_sPdbTempSavePath && File.Exists(_m_sPdbTempSavePath))
                File.Delete(_m_sPdbTempSavePath);
        }
        
        /// <summary>
        /// 在成功处理的时候进行的操作
        /// </summary>
        protected void _onDealerSuc()
        {
            _m_scLoadCounter.addDoneStepCount();
        }

        /// <summary>
        /// 在失败处理的时候进行的操作
        /// </summary>
        protected void _onDealerFail()
        {
            _m_bSuc = false;

            _m_scLoadCounter.addDoneStepCount();
        }

        /// <summary>
        /// 加载完成后的处理
        /// </summary>
        protected bool _tryLoadDownloadDll()
        {
            if(_m_bSuc)
            {
                //处理dll加载
                if(!_onDllLoaded(_m_sDllTempSavePath, _m_sPdbTempSavePath))
                {
                    _m_bSuc = false;
                    return false;
                }
                else
                {
                    try
                    {
                        //如果成功加载，则把临时文件复制到正式文件
                        File.Copy(_m_sDllTempSavePath, _m_sDllSavePath, true);
#if UNITY_EDITOR
                        File.Copy(_m_sPdbTempSavePath, _m_sPdbSavePath, true);
#endif
                    }
                    catch(Exception e)
                    {
                        Debug.LogError($"拷贝热更文件时发生错误:\n{e}");
                        _m_bSuc = false;
                        return false;
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 根据带入文件拼凑最后位置
        /// </summary>
        /// <param name="_settingPath"></param>
        protected string _makeLocalSavePath(string _settingPath)
        {
            //拼凑完整路径
#if UNITY_ANDROID && !UNITY_EDITOR
            string tmpPath = Application.persistentDataPath;
            if (null == tmpPath)
            {
                tmpPath = "/data/data" + Application.dataPath.Substring(9);
                if (tmpPath.LastIndexOf('-') == -1)
                    tmpPath = tmpPath.Substring(0, tmpPath.LastIndexOf('/')) + "/files";
                else
                    tmpPath = tmpPath.Substring(0, tmpPath.LastIndexOf('-')) + "/files";
            }
            return tmpPath + "/" + _settingPath;
#else
            return Application.persistentDataPath + "/" + _settingPath;
#endif
        }

        /// <summary>
        /// 当dll，pdb下载完毕后触发的函数
        /// 非Editor环境下，只下载和加载dll，不处理pdb
        /// </summary>
        protected bool _onDllLoadStreaming(string _dllFilePath, string _pdbFilePath)
        {
#if UNITY_EDITOR
            if(null == _dllFilePath || !File.Exists(_dllFilePath)
                                    || null == _pdbFilePath || !File.Exists(_pdbFilePath))
                return false;

            //使用前释放当前的流
            _discardMemoryStream();
            _m_st_dll = new MemoryStream(File.ReadAllBytes(_dllFilePath));
            _m_st_pdb = new MemoryStream(File.ReadAllBytes(_pdbFilePath));
            
            try
            {
#if AL_ILRUNTIME_V2
                //加载对应数据
                ALHotfixMgr_ILRuntime_Global.g_AppDomain.LoadAssembly(_m_st_dll, _m_st_pdb, new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());
#else
                //加载对应数据
                ALHotfixMgr_ILRuntime_Global.g_AppDomain.LoadAssembly(_m_st_dll, _m_st_pdb, new Mono.Cecil.Pdb.PdbReaderProvider());
#endif
            }
            catch (Exception e)
            {
                //报错了直接释放
                _discardMemoryStream();

                Debug.LogError($"dll或pdb格式错误，加载失败\n{e}");
                return false;
            }
            return true;
#else
#if UNITY_ANDROID
            UnityWebRequest loader = null;
            UnityWebRequestAsyncOperation operation = null;

            try
            {
                loader = UnityWebRequest.Get(_dllFilePath);
                operation = loader.SendWebRequest();

                //if not finish yield return
                while(!operation.isDone)
                {
                    Thread.Sleep(10);
                }

                byte[] bytes = loader.downloadHandler.data;

                //释放当前的
                _discardMemoryStream();
                _m_st_dll = new MemoryStream(bytes);

                try
                {
                    //加载对应数据
                    ALHotfixMgr_ILRuntime_Global.g_AppDomain.LoadAssembly(_m_st_dll);
                }
                catch(Exception e)
                {
                    //报错了释放当前的
                    _discardMemoryStream();

                    Debug.LogError($"dll或pdb格式错误，加载失败\n{e}");
                    return false;
                }
            }
            catch(Exception e)
            {

                return false;
            }
            finally
            {
                if(loader != null)
                    loader.Abort();
            }
            return true;
#else
            if(null == _dllFilePath || !File.Exists(_dllFilePath))
                return false;

            //使用前释放当前的流
            _discardFileStream();
            _m_fs_dll = new FileStream(_dllFilePath, FileMode.Open, FileAccess.Read);

            try
            {
                //加载对应数据
                ALHotfixMgr_ILRuntime_Global.g_AppDomain.LoadAssembly(_m_fs_dll);
            }
            catch (Exception e)
            {
                //报错了直接释放
                _discardFileStream();

                Debug.LogError($"dll格式错误，加载失败\n{e}");
                return false;
            }
            return true;
#endif
#endif
        }
        protected bool _onDllLoaded(string _dllFilePath, string _pdbFilePath)
        {
#if UNITY_EDITOR
            if(null == _dllFilePath || !File.Exists(_dllFilePath)
                                    || null == _pdbFilePath || !File.Exists(_pdbFilePath))
                return false;

            //使用前释放当前的流
            _discardMemoryStream();
            _m_st_dll = new MemoryStream(File.ReadAllBytes(_dllFilePath));
            _m_st_pdb = new MemoryStream(File.ReadAllBytes(_pdbFilePath));
            
            try
            {
#if AL_ILRUNTIME_V2
                //加载对应数据
                ALHotfixMgr_ILRuntime_Global.g_AppDomain.LoadAssembly(_m_st_dll, _m_st_pdb, new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());
#else
                //加载对应数据
                ALHotfixMgr_ILRuntime_Global.g_AppDomain.LoadAssembly(_m_st_dll, _m_st_pdb, new Mono.Cecil.Pdb.PdbReaderProvider());
#endif
            }
            catch (Exception e)
            {
                //报错了直接释放
                _discardMemoryStream();

                Debug.LogError($"dll或pdb格式错误，加载失败\n{e}");
                return false;
            }
            return true;
#else
            if(null == _dllFilePath || !File.Exists(_dllFilePath))
                return false;

            //使用前释放当前的流
            _discardFileStream();
            _m_fs_dll = new FileStream(_dllFilePath, FileMode.Open, FileAccess.Read);

            try
            {
                //加载对应数据
                ALHotfixMgr_ILRuntime_Global.g_AppDomain.LoadAssembly(_m_fs_dll);
            }
            catch (Exception e)
            {
                //报错了直接释放
                _discardFileStream();

                Debug.LogError($"dll格式错误，加载失败\n{e}");
                return false;
            }
            return true;
#endif
        }
        protected bool _onDllLoaded(byte[] _dllBytes, byte[] _pdbBytes)
        {
#if UNITY_EDITOR
            if(null == _dllBytes || null == _pdbBytes)
                return false;

            //使用前释放当前的流
            _discardMemoryStream();
            _m_st_dll = new MemoryStream(_dllBytes);
            _m_st_pdb = new MemoryStream(_pdbBytes);
            
            try
            {
#if AL_ILRUNTIME_V2
                //加载对应数据
                ALHotfixMgr_ILRuntime_Global.g_AppDomain.LoadAssembly(_m_st_dll, _m_st_pdb, new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());
#else
                //加载对应数据
                ALHotfixMgr_ILRuntime_Global.g_AppDomain.LoadAssembly(_m_st_dll, _m_st_pdb, new Mono.Cecil.Pdb.PdbReaderProvider());
#endif
            }
            catch (Exception e)
            {
                //报错了直接释放
                _discardMemoryStream();
                
                Debug.LogError($"dll或pdb格式错误，加载失败\n{e}");
                return false;
            }
            return true;
#else
            if(null == _dllBytes)
                return false;

            //使用前释放当前的流
            _discardMemoryStream();
            _m_st_dll = new MemoryStream(_dllBytes);

            try
            {
                //加载对应数据
                ALHotfixMgr_ILRuntime_Global.g_AppDomain.LoadAssembly(_m_st_dll);
            }
            catch (Exception e)
            {
                //使用前释放当前的流
                _discardMemoryStream();

                Debug.LogError($"dll格式错误，加载失败\n{e}");
                return false;
            }
            return true;
#endif
        }
#endif
    }
}


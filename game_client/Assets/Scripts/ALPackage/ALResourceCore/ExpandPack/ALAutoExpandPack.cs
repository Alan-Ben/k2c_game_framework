using System;
using System.Threading;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

#if AL_SEVEN_ZIP
namespace ALPackage
{
    public class ALAutoExpandPack : _IALAssetBundleVersionInitFunc, _IALBaseMonoTask
    {
        /** 本对象初始化的步骤统计对象 */
        private ALStepCounter _m_scInitStepCounter;

        //用于检测是否需要解压的版本号
        private long _m_lCheckExpandVersionNum;
        //本地资源文件路径
        private string _m_sLocalVersionFolderPath;
        private string _m_sLocalVersionFilePath;
        //需要解压的文件信息
        private long _m_lExpandVersionNum;
        private List<ALAssetBundleVersionInfo> _m_lNeedExpandVersionInfo;
        private int _m_iExpandIdx;
        //总文件大小
        private long _m_lTotalSize;
        //当前已经处理的文件大小
        private long _m_lDoneSize;
        //总的临时处理文件大小，用于判断是否需要保存信息
        private long _m_lTmpDoneSize;
        //当前正在处理的文件中已经处理的大小
        private long _m_lCurDealingDoneSize;
        //本地资源补丁管理对象
        private ALLocalPatchInfo _m_lpiLocalPathInfo;

        //解压线程完成统计对象
        private ALStepCounter _m_asThreadCounter;

        /** 安卓用于控制同时解压内存的 */
        private int _m_iLoadingCount;
        private long _m_lTotalExpandSize;
        private long _m_lTotalExpandMaxSize;

        //检测间隔
        private float _m_fCheckDuration;
        //开始解压的触发函数
        private Action _m_dOnExpandStart;
        //完成解压的触发函数
        private Action<long> _m_dOnExpandDone;
        //过程回调
        private Action<long, long> _m_dProcessDelegate;

        //拷贝文件的拷贝函数
        //参数分别为，相对的asset路径， 绝对的拷贝目标路径， 拷贝完成的回调
        protected int _m_iTmpCopySerialize;
        protected Action<string, string, Action<bool>> _m_dCopyFileAction;

        //是否完成
        private bool _m_bIsDone;
        //是否需要退出
        private bool _m_bEnable;

#if UNITY_ANDROID && !UNITY_EDITOR
        public ALAutoExpandPack(long _checkVersionNum, string _localPatchFolerPath, string _patchPath, ALLocalPatchInfo _localPatchInfo, Action<string, string, Action<bool>> _copyFileAction)
        {
            _m_lCheckExpandVersionNum = _checkVersionNum;
            //创建步骤统计对象
            _m_scInitStepCounter = new ALStepCounter();
            //创建本地资源包打包资源加载对象
            _m_sLocalVersionFolderPath = _localPatchFolerPath;
            _m_sLocalVersionFilePath = Application.streamingAssetsPath + "/" + _localPatchFolerPath;

            _m_lNeedExpandVersionInfo = null;
            _m_iExpandIdx = 0;
            _m_lTotalSize = 0;
            _m_lDoneSize = 0;
            _m_lCurDealingDoneSize = 0;

            _m_asThreadCounter = new ALStepCounter();

            _m_iLoadingCount = 0;
            _m_lTotalExpandSize = 0;
            _m_lTotalExpandMaxSize = 0;

            _m_lpiLocalPathInfo = _localPatchInfo;

            _m_fCheckDuration = 1f;
            _m_dOnExpandStart = null;
            _m_dOnExpandDone = null;
            _m_dProcessDelegate = null;
            _m_bIsDone = false;
            _m_bEnable = true;

            _m_iTmpCopySerialize = 1;
            _m_dCopyFileAction = _copyFileAction;
        }
#else
        public ALAutoExpandPack(long _checkVersionNum, string _localPatchFolerPath, string _patchPath, ALLocalPatchInfo _localPatchInfo)
        {
            _m_lCheckExpandVersionNum = _checkVersionNum;
            //创建步骤统计对象
            _m_scInitStepCounter = new ALStepCounter();
            //创建本地资源包打包资源加载对象
            _m_sLocalVersionFolderPath = _localPatchFolerPath;
            _m_sLocalVersionFilePath = Application.streamingAssetsPath + "/" + _m_sLocalVersionFolderPath;

            _m_lNeedExpandVersionInfo = null;
            _m_iExpandIdx = 0;
            _m_lTotalSize = 0;
            _m_lDoneSize = 0;
            _m_lCurDealingDoneSize = 0;

            _m_asThreadCounter = new ALStepCounter();

            _m_iLoadingCount = 0;
            _m_lTotalExpandSize = 0;
            _m_lTotalExpandMaxSize = 0;

            _m_lpiLocalPathInfo = _localPatchInfo;

            _m_fCheckDuration = 1f;
            _m_dOnExpandStart = null;
            _m_dOnExpandDone = null;
            _m_dProcessDelegate = null;
            _m_bIsDone = false;
            _m_bEnable = true;

            _m_iTmpCopySerialize = 1;
            _m_dCopyFileAction = null;
        }
#endif

        public void discard()
        {
            _m_scInitStepCounter.resetAll();
            _m_dOnExpandStart = null;
            _m_dOnExpandDone = null;
            _m_dProcessDelegate = null;
            _m_bIsDone = false;
            _m_bEnable = false;

            _m_asThreadCounter.resetAll();

            _m_dCopyFileAction = null;
        }

        /*******************
         * 初始化相关资源
         * 带入检测进度间隔时间，进度过程回调，完成回调
         **/
        public void init(Action _onStartExpand, float _checkDuration, Action<long, long> _processDelegate, Action<long> _doneDelegate)
        {
            //注册回调
            _m_scInitStepCounter.regAllDoneDelegate(_onExpandDone);
            _m_scInitStepCounter.chgTotalStepCount(1);

            //检测相关函数
            _m_fCheckDuration = _checkDuration;
            _m_dProcessDelegate = _processDelegate;
            _m_dOnExpandStart = _onStartExpand;
            _m_dOnExpandDone = _doneDelegate;
            if (_m_fCheckDuration < 0.1f)
                _m_fCheckDuration = 0.1f;

            //根据内存初始化限制
            //获取当前系统安全的下载过程大小
            long totalExpandAndroidMaxSize = 10;
            if (SystemInfo.systemMemorySize > 720)
                totalExpandAndroidMaxSize = 30;
            if (SystemInfo.systemMemorySize > 1024)
                totalExpandAndroidMaxSize = 50;
            if (SystemInfo.systemMemorySize > 1720)
                totalExpandAndroidMaxSize = 200;
            _m_lTotalExpandMaxSize = totalExpandAndroidMaxSize * 1024 * 1024;

            _m_iLoadingCount = 0;
            _m_lTotalExpandSize = 0;

            _m_lTotalSize = 0;
            _m_bIsDone = false;
            //初始化版本
            _initVersion();

            //开启监控
            ALMonoTaskMgr.instance.addMonoTask(this);
        }

        /** 尝试增加解压尺寸 */
        protected bool _tryAddExpandSize(long _size)
        {
            lock(this)
            {
                //判断是否会超出安全尺寸
                if(_m_iLoadingCount > 0 && (_m_lTotalExpandSize + _size) > _m_lTotalExpandMaxSize)
                {
                    return false;
                }

                //累加次数
                _m_iLoadingCount++;
                //累加尺寸
                _m_lTotalExpandSize += _size;

                return true;
            }
        }

        /** 尝试减少解压尺寸 */
        protected void _tryRemoveExpandSize(long _size)
        {
            lock (this)
            {
                //累加次数
                _m_iLoadingCount--;
                //累加尺寸
                _m_lTotalExpandSize -= _size;
            }
        }

        /****************
         * 初始化资源版本的操作
         **/
        protected void _initVersion()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            ALAssetBundleVersionDownloadDealer.InitVersionDownload(this, _m_sLocalVersionFilePath);
#elif UNITY_IOS  //解决Ios13使用file头文件加载失败
            ALAssetBundleVersionLocalInitDealer.InitVersionDownload(this, _m_sLocalVersionFilePath);
#else
            ALAssetBundleVersionDownloadDealer.InitVersionDownload(this, "file:///" + _m_sLocalVersionFilePath);
#endif
        }
        
        //获取对应文件的路径
        protected string _getAssetPath(string _assetPath)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            return _m_sLocalVersionFilePath + "/" + _assetPath;
#else
            return _m_sLocalVersionFilePath + "/" + _assetPath;
#endif
        }

        //在解压处理完毕的时候处理
        protected void _onExpandDone()
        {
            if (null != _m_dOnExpandDone)
                _m_dOnExpandDone(_m_lExpandVersionNum);
        }

        /**************
         * 定时处理的进度监控任务
         **/
        public void deal()
        {
            if (null != _m_dProcessDelegate)
                _m_dProcessDelegate(_m_lCurDealingDoneSize + _m_lDoneSize, _m_lTotalSize);

            if(_m_bIsDone)
            {
                if (null != _m_dProcessDelegate)
                    _m_dProcessDelegate(_m_lTotalSize, _m_lTotalSize);

                //增加步骤
                _m_scInitStepCounter.addDoneStepCount();
                //释放所有信息
                discard();
            }
            else
            {
                //注册下一次调用
                //<=0的话加入下一帧，防止卡死
                if (_m_fCheckDuration <= 0)
                {
                    ALMonoTaskMgr.instance.addNextFrameTask(this);
                }
                else
                {
                    ALMonoTaskMgr.instance.addMonoTask(this, _m_fCheckDuration);
                }
            }
        }

        /***************
         * 在版本初始化完成时的处理
         **/
        public void initVersionSetList(long _versionNum, List<ALAssetBundleVersionInfo> _versionInfoList)
        {
            if(_m_lCheckExpandVersionNum == _versionNum)
            {
                //直接处理结果
                _m_bIsDone = true;
                return;
            }

            if(null == _versionInfoList)
            {
                initVersionFail();
                return;
            }

            _m_lTotalSize = 0;
            _m_lExpandVersionNum = _versionNum;
            //根据所有文件，进行解压处理
            _m_lNeedExpandVersionInfo = new List<ALAssetBundleVersionInfo>();
            for (int i = 0; i < _versionInfoList.Count; i++)
            {
                ALAssetBundleVersionInfo curVersionInfo = _versionInfoList[i];
                if (null == curVersionInfo)
                    continue;

                //判断文件是否需要更新
                ALAssetBundleVersionInfo patchVersionInfo = _m_lpiLocalPathInfo.getAssetPatchVersionInfo(curVersionInfo.assetPath);
                if (null != patchVersionInfo
                    && patchVersionInfo.fileSize == curVersionInfo.fileSize 
                    && patchVersionInfo.versionNum == curVersionInfo.versionNum 
                    && patchVersionInfo.hashTag == curVersionInfo.hashTag)
                    continue;

                _m_lNeedExpandVersionInfo.Add(curVersionInfo);
                _m_lTotalSize += curVersionInfo.fileSize;
            }

            //不需要处理则直接返回
            if(_m_lNeedExpandVersionInfo.Count <= 0)
            {
                _m_bIsDone = true;
                return;
            }

            //开始解压
            if (null != _m_dOnExpandStart)
                _m_dOnExpandStart();

            //初始化统计数据
            _m_iExpandIdx = 0;
            _m_lCurDealingDoneSize = 0;
            //根据cpu数量开启线程处理
            int threadCount = SystemInfo.processorCount * 2;
            if (threadCount < 4)
                threadCount = 4;

#if UNITY_IPHONE
            threadCount = 6; 
#endif

            //初始化线程完成统计对象
            _m_asThreadCounter.resetStepInfo();
            _m_asThreadCounter.chgTotalStepCount(threadCount);
            _m_asThreadCounter.regAllDoneDelegate(_expandDone);

            for (int i = 0; i < threadCount; i++)
            {
                //开启线程处理
#if UNITY_ANDROID && !UNITY_EDITOR
                _m_sStreamingAssetsPath = _m_sLocalVersionFilePath;
                _m_sTmpTargetPath = Application.persistentDataPath;
            
                //开启线程进行写入处理，并在写入完成后重新调用资源加载
                Thread writeThread = new Thread(_expandThreadAndroid);
                writeThread.Start();
#else
                //_m_sStreamingAssetsPath = "file:///" + _m_sLocalVersionFilePath;
                _m_sStreamingAssetsPath = _m_sLocalVersionFilePath;
                _m_sTmpTargetPath = Application.persistentDataPath;

                //开启线程进行写入处理，并在写入完成后重新调用资源加载
                ALThread writeThread = new ALThread(_expandThread);
                writeThread.startThread();
#endif
            }
        }

        //初始化成功
        protected void _expandDone()
        {
            //直接累加步骤
            _m_bIsDone = true;
            //保存最后解压信息
            _m_lpiLocalPathInfo.savePatchInfo();
        }
        //初始化失败
        public void initVersionFail()
        {
            //直接累加步骤
            _m_bIsDone = true;
            //保存最后解压信息
            _m_lpiLocalPathInfo.savePatchInfo();
        }

        /** 增加已处理文件大小 */
        protected void _addDoneFileSize(long _fileSize)
        {
            lock (_m_lNeedExpandVersionInfo)
            {
                //增加处理文件大小，并判断是否需要保存
                _m_lTmpDoneSize += _fileSize;
                _m_lDoneSize += _fileSize;

                //保存
                if (_m_lTmpDoneSize > 5120000)
                {
                    _m_lpiLocalPathInfo.savePatchInfo();
                    _m_lTmpDoneSize = 0;
                }
            }
        }

        /**********
         * 调整当前处理的大小
         **/
        protected void _chgCurDealingSize(long _size)
        {
            //调整当前处理的大小
            lock (_m_lNeedExpandVersionInfo)
            {
                _m_lCurDealingDoneSize += _size;
            }
        }

        /***********
         * 取出第一个需要解压的信息
         **/
        protected ALAssetBundleVersionInfo _popNeedExpandInfo()
        {
            lock (_m_lNeedExpandVersionInfo)
            {
                if (_m_lNeedExpandVersionInfo.Count <= _m_iExpandIdx)
                    return null;

                ALAssetBundleVersionInfo info = _m_lNeedExpandVersionInfo[_m_iExpandIdx];
                _m_iExpandIdx++;

                return info;
            }
        }

        //每个线程单独记录当前解压的处理情况对象
        protected class ALExpandProcessRecorder : SevenZip.ICodeProgress
        {
            private ALAutoExpandPack _m_aeExpander;
            //当前进度
            private long _m_lCurProcess;

            public ALExpandProcessRecorder(ALAutoExpandPack _expander)
            {
                _m_aeExpander = _expander;
            }

            //处理过程的回调
            public void SetProgress(Int64 _inSize, Int64 _outSize)
            {
                //直接设置处理字节数，不进行加锁，因为处理的频率太高
                if(_outSize - _m_lCurProcess > 1024)
                {
                    //通知变更
                    _m_aeExpander._chgCurDealingSize(_outSize - _m_lCurProcess);
                    _m_lCurProcess = _outSize;
                }
            }

            //重置数据
            public void reset()
            {
                _m_aeExpander._chgCurDealingSize(-_m_lCurProcess);
                _m_lCurProcess = 0;
            }
        }

        protected string _m_sStreamingAssetsPath;
        protected string _m_sTmpTargetPath;
#if UNITY_ANDROID && !UNITY_EDITOR
        /******************
         * 逐个开启解压操作，因为安卓平台的streaming asset目录文件只可通过www访问，因此用此特殊处理
         **/
        protected void _expandThreadAndroid()
        {
            ALAssetBundleVersionInfo tmpInfo = null;
            _m_lDoneSize = 0;

            //记录解压情况的对象
            ALExpandProcessRecorder recorder = new ALExpandProcessRecorder(this);

            //定义信号量
            Semaphore downloadSign = new Semaphore(0, 1);

            //获取临时文件状态
            int tmpFileSerialize = 0;
            string tmpFileFullPath = string.Empty;
            lock (this)
            {
                tmpFileSerialize = _m_iTmpCopySerialize++;
                tmpFileFullPath = _m_sTmpTargetPath + @"/" + "__" + tmpFileSerialize + ".tmp";
            }

            //取出一个解压信息
            tmpInfo = _popNeedExpandInfo();
            while (null != tmpInfo)
            {
                if (!_m_bEnable)
                    return;

                //尝试增加处理文件
                if (!_tryAddExpandSize(tmpInfo.fileSize))
                {
                    //休眠200毫秒重试
                    Thread.Sleep(50);
                    continue;
                }

                //判断拷贝回调是否有效
                if (null != _m_dCopyFileAction)
                {
                    //删除文件
                    ALFile.Delete(tmpFileFullPath);

                    bool res = false;
                    ALCommonTaskController.CommonActionAddMonoTask(() =>
                    {
                        try
                        {
                            //拷贝处理
                            _m_dCopyFileAction(_m_sLocalVersionFolderPath + "/" + tmpInfo.assetPath, tmpFileFullPath
                                , (bool _suc) =>
                                {
                                    //根据结果判断后续处理
                                    if (_suc)
                                    {
                                        //设置成功
                                        res = true;
                                        //等待信号量
                                        downloadSign.Release();
                                    }
                                    else
                                    {
                                        //失败进行失败处理
                                        UnityEngine.Debug.LogError("Copy File Fail: " + tmpInfo.assetPath);
                                        //等待信号量
                                        downloadSign.Release();
                                    }
                                });
                        }
                        catch(Exception _tEx)
                        {
                            UnityEngine.Debug.LogError("Copy File Err: " + _tEx.Message);
                            //等待信号量
                            downloadSign.Release();
                        }
                    });

                    //等待信号量
                    downloadSign.WaitOne();

                    //判断结果
                    if (res)
                    {
                        try
                        {
                            //增加补丁文件信息
                            _m_lpiLocalPathInfo.addLZMAPatchFileInfo(tmpInfo, tmpFileFullPath, recorder);
                        }
                        catch (Exception _ex)
                        {
                            UnityEngine.Debug.LogError("Expand Add Patch Err: " + _ex.Message);

                            //删除临时文件
                            ALFile.Delete(tmpFileFullPath);
                            //退出对应的处理尺寸
                            _tryRemoveExpandSize(tmpInfo.fileSize);
                            //休眠200毫秒重试
                            Thread.Sleep(200);

                            continue;
                        }

                        //删除临时文件
                        ALFile.Delete(tmpFileFullPath);
                    }
                    else
                    {
                        //重新处理
                        //删除临时文件
                        ALFile.Delete(tmpFileFullPath);
                        //退出对应的处理尺寸
                        _tryRemoveExpandSize(tmpInfo.fileSize);
                        //休眠200毫秒重试
                        Thread.Sleep(200);

                        continue;
                    }

                    //设置文件完成
                    _addDoneFileSize(tmpInfo.fileSize);
                    //重置本线程统计
                    recorder.reset();

                    //退出对应的处理尺寸
                    _tryRemoveExpandSize(tmpInfo.fileSize);
                }
                else
                {
                    //开启文件下载
                    ALWWWExpandDownloadDealer downloadDealer = new ALWWWExpandDownloadDealer(_m_sStreamingAssetsPath, tmpInfo.assetPath, downloadSign);
                    //开启加载
                    downloadDealer._startLoad();

                    //等待信号量
                    downloadSign.WaitOne();

                    //取出www进行处理
                    if (null != downloadDealer.bytes)
                    {
                        try
                        {
                            //增加补丁文件信息
                            _m_lpiLocalPathInfo.addLZMAPatchFileInfo(tmpInfo, downloadDealer.bytes, recorder);
                        }
                        catch (Exception _ex)
                        {
                            UnityEngine.Debug.LogError("Expand Add Patch Err: " + _ex.Message);

                            //退出对应的处理尺寸
                            _tryRemoveExpandSize(tmpInfo.fileSize);
                            //释放资源
                            downloadDealer._discard();
                            //休眠200毫秒重试
                            Thread.Sleep(200);

                            continue;
                        }
                    }

                    //释放资源
                    downloadDealer._discard();

                    //设置文件完成
                    _addDoneFileSize(tmpInfo.fileSize);
                    //重置本线程统计
                    recorder.reset();

                    //退出对应的处理尺寸
                    _tryRemoveExpandSize(tmpInfo.fileSize);
                }

                //取新需要处理的信息
                tmpInfo = _popNeedExpandInfo();
            }

            //设置完成状态
            _m_asThreadCounter.addDoneStepCount();
        }
#else
        /**************
         * 非安卓平台的处理方式
         **/
        protected void _expandThread()
        {
            ALAssetBundleVersionInfo tmpInfo = null;
            _m_lDoneSize = 0;

            //记录解压情况的对象
            ALExpandProcessRecorder recorder = new ALExpandProcessRecorder(this);

            //取出一个解压信息
            tmpInfo = _popNeedExpandInfo();
            while(null != tmpInfo)
            {
                if (!_m_bEnable)
                    return;

                //尝试增加处理文件
                if (!_tryAddExpandSize(tmpInfo.fileSize))
                {
                    //休眠200毫秒重试
                    Thread.Sleep(50);
                    continue;
                }

                //获取文件路径
                string filePath = _getAssetPath(tmpInfo.assetPath);
                if (!File.Exists(filePath))
                    continue;

                try
                {
                    //增加补丁文件信息
                    _m_lpiLocalPathInfo.addLZMAPatchFileInfo(tmpInfo, filePath, recorder);
                }
                catch(Exception _ex)
                {
                    UnityEngine.Debug.LogError("Expand Add Patch Err: " + _ex.Message);

                    //退出对应的处理尺寸
                    _tryRemoveExpandSize(tmpInfo.fileSize);
                    //休眠200毫秒重试
                    Thread.Sleep(200);

                    continue;
                }

                //设置文件完成
                _addDoneFileSize(tmpInfo.fileSize);
                //重置本线程统计
                recorder.reset();

                //退出对应的处理尺寸
                _tryRemoveExpandSize(tmpInfo.fileSize);

                //取新需要处理的信息
                tmpInfo = _popNeedExpandInfo();
            }

            //设置完成状态
            _m_asThreadCounter.addDoneStepCount();
        }
#endif
    }
}
#endif

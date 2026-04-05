using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;

using UnityEngine;

/******************
 * 本地资源信息结构体对象
 **/
namespace ALPackage
{
    public class ALLocalPatchInfo
    {
        /** 是否初始化完成 */
        private bool _m_bIsInit;

        /** 本地补丁信息对象的完整路径，不以/结尾 */
        private string _m_sPatchLocalFullPath;
        /** 存放文件对象的相对路径 */
        private string _m_sPatch;

        /////存储从本地读取出的相关信息/////
        /** 本地当前补丁版本号 */
        private long _m_lResourceVersion;
        /** 本地所有补丁文件相关信息映射表 */
        private Dictionary<string, ALAssetBundleVersionInfo> _m_dicPatchAssetsInfoDic;

        /** 需要保存的所有版本信息结构体队列，下面的变量将在线程中使用 */
        private long _m_lSaveVersionInfoVersion;
        private List<ALAssetBundleVersionInfo> _m_lSaveVersionInfoList;
        /** 是否在保存 */
        private bool _m_bIsSaving;
        /** 数据是否有变更 */
        private bool _m_bIsDataChg;

        /** 补丁资源的资源加载对象 */
        private ALLocalAssetBundleLoadMgr _m_lmLocalResLoadMgr;

        public ALLocalPatchInfo(string _path)
        {
            _m_bIsInit = false;

            _m_sPatch = _path;
            //拼凑完整路径
#if UNITY_ANDROID && !UNITY_EDITOR
            _m_sPatchLocalFullPath = Application.persistentDataPath;
            if (null == _m_sPatchLocalFullPath)
            {
                _m_sPatchLocalFullPath = "/data/data" + Application.dataPath.Substring(9);
                if (_m_sPatchLocalFullPath.LastIndexOf('-') == -1)
                    _m_sPatchLocalFullPath = _m_sPatchLocalFullPath.Substring(0, _m_sPatchLocalFullPath.LastIndexOf('/')) + "/files";
                else
                    _m_sPatchLocalFullPath = _m_sPatchLocalFullPath.Substring(0, _m_sPatchLocalFullPath.LastIndexOf('-')) + "/files";
            }
            _m_sPatchLocalFullPath = _m_sPatchLocalFullPath + "/" + _m_sPatch;
#else
            _m_sPatchLocalFullPath = Application.persistentDataPath + "/" + _m_sPatch;
#endif

            _m_lResourceVersion = 0;
            _m_dicPatchAssetsInfoDic = new Dictionary<string, ALAssetBundleVersionInfo>();

            _m_lSaveVersionInfoVersion = 0;
            _m_lSaveVersionInfoList = new List<ALAssetBundleVersionInfo>();

            _m_bIsSaving = false;
            _m_bIsDataChg = false;

            //创建加载对象
            _m_lmLocalResLoadMgr = new ALLocalAssetBundleLoadMgr(20, 3, new ALABStringLoadPathProvider(_m_sPatchLocalFullPath), false);
            //直接开始监控，设置空的版本信息
            _m_lmLocalResLoadMgr.startMonitor();
        }

        public string patchPath { get { return _m_sPatch; } }
        public string patchLocalFullPath { get { return _m_sPatchLocalFullPath; } }
        public long resourceVersion { get { return _m_lResourceVersion; } }
        public ALLocalAssetBundleLoadMgr assetBundleLoadMgr { get { return _m_lmLocalResLoadMgr; } }

        /********************
         * 初始化本补丁管理对象的相关数据集
         **/
        public void init()
        {
            if(_m_bIsInit)
            {
                UnityEngine.Debug.Log("Init Patch Info Multi Times!");
                return;
            }

            //设置状态
            _m_bIsInit = true;

            //初始化数据
            StreamReader reader = null;
            try
            {
                //检测文件夹是否存在
                if (!Directory.Exists(_m_sPatchLocalFullPath))
                    Directory.CreateDirectory(_m_sPatchLocalFullPath);

                //检测是否有临时文件存在，是则将临时文件转移到正常文件位置
                if(File.Exists(_m_sPatchLocalFullPath + "/__al_patch.tmp"))
                {
                    //移动文件，删除原文件
                    if(File.Exists(_m_sPatchLocalFullPath + "/__al_patch.info"))
                        File.Delete(_m_sPatchLocalFullPath + "/__al_patch.info");

                    //拷贝临时文件到对应位置
                    File.Copy(_m_sPatchLocalFullPath + "/__al_patch.tmp", _m_sPatchLocalFullPath + "/__al_patch.info");
                    //删除临时文件
                    ALFile.Delete(_m_sPatchLocalFullPath + "/__al_patch.tmp");
                }

                //开启对应信息的存储文件
                if(!File.Exists(_m_sPatchLocalFullPath + "/__al_patch.info"))
                    return;

                reader = File.OpenText(_m_sPatchLocalFullPath + "/__al_patch.info");
            }
            catch(Exception ex)
            {
                UnityEngine.Debug.LogError("Open Patch Info Error!\n Message: " + ex.Message);
                return;
            }

            //逐行读取信息并存储
            string perLineStr;
            //非空内容的行标记，第0行为特殊版本号信息
            int contentLineIdx = 0;

            while(true)
            {
                perLineStr = reader.ReadLine();
                //如读取出内容为null表示到文件结尾了
                if(null == perLineStr)
                    break;

                perLineStr = perLineStr.Trim();
                //判断内容是否为空，是则继续下一个循环
                if (perLineStr.Length <= 0)
                    continue;

                //逐行读取，并将每行数据转化为对应文件信息
                if(0 == contentLineIdx)
                {
                    //读取版本号
                    _m_lResourceVersion = Int64.Parse(perLineStr);
                }
                else
                {
                    //读取节点数据的信息
                    string[] splitContent = perLineStr.Split(':');

                    try
                    {
                        //逐个创建数据对象
                        ALAssetBundleVersionInfo versionInfo = new ALAssetBundleVersionInfo();
                        //按照指定格式读取
                        versionInfo.assetPath = splitContent[0];
                        versionInfo.hashTag = splitContent[1];
                        versionInfo.versionNum = Int64.Parse(splitContent[2]);
                        versionInfo.fileSize = Int64.Parse(splitContent[3]);

                        //将数据加入数据集
                        _m_dicPatchAssetsInfoDic.Add(versionInfo.assetPath.ToLowerInvariant(), versionInfo);
                    }
                    catch(Exception ex)
                    {
                        UnityEngine.Debug.Log("Create Patch Versin Info Error!\n Message: " + ex.Message);
                    }
                }

                //增加行统计
                contentLineIdx++;
            }

            //关闭文件流
            reader.Close();
            //释放资源
            reader.Dispose();

            //输出初始化完成日志
            UnityEngine.Debug.Log("Patch : " + _m_sPatch + " Init Done!");
        }

        /********************
         * 保存本信息对象
         **/
        public void savePatchInfo()
        {
            if (!_m_bIsInit)
            {
                Debug.LogError("Try save patch Info while still not inited!");
                return;
            }

            //输出初始化完成日志
            Debug.Log("Start Save Patch : " + _m_sPatch + " Info Version : " + _m_lResourceVersion);

            //判断是否正在保存，是则延迟到下次开启任务
            if(_m_bIsSaving)
            {
                //延迟10秒开启任务
                ALMonoTaskMgr.instance.addMonoTask(new ALLocalPatchAutoSaveMonoTask(this), 5);
                //退出函数
                return;
            }

            //将所有对象取出，放入队列，并带入保存线程
            lock(_m_lSaveVersionInfoList)
            {
                //清除原先数据集
                _m_lSaveVersionInfoList.Clear();

                //设置保存版本号
                _m_lSaveVersionInfoVersion = _m_lResourceVersion;

                lock (_m_dicPatchAssetsInfoDic)
                {
                    //加入新数据集
                    _m_lSaveVersionInfoList.AddRange(_m_dicPatchAssetsInfoDic.Values);
                }

                //设置保存状态
                _m_bIsDataChg = false;
                _m_bIsSaving = true;

                //开启线程进行处理
                ALThread saveThread = new ALThread(_savePatchThread);
                saveThread.startThread();
            }
        }

        /**************************
         * 获取补丁对象的版本信息
         **/
        public ALAssetBundleVersionInfo getAssetPatchVersionInfo(string _assetPath)
        {
            lock (_m_dicPatchAssetsInfoDic)
            {
                string key = _assetPath.ToLowerInvariant();
                if (!_m_dicPatchAssetsInfoDic.ContainsKey(key))
                    return null;

                return _m_dicPatchAssetsInfoDic[key];
            }
        }

        /**************************
         * 获取所有补丁信息的拷贝
         **/
        public List<ALAssetBundleVersionInfo> getAllAssetPatchInfoList()
        {
            lock (_m_dicPatchAssetsInfoDic)
            {
                List<ALAssetBundleVersionInfo> list = new List<ALAssetBundleVersionInfo>();

                list.AddRange(_m_dicPatchAssetsInfoDic.Values);

                return list;
            }
        }

        /********************
         * 克隆版本信息的集合对象
         **/
        public Dictionary<string, ALAssetBundleVersionInfo>  cloneVersionDic()
        {
            lock (_m_dicPatchAssetsInfoDic)
            {
                Dictionary<string, ALAssetBundleVersionInfo> dic = new Dictionary<string, ALAssetBundleVersionInfo>(_m_dicPatchAssetsInfoDic);

                return dic;
            }
        }
        
        /********************
         * 加载本地补丁区域资源的加载操作
         **/
        public bool loadAsset(string _resPath, bool _isNeccessary, _assetDownloadedDelegate _delegate, _assetDownloadedDelegate _lateDelegate)
        {
            //判断是否有对应补丁对象
            ALAssetBundleVersionInfo versionInfo = getAssetPatchVersionInfo(_resPath);
            if (null == versionInfo)
                return false;

            //开启加载
            _m_lmLocalResLoadMgr.addLoadAssetInfo(_resPath, false, _isNeccessary, _delegate, _lateDelegate);

            return true;
        }
        public bool loadAsset(string _resPath, bool _isNeccessary, List<_assetDownloadedDelegate> _delegateList, List<_assetDownloadedDelegate> _lateDelegateList)
        {
            //判断是否有对应补丁对象
            ALAssetBundleVersionInfo versionInfo = getAssetPatchVersionInfo(_resPath);
            if (null == versionInfo)
                return false;

            //开启加载
            _m_lmLocalResLoadMgr.addLoadAssetInfo(_resPath, false, _isNeccessary, _delegateList, _lateDelegateList);

            return true;
        }

#if AL_SEVEN_ZIP
        /******************
         * 添加补丁信息
         **/
        public void addLZMAPatchFileInfo(ALAssetBundleVersionInfo _versionInfo, byte[] _compressBytes, SevenZip.ICodeProgress _progress)
        {
            if (null == _versionInfo)
                return;

            //将文件解压后写入对应的位置
            string fileFullPath = patchLocalFullPath + "/" + _versionInfo.assetPath;
            //判断文件是否存在，存在则删除旧文件
            if (File.Exists(fileFullPath))
            {
                File.Delete(fileFullPath);
            }

            //判断文件夹是否创建
            int folderSepIdx = fileFullPath.LastIndexOf('/');
            string folderPath = fileFullPath.Substring(0, folderSepIdx);
            //判断文件夹是否存在，不存在则创建
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            //将文件解压到对应位置
            ALLZMACommon.DecompressFileLZMA(_compressBytes, fileFullPath, _progress);

            lock (_m_dicPatchAssetsInfoDic)
            {
                _m_dicPatchAssetsInfoDic.Remove(_versionInfo.assetPath.ToLowerInvariant());
                //增加对应数据集合
                _m_dicPatchAssetsInfoDic.Add(_versionInfo.assetPath.ToLowerInvariant(), _versionInfo);
            }

            //设置数据变更
            _setDataChg();
        }
        public void addLZMAPatchFileInfo(ALAssetBundleVersionInfo _versionInfo, string _srcFilePath, SevenZip.ICodeProgress _progress)
        {
            if (null == _versionInfo)
                return;

            //将文件解压后写入对应的位置
            string fileFullPath = patchLocalFullPath + "/" + _versionInfo.assetPath;
            //判断文件是否存在，存在则删除旧文件
            if (File.Exists(fileFullPath))
            {
                File.Delete(fileFullPath);
            }

            //判断文件夹是否创建
            int folderSepIdx = fileFullPath.LastIndexOf('/');
            string folderPath = fileFullPath.Substring(0, folderSepIdx);
            //判断文件夹是否存在，不存在则创建
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            //将文件解压到对应位置
            ALLZMACommon.DecompressFileLZMA(_srcFilePath, fileFullPath, _progress);

            lock (_m_dicPatchAssetsInfoDic)
            {
                _m_dicPatchAssetsInfoDic.Remove(_versionInfo.assetPath.ToLowerInvariant());
                //增加对应数据集合
                _m_dicPatchAssetsInfoDic.Add(_versionInfo.assetPath.ToLowerInvariant(), _versionInfo);
            }

            //设置数据变更
            _setDataChg();
        }
#endif

        /// <summary>
        /// 增加一个未压缩的补丁文件
        /// </summary>
        /// <param name="_versionInfo"></param>
        /// <param name="_srcFilePath"></param>
        /// <param name="_progress"></param>
        public void addUnCompressPatchFileInfo(ALAssetBundleVersionInfo _versionInfo, string _srcFilePath)
        {
            if(null == _versionInfo)
                return;

            string key = _versionInfo.assetPath.ToLowerInvariant();
            //判断文件版本号是否一致，如果一致则不进行文件操作，此处是避免再同时调用一个文件的时候，由于多次调用带来文件多次下载写入的问题
            lock(_m_dicPatchAssetsInfoDic)
            {
                ALAssetBundleVersionInfo tmpInfo;
                if(_m_dicPatchAssetsInfoDic.TryGetValue(key, out tmpInfo))
                {
                    if(tmpInfo.versionNum == _versionInfo.versionNum)
                        return;
                }
            }

            //将文件解压后写入对应的位置
            string fileFullPath = patchLocalFullPath + "/" + _versionInfo.assetPath;
            //判断文件是否存在，存在则删除旧文件
            if(File.Exists(fileFullPath))
            {
                File.Delete(fileFullPath);
            }

            //判断文件夹是否创建
            int folderSepIdx = fileFullPath.LastIndexOf('/');
            string folderPath = fileFullPath.Substring(0, folderSepIdx);
            //判断文件夹是否存在，不存在则创建
            if(!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            //将文件解压到对应位置
            File.Copy(_srcFilePath, fileFullPath, true);

            lock(_m_dicPatchAssetsInfoDic)
            {
                _m_dicPatchAssetsInfoDic.Remove(key);
                //增加对应数据集合
                _m_dicPatchAssetsInfoDic.Add(key, _versionInfo);
            }

            //设置数据变更
            _setDataChg();
        }

        /******************
         * 删除资源对象
         **/
        public void removePatchAsset(string _assetPath)
        {
            //删除文件
            string fullPath = _m_sPatchLocalFullPath + "/" + _assetPath;
            if(File.Exists(fullPath))
                File.Delete(fullPath);

            //从版本集合中删除数据
            lock (_m_dicPatchAssetsInfoDic)
            {
                _m_dicPatchAssetsInfoDic.Remove(_assetPath.ToLowerInvariant());
            }
        }

        /// <summary>
        /// 删除所有缓存信息
        /// </summary>
        /// <param name="_assetPath"></param>
        public void clearPatch()
        {
            foreach(ALAssetBundleVersionInfo vInfo in _m_dicPatchAssetsInfoDic.Values)
            {
                //删除文件
                if(File.Exists(_m_sPatchLocalFullPath + "/" + vInfo.assetPath))
                    File.Delete(_m_sPatchLocalFullPath + "/" + vInfo.assetPath);
            }
            _m_dicPatchAssetsInfoDic.Clear();

            //保存数据
            savePatchInfo();
        }

        /*******************
         * 设置数据变动
         **/
        protected internal void _setDataChg()
        {
            //如果已经是变动状态则不处理
            if (_m_bIsDataChg)
                return;

            //否则修改状态，并延迟保存版本号信息
            _m_bIsDataChg = true;
            //延迟10秒开启任务
            ALMonoTaskMgr.instance.addMonoTask(new ALLocalPatchAutoSaveMonoTask(this), 5);
        }

        /********************
         * 保存补丁信息的脚本对象
         **/
        protected void _savePatchThread()
        {
            /** 写入对象 */
            StreamWriter writer;

            //判断文件是否存在，是则将文件转移到临时位置
            if(File.Exists(_m_sPatchLocalFullPath + "/__al_patch.info"))
            {
                //删除临时位置文件
                if(File.Exists(_m_sPatchLocalFullPath + "/__al_patch.tmp"))
                    File.Delete(_m_sPatchLocalFullPath + "/__al_patch.tmp");
                //拷贝到临时位置
                File.Copy(_m_sPatchLocalFullPath + "/__al_patch.info", _m_sPatchLocalFullPath + "/__al_patch.tmp");
                //删除原位置文件
                ALFile.Delete(_m_sPatchLocalFullPath + "/__al_patch.info");
            }

            //创建新文件
            writer = File.CreateText(_m_sPatchLocalFullPath + "/__al_patch.info");

            lock (_m_lSaveVersionInfoList)
            {
                //开始写入信息
                //首先写入文件版本信息
                writer.WriteLine(_m_lSaveVersionInfoVersion.ToString());

                //逐个写入版本信息
                for (int i = 0; i < _m_lSaveVersionInfoList.Count; i++)
                {
                    ALAssetBundleVersionInfo versionInfo = _m_lSaveVersionInfoList[i];
                    if (null == versionInfo)
                        continue;

                    //写入信息
                    writer.WriteLine(versionInfo.assetPath + ":" + versionInfo.hashTag + ":" + versionInfo.versionNum + ":" + versionInfo.fileSize);
                }
            }

            //关闭文件流
            writer.Close();
            writer.Dispose();

            //删除临时位置文件
            if(File.Exists(_m_sPatchLocalFullPath + "/__al_patch.tmp"))
                File.Delete(_m_sPatchLocalFullPath + "/__al_patch.tmp");

            //设置线程状态
            _m_bIsSaving = false;
        }

        /**************
         * 获取临时下载下来的文件路径
         **/
        public string getTempDownloadLocalFile(ALAssetBundleVersionInfo _versionInfo)
        {
            string fileFullPath = patchLocalFullPath + "/_tmp/" + _versionInfo.assetPath;
            string exctractionPath = System.IO.Path.GetDirectoryName(fileFullPath);
            string fileName = Path.GetFileName(fileFullPath);
            string tempFile = Path.Combine(exctractionPath, fileName + ".tmp");
            return tempFile;
        }
    }
}

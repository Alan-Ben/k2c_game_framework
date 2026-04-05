using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using UnityEngine;
using UnityEditor;

using UnityEngine.Networking;

/******************
 * 资源管理对象的通用函数处理类
 **/
namespace ALPackage
{
    public class ALAssetBundleCommon
    {
        private static BuildTarget[] _g_arrAllBuildTarget = { BuildTarget.Android, BuildTarget.iOS, BuildTarget.StandaloneWindows };

        /*****************
         * 检测相关黄静配置是否创建
         **/
        public static void checkEnv()
        {
            for(int i = 0; i < _g_arrAllBuildTarget.Length; i++)
                checkEnv(_g_arrAllBuildTarget[i]);
        }
        public static void checkEnv(BuildTarget _buildTarget)
        {
            //检查对应路径的目录是否存在，不存在则创建目录
            if(!Directory.Exists(Application.dataPath + "/Resources"))
                AssetDatabase.CreateFolder("Assets", "Resources");
            if(!Directory.Exists(Application.dataPath + "/Resources/__ALAssetBundle"))
                AssetDatabase.CreateFolder("Assets/Resources", "__ALAssetBundle");
            if(!Directory.Exists(Application.dataPath + "/Resources/__ALAssetBundle/" + _buildTarget.ToString()))
                AssetDatabase.CreateFolder("Assets/Resources/__ALAssetBundle", _buildTarget.ToString());

            //还需要初始化相关导出资源的版本信息控制对象
        }

        /**************
         * 获取导出的相关路径信息
         **/
        public static string getAssetExportFolderFullPath(BuildTarget _buildTarget)
        {
            return "Assets/Resources/__ALAssetBundle/" + _buildTarget.ToString();
        }
        public static string getAssetExportFullPath(BuildTarget _buildTarget, string _assetName)
        {
            if(_assetName.StartsWith("/"))
                return "Assets/Resources/__ALAssetBundle/" + _buildTarget.ToString() + _assetName;
            else
                return "Assets/Resources/__ALAssetBundle/" + _buildTarget.ToString() + "/" + _assetName;
        }
        public static string getAssetExportFolderAssetPath(BuildTarget _buildTarget)
        {
            return "Resources/__ALAssetBundle/" + _buildTarget.ToString();
        }
        public static string getAssetExportAssetPath(BuildTarget _buildTarget, string _assetName)
        {
            if(_assetName.StartsWith("/"))
                return "Resources/__ALAssetBundle/" + _buildTarget.ToString() + _assetName;
            else
                return "Resources/__ALAssetBundle/" + _buildTarget.ToString() + "/" + _assetName;
        }

        /**************
         * 获取导出的相关压缩路径信息
         **/
        public static string getAssetExportCompressFolderFullPath(BuildTarget _buildTarget)
        {
            return "Assets/Resources/__ALAssetBundleCompress/" + _buildTarget.ToString();
        }
        public static string getAssetExportCompressFullPath(BuildTarget _buildTarget, string _assetName)
        {
            if(_assetName.StartsWith("/"))
                return "Assets/Resources/__ALAssetBundleCompress/" + _buildTarget.ToString() + _assetName;
            else
                return "Assets/Resources/__ALAssetBundleCompress/" + _buildTarget.ToString() + "/" + _assetName;
        }
        public static string getAssetExportCompressFolderAssetPath(BuildTarget _buildTarget)
        {
            return "Resources/__ALAssetBundleCompress/" + _buildTarget.ToString();
        }
        public static string getAssetExportCompressAssetPath(BuildTarget _buildTarget, string _assetName)
        {
            if(_assetName.StartsWith("/"))
                return "Resources/__ALAssetBundleCompress/" + _buildTarget.ToString() + _assetName;
            else
                return "Resources/__ALAssetBundleCompress/" + _buildTarget.ToString() + "/" + _assetName;
        }

        /*******************
         * 根据带入的资源名称，删除对应的资源导出文件
         **/
        public static void removeExportFile(string _assetName)
        {
            if(null == _assetName || _assetName.Length <= 0)
                return;

            //删除资源文件和manifest文件
            for(int i = 0; i < _g_arrAllBuildTarget.Length; i++)
                removeExportFile(_g_arrAllBuildTarget[i], _assetName);
        }
        public static void removeExportFile(BuildTarget _buildTarget, string _assetName)
        {
            if(null == _assetName || _assetName.Length <= 0)
                return;

            //删除资源文件和manifest文件
            AssetDatabase.DeleteAsset(getAssetExportFullPath(_buildTarget, _assetName));
            AssetDatabase.DeleteAsset(getAssetExportFullPath(_buildTarget, _assetName) + ".manifest");
        }

        /********************
         * 根据带入的平台信息，重新打包资源
         **/
        public static void exportAsset(BuildTarget _buildTarget, long _versionNum, bool _exportDependence, bool _ifCompress)
        {
            BuildAssetBundleOptions option = BuildAssetBundleOptions.UncompressedAssetBundle | BuildAssetBundleOptions.DeterministicAssetBundle;
            if(_ifCompress)
                option = option & ~BuildAssetBundleOptions.UncompressedAssetBundle | BuildAssetBundleOptions.ChunkBasedCompression;

            UnityEngine.Debug.LogError("Export Res compress: " + _ifCompress);
            //打包资源
            AssetBundleManifest maniFest = BuildPipeline.BuildAssetBundles(
                getAssetExportFolderFullPath(_buildTarget)
                , option
                , _buildTarget);

            if(null == maniFest)
            {
                //如果返回为空，是表示打包存在错误，直接抛出异常
                throw new Exception("BuildPipeline.BuildAssetBundles has error, please check the console!");
//                refreshVersionNum(_versionNum, _buildTarget);
            }
            else
            {
                //获取当前导出数据版本信息
                string srcP = Application.dataPath + "/" + getAssetExportFolderAssetPath(_buildTarget);

                //先拷贝旧数据
                if(File.Exists(srcP + "/version"))
                {
                    File.Delete(srcP + "/_pre_version");
                    File.Copy(srcP + "/version", srcP + "/_pre_version");
                }

                //将打包返回的操作信息进行后续版本号管理处理
                dealAssetBundleVersionInfo(maniFest, _buildTarget, _exportDependence, _versionNum);

                //加载新的version
                AssetBundle newAssetBundle = AssetBundle.LoadFromFile(srcP + "/version");
                //读取原始资源对象
                ALSOAssetBundleVersionSet newVersionSet = newAssetBundle.LoadAsset("version") as ALSOAssetBundleVersionSet;
                //卸载资源
                newAssetBundle.Unload(false);
                if(newVersionSet == null)
                {
                    //删除临时文件
                    File.Delete(srcP + "/_pre_version");
                    throw new Exception("version包里，加载version文件失败");
                }

                //读取源数据的版本信息
                ALSOAssetBundleVersionSet srcVersionSet = null;
                if(File.Exists(srcP + "/_pre_version"))
                {
                    AssetBundle assetBundle = AssetBundle.LoadFromFile(srcP + "/_pre_version");
                    if(null != assetBundle)
                    {
                        //读取原始资源对象
                        srcVersionSet = assetBundle.LoadAsset("version") as ALSOAssetBundleVersionSet;
                        //卸载资源
                        assetBundle.Unload(false);
                    }
                    else
                    {
                        UnityEngine.Debug.LogError(srcP + "/version" + " is null");
                    }
                }
                else
                {
                    UnityEngine.Debug.LogError(srcP + "/version" + " file not exist");
                }

                if(null != srcVersionSet)
                {
                    //新旧对比，删除不存在的旧数据
                    ALAssetBundleVersionInfo tmpInfo = null;
                    for(int i = 0; i < srcVersionSet.versionInfoList.Count; i++)
                    {
                        tmpInfo = srcVersionSet.versionInfoList[i];
                        if(null == tmpInfo)
                            continue;

                        bool enable = false;
                        //判断是否在新集合中
                        for(int n = 0; n < newVersionSet.versionInfoList.Count; n++)
                        {
                            if(newVersionSet.versionInfoList[n].assetPath.Equals(tmpInfo.assetPath))
                            {
                                enable = true;
                                break;
                            }
                        }

                        //如无效则删除
                        if(!enable)
                        {
                            ALAssetBundleCommon.removeExportFile(_buildTarget, tmpInfo.assetPath);
                        }
                    }
                }
                else
                {
                    //UnityEngine.Debug.LogError("srcVersionSet is null");
                }

                //删除临时文件
                File.Delete(srcP + "/_pre_version");
            }
        }

        /*********************
         * 自动压缩对应导出的资源
         **/
        public static ALAssetBundleCompressThreadController compressAsset(BuildTarget _buildTarget)
        {
            //需要处理的文件路径
            List<string> needExportFilePath = new List<string>();
            //需要删除的文件路径输出部分
            List<string> needRemoveFilePath = new List<string>();

            //读取源数据的版本信息
            AssetBundle assetBundle = AssetBundle.LoadFromFile(Application.dataPath + "/" + ALAssetBundleCommon.getAssetExportAssetPath(_buildTarget, "version"));
            //读取原始资源对象
            ALSOAssetBundleVersionSet srcVersionSet = null;
            if(null != assetBundle)
            {
                srcVersionSet = assetBundle.LoadAsset("version") as ALSOAssetBundleVersionSet;
                assetBundle.Unload(false);
                assetBundle = null;
            }
            else
            {
                UnityEngine.Debug.LogError("Src Folder Has not version info!");
                return null;
            }

            //判断数据是否有效
            if(null == srcVersionSet)
            {
                UnityEngine.Debug.LogError("Src Folder Has not version set!");
                return null;
            }

            //读取目标位置的版本信息对象
            UnityWebRequest exportedVersionWWW = UnityWebRequestAssetBundle.GetAssetBundle("file:///" + Application.dataPath + "/" + ALAssetBundleCommon.getAssetExportCompressAssetPath(_buildTarget, "version"));
            UnityWebRequestAsyncOperation asyncOp = exportedVersionWWW.SendWebRequest();

            int timeCount = 0;
            while(!asyncOp.isDone)
            {
                System.Threading.Thread.Sleep(100);

                timeCount++;
                if(timeCount > 100)
                {
                    //加载超时直接返回
                    UnityEngine.Debug.LogError("加载version超时!");
                    if(null != exportedVersionWWW)
                    {
                        exportedVersionWWW.Abort();
                        exportedVersionWWW.Dispose();
                    }
                    exportedVersionWWW = null;
                    return null;
                }
            }

            ALSOAssetBundleVersionSet expVersionSet = null;
            //判断加载结果
#if UNITY_2020
            if (exportedVersionWWW.result == UnityWebRequest.Result.ConnectionError
                || exportedVersionWWW.result == UnityWebRequest.Result.DataProcessingError
                || exportedVersionWWW.result == UnityWebRequest.Result.ProtocolError)
#else
            if(exportedVersionWWW.isNetworkError || exportedVersionWWW.isHttpError)
#endif
            {
            }
            else
            {
                AssetBundle exportVersionAssetBundle = DownloadHandlerAssetBundle.GetContent(exportedVersionWWW);
                if(null != exportVersionAssetBundle)
                {
                    expVersionSet = exportVersionAssetBundle.LoadAsset("version") as ALSOAssetBundleVersionSet;
                }

                //释放加载资源
                if(null != exportedVersionWWW)
                {
                    exportedVersionWWW.Abort();
                    exportedVersionWWW.Dispose();
                }
                exportedVersionWWW = null;
                if(null != exportVersionAssetBundle)
                    exportVersionAssetBundle.Unload(false);
                exportVersionAssetBundle = null;
            }

            //判断导出位置的版本信息是否有效
            if(null != expVersionSet)
            {
                //逐个文件判断是否需要导出
                for(int i = 0; i < srcVersionSet.versionInfoList.Count; i++)
                {
                    ALAssetBundleVersionInfo srcInfo = srcVersionSet.versionInfoList[i];
                    if(null == srcInfo)
                        continue;

                    //在导出的版本信息中查找需要对应的信息
                    ALAssetBundleVersionInfo exportedInfo = null;
                    for(int n = 0; n < expVersionSet.versionInfoList.Count; n++)
                    {
                        ALAssetBundleVersionInfo expInfo = expVersionSet.versionInfoList[n];
                        if(null == expInfo)
                            continue;

                        if(expInfo.assetPath.Equals(srcInfo.assetPath))
                        {
                            //判断文件是否存在，如不存在仍然需要拷贝
                            if(File.Exists(Application.dataPath + "/" + ALAssetBundleCommon.getAssetExportCompressAssetPath(_buildTarget, expInfo.assetPath)))
                            {
                                exportedInfo = expInfo;
                                break;
                            }
                        }
                    }

                    //判断是否有对应信息，以及信息版本号是否对应，不对应则添加到需要导出队列
                    if(null == exportedInfo || exportedInfo.versionNum != srcInfo.versionNum)
                    {
                        needExportFilePath.Add(srcInfo.assetPath);
                    }
                }
                //逐个文件判断是否需要删除
                for(int i = 0; i < expVersionSet.versionInfoList.Count; i++)
                {
                    ALAssetBundleVersionInfo expInfo = expVersionSet.versionInfoList[i];
                    if(null == expInfo)
                        continue;

                    //在导出的版本信息中查找需要对应的信息
                    ALAssetBundleVersionInfo srcedInfo = null;
                    for(int n = 0; n < srcVersionSet.versionInfoList.Count; n++)
                    {
                        ALAssetBundleVersionInfo srcInfo = srcVersionSet.versionInfoList[n];
                        if(null == srcInfo)
                            continue;

                        if(srcInfo.assetPath.Equals(expInfo.assetPath))
                        {
                            srcedInfo = expInfo;
                            break;
                        }
                    }

                    //判断是否有对应信息，如不存在对应信息则需要删除已导出文件
                    if(null == srcedInfo)
                    {
                        //删除对应的文件
                        needRemoveFilePath.Add(expInfo.assetPath);
                    }
                }
            }
            else
            {
                //此时需要导出所有源文件

                //逐个文件放入需要导出对象中
                for(int i = 0; i < srcVersionSet.versionInfoList.Count; i++)
                {
                    ALAssetBundleVersionInfo srcInfo = srcVersionSet.versionInfoList[i];
                    if(null == srcInfo)
                        continue;

                    needExportFilePath.Add(srcInfo.assetPath);
                }
            }

            srcVersionSet = null;
            expVersionSet = null;

            //判断是否有文件需要加密，如无则直接进行版本导出
            if(needExportFilePath.Count <= 0 && needRemoveFilePath.Count < 0)
                return null;

            //创建对应控制对象并返回
            ALAssetBundleCompressThreadController controller =
                new ALAssetBundleCompressThreadController(Application.dataPath + "/" + ALAssetBundleCommon.getAssetExportFolderAssetPath(_buildTarget)
                    , Application.dataPath + "/" + ALAssetBundleCommon.getAssetExportCompressFolderAssetPath(_buildTarget)
                    , needExportFilePath, needRemoveFilePath);

            //开启线程
            controller.startThread();
            //返回对象
            return controller;
        }
        public static void synCompressAsset(BuildTarget _buildTarget)
        {
            //需要处理的文件路径
            List<string> needExportFilePath = new List<string>();
            //需要删除的文件路径输出部分
            List<string> needRemoveFilePath = new List<string>();

            //读取源数据的版本信息
            AssetBundle assetBundle = AssetBundle.LoadFromFile(Application.dataPath + "/" + ALAssetBundleCommon.getAssetExportAssetPath(_buildTarget, "version"));
            //读取原始资源对象
            ALSOAssetBundleVersionSet srcVersionSet = null;
            if(null != assetBundle)
            {
                srcVersionSet = assetBundle.LoadAsset("version") as ALSOAssetBundleVersionSet;
                assetBundle.Unload(false);
                assetBundle = null;
            }
            else
            {
                UnityEngine.Debug.LogError("Src Folder Has not version info!");
                return;
            }

            //判断数据是否有效
            if(null == srcVersionSet)
            {
                UnityEngine.Debug.LogError("Src Folder Has not version set!");
                return;
            }

            //读取目标位置的版本信息对象
            UnityWebRequest exportedVersionWWW = UnityWebRequestAssetBundle.GetAssetBundle("file:///" + Application.dataPath + "/" + ALAssetBundleCommon.getAssetExportCompressAssetPath(_buildTarget, "version"));
            UnityWebRequestAsyncOperation asyncOp = exportedVersionWWW.SendWebRequest();
            //WWW exportedVersionWWW = new WWW("file:///" + Application.dataPath + "/" + ALAssetBundleCommon.getAssetExportCompressAssetPath(_buildTarget, "version"));

            int timeCount = 0;
            while(!asyncOp.isDone)
            {
                System.Threading.Thread.Sleep(100);

                timeCount++;
                if(timeCount > 100)
                {
                    //加载超时直接返回
                    UnityEngine.Debug.LogError("加载version超时!");
                    if(null != exportedVersionWWW)
                    {
                        exportedVersionWWW.Abort();
                        exportedVersionWWW.Dispose();
                    }
                    exportedVersionWWW = null;
                    return;
                }
            }

            ALSOAssetBundleVersionSet expVersionSet = null;
            //判断加载结果
#if UNITY_2020
            if (exportedVersionWWW.result == UnityWebRequest.Result.ConnectionError
                || exportedVersionWWW.result == UnityWebRequest.Result.DataProcessingError
                || exportedVersionWWW.result == UnityWebRequest.Result.ProtocolError)
#else
            if(exportedVersionWWW.isNetworkError || exportedVersionWWW.isHttpError)
#endif
            {
            }
            else
            {
                AssetBundle exportVersionAssetBundle = DownloadHandlerAssetBundle.GetContent(exportedVersionWWW);
                if(null != exportVersionAssetBundle)
                {
                    expVersionSet = exportVersionAssetBundle.LoadAsset("version") as ALSOAssetBundleVersionSet;
                }

                //释放加载资源
                if(null != exportedVersionWWW)
                {
                    exportedVersionWWW.Abort();
                    exportedVersionWWW.Dispose();
                }
                exportedVersionWWW = null;
                if(null != exportVersionAssetBundle)
                    exportVersionAssetBundle.Unload(false);
                exportVersionAssetBundle = null;
            }

            //判断导出位置的版本信息是否有效
            if(null != expVersionSet)
            {
                //逐个文件判断是否需要导出
                for(int i = 0; i < srcVersionSet.versionInfoList.Count; i++)
                {
                    ALAssetBundleVersionInfo srcInfo = srcVersionSet.versionInfoList[i];
                    if(null == srcInfo)
                        continue;

                    //在导出的版本信息中查找需要对应的信息
                    ALAssetBundleVersionInfo exportedInfo = null;
                    for(int n = 0; n < expVersionSet.versionInfoList.Count; n++)
                    {
                        ALAssetBundleVersionInfo expInfo = expVersionSet.versionInfoList[n];
                        if(null == expInfo)
                            continue;

                        if(expInfo.assetPath.Equals(srcInfo.assetPath))
                        {
                            //判断文件是否存在，如不存在仍然需要拷贝
                            if(File.Exists(Application.dataPath + "/" + ALAssetBundleCommon.getAssetExportCompressAssetPath(_buildTarget, expInfo.assetPath)))
                            {
                                exportedInfo = expInfo;
                                break;
                            }
                        }
                    }

                    //判断是否有对应信息，以及信息版本号是否对应，不对应则添加到需要导出队列
                    if(null == exportedInfo || exportedInfo.versionNum != srcInfo.versionNum)
                    {
                        needExportFilePath.Add(srcInfo.assetPath);
                    }
                }
                //逐个文件判断是否需要删除
                for(int i = 0; i < expVersionSet.versionInfoList.Count; i++)
                {
                    ALAssetBundleVersionInfo expInfo = expVersionSet.versionInfoList[i];
                    if(null == expInfo)
                        continue;

                    //在导出的版本信息中查找需要对应的信息
                    ALAssetBundleVersionInfo srcedInfo = null;
                    for(int n = 0; n < srcVersionSet.versionInfoList.Count; n++)
                    {
                        ALAssetBundleVersionInfo srcInfo = srcVersionSet.versionInfoList[n];
                        if(null == srcInfo)
                            continue;

                        if(srcInfo.assetPath.Equals(expInfo.assetPath))
                        {
                            srcedInfo = expInfo;
                            break;
                        }
                    }

                    //判断是否有对应信息，如不存在对应信息则需要删除已导出文件
                    if(null == srcedInfo)
                    {
                        //删除对应的文件
                        needRemoveFilePath.Add(expInfo.assetPath);
                    }
                }
            }
            else
            {
                //此时需要导出所有源文件

                //逐个文件放入需要导出对象中
                for(int i = 0; i < srcVersionSet.versionInfoList.Count; i++)
                {
                    ALAssetBundleVersionInfo srcInfo = srcVersionSet.versionInfoList[i];
                    if(null == srcInfo)
                        continue;

                    needExportFilePath.Add(srcInfo.assetPath);
                }
            }

            srcVersionSet = null;
            expVersionSet = null;

            //判断是否有文件需要加密，如无则直接进行版本导出
            if(needExportFilePath.Count <= 0 && needRemoveFilePath.Count <= 0)
                return;

            //创建对应控制对象并返回
            ALAssetBundleCompressThreadController controller =
                new ALAssetBundleCompressThreadController(Application.dataPath + "/" + ALAssetBundleCommon.getAssetExportFolderAssetPath(_buildTarget)
                    , Application.dataPath + "/" + ALAssetBundleCommon.getAssetExportCompressFolderAssetPath(_buildTarget)
                    , needExportFilePath, needRemoveFilePath);

            //开启线程
            controller.syncDeal();
        }

        /// <summary>
        /// 回滚对应的版本号配置，根据带入的version文件路径
        /// </summary>
        /// <param name="_versionPath"></param>
        public static bool fallbackVersion(string _versionPath)
        {
            //加载当前打包的版本文件信息
            ALSOAssetBundleVersionSet curPackVersionSet = Resources.Load<ALSOAssetBundleVersionSet>("__ALAssetBundle/version");

            //加载资源
            if(!File.Exists(_versionPath))
            {
                UnityEngine.Debug.LogError($"带入的回滚版本源路径 versionPath：{_versionPath}  不存在");
                return false;
            }

            //加载AB包
            AssetBundle srcOldVersion = AssetBundle.LoadFromFile(_versionPath);
            if(curPackVersionSet == null || srcOldVersion == null)
            {
                UnityEngine.Debug.LogError($"加载的回滚源版本AB包错误 versionPath：{_versionPath} ");
                return false;
            }

            try
            {
                //加载数据集
                ALSOAssetBundleVersionSet srcOldVersionSet = srcOldVersion.LoadAsset<ALSOAssetBundleVersionSet>("version");
                if(srcOldVersionSet == null)
                {
                    UnityEngine.Debug.LogError($"加载的回滚源版本信息对象错误 versionPath：{_versionPath} ");
                    return false;
                }

                //复制资源总版本号
                curPackVersionSet.versionNum = srcOldVersionSet.versionNum;
                //创建队列
                List<ALAssetBundleVersionInfo> addInfos = new List<ALAssetBundleVersionInfo>();

                //逐个遍历设置
                Debug.LogError($"开始进行数据回滚...");
                Debug.LogError($"旧打包版本信息数量：{srcOldVersionSet.versionInfoList.Count}");
                Debug.LogError($"当前打包版本信息数量：{curPackVersionSet.versionInfoList.Count}");
                for(int i = 0; i < srcOldVersionSet.versionInfoList.Count; i++)
                {
                    ALAssetBundleVersionInfo oldVInfo = srcOldVersionSet.versionInfoList[i];
                    //是否检测完成
                    bool hasChecked = false;

                    //遍历当前打包数据检查
                    for(int j = 0; j < curPackVersionSet.versionInfoList.Count; j++)
                    {
                        ALAssetBundleVersionInfo tmpInfo = curPackVersionSet.versionInfoList[j];
                        if(tmpInfo.assetPath == oldVInfo.assetPath)
                        {
                            //比对hash值
                            if(tmpInfo.hashTag == oldVInfo.hashTag)
                            {
                                curPackVersionSet.versionInfoList[j].versionNum = oldVInfo.versionNum;
                                hasChecked = true;
                                break;
                            }
                            else
                            {
                                Debug.LogWarning($"hashTag不匹配，仍然标记回滚Asset：" + oldVInfo.assetPath);
                                Debug.LogWarning($"-- 当前打包版本信息：" + tmpInfo.versionNum);
                                Debug.LogWarning($"-- 旧资源包版本信息：" + oldVInfo.versionNum);
                                curPackVersionSet.versionInfoList[j].versionNum = oldVInfo.versionNum;
                                hasChecked = true;
                            }
                        }
                    }

                    //未匹配到资源信息
                    if(!hasChecked)
                    {
                        UnityEngine.Debug.LogError($"在当前资源集有未匹配的旧资源信息 versionPath：{_versionPath} ");
                        //添加到资源信息
                        addInfos.Add(oldVInfo);
                    }
                }

                if(addInfos.Count > 0)
                {
                    UnityEngine.Debug.LogError($"在当前资源集有未匹配的旧资源信息数量 ：{addInfos.Count} ");
                    return false;
                }

                return true;
            }
            finally
            {
                if(null != srcOldVersion)
                {
                    srcOldVersion.Unload(false);
                    srcOldVersion = null;
                }

                EditorUtility.SetDirty(curPackVersionSet);//告诉Uinty 数据已经修改过需要进行保存
                AssetDatabase.SaveAssets();

                Debug.LogError($"数据回滚操作结束...");
            }
        }

        /***************
         * 根据带入的平台标记打包对应的版本信息
         **/
        public static void packCompressVersion(BuildTarget _buildTarget)
        {
            string iosP = Application.dataPath + "/" + ALAssetBundleCommon.getAssetExportFolderAssetPath(_buildTarget);
            string expP = Application.dataPath + "/" + ALAssetBundleCommon.getAssetExportCompressFolderAssetPath(_buildTarget);

            //判断路径是否存在
            if(!Directory.Exists(expP))
                return;

            //读取源数据的版本信息
            AssetBundle assetBundle = AssetBundle.LoadFromFile(iosP + "/version");

            //读取原始资源对象
            ALSOAssetBundleVersionSet srcVersionSet = assetBundle.LoadAsset("version") as ALSOAssetBundleVersionSet;
            if(null == srcVersionSet)
                return;

            //创建新的数据对象
            ALSOAssetBundleVersionSet newVersionSet = ScriptableObject.CreateInstance<ALSOAssetBundleVersionSet>();
            newVersionSet.versionNum = srcVersionSet.versionNum;

            //将资源从源目录压缩到指定目录
            //查询源目录的所有文件
            DirectoryInfo srcDirectionInfo = new DirectoryInfo(iosP);
            DirectoryInfo exportDirectionInfo = new DirectoryInfo(expP);
            //逐个version信息进行处理
            for(int i = 0; i < srcVersionSet.versionInfoList.Count; i++)
            {
                //获取版本信息
                ALAssetBundleVersionInfo versionInfo = srcVersionSet.versionInfoList[i];
                //获取文件信息
                FileInfo file = new FileInfo(iosP + "/" + versionInfo.assetPath);
                if(null == file)
                {
                    UnityEngine.Debug.LogError("Src File : " + versionInfo.assetPath + " don't exist!");
                    continue;
                }

                if(file.Name.Equals("version", StringComparison.OrdinalIgnoreCase))
                    continue;

                //替换目录，在新文件位置创建目录
                string newDirectory = file.DirectoryName.Replace(srcDirectionInfo.FullName, exportDirectionInfo.FullName);
                //判断路径是否存在
                if(!Directory.Exists(newDirectory))
                {
                    UnityEngine.Debug.LogError("Export Version for File : " + versionInfo.assetPath + " don't exist!");
                    continue;
                }

                //获取文件信息
                FileInfo fileInfo = new FileInfo(newDirectory + "/" + file.Name);

                //获取对应文件的版本信息，并添加到数据集中
                ALAssetBundleVersionInfo newVInfo = new ALAssetBundleVersionInfo();

                //获取文件的纯净路径
                newVInfo.assetPath = versionInfo.assetPath;
                newVInfo.versionNum = versionInfo.versionNum;
                newVInfo.fileSize = (int)fileInfo.Length;
                newVInfo.hashTag = versionInfo.hashTag;

                newVersionSet.versionInfoList.Add(newVInfo);
            }

            //将游戏导出资源拷贝至最终目录，并覆盖
            File.Copy(iosP + "/version", expP + "/version", true);

            //输出导出完毕
            UnityEngine.Debug.LogError("Export Web Resource Version Done!");

            srcVersionSet = null;
            newVersionSet = null;
            assetBundle.Unload(true);

            //回收内存
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
        }

        /**********************
         * 根据带入的打包平台以及打包资源的打包返回值，处理资源打包的所有版本信息
         * 将新打包的资源版本号更新入版本控制结构体，并重新进行打包操作
         **/
        public static void dealAssetBundleVersionInfo(AssetBundleManifest _manifestObj, BuildTarget _buildTarget, bool _exportDependence, long _versionNum)
        {
            //信息对象为空则表示无新打包操作，直接返回
            if(null == _manifestObj)
                return;

            //检测对应平台的版本信息结构体是否存在
            ALSOAssetBundleVersionSet versionSet = checkBuildVersionSet();
            if(null == versionSet)
                return;

            //检测对应平台的依赖关系信息结构体是否存在
            ALSOAssetBundleDependenceSet dependenceSet = checkBuildDependenceSet();
            if(null == dependenceSet)
                return;

            //获取所有asset
            string[] allAssets = _manifestObj.GetAllAssetBundles();

            List<ALAssetBundleVersionInfo> versionList = new List<ALAssetBundleVersionInfo>();
            //将新打包的所有对象的版本信息使用时间戳更新
            for(int i = 0; i < allAssets.Length; i++)
            {
                string asset = allAssets[i];
                if(null == asset || asset.Length <= 0 || asset == "version")
                    continue;

                ALAssetBundleVersionInfo assetVersionInfo = null;
                //查询对应的版本信息
                for(int n = 0; n < versionSet.versionInfoList.Count; n++)
                {
                    ALAssetBundleVersionInfo versionInfo = versionSet.versionInfoList[n];
                    if(null == versionInfo)
                        continue;

                    if(versionInfo.assetPath == asset)
                    {
                        assetVersionInfo = versionInfo;
                        break;
                    }
                }

                string hashTag = _manifestObj.GetAssetBundleHash(asset).ToString();
                //当未检索到对象时创建新对象并插入
                if(null == assetVersionInfo || assetVersionInfo.hashTag != hashTag)
                {
                    //判断是否原先无此数据或文件已更改
                    assetVersionInfo = new ALAssetBundleVersionInfo();
                    assetVersionInfo.assetPath = asset;
                    assetVersionInfo.hashTag = hashTag;
                    assetVersionInfo.versionNum = ALCommon.getNowTimeTagS();

                    //获取对应的绝对路径，以此获取文件大小
                    FileInfo fileInfo = new FileInfo(Application.dataPath + "/" + getAssetExportFolderAssetPath(_buildTarget) + "/" + asset);
                    assetVersionInfo.fileSize = fileInfo.Length;

                    versionList.Add(assetVersionInfo);
                }
                else
                {
                    //拷贝原先数据
                    ALAssetBundleVersionInfo newAssetVersionInfo = new ALAssetBundleVersionInfo();
                    newAssetVersionInfo.assetPath = asset;
                    newAssetVersionInfo.hashTag = hashTag;
                    newAssetVersionInfo.versionNum = assetVersionInfo.versionNum;
                    newAssetVersionInfo.fileSize = assetVersionInfo.fileSize;

                    //重新做一次文件大小检查
                    FileInfo fileInfo = new FileInfo(Application.dataPath + "/" + getAssetExportFolderAssetPath(_buildTarget) + "/" + asset);
                    if(fileInfo.Length != newAssetVersionInfo.fileSize)
                    {
                        newAssetVersionInfo.versionNum = ALCommon.getNowTimeTagS();
                        newAssetVersionInfo.fileSize = fileInfo.Length;
                    }

                    versionList.Add(newAssetVersionInfo);
                }
            }

            ALAssetBundleVersionInfo dependenceVersionInfo = null;
            if(_exportDependence)
            {
                //创建新的依赖对象
                ALSOAssetBundleDependenceSet newDependenceSet = ScriptableObject.CreateInstance<ALSOAssetBundleDependenceSet>();

                //将新打包的所有对象的版本信息使用时间戳更新
                for(int i = 0; i < allAssets.Length; i++)
                {
                    string asset = allAssets[i];
                    if(null == asset || asset == "version")
                        continue;

                    //开始依赖部分导出
                    //这里需要特别注意的是，GetAllDependencies 这个函数将会使得 _manifestObj 数据对象变得无效！
                    string[] dependences = _manifestObj.GetDirectDependencies(asset);
                    //当未检索到对象时创建新对象并插入
                    if(dependences.Length > 0)
                    {
                        //判断是否原先无此数据或文件已更改
                        ALAssetBundleDependenceInfo assetDependenceInfo = new ALAssetBundleDependenceInfo();
                        assetDependenceInfo.assetPath = asset;
                        assetDependenceInfo.dependenceList = new List<string>();
                        for(int n = 0; n < dependences.Length; n++)
                        {
                            string dependenceStr = dependences[n];
                            if(null == dependenceStr)
                                continue;

                            if(dependenceStr.StartsWith("common/"))
                                continue;

                            assetDependenceInfo.dependenceList.Add(dependenceStr);
                        }

                        if(assetDependenceInfo.dependenceList.Count > 0)
                            newDependenceSet.dependenceInfoList.Add(assetDependenceInfo);
                    }
                }

                AssetDatabase.CreateAsset(newDependenceSet, "Assets/Resources/__ALAssetBundle/__dependence.asset");

                //强制打包Dependence信息
                rebuildDependence(_buildTarget);

                //构建版本信息返回
                dependenceVersionInfo = new ALAssetBundleVersionInfo();
                dependenceVersionInfo.assetPath = "__dependence.unity3d";
                dependenceVersionInfo.versionNum = ALCommon.getNowTimeMill();
                dependenceVersionInfo.hashTag = dependenceVersionInfo.versionNum.ToString();

                //获取对应的绝对路径，以此获取文件大小
                FileInfo dependenceFileInfo = new FileInfo(Application.dataPath + "/" + getAssetExportFolderAssetPath(_buildTarget) + "/__dependence.unity3d");
                dependenceVersionInfo.fileSize = dependenceFileInfo.Length;
            }


            //创建新的版本对象
            ALSOAssetBundleVersionSet newVersionSet = ScriptableObject.CreateInstance<ALSOAssetBundleVersionSet>();
            //修改版本号
            if(0 == _versionNum)
                newVersionSet.versionNum = ALCommon.getNowTimeTagS();
            else
                newVersionSet.versionNum = _versionNum;

            //加入版本号队列
            if(null != dependenceVersionInfo)
                versionList.Add(dependenceVersionInfo);
            //将所有数据加入总队列
            newVersionSet.versionInfoList.AddRange(versionList);

            AssetDatabase.CreateAsset(newVersionSet, "Assets/Resources/__ALAssetBundle/version.asset");
            CreateVersionNumTxtFile(newVersionSet.versionNum);

            //强制打包version信息
            rebuildVersion(_buildTarget);
        }

        /****************
         * 强制重新打包依赖关系资源
         **/
        public static void rebuildDependence(BuildTarget _buildTarget)
        {
            //构建编译对象
            AssetBundleBuild[] buildMap = new AssetBundleBuild[1];

            buildMap[0].assetBundleName = "__dependence.unity3d";

            string[] assetNames = { "Assets/Resources/__ALAssetBundle/__dependence.asset" };
            buildMap[0].assetNames = assetNames;

            //打包操作
            BuildPipeline.BuildAssetBundles(getAssetExportFolderFullPath(_buildTarget), buildMap
                , BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.ForceRebuildAssetBundle
                    | BuildAssetBundleOptions.ChunkBasedCompression//进行压缩处理
                , _buildTarget);
        }

        /**********************
         * 修改当前Version中对应对象的值
         **/
        public static void setVersion(string _assetPath, string _hashTag, string _filePath, long _versionNum, BuildTarget _buildTarget)
        {
            //检测对应平台的版本信息结构体是否存在
            ALSOAssetBundleVersionSet versionSet = checkBuildVersionSet();
            if(null == versionSet)
                return;

            //创建新的版本对象
            ALSOAssetBundleVersionSet newVersionSet = ScriptableObject.CreateInstance<ALSOAssetBundleVersionSet>();
            //修改版本号
            newVersionSet.versionNum = ALCommon.getNowTimeTagS();

            if(null == _assetPath || _assetPath == "version")
                return;

            ALAssetBundleVersionInfo assetVersionInfo = null;
            //查询对应的版本信息
            for(int n = 0; n < versionSet.versionInfoList.Count; n++)
            {
                ALAssetBundleVersionInfo versionInfo = versionSet.versionInfoList[n];
                if(null == versionInfo)
                    continue;

                if(versionInfo.assetPath == _assetPath)
                {
                    assetVersionInfo = versionInfo;
                    break;
                }
            }

            //当未检索到对象时创建新对象并插入
            if(null == assetVersionInfo || assetVersionInfo.hashTag != _hashTag)
            {
                //判断是否原先无此数据或文件已更改
                assetVersionInfo = new ALAssetBundleVersionInfo();
                assetVersionInfo.assetPath = _assetPath;
                assetVersionInfo.hashTag = _hashTag;
                assetVersionInfo.versionNum = _versionNum;

                //获取对应的绝对路径，以此获取文件大小
                FileInfo fileInfo = new FileInfo(Application.dataPath + "/" + _filePath);
                if(null == fileInfo || !fileInfo.Exists)
                {
                    UnityEngine.Debug.LogError("can not find file: " + Application.dataPath + "/" + _filePath);
                    return;
                }
                assetVersionInfo.fileSize = fileInfo.Length;

                newVersionSet.versionInfoList.Add(assetVersionInfo);
            }
            else
            {
                //拷贝原先数据
                ALAssetBundleVersionInfo newAssetVersionInfo = new ALAssetBundleVersionInfo();
                newAssetVersionInfo.assetPath = _assetPath;
                newAssetVersionInfo.hashTag = _hashTag;
                newAssetVersionInfo.versionNum = assetVersionInfo.versionNum;
                newAssetVersionInfo.fileSize = assetVersionInfo.fileSize;

                newVersionSet.versionInfoList.Add(newAssetVersionInfo);
            }

            AssetDatabase.CreateAsset(newVersionSet, "Assets/Resources/__ALAssetBundle/version.asset");
            CreateVersionNumTxtFile(newVersionSet.versionNum);

            //强制打包version信息
            rebuildVersion(_buildTarget);
        }

        /**********************
         * 刷新版本信息
         **/
        public static void refreshVersionNum(long _versionNum, BuildTarget _buildTarget)
        {
            //检测对应平台的版本信息结构体是否存在
            ALSOAssetBundleVersionSet versionSet = checkBuildVersionSet();
            if(null == versionSet)
                return;

            //创建新的版本对象
            ALSOAssetBundleVersionSet newVersionSet = ScriptableObject.CreateInstance<ALSOAssetBundleVersionSet>();
            //修改版本号
            if(0 == _versionNum)
                newVersionSet.versionNum = ALCommon.getNowTimeTagS();
            else
                newVersionSet.versionNum = _versionNum;

            //查询对应的版本信息
            for(int n = 0; n < versionSet.versionInfoList.Count; n++)
            {
                ALAssetBundleVersionInfo versionInfo = versionSet.versionInfoList[n];
                if(null == versionInfo)
                    continue;

                newVersionSet.versionInfoList.Add(versionInfo);
            }

            AssetDatabase.CreateAsset(newVersionSet, "Assets/Resources/__ALAssetBundle/version.asset");
            CreateVersionNumTxtFile(newVersionSet.versionNum);

            //强制打包version信息
            rebuildVersion(_buildTarget);
        }

        /****************
         * 强制重新打包资源
         **/
        public static void rebuildVersion(BuildTarget _buildTarget)
        {
            //构建编译对象
            AssetBundleBuild[] buildMap = new AssetBundleBuild[1];

            buildMap[0].assetBundleName = "version";

            string[] assetNames = { "Assets/Resources/__ALAssetBundle/version.asset" };
            buildMap[0].assetNames = assetNames;

            //打包操作
            BuildPipeline.BuildAssetBundles(getAssetExportFolderFullPath(_buildTarget), buildMap
                , BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.ForceRebuildAssetBundle
                    //| BuildAssetBundleOptions.UncompressedAssetBundle 进行压缩则不可设置这个
                    | BuildAssetBundleOptions.ChunkBasedCompression//进行压缩处理
                , _buildTarget);
        }

        /// <summary>
        /// 单独打包相关文件
        /// </summary>
        /// <param name="_buildTarget"></param>
        public static AssetBundleManifest customBuild(string _assetBundlePath, string[] _buildAssetArr, bool _ifCompress, BuildTarget _buildTarget)
        {
            //构建编译对象
            AssetBundleBuild[] buildMap = new AssetBundleBuild[1];

            buildMap[0].assetBundleName = _assetBundlePath;
            buildMap[0].assetNames = _buildAssetArr;

            BuildAssetBundleOptions option = BuildAssetBundleOptions.UncompressedAssetBundle | BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.ForceRebuildAssetBundle;
            if(_ifCompress)
                option = option & ~BuildAssetBundleOptions.UncompressedAssetBundle | BuildAssetBundleOptions.ChunkBasedCompression;
            //打包操作
            return BuildPipeline.BuildAssetBundles(getAssetExportFolderFullPath(_buildTarget), buildMap
                , option, _buildTarget);
        }
        public static AssetBundleManifest customBuild(AssetBundleBuild[] _buildArr, bool _ifCompress, BuildTarget _buildTarget)
        {
            if(null == _buildArr)
                return null;

            BuildAssetBundleOptions option = BuildAssetBundleOptions.UncompressedAssetBundle | BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.ForceRebuildAssetBundle;
            if(_ifCompress)
                option = option & ~BuildAssetBundleOptions.UncompressedAssetBundle | BuildAssetBundleOptions.ChunkBasedCompression;
            //打包操作
            return BuildPipeline.BuildAssetBundles(getAssetExportFolderFullPath(_buildTarget), _buildArr
                , option, _buildTarget);
        }
        public static AssetBundleBuild buildBuildInfo(string _assetBundlePath, string[] _buildAssetArr)
        {
            //构建编译对象
            AssetBundleBuild buildInfo = new AssetBundleBuild();

            buildInfo.assetBundleName = _assetBundlePath;
            buildInfo.assetNames = _buildAssetArr;

            return buildInfo;
        }

        /*****************
         * 从版本集合中删除对应资源的版本信息
         **/
        public static void removeVersionInfo(string _assetPath)
        {
            ALSOAssetBundleVersionSet versionSet = AssetDatabase.LoadAssetAtPath("Assets/Resources/__ALAssetBundle/version.asset", typeof(ALSOAssetBundleVersionSet)) as ALSOAssetBundleVersionSet;
            if(null == versionSet)
                return;

            //创建新的版本对象
            ALSOAssetBundleVersionSet newVersionSet = ScriptableObject.CreateInstance<ALSOAssetBundleVersionSet>();
            //修改版本号
            newVersionSet.versionNum = ALCommon.getNowTimeTagS();

            for(int i = 0; i < versionSet.versionInfoList.Count; i++)
            {
                ALAssetBundleVersionInfo versionInfo = versionSet.versionInfoList[i];
                if(null == versionInfo)
                    continue;

                if(versionInfo.assetPath == _assetPath)
                {
                    versionSet.versionInfoList.RemoveAt(i);
                    continue;
                }
                else
                {
                    //插入到新的集合
                    //拷贝原先数据
                    ALAssetBundleVersionInfo newAssetVersionInfo = new ALAssetBundleVersionInfo();
                    newAssetVersionInfo.assetPath = versionInfo.assetPath;
                    newAssetVersionInfo.hashTag = versionInfo.hashTag;
                    newAssetVersionInfo.versionNum = versionInfo.versionNum;
                    newAssetVersionInfo.fileSize = versionInfo.fileSize;

                    newVersionSet.versionInfoList.Add(newAssetVersionInfo);
                }
            }
            AssetDatabase.CreateAsset(newVersionSet, "Assets/Resources/__ALAssetBundle/version.asset");
            CreateVersionNumTxtFile(newVersionSet.versionNum);
        }

        /*****************
         * 检查版本数据集对象
         **/
        public static ALSOAssetBundleVersionSet checkBuildVersionSet()
        {
            ALSOAssetBundleVersionSet versionSet = AssetDatabase.LoadAssetAtPath("Assets/Resources/__ALAssetBundle/version.asset", typeof(ALSOAssetBundleVersionSet)) as ALSOAssetBundleVersionSet;
            if(null == versionSet)
            {
                //当对象不存在则创建
                versionSet = ScriptableObject.CreateInstance<ALSOAssetBundleVersionSet>();
                AssetDatabase.CreateAsset(versionSet, "Assets/Resources/__ALAssetBundle/version.asset");
                CreateVersionNumTxtFile(versionSet.versionNum);
                //设置对象资源信息
                //AssetImporter.GetAtPath("Assets/Resources/__ALAssetBundle/version.asset").assetBundleName = "version";

                //重新载入对象
                //versionSet = AssetDatabase.LoadAssetAtPath("Assets/Resources/__ALAssetBundle/version.asset", typeof(ALSOLAssetBundleVersionSet)) as ALSOLAssetBundleVersionSet;
            }

            return versionSet;
        }

        /*****************
         * 检查依赖数据集对象
         **/
        public static ALSOAssetBundleDependenceSet checkBuildDependenceSet()
        {
            ALSOAssetBundleDependenceSet dependenceSet = AssetDatabase.LoadAssetAtPath("Assets/Resources/__ALAssetBundle/__dependence.asset", typeof(ALSOAssetBundleDependenceSet)) as ALSOAssetBundleDependenceSet;
            if(null == dependenceSet)
            {
                //当对象不存在则创建
                dependenceSet = ScriptableObject.CreateInstance<ALSOAssetBundleDependenceSet>();
                AssetDatabase.CreateAsset(dependenceSet, "Assets/Resources/__ALAssetBundle/__dependence.asset");
                //设置对象资源信息
                //AssetImporter.GetAtPath("Assets/Resources/__ALAssetBundle/version.asset").assetBundleName = "version";

                //重新载入对象
                //versionSet = AssetDatabase.LoadAssetAtPath("Assets/Resources/__ALAssetBundle/version.asset", typeof(ALSOLAssetBundleVersionSet)) as ALSOLAssetBundleVersionSet;
            }

            return dependenceSet;
        }

        /********************
         * 输出一个unity文件中的包含对象列表
         **/
        public static void printUnity3DFileList()
        {
            //选择需要输出的文件对象
            string objPath = EditorUtility.OpenFilePanel("Select Unity File Path", "", "");

            if(null == objPath || objPath.Length <= 0)
                return;

            AssetBundle assetBundle = AssetBundle.LoadFromFile(objPath);

            UnityEngine.Object[] objsList = assetBundle.LoadAllAssets();
            if(null != objsList)
            {
                //输出日志
                UnityEngine.Debug.LogError("Loaded Asset: " + objPath + " contain total: " + objsList.Length + " objects.");

                StringBuilder strBuilder = new StringBuilder();
                for(int i = 0; i < objsList.Length; i++)
                {
                    //输出对象名
                    strBuilder.Append(objsList[i].name + " - " + objsList[i].GetType())
                        .Append("\n");
                }

                //输出日志
                UnityEngine.Debug.LogError("Asset: " + objPath + " objects' name: " + strBuilder.ToString());
            }

            //卸载资源
            assetBundle.Unload(true);
        }

        /**************
         * 设置对应路径的资源对象的导出资源信息
         **/
        public static void setAssetBundleName(string _assetPath, string _assetBundleName)
        {
            AssetImporter assetImporter = AssetImporter.GetAtPath(_assetPath + ".asset");
            if(null == assetImporter)
            {
                UnityEngine.Debug.LogError("get AssetImporter is null: " + _assetPath);
                return;
            }

            assetImporter.assetBundleName = _assetBundleName;
        }
        public static void setAssetBundleName(string _assetPath, string _assetBundleName, string _variantName)
        {
            AssetImporter assetImporter = AssetImporter.GetAtPath(_assetPath + ".asset");
            if(null == assetImporter)
            {
                UnityEngine.Debug.LogError("get AssetImporter is null: " + _assetPath);
                return;
            }

            assetImporter.assetBundleName = _assetBundleName;
            assetImporter.assetBundleVariant = _variantName;
        }

        public static void setFileAssetBundleName(string _fileAssetPath, string _assetBundleName)
        {
            AssetImporter assetImporter = AssetImporter.GetAtPath(_fileAssetPath);
            if (null == assetImporter)
            {
                UnityEngine.Debug.LogError("get AssetImporter is null: " + _fileAssetPath);
                return;
            }

            assetImporter.assetBundleName = _assetBundleName;
        }
        public static void setFileAssetBundleName(string _fileAssetPath, string _assetBundleName, string _variantName)
        {
            AssetImporter assetImporter = AssetImporter.GetAtPath(_fileAssetPath);
            if (null == assetImporter)
            {
                UnityEngine.Debug.LogError("get AssetImporter is null: " + _fileAssetPath);
                return;
            }

            assetImporter.assetBundleName = _assetBundleName;
            assetImporter.assetBundleVariant = _variantName;
        }

        /**************
         * 资源版本号另存为 version_num.txt到_ALAssetBundle目录下
         **/
        public static void CreateVersionNumTxtFile(long _versionNum)
        {
            string path = Application.dataPath + "/Resources/__ALAssetBundle/version_num.txt";
            if(File.Exists(path))
                File.Delete(path);
            File.WriteAllText(path, _versionNum.ToString());
        }
    }
}

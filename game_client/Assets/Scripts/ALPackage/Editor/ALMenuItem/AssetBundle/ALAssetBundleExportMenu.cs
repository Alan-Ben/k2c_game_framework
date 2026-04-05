using System;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEditor;

/****************
 * 资源打包函数
 **/
namespace ALPackage
{
    public class ALAssetBundleExportMenu
    {
        //压缩的处理线程对象
        private static ALAssetBundleCompressThreadController _m_tcCompressThreadController = null;

        public static void exportAsset(bool _exportDependence, bool _ifCompress)
        {
            exportAsset(0, _exportDependence, _ifCompress);
        }
        
        //==== 打包资源
        public static void exportAsset(long _versionNum, bool _exportDependence, bool _ifCompress)
        {
#if UNITY_IPHONE
            //检查对应路径的目录是否存在，不存在则创建目录
            ALAssetBundleCommon.checkEnv(BuildTarget.iOS);

            //进行打包操作
            ALAssetBundleCommon.exportAsset(BuildTarget.iOS, _versionNum, _exportDependence, _ifCompress);
#endif
#if UNITY_ANDROID
            //检查对应路径的目录是否存在，不存在则创建目录
            ALAssetBundleCommon.checkEnv(BuildTarget.Android);

            //进行打包操作
            ALAssetBundleCommon.exportAsset(BuildTarget.Android, _versionNum, _exportDependence, _ifCompress);
#endif
#if UNITY_WEBPLAYER
            //检查对应路径的目录是否存在，不存在则创建目录
            ALAssetBundleCommon.checkEnv(BuildTarget.WebPlayer);

            //进行打包操作
            ALAssetBundleCommon.exportAsset(BuildTarget.WebPlayer, _versionNum, _exportDependence, _ifCompress);
#endif
#if UNITY_STANDALONE_WIN
            //检查对应路径的目录是否存在，不存在则创建目录
            ALAssetBundleCommon.checkEnv(BuildTarget.StandaloneWindows);

            //进行打包操作
            ALAssetBundleCommon.exportAsset(BuildTarget.StandaloneWindows, _versionNum, _exportDependence, _ifCompress);
#endif

            UnityEngine.Debug.LogError("Export AssetBundle Done!");
        }

        //==== 压缩资源
        public static void compressAsset()
        {
#if UNITY_IPHONE
            //进行压缩操作
            _m_tcCompressThreadController = ALAssetBundleCommon.compressAsset(BuildTarget.iOS);
#endif
#if UNITY_ANDROID
            //进行压缩操作
            _m_tcCompressThreadController = ALAssetBundleCommon.compressAsset(BuildTarget.Android);
#endif
#if UNITY_WEBPLAYER
            //进行压缩操作
            _m_tcCompressThreadController = ALAssetBundleCommon.compressAsset(BuildTarget.WebPlayer);
#endif
#if UNITY_STANDALONE_WIN
            //进行压缩操作
            _m_tcCompressThreadController = ALAssetBundleCommon.compressAsset(BuildTarget.StandaloneWindows);
#endif
        }
        //[MenuItem("ALMenu/AssetBundle/Compress AssetBundle", true)]
        public static bool isCompressAssetEnable()
        {
            if (null == _m_tcCompressThreadController)
                return true;

            if (_m_tcCompressThreadController.isWorking)
                return false;

            return true;
        }

        //[MenuItem("ALMenu/AssetBundle/Stop Compress AssetBundle")]
        public static void stopCompressAsset()
        {
            if (null != _m_tcCompressThreadController && _m_tcCompressThreadController.isWorking)
                _m_tcCompressThreadController.stopThread();
        }
        //[MenuItem("ALMenu/AssetBundle/Stop Compress AssetBundle", true)]
        public static bool isStopCompressAssetEnable()
        {
            if (null != _m_tcCompressThreadController && _m_tcCompressThreadController.isWorking)
                return true;

            return false;
        }

        //==== 打包资源版本
        public static void packCompressVersion()
        {
#if UNITY_IPHONE
            //执行打包版本对象的操作
            ALAssetBundleCommon.packCompressVersion(BuildTarget.iOS);
#endif
#if UNITY_ANDROID
            //执行打包版本对象的操作
            ALAssetBundleCommon.packCompressVersion(BuildTarget.Android);
#endif
#if UNITY_WEBPLAYER
            //执行打包版本对象的操作
            ALAssetBundleCommon.packCompressVersion(BuildTarget.WebPlayer);
#endif
#if UNITY_STANDALONE_WIN
            //执行打包版本对象的操作
            ALAssetBundleCommon.packCompressVersion(BuildTarget.StandaloneWindows);
#endif
        }

        public static bool isPackCompressVersionEnable()
        {
            if (null == _m_tcCompressThreadController)
                return true;

            if (_m_tcCompressThreadController.isWorking)
                return false;

            return true;
        }


        //输出一个导出的unity3d文件中的对象列表
        [MenuItem("ALMenu/AssetBundle/Print Unity3d Obj List", false, 20)]
        public static void printUnity3DObjList()
        {
            ALAssetBundleCommon.printUnity3DFileList();
        }
    }
}

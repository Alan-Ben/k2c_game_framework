using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

/*********************
 * 监控所有al模块的asset相关变更
 **/
namespace ALPackage
{
    //public class ALAssetBundleMonitor : AssetPostprocessor
    //{
    //    public void OnPostprocessAssetbundleNameChanged(string assetPath, string previousAssetBundleName, string newAssetBundleName)
    //    {
            //暂时不做资源相关处理，在导出时做比对处理
            //判断原先资源对象是否为空，是则需要删除原资源对象的对应资源导出文件信息
            //string[] preAssetPaths = AssetDatabase.GetAssetPathsFromAssetBundle(previousAssetBundleName);
            //if (null == preAssetPaths || preAssetPaths.Length <= 0)
            //{
            //    //删除所有资源对象
            //    ALAssetBundleCommon.removeExportFile(previousAssetBundleName);
            //    //删除相关资源版本信息
            //    ALAssetBundleCommon.removeVersionInfo(previousAssetBundleName);
            //}

            //Debug.Log("Asset " + assetPath + " has been moved from assetBundle " + previousAssetBundleName + " to assetBundle " + newAssetBundleName + ".");
    //    }
    //}
}

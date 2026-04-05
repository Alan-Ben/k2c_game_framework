using System;
using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    /// <summary>
    /// AB包加载类中的路径数据提供类接口对象
    /// </summary>
    public interface _IALAssetBundleLoadPathProvider
    {
        /// <summary>
        /// 获取路径
        /// </summary>
        string roolURL { get; }

        /// <summary>
        /// 根据带入的路径获取实际的加载路径
        /// </summary>
        /// <param name="_assetPath"></param>
        /// <returns></returns>
        string getAssetPath(string _assetPath);

#if UNITY_ANDROID
        /// <summary>
        /// 根据带入的路径获取安卓实际的加载路径
        /// </summary>
        /// <param name="_assetPath"></param>
        /// <returns></returns>
        string getAndroidAssetPath(string _assetPath);
#endif
    }
}

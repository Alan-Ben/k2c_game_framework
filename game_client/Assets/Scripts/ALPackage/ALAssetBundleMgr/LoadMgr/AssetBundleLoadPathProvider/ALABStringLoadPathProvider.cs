using System;
using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    /// <summary>
    /// AB包加载类中的路径数据提供类接口对象
    /// </summary>
    public class ALABStringLoadPathProvider : _IALAssetBundleLoadPathProvider
    {
        private string _m_sRootPath;

#if UNITY_ANDROID
        private string _m_sAndroidRootPath = "";
#endif

        public ALABStringLoadPathProvider(string _rootPath)
        {
            _m_sRootPath = _rootPath;
#if UNITY_ANDROID
    //新版本的unity不需要额外执行replace了
    #if UNITY_2021_3_OR_NEWER
            _m_sAndroidRootPath = _rootPath;
    #else
            _m_sAndroidRootPath = _rootPath.Replace("jar:file://", "").Replace("!/assets", "!assets");
    #endif
#endif
        }

        /// <summary>
        /// 获取路径
        /// </summary>
        public string roolURL { get { return _m_sRootPath; } }

        /// <summary>
        /// 根据带入的路径获取实际的加载路径
        /// </summary>
        /// <param name="_assetPath"></param>
        /// <returns></returns>
        public string getAssetPath(string _assetPath)
        {
            return string.Format("{0}/{1}", _m_sRootPath, _assetPath);
        }

#if UNITY_ANDROID
        /// <summary>
        /// 根据带入的路径获取安卓实际的加载路径
        /// </summary>
        /// <param name="_assetPath"></param>
        /// <returns></returns>
        public string getAndroidAssetPath(string _assetPath)
        {
            return string.Format("{0}/{1}", _m_sAndroidRootPath, _assetPath);
        }
#endif
    }
}

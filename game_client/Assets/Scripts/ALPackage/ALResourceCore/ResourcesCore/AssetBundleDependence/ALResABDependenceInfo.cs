using System.Collections.Generic;
using UnityEngine;

namespace ALPackage
{
    /// <summary>
    /// 各资源打包文件互相依赖的信息
    /// </summary>
    public class ALResABDependenceInfo
    {
        //判断依赖是否已经加载
        private bool _m_bIsLoadedDependence;
        private string _m_sAsset;
        //依赖包asset列表
        private List<string> _m_lDependenceList;

        public ALResABDependenceInfo(ALAssetBundleDependenceInfo _info)
        {
            _m_sAsset = _info.assetPath;
            _m_bIsLoadedDependence = false;
            _m_lDependenceList = new List<string>(_info.dependenceList);
        }

        public string asset { get { return _m_sAsset; } }
        public bool isLoadedDependence { get { return _m_bIsLoadedDependence; } }
        public List<string> dependenceList { get { return _m_lDependenceList; } }

        /// <summary>
        /// 设置已经加载了依赖
        /// </summary>
        public void setLoadedDependence()
        {
            _m_bIsLoadedDependence = true;
        }
    }
}
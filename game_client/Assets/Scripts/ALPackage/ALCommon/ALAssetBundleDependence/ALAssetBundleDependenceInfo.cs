using UnityEngine;
using System.Collections.Generic;

namespace ALPackage
{
    /// <summary>
    /// 单个AssetBundle依赖关系信息
    /// </summary>
    [System.Serializable]
    public class ALAssetBundleDependenceInfo
    {
        /** 用于数据查询的路径 */
        public string assetPath;
        public List<string> dependenceList;
    }
}

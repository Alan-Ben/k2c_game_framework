using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    /// <summary>
    /// 资源依赖关系数据集合
    /// </summary>
    public class ALSOAssetBundleDependenceSet : ScriptableObject
    {
        /** 每个文件的版本信息队列 */
        public List<ALAssetBundleDependenceInfo> dependenceInfoList = new List<ALAssetBundleDependenceInfo>();
    }
}

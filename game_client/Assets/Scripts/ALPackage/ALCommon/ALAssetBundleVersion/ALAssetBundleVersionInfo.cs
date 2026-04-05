using UnityEngine;

namespace ALPackage
{
    /**********************
     * 资源版本号存储对象
     **/
    [System.Serializable]
    public class ALAssetBundleVersionInfo
    {
        /** 用于数据查询的路径 */
        public string assetPath;
        public string hashTag;
        public long versionNum;
        public long fileSize;
    }
}

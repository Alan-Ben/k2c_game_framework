using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    /**********************
     * 资源版本号存储对象
     **/
    public class ALSOAssetBundleVersionSet : ScriptableObject
    {
        /** 版本编号 */
        public long versionNum;
        /** 每个文件的版本信息队列 */
        public List<ALAssetBundleVersionInfo> versionInfoList = new List<ALAssetBundleVersionInfo>();
    }
}

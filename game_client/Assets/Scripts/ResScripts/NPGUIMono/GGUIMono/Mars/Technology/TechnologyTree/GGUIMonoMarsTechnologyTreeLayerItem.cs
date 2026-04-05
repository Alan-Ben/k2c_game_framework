using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoMarsTechnologyTreeLayerItem : _TALUGUIMonoGridItem
    {
        [ALHeader("科技层级预制体父节点")]
        public Transform layerPrefabParent;

        [ALHeader("资源路径(ab包路径)")]
        public string layerPrefabAssetPath;

        [ALHeader("资源名前缀, 资源名格式为: {资源名前缀}_{当前层科技数量}_{下一层科技数量}")]
        public string layerPrefabObjNamePrefix;
    }
}
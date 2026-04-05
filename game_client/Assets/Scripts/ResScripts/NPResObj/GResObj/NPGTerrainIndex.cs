using UnityEngine;
using System.Collections;


[System.Serializable]
public class NPGTerrainIndex : BasicResIndexInfo
{
    /************
     * 资源加载路径
     **/
    protected override string customAssetPath { get { return $"terrain/terrain_{mainId}.unity3d"; } }
    protected override string customObjName { get { return $"terrain_{mainId}"; } }
}

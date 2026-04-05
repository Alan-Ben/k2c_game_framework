using System;
using UnityEngine;
using ALPackage;


/******************
 * 场景的资源索引对象
 **/
[System.Serializable]
public class NPPSceneIndex : BasicResIndexInfo
{
    /************
     * 资源加载路径
     **/
    protected override string customAssetPath { get { return $"scenes/pscene_{mainId}.unity3d"; } }
    protected override string customObjName { get { return $"pscene_{mainId}_{subId}"; } }
}

using UnityEngine;
using System.Collections;


[System.Serializable]
public class NPGSceneIndex : BasicResIndexInfo
{

    public void ParseFromString(string _str)
    {
        readIndex(_str, string.Empty);
    }

    /************
     * 资源加载路径
     **/
    protected override string customAssetPath { get { return $"scenes/gscene_{mainId}.unity3d"; } }
    protected override string customObjName { get { return $"gscene_{mainId}_{subId}"; } }
}

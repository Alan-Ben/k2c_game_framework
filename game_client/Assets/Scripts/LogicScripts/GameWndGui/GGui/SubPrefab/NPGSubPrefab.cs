using UnityEngine;
using System.Collections;
using ALPackage;
using System;
using System.Collections.Generic;


namespace GOE
{
    public class NPGSubPrefab : ALSubPrefabBasicObj
    {
        public NPGSubPrefab(NPCommonAssetPathInfo _assetPathInfo, Transform _parent)
            : base(_assetPathInfo.asset_path, _assetPathInfo.obj_name, _parent, GameResCore.instance)
        {
        }
        public NPGSubPrefab(string _monoAssetPath, string _monoObjName, Transform _parent)
            : base(_monoAssetPath, _monoObjName, _parent, GameResCore.instance)
        {
        }
    }
}

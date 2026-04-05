using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace ALPackage
{
    public class ALCreateCommonFunc
    {
        /** 创建对象的函数 */
        public static T createSOObj<T>() where T : ScriptableObject
        {
            string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            assetPath = assetPath.Substring(0, assetPath.LastIndexOf('/'));

            T obj = ScriptableObject.CreateInstance<T>();

            AssetDatabase.CreateAsset(obj, assetPath + "/" + typeof(T) + ".asset");

            return obj;
        }
        public static T createSOObj<T>(string _objName) where T : ScriptableObject
        {
            string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            assetPath = assetPath.Substring(0, assetPath.LastIndexOf('/'));

            T obj = ScriptableObject.CreateInstance<T>();

            AssetDatabase.CreateAsset(obj, assetPath + "/" + _objName + ".asset");

            return obj;
        }

        public static T createSOObjByFullPath<T>(string _fullPath) where T : ScriptableObject
        {
            T obj = ScriptableObject.CreateInstance<T>();

            if (_fullPath.EndsWith(".asset"))
            {
                AssetDatabase.CreateAsset(obj, _fullPath);
            }
            else
            {
                AssetDatabase.CreateAsset(obj, _fullPath + ".asset");
            }

            return obj;
        }
    }
}

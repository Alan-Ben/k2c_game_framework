using UnityEditor;
using UnityEngine;

namespace GOE
{
    public class UnityEditorExtension
    {
        public static T LoadLocalObjectByAssetBundleAndAssetName<T>(string assetPath, string objName) where T : UnityEngine.Object
        {
            string[] paths = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(assetPath, objName);
            if (paths == null || paths.Length <= 0)
                return null;

            T unitRefSet = AssetDatabase.LoadAssetAtPath<T>(paths[0]);
            if (unitRefSet == null)
                return null;

            return unitRefSet;
        }
        
        //根据资源信息获取资源图标
        public static Texture2D GetAssetIcon(string path)
        {
            Object obj = AssetDatabase.LoadAssetAtPath(path, typeof(Object));
            if (obj != null)
            {
                Texture2D icon = AssetPreview.GetMiniThumbnail(obj);
                if (icon == null)
                    icon = AssetPreview.GetMiniTypeThumbnail(obj.GetType());
                return icon;
            }
            return null;
        }    
        public static void HighLightObject(string _path)
        {
            var assetObject = AssetDatabase.LoadAssetAtPath(_path, typeof(UnityEngine.Object));
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = assetObject;
            EditorGUIUtility.PingObject(assetObject);
        }
    }
}
using System;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using UnityEditor;
using UnityEngine;

namespace GOE
{
    [CustomEditor(typeof(TMP_FontAsset))]
    public class TMPFontAssetInspector : TMP_FontAssetEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }

        private class SaveHandle : AssetModificationProcessor
        {
            private static string[] OnWillSaveAssets(string[] paths)
            {
                try
                {
                    foreach (string path in paths)
                    {
                        var assetType = AssetDatabase.GetMainAssetTypeAtPath(path);

                        // GetMainAssetTypeAtPath() sometimes returns null, for example, when path leads to .meta file
                        if (assetType == null)
                            continue;

                        // TMP_FontAsset is not marked as sealed class, so also checking for subclasses just in case
                        if (assetType != typeof(TMP_FontAsset) &&
                            assetType.IsSubclassOf(typeof(TMP_FontAsset)) == false)
                            continue;
                        
                        // Loading the asset only when we sure it is a font asset
                        var fontAsset = AssetDatabase.LoadMainAssetAtPath(path) as TMP_FontAsset;

                        // Theoretically this case is not possible due to asset type check above, but to be on the safe side check for null
                        if (fontAsset == null)
                            continue;

                        SerializedObject so = new SerializedObject(fontAsset);
                        if (fontAsset.atlasPopulationMode != AtlasPopulationMode.Dynamic || !(so.FindProperty("m_ClearDynamicDataOnBuild")?.boolValue ?? false))
                            continue;

                        // Debug.Log("Clearing font asset data at " + path);
                        fontAsset.ClearFontAssetData(true);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    Debug.LogError("Something went wrong while clearing dynamic font data. For more info look at log message above.");
                }
                
                return paths;
            }
        }
    }
}
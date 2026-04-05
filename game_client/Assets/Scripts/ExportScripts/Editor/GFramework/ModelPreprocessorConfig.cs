using System;
using System.Collections.Generic;
//using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace AssetPreprocessor.Scripts.Editor
{
    [CreateAssetMenu(menuName="ScriptableObject/AssetPreprocessor/ModelPreprocessorConfig")]
    public class ModelPreprocessorConfig : BasePreprocessorConfig
    {
        [Serializable]
        public class DeleteAnimCurveInfo
        {
            public string name;
            public string path;
        }
        [Header("Mesh Settings")]
        public bool EnableReadWrite = false;
        public ModelImporterNormals importNormals = ModelImporterNormals.Import;
        public ModelImporterTangents importTangents = ModelImporterTangents.CalculateMikk;
        public bool GenerateLightmapUVs;
        
        [Header("Rig settings")]
        public ModelImporterAvatarSetup AvatarSetup = ModelImporterAvatarSetup.NoAvatar;

        public ModelImporterSkinWeights skinWeights = ModelImporterSkinWeights.Standard;
        [Range(1, 255)]
        public int maxBonesPerVertex = 4;
        [Range(0.001f, 0.5f)]
        public float minBoneWeight = 0.001f;
        [Tooltip("仅当AvatarSetup设置为CreateFromThisModel时才有效，可优化骨骼信息，注意有骨骼挂点要关闭此选项")]
        public bool OptimizeGameObjects;
        
        [Header("Animation Settings")]
        public ModelImporterAnimationCompression ModelImporterAnimationCompression = ModelImporterAnimationCompression.Optimal;
        public bool DeleteScaleData = true;
        public List<string> ignoreDeleteScaleNames = new List<string>();
        public List<DeleteAnimCurveInfo> additionRemoveCurves = new List<DeleteAnimCurveInfo>();
        public bool CompressFloat = true;
        #region AnimationType为Human时的导入设置，用于是否将根运动烘培到骨骼，暂且无用注释
        /* AnimationType为Human时的导入设置，用于是否将根运动烘培到骨骼，暂且无用注释 
        [Header("Human Animation Processing")]
        public bool EnableAnimationPreprocessing;
        public bool KeepOriginalOrientation;
        public bool KeepOriginalPositionXZ;
        public bool KeepOriginalPositionY = true;
        public ClipAnimationMaskType ClipAnimationMaskType = ClipAnimationMaskType.None;
        public List<string> MaskBonesToEnable;
        */
        #endregion
    }
}

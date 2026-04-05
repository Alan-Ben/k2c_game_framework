using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AssetPreprocessor.Scripts.Editor
{
    public class ModelPreprocessor : AssetPostprocessor
    {
        /// <summary>
        /// 对模型资源序列化信息做处理，即大部分为模型导入Inspector视图中的选项
        /// </summary>
        private void OnPreprocessModel()
        {
            var modelImporter = (ModelImporter) assetImporter;
            var modelName = AssetPreprocessorUtils.GetAssetNameFromPath(modelImporter.assetPath);

            //获取模型导入设置配置文件，通过ProjectSetting中的配置获取
            var configs = AutoImportAssetProjectSettingConfig.GetOrCreateSettings().autoImportAssetConfig?.ModelPreprocessorConfigs;
            //var configs = AssetPreprocessorUtils.GetScriptableObjectsOfType<ModelPreprocessorConfig>();
            if (configs==null || configs.Count == 0)
            {
                return;
            }
            configs = configs
                .Where(conf => conf.ShouldUseConfigForAssetImporter(assetImporter))
                .ToList();
            configs.Sort((config1, config2) => config1.ConfigSortOrder.CompareTo(config2.ConfigSortOrder));
            ModelPreprocessorConfig config = null;
            for (var i = 0; i < configs.Count; i++)
            {
                var configToTest = configs[i];
                //运用第一个匹配配置
                config = configToTest;
                break;
            }
            if (config == null) return; //如果没有匹配配置则跳过预处理

            
            //初次导入自动设置会将灯光导入设置为false，如果关闭了灯光导入(代表二次修改)则跳过预处理
            //判断是否强制处理
            string isForce="";
            if (modelImporter.importLights==false)
            {
                if (!config.ForcePreprocess)
                {
                    return;
                }
                isForce = "配置文件开启了强制规范：";
            }
            
            
            //根据配置文件对模型进行导入预处理
            UnityEngine.Debug.Log(isForce+ $"模型导入自动设置--模型文件名: {modelName}，导入设置配置文件:{config.name}", config);
            //Mesh选项面板设置
            modelImporter.isReadable = config.EnableReadWrite;
            modelImporter.importNormals = config.importNormals;
            modelImporter.importTangents = config.importTangents;
            modelImporter.generateSecondaryUV = config.GenerateLightmapUVs;
            modelImporter.importLights = false;
            //Rig选项面板设置
            modelImporter.avatarSetup = config.AvatarSetup;
            modelImporter.skinWeights = config.skinWeights;
            modelImporter.maxBonesPerVertex = config.maxBonesPerVertex;
            modelImporter.minBoneWeight = config.minBoneWeight;
            modelImporter.optimizeGameObjects = config.OptimizeGameObjects;
            //Animation选项面板设置
            modelImporter.animationCompression = config.ModelImporterAnimationCompression;
        }

        
        /// <summary>
        /// 模型导入后对所包含的资源做处理，用于改变所导入的游戏对象、网格、动画剪辑的数据源。
        /// 当前资源改变包含：
        ///     检查动画文件是否包含网格,是则报错提示
        ///     把fbx使用的默认材质改为空
        ///     去掉动画的Scale
        ///     压缩动画浮点精度
        ///     将根运动烘培到骨骼
        /// </summary>
        /// <param name="model"></param>
        private void OnPostprocessModel(GameObject model)
        {
            //检查动画文件是否包含模型、把fbx使用的默认材质改为空
            if (model != null)
            {
                SkinnedMeshRenderer[] renders = model.GetComponentsInChildren<SkinnedMeshRenderer>();
                if (renders != null)
                {
                    foreach (var ren in renders)
                    {
                        ren.skinnedMotionVectors = false;
                    }

                    if (renders.Length > 0 && assetPath.Contains("@"))
                    {
                        UnityEngine.Debug.LogError($"动画文件包含模型，请重新导出动画文件并剔除Mesh：{assetPath}", model);
                    }
                }
                // 用于把fbx使用默认材质的改为空，防止打包的时候包含默认shader
                Renderer[] renderers = model.GetComponentsInChildren<Renderer>();
                if (renderers != null)
                {
                    foreach (var ren in renderers)
                    {
                        if (ren.sharedMaterial != null &&
                            (ren.sharedMaterial.shader != null && ren.sharedMaterial.shader.name == "Standard"))
                        {
                            ren.sharedMaterial = null;
                        }

                        if (ren.sharedMaterials != null)
                        {
                            for (int i = 0; i < ren.sharedMaterials.Length; i++)
                            {
                                var mat = ren.sharedMaterials[i];
                                if (mat != null && (mat.shader != null && (mat != null && mat.shader.name == "Standard")))
                                {
                                    ren.sharedMaterials[i] = null;
                                }
                            }
					
                        }
                    }
                }
            }
            
            //通过模型导入设置配置文件，判断是否进行：去掉动画的Scale、压缩动画浮点精度、将根运动烘培到骨骼操作
            var modelImporter = (ModelImporter) assetImporter;
            var modelName = AssetPreprocessorUtils.GetAssetNameFromPath(modelImporter.assetPath);
            var configs = AutoImportAssetProjectSettingConfig.GetOrCreateSettings().autoImportAssetConfig?.ModelPreprocessorConfigs;
            //var configs = AssetPreprocessorUtils.GetScriptableObjectsOfType<ModelPreprocessorConfig>();
            if (configs==null || configs.Count == 0)
            {
                return;
            }
            configs = configs
                .Where(conf => conf.ShouldUseConfigForAssetImporter(assetImporter))
                .ToList();
            configs.Sort((config1, config2) => config1.ConfigSortOrder.CompareTo(config2.ConfigSortOrder));
            ModelPreprocessorConfig config = null;
            for (var i = 0; i < configs.Count; i++)
            {
                var configToTest = configs[i];
                //运用第一个匹配配置
                config = configToTest;
                break;
            }
            if (config == null) return; //如果没有匹配配置则跳过预处理

            //根据配置文件判断是否去掉动画的Scale
            List<AnimationClip> animationClipList = new List<AnimationClip>(AnimationUtility.GetAnimationClips(model));
            if (animationClipList.Count == 0)
            {
                AnimationClip[] objectList = UnityEngine.Object.FindObjectsOfType(typeof(AnimationClip)) as AnimationClip[];
                animationClipList.AddRange(objectList);
            }
            foreach (AnimationClip theAnimation in animationClipList)
            {
                try
                {
                    //根据配置文件判断是否去除动画Scale数据
                    if(config.DeleteScaleData)
                    {
                        //去除scale曲线
                        foreach (EditorCurveBinding theCurveBinding in AnimationUtility.GetCurveBindings(theAnimation))
                        {
                            string name = theCurveBinding.propertyName.ToLowerInvariant();
                            if (name.Contains("scale"))
                            {
                                bool ignore = false;
                                foreach (var igName in config.ignoreDeleteScaleNames)
                                {
                                    if (theCurveBinding.path.Contains(igName))
                                    {
                                        ignore = true;
                                        break;
                                    }
                                }
                                if(!ignore)
                                    AnimationUtility.SetEditorCurve(theAnimation, theCurveBinding, null);
                            }
                            foreach (var curves in config.additionRemoveCurves)
                            {
                                if(theCurveBinding.propertyName.Contains(curves.name) && curves.path == theCurveBinding.path)
                                {
                                    AnimationUtility.SetEditorCurve(theAnimation, theCurveBinding, null);
                                }
                            }
                        }
                    }

                   

                    //根据配置文件判断是否压缩动画浮点精度
                    if (config.CompressFloat)
                    {
                        //浮点数精度压缩到f3
                        AnimationClipCurveData[] curves = null;
                        curves = AnimationUtility.GetAllCurves(theAnimation);
                        Keyframe key;
                        Keyframe[] keyFrames;
                        for (int ii = 0; ii < curves.Length; ++ii)
                        {
                            AnimationClipCurveData curveDate = curves[ii];
                            if (curveDate.curve == null || curveDate.curve.keys == null)
                            {
                                //Debug.LogWarning(string.Format("AnimationClipCurveData {0} don't have curve; Animation name {1} ", curveDate, animationPath));
                                continue;
                            }

                            keyFrames = curveDate.curve.keys;
                            for (int i = 0; i < keyFrames.Length; i++)
                            {
                                key = keyFrames[i];
                                key.value = float.Parse(key.value.ToString("f3"));
                                key.inTangent = float.Parse(key.inTangent.ToString("f3"));
                                key.outTangent = float.Parse(key.outTangent.ToString("f3"));
                                keyFrames[i] = key;
                            }

                            curveDate.curve.keys = keyFrames;
                            theAnimation.SetCurve(curveDate.path, curveDate.type, curveDate.propertyName, curveDate.curve);
                        } 
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError(string.Format("CompressAnimationClip Failed !!! animationPath : {0} error: {1}",
                        assetPath, e));
                }
            }
            
            #region AnimationType为Human时的导入设置，用于是否将根运动烘培到骨骼，暂且无用注释 
            /* AnimationType为Human时的导入设置，用于是否将根运动烘培到骨骼，暂且无用注释 
            if (config.EnableAnimationPreprocessing &&
                modelImporter.animationType == ModelImporterAnimationType.Human)
            {
                var clips = modelImporter.clipAnimations.ToArray();
                
                // Enable the bones for ALL of the model's animation clips.
                for (var i = 0; i < clips.Length; i++)
                {
                    var clip = clips[i];

                    if (clip.maskType != ClipAnimationMaskType.CreateFromThisModel) continue;
                    
                    var serializedObject = new SerializedObject(modelImporter);
                    
                    var transformMask = serializedObject.FindProperty($"m_ClipAnimations.Array.data[{i}].transformMask");

                    var arrayProperty = transformMask.FindPropertyRelative("Array");

                    for (var j = 0; j < arrayProperty.arraySize; j++)
                    {
                        var element = arrayProperty.GetArrayElementAtIndex(j);
                        
                        var bonePath = element.FindPropertyRelative("m_Path").stringValue;
                        
                        var shouldEnable = false;
                        
                        if (AssetPreprocessorUtils.DoesRegexStringListMatchString(config.MaskBonesToEnable, bonePath))
                        {
                            Debug.Log($"{clip.name} - Enabling Mask bone: {bonePath}", model);

                            shouldEnable = true;
                        }
                        
                        element.FindPropertyRelative("m_Weight").floatValue = shouldEnable ? 1 : 0;
                        
                        serializedObject.ApplyModifiedProperties();
                    }
                }
                    
                EditorUtility.SetDirty(model);
            }
            */
            #endregion
        }
        
        #region AnimationType为Human时的导入设置，用于是否将根运动烘培到骨骼，暂且无用注释 
        /* AnimationType为Human时的导入设置，用于是否将根运动烘培到骨骼，暂且无用注释 
        private void OnPreprocessAnimation()
        {
            
            var modelImporter = assetImporter as ModelImporter;
            Debug.Log("CCC:"+modelImporter.defaultClipAnimations.Length);
            if (modelImporter.defaultClipAnimations.Length == 0) return;
            
            var modelName = AssetPreprocessorUtils.GetAssetNameFromPath(modelImporter.assetPath);
            
            var configs = AssetPreprocessorUtils.GetScriptableObjectsOfType<ModelPreprocessorConfig>();
            
            if (configs.Count == 0)
            {
                Debug.Log($"Could not find a {nameof(ModelPreprocessorConfig)} in project.");

                return;
            }
            
            configs = configs
                .Where(conf => conf.ShouldUseConfigForAssetImporter(assetImporter))
                .Where(conf => conf.EnableAnimationPreprocessing)
                .ToList();
            
            configs.Sort((config1, config2) => config1.ConfigSortOrder.CompareTo(config2.ConfigSortOrder));

            ModelPreprocessorConfig config = null;

            for (var i = 0; i < configs.Count; i++)
            {
                var configToTest = configs[i];

                // Found matching config.
                config = configToTest;
                break;
            }

            // If could not find a matching config, don't process the asset.
            if (config == null) return;
            
            if (modelImporter.clipAnimations.Length > 0 && !config.ForcePreprocess)
            {
                Debug.Log($"Skipping Animation Preprocess - {modelName} - Animation has been preprocessed already before.", config);
                
                return;
            }
            
            Debug.Log($"Processing animations for: {modelName}");
            Debug.Log($"Using: {config.name}", config);
            
            var clips = modelImporter.defaultClipAnimations.ToArray();

            for (var i = 0; i < clips.Length; i++)
            {
                var clip = clips[i];
                
                clip.keepOriginalPositionXZ = config.KeepOriginalPositionXZ;
                clip.keepOriginalOrientation = config.KeepOriginalOrientation;
                clip.keepOriginalPositionY = config.KeepOriginalPositionY;
                
                clip.maskType = config.ClipAnimationMaskType;
                
                clips[i] = clip;
            }

            modelImporter.clipAnimations = clips;
        }
        */
        #endregion
    }
}

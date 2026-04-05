
using System.Collections.Generic;
using System.Linq;
using GOE;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ToolsHelpEditor
{
    [MenuItem("Assets/清理工具/清理无用的材质球属性")]
    [MenuItem("Tools/清理工具/清理无用的材质球属性")]
    static void ClearMaterialProperties()
    {
        UnityEngine.Object[] objs = Selection.GetFiltered(typeof(Material), SelectionMode.DeepAssets);
        for(int i=0;i<objs.Length;i++)
        {
            EditorUtility.DisplayProgressBar("清理中...", objs[i].name, i/objs.Length);
            Material mat = objs[i] as Material;
            if(mat)
            {
                SerializedObject psSource = new SerializedObject(mat);
                SerializedProperty emissionProperty = psSource.FindProperty("m_SavedProperties");
                SerializedProperty texEnvs = emissionProperty.FindPropertyRelative("m_TexEnvs");

                string info = "";
                if(CleanMaterialSerializedProperty(texEnvs, mat, ref info))
                {
                    UnityEngine.Debug.LogError($"清除材质的无用属性:{mat.name}, {info}", mat);
                    psSource.ApplyModifiedProperties();
                    EditorUtility.SetDirty(mat);
                }
            }
        }
        AssetDatabase.SaveAssets();
        EditorUtility.ClearProgressBar();
    }
    
    private static bool CleanMaterialSerializedProperty(SerializedProperty property, Material mat, ref string _info)
    {
        bool res = false;
        for (int j = property.arraySize - 1; j >= 0; j--)
        {
            string propertyName = property.GetArrayElementAtIndex(j).FindPropertyRelative("first").stringValue;
            if(!mat.HasProperty(propertyName))
            {
                if(propertyName == "_MainTex")
                {
                    if(property.GetArrayElementAtIndex(j).FindPropertyRelative("second").FindPropertyRelative("m_Texture").objectReferenceValue != null)
                    {
                        property.GetArrayElementAtIndex(j).FindPropertyRelative("second").FindPropertyRelative("m_Texture").objectReferenceValue = null;
                        res = true;
                    }
                }
                else
                {
                    string path = "";
                    var pt = property.GetArrayElementAtIndex(j);
                    if (pt != null)
                    {
                        path = AssetDatabase.GetAssetPath(pt.FindPropertyRelative("second")?
                            .FindPropertyRelative("m_Texture")?.objectReferenceValue);
                    }
                    _info += $"{propertyName}:{path}\n";
                    
                    property.DeleteArrayElementAtIndex(j);
                    res = true;
                }
            }
        }
        return res;
    }
    
    [MenuItem("Assets/清理工具/清理无用的特效Mesh属性")]
    [MenuItem("Tools/清理工具/清理无用的特效Mesh属性")]
    public static void CheckParticleSystemRenderer()
    {
        Object[] gos = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        if (gos == null) return;
        
        foreach (var item in gos)
        {
            // Filter non-prefab type,
            if (PrefabUtility.GetPrefabType(item) != PrefabType.Prefab)
            {
                continue;
            }

            GameObject gameObj = item as GameObject;
            if(gameObj == null)
                continue;

            bool hasValidMesh = false;
            ParticleSystemRenderer[] renders = gameObj.GetComponentsInChildren<ParticleSystemRenderer>(true);

            string info = "";
            if (renders != null)
            {
                foreach (var renderItem in renders)
                {
                    if (renderItem.renderMode != ParticleSystemRenderMode.Mesh)
                    {
                        if (renderItem.mesh != null)
                        {
                            hasValidMesh = true;
                            info += $"Render mesh:{renderItem.gameObject.name} :{renderItem.mesh.name}\n";
                            renderItem.mesh = null;
                        }
                    }
                }
            }

            var particleSystems = gameObj.GetComponentsInChildren<ParticleSystem>();
            if (particleSystems != null)
            {
                foreach (var particleSystem in particleSystems)
                {
                    var shape = particleSystem.shape;
                    if (shape.shapeType != ParticleSystemShapeType.Mesh)
                    {
                        if (shape.mesh != null)
                        {
                            hasValidMesh = true;
                            info += $"Shape mesh:{particleSystem.gameObject.name} :{shape.mesh.name}\n";
                            shape.mesh = null;
                        }
                    }
                }
            }

            if (hasValidMesh)
            {
                UnityEngine.Debug.LogError($"存在无效Mesh引用：{AssetDatabase.GetAssetPath(gameObj)}\n{info}", gameObj);
                EditorUtility.SetDirty(gameObj);
            }
        }

        AssetDatabase.SaveAssets();
    }
 

    [MenuItem("Assets/清理工具/清理无用的特效组件")]
    [MenuItem("Tools/清理工具/清理无用的特效组件")]
    public static void CheckParticleSystemUseLess()
    {
        Object[] gos = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        if (gos == null) return;

        List<GameObject> dirtyGameObj = new List<GameObject>();
        foreach (var item in gos)
        {
            // Filter non-prefab type,
            if (PrefabUtility.GetPrefabType(item) != PrefabType.Prefab)
            {
                continue;
            }

            GameObject gameObj = item as GameObject;
            if(gameObj == null)
                continue;

            bool hasValidMesh = false;
            string info = "";

            ParticleSystemRenderer[] renders = gameObj.GetComponentsInChildren<ParticleSystemRenderer>(true);
            if (renders != null)
            {
                foreach (var renderItem in renders)
                {
                    if (!renderItem.enabled)
                    {
                        var pt = renderItem.GetComponent<ParticleSystem>();
                        info += $"无效Render:{renderItem.gameObject.name}\n";
                        hasValidMesh = true;
                        GameObject.DestroyImmediate(pt,true);
                        GameObject.DestroyImmediate(renderItem,true);
                    }
                }
            }

            if (hasValidMesh)
            {
                UnityEngine.Debug.LogError($"存在无效特效组件：{AssetDatabase.GetAssetPath(gameObj)}\n{info}", gameObj);
                dirtyGameObj.Add(gameObj);
            }
        }

        foreach (var gobj in dirtyGameObj)
        {
            EditorUtility.SetDirty(gobj);
        }

        AssetDatabase.SaveAssets();
    }
    
    [MenuItem("Assets/清理工具/检查包含模型的动画文件")]
    [MenuItem("Tools/清理工具/检查包含模型的动画文件")]
    public static void CheckAnimFileContainModel()
    {
        Object[] gos = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        if (gos == null) return;

        foreach (var item in gos)
        {
            GameObject gameObj = item as GameObject;
            if(gameObj == null)
                continue;
            var path = AssetDatabase.GetAssetPath(item);
            if(!path.Contains("@"))
                continue;

            SkinnedMeshRenderer[] renders = gameObj.GetComponentsInChildren<SkinnedMeshRenderer>(true);
            if (renders != null&& renders.Length> 0)
            {
                UnityEngine.Debug.LogError($"动画文件包含模型，需剔除：{path}", gameObj);
            }
        }
    }
    
    
    
    [MenuItem("CONTEXT/RawImage/替换为Image")]
    static void ChangeRawImageToImage(MenuCommand menuCommand)
    {
        var rawImage = menuCommand.context as RawImage;//设置父节点为当前选中物体
        if (rawImage != null)
        {
            var go = rawImage.gameObject;
            if (PrefabUtility.IsPartOfPrefabInstance(go))
            {
                Debug.LogError("请进入预制体修改");
            }
            else
            {
                string path = AssetDatabase.GetAssetPath(rawImage.texture);
                var spt = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                if (spt == null)
                {
                    Debug.LogError("RawImage引用的没有Sprite，不能替换");
                }
                else
                {
                    var mat = rawImage.material;
                    bool maskable = rawImage.maskable;
                    bool raycastTarget = rawImage.raycastTarget;
                    Color col = rawImage.color;
                    Object.DestroyImmediate(rawImage);
                    var img = go.AddComponent<Image>();
                    img.sprite = spt;
                    if(mat != Graphic.defaultGraphicMaterial)
                        img.material = mat;
                    img.maskable = maskable;
                    img.raycastTarget = raycastTarget;
                    img.color = col;
                    EditorUtility.SetDirty(go);
                }
               
            }
        }
    }
    [MenuItem("Assets/检查工具/检查RawImage引用Sprite")]
    [MenuItem("Tools/检查工具/检查RawImage引用Sprite")]
    public static void CheckRawImageUseSprite()
    {
        Object[] gos = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        if (gos == null) return;

        foreach (var item in gos)
        {
            GameObject gameObj = item as GameObject;
            if(gameObj == null)
                continue;
            var images = gameObj.GetComponentsInChildren<RawImage>();
            foreach (var img in images)
            {
                if(img.texture == null)
                    continue;
                string path = AssetDatabase.GetAssetPath(img.texture);
                var importer = AssetImporter.GetAtPath(path) as TextureImporter;
                if (importer.textureType == TextureImporterType.Sprite)
                {
                    Debug.LogError($"Prefab:{gameObj.name}  Spt:{img.texture.name}  go:{img.gameObject.name} 存在RawImage引用Sprite \n", gameObj);
                }
            }
        }
    }
    
    [MenuItem("Tools/检查工具/查找挂载了Text脚本的资源")]
    public static void checkUseText()
    {
        if(EditorApplication.isPlaying)
            return;
        List<Text> list = EditorUtil.findAssetsWithComponent<Text>(true, true, new string[] { "Assets/" });
        foreach (var textItem in list)
        {
            if(textItem is TextEx)
                continue;
            
            UnityEngine.Debug.Log(EditorUtil.GetHierarchyPath(textItem.gameObject), textItem.transform.root.gameObject );
        }
    }
        
    [MenuItem("Tools/检查工具/查找丢失引用的prefabV2", false)]
    public static void checkMissingPrefabV2()
    {
        if(EditorApplication.isPlaying)
            return;
        List<GameObject> list = EditorUtil.checkMissingPrefabV2();
        foreach (GameObject gameObject in list)
        {
            if(gameObject != null)
            {
                Debug.Log($"{EditorUtil.GetFullPath(gameObject)}==={gameObject.name}", gameObject);
            }
        }
    }
    
    [MenuItem("Tools/检查工具/移除Missing的脚本")]
    private static void FindAndRemoveMissingInSelected()
    {
        // EditorUtility.CollectDeepHierarchy does not include inactive children
        var deeperSelection = Selection.gameObjects.SelectMany(go => go.GetComponentsInChildren<Transform>(true))
            .Select(t => t.gameObject);
        var prefabs = new HashSet<Object>();
        int compCount = 0;
        int goCount = 0;
        foreach (var go in deeperSelection)
        {
            int count = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(go);
            if (count > 0)
            {
                if (PrefabUtility.IsPartOfAnyPrefab(go))
                {
                    RecursivePrefabSource(go, prefabs, ref compCount, ref goCount);
                    count = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(go);
                    // if count == 0 the missing scripts has been removed from prefabs
                    if (count == 0)
                        continue;
                    // if not the missing scripts must be prefab overrides on this instance
                }

                Undo.RegisterCompleteObjectUndo(go, "Remove missing scripts");
                GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);
                compCount += count;
                goCount++;
            }
        }

        Debug.Log($"Found and removed {compCount} missing scripts from {goCount} GameObjects");
    }

    // Prefabs can both be nested or variants, so best way to clean all is to go through them all
    // rather than jumping straight to the original prefab source.
    private static void RecursivePrefabSource(GameObject instance, HashSet<Object> prefabs, ref int compCount,
        ref int goCount)
    {
        var source = PrefabUtility.GetCorrespondingObjectFromSource(instance);
        // Only visit if source is valid, and hasn't been visited before
        if (source == null || !prefabs.Add(source))
            return;

        // go deep before removing, to differantiate local overrides from missing in source
        RecursivePrefabSource(source, prefabs, ref compCount, ref goCount);

        int count = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(source);
        if (count > 0)
        {
            Undo.RegisterCompleteObjectUndo(source, "Remove missing scripts");
            GameObjectUtility.RemoveMonoBehavioursWithMissingScript(source);
            compCount += count;
            goCount++;
        }
    }
    
    [MenuItem("Tools/检查工具/检查Common开头的ab资源是否都在game_common_info配置")]
    public static List<string> checkCommonABResources()
    {
        List<string> abResourcePaths = new List<string>();
        string[] assetBundleNames = AssetDatabase.GetAllAssetBundleNames();
        abResourcePaths.AddRange(assetBundleNames);
        string[] unusedAssetBundleNames = AssetDatabase.GetUnusedAssetBundleNames();
        foreach (string unusedBundleName in unusedAssetBundleNames)
        {
            if (abResourcePaths.Contains(unusedBundleName))
            {
                abResourcePaths.Remove(unusedBundleName);
            }
        }

        List<string> resultNames = new List<string>();
        foreach (string bundleName in abResourcePaths)
        {
            if(!bundleName.StartsWith("common/"))
                continue;
            resultNames.Add(bundleName);
        }
        
        string path = "Assets/Resources/GameRes/common/__NPGCommonInfo/" + NPGSOGameCommonInfo.objName + ".asset";//
        NPGSOGameCommonInfo commonInfo = AssetDatabase.LoadAssetAtPath<NPGSOGameCommonInfo>(path);
        if(null == commonInfo)
            return null;
        
        List<string> resultList = new List<string>();
        foreach (string name in resultNames)
        {
            if(commonInfo.commonAssetBundlePathList.Contains(name))
                continue;
            resultList.Add(name);
            Debug.LogError($"----------------{name}没有配置在：game_common_info中");
        }
        return resultList;
    }

    [MenuItem("Assets/TATools/取消所选路径下的贴图的sRGB")]
    static void SetTextureToNosRGB()
    {
        UnityEngine.Object[] objs = Selection.GetFiltered(typeof(Texture), SelectionMode.DeepAssets);
        for(int i=0;i<objs.Length;i++)
        {
            EditorUtility.DisplayProgressBar("清理中...", objs[i].name, i/objs.Length);
            Texture tex = objs[i] as Texture;
            if (!tex) continue;
            var textureImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(tex)) as TextureImporter;
            if (textureImporter == null) continue;
            
            textureImporter.sRGBTexture = false;
            textureImporter.SaveAndReimport();
        }
        AssetDatabase.SaveAssets();
        EditorUtility.ClearProgressBar();
    }
    
    [MenuItem("Assets/AB包路径帮助/把AB包路径前面加上 add_pack ")]
    static void setAssetBundleNameAddPack()
    {
        Object[] objs = Selection.objects;
        if (objs == null || objs.Length <= 0)
            return;
        
        string[] paths = new string[objs.Length];
        for (int i = 0; i < objs.Length; i++)
        {
            Object obj = objs[i];
            paths[i] = AssetDatabase.GetAssetPath(obj);
            
            AssetImporter importer = AssetImporter.GetAtPath(paths[i]);
            if (importer == null)
                continue;
            
            if (string.IsNullOrEmpty(importer.assetBundleName)) continue;
            if (importer.assetBundleName.StartsWith(ResIndexConst.addPackPathRoot)) continue;

            importer.assetBundleName = ResIndexConst.addPackPathRoot + importer.assetBundleName;
            importer.SaveAndReimport();
        }

        string[] guids = AssetDatabase.FindAssets("", paths);
        if (guids == null)
            return;
        
        for (int i = 0; i < guids.Length; i++)
        {
            string guid = guids[i];
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            AssetImporter importer = AssetImporter.GetAtPath(assetPath);
            if (importer == null)
                continue;

            EditorUtility.DisplayProgressBar("替换中", importer.name, (float) i / guids.Length);
            
            if (string.IsNullOrEmpty(importer.assetBundleName)) continue;
            if (importer.assetBundleName.StartsWith(ResIndexConst.addPackPathRoot)) continue;

            importer.assetBundleName = ResIndexConst.addPackPathRoot + importer.assetBundleName;
            importer.SaveAndReimport();
        }
        AssetDatabase.SaveAssets();
        EditorUtility.ClearProgressBar();
    }
    [MenuItem("Assets/AB包路径帮助/把AB包路径前面去掉 add_pack ")]
    static void resetAssetBundleNameAddPack() 
    {
        Object[] objs = Selection.objects;
        if (objs == null || objs.Length <= 0)
            return;
        
        string[] paths = new string[objs.Length];
        for (int i = 0; i < objs.Length; i++)
        {
            Object obj = objs[i];
            paths[i] = AssetDatabase.GetAssetPath(obj);
            
            AssetImporter importer = AssetImporter.GetAtPath(paths[i]);
            if (importer == null)
                continue;
            
            if (string.IsNullOrEmpty(importer.assetBundleName)) continue;
            if (!importer.assetBundleName.StartsWith(ResIndexConst.addPackPathRoot)) continue;
            
            importer.assetBundleName = importer.assetBundleName.Remove(0, ResIndexConst.addPackPathRoot.Length);
            importer.SaveAndReimport();
        }

        string[] guids = AssetDatabase.FindAssets("", paths);
        if (guids == null)
            return;
        
        for (int i = 0; i < guids.Length; i++)
        {
            string guid = guids[i];
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            AssetImporter importer = AssetImporter.GetAtPath(assetPath);
            if (importer == null)
                continue;

            EditorUtility.DisplayProgressBar("替换中", importer.name, (float) i / guids.Length);
            
            if (string.IsNullOrEmpty(importer.assetBundleName)) continue;
            if (!importer.assetBundleName.StartsWith(ResIndexConst.addPackPathRoot)) continue;
            
            importer.assetBundleName = importer.assetBundleName.Remove(0, ResIndexConst.addPackPathRoot.Length);
            importer.SaveAndReimport();
        }
        AssetDatabase.SaveAssets();
        EditorUtility.ClearProgressBar();
    }
    
    // 自定义你想要应用贴图的路径
    static string texturePath = "Assets/Resources/GameRes/tex_common/tex_test/mip/showmip.png";
    
    private static readonly int BaseMap = Shader.PropertyToID("_BaseMap");
    private static readonly int BaseMap1 = Shader.PropertyToID("_BaseMap1");
    private static readonly int BaseMap2 = Shader.PropertyToID("_BaseMap2");
    private static readonly int Metallic = Shader.PropertyToID("_Metallic");
    private static readonly int Metallic1 = Shader.PropertyToID("_Metallic1");
    private static readonly int Metallic2 = Shader.PropertyToID("_Metallic2");
    
    
    [MenuItem("Tools/检查工具/检查模型贴图最大尺寸")]
    static void ApplyTextureToMaterial()
    {
        
        // 获取在Hierarchy面板中选择的游戏对象
        if (Selection.activeObject == null || EditorUtility.IsPersistent(Selection.activeObject))
        {
            Debug.LogError("检查模型贴图最大尺寸 -> 请选择Hierarchy面板中的对象");
            return;
        }
        GameObject[] selectedObjects = Selection.gameObjects;


        // 加载你想要应用的贴图
        Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);

        if (texture == null)
        {
            Debug.LogError("找不到指定的贴图 " + texturePath);
            return;
        }

        foreach (GameObject obj in selectedObjects)
        {
            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>(true);

            foreach (Renderer renderer in renderers)
            {
                // 遍历该Renderer上的每一个材质球
                Material[] newMaterials = new Material[renderer.sharedMaterials.Length];

                for (int i = 0; i < renderer.sharedMaterials.Length; i++)
                {
                    // 创建每一个材质球的新实例（克隆）
                    newMaterials[i] = new Material(renderer.sharedMaterials[i]);
                    newMaterials[i].name += "(Temp)";
                    newMaterials[i].SetTexture(BaseMap ,texture);
                    newMaterials[i].SetTexture(BaseMap1 ,texture);
                    newMaterials[i].SetTexture(BaseMap2 ,texture);
                    newMaterials[i].SetFloat(Metallic, 0f);
                    newMaterials[i].SetFloat(Metallic1, 0f);
                    newMaterials[i].SetFloat( Metallic2, 0f);
                }
                // 应用新材质数组到Renderer
                renderer.materials = newMaterials;
            }
        }

        Debug.Log("替换完毕");
    }

    [MenuItem("Assets/检查工具/输出模型信息/预制体prefab")]
    [MenuItem("Tools/检查工具/输出模型信息/预制体prefab")]
    static void DebugModelInfoPrefab()
    {
        DebugModelInfo(true);
    }
    
    [MenuItem("Assets/检查工具/输出模型信息/模型fbx")]
    [MenuItem("Tools/检查工具/输出模型信息/模型fbx")]
    static void DebugModelInfoMesh()
    {
        DebugModelInfo(false);
    }
    
    static void DebugModelInfo(bool isPrefab)
    {
        UnityEngine.Object[] objs = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

        string modelInfo =string.Format("<color=#ff0000>{0}</color>", "名称ID\t顶点数量\t三角面数量\t是否含有Color\t是否含有uv1\t是否含有uv2\t是否含有uv3\t套装效果图");

        foreach (var obj in objs)
        {
            if (isPrefab)
            {
                if (PrefabUtility.IsPartOfModelPrefab(obj))
                    continue;
            }
            else
            {
                if (!PrefabUtility.IsPartOfModelPrefab(obj))
                    continue;
            }

     
            GameObject go = obj as GameObject;
            if(go == null)
                continue;
            SkinnedMeshRenderer[] skinRenderers = go.GetComponentsInChildren<SkinnedMeshRenderer>(true);
            List<string> skinRenderMeshInfoList = new List<string>();

            int vertexCount, trianglesCount;
            bool hasColor, hasUV1, hasUV2, hasUV3;
            vertexCount = trianglesCount = 0;
            hasColor = hasUV1 = hasUV2 = hasUV3 = false;
            
            foreach (SkinnedMeshRenderer skinRenderer in skinRenderers)
            {
                Mesh skinRenderMesh = skinRenderer.sharedMesh;
                vertexCount += skinRenderMesh.vertexCount;  //顶点数量
                trianglesCount += skinRenderMesh.triangles.Length / 3;  //三角面数量
                hasColor = hasColor || skinRenderMesh.colors.Length != 0;   //是否含有color
                hasUV1 = hasUV1 || (skinRenderMesh.uv.Length != 0); //是否含有uv1
                hasUV2 = hasUV2 || (skinRenderMesh.uv2.Length != 0);    //是否含有uv2
                hasUV3 = hasUV3 || (skinRenderMesh.uv3.Length != 0);    //是否含有uv3

                //子信息
                // skinRenderMeshInfoList.Add("\n" + skinRenderMesh.name + "\t" + skinRenderMesh.vertexCount + "\t" + 
                //                        skinRenderMesh.triangles.Length/3 + "\t" + (skinRenderMesh.colors.Length != 0) + "\t" + 
                //                        (skinRenderMesh.uv.Length != 0) + "\t" + (skinRenderMesh.uv2.Length != 0) + "\t" + 
                //                        (skinRenderMesh.uv3.Length != 0));
            }

            //添加单个汇总信息
            modelInfo += string.Format("<color=#7B68EE>{0}</color>", "\n" + obj.name + "\t" +
                                                                     vertexCount + "\t" +
                                                                     trianglesCount + "\t" +
                                                                     hasColor + "\t" +
                                                                     hasUV1 + "\t" +
                                                                     hasUV2 + "\t" +
                                                                     hasUV3);
            
            //添加子信息
            // foreach (var skinRenderMeshInfo in skinRenderMeshInfoList)
            // {
            //     modelInfo += skinRenderMeshInfo;
            // }
        }
        
        Debug.Log(modelInfo);
        Debug.Log("输出模型信息完毕");
    }
}

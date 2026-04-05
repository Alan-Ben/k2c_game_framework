using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class RawImageMeshHelpWnd : EditorWindow
    {
        [MenuItem("Tools/RawImageMesh Help")]
        static void showWindow()
        {
            EditorWindow.GetWindow(typeof(RawImageMeshHelpWnd));
        }
        RawImageMeshHelpWnd()
        {
            this.titleContent = new GUIContent("RawImageMesh Help Wnd");
        }

        
        
        private string _m_prefabSavePath = "Assets/Resources/GameRes/go/go_2_space/go_209xxx_mini_map/go_mini_map/al_fog/";
        private string _m_prefabName = "go_209208_1";
        private string _m_ABName = "go/go_209208";
        private string _m_sAssetBundleVariant="unity3d";   
        private int _m_layer = 5;
        private int _m_startIndex = 1;
        private bool _m_exchangeYZ = true;

        private Vector2 _m_mapUISize = new Vector2(1180,1180);
        private Vector2 _m_mapModelSize = new Vector2(300, 300);
        private Vector2 _m_curModelSize = new Vector2(200, 200);
        private Vector2 _m_curModelPosOffset = new Vector2(100, 100);
        private RectTransform _m_rootTrans;
        private Material _m_material;
        

        [SerializeField]
        private List<MeshFilter> _m_lMeshFilters = new List<MeshFilter>();
        
        [SerializeField] private List<String> _m_lNames = new List<string>();
        
        private SerializedObject so;
        private SerializedProperty meshFiltersProperty;
        private SerializedObject so_name;
        private SerializedProperty nameProperty;
        
        private void OnEnable()
        {
            so = new SerializedObject(this);
            meshFiltersProperty = so.FindProperty("_m_lMeshFilters");
            _m_material = AssetDatabase.LoadAssetAtPath<Material>("Assets/Resources/GameRes/scenes/res_fog/UIFogTemplate.mat");
        }

        private void OnGUI()
        {
            _m_prefabSavePath = EditorGUILayout.TextField("预制体保存路径", _m_prefabSavePath);
            _m_prefabName = EditorGUILayout.TextField("预制体名字", _m_prefabName);
            _m_ABName= EditorGUILayout.TextField("AB包名字", _m_ABName);
            _m_startIndex = EditorGUILayout.IntField("预制体开始编号", _m_startIndex);

            _m_mapUISize = EditorGUILayout.Vector2Field("Map UI Size", _m_mapUISize);
            _m_mapModelSize = EditorGUILayout.Vector2Field("Map Model Size", _m_mapModelSize);
            _m_curModelSize = EditorGUILayout.Vector2Field("Cur Model Size", _m_curModelSize);
            _m_curModelPosOffset = EditorGUILayout.Vector2Field("cur ModelPosOffset", _m_curModelPosOffset);
            _m_rootTrans = EditorGUILayout.ObjectField("_m_rootTrans", _m_rootTrans, typeof(RectTransform), true) as RectTransform;
            _m_material = EditorGUILayout.ObjectField("Material", _m_material, typeof(Material), true) as Material;


            so.Update();
            EditorGUILayout.PropertyField(meshFiltersProperty, new GUIContent("Mesh Filters"));
            so.ApplyModifiedProperties();
            if (GUILayout.Button("生成"))
            {
                int index = _m_startIndex;
                foreach (var meshFilter in _m_lMeshFilters)
                {
                    string prefabName = _m_prefabName + index;
                    string path = _m_prefabSavePath + prefabName +".prefab";
                    //创建预制体和fog网格子物体
                    GameObject go =  new GameObject(prefabName);
                    GameObject fog = new GameObject("UIFog");
                    
                    fog.transform.parent = go.transform;
                    go.layer = _m_layer;
                    fog.layer = _m_layer;
                    RectTransform goRectTransform =go.AddComponent<RectTransform>();
                    goRectTransform.anchorMin = new Vector2(0, 0);
                    goRectTransform.anchorMax = new Vector2(0, 0);
                    go.AddComponent<CanvasRenderer>();
                    
                    //给子物体fog添加对应信息
                    RawImageMesh rawImageMesh = fog.AddComponent<RawImageMesh>();
                    rawImageMesh.m_mesh = meshFilter.sharedMesh;
                    float scale = (_m_mapUISize.x / _m_mapModelSize.x);
                    rawImageMesh.m_overrideScale = Vector3.one * scale;
                    rawImageMesh.material = _m_material;
                    RectTransform rectTransform = fog.GetComponent<RectTransform>();
                    if(rectTransform == null)
                        rectTransform = fog.AddComponent<RectTransform>();
                    CanvasRenderer cr = fog.GetComponent<CanvasRenderer>();
                    if(cr == null)
                        fog.AddComponent<CanvasRenderer>();
                    fog.transform.localScale = Vector3.one;
                    
                    Vector3 pos = (meshFilter.transform.localPosition +
                                   new Vector3(_m_curModelPosOffset.x, _m_exchangeYZ ? 0 :_m_curModelPosOffset.y, _m_exchangeYZ ? _m_curModelPosOffset.y : 0)) * scale;
                    pos = _m_exchangeYZ ? new Vector3(pos.x,pos.z, pos.y) : pos ;
                    rectTransform.localPosition = pos;
                    AssetImporter asset=AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(PrefabUtility.SaveAsPrefabAsset(go, path)));
                    asset.assetBundleName = _m_ABName;
                    asset.assetBundleVariant = _m_sAssetBundleVariant;
                    DestroyImmediate(go);
                    index++;
                }
                AssetDatabase.Refresh();

            }
        }
    }
}
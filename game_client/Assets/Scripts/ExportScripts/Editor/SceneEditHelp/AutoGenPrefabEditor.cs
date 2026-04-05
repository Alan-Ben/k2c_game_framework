using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoGenPrefab
{
    class AutoPrefabInfoMgr
    {
        class SpritePrefabInfo
        {
            public Sprite sprite;
            public GameObject prefab;
            public List<SpriteRenderer> renders = new List<SpriteRenderer>();
        }
        class MeshRenderInfo
        {
            public Mesh mesh;
            public GameObject prefab;
            public List<MeshFilter> filters = new List<MeshFilter>();
        }
        class PrefabOutermostInfo
        {
            public string path;
            public GameObject prefab;
            public List<GameObject> gos = new List<GameObject>();
        }
        
        private string _m_prefabFolder = "Assets/Resources/GameRes/scenes/gscene_9999_space_res/prefabs";
        private string _m_sSpriteNamePattern = "{0}/prefab{1}.prefab";
        private string _m_sMeshNamePattern = "{0}/prefab{1}.prefab";
        private string _m_sModelNamePattern = "{0}/prefab{1}.prefab";

        List<SpritePrefabInfo> spritePrefabInfos = new List<SpritePrefabInfo>();
        List<MeshRenderInfo> meshRenderInfos = new List<MeshRenderInfo>();
        private List<PrefabOutermostInfo> prefabOutermostInfos = new List<PrefabOutermostInfo>();

        private bool _m_placeToDifRoot = true;
        private GameObject _m_allRoot;
        public AutoPrefabInfoMgr(string _prefabFolder, string _spritePattern, string _meshPattern, string _modelPattern)
        {
            _m_prefabFolder = _prefabFolder;
            _m_sSpriteNamePattern = _spritePattern;
            _m_sMeshNamePattern = _meshPattern;
            _m_sModelNamePattern = _modelPattern;
        }
        
        public void addSprite(SpriteRenderer _spriteRenderer)
        {
            foreach (var info in spritePrefabInfos)
            {
                if (info.sprite == _spriteRenderer.sprite)
                {
                    info.renders.Add(_spriteRenderer);
                    return;
                }
            }
            SpritePrefabInfo nInfo = new SpritePrefabInfo();
            nInfo.sprite = _spriteRenderer.sprite;
            nInfo.renders.Add(_spriteRenderer);
            spritePrefabInfos.Add(nInfo);
        }

        public void addMeshRenderer(MeshFilter _filter)
        {
            foreach (var info in meshRenderInfos)
            {
                if (info.mesh == _filter.sharedMesh)
                {
                    info.filters.Add(_filter);
                    return;
                }
            }
            MeshRenderInfo nInfo = new MeshRenderInfo();
            nInfo.mesh = _filter.sharedMesh;
            nInfo.filters.Add(_filter);
            meshRenderInfos.Add(nInfo);
        }

        public void addOutMostPrefab(GameObject _go)
        {
            string path = AssetDatabase.GetAssetPath(PrefabUtility.GetCorrespondingObjectFromSource(_go));
            ModelImporter importer = AssetImporter.GetAtPath(path) as ModelImporter;
            if(importer == null)
                return;
            foreach (var info in prefabOutermostInfos)
            {
                if (info.path == path)
                {
                    info.gos.Add(_go);
                    return;
                }
            }
            PrefabOutermostInfo nInfo = new PrefabOutermostInfo();
            nInfo.path = path;
            nInfo.gos.Add(_go);
            prefabOutermostInfos.Add(nInfo);
        }

        public void replacePrefab(bool _placeToDifRoot)
        {
            _m_placeToDifRoot = _placeToDifRoot;
            _m_allRoot = new GameObject($"prefabs_root");

            replaceSpritePrefab();
            replaceMeshPrefab();
            replaceOutermostPrefab();
        }
        public void replaceSpritePrefab(bool _placeToDifRoot)
        {
            _m_placeToDifRoot = _placeToDifRoot;
            _m_allRoot = new GameObject($"prefabs_root");

            replaceSpritePrefab2();
        }
        void replaceSpritePrefab2()
        {
            foreach (var info in spritePrefabInfos)
            {
                GameObject root = !_m_placeToDifRoot ? _m_allRoot : new GameObject($"sprite_{info.sprite.name}_root");
                
                if (info.renders.Count <= 0)
                {
                    Debug.LogError($"{info.sprite}render数量不对", info.sprite);
                    continue;
                }
                if (info.renders[0] == null)
                {
                    Debug.LogError($"{info.sprite}render 为空， 请检查", info.sprite);
                    continue;
                }
                var nGo = GameObject.Instantiate(info.renders[0].gameObject);
                nGo.transform.position = Vector3.zero;
                nGo.transform.rotation = Quaternion.identity;
                nGo.transform.localScale = Vector3.one;
                
                string prefabPath = string.Format(_m_sSpriteNamePattern,_m_prefabFolder,info.sprite.name);

                var prefabAsset = PrefabUtility.SaveAsPrefabAsset(nGo, prefabPath);
                Object.DestroyImmediate(nGo);
                foreach (var render in info.renders)
                {
                    if(!render.transform.gameObject.activeInHierarchy)
                        continue;
                    
                    GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefabAsset, render.transform.parent);
                    go.transform.position = render.transform.position;
                    go.transform.rotation = render.transform.rotation;
                    go.transform.localScale = render.transform.localScale;
                    var nren = go.GetComponent<SpriteRenderer>();
                    nren.color = render.color;
                    Object.DestroyImmediate(render.gameObject);
                }
            }

        }

        
        void replaceSpritePrefab()
        {
            foreach (var info in spritePrefabInfos)
            {
                GameObject root = !_m_placeToDifRoot ? _m_allRoot : new GameObject($"sprite_{info.sprite.name}_root");
                
                if (info.renders.Count <= 0)
                {
                    Debug.LogError($"{info.sprite}render数量不对", info.sprite);
                }
                var nGo = GameObject.Instantiate(info.renders[0].gameObject);
                nGo.transform.position = Vector3.zero;
                nGo.transform.localScale = Vector3.one;
                string prefabPath = string.Format(_m_sSpriteNamePattern,_m_prefabFolder,info.sprite.name);

                var prefabAsset = PrefabUtility.SaveAsPrefabAsset(nGo, prefabPath);
                Object.DestroyImmediate(nGo);
                foreach (var render in info.renders)
                {
                    if(!render.transform.gameObject.activeInHierarchy)
                        continue;
                    
                    GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefabAsset, root.transform);
                    go.transform.position = render.transform.position;
                    go.transform.rotation = render.transform.rotation;
                    go.transform.localScale = render.transform.lossyScale;
                    var nren = go.GetComponent<SpriteRenderer>();
                    nren.color = render.color;
                    Object.DestroyImmediate(render.gameObject);
                }
            }

        }

        void replaceMeshPrefab()
        {
            foreach (var info in meshRenderInfos)
            {
                GameObject root = !_m_placeToDifRoot ? _m_allRoot : new GameObject($"mesh_{info.mesh.name}_root");
                if (info.filters.Count <= 0)
                {
                    Debug.LogError($"{info.mesh}render数量不对", info.mesh);
                }
                var nGo = GameObject.Instantiate(info.filters[0].gameObject);
                nGo.transform.position = Vector3.zero;
                nGo.transform.localScale = Vector3.one;
                string prefabPath = string.Format(_m_sMeshNamePattern,_m_prefabFolder,info.mesh.name);

                var prefabAsset = PrefabUtility.SaveAsPrefabAsset(nGo, prefabPath);
                Object.DestroyImmediate(nGo);
                foreach (var render in info.filters)
                {
                    if(!render.transform.gameObject.activeInHierarchy)
                        continue;
                    
                    GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefabAsset, root.transform);
                    go.transform.position = render.transform.position;
                    go.transform.rotation = render.transform.rotation;
                    go.transform.localScale = render.transform.lossyScale;
                    Object.DestroyImmediate(render.gameObject);
                }
            }
        }
        
        void replaceOutermostPrefab()
        {
            foreach (var info in prefabOutermostInfos)
            {
                string name = Path.GetFileNameWithoutExtension(info.path);
                GameObject root = !_m_placeToDifRoot ? _m_allRoot : new GameObject($"model_{name}_root");
                var prefab = AssetDatabase.LoadMainAssetAtPath(info.path);
                string prefabPath = string.Format(_m_sModelNamePattern,_m_prefabFolder,name);

                var nGo = (GameObject) PrefabUtility.InstantiatePrefab(prefab);
                var prefabAsset = PrefabUtility.SaveAsPrefabAsset(nGo, prefabPath);
                Object.DestroyImmediate(nGo);
                foreach (var render in info.gos)
                {
                    if(!render.transform.gameObject.activeInHierarchy)
                        continue;
                    
                    GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefabAsset, root.transform);
                    go.transform.position = render.transform.position;
                    go.transform.rotation = render.transform.rotation;
                    go.transform.localScale = render.transform.lossyScale;
                    Object.DestroyImmediate(render.gameObject);
                }
            }
        }

    }

    // [MenuItem("Tools/分析场景并自动生成预制体")]
    // private static void ScanSceneAndGenPrefab()
    // {
    //     var scene = SceneManager.GetActiveScene();
    //     var gos = scene.GetRootGameObjects();
    //     scaneRootGo(gos);
    // }
    //
    // [MenuItem("Tools/分析选中物体子节点并自动生成预制体")]
    // private static void ScanSelectionAndGenPrefab()
    // {
    //     scaneRootGo(Selection.gameObjects, );
    // }
    //
    public static void ScanGosAndGenPrefab(GameObject[] gos, string _prefabFolder,  string _spritePattern, string _meshPattern, string _modelPattern, bool _placeToDifRoot)
    {
        AutoPrefabInfoMgr prefabInfoMgr = new AutoPrefabInfoMgr(_prefabFolder, _spritePattern, _meshPattern, _modelPattern);

        foreach (var rootGo in gos)
        {
            bool isOutermostPrefab = PrefabUtility.IsOutermostPrefabInstanceRoot(rootGo);
            if (isOutermostPrefab)
            {
                prefabInfoMgr.addOutMostPrefab(rootGo);
                continue;
            }
            var trans = rootGo.GetComponentsInChildren<Transform>();

            foreach (var tran in trans)
            {
                if (PrefabUtility.IsOutermostPrefabInstanceRoot(tran.gameObject))
                {
                    prefabInfoMgr.addOutMostPrefab(tran.gameObject);
                }
            }

            var spriteRenderers = rootGo.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
                bool isPartOfAnyPrefab = PrefabUtility.IsPartOfAnyPrefab(spriteRenderer);
                if(isPartOfAnyPrefab)
                    continue;
                prefabInfoMgr.addSprite(spriteRenderer);
            }
            var meshFilters = rootGo.GetComponentsInChildren<MeshFilter>();
            foreach (var meshFilter in meshFilters)
            {
                bool isPartOfAnyPrefab = PrefabUtility.IsPartOfAnyPrefab(meshFilter);
                
                if(!isPartOfAnyPrefab)
                    prefabInfoMgr.addMeshRenderer(meshFilter);
            }
        }
       
        prefabInfoMgr.replacePrefab(_placeToDifRoot);
    }
    public static void ScanSpriteAndGenPrefab(GameObject[] gos, string _prefabFolder, string _spritePattern, string _meshPattern, string _modelPattern, bool _placeToDifRoot)
    {
        AutoPrefabInfoMgr prefabInfoMgr = new AutoPrefabInfoMgr(_prefabFolder, _spritePattern, _meshPattern, _modelPattern);

        foreach (var rootGo in gos)
        {
            bool isOutermostPrefab = PrefabUtility.IsOutermostPrefabInstanceRoot(rootGo);
            if (isOutermostPrefab)
            {
                prefabInfoMgr.addOutMostPrefab(rootGo);
                continue;
            }
            var trans = rootGo.GetComponentsInChildren<Transform>();

            foreach (var tran in trans)
            {
                if (PrefabUtility.IsOutermostPrefabInstanceRoot(tran.gameObject))
                {
                    prefabInfoMgr.addOutMostPrefab(tran.gameObject);
                }
            }

            var spriteRenderers = rootGo.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
                bool isPartOfAnyPrefab = PrefabUtility.IsPartOfAnyPrefab(spriteRenderer);
                if(isPartOfAnyPrefab)
                    continue;
                prefabInfoMgr.addSprite(spriteRenderer);
            }
            var meshFilters = rootGo.GetComponentsInChildren<MeshFilter>();
            foreach (var meshFilter in meshFilters)
            {
                bool isPartOfAnyPrefab = PrefabUtility.IsPartOfAnyPrefab(meshFilter);
                
                if(!isPartOfAnyPrefab)
                    prefabInfoMgr.addMeshRenderer(meshFilter);
            }
        }
       
        prefabInfoMgr.replaceSpritePrefab(_placeToDifRoot);
    }
}
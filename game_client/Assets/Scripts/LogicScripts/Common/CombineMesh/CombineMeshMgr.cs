using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class CombineMeshMgr
    {
        private static CombineMeshMgr _m_instance;

        public static CombineMeshMgr instance
        {
            get
            {
                if(_m_instance == null)
                {
                    _m_instance = new CombineMeshMgr();
                }
                return _m_instance;
            }
        }
          
        public class CombineSkinnedGroupInfo
        {
            /** 使用同一材质的网格对象的绑定列表 */
            protected List<CombineInstance> _meshList;

            private Mesh _m_mCombineMesh;
            private List<Mesh> _m_lMeshes;

            public CombineSkinnedGroupInfo(List<SkinnedMeshRenderer> _skinMeshRenderers)
            {
                _m_lMeshes = new List<Mesh>();
                _meshList = new List<CombineInstance>();

                foreach (SkinnedMeshRenderer render in _skinMeshRenderers)
                {
                    Mesh mesh = render.sharedMesh;
                    _m_lMeshes.Add(mesh);
                    for (int i = 0; i < mesh.subMeshCount; i++)
                    {
                        CombineInstance combineInstance = new CombineInstance();
                        combineInstance.mesh = mesh;
                        combineInstance.subMeshIndex = i;
                        _meshList.Add(combineInstance);
                    }
                }
            }

            /**************
             * 使用本集合中所有的网格合并创建出新的一个使用同一材质的网格
             **/
            public Mesh getMesh()
            {
                if (_m_mCombineMesh != null)
                    return _m_mCombineMesh;
                Mesh newMesh = new Mesh();
                newMesh.CombineMeshes(_meshList.ToArray(), true, false);
                _m_mCombineMesh = newMesh;
                return newMesh;
            }

            /// <summary>
            /// 是否匹配这个Mesh合并组
            /// </summary>
            /// <param name="_combineRenderList"></param>
            /// <returns></returns>
            public bool isMatch(List<SkinnedMeshRenderer> _combineRenderList)
            {
                if(_m_lMeshes.Count != _combineRenderList.Count)
                    return false;
                
                for (var i = 0; i < _combineRenderList.Count; i++)
                {
                    var skin = _combineRenderList[i];
                    if (skin.sharedMesh != _m_lMeshes[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        
        /// <summary>
        /// 已经有的合并好的Mesh组
        /// </summary>
        private List<CombineSkinnedGroupInfo> _m_lCombineMeshGroups = new List<CombineSkinnedGroupInfo>();
        /// <summary>
        /// 临时保存Skin分组用
        /// </summary>
        Dictionary<Material, List<SkinnedMeshRenderer>> _m_dTmpSkinnedGroups = new Dictionary<Material, List<SkinnedMeshRenderer>>();
        /// <summary>
        /// 找到能合并的Skin,合并，并返回未合并、已合并、合并结果列表
        /// </summary>
        /// <param name="_root"></param>
        /// <param name="renderers">要合并的SkineMeshRenderers</param>
        /// <param name="_combineResultRenders"></param>
        /// <param name="_unCombineRenders"></param>
        /// <param name="_beCombineRenders"></param>
        public void combineMeshs(Transform _root, SkinnedMeshRenderer[] renderers,
            out List<SkinnedMeshRenderer> _combineResultRenders, out List<SkinnedMeshRenderer> _unCombineRenders, out List<SkinnedMeshRenderer> _beCombineRenders)
        {
            _combineResultRenders = new List<SkinnedMeshRenderer>();
            _beCombineRenders = new List<SkinnedMeshRenderer>();
            _unCombineRenders = new List<SkinnedMeshRenderer>();
            List<SkinnedMeshRenderer> combineSkinMeshRenders = new List<SkinnedMeshRenderer>();
            foreach (SkinnedMeshRenderer renderer in renderers)
            {
                // 增加对模型是否可读来加入合并模型的列表里，避免一些不可读的模型导致报错
                if (renderer.enabled && renderer.sharedMesh.isReadable && renderer.sharedMaterials.Length > 0 && _isRendererSubMeshSameMat(renderer))
                {
                    combineSkinMeshRenders.Add(renderer);
                }
                else
                {
                    _unCombineRenders.Add(renderer);
#if UNITY_EDITOR
                    if (!renderer.sharedMesh.isReadable)
                    {
                        Mesh sharedMesh = renderer.sharedMesh;
                        Debug.LogError($"此模型未开启读写，执行合并mesh会出错，需要开启一下{sharedMesh}",sharedMesh);
                    }
#endif
                }
            }
            _m_dTmpSkinnedGroups.Clear();
            foreach (var render in combineSkinMeshRenders)
            {
                if (!_m_dTmpSkinnedGroups.TryGetValue(render.sharedMaterial, out List<SkinnedMeshRenderer> group))
                {
                    group = new List<SkinnedMeshRenderer>();
                    _m_dTmpSkinnedGroups.Add(render.sharedMaterial, group);
                }
                group.Add(render);
            }

            int index = 0;
            foreach (KeyValuePair<Material, List<SkinnedMeshRenderer>> matGroup in _m_dTmpSkinnedGroups)
            {
                List<SkinnedMeshRenderer> group = matGroup.Value;
                if (group.Count > 1)
                {
                    GameObject sub = new GameObject($"combineSkin_{index}", typeof(SkinnedMeshRenderer));
                    sub.transform.SetParent(_root, false);
                    Mesh mesh = getCombinedMesh(matGroup.Value);
                    SkinnedMeshRenderer renderer = sub.GetComponent<SkinnedMeshRenderer>();
                    if (null == renderer)
                        renderer = _root.gameObject.AddComponent<SkinnedMeshRenderer>();
                    
                    List<Transform> bones = new List<Transform>();
                    for (int i = 0; i < group.Count; i++)
                    {
                        SkinnedMeshRenderer childSMR = group[i];
                        if (null == childSMR)
                            continue;
                        for (int meshIdx = 0; meshIdx < childSMR.sharedMesh.subMeshCount; meshIdx++)
                        {
                            bones.AddRange(childSMR.bones);
                        }
                        childSMR.gameObject.SetActive(false);
                    }
                    renderer.sharedMesh = mesh;
                    renderer.bones = bones.ToArray();
                    renderer.materials = new []{matGroup.Key};
                    _combineResultRenders.Add(renderer);
                    _beCombineRenders.AddRange(group);
                    index++;
                }
                else
                {
                    foreach (var renderer in group)
                    {
                        _unCombineRenders.Add(renderer);
                    }
                }
            }
            _m_dTmpSkinnedGroups.Clear();
        }

        /// <summary>
        /// 是否所有SubMesh都是同一个材质
        /// </summary>
        /// <param name="renderer"></param>
        /// <returns></returns>
        private bool _isRendererSubMeshSameMat(SkinnedMeshRenderer renderer)
        {
            if (renderer.sharedMaterials.Length <= 1)
                return true;
            Material mat0 = renderer.sharedMaterials[0];
            for (var i = 1; i < renderer.sharedMaterials.Length; i++)
            {
                var mat = renderer.sharedMaterials[i];
                if (mat0 != mat) return false;
            }

            return true;
        }
        
        /// <summary>
        /// 把列表的Mesh合并并范围，如果已经合并过同样Mesh的则直接返回Mesh
        /// </summary>
        /// <param name="_combineRenderList"></param>
        /// <returns></returns>
        private Mesh getCombinedMesh(List<SkinnedMeshRenderer> _combineRenderList)
        {
            if (null == _combineRenderList)
                return null;

            CombineSkinnedGroupInfo matchGroup = null;
            foreach (CombineSkinnedGroupInfo groupInfo in _m_lCombineMeshGroups)
            {
                if (groupInfo.isMatch(_combineRenderList))
                {
                    matchGroup = groupInfo;
                    break;
                }
            }

            if (matchGroup == null)
            {
                matchGroup = new CombineSkinnedGroupInfo(_combineRenderList);
                if (_m_lCombineMeshGroups.Count < 20)
                {
                    _m_lCombineMeshGroups.Add(matchGroup);
                }
                else
                {
                    Debug.LogError($"Ben:CombineMeshMgr 合并列表数量超过10，需要检查");
                }
            }
            return matchGroup.getMesh();
        }
    }
}
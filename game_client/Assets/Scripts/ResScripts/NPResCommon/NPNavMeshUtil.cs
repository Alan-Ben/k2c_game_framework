using UnityEngine;
using UnityEngine.AI;

namespace GOE
{
    public static class NPNavMeshUtil
    {
        /// <summary>
        /// 检查并创建一个 NavMeshBuildSource 以供生成 NavMesh 
        /// </summary>
        public static bool checkAndCreateBuildSource(Transform _trans, int _area, out NavMeshBuildSource _buildSource)
        {
            _buildSource = new NavMeshBuildSource();
            _buildSource.transform = _trans.localToWorldMatrix;
            _buildSource.area = _area;
            
            // 如果存在 collider 优先使用 collider
            Collider collider = _trans.GetComponent<Collider>();
            if (collider == null)
            {
                // 如果没有 collider 则看看有没有 mesh ，有就用
                MeshFilter meshFilter = _trans.GetComponent<MeshFilter>();
                if (meshFilter == null || meshFilter.sharedMesh == null)
                    return false;
                
                _buildSource.sourceObject = meshFilter.sharedMesh;
                _buildSource.component = null;
                _buildSource.size = Vector3.one;
                _buildSource.shape = NavMeshBuildSourceShape.Mesh;
            }
            else
            {
                _buildSource.component = collider;
                switch (collider)
                {
                    case BoxCollider boxCollider:
                        _buildSource.transform = Matrix4x4.Translate((_buildSource.transform * boxCollider.center.toVector4(0f)).homogeneousToCartesian()) * _buildSource.transform;
                        _buildSource.sourceObject = null;
                        _buildSource.size = boxCollider.size;
                        _buildSource.shape = NavMeshBuildSourceShape.Box;
                        break;
                    case CapsuleCollider capsuleCollider:
                        _buildSource.transform = Matrix4x4.Translate((_buildSource.transform * capsuleCollider.center.toVector4(0f)).homogeneousToCartesian()) * _buildSource.transform;
                        _buildSource.sourceObject = null;
                        _buildSource.size = new Vector3(capsuleCollider.radius,
                            capsuleCollider.height / 2f - capsuleCollider.radius, capsuleCollider.radius) * 2f;
                        _buildSource.shape = NavMeshBuildSourceShape.Capsule;
                        break;
                    case SphereCollider sphereCollider:
                        _buildSource.transform = Matrix4x4.Translate((_buildSource.transform * sphereCollider.center.toVector4(0f)).homogeneousToCartesian()) * _buildSource.transform;
                        _buildSource.sourceObject = null;
                        _buildSource.size = Vector3.one * (sphereCollider.radius * 2f);
                        _buildSource.shape = NavMeshBuildSourceShape.Sphere;
                        break;
                    case MeshCollider meshCollider:
                        _buildSource.sourceObject = meshCollider.sharedMesh;
                        _buildSource.component = null;
                        _buildSource.size = Vector3.one;
                        _buildSource.shape = NavMeshBuildSourceShape.Mesh;
                        break;
                    case TerrainCollider terrainCollider:
                        _buildSource.sourceObject = terrainCollider.terrainData;
                        _buildSource.size = Vector3.one;
                        _buildSource.shape = NavMeshBuildSourceShape.Terrain;
                        break;
                }
            }

            return true;
        }
    }
}
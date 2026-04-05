using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    /****************
     * Combine操作的子操作对象
     **/
    public class ALCombineMeshParentMono : MonoBehaviour
    {
        /** Mesh编号，用于与子对象核对 */
        public int timeTag;
        public int combineMeshID = 0;

        // Use this for initialization
        void Start()
        {
            //在本对象中查询子对象
            ALCombineMeshChildMono[] childMonoObjs = transform.GetComponentsInChildren<ALCombineMeshChildMono>(true);

            if (null == childMonoObjs || childMonoObjs.Length <= 0)
                return;

            List<CombineInstance> combineInstanceList = new List<CombineInstance>();
            //获取对象的MeshFilter创建绑定对象
            for (int i = 0; i < childMonoObjs.Length; i++)
            {
                ALCombineMeshChildMono childMono = childMonoObjs[i];

                if (null == childMono)
                    continue;

                //验证MeshID
                if (childMono.combineMeshID != combineMeshID || childMono.timeTag != timeTag)
                    continue;

                MeshFilter meshFilter = childMono.transform.GetComponent<MeshFilter>();
                if (null == meshFilter)
                    continue;

                //获取子对象网格
                CombineInstance combineInstance = new CombineInstance();
                combineInstance.mesh = meshFilter.sharedMesh;
                combineInstance.subMeshIndex = 0;
                combineInstance.transform = meshFilter.transform.localToWorldMatrix;

                combineInstanceList.Add(combineInstance);

                //设置本对象无效
                childMono.gameObject.SetActive(false);
            }

            //合并Mesh
            MeshFilter selfFilter = GetComponent<MeshFilter>();
            if (null == selfFilter)
                selfFilter = gameObject.AddComponent<MeshFilter>();

            //记录旧Mesh用于资源释放
            Mesh oldMesh = selfFilter.mesh;

            selfFilter.mesh = new Mesh();
            selfFilter.mesh.CombineMeshes(combineInstanceList.ToArray(), true, true);

            //释放旧Mesh
            ALUnityCommon.releaseGameObj(oldMesh);
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}
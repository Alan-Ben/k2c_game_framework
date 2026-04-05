using System;
using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    public static class ALUnityCommon
    {
        /*********************
         * 将某一物件下的所有物件渲染对象进行绑定
         **/
        public static void combineSkinnedMesh(Transform _root)
        {
            //绑定对象列表
            List<CombineInstance> combineInstanceList = new List<CombineInstance>();
            //绑定材质列表
            List<Material> materialList = new List<Material>();
            //绑定对象骨骼列表
            List<Transform> boneList = new List<Transform>();
            Dictionary<string, Transform> boneDic = new Dictionary<string, Transform>();

            //获取所有子对象中的SkinnedMeshRenderer对象
            SkinnedMeshRenderer[] childrenSkinnedArr = _root.GetComponentsInChildren<SkinnedMeshRenderer>(true);

            //逐个处理绑定
            for (int i = 0; i < childrenSkinnedArr.Length; i++)
            {
                SkinnedMeshRenderer childSMR = childrenSkinnedArr[i];

                if (null == childSMR)
                    continue;

                //获取所有子MESH
                //注意smr.materials中包含的材质数量和顺序与下面的sub mesh是对应的
                for (int meshIdx = 0; meshIdx < childSMR.sharedMesh.subMeshCount; meshIdx++)
                {
                    CombineInstance combineInstance = new CombineInstance();
                    combineInstance.mesh = childSMR.sharedMesh;
                    combineInstance.subMeshIndex = meshIdx;

                    //添加到队列中
                    combineInstanceList.Add(combineInstance);
                }

                //添加材质
                //不进行贴图判断是因为可能因为UV设置不同，使用的还是同一个贴图，但是材质效果是不同的
                //for (int materIdx = 0; materIdx < childSMR.materials.Length; materIdx++)
                //{
                //    Material material = childSMR.materials[materIdx];
                //    if (null == material)
                //        continue;
                //    //判断是否已经包含
                //    if (mainTextureSet.Contains(material.mainTexture))
                //        continue;
                //    materialList.AddRange(childSMR.materials);
                //    //将贴图放入判断集合
                //    mainTextureSet.Add(material.mainTexture);
                //}
                materialList.AddRange(childSMR.materials);

                //http://www.cnblogs.com/oldman/articles/2395518.html
                //添加所有骨骼
                // 网格点与骨骼的对应关系是通过Mesh数据结构中的BoneWeight数组来实现的。该数组
                // 与网格顶点数组对应，记录了每个网格点受骨骼（骨骼记录在SinkedMeshRender的bones
                // 数组中，按下标索引）影响的权重。
                // 而此处，示例程序提供的肢体Assets并不包含骨骼，而是返回骨骼名称。因此，推断
                // GetBoneNames()返回的骨骼名称应该与实际骨骼数组的顺序相同。
                Transform bone = null;
                for (int n = 0; n < childSMR.bones.Length; n++)
                {
                    bone = childSMR.bones[n];
                    if (null == bone)
                        continue;

                    Transform realBone = null;
                    if (boneDic.ContainsKey(bone.name))
                    {
                        realBone = boneDic[bone.name];
                    }
                    else
                    {
                        realBone = bone;
                        boneDic.Add(bone.name, bone);
                    }

                    boneList.Add(realBone);
                }

                //设置子对象Mesh无效
                childSMR.gameObject.SetActive(false);
            }

            //获取根节点SkinnedMeshRenderer 或为根节点添加
            SkinnedMeshRenderer renderer = _root.GetComponent<SkinnedMeshRenderer>();
            if (null == renderer)
                renderer = _root.gameObject.AddComponent<SkinnedMeshRenderer>();

            //设置脚本有效
            renderer.enabled = true;

            //记录旧Mesh，需要删除
            Mesh oldMesh = renderer.sharedMesh;
            //进行绑定操作
            renderer.sharedMesh = new Mesh();

            renderer.sharedMesh.CombineMeshes(combineInstanceList.ToArray(), false, false);
            renderer.bones = boneList.ToArray();
            renderer.materials = materialList.ToArray();

            //删除旧资源
            releaseGameObj(oldMesh);
        }

        /**************
         * 绑定对象身上操作的材质分组信息
         **/
        public class ALCombineSkinnedMaterialGroupInfo
        {
            /** 通用的材质对象 */
            public Material material;
            /** 使用同一材质的网格对象的绑定列表 */
            protected List<CombineInstance> _meshList;
            /** 使用同一材质的对象的骨骼队列 */
            protected List<Transform> _boneList = new List<Transform>();

            public ALCombineSkinnedMaterialGroupInfo(Material _material)
            {
                material = _material;
                _meshList = new List<CombineInstance>();
                _boneList = new List<Transform>();
            }

            /**************
             * 使用本集合中所有的网格合并创建出新的一个使用同一材质的网格
             **/
            public Mesh getMesh()
            {
                Mesh newMesh = new Mesh();
                newMesh.CombineMeshes(_meshList.ToArray(), true, false);

                return newMesh;
            }

            /**************
             * 获取本集合的骨骼队列
             **/
            public List<Transform> boneList
            {
                get { return _boneList; }
            }

            /***************
             * 增加绑定的网格对象
             **/
            public void addCombineMesh(CombineInstance _combineInstance)
            {
                _meshList.Add(_combineInstance);
            }

            /***************
             * 增加对象的骨骼处理
             **/
            public void addBone(Transform _bone)
            {
                _boneList.Add(_bone);
            }
        }

        /*********************
         * 将某一物件下的所有物件渲染对象进行绑定
         **/
        public static void combineSkinnedMesh(Transform _root, List<SkinnedMeshRenderer> _rootRenderList, List<SkinnedMeshRenderer> _combineRenderList)
        {
            if (null == _combineRenderList)
                return;

            //绑定对象骨骼列表
            Dictionary<string, Transform> boneDic = new Dictionary<string, Transform>();

            //将基本对象中的骨骼取出用于在后续操作中获取对应骨骼
            //并将基本对象中的mesh屏蔽，不进行合并mesh的操作
            for (int i = 0; i < _rootRenderList.Count; i++)
            {
                SkinnedMeshRenderer childSMR = _rootRenderList[i];
                if (null == childSMR)
                    continue;

                Transform bone = null;
                for (int n = 0; n < childSMR.bones.Length; n++)
                {
                    bone = childSMR.bones[n];
                    if (null == bone)
                        continue;

                    //骨骼不在集合内则添加进集合数据中
                    if (!boneDic.ContainsKey(bone.name))
                        boneDic.Add(bone.name, bone);

                    //设置蒙皮对象无效，不进行网格合并
                    childSMR.gameObject.SetActive(false);
                }
            }

            List<ALCombineSkinnedMaterialGroupInfo> combineInfoGroupList = new List<ALCombineSkinnedMaterialGroupInfo>();
            ALCombineSkinnedMaterialGroupInfo tmpGroupInfo = null;
            //逐个处理子对象的相关绑定
            for (int i = 0; i < _combineRenderList.Count; i++)
            {
                SkinnedMeshRenderer childSMR = _combineRenderList[i];
                if (null == childSMR)
                    continue;

                //获取所有子MESH
                //注意smr.materials中包含的材质数量和顺序与下面的sub mesh是对应的
                for (int meshIdx = 0; meshIdx < childSMR.sharedMesh.subMeshCount; meshIdx++)
                {
                    CombineInstance combineInstance = new CombineInstance();
                    combineInstance.mesh = childSMR.sharedMesh;
                    combineInstance.subMeshIndex = meshIdx;

                    //获取对应材质
                    Material subMaterial = childSMR.sharedMaterials[meshIdx];

                    int groupIdx = 0;
                    tmpGroupInfo = null;
                    //从集合中查询是否有一致的
                    for (; groupIdx < combineInfoGroupList.Count; groupIdx++)
                    {
                        tmpGroupInfo = combineInfoGroupList[groupIdx];
                        if (tmpGroupInfo.material == subMaterial)
                        {
                            //加入对应group
                            tmpGroupInfo.addCombineMesh(combineInstance);
                            break;
                        }
                    }

                    //当检索超出队列，则创建新节点
                    if (groupIdx >= combineInfoGroupList.Count)
                    {
                        tmpGroupInfo = new ALCombineSkinnedMaterialGroupInfo(subMaterial);
                        combineInfoGroupList.Add(tmpGroupInfo);

                        tmpGroupInfo.addCombineMesh(combineInstance);
                    }

                    //添加所有骨骼
                    // 网格点与骨骼的对应关系是通过Mesh数据结构中的BoneWeight数组来实现的。该数组
                    // 与网格顶点数组对应，记录了每个网格点受骨骼（骨骼记录在SinkedMeshRender的bones
                    // 数组中，按下标索引）影响的权重。
                    // 而此处，示例程序提供的肢体Assets并不包含骨骼，而是返回骨骼名称。因此，推断
                    // GetBoneNames()返回的骨骼名称应该与实际骨骼数组的顺序相同。
                    Transform bone = null;
                    for (int n = 0; n < childSMR.bones.Length; n++)
                    {
                        bone = childSMR.bones[n];
                        if (null == bone)
                            continue;

                        Transform realBone = null;
                        if (boneDic.ContainsKey(bone.name))
                        {
                            realBone = boneDic[bone.name];
                        }
                        else
                        {
                            UnityEngine.Debug.LogError("Bone: " + bone.name + " Can not find in the basic model!");
                            realBone = bone;
                            boneDic.Add(bone.name, bone);
                        }

                        tmpGroupInfo.addBone(realBone);
                    }
                }

                //设置子对象Mesh无效
                childSMR.gameObject.SetActive(false);
            }

            //获取根节点SkinnedMeshRenderer 或为根节点添加
            SkinnedMeshRenderer renderer = _root.GetComponent<SkinnedMeshRenderer>();
            if (null == renderer)
                renderer = _root.gameObject.AddComponent<SkinnedMeshRenderer>();

            //绑定对象列表
            List<CombineInstance> combineInstanceList = new List<CombineInstance>();
            //绑定材质列表
            List<Material> materialList = new List<Material>();
            //骨骼队列
            List<Transform> boneList = new List<Transform>();
            //创建新的绑定mesh队列
            for (int meshIdx = 0; meshIdx < combineInfoGroupList.Count; meshIdx++)
            {
                tmpGroupInfo = combineInfoGroupList[meshIdx];

                CombineInstance combineInstance = new CombineInstance();
                combineInstance.mesh = tmpGroupInfo.getMesh();
                combineInstance.subMeshIndex = 0;

                //加入队列
                combineInstanceList.Add(combineInstance);
                //加入材质
                materialList.Add(tmpGroupInfo.material);
                //加入骨骼
                boneList.AddRange(tmpGroupInfo.boneList);
            }

            //记录旧Mesh，需要删除
            Mesh oldMesh = renderer.sharedMesh;
            //进行绑定操作
            renderer.sharedMesh = new Mesh();

            renderer.sharedMesh.CombineMeshes(combineInstanceList.ToArray(), false, false);
            renderer.bones = boneList.ToArray();
            renderer.materials = materialList.ToArray();

            //释放所有group创建出的mesh
            for (int i = 0; i < combineInstanceList.Count; i++)
            {
                releaseGameObj(combineInstanceList[i].mesh);
            }

            //删除旧资源
            releaseGameObj(oldMesh);
        }

        /*********************
         * 将所有Mesh进行绑定处理
         **/
        public static Mesh combineMesh(List<MeshFilter> _combineRenderList)
        {
            if (null == _combineRenderList)
                return null;

            /** 使用同一材质的网格对象的绑定列表 */
            List<CombineInstance> _meshList = new List<CombineInstance>();
            //逐个处理子对象的相关绑定
            for (int i = 0; i < _combineRenderList.Count; i++)
            {
                MeshFilter childMR = _combineRenderList[i];
                if (null == childMR)
                    continue;

                //获取所有子MESH
                //注意smr.materials中包含的材质数量和顺序与下面的sub mesh是对应的
                for (int meshIdx = 0; meshIdx < childMR.sharedMesh.subMeshCount; meshIdx++)
                {
                    CombineInstance combineInstance = new CombineInstance();
                    combineInstance.mesh = childMR.sharedMesh;
                    combineInstance.subMeshIndex = meshIdx;

                    _meshList.Add(combineInstance);
                }

                //设置子对象Mesh无效
                childMR.gameObject.SetActive(false);
            }

            //记录旧Mesh，需要删除
            Mesh newMesh = new Mesh();
            newMesh.CombineMeshes(_meshList.ToArray(), false, false);

            return newMesh;
        }

        /*********************
         * 将带入的蒙皮对象队列中的所有对象的骨骼与根节点对象的骨骼进行拼接
         **/
        public static void linkSkinnedMeshBones(List<SkinnedMeshRenderer> _rootRenderList, List<SkinnedMeshRenderer> _linkBonesRenderList)
        {
            if (null == _linkBonesRenderList)
                return;

            //存放骨骼名称和对应骨骼的映射表
            Dictionary<string, Transform> boneDic = new Dictionary<string, Transform>();
            //骨骼根节点
            Transform rootBone = null;

            //将基本对象中的骨骼取出用于在后续操作中获取对应骨骼
            //并将基本对象中的mesh屏蔽，不进行合并mesh的操作
            for (int i = 0; i < _rootRenderList.Count; i++)
            {
                SkinnedMeshRenderer childSMR = _rootRenderList[i];
                if (null == childSMR)
                    continue;

                //获取骨骼根节点对象
                if (null == rootBone)
                    rootBone = childSMR.rootBone;
                
                Transform bone = null;
                for(int n = 0; n < childSMR.bones.Length; n++)
                {
                    bone = childSMR.bones[n];
                    if(null == bone)
                        continue;

                    //骨骼不在集合内则添加进集合数据中
                    if (!boneDic.ContainsKey(bone.name))
                        boneDic.Add(bone.name, bone);

                    //设置蒙皮对象无效，不进行网格合并
                    childSMR.gameObject.SetActive(false);
                }
            }

            //逐个处理子对象的相关绑定
            for (int i = 0; i < _linkBonesRenderList.Count; i++)
            {
                SkinnedMeshRenderer linkRender = _linkBonesRenderList[i];
                if (null == linkRender)
                    continue;

                //绑定对象骨骼列表
                List<Transform> boneList = new List<Transform>();

                //将需要连接的对象的骨骼集合获取到，之后重置本对象中的骨骼列表即可
                Transform bone = null;
                for(int n = 0; n < linkRender.bones.Length; n++)
                {
                    bone = linkRender.bones[n];
                    if(null == bone)
                        continue;

                    Transform realBone = null;
                    if (boneDic.ContainsKey(bone.name))
                    {
                        realBone = boneDic[bone.name];
                    }
                    else
                    {
                        UnityEngine.Debug.LogError("Bone: " + bone.name + " Can not find in the basic model!");
                        realBone = bone;
                        boneDic.Add(bone.name, bone);
                    }

                    boneList.Add(realBone);
                }

                //设置骨骼根节点
                linkRender.rootBone = rootBone;
                //设置对象的新骨骼列表
                linkRender.bones = boneList.ToArray();
            }
        }

        /**********************
         * 向下发射射线获取地面点位置
         **/
        public static Vector3 getInstanceGroundPoint(Vector3 _pos)
        {
            //使用射线判断角色离地面距离，抬高0.1米进行计算，保证不穿透，同时需要将角色控制对象的偏移计算进去
            _pos.y += 0.1f;
            RaycastHit rayHit = new RaycastHit();
            if (Physics.Raycast(
                _pos
                , new Vector3(0, -1, 0)
                , out rayHit
                , 10f
                , ALGlobalControl.instance.mapItemLayerMask))
            {
                return rayHit.point;
            }
            else
            {
                //无碰撞表示当作当前点处理
                return _pos;
            }
        }

        /*****************
         * 释放所有烘培贴图相关资源
         **/
        public static void releaseAllLightMap()
        {
            LightmapSettings.lightmaps = null;
        }

        /******************
         * 移动当前焦点输入对象到最后位置
         **/
        public static void moveFocusTextToEnd()
        {
            TextEditor obj = GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl) as TextEditor;

            if (null == obj)
                return;

            obj.SelectTextEnd();
        }

        /*******************
         * 根据摄像头朝向获取相对坐标位置的实际场景位置
         **/
        public static Vector3 getTransPosByCameraForward(Vector3 _normalForward, Vector3 _pos)
        {
            //计算x轴方向向量
            Vector3 forwardX = new Vector3(_normalForward.z, 0, -_normalForward.x);
            forwardX.Normalize();
            //计算y轴方向向量，假设摄像头都是正向的
            float hLen = Mathf.Sqrt(1 - (_normalForward.y * _normalForward.y));

            Vector3 forwardY = new Vector3(-_normalForward.x * (_normalForward.y / hLen), hLen, -_normalForward.z * (_normalForward.y / hLen));
            return (_pos.x * forwardX) + (_pos.y * forwardY) + (_pos.z * _normalForward);
        }
        public static Vector3 getTransPosByCameraForward(Quaternion _quaternion, Vector3 _pos)
        {
            //计算反向旋转结果
            return (Quaternion.Inverse(_quaternion) * _pos);
        }

        /*******************
         * 根据摄像头朝向获取水平位置相对坐标位置的实际场景位置
         **/
        public static Vector3 getPanelPosByCameraForward(Vector3 _normalForward, Vector3 _pos)
        {
            //计算x轴方向向量
            Vector3 forwardX = new Vector3(_normalForward.z, 0, -_normalForward.x);
            forwardX.Normalize();
            Vector3 forwardZ = new Vector3(-forwardX.z, 0, forwardX.x);
            //根据两个朝向计算最后位置
            return (_pos.x * forwardX) + (_pos.z * forwardZ);
        }
        public static Vector3 getPanelPosByCameraForward (Vector3 _normalForward, Vector2 _pos) {
            //计算x轴方向向量
            Vector3 forwardX = new Vector3(_normalForward.z, 0, -_normalForward.x);
            forwardX.Normalize();
            Vector3 forwardZ = new Vector3(-forwardX.z, 0, forwardX.x);
            //根据两个朝向计算最后位置
            return (_pos.x * forwardX) + (_pos.y * forwardZ);
        }

        /*****************
         * 获取某一朝向的方向变量
         **/
        public static Quaternion getForwardQuaternion(Vector3 _forward)
        {
            if(Math.Abs(_forward.x - 0) < 0.005f)
            {
                if(_forward.x > 0)
                    _forward.x = 0.005f;
                else
                    _forward.x = -0.005f;
            }
            if(Math.Abs(_forward.z - 0) < 0.005f)
            {
                if(_forward.z > 0)
                    _forward.z = 0.005f;
                else
                    _forward.z = -0.005f;
            }

            return Quaternion.FromToRotation(Vector3.forward, _forward);
        }

#if AL_CREATURE_SYS
        /**********************
         * 根据带入的移动方向，以及相机对象，返回具体的移动方向
         **/
        public static Vector3 getMoveDirection(Vector3 _cameraForward, int _direction)
        {
            //获取当前摄像头的前进和右边方向
            Vector3 forward = new Vector3(_cameraForward.x, 0, _cameraForward.z);
            Vector3 right = new Vector3(_cameraForward.z, 0, -_cameraForward.x);

            float speedF = 0;
            float speedR = 0;

            if((_direction & ALCreatureControl.MOVE_FORWARD) != 0)
            {
                speedF += 1;
            }
            if((_direction & ALCreatureControl.MOVE_BACK) != 0)
            {
                speedF -= 1;
            }
            if((_direction & ALCreatureControl.MOVE_LEFT) != 0)
            {
                speedR -= 1;
            }
            if((_direction & ALCreatureControl.MOVE_RIGHT) != 0)
            {
                speedR += 1;
            }

            //根据获得的方向速度获得实际的前进方向
            if(speedF != 0 || speedR != 0)
            {
                //计算方向向量，并将向量长度归为1
                Vector3 moveDirection;
                moveDirection = (forward * speedF) + (right * speedR);
                moveDirection.Normalize();

                return moveDirection;
            }
            else
            {
                return new Vector3(0, 0, 0);
            }
        }
#endif
        public static Vector3 getMoveDirection(Vector3 _cameraForward, Vector2 _direction)
        {
            //获取当前摄像头的前进和右边方向
            Vector3 forward = new Vector3(_cameraForward.x, 0, _cameraForward.z);
            Vector3 right = new Vector3(_cameraForward.z, 0, -_cameraForward.x);

            //根据获得的方向速度获得实际的前进方向
            if(_direction.x != 0 || _direction.y != 0)
            {
                //计算方向向量，并将向量长度归为1
                Vector3 moveDirection;
                moveDirection = (forward * _direction.y) + (right * _direction.x);
                moveDirection.Normalize();

                return moveDirection;
            }
            else
            {
                return new Vector3(0, 0, 0);
            }
        }


        /// <summary>
        /// 将某一个transform移动到其父节点下的第一个子对象
        /// </summary>
        /// <param name="_trans"></param>
        public static void moveTransformToFirst(Transform _trans)
        {
            if(null == _trans)
                return;

            _trans.SetAsFirstSibling();
        }
        public static void moveTransformToFirst(_AALBasicUIWndMono _wnd)
        {
            if(null == _wnd || null == _wnd.transform)
                return;

            _wnd.transform.SetAsFirstSibling();
        }
        /// <summary>
        /// 将某一个transform移动到其父节点下的最后一个子对象
        /// </summary>
        /// <param name="_trans"></param>
        public static void moveTransformToLast(Transform _trans)
        {
            if(null == _trans)
                return;

            _trans.SetAsLastSibling();
        }
        public static void moveTransformToLast(GameObject _go)
        {
            if (null == _go || null == _go.transform)
                return;

            _go.transform.SetAsLastSibling();
        }
        public static void moveTransformToLast(_AALBasicUIWndMono _wnd)
        {
            if(null == _wnd || null == _wnd.transform)
                return;

            _wnd.transform.SetAsLastSibling();
        }
        /// <summary>
        /// 将某一个transform移动到其父节点下的指定位置的子对象
        /// </summary>
        /// <param name="_trans"></param>
        public static void moveTransformToIndex(Transform _trans, int _index)
        {
            if(null == _trans)
                return;

            _trans.SetSiblingIndex(_index);
        }
        public static void moveTransformToIndex(_AALBasicUIWndMono _wnd, int _index)
        {
            if(null == _wnd || null == _wnd.transform)
                return;

            _wnd.transform.SetSiblingIndex(_index);
        }

        /// <summary>
        /// 刷新一次layer
        /// </summary>
        /// <param name="_wnd"></param>
        public static void refreshLayer(_AALBasicUIWndMono _wnd)
        {
            if(null == _wnd || null == _wnd.transform)
                return;

            ALUILayerMgr.instance.refreshNodeLayer(_wnd.transform);
        }
        public static void refreshLayer(Transform _transform)
        {
            if(null == _transform)
                return;

            ALUILayerMgr.instance.refreshNodeLayer(_transform);
        }
        public static void refreshLayer(GameObject _go)
        {
            if(null == _go || null == _go.transform)
                return;

            ALUILayerMgr.instance.refreshNodeLayer(_go.transform);
        }
        
        /***********************
         * 统一释放资源函数
         **/
        public static void releaseGameObj(UnityEngine.MonoBehaviour _obj)
        {
            if(null == _obj || null == _obj.gameObject)
                return;

            GameObject.Destroy(_obj.gameObject);
        }
        public static void releaseGameObj(UnityEngine.Object _obj)
        {
            if (null == _obj)
                return;
#if UNITY_EDITOR
            if(_obj is GameObject == false && _obj is Mesh == false)
            {
                Debug.LogError($"尝试销毁Component: {_obj}，一般来说是想要销毁GameObject的，请检查");
            }
#endif
            GameObject.Destroy(_obj);
        }
        public static void releaseGameObj(_AALBasicUIWndMono _wndMono)
        {
            if(null == _wndMono || null == _wndMono.gameObject)
                return;

            GameObject.Destroy(_wndMono.gameObject);
        }
        
        public static T AddMissingComponent<T>(this GameObject _go) where T : Component
        {
            T comp = _go.GetComponent<T>();
            if (comp == null)
            {
                comp = _go.AddComponent<T>();
            }
            return comp;
        }

        public static T AddMissingComponent<T>(this Component _comp) where T : Component
        {
            GameObject go = _comp.gameObject;
            T comp = go.GetComponent<T>();
            if (comp == null)
            {
                comp = go.AddComponent<T>();
            }
            return comp;
        }
    }
}

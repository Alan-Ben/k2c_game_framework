using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace ALPackage
{
    public class ALEditorMenu
    {
        [MenuItem("ALMenu/ALEditor/Manage All Selected GameObjects' Meshes by Material in Hierarchy")]
        public static void exportGameObject()
        {
            //获取选中的所有对象
            GameObject[] selectedObjs = Selection.gameObjects;

            //存储已经处理过的Material集合
            Dictionary<Texture, int> dealedMaterialDic = new Dictionary<Texture, int>();
            //不同材质对应的父节点
            Dictionary<int, GameObject> materialParentObjDic = new Dictionary<int, GameObject>();
            //创建根节点
            GameObject rootObj = new GameObject();
            System.DateTime now = System.DateTime.Now;
            rootObj.name = "ALMeshManageRoot_" + now.Year + "_" + now.Month + "_" + now.Day + "_" + now.Hour + "_" + now.Minute + "_" + now.Second;

            //使用时间进行标记，防止不同时间进行的分类进行了统一处理
            int timeTag = ALCommon.getNowTimeSec();
            int materialID = 0;

            //每个对象进行处理
            for (int i = 0; i < selectedObjs.Length; i++)
            {
                GameObject gameObj = selectedObjs[i];

                if (null == gameObj)
                    continue;

                //获取所有对象的渲染脚本，为获取材质做准备
                MeshRenderer[] meshRenderList = gameObj.GetComponentsInChildren<MeshRenderer>(true);

                for (int renderIdx = 0; renderIdx < meshRenderList.Length; renderIdx++)
                {
                    MeshRenderer render = meshRenderList[renderIdx];
                    if (null == render)
                        continue;

                    //拥有parent脚本意味着本对象是脚本生成
                    if (render.gameObject.GetComponent<ALCombineMeshParentMono>() != null)
                        continue;

                    //获取材质
                    Material renderMaterial = render.material;
                    if (null == renderMaterial)
                        continue;

                    //获取材质编号
                    int mID = 0;
                    GameObject parentObj = null;

                    if (dealedMaterialDic.ContainsKey(renderMaterial.mainTexture))
                    {
                        mID = dealedMaterialDic[renderMaterial.mainTexture];
                        //获取父节点
                        parentObj = materialParentObjDic[mID];
                    }
                    else
                    {
                        materialID++;
                        mID = materialID;

                        //添加新材质
                        dealedMaterialDic.Add(renderMaterial.mainTexture, mID);
                        //创建新的父节点
                        parentObj = new GameObject();
                        parentObj.name = "Material_" + mID;
                        //放置到根节点下
                        parentObj.transform.SetParent(rootObj.transform);
                        //在父节点上增加脚本
                        ALCombineMeshParentMono parentCombineMono = parentObj.AddComponent<ALCombineMeshParentMono>();
                        parentCombineMono.combineMeshID = mID;
                        parentCombineMono.timeTag = timeTag;
                        //父节点增加绘制Render
                        MeshRenderer parentCombineRender = parentObj.AddComponent<MeshRenderer>();
                        Material[] materialArr = new Material[1];
                        materialArr[0] = renderMaterial;
                        parentCombineRender.materials = materialArr;
                        //设置是否使用阴影
                        parentCombineRender.shadowCastingMode = ALSOGlobalSetting.Instance.shadowCastingMode;
                        //加入ID映射集合
                        materialParentObjDic.Add(mID, parentObj);
                    }

                    //将本对象的CombineChilde脚本删除
                    ALCombineMeshChildMono childMono = render.transform.GetComponent<ALCombineMeshChildMono>();
                    if (null == childMono)
                        childMono = render.gameObject.AddComponent<ALCombineMeshChildMono>();

                    //设置材质ID
                    childMono.combineMeshID = mID;
                    childMono.timeTag = timeTag;
                    //设置本对象的父节点
                    render.transform.SetParent(parentObj.transform);

                    //当本对象有Combine父脚本则设置其无效
                    ALCombineMeshParentMono parentMono = render.transform.GetComponent<ALCombineMeshParentMono>();
                    if (null != parentMono)
                        parentMono.enabled = false;
                }
            }
        }
    }
}

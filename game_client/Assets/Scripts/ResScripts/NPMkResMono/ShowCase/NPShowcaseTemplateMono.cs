using System;
using System.Collections.Generic;
using ALPackage;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace GOE
{
    /// <summary>
    /// showCase对象mono，用于配置单位加载的位置、摄像头信息
    /// </summary>
    public class NPShowcaseTemplateMono : MonoBehaviour
    {
        [ALHeader("---------说明策划填写备注----------------------------------")]
        public string info;
        [ALHeader("视觉中心, MainCamera类型跟随UI视觉中心偏移， 需配合ShowCase UI的viewCenterRect使用")]
        public Transform viewCenter;
        [ALHeader("单位加载位置列表")]
        public List<Transform> unitList;
        [ALHeader("需要全局注册的单位动画标记列表,索引跟上面单位列表对应，可以支持对加载的不同单位播放动画")]
        public List<string> regAnimatorTagList;
        [ALHeader("单位加载后初始位置是否需用0，0，0,当作初始坐标和角度")]
        public bool useDefaultPos;
        
        [ALHeader("摄像机机位配置数据")]
        public CameraPlaceholder cameraPlaceholders;
        [ALHeader("是否开启玩家模型的阴影范围裁剪边界")]
        public bool isOpenPlayerShadowBounds = false;
        [ALHeader("玩家模型的阴影范围,配相对坐标就行，程序会加上父节点偏移")]
        public Bounds playerShadowBounds;
        [ALHeader("模板灯光配置配置")]
        public GLightGoIndex lightGoIndex;
        
        //最终用到的阴影范围
        public Bounds finalPlayerShadowBounds
        {
            get
            {
                Bounds finalValue = playerShadowBounds;
                finalValue.center += transform.position;
                return finalValue;
            }
        }

        //根据index获取对应动画标记
        public string getAnimatorTagByIndex(int _index)
        {
            if (null != regAnimatorTagList && regAnimatorTagList.Count > _index)
                return regAnimatorTagList[_index];
            return null;
        }
        
#if UNITY_EDITOR
        private void Update()
        {
            if(isOpenPlayerShadowBounds)
                DebugPlus.DrawBox(finalPlayerShadowBounds.center, finalPlayerShadowBounds.size, Quaternion.identity, Color.red);
        }
#endif
    }
}
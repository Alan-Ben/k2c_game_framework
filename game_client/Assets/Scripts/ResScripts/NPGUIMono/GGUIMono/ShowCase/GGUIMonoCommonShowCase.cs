
using ALPackage;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace GOE
{
    [System.Serializable]
    public class NPGGUIMonoCommonShowCase_GoMono
    {
        [ALHeader("加载到舞台的位置")]
        public int loadIndex;
        [ALHeader("加载的go对象")]
        public NPGGoIndex goIndex;
    }
    
    /// <summary>
    /// showcase通用子窗口
    /// </summary>
    public class GGUIMonoCommonShowCase : _AALBasicUIWndMono
    {
        [ALHeader("正在Loading时，显示的go")]
        public GameObject goLoading;
        
        [ALHeader("渲染图片")]
        public RawImage imageBodyAndBG;
        
        [ALHeader("舞台配置")]
        public NPGShowcaseIndex templateIndex;

        [ALHeader("舞台自定义加载go数据列表，如果跟程序传参位置冲突以程序传参为准")]
        public List<NPGGUIMonoCommonShowCase_GoMono> loadGoMonoList;
        
        [ALHeader("显示时候执行的effect")]
        public _NPPlayerEffectSerializeInfo initShowEffect;
        
        [ALHeader("应该用哪一种摄像机")]
        public EShowcaseCameraType cameraType = EShowcaseCameraType.MainCamera;

        [ALHeader("是否根据RawImage的大小调整RT的大小")]
        public bool isResizeRTByRect = false;
        
        [ALHeader("拖拽点击区域")]
        public GameObject btnDragArea;
        
        [ALHeader("是否允许拖拽旋转")]
        public bool isRotate;
        [ALHeader("拖拽旋转的单位索引（第几个index）")]
        public List<int> rotateUnitIndexList;
        [ALHeader("拖拽旋转速度（ui上移动的单位距离 * rotateSpeed = 旋转角度）")]
        [Min(0.1f)]
        public float rotateSpeed = 0.2f;
        
        [ALHeader("UI视觉中心（设置了值会开启相机跟随UI视觉中心位置进行偏移)")]
        [ALHeader("--------MainCamera类型跟随UI视觉中心偏移--------")]
        [Tooltip("1.只针对Camera Type为Main Camera的有用\n 2.设置了值就开启相机跟随UI视觉中心偏移 \n")]
        public RectTransform viewCenterRect;
    }
}
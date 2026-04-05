using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


namespace UnityEngine.Experimental.Rendering.Universal
{
#if UNITY_EDITOR
    using UnityEditor;
    [CustomPropertyDrawer(typeof(ScreenBlurRenderFeature.FilterSettings))]
    public class FilterSettingsDrawer : PropertyDrawer
    {
        internal class Styles
        {
            public static float defaultLineSpace = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            public static GUIContent filtersHeader = new GUIContent("Filters", "Settings that control which objects should be rendered.");
            public static GUIContent renderQueueFilter = new GUIContent("Queue", "Only render objects in the selected render queue range.");
            public static GUIContent layerMask = new GUIContent("Layer Mask", "Only render objects in a layer that match the given layer mask.");
            public static GUIContent sortingLayerAll = new GUIContent("Sorting Layer All", "全部渲染");
            public static GUIContent frontSortingLayerStyle = EditorGUIUtility.TrTextContent("Front Sorting Layer", "Name of the Renderer's sorting layer");
            public static GUIContent backSortingLayerStyle = EditorGUIUtility.TrTextContent("Back Sorting Layer", "Name of the Renderer's sorting layer");
        }

        private SerializedProperty m_RenderQueue;
        private SerializedProperty m_LayerMask;
        private SerializedProperty m_SortingLayerAll;
        private SerializedProperty m_FrontSortingLayerID;
        private SerializedProperty m_BackSortingLayerID;
        
        class HeaderBool
        {
            public string key;
            public bool value;

            public HeaderBool(string _key, bool _default = false)
            {
                key = _key;
                if (EditorPrefs.HasKey(key))
                    value = EditorPrefs.GetBool(key);
                else
                    value = _default;
                EditorPrefs.SetBool(key, value);
            }
        }
        private HeaderBool m_FiltersFoldout = new HeaderBool($"ScreenBlurRenderFeature.FilterSettings.FiltersFoldout", true);

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            m_SortingLayerAll = property.FindPropertyRelative("SortingLayerAll");
            m_FrontSortingLayerID = property.FindPropertyRelative("FrontSortingLayerID");
            m_BackSortingLayerID = property.FindPropertyRelative("BackSortingLayerID");
            m_RenderQueue = property.FindPropertyRelative("RenderQueueType");
            m_LayerMask = property.FindPropertyRelative("LayerMask");

            rect.height = EditorGUIUtility.singleLineHeight;
            m_FiltersFoldout.value = EditorGUI.Foldout(rect, m_FiltersFoldout.value, label, true);
            SaveHeaderBool(m_FiltersFoldout);
            rect.y += Styles.defaultLineSpace;
            if (m_FiltersFoldout.value)
            {
                EditorGUI.indentLevel++;
                //Render queue filter
                EditorGUI.PropertyField(rect, m_RenderQueue, Styles.renderQueueFilter);
                rect.y += Styles.defaultLineSpace;
                //Layer mask
                EditorGUI.PropertyField(rect, m_LayerMask, Styles.layerMask);
                rect.y += Styles.defaultLineSpace;
                EditorGUI.PropertyField(rect, m_SortingLayerAll, Styles.sortingLayerAll);
                rect.y += Styles.defaultLineSpace;
                if (!m_SortingLayerAll.boolValue)
                {
                    SortingLayerField(rect, Styles.frontSortingLayerStyle, m_FrontSortingLayerID);
                    rect.y += Styles.defaultLineSpace;
                    SortingLayerField(rect, Styles.backSortingLayerStyle, m_BackSortingLayerID);
                    rect.y += Styles.defaultLineSpace;
                }
                EditorGUI.indentLevel--;
            }

        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = Styles.defaultLineSpace;
            m_SortingLayerAll = property.FindPropertyRelative("SortingLayerAll");

            height += Styles.defaultLineSpace * (m_FiltersFoldout.value ? (m_SortingLayerAll.boolValue ? 3: 5) : 0);
            return height;
        }

        public void SortingLayerField(Rect rect,GUIContent label, SerializedProperty property)
        {
            string[] layerNames = new string[SortingLayer.layers.Length];
            for (int i = 0; i < SortingLayer.layers.Length; i++)
                layerNames[i] = SortingLayer.layers[i].name;
            GUIContent[] layerContents = new GUIContent[layerNames.Length];
            for (int i = 0; i < layerNames.Length; i++)
                layerContents[i] = new GUIContent(layerNames[i]);
		    
            int baseValue = SortingLayer.GetLayerValueFromName(layerNames[0]);

            int layerValue = SortingLayer.GetLayerValueFromID(property.intValue) - baseValue;
            layerValue = EditorGUI.Popup(rect, label, layerValue,layerContents);
            SortingLayer layer = SortingLayer.layers[layerValue];
            property.intValue = layer.id;
        }
        private void SaveHeaderBool(HeaderBool boolObj)
        {
            EditorPrefs.SetBool(boolObj.key, boolObj.value);
        }
    }
#endif
    
    
    public class ScreenBlurRenderFeature : ScriptableRendererFeature
    {
        
        [System.Serializable]
        public class BlurSettings
        {
            //模糊扩散度
            [SerializeField, Range(0.0f, 20.0f), Header("[模糊扩散度]进行高斯模糊时，相邻像素点的间隔。此值越大相邻像素间隔越远，图像越模糊。但过大的值会导致失真。")]
            public float BlurSpreadSize = 2.0f;

            [SerializeField, Range(1, 8), Header("[迭代次数]此值越大,则模糊操作的迭代次数越多，模糊效果越好，但消耗越大。")]
            public int BlurIterations = 1;

            [SerializeField, Header("模糊材质")]
            public Material BlurMaterial;

            [SerializeField, Header("是否要开启降采样，如果是RenderTexture直接变小的就不需要，直接相机则需要")]
            public bool needDownSample = false;

            [SerializeField, Range(0, 6), Header("[降采样次数]向下采样的次数。此值越大,则采样间隔越大,需要处理的像素点越少,运行速度越快。")]
            public int DownSampleNum = 2;
            public Color BlurColor = Color.white;
            
            [SerializeField, Header("Blit材质")]
            public Material BlitMaterial;

            public string ShaderKeyword;
        }
        
        
        [System.Serializable]
        public class FilterSettings
        {
            public string passTag = "RenderObjectsFeature";
            // TODO: expose opaque, transparent, all ranges as drop down
            public RenderQueueType RenderQueueType;
            public LayerMask LayerMask;
            public bool SortingLayerAll = true;
            public int FrontSortingLayerID;
            public int BackSortingLayerID;

            public FilterSettings()
            {
                RenderQueueType = RenderQueueType.Transparent;
                LayerMask = 0;
            }

            public SortingLayerRange getSortingLayerRange()
            {
                var sortingLayerRange = SortingLayerRange.all;
                if (!SortingLayerAll)
                {
                    int layerFront = SortingLayer.GetLayerValueFromID(FrontSortingLayerID);
                    int layerBack = SortingLayer.GetLayerValueFromID(BackSortingLayerID);
                    sortingLayerRange = new SortingLayerRange((short) layerFront, (short) layerBack);
                }
                return sortingLayerRange;
            }
        }

        
        public RenderPassEvent Event = RenderPassEvent.AfterRenderingTransparents;

        public BlurSettings blurSettings;
        [Header("受模糊影响")]public FilterSettings beforeBlurFilter = new FilterSettings();
        [Header("【不】受模糊影响")]public FilterSettings afterBlurFilter = new FilterSettings();

        
        private ScreenBlurPass _m_screenBlurPass;

        private CopyColorSimplePass _m_copyColorPass;
        
        private RenderObjectsPass _m_beforeBlurPass;
        private RenderObjectsPass _m_afterBlurPass;

        // private RenderTexture _m_staticBlurTexture;
        
        public override void Create()
        {
            _m_beforeBlurPass = new RenderObjectsPass(beforeBlurFilter.passTag, Event, null, beforeBlurFilter.RenderQueueType, beforeBlurFilter.LayerMask, beforeBlurFilter.getSortingLayerRange(), new RenderObjects.CustomCameraSettings());
            _m_screenBlurPass = new ScreenBlurPass(Event, blurSettings.BlurMaterial);
            _m_copyColorPass = new CopyColorSimplePass(Event, blurSettings.BlitMaterial, blurSettings.ShaderKeyword);
            _m_afterBlurPass = new RenderObjectsPass(afterBlurFilter.passTag, Event, null, afterBlurFilter.RenderQueueType, afterBlurFilter.LayerMask, afterBlurFilter.getSortingLayerRange(), new RenderObjects.CustomCameraSettings());
        }

        private void OnDisable()
        {
            releaseBlurRt();
            Dispose();
        }

        void initBlurRt(RenderTextureDescriptor _descriptor)
        {
            releaseBlurRt();
            // _m_staticBlurTexture = RenderTexture.GetTemporary(_descriptor);
        }
        
        void releaseBlurRt()
        {
            // if (_m_staticBlurTexture != null)
            // {
            //     RenderTexture.ReleaseTemporary(_m_staticBlurTexture);
            //     _m_staticBlurTexture = null;
            // }
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            switch (MJUniversalRenderAPI.uiBlurState)
            {
                case UIBlurState.Default:
                    releaseBlurRt();
                    renderer.EnqueuePass(_m_beforeBlurPass);
                    renderer.EnqueuePass(_m_afterBlurPass);
                    break;
                case UIBlurState.StaticBlur:
                    renderer.EnqueuePass(_m_beforeBlurPass);
                    _m_screenBlurPass.Setup(blurSettings.BlurSpreadSize, blurSettings.BlurIterations, blurSettings.BlurMaterial, blurSettings.needDownSample, blurSettings.DownSampleNum, blurSettings.BlurColor);
                    renderer.EnqueuePass(_m_screenBlurPass);
                    initBlurRt(renderingData.cameraData.cameraTargetDescriptor);
                    _m_copyColorPass.SetupScreenToTarget(MJUniversalRenderAPI.uiBlurRenderTexture);
                    renderer.EnqueuePass(_m_copyColorPass);
                    renderer.EnqueuePass(_m_afterBlurPass);
                    MJUniversalRenderAPI.uiBlurState = UIBlurState.AfterStaticBlur;
                    break;
                case UIBlurState.AfterStaticBlur:
                    // _m_copyColorPass.SetupTargetToScreeen(_m_staticBlurTexture);
                    // renderer.EnqueuePass(_m_copyColorPass);
                    renderer.EnqueuePass(_m_afterBlurPass);
                    break;
                case UIBlurState.RealTimeBlur:
                    releaseBlurRt();
                    renderer.EnqueuePass(_m_beforeBlurPass);
                    _m_screenBlurPass.Setup(blurSettings.BlurSpreadSize, blurSettings.BlurIterations, blurSettings.BlurMaterial, blurSettings.needDownSample, blurSettings.DownSampleNum, blurSettings.BlurColor);
                    renderer.EnqueuePass(_m_screenBlurPass);
                    renderer.EnqueuePass(_m_afterBlurPass);
                    break;
                default:
                    releaseBlurRt();
                    Debug.LogError("ScreenBlurRenderFeature BlurState Error");
                    break;
            }
         
        }
    }
}
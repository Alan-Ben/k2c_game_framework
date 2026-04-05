using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public enum EMaterialValueType
    {
        INT,
        FLOAT,
        COLOR,
        VECTOR4,
    }   
    #if UNITY_EDITOR
    
    public class MaterialBridgeMgr
    {
        private static MaterialBridgeMgr _g_instance = new MaterialBridgeMgr();
        /// <summary>
        /// 仅编辑器下用
        /// </summary>
        [NotNull]
        public static MaterialBridgeMgr instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new MaterialBridgeMgr();

                return _g_instance;
            }
        }
 
        private Dictionary<Material, Material> _m_dMats = new Dictionary<Material, Material>();

        public Material getMat(Material _mat)
        {
            if (_mat == null)
                return null;
            Material mat;
            if (!_m_dMats.TryGetValue(_mat, out mat))
            {
                mat = UnityEngine.Object.Instantiate(_mat);
                _m_dMats.Add(_mat, mat);
            }
            return mat;
        }
    }
    
    #endif
    
    /// <summary>
    /// RawImage材质桥,用于animation控制材质的shader参数
    /// </summary>
    public class GGUIMonoTextureMatiralBridge: MonoBehaviour
    {
        [ALHeader("目标rawimage")]
        public RawImage targetRawImage;
        [ALHeader("开始材质动画")]
        public bool startWithMaterial;
        [ALHeader("目标参数名")]
        public string targetValueName;
        [ALHeader("目标参数名")]
        public EMaterialValueType targetValueType;
        [ALHeader("目标Vector4参数值")]
        public Vector4 vec4Value;
        [ALHeader("目标Color参数值")]
        public Color colorValue;
        [ALHeader("目标float参数值")]
        public float floatValue;
        [ALHeader("目标int参数值")]
        public int intValue;

        private void Awake()
        {
            if (null != targetRawImage)
            {
#if UNITY_EDITOR
                targetRawImage.material =MaterialBridgeMgr.instance.getMat(targetRawImage.material);
#endif
            }
        }
        
        public void setMaterialValue()
        {
            if(!startWithMaterial || null == targetRawImage)
                return;
            if (null == targetRawImage.material || string.IsNullOrEmpty(targetValueName))
                return;
            switch (targetValueType)
            {
                case EMaterialValueType.INT:
                    targetRawImage.material.SetInt(targetValueName, intValue);
                    break;
                case EMaterialValueType.FLOAT:
                    targetRawImage.material.SetFloat(targetValueName, floatValue);
                    break;
                case EMaterialValueType.COLOR:
                    targetRawImage.material.SetColor(targetValueName, colorValue);
                    break;
                case EMaterialValueType.VECTOR4:
                    targetRawImage.material.SetVector(targetValueName, vec4Value);
                    break;
            }
        }
    }
}
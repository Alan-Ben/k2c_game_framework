using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    
    /// <summary>
    /// Graphic材质桥,用于animation控制材质的shader参数
    /// </summary>
    public class GGUIMonoGraphicMatiralBridge: MonoBehaviour
    {
        [ALHeader("目标Graphic")]
        public Graphic targetGraphic;
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
            if (null != targetGraphic)
            {
#if UNITY_EDITOR
                targetGraphic.material =MaterialBridgeMgr.instance.getMat(targetGraphic.material);
#endif
            }
        }
        
        public void setMaterialValue()
        {
            if(!startWithMaterial || targetGraphic == null)
                return;
            Material mat = targetGraphic.material;
            if (null == mat || string.IsNullOrEmpty(targetValueName))
                return;
            switch (targetValueType)
            {
                case EMaterialValueType.INT:
                    mat.SetInt(targetValueName, intValue);
                    break;
                case EMaterialValueType.FLOAT:
                    mat.SetFloat(targetValueName, floatValue);
                    break;
                case EMaterialValueType.COLOR:
                    mat.SetColor(targetValueName, colorValue);
                    break;
                case EMaterialValueType.VECTOR4:
                    mat.SetVector(targetValueName, vec4Value);
                    break;
            }
        }
    }
}
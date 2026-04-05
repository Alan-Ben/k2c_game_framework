using UnityEngine;

namespace GOE
{
    public enum ENPOutLineType
    {
        NONE,
        MODEL, //3d模型
        SPRITE  //贴图
    }

    /// <summary>
    /// 描边配置
    /// </summary>
    [System.Serializable]
    public class OutLineSetting
    {
        [ALHeader("描边类型")]
        public ENPOutLineType outLineType;

        [ALHeader("贴图类型描边配置")]
        public OutLineSpriteSetting spriteSetting;
        
        [ALHeader("模型类型描边配置")]
        public OutLine3DSetting modelSetting;
    }

    /// <summary>
    /// 贴图类型的描边配置
    /// </summary>
    [System.Serializable]
    public class OutLineSpriteSetting
    {
        [ALHeader("sprite描边加载父节点，只在sprite类型下生效")] 
        public Transform spriteOutLineParent;
        
        [ALHeader("sprite描边的资源路径，只在sprite类型下生效")] 
        public NPGGoIndex spriteOutLineGoIndex;
    }
    
    /// <summary>
    /// 3d模型类型的描边配置
    /// </summary>
    [System.Serializable]
    public class OutLine3DSetting
    {
        [ALHeader("模型描边mesh,只在模型描边类型生效")]
        public SkinnedMeshRenderer skinnedMeshRenderer;
        
        [ALHeader("模型描边的宽度,只在模型描边类型生效")] 
        public float outLineWidth;

        [ALHeader("模型描边的颜色,只在模型描边类型生效")] 
        public Color outLineColor;
    }
}
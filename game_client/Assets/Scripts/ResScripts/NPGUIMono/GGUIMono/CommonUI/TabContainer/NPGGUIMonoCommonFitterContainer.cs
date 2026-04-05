using System.Collections.Generic;

namespace GOE
{

    /// <summary>
    /// 筛选类型结构
    /// </summary>
    [System.Serializable]
    public class NPGGUICommonFitterMono<T>
    {
        [ALHeader("筛选类型")]
        public T type;
        [ALHeader("对应翻译文本")]
        public string keyStr;
        [ALHeader("对应图片资源")]
        public NPGTextureIndex iconIndex;
    }

    /// <summary>
    /// 通用排序容器
    /// </summary>
    public class NPGGUIMonoCommonFitterContainer : _ATNPGGUIMonoShowAnimContainer<NPGGUIMonoCommonFitterTab>
    {
    }
}

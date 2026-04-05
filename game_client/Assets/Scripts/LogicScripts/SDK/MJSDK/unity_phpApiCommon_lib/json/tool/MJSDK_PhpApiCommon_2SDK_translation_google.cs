using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// Engine->SDK(translation-google:谷歌云翻译)消息结构体
    /// </summary>
    public class MJSDK_PhpApiCommon_2SDK_translation_google : MJSDK_2SDK_Base
    {
        //翻译内容文本
        public string content;
        //目标语言标识
        public string target;
        //翻译内容语言标识 支持语言查询链接： https://cloud.google.com/translate/docs/languages?hl=zh-cn
        public string source;
    }
}

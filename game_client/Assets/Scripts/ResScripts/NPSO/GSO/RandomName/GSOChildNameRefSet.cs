using ALPackage;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 子嗣名称表
    /// </summary>
    [System.Serializable]
    public class ChildNameRefObj : _IALBasicRefObj
    {
        public long _refId { get { return id; } }

        public long id;//唯一id
        public string boy_name;//男娃名称
        public string girl_name;//女娃名称
    }

    public class GSOChildNameRefSet : _TALSOBasicRefSet<ChildNameRefObj>
    {
        /************
     * 资源加载路径
     **/
        public static List<ENPLanguage> languages = new List<ENPLanguage>()
        {
            ENPLanguage.ZH_CN, //2简体中文
            ENPLanguage.EN_US, //英语(美国)
            ENPLanguage.ZH_TW, //3中国台湾
            ENPLanguage.RU_RU, //6俄语(俄罗斯)
            ENPLanguage.TR_TR, //8土耳其语
            ENPLanguage.KO_KR,    //韩语
            ENPLanguage.AR_AR,    //阿拉伯语
            ENPLanguage.JA_JP,    //日语
        };
        public static string objName { get { return "child_name"; } }
    }
}
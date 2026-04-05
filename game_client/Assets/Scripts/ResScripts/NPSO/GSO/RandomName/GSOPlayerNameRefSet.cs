using ALPackage;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 玩家名称表
    /// </summary>
    [System.Serializable]
    public class PlayerNameRefObj : _IALBasicRefObj
    {
        public long _refId { get { return id; } }

        public long id;//唯一id
        public string prefix_name;//称谓 (姓)
        public string suffix_name;//名称 (名)
    }

    public class GSOPlayerNameRefSet : _TALSOBasicRefSet<PlayerNameRefObj>
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
        public static string objName { get { return "player_name"; } }
    }
}
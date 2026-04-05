using ALPackage;
using System.Collections.Generic;
using System;
using CommonEnum;

namespace GOE
{

    [System.Serializable]
    public class NPRankRefObj : _IALBasicRefObj
    {
        public long _refId { get { return rank_id; } }

        public int rank_id;

        public string name; //排行榜名字
        public List<string> name_args; //排行榜名字参数
        public string score_name; //排行分数使用的key
        public ERankDetailShowType show_type; //展示类型(点击排行榜展示的具体数据)
        public NPEnum.ERankType rank_type; //排行榜类型
        public EValueFormatType process_num_format; //数据获取类型

        public string nameStr { get { return TextTranslate.instance.getLanguage(name, name_args); } }

    }


    /// <summary>
    /// 通用排行榜表
    /// </summary>
    public class NPSORankRefSet : _TALSOBasicRefSet<NPRankRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/rank_refdata.unity3d"; } }
        public static string objName { get { return "rank"; } }
    }
}


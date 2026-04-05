using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 成就阶段数据
    /// </summary>
    [System.Serializable]
    public class AchieveStepRefObj:_IALBasicRefObj
    {
        public long _refId { get { return id; } }
        public long id;//唯一识别ID
        public long achieve_id;//所属成就ID
        public int step;//活动阶段，静态表配置从1开始
        public long process_count;//进度条计数目标值
        public List<NPCommonCostItem> done_item_list;//完成成就奖励物品列表
        public string name;
        public List<string> name_args;

        public string getName { get => TextTranslate.instance.getLanguage(name, name_args); }
    }

    public class GSOAchieveStepRefSet : _TALSOBasicRefSet<AchieveStepRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/achieve_refdata.unity3d"; } }
        public static string objName { get { return "achieve_step"; } }
    }
}


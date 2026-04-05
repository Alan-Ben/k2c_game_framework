using System.Collections.Generic;
using ALPackage;
using CommonEnum;

namespace GOE
{
    /// <summary>
    /// 成就点数数据
    /// </summary>
    [System.Serializable]
    public class AchievePointStepRefObj:_IALBasicRefObj
    {
        public long _refId { get { return id; } }
        public long id;//唯一id
        public long step_id;//阶段id
        public EAchieveType type;//成就类型
        public NPCommonCostItem need_point;//需要的成就点数
        public List<NPCommonCostItem> reward_item_list;//奖励物品列表
    }

    public class GSOAchievePointStepRefSet : _TALSOBasicRefSet<AchievePointStepRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/achieve_refdata.unity3d"; } }
        public static string objName { get { return "achieve_point_step"; } }
    }
}


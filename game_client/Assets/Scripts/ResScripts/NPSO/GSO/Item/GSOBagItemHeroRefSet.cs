using System.Collections.Generic;
using ALPackage;
using Common.BagItemUseEnum;
using CommonEnum;

namespace GOE
{
    /// <summary>
    /// 骑士物品表
    /// </summary>
    [System.Serializable]
    public class BagItemHeroRefObj : _IALBasicRefObj
    {
        public long _refId { get { return id; } }
        public long id; //物品ID
        public EBagItemUse_HeroType show_type;//展示类型
        public EBagItemUse_TargetType select_type;//选择对象类型
        public ESpecAttrType attr_type;//目标特长属性类型
        public _NPPlayerVariableSerializeInfo value;//值（高级公式）
        public List<long> show_value_list;//客户端展示加成值列表
    }

    /// <summary>
    /// 骑士物品表
    /// </summary>
    public class GSOBagItemHeroRefSet : _TALSOBasicRefSet<BagItemHeroRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/bag_refdata.unity3d"; } }
        public static string objName { get { return "bag_item_hero"; } }
    }
}


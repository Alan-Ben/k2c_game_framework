using System.Collections.Generic;
using ALPackage;
using Common.BagItemUseEnum;

namespace GOE
{
    /// <summary>
    /// 情人物品表
    /// </summary>
    [System.Serializable]
    public class BagItemConsortRefObj : _IALBasicRefObj
    {
        public long _refId { get { return id; } }
        public long id; //物品ID
        public EBagItemUse_ConsortType show_type;//展示类型
        public EBagItemUse_TargetType select_type;//选择对象类型
        public bool is_show_in_consort_detail;//是否在妃子详情赠送展示
        public NPGTextureIndex gain_item_icon;//使用后获得的对应图标
        public string consort_detail_give_desc;//妃子详情赠送显示描述
        public List<string> consort_detail_give_desc_args;//妃子详情赠送显示描述参数
        public _NPPlayerVariableSerializeInfo value;//值（高级公式）
        public List<long> show_value_list;//客户端展示加成值列表

        /// <summary>
        /// 是否是妃子详情页面可赠送的物品
        /// </summary>
        /// <returns></returns>
        public bool isConsortDetailSendGiftBagItem()
        {
            return is_show_in_consort_detail &&
                   (show_type == EBagItemUse_ConsortType.INTIMACY || show_type == EBagItemUse_ConsortType.CHARM) &&
                   select_type == EBagItemUse_TargetType.SELECT;
        }
    }

    /// <summary>
    /// 骑士物品表
    /// </summary>
    public class GSOBagItemConsortRefSet : _TALSOBasicRefSet<BagItemConsortRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/bag_refdata.unity3d"; } }
        public static string objName { get { return "bag_item_consort"; } }
    }
}


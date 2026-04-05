using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 礼包组表
	/// </summary>
	[Serializable]
	public class GiftPackGroupRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
        public List<long> gift_pack_id_list;//礼包id列表
        public _NPPlayerConditionSerializeInfo show_condition;//显示条件
        public EGiftPackGroupShowType show_type;//分组展示类型
        public long activity_id;//活动id
        public string name;//礼包组名称
        public string tab_name;//页签名称
        public string activity_tab_name;//活动里礼包页签名称
        public NPGTextureIndex tex_banner;//横幅图片
        public string refresh_desc;//限购次数刷新描述
        public long page_ui_res_id;//礼包组页面资源id
        public long title_ui_res_id;//礼包组标题资源id

        /// <summary>
        /// 是否可以显示
        /// </summary>
        public bool canShow { get { return show_condition == null || show_condition.isEmpty || show_condition.IsEnable(null); } }
    }

	public class GSOGiftPackGroupRefSet : _TALSOBasicRefSet<GiftPackGroupRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/gift_pack_refdata.unity3d"; } }
		public static string objName { get { return "gift_pack_group"; } }
	}
}
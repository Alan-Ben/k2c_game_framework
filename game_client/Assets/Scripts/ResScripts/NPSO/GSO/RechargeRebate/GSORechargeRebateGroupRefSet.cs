using ALPackage;
using Common.RechargeRebateEnum;
using System;

namespace GOE
{
	/// <summary>
	/// 充值返利组表
	/// </summary>
	[Serializable]
	public class RechargeRebateGroupRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		public ERechargeRebateType type;//类型
		public NPGTextureIndex tex_banner;//banner图片
		public long mail_id;//邮件id
		public string name;//组名称
		public string tab_name;//页签名称
		public string count_desc;//计数描述
		public string desc;//描述
		public long title_ui_res_id;//标题资源id
    }

	public class GSORechargeRebateGroupRefSet : _TALSOBasicRefSet<RechargeRebateGroupRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/recharge_rebate_refdata.unity3d"; } }
		public static string objName { get { return "recharge_rebate_group"; } }
	}
}
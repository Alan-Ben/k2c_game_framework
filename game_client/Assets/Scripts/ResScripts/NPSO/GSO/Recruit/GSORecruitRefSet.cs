using ALPackage;
using System;
using System.Collections.Generic;
using NPEnum;

namespace GOE
{
	public enum ERecruitItemType
	{
		NORMAL,
		HERO,
		CONSORT,
	}
	
	/// <summary>
	/// 招募表
	/// </summary>
	[Serializable]
	public class RecruitRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		
		public long shop_id;//商店id
		public NPCommonCostItem cost_item;//兑换消耗
		public NPCommonCostItem gain_item;//获得物品
		public _NPPlayerConditionSerializeInfo condition;//兑换条件
		public string condition_desc;//兑换条件描述
		public List<string> condition_desc_args_list;//兑换条件描述参数列表
		public int sort_id;//排序id
		public string exchange_second_check_content;//兑换二次确认弹窗内容
		public _NPPlayerConditionSerializeInfo can_goto_recruit_condition;//可前往招募的条件
		public string can_goto_recruit_desc;//可前往招募的描述
		public _NPPlayerEffectSerializeInfo can_goto_recruit_goto_effect;//可前往招募的前往效果
		
		/// <summary>
		/// 是否可前往招募
		/// </summary>
		/// <returns></returns>
		public bool canGotoRecruit()
		{
			if (can_goto_recruit_condition != null && can_goto_recruit_condition.hasCondition)
			{
				return can_goto_recruit_condition.IsEnable(null);
			}

			return false;
		}
	}

	public class GSORecruitRefSet : _TALSOBasicRefSet<RecruitRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/recruit_refdata.unity3d"; } }
		public static string objName { get { return "recruit"; } }
	}
}
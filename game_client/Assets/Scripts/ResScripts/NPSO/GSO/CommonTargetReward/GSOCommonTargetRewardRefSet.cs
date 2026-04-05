using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 通用目标奖励表
	/// </summary>
	[Serializable]
	public class CommonTargetRewardRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		public List<NPCommonCostItem> gain_item_list;//奖励物品列表
		public long process_count;//进度条计数目标值
		public _NPPlayerVariableSerializeInfo process_cur_count;//计数器的高级公式(ENPPlayerVariableType)
		public _NPPlayerConditionSerializeInfo show_condition; //显示条件
		public string target_txt;//目标描述
		public _NPPlayerEffectSerializeInfo go_to;//跳转效果
		public long red_tip_id;//红点id
	}

	public class GSOCommonTargetRewardRefSet : _TALSOBasicRefSet<CommonTargetRewardRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
		public static string objName { get { return "common_target_reward"; } }
	}
}
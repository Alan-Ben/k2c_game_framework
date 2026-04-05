using ALPackage;
using System;
using System.Collections.Generic;
using NPEnum;
using UnityEngine;

namespace GOE
{
	/// <summary>
	/// 联盟杂物委托品质表
	/// </summary>
	[Serializable]
	public class GuildRandomEntrustQualityRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		
		public EQuality quality;//品质
		public long count;//达成所需进度
		public List<NPCommonCostItem> reawrd_item_list;//奖励列表
		public long gain_currency_per;//每次委托处理的金币收益万分比
        public string quality_name;//品质名称
        public Color name_color;//名称颜色
    }

	public class GSOGuildRandomEntrustQualityRefSet : _TALSOBasicRefSet<GuildRandomEntrustQualityRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/guild_refdata.unity3d"; } }
		public static string objName { get { return "guild_random_entrust_quality"; } }
	}
}
using ALPackage;
using System;
using System.Collections.Generic;
using Common.DungeonEnum;

namespace GOE
{
	/// <summary>
	/// 午间副本宝箱
	/// </summary>
	[Serializable]
	public class MiddayDungeonBoxRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		public List<NPCommonCostItem> item_list;//领取时的奖励
		public NPGTextureIndex box_icon;//图标
		public NPGTextureIndex box_banner; // 聊天banner
		public EDungeonBoxType box_type;//宝箱类型
		public int can_draw_limit;//宝箱可被领取次数
		public long duration_sec;//宝箱有效期（秒）
		public long draw_box_fixed_cd_id;//领取宝箱次数上限（fixedCdId）
	}

	public class GSOMiddayDungeonBoxRefSet : _TALSOBasicRefSet<MiddayDungeonBoxRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/midday_dungeon_box_refdata.unity3d"; } }
		public static string objName { get { return "midday_dungeon_box"; } }
	}
}
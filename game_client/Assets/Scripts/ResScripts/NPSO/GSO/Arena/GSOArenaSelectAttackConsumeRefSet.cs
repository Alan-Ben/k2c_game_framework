using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 竞技场指定谈判道具表
	/// </summary>
	[Serializable]
	public class ArenaSelectAttackConsumeRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
        public EArenaSelectAttackConsumeItemType type;//道具类型(普通、高级、特级)
		public NPCommonCostItem cost;//消耗
        public long ratio;//额外获得影响力和商会硬币倍数
	}

	public class GSOArenaSelectAttackConsumeRefSet : _TALSOBasicRefSet<ArenaSelectAttackConsumeRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/arena_refdata.unity3d"; } }
		public static string objName { get { return "arena_select_attack_consume"; } }
	}
}
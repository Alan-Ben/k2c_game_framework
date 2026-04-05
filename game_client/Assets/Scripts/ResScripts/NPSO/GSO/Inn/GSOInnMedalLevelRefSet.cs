using ALPackage;
using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace GOE
{
	/// <summary>
	/// 旅店奖牌等级表
	/// </summary>
	[Serializable]
	public class InnMedalLevelRefObj : _IALBasicRefObj
	{
		public long _refId { get { return level; } }
		public long level;
		public int need_inn_level; // 所需旅店等级
		public PlayerBonusPropertyModifier bonus_prop_modifier;
	}

	public class GSOInnMedalLevelRefSet : _TALSOBasicRefSet<InnMedalLevelRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/inn_refdata.unity3d"; } }
		public static string objName { get { return "inn_medal_level"; } }
	}
}
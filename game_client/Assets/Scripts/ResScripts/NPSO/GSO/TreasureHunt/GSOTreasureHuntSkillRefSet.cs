using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 太空寻宝 - 技能表
	/// </summary>
	[Serializable]
	public class TreasureHuntSkillRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;//技能ID
		
		public string name;//技能名称
		public string desc;//技能描述
		public NPGSpriteIndex icon;//技能图标
	}

	public class GSOTreasureHuntSkillRefSet : _TALSOBasicRefSet<TreasureHuntSkillRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/treasure_hunt_refdata.unity3d"; } }
		public static string objName { get { return "treasure_hunt_skill"; } }
	}
}
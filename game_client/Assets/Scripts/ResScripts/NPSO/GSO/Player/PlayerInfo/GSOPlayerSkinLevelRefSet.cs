using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 玩家皮肤等级表
	/// </summary>
	[Serializable]
	public class PlayerSkinLevelRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
        public long player_skin_id;//皮肤id
        public long skin_level;//皮肤等级
        public NPCommonCostItem upgrade_cost;//升级到下级道具（CommonCostItem）
        public NPPlayerPropertyModifier player_property;//皮肤该等级的属性
    }

	public class GSOPlayerSkinLevelRefSet : _TALSOBasicRefSet<PlayerSkinLevelRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/player_refdata.unity3d"; } }
		public static string objName { get { return "player_skin_level"; } }
	}
}
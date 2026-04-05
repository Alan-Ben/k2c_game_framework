using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 创角预设表
	/// </summary>
	[Serializable]
	public class PlayerCreatPlayerPrefabRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		public long skin_id; // 皮肤id
		public long icon_id; // 头像id
	}

	public class GSOPlayerCreatPlayerPrefabRefSet : _TALSOBasicRefSet<PlayerCreatPlayerPrefabRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/player_refdata.unity3d"; } }
		public static string objName { get { return "player_create_player_prefab"; } }
	}
}
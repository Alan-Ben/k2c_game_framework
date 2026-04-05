using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 玩家权限表
	/// </summary>
	[Serializable]
	public class PlayerPermissionsRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
	}

	public class GSOPlayerPermissionsRefSet : _TALSOBasicRefSet<PlayerPermissionsRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/player_permissions_refdata.unity3d"; } }
		public static string objName { get { return "player_permissions"; } }
	}
}
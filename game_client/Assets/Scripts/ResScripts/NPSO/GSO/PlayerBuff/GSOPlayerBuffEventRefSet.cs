using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 玩家buff事件表
	/// </summary>
	[Serializable]
	public class PlayerBuffEventRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
	}

	public class GSOPlayerBuffEventRefSet : _TALSOBasicRefSet<PlayerBuffEventRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/player_buff_refdata.unity3d"; } }
		public static string objName { get { return "player_buff_event"; } }
	}
}
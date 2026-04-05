using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 玩家组合称号前缀表
	/// </summary>
	[Serializable]
	public class PlayerTitlePrefixRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
        public _NPPlayerConditionSerializeInfo unlock_condition;//解锁条件
        public List<string> add_msg_type_list;//客户端需要监听的枚举数组{WinMsgType}
    }

	public class GSOPlayerTitlePrefixRefSet : _TALSOBasicRefSet<PlayerTitlePrefixRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/player_refdata.unity3d"; } }
		public static string objName { get { return "player_title_prefix"; } }
	}
}
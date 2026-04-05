using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 玩家限时称号分组表
	/// </summary>
	[Serializable]
	public class PlayerTitleLimitGroupRefObj : _IALBasicRefObj
	{
		public long _refId { get { return group_id; } }
		public long group_id;
        public string desc;//描述
        public List<string> desc_args;//描述参数
    }

	public class GSOPlayerTitleLimitGroupRefSet : _TALSOBasicRefSet<PlayerTitleLimitGroupRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/player_refdata.unity3d"; } }
		public static string objName { get { return "player_title_limit_group"; } }
	}
}
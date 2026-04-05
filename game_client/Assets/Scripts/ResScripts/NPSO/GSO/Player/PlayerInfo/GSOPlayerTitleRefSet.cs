using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 玩家称号表
	/// </summary>
	[Serializable]
	public class PlayerTitleRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
        public EPlayerTitleTabType show_type;//展示类型（固定、限时）
        public long limit_group_id;//限时类型分组id（展示从大到小）
        public List<WCGPairInt> asset_path_id_list;//获得次数对应prefab路径
        public long expire_time_sec;//过期时间（秒）
        public NPGTextureIndex icon; //称号图标
	}

	public class GSOPlayerTitleRefSet : _TALSOBasicRefSet<PlayerTitleRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/player_refdata.unity3d"; } }
		public static string objName { get { return "player_title"; } }
	}
}
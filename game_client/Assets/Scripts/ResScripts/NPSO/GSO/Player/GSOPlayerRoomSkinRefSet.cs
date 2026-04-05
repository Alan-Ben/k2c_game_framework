using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 玩家卧室皮肤表
	/// </summary>
	[Serializable]
	public class PlayerRoomSkinRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;

		public NPGGoIndex building_index;// 建筑资源GoIndex
		public long room_scene_id;// 卧室场景id
		public NPGTextureIndex card_img;// 卡片图片
		public NPGTextureIndex bg_img;// 背景图片
		public NPGGoIndex travel_pos_res_go_index;// 游历地点资源GoIndex
	}

	public class GSOPlayerRoomSkinRefSet : _TALSOBasicRefSet<PlayerRoomSkinRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/player_refdata.unity3d"; } }
		public static string objName { get { return "player_room_skin"; } }
	}
}

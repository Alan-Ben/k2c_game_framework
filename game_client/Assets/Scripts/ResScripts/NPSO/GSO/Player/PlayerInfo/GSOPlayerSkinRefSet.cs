using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 玩家皮肤表
	/// </summary>
	[Serializable]
	public class PlayerSkinRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
        public bool is_hide;//皮肤是否隐藏
		public long sort_id;//排序id(小的排前面)
        public NPCommonCostItem unlock_item;//解锁道具（CommonCostItem）
        public NPGGoIndex td_show;//全身形象
        public NPGTextureIndex card_image;//卡牌半身像
    }

	public class GSOPlayerSkinRefSet : _TALSOBasicRefSet<PlayerSkinRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/player_refdata.unity3d"; } }
		public static string objName { get { return "player_skin"; } }
	}
}
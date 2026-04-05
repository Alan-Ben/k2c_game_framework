using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 午间副本Boss波次表
	/// </summary>
	[Serializable]
	public class MiddayDungeonWaveRefObj : _IALBasicRefObj
	{
		public long _refId { get { return wave; } }
		public long wave;//波次
		public long boss_blood;//boss血量
		public int dungeon_coin;//获得副本积分
		public long hero_exp_reward_ratio;//大臣经验奖励倍数（万分比）
		public long box_drop_per;//宝箱掉落概率（万分比）
		public long box_id;//宝箱id
		public string boss_name;//boss名字
        public NPGGoIndex boss_go_index;//boss 场景 形象
        public GVideoClipIndex videoIndex = new GVideoClipIndex(2202, 1); // 未完成视频
        public GVideoClipIndex finishVideoIndex = new GVideoClipIndex(2203, 1); // 完成视频

	}

	public class GSOMiddayDungeonWaveRefSet : _TALSOBasicRefSet<MiddayDungeonWaveRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/midday_dungeon_refdata.unity3d"; } }
		public static string objName { get { return "midday_dungeon_wave"; } }
	}
}
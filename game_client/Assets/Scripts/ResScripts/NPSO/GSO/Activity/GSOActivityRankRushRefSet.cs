using ALPackage;
using System;
using System.Collections.Generic;
using NPEnum;

namespace GOE
{
	/// <summary>
	/// 活动限时冲榜表
	/// </summary>
	[Serializable]
	public class ActivityRankRushRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
        public long rank_id;//排行榜id
        public NPGTextureIndex banner_tex;//banner图
        public List<long> access_id_list;//获取途径id列表
        public string ranking_tip;//排名说明描述
        public _NPPlayerVariableSerializeInfo max_record_value;//历史最高值读取的高级公式
        public string initial_value_desc;//冲榜初始值描述
		public bool is_show_in_rank_rush_wnd;//是否展示在冲榜活动界面
        public long reward_red_tip_id;//排行奖励红点id（没展示在冲榜活动界面的才需要）
        public bool is_show_ranking_change_tip;//是否展示排名变动提示

        /// <summary>
        /// 是否有展示历史最高值
        /// </summary>
        public bool haveMaxRecordValue { get { return max_record_value != null && !string.IsNullOrEmpty(max_record_value.s_variable); } }
    }

	public class GSOActivityRankRushRefSet : _TALSOBasicRefSet<ActivityRankRushRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/activity_refdata.unity3d"; } }
		public static string objName { get { return "activity_rank_rush"; } }
	}
}
using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 火星航行表
	/// </summary>
	[Serializable]
	public class MarsGoRouteRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
        public long stage_id;//节点id
        public long continue_secs;//持续时间（秒）
        public List<NPCommonCostItem> arrive_item_list;//到达奖励物品列表
        public NPGGoIndex bg_index;//背景视频资源
        public NPGTextureIndex banner_tex;//当前节点banner图
        public string msg_stage_name;//火星留言用的节点名称
        public string name;//当前节点名称
        public string desc;//当前节点描述
		public string desc_args;//当前节点描述参数
        public string arrive_desc;//抵达当前节点描述
		public string arrive_desc_args;//抵达当前节点描述参数
		public string arrive_confirm_btn_desc;//抵达当前节点确认按钮描述
        public long arrive_dialogue_id;//抵达当前节点对话id
        public List<long> log_id_list;//航行日志id列表，会根据航行总时长计算平均显示间隔时间逐个展示
        public long done_simple_unlock_id;//完成条件simpleUnlockId
    }

	public class GSOMarsGoRouteRefSet : _TALSOBasicRefSet<MarsGoRouteRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/mars_refdata.unity3d"; } }
		public static string objName { get { return "mars_go_route"; } }
	}
}
using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 倒计时事件表
	/// </summary>
	[Serializable]
	public class CountdownEventRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
        public string name;//名称
        public int can_trigger_num;//可以触发次数
        public bool undone_need_reset;//超时是否重置
        public long quest_id;//任务id
        public long duration;//CD时长(秒)
        public string desc;//描述
        public List<string> desc_args;//描述参数
        public NPGTextureIndex entrance_tex;//入口贴图
        public long done_dialogue_id;//事件完成对话id
        public long undone_dialogue_id;//事件超时未完成对话id
        public long trigger_dialogue_id;//事件触发时对话id
        public long ui_res_id;//弹窗界面资源id
    }

	public class GSOCountdownEventRefSet : _TALSOBasicRefSet<CountdownEventRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/countdown_event_refdata.unity3d"; } }
		public static string objName { get { return "countdown_event"; } }
	}
}
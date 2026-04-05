using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 关卡 节剧情 配表数据
	/// </summary>
	[Serializable]
	public class ChapterStagePlotRefObj : _IALBasicRefObj
	{
		public long _refId { get { return plot_id; } }
		public long plot_id;// 节id
		public string name;// 剧情名称
		public List<string> name_args;// 剧情名称参数
		public long dialog_id; // 对话id
		public _NPPlayerConditionSerializeInfo unlock_condition; // 解锁条件
	}

	public class GSOChapterStagePlotRefSet : _TALSOBasicRefSet<ChapterStagePlotRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/chapter_refdata.unity3d"; } }
		public static string objName { get { return "chapter_stage_plot"; } }
	}
}
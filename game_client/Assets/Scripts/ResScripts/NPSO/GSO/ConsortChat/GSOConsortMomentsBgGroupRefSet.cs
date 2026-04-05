using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 妃子朋友圈背景组表
	/// </summary>
	[Serializable]
	public class ConsortMomentsBgGroupRefObj : _IALBasicRefObj
	{
		public long _refId { get { return bg_group_id; } }
		public long bg_group_id;//背景图片组id
		public List<long> bg_img_id_list;// 图片背景id
		public string bg_prompt_key;// 背景主题描述翻译key
	}

	public class GSOConsortMomentsBgGroupRefSet : _TALSOBasicRefSet<ConsortMomentsBgGroupRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/consort_chat_refdata.unity3d"; } }
		public static string objName { get { return "consort_moments_bg_group"; } }
	}
}
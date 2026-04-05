using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 妃子朋友圈妃子图片组表
	/// </summary>
	[Serializable]
	public class ConsortMomentsConsortGroupRefObj : _IALBasicRefObj
	{
		public long _refId { get { return consort_img_group_id; } }
		public long consort_img_group_id;//背景图片组id
		public List<long> consort_img_id_list;// 妃子图片id
		public string consort_prompt_key;// 妃子描述翻译key
	}

	public class GSOConsortMomentsConsortGroupRefSet : _TALSOBasicRefSet<ConsortMomentsConsortGroupRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/consort_chat_refdata.unity3d"; } }
		public static string objName { get { return "consort_moments_consort_group"; } }
	}
}
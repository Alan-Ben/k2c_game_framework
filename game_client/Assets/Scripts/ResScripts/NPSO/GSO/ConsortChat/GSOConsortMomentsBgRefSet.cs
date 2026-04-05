using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 妃子朋友圈背景图片表
	/// </summary>
	[Serializable]
	public class ConsortMomentsBgRefObj : _IALBasicRefObj
	{
		public long _refId { get { return bg_id; } }
		public long bg_id;//妃子图片id
		public NPGTextureIndex img_index;// 背景图
		public NPGGoIndex prefab_index;
	}

	public class GSOConsortMomentsBgRefSet : _TALSOBasicRefSet<ConsortMomentsBgRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/consort_chat_refdata.unity3d"; } }
		public static string objName { get { return "consort_moments_bg"; } }
	}
}
using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 妃子朋友圈妃子图片表
	/// </summary>
	[Serializable]
	public class ConsortMomentsConsortRefObj : _IALBasicRefObj
	{
		public long _refId { get { return consort_img_id; } }
		public long consort_img_id;//妃子图片id
		public NPGTextureIndex img_index;//妃子图
		public NPGGoIndex prefab_index;
	}

	public class GSOConsortMomentsConsortRefSet : _TALSOBasicRefSet<ConsortMomentsConsortRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/consort_chat_refdata.unity3d"; } }
		public static string objName { get { return "consort_moments_consort"; } }
	}
}
using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 妃子朋友圈照片表
	/// </summary>
	[Serializable]
	public class ConsortChatImageGroupRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		public EConsortChatShotType shot_type;
		public long bg_img_id;//背景id
		public long consort_img_id;//人物图片id
	}

	public class GSOConsortChatImageGroupRefSet : _TALSOBasicRefSet<ConsortChatImageGroupRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/consort_chat_refdata.unity3d"; } }
		public static string objName { get { return "consort_chat_image_group"; } }
	}
}
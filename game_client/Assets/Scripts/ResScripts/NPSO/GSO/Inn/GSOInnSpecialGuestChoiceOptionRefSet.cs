using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 特殊客人选择选项表
	/// </summary>
	[Serializable]
	public class InnSpecialGuestChoiceOptionRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		public NPGTextureIndex icon;
		public string option_desc;
	}

	public class GSOInnSpecialGuestChoiceOptionRefSet : _TALSOBasicRefSet<InnSpecialGuestChoiceOptionRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/inn_refdata.unity3d"; } }
		public static string objName { get { return "inn_special_guest_choice_option"; } }
	}
}

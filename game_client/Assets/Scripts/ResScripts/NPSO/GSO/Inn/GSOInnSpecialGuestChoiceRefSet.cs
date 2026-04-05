using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 特殊客人选择表
	/// </summary>
	[Serializable]
	public class InnSpecialGuestChoiceRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		public string question_desc;
		public List<long> option_id_list;
	}

	public class GSOInnSpecialGuestChoiceRefSet : _TALSOBasicRefSet<InnSpecialGuestChoiceRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/inn_refdata.unity3d"; } }
		public static string objName { get { return "inn_special_guest_choice"; } }
	}
}

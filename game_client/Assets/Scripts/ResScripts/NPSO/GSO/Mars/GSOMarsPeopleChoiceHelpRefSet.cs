using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 火星居民选择求助表
	/// </summary>
	[Serializable]
	public class MarsPeopleChoiceHelpRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;

		public List<string> option_desc_list;//选项描述列表
		public List<string> option_result_desc_list;//选项结果描述列表
		public List<int> option_add_satisfaction_degree_list;//选项增加满意度列表
		public List<long> option_reward_id_list;//选项获取奖励列表
	}

	public class GSOMarsPeopleChoiceHelpRefSet : _TALSOBasicRefSet<MarsPeopleChoiceHelpRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/mars_refdata.unity3d"; } }
		public static string objName { get { return "mars_people_choice_help"; } }
	}
}

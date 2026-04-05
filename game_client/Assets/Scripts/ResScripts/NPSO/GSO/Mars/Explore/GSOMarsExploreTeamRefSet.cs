using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 火星探索队伍表
	/// </summary>
	[Serializable]
	public class MarsExploreTeamRefObj : _IALBasicRefObj
	{
		public long _refId { get { return team_id; } }
		public long team_id;
		public string team_name;
		public _NPPlayerConditionSerializeInfo show_cond;
		public _NPPlayerConditionSerializeInfo unlock_cond;
		public string unlock_cond_desc;
		public List<string> unlock_cond_param_list;
		public _NPPlayerEffectSerializeInfo unlock_jump;
	}

	public class GSOMarsExploreTeamRefSet : _TALSOBasicRefSet<MarsExploreTeamRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/mars_refdata.unity3d"; } }
		public static string objName { get { return "mars_explore_team"; } }
	}
}

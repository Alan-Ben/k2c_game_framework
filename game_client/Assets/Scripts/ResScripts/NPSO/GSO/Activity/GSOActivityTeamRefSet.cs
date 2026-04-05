using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 活动组队表
	/// </summary>
	[Serializable]
	public class ActivityTeamRefObj : _IALBasicRefObj
	{
		public long _refId { get { return activity_id; } }
		public long activity_id;
		public int member_limit;//队伍人数上限（包括队长）
	}

	public class GSOActivityTeamRefSet : _TALSOBasicRefSet<ActivityTeamRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/activity_refdata.unity3d"; } }
		public static string objName { get { return "activity_team"; } }
	}
}
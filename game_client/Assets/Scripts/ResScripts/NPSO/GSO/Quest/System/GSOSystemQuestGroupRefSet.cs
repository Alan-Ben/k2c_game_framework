using ALPackage;
using System;
using Common.QuestEnum;

namespace GOE
{
	/// <summary>
	/// 系统任务组表
	/// </summary>
	[Serializable]
	public class SystemQuestGroupRefObj : _IALBasicRefObj
	{
		public long _refId { get { return group_id; } }
		public long group_id;//任务组id
        public EDailyQuestType daily_quest_type;//每日任务类型
        public long red_tip_id;//红点id
    }

	public class GSOSystemQuestGroupRefSet : _TALSOBasicRefSet<SystemQuestGroupRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/system_quest_refdata.unity3d"; } }
		public static string objName { get { return "system_quest_group"; } }
	}
}
using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 活动换皮表
	/// </summary>
	[Serializable]
	public class ActivityPrefabSkinRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
        public long activity_id;//活动id
        public string key;//预制体资源key
        public long ui_res_id;//换皮资源id
    }

	public class GSOActivityPrefabSkinRefSet : _TALSOBasicRefSet<ActivityPrefabSkinRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/activity_refdata.unity3d"; } }
		public static string objName { get { return "activity_prefab_skin"; } }
	}
}
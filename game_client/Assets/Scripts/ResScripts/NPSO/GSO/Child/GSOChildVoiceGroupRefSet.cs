using ALPackage;
using CommonEnum;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 子嗣配音组表
	/// </summary>
	[Serializable]
	public class ChildVoiceGroupRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
        public EChildSexType sex;//性别
        public EChildVoiceType voice_type;//配音类型
        public List<long> voice_id_list;//配音id列表
        public _NPPlayerConditionSerializeInfo unlock_condition;//解锁条件
    }

	public class GSOChildVoiceGroupRefSet : _TALSOBasicRefSet<ChildVoiceGroupRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/child_refdata.unity3d"; } }
		public static string objName { get { return "child_voice_group"; } }
	}
}
using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 火星建筑建造条件表
	/// </summary>
	[Serializable]
	public class MarsBuildingConditionRefObj : _IALBasicRefObj
#if NP_GAME
		, _IConditionDescShow
#endif
	{
		public long _refId { get { return id; } }
		public long id;
		public NPGTextureIndex icon;
		public _NPPlayerConditionSerializeInfo condition;
		public string desc;
		public List<string> desc_param_list;
		public _NPPlayerEffectSerializeInfo jump_effect;
		
#if NP_GAME
		public NPGTextureIndex conditionIcon
		{
			get { return icon; }
		}
		public string conditionDesc
		{
			get { return TextTranslate.instance.getLanguage(desc, desc_param_list); }
		}
		public bool conditionIsEnable
		{
			get { return condition == null || condition.IsEnable(null); }
		}
		public void jumpFunc()
		{
			jump_effect?.dealEffect();
		}
#endif
	}

	public class GSOMarsBuildingConditionRefSet : _TALSOBasicRefSet<MarsBuildingConditionRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/mars_refdata.unity3d"; } }
		public static string objName { get { return "mars_building_condition"; } }
	}
}

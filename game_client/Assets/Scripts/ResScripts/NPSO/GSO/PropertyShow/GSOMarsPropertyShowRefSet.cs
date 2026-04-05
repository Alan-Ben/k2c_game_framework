using ALPackage;
using System;
using Common.MarsEnum;
using NPEnum;

namespace GOE
{
	/// <summary>
	/// 火星属性展示表
	/// </summary>
	[Serializable]
	public class MarsPropertyShowRefObj : _IALBasicRefObj, _IEnumPropertyShow
	{
		public long _refId { get { return (long)type; } }
		public EMarsPropertyType type;

		public bool is_add_per;//是否万分比加成
		public string simple_name;//简易名称
		
		public Enum propertyEnum { get { return type; } }
		public bool isAddPer { get { return is_add_per; } }
		public string simpleName { get { return TextTranslate.instance.getLanguage(simple_name); } }
	}

	public class GSOMarsPropertyShowRefSet : _TALSOBasicRefSet<MarsPropertyShowRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/property_show_refdata.unity3d"; } }
		public static string objName { get { return "mars_property_show"; } }
	}
}
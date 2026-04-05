using ALPackage;
using System;
using System.Collections.Generic;
using Common.MarsEnum;
using NPEnum;

namespace GOE
{
	/// <summary>
	/// 火星科技类型表
	/// </summary>
	[Serializable]
	public class MarsTechnologyTypeRefObj : _IALBasicRefObj
	{
		public long _refId { get { return (long)type; } }
		public EMarsTechnologyType type;//科技类型
		
		public string name;//科技类型名称
		public List<EMarsPropertyType> mars_property_type;//火星属性类型列表
		public List<ENPPlayerPropertyType> player_property_type;//玩家属性类型列表
	}

	public class GSOMarsTechnologyTypeRefSet : _TALSOBasicRefSet<MarsTechnologyTypeRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/mars_refdata.unity3d"; } }
		public static string objName { get { return "mars_technology_type"; } }
	}
}

using ALPackage;
using System;
using NPEnum;

namespace GOE
{
	/// <summary>
	/// 博物馆表
	/// </summary>
	[Serializable]
	public class MuseumItemRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		public long upgrade_cost_group_id;
		public string name;
		public string desc;
		public NPGTextureIndex icon;
		public NPGTextureIndex skill_icon;
		public EQuality quality;

		[NonSerialized, ALAutoExportVariableAttr(true, true, true)]
		public NPQualityRefObj quality_ref;
		[NonSerialized, ALAutoExportVariableAttr(true, true, true)]
		public NPQualityExtRefObj quality_ext_ref;
	}

	public class GSOMuseumItemRefSet : _TALSOBasicRefSet<MuseumItemRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/museum_refdata.unity3d"; } }
		public static string objName { get { return "museum_item"; } }
	}
}
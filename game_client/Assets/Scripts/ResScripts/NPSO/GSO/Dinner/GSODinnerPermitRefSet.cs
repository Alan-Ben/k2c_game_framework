using ALPackage;
using System;
using Common.DinnerEnum;

namespace GOE
{
	/// <summary>
	/// 宴会凭证表
	/// </summary>
	[Serializable]
	public class DinnerPermitRefObj : _IALBasicRefObj
	{
		public long _refId { get { return (long)permit_type; } }

		public EDinnerPermitType permit_type;//凭证类型(EDinnerPermitType)
		public long dinner_id;//宴会id
		public long lifeTs;//有效期
		public string permit_desc;//凭证描述
		public NPGTextureIndex permit_icon;// 凭证图标
	}

	public class GSODinnerPermitRefSet : _TALSOBasicRefSet<DinnerPermitRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/dinner_refdata.unity3d"; } }
		public static string objName { get { return "dinner_permit"; } }
	}
}
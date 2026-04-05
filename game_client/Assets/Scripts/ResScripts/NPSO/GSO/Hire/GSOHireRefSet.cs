using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 招聘体验表
	/// </summary>
	[Serializable]
	public class HireRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
        public string name;//名称
        public int age;//年龄
        public NPGTextureIndex tex_character;//人物形象
        public string introduce;//介绍
        public List<string> tag_list;//标签列表
    }

	public class GSOHireRefSet : _TALSOBasicRefSet<HireRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/hire_refdata.unity3d"; } }
		public static string objName { get { return "hire"; } }
	}
}
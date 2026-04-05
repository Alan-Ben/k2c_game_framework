using ALPackage;
using System;
using UnityEngine;

namespace GOE
{
	/// <summary>
	/// 火星探索矿点表
	/// </summary>
	[Serializable]
	public class MarsExploreMineRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		public int mine_lvl;
		public string name;
		public string desc;
		public NPCommonItem res_type;
		public long res_type_refresh_wei;
		public long res_num;
		public long mars_mine_collects_speed;
		public NPGGoIndex scene_go_index;
		public NPGTextureIndex icon;
		public NPGTextureIndex banner;
		public Color mine_ui_color;
	}

	public class GSOMarsExploreMineRefSet : _TALSOBasicRefSet<MarsExploreMineRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/mars_refdata.unity3d"; } }
		public static string objName { get { return "mars_explore_mine"; } }
	}
}

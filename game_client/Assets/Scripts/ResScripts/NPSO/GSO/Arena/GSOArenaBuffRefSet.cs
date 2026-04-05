using ALPackage;
using System;
using Common.ArenaEnum;

namespace GOE
{
	/// <summary>
	/// 竞技场临时增益表
	/// </summary>
	[Serializable]
	public class ArenaBuffRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
        public EArenaBuffType type;//类型
        public long value;//效果值（万分比）
        public NPCommonCostItem cost;//消耗
        public string name;//名称
        public NPGTextureIndex icon;//图标
        public string desc;//描述
        public string desc_args;//描述参数
    }

	public class GSOArenaBuffRefSet : _TALSOBasicRefSet<ArenaBuffRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/arena_refdata.unity3d"; } }
		public static string objName { get { return "arena_buff"; } }
	}
}
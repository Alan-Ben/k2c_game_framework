using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 抽卡道具展示信息表
	/// </summary>
	[Serializable]
	public class GachaItemShowInfoRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;

		public long show_group;//展示分组(id小的展示在前)
		public long item_id;//展示物品id
		public string show_prob;//展示概率

		[NonSerialized]
		private GachaItemRefObj _m_gachaItemRefObj;
		public GachaItemRefObj gachaItemRefObj
		{
			get
			{
#if NP_GAME
				if (_m_gachaItemRefObj == null)
					_m_gachaItemRefObj = GRefdataCoreMgr.instance.gachaItemRefCore.getRef(item_id);

				if (_m_gachaItemRefObj == null)
				{
					Debug.LogError($"gacha_item表中找不到id为{item_id}的数据");
				}
#endif
				return _m_gachaItemRefObj;
			}
		}
	}

	public class GSOGachaItemShowInfoRefSet : _TALSOBasicRefSet<GachaItemShowInfoRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/gacha_refdata.unity3d"; } }
		public static string objName { get { return "gacha_item_show_info"; } }
	}
}
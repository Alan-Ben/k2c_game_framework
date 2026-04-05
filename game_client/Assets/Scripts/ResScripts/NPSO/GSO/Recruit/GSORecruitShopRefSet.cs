using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 招募商店表
	/// </summary>
	[Serializable]
	public class RecruitShopRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		
		public string name;//商店名
		public NPCommonItem recruit_cost_show_item;//招募消耗展示物品
		
		// 招募表
		[NonSerialized][ALAutoExportVariableAttr(true, true, true)]
		private List<RecruitRefObj> _m_lRecruitRefObjList = null;
		public List<RecruitRefObj> recruitRefObjList
		{
			get
			{
#if NP_GAME
				if (_m_lRecruitRefObjList == null)
				{
					_m_lRecruitRefObjList = new List<RecruitRefObj>();

					foreach (var item in GRefdataCoreMgr.instance.recruitRefCore.refList)
					{
						if(item != null && item.shop_id == id)
							_m_lRecruitRefObjList.Add(item);
					}
				}			
#endif
				return _m_lRecruitRefObjList;
			}
		}
		
		public long red_tip_id;//红点id
	}

	public class GSORecruitShopRefSet : _TALSOBasicRefSet<RecruitShopRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/recruit_refdata.unity3d"; } }
		public static string objName { get { return "recruit_shop"; } }
	}
}
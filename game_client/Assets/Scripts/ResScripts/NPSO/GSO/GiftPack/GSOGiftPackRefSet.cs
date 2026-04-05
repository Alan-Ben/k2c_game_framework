using ALPackage;
using System;
using System.Collections.Generic;
using CommonEnum;
using JetBrains.Annotations;

namespace GOE
{
	/// <summary>
	/// 礼包表
	/// </summary>
	[Serializable]
	public class GiftPackRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		public EGiftPackType type;//礼包类型（EGiftPackType）
		public NPGTextureIndex icon;//图标
        public List<NPCommonCostItem> cost_list;//消耗列表
        public long sort_id;//排序id
        public List<NPCommonCostItem> item_list;//奖励物品列表
        public int buy_limit_count;//限购次数
        public NPTimeRefreshInfo buy_limit_refresh_time;//限购次数刷新时间（ENPTimeRefreshType）
        public string name;//礼包名称
        public List<string> name_args;//礼包名称参数列表
        public string desc;//礼包描述
        public List<string> desc_args_list;//礼包描述参数列表
        public long profit_per;//礼包价值万分比

        /// <summary>
        /// 是否是免费礼包
        /// </summary>
        public bool isFree { get { return cost_list == null || cost_list.Count <= 0; } }
		/// <summary>
		/// 是否是无限制购买次数的礼包
		/// </summary>
		public bool isNotLimit { get { return buy_limit_count <= 0; } }

		/// <summary>
		/// 特殊展示奖励物品
		/// </summary>
		public NPCommonCostItem specialShowRewardItem
		{
			get
			{
				// 潜规则：奖励列表的第一个物品作为特殊展示物品
				return item_list?.SafeGet(0);
			}
		}
		
		/// <summary>
		/// 普通展示奖励物品列表
		/// </summary>
		[NotNull] public List<NPCommonCostItem> normalShowRewardItemList
		{
			get
			{
				// 潜规则：奖励列表的第一个物品作为特殊展示物品，其他的作为普通展示物品
				if (item_list == null || item_list.Count <= 1)
					return new List<NPCommonCostItem>();
				
				return item_list.GetRange(1, item_list.Count - 1);
			}
		}

		/// <summary>
		/// 获取其他展示奖励物品列表
		/// </summary>
		/// <param name="_resList"></param>
		public void getNormalShowRewardItemList(List<NPCommonCostItem> _resList)
		{
			_resList?.Clear();
			if(_resList == null || item_list == null || item_list.Count <= 1)
				return;
			
			for(int i = 1; i < item_list.Count; i++)
			{
				_resList.Add(item_list[i]);
			}
		}
	}

	public class GSOGiftPackRefSet : _TALSOBasicRefSet<GiftPackRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/gift_pack_refdata.unity3d"; } }
		public static string objName { get { return "gift_pack"; } }
	}
}
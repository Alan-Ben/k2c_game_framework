using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 抽卡卡池表
	/// </summary>
	[Serializable]
	public class GachaPoolRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;//卡池id
		
		public long activity_id;//关联活动id(没配置即常驻卡池, 如果配置了则活动开启才能抽)
		public _NPPlayerConditionSerializeInfo condition;//可进行抽卡的条件
		public string condition_desc;//条件描述
		public List<string> condition_desc_args_list;//条件描述参数
		public NPCommonCostItem roll_cost;//单次抽卡消耗
		public NPCommonCostItem ten_roll_cost;//十连抽消耗
		public long can_ten_roll_red_tip_id;//可十抽红点id
		public List<long> show_item_group_list;//展示的道具组
		public int cumulative_reward_need_num;//获得累计奖励抽卡次数
		public NPCommonCostItem cumulative_reward_item;//抽卡累计奖励
		public long roll_record_limit;//召唤记录上限
		public long roll_show_skip_simple_unlock_id;//抽卡表现跳过simple_unlock_id
		public long can_draw_cumulative_reward_red_tip_id;//可领取累计抽卡次数红点
		public long fixed_cd_id;//免费抽卡固定CD配置id
		public long can_free_draw_red_tip_id;//可免费抽卡红点id
		
		private List<List<GachaItemShowInfoRefObj>> _m_lShowItemGroupList;
		public  List<List<GachaItemShowInfoRefObj>> showItemGroupList
		{
			get
			{
#if NP_GAME
				if (_m_lShowItemGroupList == null)
				{
					_m_lShowItemGroupList = new List<List<GachaItemShowInfoRefObj>>();
					List<GachaItemShowInfoRefObj> showItemInfoList = null;
					foreach (var show_item_group in show_item_group_list)
					{
						showItemInfoList = new List<GachaItemShowInfoRefObj>();
						_m_lShowItemGroupList.Add(showItemInfoList);
						foreach (var item in GRefdataCoreMgr.instance.gachaItemShowInfoRefCore.refList)
						{
							if (item != null && item.show_group == show_item_group)
								showItemInfoList.Add(item);
						}
					}
				}
#endif
				return _m_lShowItemGroupList;
			}
		}
	}

	public class GSOGachaPoolRefSet : _TALSOBasicRefSet<GachaPoolRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/gacha_refdata.unity3d"; } }
		public static string objName { get { return "gacha_pool"; } }
	}
}